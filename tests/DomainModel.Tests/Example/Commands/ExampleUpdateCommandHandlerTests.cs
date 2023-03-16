using DomainModel.Data.Models;
using DomainModel.Example.Commands;
using DomainModel.Shared.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Test.Shared.TestData;

namespace DomainModel.Tests.Example.Commands;

[Collection("ExampleCollection")]
public class ExampleUpdateCommandHandlerTests : DomainModelTest<ExampleUpdateCommandHandler>
{
    public ExampleUpdateCommandHandlerTests()
    {
        transaction = ServiceProvider.GetRequiredService<DomainModelTransaction>();
        transaction.ResetExampleEntities();

        transaction.AddExampleEntity(new ExampleEntity()
        {
            Id = 10,
            Name = "Muffi",
            Description = "Head of People",
            Phone = "12348765",
            Email = "muffi@abtion.com"
        });

        testData = ServiceProvider.GetRequiredService<ExampleTestData>();
    }

    private readonly DomainModelTransaction transaction;
    private readonly ExampleTestData testData;

    private ExampleUpdateCommand CreateValidCommand()
    {
        var request = new ExampleUpdateCommand()
        {
            Id = 10,
            Name = "MuffiNew",
            Description = "Head of Dogs",
            Phone = "56784321",
            Email = "iffum@abtion.com"
        };

        return request;
    }

    [Fact]
    public async void Given_RequestIsValid_When_HandlerIsCalled_Then_TheEntityIsUpdatedAndReturned()
    {
        var command = CreateValidCommand();
        var sut = GetSystemUnderTest();

        var result = await sut.Handle(command, new());

        result.ExampleEntity.Should().NotBeNull();
        result.ExampleEntity.Id.Should().Be(10);
        result.ExampleEntity.Name.Should().Be("MuffiNew");
        result.ExampleEntity.Description.Should().Be("Head of Dogs");
        result.ExampleEntity.Phone.Should().Be("56784321");
        result.ExampleEntity.Email.Should().Be("iffum@abtion.com");
    }

    [Fact]
    public async void Given_EntityDoesNotExist_When_HandlerIsCalled_Then_AnExceptionIsThrown()
    {
        var command = CreateValidCommand();
        var sut = GetSystemUnderTest();

        command.Id = 1234;

        Task result() => sut.Handle(command, new());

        await Assert.ThrowsAsync<EntityNotFoundException>(result);
    }
}