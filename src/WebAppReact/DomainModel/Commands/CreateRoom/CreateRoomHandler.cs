using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using WebAppReact.Exceptions;
using WebAppReact.Hubs;
using WebAppReact.Models;
using WebAppReact.Services;

namespace WebAppReact.DomainModel.Commands.CreateRoom
{
    public class CreateRoomHandler
    {
        private readonly ITwilioService twilioService;
        private readonly DomainModelTransaction transaction;
        private readonly CustomerHub customerHub;
        private readonly TechnicianHub technicianHub;
        private readonly ICurrentDateTimeService currentDateTimeService;
        private readonly ICurrentUserService currentUserService;
        private readonly UserManager<ApplicationUser> userManager;

        public CreateRoomHandler(ITwilioService twilioService, DomainModelTransaction transaction, CustomerHub customerHub, TechnicianHub technicianHub,ICurrentDateTimeService currentDateTimeService, ICurrentUserService currentUserService, UserManager<ApplicationUser> userManager)
        {
            this.twilioService = twilioService ?? throw new ArgumentNullException(nameof(twilioService));
            this.transaction = transaction ?? throw new ArgumentNullException(nameof(transaction));
            this.customerHub = customerHub ?? throw new ArgumentNullException(nameof(customerHub));
            this.technicianHub = technicianHub ?? throw new ArgumentNullException(nameof(technicianHub));
            this.currentDateTimeService = currentDateTimeService ?? throw new ArgumentNullException(nameof(currentDateTimeService));
            this.currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        public async Task<CreateRoomResponse> Handle(CreateRoomRequest createRoomServiceRequest, CancellationToken cancellationToken)
        {
            var supportTicketId = new Guid(createRoomServiceRequest.SupportTicketId);

            var query = from st in transaction.Entities<SupportTicket>()
                        where st.SupportTicketId == supportTicketId
                        select st;

            if (!query.Any())
                throw new SupportTicketNotFoundException(supportTicketId);

            var supportTicket = query.FirstOrDefault();

            var roomName = $"room_{createRoomServiceRequest.SupportTicketId}";
            var roomSid = await twilioService.CreateRoom(roomName);

            var videoGrantForCustomerToken = twilioService.CreateVideoGrant(supportTicket.CustomerName, roomName);
            var videoGrantForTechnicianToken = twilioService.CreateVideoGrant("Care1 Technician", roomName);
            supportTicket.TwilioRoomName = roomName;
            supportTicket.TwilioRoomSid = roomSid;
            supportTicket.TwilioVideoGrantForCustomerToken = videoGrantForCustomerToken;
            supportTicket.TwilioVideoGrantForTechnicianToken = videoGrantForTechnicianToken;
            supportTicket.CallStartedAt = currentDateTimeService.CurrentDateTime();
            supportTicket.TechnicianUserId = (await currentUserService.CurrentUser()).Id;

            await transaction.SaveChangesAsync();

            var technicianFullName = "N/A";
            technicianFullName = (await userManager.FindByIdAsync(supportTicket.TechnicianUserId))?.FullName;


            await customerHub.TechnicianHasStartedCall(new TechnicianHasStartedCallMessage(createRoomServiceRequest.SupportTicketId));
            await technicianHub.SupportTicketUpdated(new SupportTicketUpdatedMessage(
                new Queries.ReadSupportTicket.ReadSupportTicketResponse.SupportTicketRecord(
                    supportTicket.CustomerName,
                    supportTicket.CustomerEmail,
                    supportTicket.CustomerPhone,
                    supportTicket.SupportTicketId,
                    supportTicket.CreatedAt,
                    supportTicket.Brand,
                    supportTicket.CallStartedAt,
                    supportTicket.CallEndedAt,
                    supportTicket.TechnicianUserId,
                    technicianFullName
                )
            ));


            return new CreateRoomResponse()
            {
                TwilioRoomName = roomName,
                TwilioVideoGrantForCustomerToken = videoGrantForCustomerToken,
                TwilioVideoGrantForTechnicianToken = videoGrantForTechnicianToken
            };
        }
    }
}
