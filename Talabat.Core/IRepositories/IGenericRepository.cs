using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.Core.IRepositories
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<T?> GetAsync(int Id);
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecifications<T> spec);
        Task<T?> GetWithSpecAsync(ISpecifications<T> spec);
        Task<int> GetCountAsync(ISpecifications<T> spec);
        Task AddAsync(T entity);
        void Update(T entity);
        void DeleteAsync(T entity);

    }
}
