using EPAM.Cache.Models;
using EPAM.EF.Entities.Abstraction;
using EPAM.EF.Interfaces;
using Microsoft.Extensions.Configuration;

namespace EPAM.Cache.Abstraction
{
    public abstract class BaseCache
    {
        private readonly ISystemContext _context;
        protected readonly CacheOptions? CacheOptions;

        protected BaseCache(IConfiguration configuration, ISystemContext context)
        {
            _context = context;
            CacheOptions = configuration.GetSection("CacheOptions").Get<CacheOptions>();
        }

        public void Attach<T>(T? item) where T : Entity
        {
            if (item == null) return;

            _context.GetDbSet<T>().Attach(item);
        }
    }
}
