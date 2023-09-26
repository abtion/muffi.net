using DomainModel.Example.Services;
using DomainModel.Example.ViewModels;
using DomainModel.Shared;
using DomainModel.Shared.Exceptions;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DomainModel.Example.Queries;

public class ExampleLoadSingleQueryHandler
    : IQueryHandler<ExampleLoadSingleQuery, ExampleLoadSingleResponse>
{
    private readonly DomainModelTransaction domainModelTransaction;
    private readonly IExampleReverseStringService exampleService;

    public ExampleLoadSingleQueryHandler(
        DomainModelTransaction domainModelTransaction,
        IExampleReverseStringService exampleService
    )
    {
        this.domainModelTransaction =
            domainModelTransaction
            ?? throw new ArgumentNullException(nameof(domainModelTransaction));
        this.exampleService =
            exampleService ?? throw new ArgumentNullException(nameof(exampleService));
    }

    public async Task<ExampleLoadSingleResponse> Handle(
        ExampleLoadSingleQuery request,
        CancellationToken cancellationToken
    )
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var query =
            from exampleEntity in domainModelTransaction.ExampleEntities().WithId(request.Id)
            select new ExampleEntityRecord(
                exampleEntity.Id,
                exampleEntity.Name,
                exampleService.ReverseString(exampleEntity.Description),
                exampleEntity.Email,
                exampleEntity.Phone
            );

        if (!query.Any())
            throw new EntityNotFoundException(request.Id);

        return await Task.FromResult(
            new ExampleLoadSingleResponse() { ExampleEntity = query.Single() }
        );
    }
}
