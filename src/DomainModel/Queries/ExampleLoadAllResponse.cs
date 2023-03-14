using DomainModel.Models;
using System.Collections.Generic;

namespace DomainModel.Queries;

public record ExampleLoadAllResponse {
    public ExampleLoadAllResponse(IList<ExampleEntityRecord> exampleEntities) {
        ExampleEntities = exampleEntities;
    }

    public IList<ExampleEntityRecord>? ExampleEntities { get; }

}