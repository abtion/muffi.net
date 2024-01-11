using Domain.Example.Entities;
using Domain.Example.Notifications;
using Domain.Example.Specifications;

namespace Domain.Example.UseCases;

public class DeleteExampleUseCase(IUnitOfWork UnitOfWork, IRepository<ExampleEntity> Repository, IMediator Mediator)
{
    public async Task Handle(int exampleId, CancellationToken cancellationToken)
    {
        var query = await Repository.GetAll(new WithId(exampleId), cancellationToken);

        var entityToDelete = query.SingleOrDefault() ?? throw new EntityNotFoundException(exampleId);

        Repository.RemoveEntity(entityToDelete);
        await UnitOfWork.SaveChangesAsync(cancellationToken);

        await Mediator.Publish(new ExampleDeletedNotification(exampleId), cancellationToken);
    }
}
