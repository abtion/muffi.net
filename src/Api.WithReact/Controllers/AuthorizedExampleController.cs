using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DomainModel.Commands.ExampleCreateCommand;
using DomainModel.Commands.ExampleDeleteCommand;
using DomainModel.Queries.ExampleQuery;
using DomainModel.Queries.ExampleQueryAll;

namespace MuffiNet.FrontendReact.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AuthorizedExampleController : ControllerBase {
    [HttpGet("{idOfExampleEntity}")]
    public async Task<ActionResult<ExampleQueryResponse>> ExampleQuery([FromServices] ExampleQueryHandler handler, [FromRoute] int idOfExampleEntity, CancellationToken cancellationToken) {
        return await handler.Handle(new ExampleQueryRequest() { Id = idOfExampleEntity }, cancellationToken);
    }

    // endpoints with complex names must use all lower case and hyphens to separate words
    [HttpGet("get-all")]
    public async Task<ActionResult<ExampleQueryAllResponse>> ExampleQueryAll([FromServices] ExampleQueryAllHandler handler, CancellationToken cancellationToken) {
        return await handler.Handle(new ExampleQueryAllRequest(), cancellationToken);
    }

    [HttpPut]
    public async Task<ActionResult<ExampleCreateCommandResponse>> ExampleCreateCommand([FromServices] ExampleCreateCommandHandler handler, ExampleCreateCommandRequest request, CancellationToken cancellationToken) {
        return await handler.Handle(request, cancellationToken);
    }

    [HttpPost]
    public async Task<ActionResult<ExampleDeleteCommandResponse>> ExampleDeleteCommand([FromServices] ExampleDeleteCommandHandler handler, ExampleDeleteCommandRequest request, CancellationToken cancellationToken) {
        return await handler.Handle(request, cancellationToken);
    }
}