using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MuffiNet.FrontendReact.Data;
using MuffiNet.FrontendReact.DomainModel;
using MuffiNet.FrontendReact.Models;
using MuffiNet.FrontendReact.Test.TestData;

namespace MuffiNet.FrontendReact.Test.DomainModel
{
    public class DomainModelBuilder
    {
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration, string databaseName)
        {
            services.AddSingleton(Options.Create<OperationalStoreOptions>(new OperationalStoreOptions()));

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase(databaseName));

            // add an InMemory
            services.AddIdentityCore<ApplicationUser>()
                .AddUserManager<UserManager<ApplicationUser>>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddDomainModel();

            services.AddScoped<SupportTicketTestData>();
        }
    }
}