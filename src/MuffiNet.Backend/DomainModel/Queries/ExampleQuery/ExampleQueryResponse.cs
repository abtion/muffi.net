using MuffiNet.Backend.Models;

namespace MuffiNet.Backend.DomainModel.Queries.ExampleQuery
{
    public class ExampleQueryResponse
    {
        public ExampleEntityRecord? ExampleEntity { get; set; }

        public record ExampleEntityRecord(
            int Id,
            string Name,
            string Description,
            string Email,
            string Phone
        );
    }
}