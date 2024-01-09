using Domain.Example.Entities;
using Domain.Shared;
using System.Threading;
using System.Threading.Tasks;

namespace Test.Shared.TestData;

public class ExampleTestData(IUnitOfWork UnitOfWork, IRepository<ExampleEntity> Repository)
{
    public async Task AddExampleEntitiesToDatabase(int numberOfEntitiesToAdd, CancellationToken cancellationToken)
    {
        for (int i = 0; i < numberOfEntitiesToAdd; i++)
        {
            Repository.AddEntity(new ExampleEntity()
            {
                Id = i + 1,
                Name = $"Name {i}",
                Description = $"Description {i}"
            });
        }

        await UnitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task ResetExampleEntities(CancellationToken cancellationToken)
    {
        var entities = await Repository.GetAll(cancellationToken);

        foreach (var entity in entities)
            Repository.RemoveEntity(entity);

        await UnitOfWork.SaveChangesAsync(cancellationToken);
    }
}