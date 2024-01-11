using Domain.Example.Entities;
using Domain.Example.Notifications;

namespace Domain.Example.UseCases;

public class UpdateExampleUseCase(IUnitOfWork UnitOfWork, IMediator Mediator)
{
    public async Task<ExampleEntity> Handle(ExampleEntity entity, CancellationToken cancellationToken)
    {
        await UnitOfWork.SaveChangesAsync(cancellationToken);

        await Mediator.Publish(new ExampleUpdatedNotification(entity), cancellationToken);

        return entity;
    }
}