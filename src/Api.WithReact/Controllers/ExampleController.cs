using Api.WithReact.Hubs;
using DomainModel.Commands;
using DomainModel.Models;
using DomainModel.Queries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;
using Microsoft.Graph.ExternalConnectors;

namespace Api.WithReact.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExampleController : ControllerBase {
    [HttpGet("{exampleEntityId}")]
    public async Task<ExampleLoadSingleResponse> ExampleQuery(
        [FromServices] ExampleLoadSingleQueryHandler handler,
        [FromRoute] int exampleEntityId,
        CancellationToken cancellationToken) {
        return await handler.Handle(new ExampleLoadSingleQuery() { Id = exampleEntityId }, cancellationToken);
    }

    // endpoints with complex names must use all lower case and hyphens to separate words
    [HttpGet("get-all")]
    public async Task<ExampleLoadAllResponse> ExampleQueryAll(
        [FromServices] ExampleLoadAllQueryHandler handler,
        CancellationToken cancellationToken) {
        return await handler.Handle(new ExampleLoadAllQuery(), cancellationToken);
    }

    [HttpPut()]
    public async Task<ExampleCreateResponse> ExampleCreateCommand(
        [FromServices] ExampleCreateCommandHandler handler,
        [FromServices] IExampleHubContract exampleHub,
        ExampleCreateCommand request,
        CancellationToken cancellationToken) {

        var result = await handler.Handle(request, cancellationToken);

        if (result is not null && result.ExampleEntity is not null) {
            await exampleHub.SomeEntityCreated(new SomeEntityCreatedMessage(new ExampleEntityRecord(
                result.ExampleEntity.Id,
                result.ExampleEntity.Name,
                result.ExampleEntity.Description,
                result.ExampleEntity.Email,
                result.ExampleEntity.Phone
            )));

            return result;
        }

        throw new NotImplementedException();
    }

    [HttpPost()]
    public async Task<ExampleDeleteResponse> ExampleDeleteCommand(
        [FromServices] ExampleDeleteCommandHandler handler,
        [FromServices] IExampleHubContract exampleHub,
        ExampleDeleteCommand request,
        CancellationToken cancellationToken) {
        var result = await handler.Handle(request, cancellationToken);

        await exampleHub.SomeEntityDeleted(new SomeEntityDeletedMessage(request.Id));

        return result;
    }

    [HttpPost()]
    public async Task<ExampleUpdateResponse> ExampleUpdateCommand(
    [FromServices] ExampleUpdateCommandHandler handler,
    [FromServices] IExampleHubContract exampleHub,
    ExampleUpdateCommand request,
    CancellationToken cancellationToken) {
        var result = await handler.Handle(request, cancellationToken);

        if (result is not null && result.ExampleEntity is not null) {
            await exampleHub.SomeEntityUpdated(new SomeEntityUpdatedMessage(new ExampleEntityRecord(
                result.ExampleEntity.Id,
                result.ExampleEntity.Name,
                result.ExampleEntity.Description,
                result.ExampleEntity.Email,
                result.ExampleEntity.Phone
            )));

            return result;
        }

        throw new NotImplementedException();
    }
}