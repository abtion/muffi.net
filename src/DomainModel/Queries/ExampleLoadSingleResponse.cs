using DomainModel.Models;

namespace DomainModel.Queries;

public record ExampleLoadSingleResponse {
    public ExampleEntityRecord? ExampleEntity { get; set; }
}