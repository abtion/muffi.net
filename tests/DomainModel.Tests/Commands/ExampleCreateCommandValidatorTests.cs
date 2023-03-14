using DomainModel.Commands;
using FluentAssertions;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DomainModel.Tests.Commands {
    public class ExampleCreateCommandValidatorTests : DomainModelTest<IMediator> {
        [Fact]
        public async void Given_NameIsMissing_When_CommandIsSent_Then_ValidatorRaisesException() {
            var mediator = await CreateSut();

            try {
                await mediator.Send(new ExampleCreateCommand() {
                    Name = string.Empty,
                    Description = "My description",
                    Email = "a@b.c",
                    Phone = "112"
                });

                false.Should().BeTrue("The above statement should throw an exception");
            }
            catch (ValidationException ex) {
                ex.Errors.Should().HaveCount(1);

                var error = ex.Errors.First();

                error.Severity.Should().Be(Severity.Error);
                error.ErrorMessage.Should().Be("Name cannot be empty");
            }
        }

        [Fact]
        public async void Given_DescriptionIsMissing_When_CommandIsSent_Then_ValidatorRaisesException() {
            var mediator = await CreateSut();

            try {
                await mediator.Send(new ExampleCreateCommand() {
                    Name = "My name",
                    Description = string.Empty,
                    Email = "a@b.c",
                    Phone = "112"
                });

                false.Should().BeTrue("The above statement should throw an exception");
            }
            catch (ValidationException ex) {
                ex.Errors.Should().HaveCount(1);

                var error = ex.Errors.First();

                error.Severity.Should().Be(Severity.Error);
                error.ErrorMessage.Should().Be("Description cannot be empty");
            }
        }

        [Fact]
        public async void Given_EmailIsMissing_When_CommandIsSent_Then_ValidatorRaisesException() {
            var mediator = await CreateSut();

            try {
                await mediator.Send(new ExampleCreateCommand() {
                    Name = "My name",
                    Description = "My description",
                    Email = string.Empty,
                    Phone = "112"
                });

                false.Should().BeTrue("The above statement should throw an exception");
            }
            catch (ValidationException ex) {
                ex.Errors.Should().HaveCount(2);

                var error = ex.Errors.First();

                error.Severity.Should().Be(Severity.Error);
                error.ErrorMessage.Should().Be("Email address cannot be empty");
            }
        }

        [Fact]
        public async void Given_EmailHasWrongFormat_When_CommandIsSent_Then_ValidatorRaisesException() {
            var mediator = await CreateSut();

            try {
                await mediator.Send(new ExampleCreateCommand() {
                    Name = "My name",
                    Description = "My description",
                    Email = "ab",
                    Phone = "112"
                });

                false.Should().BeTrue("The above statement should throw an exception");
            }
            catch (ValidationException ex) {
                ex.Errors.Should().HaveCount(1);

                var error = ex.Errors.First();

                error.Severity.Should().Be(Severity.Error);
                error.ErrorMessage.Should().Be("Please specify a valid email address");
            }
        }

        [Fact]
        public async void Given_PhoneIsMissing_When_CommandIsSent_Then_ValidatorRaisesException() {
            var mediator = await CreateSut();

            try {
                await mediator.Send(new ExampleCreateCommand() {
                    Name = "My name",
                    Description = "My description",
                    Email = "a@b.c",
                    Phone = string.Empty
                });

                false.Should().BeTrue("The above statement should throw an exception");
            }
            catch (ValidationException ex) {
                ex.Errors.Should().HaveCount(1);

                var error = ex.Errors.First();

                error.Severity.Should().Be(Severity.Error);
                error.ErrorMessage.Should().Be("Phone cannot be empty");
            }
        }

        [Fact]
        public async void Given_CommandIsValid_When_CommandIsSent_Then_ValidatorPasses() {
            var mediator = await CreateSut();

            var response = await mediator.Send(new ExampleCreateCommand() {
                Name = "My name",
                Description = "My description",
                Email = "a@b.c",
                Phone = "112"
            });

            response.Should().NotBeNull();
            response.ExampleEntity.Should().NotBeNull();
            response.ExampleEntity.Id.Should().BeGreaterThan(0);
        }

        protected internal override Task<IMediator> CreateSut() {
            return Task.FromResult(ServiceProvider.GetService<IMediator>());
        }
    }
}