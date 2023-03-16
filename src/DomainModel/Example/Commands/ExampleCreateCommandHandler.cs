using DomainModel.Data.Models;
using DomainModel.Example.Notifications;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DomainModel.Example.Commands;

public class ExampleCreateCommandHandler : IRequestHandler<ExampleCreateCommand, ExampleCreateResponse>
{
    private readonly DomainModelTransaction domainModelTransaction;
    private readonly IMediator mediator;

    public ExampleCreateCommandHandler(
        DomainModelTransaction domainModelTransaction,
        IMediator mediator)
    {
        this.domainModelTransaction = domainModelTransaction;
        this.mediator = mediator;
    }

    public async Task<ExampleCreateResponse> Handle(
        ExampleCreateCommand request,
        CancellationToken cancellationToken)
    {
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

        await mediator.Publish(new ExampleCreatedNotification(entity), cancellationToken);

        return new ExampleCreateResponse() { ExampleEntity = entity };
    }
}