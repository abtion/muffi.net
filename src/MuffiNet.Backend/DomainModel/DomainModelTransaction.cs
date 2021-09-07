using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using MuffiNet.Backend.Data;
using MuffiNet.Backend.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MuffiNet.Backend.DomainModel
{
    /// <summary>
    /// This class is used to wrap the DBContext from EntityFramework - always use this class when accessing the database
    /// </summary>
    public class DomainModelTransaction
    {
        public DomainModelTransaction(ApplicationDbContext applicationDbContext)
        {
            DbContext = applicationDbContext;
        }

        private ApplicationDbContext DbContext { get; set; }

        // Remove this section -->
        private static List<ExampleEntity>? exampleEntities;
        public IQueryable<ExampleEntity> ExampleEntities()
        {
            if (exampleEntities is null)
                exampleEntities = new List<ExampleEntity>();

            return exampleEntities.AsQueryable();
        }

        public void ResetExampleEntities()
        {
            exampleEntities = null;
        }

        public void AddExampleEntity(ExampleEntity entity)
        {
            if (exampleEntities is null)
                exampleEntities = new List<ExampleEntity>();

            exampleEntities.Add(entity);
        }

        public void RemoveExampleEntity(int entityId)
        {
            exampleEntities?.Remove(exampleEntities.Where(e => e.Id == entityId).First());
        }
        // <-- Remove this section

        /// <summary>
        /// This generic method should be used by commands when accessing entities.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IQueryable<T> Entities<T>() where T : class
        {
            return DbContext.Set<T>();
        }

        /// <summary>
        /// This generic method should be used by queries when accessing entities (diables Entity Tracking in EntityFramwork 
        /// for improved reading performance)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
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