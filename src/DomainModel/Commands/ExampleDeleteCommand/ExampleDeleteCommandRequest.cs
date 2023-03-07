using MediatR;

namespace DomainModel.Commands.ExampleDeleteCommand;

public class ExampleDeleteCommandRequest : IRequest<ExampleDeleteCommandResponse>
{
    public int Id { get; set; }
}
