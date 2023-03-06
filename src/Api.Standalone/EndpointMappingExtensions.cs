using Microsoft.AspNetCore.Mvc;
using DomainModel.Commands.ExampleDeleteCommand;
using DomainModel.Queries.ExampleQuery;
using DomainModel.Queries.ExampleQueryAll;

public static class EndpointMappingExtensions {
    // endpoints with complex names must use all lower case and hyphens to separate words

    public static void MapEndpoints(this WebApplication app) {
        app.MapGet("/api/example-query", async ([FromServices] ExampleQueryHandler handler, [FromQuery] int exampleEntityId, CancellationToken cancellationToken) => {
            return await handler.Handle(new ExampleQueryRequest() { Id = exampleEntityId }, cancellationToken);
        })
        .WithTags("MinimalApi");

        app.MapGet("/api/example-query-all", async ([FromServices] ExampleQueryAllHandler handler, CancellationToken cancellationToken) => {
            return await handler.Handle(new ExampleQueryAllRequest(), cancellationToken);
        })
        .WithTags("MinimalApi");

        app.MapPut("/api/example-create-command", async ([FromServices] ExampleDeleteCommandHandler handler, ExampleDeleteCommandRequest request, CancellationToken cancellationToken) => {
            return await handler.Handle(request, cancellationToken);
        })
        .WithTags("MinimalApi");

        app.MapPost("/api/example-delete-command", async ([FromServices] ExampleDeleteCommandHandler handler, ExampleDeleteCommandRequest request, CancellationToken cancellationToken) => {
            return await handler.Handle(request, cancellationToken);
        })
        .WithTags("MinimalApi");
    }
}