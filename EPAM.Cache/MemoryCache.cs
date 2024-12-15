using EPAM.Cache.Abstraction;
using EPAM.Cache.Interfaces;
using EPAM.EF.Entities.Abstraction;
using EPAM.EF.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace EPAM.Cache
{
    public sealed class MemoryCache : BaseCache, ICache
    {
        private readonly IMemoryCache _memoryCache;

        public MemoryCache(IMemoryCache memoryCache, IConfiguration configuration, ISystemContext context) : base(configuration, context)
        {
            _memoryCache = memoryCache;
        }

        public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
        {
            T? result;
            if (CacheOptions != null && CacheOptions.IsSynchronized)
            {
                lock (LockObject)
                {
                    result = _memoryCache.Get<T>(key);
                }
            }
            else
            {
                result = _memoryCache.Get<T>(key);
            }

            if (typeof(T).IsSubclassOf(typeof(Entity)))
            {
                Attach(result as Entity);
            }

            return await Task.FromResult(result);
        }

        public async Task SetAsync<T>(string key, T item, CancellationToken cancellationToken = default)
        {
            var options = new MemoryCacheEntryOptions();

            if (CacheOptions != null)
            {
                options = options
                    .SetSlidingExpiration(TimeSpan.FromSeconds(CacheOptions.SlidingExpirationInSeconds))
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(CacheOptions.AbsoluteExpiration));
            }

            if (CacheOptions != null && CacheOptions.IsSynchronized)
            {
                lock (LockObject)
                {
                    _memoryCache.Set(key, item, options);
                }
            }
            else
            {
                _memoryCache.Set(key, item, options);
            }

            await Task.CompletedTask;
        }
    }
}
