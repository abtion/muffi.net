using MediatR;

namespace MuffiNet.Backend.DomainModel.Commands.ExampleCommand
{
    public class ExampleCommandRequest : IRequest<ExampleCommandResponse>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
