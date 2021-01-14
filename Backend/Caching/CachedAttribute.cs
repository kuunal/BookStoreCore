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
                     JsonConvert.SerializeObject(new Response<CartDetailedResponseDto>
                     {
                         StatusCode = 200,
                         Message = "Success",
                         Data = JsonConvert.DeserializeObject<CartDetailedResponseDto>(cachedResponse),
                     }),
                    ContentType = "application/json",
                    StatusCode = 200
                };
                context.Result = contentResult;
                return;
            }
            var executedContextResult = await next();
        }
    }
}
