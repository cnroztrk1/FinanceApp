using System;
using System.Linq;

namespace Data.Repos
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();

        Task<IEnumerable<T>> GetAllAsyncNoTenant();
        Task AddAsync(T entity);
        void Update(T entity);
        void Remove(T entity);
    }

}
