using MediatR;

namespace DomainModel.Commands;

public record ExampleCreateCommand : IRequest<ExampleCreateResponse> {
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
}