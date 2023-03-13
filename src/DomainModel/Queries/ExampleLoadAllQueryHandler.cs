using DomainModel.Models;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DomainModel.Queries;

public class ExampleLoadAllQueryHandler : IRequestHandler<ExampleLoadAllQuery, ExampleLoadAllResponse> {
    private readonly DomainModelTransaction domainModelTransaction;

    public ExampleLoadAllQueryHandler(DomainModelTransaction domainModelTransaction) {
        this.domainModelTransaction = domainModelTransaction ?? throw new ArgumentNullException(nameof(domainModelTransaction));
    }

    public async Task<ExampleLoadAllResponse> Handle(ExampleLoadAllQuery request, CancellationToken cancellationToken) {
        if (request is null) {
            throw new ArgumentNullException(nameof(request));
        }
        var query = from exampleEntity in domainModelTransaction.ExampleEntities().All()
                    select new ExampleEntityRecord(
                        exampleEntity.Id,
                        exampleEntity.Name,
                        exampleEntity.Description,
                        exampleEntity.Email,
                        exampleEntity.Phone
                    );

        return await Task.FromResult(new ExampleLoadAllResponse(query.ToList()));
    }
}