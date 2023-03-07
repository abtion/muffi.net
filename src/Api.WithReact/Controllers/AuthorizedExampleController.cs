using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DomainModel.Queries;
using DomainModel.Commands;

namespace Api.WithReact.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AuthorizedExampleController : ControllerBase {
    [HttpGet("{idOfExampleEntity}")]
    public async Task<ActionResult<ExampleLoadSingleResponse>> ExampleQuery([FromServices] ExampleLoadSingleQueryHandler handler, [FromRoute] int idOfExampleEntity, CancellationToken cancellationToken) {
        return await handler.Handle(new ExampleLoadSingleQuery() { Id = idOfExampleEntity }, cancellationToken);
    }

    // endpoints with complex names must use all lower case and hyphens to separate words
    [HttpGet("get-all")]
    public async Task<ActionResult<ExampleLoadAllResponse>> ExampleQueryAll([FromServices] ExampleLoadAllQueryHandler handler, CancellationToken cancellationToken) {
        return await handler.Handle(new ExampleLoadAllQuery(), cancellationToken);
    }

    [HttpPut]
    public async Task<ActionResult<ExampleCreateResponse>> ExampleCreateCommand([FromServices] ExampleCreateCommandHandler handler, ExampleCreateCommand request, CancellationToken cancellationToken) {
        return await handler.Handle(request, cancellationToken);
    }

    [HttpPost]
    public async Task<ActionResult<ExampleDeleteResponse>> ExampleDeleteCommand([FromServices] ExampleDeleteCommandHandler handler, ExampleDeleteCommand request, CancellationToken cancellationToken) {
        return await handler.Handle(request, cancellationToken);
    }
}