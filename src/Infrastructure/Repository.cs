using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Domain.Shared;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class Repository<T>(ApplicationDbContext DbContext) : IRepository<T> where T : class
{
    public void AddEntity(T entity)
    {
        DbContext.Set<T>().Add(entity);
    }

    public async Task<List<T>> GetAll(ISpecification<T> specification, CancellationToken cancellationToken)
    {
        var query = SpecificationEvaluator.Default.GetQuery(query: DbContext.Set<T>().AsQueryable(), specification: specification);

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<List<T>> GetAll(CancellationToken cancellationToken)
    {
        return await DbContext.Set<T>().ToListAsync(cancellationToken);
    }

    public void RemoveEntity(T entity)
    {
        DbContext.Set<T>().Remove(entity);
    }
}
