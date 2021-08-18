using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using MuffiNet.Backend.HubContracts;
using System.Threading.Tasks;

namespace MuffiNet.FrontendReact.Hubs
{
    public class ExampleHub : Hub, IExampleHubContract
    {
        private readonly IHubContext<ExampleHub> context;

        public ExampleHub(IHubContext<ExampleHub> context)
        {
            this.context = context;
        }

        public async virtual Task SomeEntityUpdated(SomeEntityUpdatedMessage message)
        {
            await context.Clients.All.SendAsync(nameof(SomeEntityUpdated), message);
        }

        public async virtual Task SomeEntityCreated(SomeEntityCreatedMessage message)
        {
            await context.Clients.All.SendAsync(nameof(SomeEntityCreated), message);
        }

        public async virtual Task SomeEntityDeleted(SomeEntityDeletedMessage message)
        {
            await context.Clients.All.SendAsync(nameof(SomeEntityDeleted), message);
        }
    }
}