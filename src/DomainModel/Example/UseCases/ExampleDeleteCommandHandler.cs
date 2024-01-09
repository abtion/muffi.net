using Domain.Example.Entities;
using Domain.Example.Notifications;
using Domain.Example.Specifications;

using static Domain.Example.Commands.ExampleDeleteCommandHandler;

namespace Domain.Example.Commands;

public class ExampleDeleteCommandHandler : ICommandHandler<ExampleDeleteCommand, ExampleDeleteResponse>
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IRepository<ExampleEntity> repository;
    private readonly IMediator mediator;

    public ExampleDeleteCommandHandler(IUnitOfWork unitOfWork, IRepository<ExampleEntity> repository, IMediator mediator)
    {
        this.unitOfWork = unitOfWork;
        this.repository = repository;
        this.mediator = mediator;
    }

    public async Task<ExampleDeleteResponse> Handle(ExampleDeleteCommand request, CancellationToken cancellationToken)
    {
        var query = await repository.GetAll(new WithId(request.Id), cancellationToken);
        var entityToDelete = query.SingleOrDefault();

        if (entityToDelete is null)
            throw new EntityNotFoundException(request.Id);

        repository.RemoveEntity(entityToDelete);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        await mediator.Publish(new ExampleDeletedNotification(request.Id), cancellationToken);

        return new ExampleDeleteResponse();
    }

    public record ExampleDeleteCommand : ICommand<ExampleDeleteResponse>
    {
        public int Id { get; set; }
    }

    public record ExampleDeleteResponse
    {
    }
}
