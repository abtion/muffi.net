using MuffiNet.Backend.Models;
using System.Collections.Generic;

namespace MuffiNet.Backend.DomainModel.Queries.ExampleQueryAll
{
    public class ExampleQueryAllResponse
    {
        public List<ExampleEntity> ExampleEntities { get; set; }
    }
}