using MediatR;

namespace MuffiNet.Backend.DomainModel.Commands.ExampleCreateCommand
{
    public class ExampleCreateCommandRequest : IRequest<ExampleCreateCommandResponse>
    {
        public string Name {  get; set; }
        public string Description {  get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
