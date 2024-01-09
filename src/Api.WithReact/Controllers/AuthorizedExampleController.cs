﻿using Domain.Example.Commands;
using Domain.Example.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Domain.Example.Commands.ExampleCreateCommandHandler;
using static Domain.Example.Commands.ExampleDeleteCommandHandler;
using static Domain.Example.Commands.ExampleUpdateCommandHandler;
using static Domain.Example.Queries.ExampleLoadAllQueryHandler;
using static Domain.Example.Queries.ExampleLoadSingleQueryHandler;

namespace Api.WithReact.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AuthorizedExampleController : ControllerBase
{
    [HttpGet("{exampleEntityId}")]
    public async Task<ExampleLoadSingleResponse> ExampleQuery(
        [FromServices] ExampleLoadSingleQueryHandler handler,
        [FromRoute] int exampleEntityId,
        CancellationToken cancellationToken
    )
    {
        return await handler.Handle(
            new ExampleLoadSingleQuery() { Id = exampleEntityId },
            cancellationToken
        );
    }

    // endpoints with complex names must use all lower case and hyphens to separate words
    [HttpGet("get-all")]
    public async Task<ExampleLoadAllResponse> ExampleQueryAll(
        [FromServices] ExampleLoadAllQueryHandler handler,
        CancellationToken cancellationToken
    )
    {
        return await handler.Handle(new ExampleLoadAllQuery(), cancellationToken);
    }

    [HttpPut()]
    public async Task<ExampleCreateResponse> ExampleCreateCommand(
        [FromServices] ExampleCreateCommandHandler handler,
        ExampleCreateCommand request,
        CancellationToken cancellationToken
    )
    {
        return await handler.Handle(request, cancellationToken);
    }

    [HttpDelete()]
    public async Task<ExampleDeleteResponse> ExampleDeleteCommand(
        [FromServices] ExampleDeleteCommandHandler handler,
        ExampleDeleteCommand request,
        CancellationToken cancellationToken
    )
    {
        return await handler.Handle(request, cancellationToken);
    }

    [HttpPost()]
    public async Task<ExampleUpdateResponse> ExampleUpdateCommand(
        [FromServices] ExampleUpdateCommandHandler handler,
        ExampleUpdateCommand request,
        CancellationToken cancellationToken
    )
    {
        return await handler.Handle(request, cancellationToken);
    }
}
