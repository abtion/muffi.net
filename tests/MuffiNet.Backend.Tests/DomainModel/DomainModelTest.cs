using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using MuffiNet.Backend.Data;
using MuffiNet.Backend.DomainModel;
using MuffiNet.Backend.Models;
using MuffiNet.Test.Shared.TestData;
using MuffiNet.Test.Shared;

namespace MuffiNet.Backend.Tests.DomainModel
{
    public abstract class DomainModelTest<T>
    {
        public DomainModelTest()
        {
            var servicesBuilder = new DomainModelBuilderForTest();
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