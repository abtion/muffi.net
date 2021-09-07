using MuffiNet.Backend.Models;
using System.Collections.Generic;

namespace MuffiNet.Backend.DomainModel.Queries.ExampleQueryAll
{
    public class ExampleQueryAllResponse
    {
        public List<ExampleEntityRecord>? ExampleEntities { get; set; }

        public record ExampleEntityRecord(
           int Id,
           string Name,
           string Description,
           string Email,
           string Phone
       );
    }
}