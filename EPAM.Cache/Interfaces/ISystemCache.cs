using EPAM.Cache.Enums;

namespace EPAM.Cache.Interfaces
{
    public interface ISystemCache
    {
        ICache Cache(CacheTypes cacheTypes);
    }
}
