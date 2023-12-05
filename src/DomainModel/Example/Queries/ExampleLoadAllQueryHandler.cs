using DomainModel.Example.ViewModels;
using DomainModel.Shared;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DomainModel.Example.Queries;

public class ExampleLoadAllQueryHandler : IQueryHandler<ExampleLoadAllQuery, ExampleLoadAllResponse>
{
    private readonly DomainModelTransaction domainModelTransaction;

    public ExampleLoadAllQueryHandler(DomainModelTransaction domainModelTransaction)
    {
        this.domainModelTransaction =
            domainModelTransaction
            ?? throw new ArgumentNullException(nameof(domainModelTransaction));
    }

    public async Task<ExampleLoadAllResponse> Handle(
        ExampleLoadAllQuery request,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(request);

        var query =
            from exampleEntity in domainModelTransaction.ExampleEntities().All()
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
