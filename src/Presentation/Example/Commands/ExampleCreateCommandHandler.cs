using Domain.Example.UseCases;
using Presentation.Example.Dtos;
using Presentation.Example.Mappers;

namespace Presentation.Example.Commands;

public class ExampleCreateCommandHandler(CreateExampleUseCase UseCase, ExampleMapper Mapper) : ICommandHandler<ExampleCreateCommand, ExampleCreateResponse>
{
    public async Task<ExampleCreateResponse> Handle(ExampleCreateCommand request, CancellationToken cancellationToken)
    {
        var entity = Mapper.MapCreateCommandToEntity(request);

        var result = await UseCase.Handle(entity, cancellationToken);

        var dto = Mapper.MapEntityToDto(result);

        return new ExampleCreateResponse(dto);
    }
}
