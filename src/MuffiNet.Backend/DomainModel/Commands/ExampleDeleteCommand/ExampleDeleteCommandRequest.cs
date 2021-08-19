using MediatR;

namespace MuffiNet.Backend.DomainModel.Commands.ExampleDeleteCommand
{
    public class ExampleDeleteCommandRequest : IRequest<ExampleDeleteCommandResponse>
    {
        public int Id { get; set; }
    }
}
