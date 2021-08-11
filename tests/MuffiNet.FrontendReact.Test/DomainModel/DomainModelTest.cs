using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using MuffiNet.FrontendReact.Data;
using MuffiNet.FrontendReact.DomainModel;
using MuffiNet.FrontendReact.Models;
using MuffiNet.Test.Shared.TestData;

namespace MuffiNet.FrontendReact.Test.DomainModel
{
    public abstract class DomainModelTest<T>
    {
        public DomainModelTest()
        {
            var servicesBuilder = new DomainModelBuilder();
            var serviceCollection = new ServiceCollection();

            servicesBuilder.ConfigureServices(serviceCollection, null, this.GetType().Name);

            ServiceProvider = serviceCollection.BuildServiceProvider();

            var context = ServiceProvider.GetService<ApplicationDbContext>();
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            TestData = ServiceProvider.GetService<SupportTicketTestData>();
            Transaction = ServiceProvider.GetService<DomainModelTransaction>();
            UserManager = ServiceProvider.GetService<UserManager<ApplicationUser>>();

            // create a test user
            UserManager.CreateAsync(ApplicationUserTestData.CreateApplicationUser());
        }

        protected internal IServiceProvider ServiceProvider { get; private set; }

        protected internal abstract Task<T> CreateSut();

        protected internal SupportTicketTestData TestData { get; private set; }

        protected internal DomainModelTransaction Transaction { get; private set; }

        protected internal UserManager<ApplicationUser> UserManager { get; private set; }
    }
}