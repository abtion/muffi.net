using DomainModel.Shared;

namespace DomainModel.Example.Queries;

public record ExampleLoadSingleQuery : IQuery<ExampleLoadSingleResponse>
{
    public int Id { get; set; }
}