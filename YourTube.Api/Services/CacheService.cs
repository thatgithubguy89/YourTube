using Microsoft.Extensions.Caching.Memory;

namespace YourTube.Api.Services
{
    public class CacheService<T> : ICacheService<T> where T : class
    {
        private readonly IMemoryCache _cache;

        public CacheService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public void SetItems(string key, List<T> values)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

            var options = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(5))
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(10))
                .SetPriority(CacheItemPriority.Low);

            _cache.Set(key, values, options);
        }

        public void SetItem(string key, T value)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

            var options = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(5))
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(10))
                .SetPriority(CacheItemPriority.Low);

            _cache.Set(key, value, options);
        }

        public void DeleteItems(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

            _cache.Remove(key);
        }

        public T GetItem(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

            return _cache.Get<T>(key);
        }

        public List<T> GetItems(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

            return _cache.Get<List<T>>(key);
        }
    }
}
