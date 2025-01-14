using Microsoft.EntityFrameworkCore;
using Talabat.Core.Entities;
using Talabat.Core.IRepositories;
using Talabat.Core.Specifications;
using Talabat.Repository.Data;

namespace Talabat.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly ApplicationDbContext dbContext;

        public GenericRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<T?> GetAsync(int Id)
        {
            return await this.dbContext.Set<T>().FindAsync(Id);
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await this.dbContext.Set<T>().ToListAsync();
        }


        public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecifications<T> spec)
        {
            return await GetQuery(spec).ToListAsync();
        }

        public async Task<T?> GetWithSpecAsync(ISpecifications<T> spec)
        {
            return await GetQuery(spec).FirstOrDefaultAsync();
        }
        public async Task<int> GetCountAsync(ISpecifications<T> spec)
        {
            return await GetQuery(spec).CountAsync();
        }
        private IQueryable<T> GetQuery(ISpecifications<T> spec)
        {
            return SpecificationEvaluator<T>.BuildQuery(dbContext.Set<T>(), spec);
        }

        public async Task AddAsync(T entity) => await dbContext.Set<T>().AddAsync(entity);

        public void Update(T entity) => dbContext.Set<T>().Update(entity);


        public void Delete(T entity) => dbContext.Set<T>().Remove(entity);

    }
}
