using DomainModel.Example.Notifications;
using DomainModel.Shared;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace DomainModel.Example.Commands;

public class ExampleDeleteCommandHandler
    : ICommandHandler<ExampleDeleteCommand, ExampleDeleteResponse>
{
    private readonly DomainModelTransaction domainModelTransaction;
    private readonly IMediator mediator;

    public ExampleDeleteCommandHandler(
        DomainModelTransaction domainModelTransaction,
        IMediator mediator
    )
    {
        this.domainModelTransaction = domainModelTransaction;
        this.mediator = mediator;
    }

    public async Task<ExampleDeleteResponse> Handle(
        ExampleDeleteCommand request,
        CancellationToken cancellationToken
    )
    {
        domainModelTransaction.RemoveExampleEntity(request.Id);

        await domainModelTransaction.SaveChangesAsync();

        await mediator.Publish(new ExampleDeletedNotification(request.Id), cancellationToken);

        return new ExampleDeleteResponse();
    }
}
