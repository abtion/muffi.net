using MediatR;

namespace MuffiNet.Backend.DomainModel.Commands.ExampleUpdateCommand
{
    public class ExampleUpdateCommandRequest : IRequest<ExampleUpdateCommandResponse>
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
    }
}
