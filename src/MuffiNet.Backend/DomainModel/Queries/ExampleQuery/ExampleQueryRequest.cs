using MediatR;

namespace MuffiNet.Backend.DomainModel.Queries.ExampleQuery
{
    public class ExampleQueryRequest : IRequest<ExampleQueryResponse>
    {
        public int Id { get; set; }
    }
}