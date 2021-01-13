using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using ModelLayer;
using ModelLayer.CartDto;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caching
{
    public class CachedAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int _timeToLiveSeconds;
        private string key;
        public CachedAttribute(int timeToLiveSeconds)
        {
            _timeToLiveSeconds = timeToLiveSeconds;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cacheSettings = context.HttpContext.RequestServices.GetRequiredService<CacheConfiguration>();
            key = (string)context.HttpContext.Items["userId"];
            if (!cacheSettings.IsEnabled)
            {
                await next();
                return;
            }
            var cachedService = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();
            string cachedResponse = await cachedService.GetCachedResponseAsync(key);

            if (!string.IsNullOrEmpty(cachedResponse) && context.HttpContext.Request.Method == "GET")
            {
                var contentResult = new ContentResult
                {
                    Content =  
                     JsonConvert.SerializeObject(new {
                        StatusCode= 200,
                        Message = "Success",
                        Data=cachedResponse,
                    }),
                    ContentType = "application/json",
                    StatusCode = 200
                };
                context.Result = contentResult;
                return;
            }


            var executedContextResult = await next();

            if (executedContextResult.Result is OkObjectResult okObjectResult)
            {
                string responseData = JsonConvert.SerializeObject(okObjectResult.Value);
                string[] pathArray = context.HttpContext.Request.Path.Value.Split("/");
                int? pathParam = null;
                string cachedData = await cachedService.GetCachedResponseAsync(key);

                List<CartResponseDto> currentUserData = null;
                if (cachedData != null)
                {
                    currentUserData = JsonConvert.DeserializeObject<List<CartResponseDto>>(cachedData);
                }
                if (pathArray[pathArray.Length - 1].All(char.IsDigit))
                {
                    pathParam = Convert.ToInt32(pathArray[pathArray.Length - 1]);
                }

                switch (context.HttpContext.Request.Method)
                {
                    case "GET":
                        var responseDataList = JsonConvert.DeserializeObject<Response<List<CartResponseDto>>>(responseData).Data;
                        await cachedService.CacheResponseAsync(key, responseDataList, TimeSpan.FromSeconds(_timeToLiveSeconds));
                        break;
                    case "POST":
                        if (cachedData == null)
                        {
                            break;
                        }
                        int? bookId = pathParam;
                        if (bookId == null)
                        {
                            var deserializedResponse = JsonConvert.DeserializeObject<Response<CartResponseDto>>(responseData).Data;
                            currentUserData.Add(deserializedResponse);
                        }
                        else
                        {
                            var deserializedResponse = JsonConvert.DeserializeObject<Response<CartResponseDto>>(responseData).Data;
                            currentUserData.Where(cart => cart.Book.Id == bookId).Select(cart => cart = deserializedResponse);
                        }
                        await cachedService.CacheResponseAsync(key, currentUserData, TimeSpan.FromSeconds(600));
                        break;
                    case "DELETE":
                        bookId = pathParam;
                        if (cachedData == null)
                        {
                            break;
                        }
                        if (currentUserData.Remove(currentUserData.FirstOrDefault(cart => cart.Book.Id == bookId)))
                        {
                            await cachedService.CacheResponseAsync(key, currentUserData, TimeSpan.FromSeconds(600));
                        }
                        break;
                    default:
                        throw new Exception("Something went wrong");
                }
            }


        }
    }
}
