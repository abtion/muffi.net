using DomainModel.Shared;

namespace DomainModel.Commands;

public record ExampleDeleteCommand : ICommand<ExampleDeleteResponse> {
    public int Id { get; set; }
}