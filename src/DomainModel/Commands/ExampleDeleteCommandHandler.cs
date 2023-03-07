using MediatR;
using DomainModel.HubContracts;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DomainModel.Commands;

public class ExampleDeleteCommandHandler : IRequestHandler<ExampleDeleteCommand, ExampleDeleteResponse>
{
    private readonly DomainModelTransaction domainModelTransaction;
    private readonly IExampleHubContract exampleHub;

    public ExampleDeleteCommandHandler(DomainModelTransaction domainModelTransaction, IExampleHubContract exampleHub)
    {
        this.domainModelTransaction = domainModelTransaction ?? throw new ArgumentNullException(nameof(domainModelTransaction));
        this.exampleHub = exampleHub ?? throw new ArgumentNullException(nameof(exampleHub));
    }

    public async Task<ExampleDeleteResponse> Handle(ExampleDeleteCommand request, CancellationToken cancellationToken)
    {
        if (request is null)
            throw new ArgumentNullException(nameof(request));

        domainModelTransaction.RemoveExampleEntity(request.Id);
        await domainModelTransaction.SaveChangesAsync();

        await exampleHub.SomeEntityDeleted(new SomeEntityDeletedMessage(request.Id));

        return new ExampleDeleteResponse();
    }
}