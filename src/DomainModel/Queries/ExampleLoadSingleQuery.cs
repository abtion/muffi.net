using DomainModel.Shared;

namespace DomainModel.Queries;

public record ExampleLoadSingleQuery : IQuery<ExampleLoadSingleResponse> {
    public int Id { get; set; }
}