using EPAM.EF.Entities.Abstraction;

namespace EPAM.Cache.Interfaces
{
    public interface ICache
    {
        void Attach<T>(T? item) where T : Entity;

        Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default);

        Task Set<T>(string key, T item, CancellationToken cancellationToken = default);
    }
}
