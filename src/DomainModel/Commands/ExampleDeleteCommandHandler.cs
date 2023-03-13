using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DomainModel.Commands;

public class ExampleDeleteCommandHandler : IRequestHandler<ExampleDeleteCommand, ExampleDeleteResponse> {
    private readonly DomainModelTransaction domainModelTransaction;

    public ExampleDeleteCommandHandler(DomainModelTransaction domainModelTransaction) {
        this.domainModelTransaction = domainModelTransaction ?? throw new ArgumentNullException(nameof(domainModelTransaction));
    }

    public async Task<ExampleDeleteResponse> Handle(ExampleDeleteCommand request, CancellationToken cancellationToken) {
        if (request is null)
            throw new ArgumentNullException(nameof(request));

        domainModelTransaction.RemoveExampleEntity(request.Id);
        await domainModelTransaction.SaveChangesAsync();

        return new ExampleDeleteResponse();
    }
}