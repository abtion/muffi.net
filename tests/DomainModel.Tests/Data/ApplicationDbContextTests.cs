using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using DomainModel.Data;
using Xunit;

namespace MuffiNet.Backend.Tests.Data;

[Collection("DataCollection")]
public class ApplicationDbContextTests {
    [Fact]
    public void ApplicationDbContext_Should_Be_Constructed() {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDbContext")
            .Options;

        using (var _context = new ApplicationDbContext(options)) {
            _context.Should().NotBeNull();
        }
    }
}