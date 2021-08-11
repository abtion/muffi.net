using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace MuffiNet.FrontendReact.Hubs
{
    public interface IExampleHubContract
    {
        Task SomeEntityUpdated(SomeEntityUpdatedMessage message);
        Task SomeEntityCreated(SomeEntityCreatedMessage message);
        Task SomeEntityDeleted(SomeEntityDeletedMessage message);
    }

    [Authorize]
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

    public class SomeEntityCreatedMessage
    {
        public SomeEntityCreatedMessage(string entityId)
        {
            EntityId = entityId;
        }

        public string EntityId { get; private set; }
    }

    public class SomeEntityUpdatedMessage
    {
        public SomeEntityUpdatedMessage(string entityId)
        {
            EntityId = entityId;
        }

        public string EntityId { get; private set; }
    }

    public class SomeEntityDeletedMessage
    {
        public SomeEntityDeletedMessage(string entityId)
        {
            EntityId = entityId;
        }

        public string EntityId { get; private set; }
    }
}