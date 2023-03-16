using DomainModel.Shared;

namespace DomainModel.Example.Commands;

public record ExampleDeleteCommand : ICommand<ExampleDeleteResponse>
{
    public int Id { get; set; }
}