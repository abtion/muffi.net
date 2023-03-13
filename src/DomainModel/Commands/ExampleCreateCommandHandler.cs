using DomainModel.Models;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DomainModel.Commands;

public class ExampleCreateCommandHandler : IRequestHandler<ExampleCreateCommand, ExampleCreateResponse> {
    private readonly DomainModelTransaction domainModelTransaction;

    public ExampleCreateCommandHandler(DomainModelTransaction domainModelTransaction) {
        this.domainModelTransaction = domainModelTransaction ?? throw new ArgumentNullException(nameof(domainModelTransaction));
    }

    public async Task<ExampleCreateResponse> Handle(ExampleCreateCommand request, CancellationToken cancellationToken) {
        if (request is null) {
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

        return new ExampleCreateResponse() { ExampleEntity = entity };
    }
}