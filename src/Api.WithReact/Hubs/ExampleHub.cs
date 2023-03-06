using Microsoft.AspNetCore.SignalR;
using DomainModel.HubContracts;
using System.Diagnostics.CodeAnalysis;

namespace MuffiNet.FrontendReact.Hubs;

[ExcludeFromCodeCoverage]
public class ExampleHub : Hub, IExampleHubContract {
    private readonly IHubContext<ExampleHub> context;

    public ExampleHub(IHubContext<ExampleHub> context) {
        this.context = context;
    }

    public async virtual Task SomeEntityUpdated(SomeEntityUpdatedMessage message) {
        await context.Clients.All.SendAsync(nameof(SomeEntityUpdated), message);
    }

    public async virtual Task SomeEntityCreated(SomeEntityCreatedMessage message) {
        await context.Clients.All.SendAsync(nameof(SomeEntityCreated), message);
    }

    public async virtual Task SomeEntityDeleted(SomeEntityDeletedMessage message) {
        await context.Clients.All.SendAsync(nameof(SomeEntityDeleted), message);
    }
}