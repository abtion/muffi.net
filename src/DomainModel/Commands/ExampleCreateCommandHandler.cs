using MediatR;
using DomainModel.HubContracts;
using DomainModel.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DomainModel.Commands;

public class ExampleCreateCommandHandler : IRequestHandler<ExampleCreateCommand, ExampleCreateResponse>
{
    private readonly DomainModelTransaction domainModelTransaction;
    private readonly IExampleHubContract exampleHub;

    public ExampleCreateCommandHandler(DomainModelTransaction domainModelTransaction, IExampleHubContract exampleHub)
    {
        this.domainModelTransaction = domainModelTransaction ?? throw new ArgumentNullException(nameof(domainModelTransaction));
        this.exampleHub = exampleHub ?? throw new ArgumentNullException(nameof(exampleHub));
    }

    public async Task<ExampleCreateResponse> Handle(ExampleCreateCommand request, CancellationToken cancellationToken)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var entity = new ExampleEntity();

        // setting the Id since there is no database to do it
        if (domainModelTransaction.ExampleEntities().Any())
            entity.Id = domainModelTransaction.ExampleEntities().Max(p => p.Id) + 1;
        else
            entity.Id = 1;

        entity.Name = request.Name;
        entity.Description = request.Description;
        entity.Email = request.Email;
        entity.Phone = request.Phone;

        domainModelTransaction.AddExampleEntity(entity);

        await domainModelTransaction.SaveChangesAsync();

        await exampleHub.SomeEntityCreated(new SomeEntityCreatedMessage(new ExampleEntityRecord(
            entity.Id,
            entity.Name,
            entity.Description,
            entity.Email,
            entity.Phone
        )));

        return new ExampleCreateResponse() { ExampleEntity = entity };
    }
}