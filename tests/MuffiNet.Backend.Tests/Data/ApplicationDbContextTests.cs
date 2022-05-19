using Duende.IdentityServer.EntityFramework.Options;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MuffiNet.Backend.Data;
using Xunit;

namespace MuffiNet.Backend.Tests.Data;

[Collection("DataCollection")]
public class ApplicationDbContextTests
{
    [Fact]
    public void ApplicationDbContext_Should_Be_Constructed()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDbContext")
            .Options;

        OperationalStoreOptions storeOptions = new OperationalStoreOptions
        {
            //populate needed members
        };

        IOptions<OperationalStoreOptions> operationalStoreOptions = Options.Create(storeOptions);

        using (var _context = new ApplicationDbContext(options, operationalStoreOptions))
        {
            _context.Should().NotBeNull();
        }
    }
}