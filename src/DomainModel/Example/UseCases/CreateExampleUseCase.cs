using Domain.Example.Entities;
using Domain.Example.Notifications;

namespace Domain.Example.UseCases;

public class CreateExampleUseCase(IUnitOfWork UnitOfWork, IRepository<ExampleEntity> Repository, IMediator Mediator)
{
    public async Task<ExampleEntity> Handle(ExampleEntity entity, CancellationToken cancellationToken) 
    {
        Repository.AddEntity(entity);

        await UnitOfWork.SaveChangesAsync(cancellationToken);

        await Mediator.Publish(new ExampleCreatedNotification(entity), cancellationToken);

        return entity;
    }
}
