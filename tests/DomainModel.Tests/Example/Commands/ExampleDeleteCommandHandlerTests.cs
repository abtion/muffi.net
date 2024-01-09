using Domain.Example.Commands;
using Domain.Example.Entities;
using Domain.Shared;
using Test.Shared.TestData;

using static Domain.Example.Commands.ExampleDeleteCommandHandler;

namespace Domain.Tests.Example.Commands;

[Collection("ExampleCollection")]
public class ExampleDeleteCommandHandlerTests : DomainModelTest<ExampleDeleteCommandHandler>
{
    public ExampleDeleteCommandHandlerTests()
    {
        repository = ServiceProvider.GetRequiredService<IRepository<ExampleEntity>>();
        unitOfWork = ServiceProvider.GetService<IUnitOfWork>();
        testData = ServiceProvider.GetRequiredService<ExampleTestData>();
    }

    private readonly IRepository<ExampleEntity> repository;
    private readonly ExampleTestData testData;
    private readonly IUnitOfWork unitOfWork;

    private ExampleDeleteCommand CreateValidCommand()
    {
        var request = new ExampleDeleteCommand()
        {
            Id = 1,
        };

        return request;
    }

    [Fact]
    public async void Given_RequestIsValid_When_HandlerIsCalled_Then_TheEntityIsRemoved()
    {
        await testData.ResetExampleEntities(new());
        
        repository.AddEntity(new ExampleEntity()
        {
            Id = 1,
            Name = "Muffi",
            Description = "Head of People",
            Phone = "89898989"
        });
        
        await unitOfWork.SaveChangesAsync(new());

        var command = CreateValidCommand();
        var sut = GetSystemUnderTest();
        
        _ = await sut.Handle(command, new());

        (await repository.GetAll(new())).Should().BeEmpty();
    }

    [Fact]
    public async void Given_RequestIsValid_When_HandlerIsCalled_Then_TheReturnTypeIsNotNull()
    {
        await testData.ResetExampleEntities(new());

        repository.AddEntity(new ExampleEntity()
        {
            Id = 1,
            Name = "Muffi",
            Description = "Head of People",
            Phone = "89898989"
        });

        await unitOfWork.SaveChangesAsync(new());

        var command = CreateValidCommand();
        var sut = GetSystemUnderTest();

        var result = await sut.Handle(command, new());

        result.Should().NotBeNull();
    }
}