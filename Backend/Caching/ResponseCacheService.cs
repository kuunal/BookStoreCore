using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Caching
{
    public class ResponseCacheService
    {
        private readonly IDistributedCache _distribitedCache;

        public ResponseCacheService(IDistributedCache distribitedCache)
        {
            _distribitedCache = distribitedCache;
        }

        public async Task CacheResponseAsync(string cacheKey, object response, TimeSpan timeToLive)
        {
            if (response == null)
            {
                return;
            }
            var serializedResponse = JsonConvert.SerializeObject(response);
            await _distribitedCache.SetStringAsync(cacheKey, serializedResponse, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = timeToLive
            });
        }

        public async Task<string> GetCachedResponseAsync(string key)
        {
            var cachedResponse = await _distribitedCache.GetStringAsync(key);
            return string.IsNullOrEmpty(cachedResponse) ? null : cachedResponse;
        }
    }
}
