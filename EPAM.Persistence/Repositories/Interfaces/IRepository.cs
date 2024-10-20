﻿namespace EPAM.Persistence.Repositories.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetAsync(Guid id);

        Task<IEnumerable<T>> GetAllAsync();

        Task CreateAsync(T entity);

        Task UpdateAsync(T entity);

        Task DeleteAsync(Guid id);
    }
}
