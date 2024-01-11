namespace Presentation.Example.Dtos;

public record ExampleUpdateResponse(ExampleDto ExampleEntity)
{
    public ExampleDto ExampleEntity { get; } = ExampleEntity;
}
