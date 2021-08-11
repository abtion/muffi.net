using System;
using System.Threading.Tasks;
using WebAppReact.DomainModel;
using WebAppReact.Models;

namespace WebAppReact.Test.TestData
{
    public class SupportTicketTestData
    {
        private readonly DomainModelTransaction domainModelTransaction;

        public SupportTicketTestData(DomainModelTransaction domainModelTransaction)
        {
            this.domainModelTransaction = domainModelTransaction ?? throw new ArgumentNullException(nameof(domainModelTransaction));
        }

        public async Task<SupportTicket> CreateSupportTicketWithInitalFields(string supportTicketId, DateTime createdAt)
        {
            var supportTicket = new SupportTicket()
            {
                SupportTicketId = new Guid(supportTicketId),
                CustomerEmail = "a@b.c",
                CustomerName = "Ruth",
                CustomerPhone = "70 70 70 70",
                CreatedAt = createdAt,
            };

            await domainModelTransaction.AddAsync<SupportTicket>(supportTicket);
            await domainModelTransaction.SaveChangesAsync();

            return supportTicket;
        }

        public async Task<SupportTicket> CreateSupportTicketWithTwilioRoom(Guid supportTicketId)
        {
            var supportTicket = new SupportTicket()
            {
                CustomerName = "Ruth",
                CustomerEmail = "Ruth@msn.dk",
                CustomerPhone = "12345678",
                SupportTicketId = supportTicketId,
                CreatedAt = new DateTime(2021, 06, 17),
                TwilioRoomName = $"room_{supportTicketId}",
                TwilioVideoGrantForCustomerToken = "customer1234567890",
                TwilioVideoGrantForTechnicianToken = "technician1234567890",
                TechnicianUserId = "e0169f6f-c521-4d75-9144-a46c692af355",
                TwilioRoomSid = "RM2124b63d675b17ad52b8af15d2bc511d"
            };

            await domainModelTransaction.AddAsync<SupportTicket>(supportTicket);
            await domainModelTransaction.SaveChangesAsync();

            return supportTicket;
        }

        public async Task<SupportTicket> CreateSupportTicketWithTwilioRoom(Guid supportTicketId, string twilioTechToken, string twilioCustToken, string twilioRoomSid)
        {
            var supportTicket = new SupportTicket
            {
                CustomerName = "Ruth",
                CustomerEmail = "Ruth@msn.dk",
                CustomerPhone = "12345678",
                SupportTicketId = supportTicketId,
                CreatedAt = new DateTime(2021, 06, 17),
                Brand = "Apple",
                TwilioRoomName = $"room_{supportTicketId}",
                TwilioVideoGrantForCustomerToken = twilioTechToken,
                TwilioVideoGrantForTechnicianToken = twilioCustToken,
                TwilioRoomSid = twilioRoomSid
            };

            await domainModelTransaction.AddAsync<SupportTicket>(supportTicket);
            await domainModelTransaction.SaveChangesAsync();

            return supportTicket;
        }

        public async Task CreateDemoSupportTicketsWhereCallHasEnded(int numberOfSupportTicketsToCreate)
        {
            for (int i = 0; i < numberOfSupportTicketsToCreate; i++)
            {
                var supportTicket = new SupportTicket
                {
                    CustomerName = $"Customer {i}",
                    CustomerEmail = $"Customer{i}@mail.dk",
                    CustomerPhone = "12345678",
                    SupportTicketId = Guid.NewGuid(),
                    CreatedAt = new DateTime(2021, 06, 17),
                    Brand = "Apple",
                    CallStartedAt = new DateTime(2021, 06, 23, 12, 00, 00),
                    CallEndedAt = new DateTime(2021, 06, 23, 12, 07, 00)
                };

                await domainModelTransaction.AddAsync<SupportTicket>(supportTicket);
            }
            await domainModelTransaction.SaveChangesAsync();
        }

        public async Task CreateDemoSupportTicketsWhereCallHasEnded(string supportTicketId, DateTime createdAt)
        {
            var supportTicket = new SupportTicket
            {
                CustomerName = $"Customer",
                CustomerEmail = $"Customer@mail.dk",
                CustomerPhone = "12345678",
                SupportTicketId = new Guid(supportTicketId),
                CreatedAt = createdAt,
                Brand = "Apple",
                CallStartedAt = createdAt.AddMinutes(30),
                CallEndedAt = createdAt.AddMinutes(38)
            };

            await domainModelTransaction.AddAsync<SupportTicket>(supportTicket);
            await domainModelTransaction.SaveChangesAsync();
        }

        public async Task CreateDemoSupportTicketsWhereCallHasStarted(int numberOfSupportTicketsToCreate)
        {
            for (int i = 0; i < numberOfSupportTicketsToCreate; i++)
            {
                var supportTicket = new SupportTicket()
                {
                    CustomerName = $"Customer {i}",
                    CustomerEmail = $"Customer{i}@mail.dk",
                    CustomerPhone = "12345678",
                    SupportTicketId = Guid.NewGuid(),
                    CreatedAt = new DateTime(2021, 06, 17),
                    Brand = "Apple",
                    CallStartedAt = new DateTime(2021, 06, 23, 12, 00, 00),
                    CallEndedAt = null,
                    TechnicianUserId = "e0169f6f-c521-4d75-9144-a46c692af355",
                };

                await domainModelTransaction.AddAsync<SupportTicket>(supportTicket);
            }

            await domainModelTransaction.SaveChangesAsync();
        }

        public async Task<SupportTicket> CreateDemoSupportTicketsWhereCallHasStarted()
        {
            var supportTicket = new SupportTicket()
            {
                CustomerName = $"Customer",
                CustomerEmail = $"Customer@mail.dk",
                CustomerPhone = "12345678",
                SupportTicketId = Guid.NewGuid(),
                CreatedAt = new DateTime(2021, 06, 17),
                Brand = "Apple",
                CallStartedAt = new DateTime(2021, 06, 23, 12, 00, 00),
                CallEndedAt = null,
                TwilioRoomName = "TheGreenRoom",
                TwilioVideoGrantForCustomerToken = "123456",
                TwilioVideoGrantForTechnicianToken = "890123",
                TechnicianUserId = "e0169f6f-c521-4d75-9144-a46c692af355",
            };

            await domainModelTransaction.AddAsync<SupportTicket>(supportTicket);
            await domainModelTransaction.SaveChangesAsync();

            return supportTicket;
        }
    }
}
