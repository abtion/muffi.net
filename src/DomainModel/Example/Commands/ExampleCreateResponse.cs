using DomainModel.Data.Models;

namespace DomainModel.Example.Commands;

public record ExampleCreateResponse
{
    public ExampleEntity? ExampleEntity { get; set; }
}