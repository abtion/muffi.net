using MuffiNet.Backend.Models;
using System.Collections.Generic;

namespace MuffiNet.Backend.DomainModel.Queries.ExampleQueryAll
{
    public class ExampleQueryAllResponse
    {
        public IList<ExampleEntityRecord>? ExampleEntities { get; }
        public ExampleQueryAllResponse(IList<ExampleEntityRecord> exampleEntities)
        {
            ExampleEntities = exampleEntities;
        }
    }
}