using Domain.Example.Entities;
using Domain.Example.Specifications;
using Presentation.Example.Dtos;
using Presentation.Example.Mappers;
using Presentation.Shared;

using static Domain.Example.Queries.ExampleLoadSingleQueryHandler;

namespace Domain.Example.Queries;

public class ExampleLoadSingleQueryHandler(IRepository<ExampleEntity> Repository, ExampleMapper Mapper) : IQueryHandler<ExampleLoadSingleQuery, ExampleLoadSingleResponse>
{
    public async Task<ExampleLoadSingleResponse> Handle(ExampleLoadSingleQuery request, CancellationToken cancellationToken)
    {
        var query = await Repository.GetAll(new WithId(request.Id), cancellationToken);                    

        if (query.Count == 0)
            throw new EntityNotFoundException(request.Id);

        return new ExampleLoadSingleResponse(Mapper.MapEntityToDto(query.Single()));
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