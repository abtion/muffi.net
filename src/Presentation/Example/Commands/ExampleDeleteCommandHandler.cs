using Domain.Example.UseCases;
using Presentation.Example.Dtos;

namespace Presentation.Example.Commands;

public class ExampleDeleteCommandHandler(DeleteExampleUseCase useCase) : ICommandHandler<ExampleDeleteCommand, ExampleDeleteResponse>
{
    public async Task<ExampleDeleteResponse> Handle(ExampleDeleteCommand request, CancellationToken cancellationToken)
    {
        await useCase.Handle(request.Id, cancellationToken);

        return new ExampleDeleteResponse();
    }
}