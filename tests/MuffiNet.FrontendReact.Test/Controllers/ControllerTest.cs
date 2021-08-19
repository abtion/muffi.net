using Microsoft.Extensions.DependencyInjection;
using MuffiNet.Backend.Data;
using MuffiNet.Test.Shared;
using MuffiNet.Test.Shared.TestData;
using System;

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

            TestData = ServiceProvider.GetService<ExampleTestData>();

            var context = ServiceProvider.GetService<ApplicationDbContext>();
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }

        protected internal IServiceProvider ServiceProvider { get; set; }

        protected internal ExampleTestData TestData { get; private set; }
    }
}