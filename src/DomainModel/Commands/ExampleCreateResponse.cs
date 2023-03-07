using DomainModel.Models;

namespace DomainModel.Commands;

public record ExampleCreateResponse {
    public ExampleEntity? ExampleEntity { get; set; }
}