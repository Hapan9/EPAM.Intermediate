using EPAM.Cache.Enums;

namespace EPAM.Cache.Interfaces
{
    public interface ISystemCache
    {
        ICache GetCache(CacheTypes cacheTypes);
    }
}
