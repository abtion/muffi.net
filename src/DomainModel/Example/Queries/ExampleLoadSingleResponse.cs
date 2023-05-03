using DomainModel.Example.ViewModels;

namespace DomainModel.Example.Queries;

public record ExampleLoadSingleResponse
{
    public ExampleEntityRecord? ExampleEntity { get; set; }
}