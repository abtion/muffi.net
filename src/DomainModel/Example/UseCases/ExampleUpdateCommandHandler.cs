using Domain.Example.Entities;
using Domain.Example.Notifications;
using Domain.Example.Specifications;
using static Domain.Example.Commands.ExampleUpdateCommandHandler;

namespace Domain.Example.Commands;

public class ExampleUpdateCommandHandler(IUnitOfWork UnitOfWork, IRepository<ExampleEntity> Repository, IMediator Mediator) : ICommandHandler<ExampleUpdateCommand, ExampleUpdateResponse>
{
    public async Task<ExampleUpdateResponse> Handle(ExampleUpdateCommand request, CancellationToken cancellationToken)
    {
        var query = await Repository.GetAll(new WithId(request.Id), cancellationToken);
        var entity = query.SingleOrDefault() ?? throw new EntityNotFoundException(request.Id);

        entity.Name = request.Name;
        entity.Description = request.Description;
        entity.Email = request.Email;
        entity.Phone = request.Phone;

        await UnitOfWork.SaveChangesAsync(cancellationToken);

        await Mediator.Publish(new ExampleUpdatedNotification(entity), cancellationToken);

        return new ExampleUpdateResponse() { ExampleEntity = entity };
    }

    public record ExampleUpdateCommand : ICommand<ExampleUpdateResponse>
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
    }

    public record ExampleUpdateResponse
    {
        public ExampleEntity? ExampleEntity { get; set; }
    }
}
