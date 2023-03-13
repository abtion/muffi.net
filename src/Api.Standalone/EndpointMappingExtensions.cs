﻿using DomainModel.Commands;
using DomainModel.Queries;
using Microsoft.AspNetCore.Mvc;

public static class EndpointMappingExtensions {
    // endpoints with complex names must use all lower case and hyphens to separate words

    public static void MapEndpoints(this WebApplication app) {
        app.MapGet("/api/example-query", async ([FromServices] ExampleLoadSingleQueryHandler handler, [FromQuery] int exampleEntityId, CancellationToken cancellationToken) => {
            return await handler.Handle(new ExampleLoadSingleQuery() { Id = exampleEntityId }, cancellationToken);
        })
        .WithTags("MinimalApi");

        app.MapGet("/api/example-query-all", async ([FromServices] ExampleLoadAllQueryHandler handler, CancellationToken cancellationToken) => {
            return await handler.Handle(new ExampleLoadAllQuery(), cancellationToken);
        })
        .WithTags("MinimalApi");

        app.MapPut("/api/example-create-command", async ([FromServices] ExampleDeleteCommandHandler handler, ExampleDeleteCommand request, CancellationToken cancellationToken) => {
            return await handler.Handle(request, cancellationToken);
        })
        .WithTags("MinimalApi");

        app.MapPost("/api/example-delete-command", async ([FromServices] ExampleDeleteCommandHandler handler, ExampleDeleteCommand request, CancellationToken cancellationToken) => {
            return await handler.Handle(request, cancellationToken);
        })
        .WithTags("MinimalApi");
    }
}