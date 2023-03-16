using DomainModel.ViewModels;
using System.Collections.Generic;

namespace DomainModel.Example.Queries;

public record ExampleLoadAllResponse
{
    public ExampleLoadAllResponse(IList<ExampleEntityRecord> exampleEntities)
    {
        ExampleEntities = exampleEntities;
    }

    public IList<ExampleEntityRecord>? ExampleEntities { get; }

}