using DomainModel.Shared;

namespace DomainModel.Commands;

public record ExampleCreateCommand : ICommand<ExampleCreateResponse> {
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
}