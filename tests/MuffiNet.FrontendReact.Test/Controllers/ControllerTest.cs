using Microsoft.Extensions.DependencyInjection;
using System;
using MuffiNet.FrontendReact.Data;
using MuffiNet.FrontendReact.Test.DomainModel;
using MuffiNet.Test.Shared.TestData;

namespace MuffiNet.FrontendReact.Test.Controllers
{
    public abstract class ControllerTest
    {
        public ControllerTest()
        {
            var servicesBuilder = new DomainModelBuilder();
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