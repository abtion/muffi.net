using Domain.Example.Commands;
using Domain.Example.Entities;
using Domain.Example.Specifications;
using Domain.Shared;
using Domain.Shared.Exceptions;
using Infrastructure;
using Infrastructure.Data;
using System.Linq;
using Test.Shared.TestData;

using static Domain.Example.Commands.ExampleUpdateCommandHandler;

namespace Domain.Tests.Example.Commands;

[Collection("ExampleCollection")]
public class ExampleUpdateCommandHandlerTests : DomainModelTest<ExampleUpdateCommandHandler>
{
    public ExampleUpdateCommandHandlerTests()
    {
        repository = ServiceProvider.GetRequiredService<IRepository<ExampleEntity>>();
        unitOfWork = ServiceProvider.GetService<IUnitOfWork>();
        testData = ServiceProvider.GetRequiredService<ExampleTestData>();
    }

    private readonly IRepository<ExampleEntity> repository;
    private readonly ExampleTestData testData;
    private readonly IUnitOfWork unitOfWork;

    private int IdOfFirstEntity;

    private async Task< ExampleUpdateCommand> CreateValidCommand()
    {
        await testData.ResetExampleEntities(new());

        repository.AddEntity(new ExampleEntity()
        {
            Name = "Muffi",
            Description = "Head of People",
            Phone = "12348765",
            Email = "muffi@abtion.com"
        });

        await unitOfWork.SaveChangesAsync(new());

        var query = await repository.GetAll(new());

        IdOfFirstEntity = query.FirstOrDefault().Id;

        var request = new ExampleUpdateCommand()
        {
            Id = IdOfFirstEntity,
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
        var command = await CreateValidCommand();
        var sut = GetSystemUnderTest();

        var result = await sut.Handle(command, new());

        result.ExampleEntity.Should().NotBeNull();
        result.ExampleEntity.Id.Should().Be(IdOfFirstEntity);
        result.ExampleEntity.Name.Should().Be("MuffiNew");
        result.ExampleEntity.Description.Should().Be("Head of Dogs");
        result.ExampleEntity.Phone.Should().Be("56784321");
        result.ExampleEntity.Email.Should().Be("iffum@abtion.com");
    }

    [Fact]
    public async void Given_EntityDoesNotExist_When_HandlerIsCalled_Then_AnExceptionIsThrown()
    {
        var command = await CreateValidCommand();
        var sut = GetSystemUnderTest();

        command.Id = 1234;

        Task result() => sut.Handle(command, new());

        await Assert.ThrowsAsync<EntityNotFoundException>(result);
    }
}