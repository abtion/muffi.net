namespace Presentation.Example.Dtos;

public record ExampleDeleteCommand : ICommand<ExampleDeleteResponse>
{
    public int Id { get; set; }
}
