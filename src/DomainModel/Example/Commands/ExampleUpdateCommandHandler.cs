using DomainModel.Example.Notifications;
using DomainModel.Shared;
using DomainModel.Shared.Exceptions;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DomainModel.Example.Commands;

public class ExampleUpdateCommandHandler
    : ICommandHandler<ExampleUpdateCommand, ExampleUpdateResponse>
{
    private readonly DomainModelTransaction domainModelTransaction;
    private readonly IMediator mediator;

    public ExampleUpdateCommandHandler(
        DomainModelTransaction domainModelTransaction,
        IMediator mediator
    )
    {
        this.domainModelTransaction = domainModelTransaction;
        this.mediator = mediator;
    }

    public async Task<ExampleUpdateResponse> Handle(
        ExampleUpdateCommand request,
        CancellationToken cancellationToken
    )
    {
        var entity = domainModelTransaction.ExampleEntities().WithId(request.Id).SingleOrDefault();

        if (entity == null)
            throw new EntityNotFoundException(request.Id);

        entity.Name = request.Name;
        entity.Description = request.Description;
        entity.Email = request.Email;
        entity.Phone = request.Phone;

        await domainModelTransaction.SaveChangesAsync();

        await mediator.Publish(new ExampleUpdatedNotification(entity), cancellationToken);

        return new ExampleUpdateResponse() { ExampleEntity = entity };
    }
}
