using Domain.Example.Entities;
using Domain.Example.Notifications;
using Domain.Shared;
using static Domain.Example.Commands.ExampleCreateCommandHandler;

namespace Domain.Example.Commands;

public class ExampleCreateCommandHandler(IUnitOfWork UnitOfWork, IRepository<ExampleEntity> Repository, IMediator Mediator) : ICommandHandler<ExampleCreateCommand, ExampleCreateResponse>
{
    public async Task<ExampleCreateResponse> Handle(ExampleCreateCommand request, CancellationToken cancellationToken)
    {
        var entity = new ExampleEntity
        {
            Name = request.Name,
            Description = request.Description,
            Email = request.Email,
            Phone = request.Phone
        };

        Repository.AddEntity(entity);

        await UnitOfWork.SaveChangesAsync(cancellationToken);

        await Mediator.Publish(new ExampleCreatedNotification(entity), cancellationToken);

        return new ExampleCreateResponse() { ExampleEntity = entity };
    }

    public record ExampleCreateCommand : ICommand<ExampleCreateResponse>
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
    }

    public record ExampleCreateResponse
    {
        public ExampleEntity? ExampleEntity { get; set; }
    }
}
