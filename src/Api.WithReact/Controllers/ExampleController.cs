using Presentation.Example.Queries;
using Microsoft.AspNetCore.Mvc;
using Presentation.Example.Commands;
using Presentation.Example.Dtos;
using MediatR;

using static Presentation.Example.Queries.ExampleLoadAllQueryHandler;
using static Presentation.Example.Queries.ExampleLoadSingleQueryHandler;

namespace Api.WithReact.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExampleController(IMediator Mediator) : ControllerBase
{
    [HttpGet("{exampleEntityId}")]
    public async Task<ExampleLoadSingleResponse> ExampleQuery([FromServices] ExampleLoadSingleQueryHandler handler, [FromRoute] int exampleEntityId, CancellationToken cancellationToken)
    {
        return await handler.Handle(new ExampleLoadSingleQuery() { Id = exampleEntityId }, cancellationToken);
    }

    // endpoints with complex names must use all lower case and hyphens to separate words
    [HttpGet("get-all")]
    public async Task<ExampleLoadAllResponse> ExampleQueryAll([FromServices] ExampleLoadAllQueryHandler handler, CancellationToken cancellationToken)
    {
        return await handler.Handle(new ExampleLoadAllQuery(), cancellationToken);
    }

    [HttpPut()]
    public async Task<ExampleCreateResponse> ExampleCreateCommand([FromServices] ExampleCreateCommandHandler handler, ExampleCreateCommand request, CancellationToken cancellationToken)
    {
        return await Mediator.Send(request, cancellationToken);
        //return await handler.Handle(request, cancellationToken);
    }

    [HttpDelete()]
    public async Task<ExampleDeleteResponse> ExampleDeleteCommand([FromServices] ExampleDeleteCommandHandler handler, ExampleDeleteCommand request, CancellationToken cancellationToken)
    {
        return await Mediator.Send(request, cancellationToken);

        //return await handler.Handle(request, cancellationToken);
    }

    [HttpPost()]
    public async Task<ExampleUpdateResponse> ExampleUpdateCommand([FromServices] ExampleUpdateCommandHandler handler, ExampleUpdateCommand request, CancellationToken cancellationToken)
    {
        return await Mediator.Send(request, cancellationToken);
        //return await handler.Handle(request, cancellationToken);
    }
}
