using Domain.Example.Entities;
using Presentation.Example.Dtos;
using Presentation.Example.Mappers;
using static Presentation.Example.Queries.ExampleLoadAllQueryHandler;

namespace Presentation.Example.Queries;

public class ExampleLoadAllQueryHandler(IRepository<ExampleEntity> Repository, ExampleMapper Mapper) : IQueryHandler<ExampleLoadAllQuery, ExampleLoadAllResponse>
{
    public async Task<ExampleLoadAllResponse> Handle(ExampleLoadAllQuery request, CancellationToken cancellationToken)
    {
        var entities = await Repository.GetAll(cancellationToken);
        var dtos = new List<ExampleDto>();

        foreach (var entity in entities) 
        {
            dtos.Add(Mapper.MapEntityToDto(entity));
        }

        return new ExampleLoadAllResponse(dtos);
    }

    public record ExampleLoadAllQuery : IQuery<ExampleLoadAllResponse>
    {
    }

    public record ExampleLoadAllResponse(IList<ExampleDto> ExampleEntities)
    {
        public IList<ExampleDto> ExampleEntities { get; } = ExampleEntities;
    }
}