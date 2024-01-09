using Domain.Example.Entities;
using Domain.Example.Specifications;
using Presentation.Example.Dtos;

using static Domain.Example.Queries.ExampleLoadSingleQueryHandler;

namespace Domain.Example.Queries;

public class ExampleLoadSingleQueryHandler(IRepository<ExampleEntity> Repository) : IQueryHandler<ExampleLoadSingleQuery, ExampleLoadSingleResponse>
{
    public async Task<ExampleLoadSingleResponse> Handle(ExampleLoadSingleQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var query = from exampleEntity in await Repository.GetAll(new WithId(request.Id), cancellationToken)
                    select new ExampleDto(
                        exampleEntity.Id,
                        exampleEntity.Name,
                        exampleEntity.Description,
                        exampleEntity.Email,
                        exampleEntity.Phone);

        if (!query.Any())
            throw new EntityNotFoundException(request.Id);

        return new ExampleLoadSingleResponse(query.Single());
    }
    public record ExampleLoadSingleQuery : IQuery<ExampleLoadSingleResponse>
    {
        public int Id { get; set; }
    }

    public record ExampleLoadSingleResponse(ExampleDto ExampleEntity)
    {
        public ExampleDto ExampleEntity { get; } = ExampleEntity;
    }
}
