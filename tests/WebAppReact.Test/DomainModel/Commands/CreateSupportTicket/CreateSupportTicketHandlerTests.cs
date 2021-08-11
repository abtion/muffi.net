﻿using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebAppReact.Data;
using WebAppReact.DomainModel;
using WebAppReact.DomainModel.Commands.CreateSupportTicket;
using WebAppReact.Test.Mocks;
using Xunit;

namespace WebAppReact.Test.DomainModel.Commands.CreateSupportTicket
{
    public class CreateSupportTicketHandlerTests : DomainModelTest<CreateSupportTicketHandler>
    {
        private TechnicianHubMock technicianHub;

        protected internal async override Task<CreateSupportTicketHandler> CreateSut()
        {
            var domainModelTransaction = ServiceProvider.GetService<DomainModelTransaction>();
            var currentDateTimeService = CurrentDateTimeServiceMock.MockCurrentDateTimeService();

            technicianHub = new TechnicianHubMock();

            return await Task.FromResult(new CreateSupportTicketHandler(domainModelTransaction, currentDateTimeService, technicianHub));
        }

        private CreateSupportTicketRequest CreateValidRequest()
        {
            var request = new CreateSupportTicketRequest()
            {
                CustomerName = "Ruth",
                CustomerEmail = "Ruth@msn.dk",
                CustomerPhone = "12345678",
                Brand = "Apple"
            };

            return request;
        }

        [Fact]
        public async void Given_RequestIsValid_When_CreateSuportTicketHandlerIsCalled_Then_ASupportTicketIsCreated()
        {
            var request = CreateValidRequest();
            var cancellationToken = new CancellationToken();
            var sut = await CreateSut();

            var result = await sut.Handle(request, cancellationToken);

            Assert.Equal(1, ServiceProvider.GetService<ApplicationDbContext>().SupportTickets.Count());

            var supportTicket = ServiceProvider.GetService<ApplicationDbContext>().SupportTickets.First();

            Assert.Equal(1, supportTicket.Id);
            Assert.Equal(request.CustomerName, supportTicket.CustomerName);
            Assert.Equal(request.CustomerEmail, supportTicket.CustomerEmail);
            Assert.Equal(request.CustomerPhone, supportTicket.CustomerPhone);
            Assert.Equal(CurrentDateTimeServiceMock.MockedDateTime(), supportTicket.CreatedAt);
            Assert.Equal(request.Brand, supportTicket.Brand);
            Assert.NotEqual(Guid.Empty, result.SupportTicketId);
        }

        [Fact]
        public async void Given_RequestIsValid_When_CreateSuportTicketHandlerIsCalled_Then_ASupportTicketIdIsGenerated()
        {
            var request = CreateValidRequest();
            var cancellationToken = new CancellationToken();
            var sut = await CreateSut();

            var result = await sut.Handle(request, cancellationToken);

            Assert.NotEqual(Guid.Empty, ServiceProvider.GetService<ApplicationDbContext>().SupportTickets.First().SupportTicketId);
            Assert.NotEqual(Guid.Empty, result.SupportTicketId);
        }

        [Fact]
        public async void Given_RequestIsValid_When_CreateSuportTicketHandlerIsCalledTwice_Then_TheIdIncrements()
        {
            var request = CreateValidRequest();
            var cancellationToken = new CancellationToken();
            var sut = await CreateSut();

            await sut.Handle(request, cancellationToken);
            await sut.Handle(request, cancellationToken);

            Assert.Equal(2, ServiceProvider.GetService<ApplicationDbContext>().SupportTickets.Count());

            Assert.Equal(1, ServiceProvider.GetService<ApplicationDbContext>().SupportTickets.First().Id);
            Assert.Equal(2, ServiceProvider.GetService<ApplicationDbContext>().SupportTickets.Last().Id);
        }

        [Fact]
        public async void Given_RequestIsValid_When_CreateSuportTicketHandlerIsCalled_Then_AMessageIsSentToTheTechnician()
        {
            var request = CreateValidRequest();
            var cancellationToken = new CancellationToken();
            var sut = await CreateSut();

            var response = await sut.Handle(request, cancellationToken);

            Assert.Equal(1, technicianHub.SupportTicketCreatedMessageCounter);

            var supportTicketEntity = ServiceProvider.GetService<ApplicationDbContext>().SupportTickets.First();
            Assert.Equal(response.SupportTicketId, supportTicketEntity.SupportTicketId);

            Assert.Equal(supportTicketEntity.Brand, technicianHub.LatestSupportTicketCreatedMessage.SupportTicket.Brand);
            Assert.Equal(supportTicketEntity.CreatedAt, technicianHub.LatestSupportTicketCreatedMessage.SupportTicket.CreatedAt);
            Assert.Equal(supportTicketEntity.CustomerEmail, technicianHub.LatestSupportTicketCreatedMessage.SupportTicket.CustomerEmail);
            Assert.Equal(supportTicketEntity.CustomerName, technicianHub.LatestSupportTicketCreatedMessage.SupportTicket.CustomerName);
            Assert.Equal(supportTicketEntity.CustomerPhone, technicianHub.LatestSupportTicketCreatedMessage.SupportTicket.CustomerPhone);
            Assert.Equal(supportTicketEntity.SupportTicketId, technicianHub.LatestSupportTicketCreatedMessage.SupportTicket.SupportTicketId);
        }

        [Fact]
        public async void Given_RequestIsNull_When_CreateSuportTicketHandlerIsCalled_Then_AnArgumentNullExceptionIsThrown()
        {
            var cancellationToken = new CancellationToken();
            var sut = await CreateSut();

            await Assert.ThrowsAsync<ArgumentNullException>(() => sut.Handle(null, cancellationToken));
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async void Given_CustomerNameIsNotValid_When_CreateSuportTicketHandlerIsCalled_Then_AnArgumentNullExceptionIsThrown(string value)
        {
            var sut = await CreateSut();
            var cancellationToken = new CancellationToken();

            var request = CreateValidRequest();
            request.CustomerName = value;

            await Assert.ThrowsAsync<ArgumentNullException>(() => sut.Handle(request, cancellationToken));
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async void Given_CustomerEmailIsNotValid_When_CreateSuportTicketHandlerIsCalled_Then_AnArgumentNullExceptionIsThrown(string value)
        {
            var sut = await CreateSut();
            var cancellationToken = new CancellationToken();

            var request = CreateValidRequest();
            request.CustomerEmail = value;

            await Assert.ThrowsAsync<ArgumentNullException>(() => sut.Handle(request, cancellationToken));
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async void Given_CustomerPhoneIsNotValid_When_CreateSuportTicketHandlerIsCalled_Then_AnArgumentNullExceptionIsThrown(string value)
        {
            var sut = await CreateSut();
            var cancellationToken = new CancellationToken();

            var request = CreateValidRequest();
            request.CustomerPhone = value;

            await Assert.ThrowsAsync<ArgumentNullException>(() => sut.Handle(request, cancellationToken));
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async void Given_CustomerBrandIsNotValid_When_CreateSuportTicketHandlerIsCalled_Then_AnArgumentNullExceptionIsThrown(string value)
        {
            var sut = await CreateSut();
            var cancellationToken = new CancellationToken();

            var request = CreateValidRequest();
            request.Brand = value;

            await Assert.ThrowsAsync<ArgumentNullException>(() => sut.Handle(request, cancellationToken));
        }
    }
}