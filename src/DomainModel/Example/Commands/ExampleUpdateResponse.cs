using DomainModel.Data.Models;

namespace DomainModel.Example.Commands;

public record ExampleUpdateResponse
{
    public ExampleEntity? ExampleEntity { get; set; }
}