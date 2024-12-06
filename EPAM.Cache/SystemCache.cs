using EPAM.Cache.Enums;
using EPAM.Cache.Interfaces;
using EPAM.EF.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace EPAM.Cache
{
    public sealed class SystemCache : ISystemCache
    {
        private readonly ICache _memoryCache;
        private readonly ICache _distributedCache;

        public SystemCache(IMemoryCache memoryCache, IDistributedCache distributedCache, IConfiguration configuration, ISystemContext context)
        {
            _memoryCache = new MemoryCache(memoryCache, configuration, context);
            _distributedCache = new SqlCache(distributedCache, configuration, context);
        }

        /// <summary>
        /// Just to check different cache types
        /// </summary>
        /// <param name="cacheType"></param>
        /// <returns></returns>
        public ICache Cache(CacheTypes cacheType)
        {
            if (cacheType == CacheTypes.MemoryCache)
            {
                return _memoryCache;
            }

            return _distributedCache;
        }
    }
}
