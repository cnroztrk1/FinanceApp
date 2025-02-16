using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;

namespace FinanceApp.Common
{
    public class MemoryCacheService : ICacheService
    {
        private readonly IMemoryCache _cache;

        public MemoryCacheService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public async Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> fetchFunction, TimeSpan expiration)
        {
            if (!_cache.TryGetValue(key, out T cacheValue))
            {
                cacheValue = await fetchFunction();
                _cache.Set(key, cacheValue, expiration);
            }
            return cacheValue;
        }

        public void Remove(string key)
        {
            _cache.Remove(key);
        }
    }
}
