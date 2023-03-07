using Microsoft.AspNetCore.Mvc;
using DomainModel.Commands;
using DomainModel.Queries;

namespace Api.WithReact.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExampleController : ControllerBase {
    [HttpGet("{exampleEntityId}")]
    public async Task<ActionResult<ExampleLoadSingleResponse>> ExampleQuery([FromServices] ExampleLoadSingleQueryHandler handler, [FromRoute] int exampleEntityId, CancellationToken cancellationToken) {
        return await handler.Handle(new ExampleLoadSingleQuery() { Id = exampleEntityId }, cancellationToken);
    }

    // endpoints with complex names must use all lower case and hyphens to separate words
    [HttpGet("get-all")]
    public async Task<ActionResult<ExampleLoadAllResponse>> ExampleQueryAll([FromServices] ExampleLoadAllQueryHandler handler, CancellationToken cancellationToken) {
        return await handler.Handle(new ExampleLoadAllQuery(), cancellationToken);
    }

    [HttpPut()]
    public async Task<ActionResult<ExampleCreateResponse>> ExampleCreateCommand([FromServices] ExampleCreateCommandHandler handler, ExampleCreateCommand request, CancellationToken cancellationToken) {
        return await handler.Handle(request, cancellationToken);
    }

    [HttpPost()]
    public async Task<ActionResult<ExampleDeleteResponse>> ExampleDeleteCommand([FromServices] ExampleDeleteCommandHandler handler, ExampleDeleteCommand request, CancellationToken cancellationToken) {
        return await handler.Handle(request, cancellationToken);
    }
}