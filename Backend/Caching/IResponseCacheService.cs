using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Caching
{
    public interface IResponseCacheService
    {
        Task<string> GetCachedResponseAsync(string key);
        Task CacheResponseAsync(string cacheKey, object response, TimeSpan timeToLive);
        Task RemoveCacheByKeyAsync(string key);
    }
}
