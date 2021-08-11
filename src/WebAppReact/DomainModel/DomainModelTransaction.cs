using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq;
using System.Threading.Tasks;
using WebAppReact.Data;

namespace WebAppReact.DomainModel
{
    public class DomainModelTransaction
    {
        public DomainModelTransaction(ApplicationDbContext applicationDbContext)
        {
            DbContext = applicationDbContext;
        }

        private ApplicationDbContext DbContext { get; set; }

        public IQueryable<T> Entities<T>() where T : class
        {
            return DbContext.Set<T>();
        }

        public IQueryable<T> EntitiesNoTracking<T>() where T : class
        {
            return DbContext.Set<T>().AsNoTracking<T>();
        }

        public async Task AddAsync<T>(T entity) where T : class
        {
            await DbContext.AddAsync<T>(entity);
        }

        public void Remove<T>(T entity) where T : class
        {
            DbContext.Remove<T>(entity);
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await DbContext.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            await DbContext.Database.CommitTransactionAsync();
        }

        public async Task RollbackTransactionAsync()
        {
            await DbContext.Database.RollbackTransactionAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await DbContext.SaveChangesAsync();
        }
    }
}