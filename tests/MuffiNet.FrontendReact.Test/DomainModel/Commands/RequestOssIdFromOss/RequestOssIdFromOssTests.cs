using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MuffiNet.FrontendReact.DomainModel;
using MuffiNet.FrontendReact.DomainModel.Commands.RequestOssIdFromOss;
using MuffiNet.FrontendReact.Exceptions;
using MuffiNet.FrontendReact.Models;
using MuffiNet.FrontendReact.Test.Mocks;
using MuffiNet.FrontendReact.Test.TestData;
using Xunit;
using FluentAssertions;


namespace MuffiNet.FrontendReact.Test.DomainModel.Commands.RequestOssIdFromOss
{
    public class RequestOssIdFromOssTests : DomainModelTest<RequestOssIdFromOssHandler>
    {
        private string supportTicketId = "EAA147BF-2D9A-4F41-BADE-17085AB464DF";
        private ExampleServiceMock exampleService;

        protected internal async override Task<RequestOssIdFromOssHandler> CreateSut()
        {
            var transaction = ServiceProvider.GetService<DomainModelTransaction>();

            var supportTicketTestData = new SupportTicketTestData(transaction);

            await TestData.CreateSupportTicketWithInitalFields(supportTicketId, DateTime.Now);

            exampleService = new ExampleServiceMock();
            var currentUserServiceMock = new CurrentUserServiceMock();

            return new RequestOssIdFromOssHandler(transaction, exampleService, currentUserServiceMock);
        }

        private RequestOssIdFromOssRequest CreateValidRequest()
        {
            var request = new RequestOssIdFromOssRequest()
            {
                SupportTicketId = supportTicketId,
            };

            return request;
        }

        [Fact]
        public async void Given_RequestIsValid_When_RequestOssIdFromOssHandlerIsCalled_Then_AnOssIdIsAddedOnTheSupportTicket()
        {
            var request = CreateValidRequest();
            var cancellationToken = new CancellationToken();
            var sut = await CreateSut();

            var result = await sut.Handle(request, cancellationToken);

            result.OssId.Should().NotBeEmpty();
            var transaction = ServiceProvider.GetService<DomainModelTransaction>();
            var supportTicket = transaction.Entities<SupportTicket>().Where(p => p.SupportTicketId == new Guid(request.SupportTicketId)).SingleOrDefault();
            supportTicket.OssId.Should().Be(result.OssId);
            result.OssId.Should().Be("123456");
        }


        [Fact]
        public async void Given_SupportTicketIsNotInTheDatabase_When_RequestOssIdFromOssHandlerIsCalled_Then_AnExceptionIsThrown()
        {
            var request = CreateValidRequest();

            request.SupportTicketId = "E9BC048E-10C7-4A9E-AC28-6B75CA401207";

            var cancellationToken = new CancellationToken();
            var sut = await CreateSut();

            await Assert.ThrowsAsync<SupportTicketNotFoundException>(() => sut.Handle(request, cancellationToken));
        }

    }
}
