using MuffiNet.Backend.Models;
using System.Threading.Tasks;

namespace MuffiNet.Backend.HubContracts
{
    public interface IExampleHubContract
    {
        Task SomeEntityUpdated(SomeEntityUpdatedMessage message);
        Task SomeEntityCreated(SomeEntityCreatedMessage message);
        Task SomeEntityDeleted(SomeEntityDeletedMessage message);
    }

    public class SomeEntityCreatedMessage
    {
        public SomeEntityCreatedMessage(ExampleEntity entity)
        {
            Entity = entity;
        }

        public ExampleEntity Entity { get; private set; }
    }

    public class SomeEntityUpdatedMessage
    {
        public SomeEntityUpdatedMessage(ExampleEntity entity)
        {
            Entity = entity;
        }

        public ExampleEntity Entity { get; private set; }
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