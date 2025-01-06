using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.IRepositories;
using Talabat.Repository.Data;

namespace Talabat.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;
        //mn  
        private readonly Dictionary<string, object> Repositories;
        //inject this in the Services
        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            Repositories = new Dictionary<string, object>();
        }
        public IGenericRepository<T> Repository<T>() where T : BaseEntity
        {
            var type = typeof(T).Name;//Category Order ... 
            if (!Repositories.ContainsKey(type))
            {
                var Repository = new GenericRepository<T>(_dbContext);
                Repositories.Add(type, Repository);
            }
            return (IGenericRepository<T>)Repositories[type];
        }

        public async ValueTask DisposeAsync()
        {
            await _dbContext.DisposeAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}
