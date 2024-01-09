using Ardalis.Specification;

namespace Domain.Shared;

public interface IRepository<T> where T : class
{
    public void AddEntity(T entity);

    public void RemoveEntity(T entity);

    public Task<List<T>> GetAll(ISpecification<T> specification, CancellationToken cancellationToken);

    public Task<List<T>> GetAll(CancellationToken cancellationToken);
}
