using Microsoft.Extensions.DependencyInjection;
using System;
using MuffiNet.Backend.Data;
using MuffiNet.Test.Shared.TestData;
using MuffiNet.Test.Shared;

namespace MuffiNet.FrontendReact.Test.Controllers
{
    public abstract class ControllerTest
    {
        public ControllerTest()
        {
            var servicesBuilder = new DomainModelBuilderForTest();
            var serviceCollection = new ServiceCollection();

            servicesBuilder.ConfigureServices(serviceCollection, null, this.GetType().Name);

            ServiceProvider = serviceCollection.BuildServiceProvider();

            TestData = ServiceProvider.GetService<SupportTicketTestData>();

            var context = ServiceProvider.GetService<ApplicationDbContext>();
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }

        protected internal IServiceProvider ServiceProvider { get; set; }

        protected internal SupportTicketTestData TestData { get; private set; }
    }
}