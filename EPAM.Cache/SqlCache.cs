using EPAM.Cache.Abstraction;
using EPAM.Cache.Interfaces;
using EPAM.EF.Entities.Abstraction;
using EPAM.EF.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace EPAM.Cache
{
    public sealed class SqlCache : BaseCache, ICache
    {
        private readonly IDistributedCache _distributedCache;

        public SqlCache(IDistributedCache distributedCache, IConfiguration configuration, ISystemContext context) : base(configuration, context)
        {
            _distributedCache = distributedCache;
        }

        public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
        {
            var resultString = await _distributedCache.GetStringAsync(key, cancellationToken).ConfigureAwait(false);

            if (string.IsNullOrEmpty(resultString)) return default(T);

            var result = JsonConvert.DeserializeObject<T>(resultString);

            if (result != null && typeof(T).IsSubclassOf(typeof(Entity)))
            {
                Attach(result as Entity);
            }

            return result;
        }

        public async Task SetAsync<T>(string key, T item, CancellationToken cancellationToken = default)
        {
            var json = JsonConvert.SerializeObject(item);

            var options = new DistributedCacheEntryOptions();

            if (CacheOptions != null)
            {
                options = options
                    .SetSlidingExpiration(TimeSpan.FromSeconds(CacheOptions.SlidingExpirationInSeconds))
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(CacheOptions.AbsoluteExpiration));
            }

            await _distributedCache.SetStringAsync(key, json, options, cancellationToken).ConfigureAwait(false);
        }
    }
}
