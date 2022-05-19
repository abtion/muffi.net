using MediatR;

namespace MuffiNet.Backend.DomainModel.Commands.ExampleCreateCommand;

public class ExampleCreateCommandRequest : IRequest<ExampleCreateCommandResponse>
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
}
