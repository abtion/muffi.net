using Domain.Example.Entities;
using Presentation.Example.Dtos;

using static Domain.Example.Queries.ExampleLoadAllQueryHandler;

namespace Domain.Example.Queries;

public class ExampleLoadAllQueryHandler(IRepository<ExampleEntity> Repository) : IQueryHandler<ExampleLoadAllQuery, ExampleLoadAllResponse>
{
    public async Task<ExampleLoadAllResponse> Handle(
        ExampleLoadAllQuery request,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(request);

        var query = from exampleEntity in await Repository.GetAll(cancellationToken)
                    select new ExampleDto(
                        exampleEntity.Id,
                        exampleEntity.Name,
                        exampleEntity.Description,
                        exampleEntity.Email,
                        exampleEntity.Phone);

        return await Task.FromResult(new ExampleLoadAllResponse(query.ToList()));
    }

    public record ExampleLoadAllQuery : IQuery<ExampleLoadAllResponse>
    {
    }

    public record ExampleLoadAllResponse
    {
        public ExampleLoadAllResponse(IList<ExampleDto> exampleEntities)
        {
            ExampleEntities = exampleEntities;
        }

        public IList<ExampleDto> ExampleEntities { get; }

    }
}
