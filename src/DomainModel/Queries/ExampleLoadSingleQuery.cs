using MediatR;

namespace DomainModel.Queries;

public record ExampleLoadSingleQuery : IRequest<ExampleLoadSingleResponse>
{
    public int Id { get; set; }
}