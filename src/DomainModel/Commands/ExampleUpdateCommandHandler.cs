using MediatR;
using DomainModel.Exceptions;
using DomainModel.HubContracts;
using DomainModel.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DomainModel;

namespace DomainModel.Commands;

public class ExampleUpdateCommandHandler : IRequestHandler<ExampleUpdateCommand, ExampleUpdateResponse>
{
    private readonly DomainModelTransaction domainModelTransaction;
    private readonly IExampleHubContract exampleHub;

    public ExampleUpdateCommandHandler(DomainModelTransaction domainModelTransaction, IExampleHubContract exampleHub)
    {
        this.domainModelTransaction = domainModelTransaction ?? throw new ArgumentNullException(nameof(domainModelTransaction));
        this.exampleHub = exampleHub ?? throw new ArgumentNullException(nameof(exampleHub));
    }

    public async Task<ExampleUpdateResponse> Handle(ExampleUpdateCommand request, CancellationToken cancellationToken)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var entity = domainModelTransaction.ExampleEntities().WithId(request.Id).SingleOrDefault();

        if (entity == null)
            throw new ExampleEntityNotFoundException(request.Id);

        entity.Name = request.Name;
        entity.Description = request.Description;
        entity.Email = request.Email;
        entity.Phone = request.Phone;

        await domainModelTransaction.SaveChangesAsync();

        await exampleHub.SomeEntityUpdated(new SomeEntityUpdatedMessage(new ExampleEntityRecord(
            entity.Id,
            entity.Name,
            entity.Description,
            entity.Email,
            entity.Phone
        )));

        return new ExampleUpdateResponse() { ExampleEntity = entity };
    }
}