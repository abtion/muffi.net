using DomainModel.Shared.Exceptions;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DomainModel.Commands;

public class ExampleUpdateCommandHandler : IRequestHandler<ExampleUpdateCommand, ExampleUpdateResponse> {
    private readonly DomainModelTransaction domainModelTransaction;

    public ExampleUpdateCommandHandler(DomainModelTransaction domainModelTransaction) {
        this.domainModelTransaction = domainModelTransaction ?? throw new ArgumentNullException(nameof(domainModelTransaction));
    }

    public async Task<ExampleUpdateResponse> Handle(ExampleUpdateCommand request, CancellationToken cancellationToken) {
        if (request is null) {
            throw new ArgumentNullException(nameof(request));
        }

        var entity = domainModelTransaction.ExampleEntities().WithId(request.Id).SingleOrDefault();

        if (entity == null)
            throw new EntityNotFoundException(request.Id);

        entity.Name = request.Name;
        entity.Description = request.Description;
        entity.Email = request.Email;
        entity.Phone = request.Phone;

        await domainModelTransaction.SaveChangesAsync();

        return new ExampleUpdateResponse() { ExampleEntity = entity };
    }
}