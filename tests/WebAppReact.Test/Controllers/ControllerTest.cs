using Microsoft.Extensions.DependencyInjection;
using System;
using WebAppReact.Data;
using WebAppReact.Test.DomainModel;
using WebAppReact.Test.TestData;

namespace WebAppReact.Test.Controllers
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