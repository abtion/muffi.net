using MediatR;

namespace DomainModel.Commands;

public record ExampleDeleteCommand : IRequest<ExampleDeleteResponse> {
    public int Id { get; set; }
}