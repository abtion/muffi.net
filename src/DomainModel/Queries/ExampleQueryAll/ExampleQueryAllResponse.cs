using DomainModel.Models;
using System.Collections.Generic;

namespace DomainModel.Queries.ExampleQueryAll;

public class ExampleQueryAllResponse
{
    public IList<ExampleEntityRecord>? ExampleEntities { get; }
    public ExampleQueryAllResponse(IList<ExampleEntityRecord> exampleEntities)
    {
        ExampleEntities = exampleEntities;
    }
}