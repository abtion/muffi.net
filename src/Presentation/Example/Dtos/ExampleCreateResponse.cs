namespace Presentation.Example.Dtos;

public record ExampleCreateResponse(ExampleDto ExampleEntity)
{
    public ExampleDto ExampleEntity { get; } = ExampleEntity;
}
