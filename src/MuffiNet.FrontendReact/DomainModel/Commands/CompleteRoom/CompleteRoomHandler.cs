using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MuffiNet.FrontendReact.Exceptions;
using MuffiNet.FrontendReact.Hubs;
using MuffiNet.FrontendReact.Models;
using MuffiNet.FrontendReact.Services;

namespace MuffiNet.FrontendReact.DomainModel.Commands.CompleteRoom
{
    public class CompleteRoomHandler : IRequestHandler<CompleteRoomRequest, CompleteRoomResponse>
    {
        private readonly DomainModelTransaction transaction;
        private readonly ICurrentDateTimeService currentDateTimeService;
        private readonly CustomerHub customerHub;
        private readonly TechnicianHub technicianHub;
        private readonly ITwilioService twilioService;

        public CompleteRoomHandler(DomainModelTransaction transaction, ICurrentDateTimeService currentDateTimeService, CustomerHub customerHub, TechnicianHub technicianHub, ITwilioService twilioService)
        {
            this.transaction = transaction ?? throw new ArgumentNullException(nameof(transaction));
            this.currentDateTimeService = currentDateTimeService ?? throw new ArgumentNullException(nameof(currentDateTimeService));
            this.customerHub = customerHub ?? throw new ArgumentNullException(nameof(customerHub));
            this.technicianHub = technicianHub ?? throw new ArgumentNullException(nameof(technicianHub));
            this.twilioService = twilioService ?? throw new ArgumentNullException(nameof(twilioService));
        }

        public async Task<CompleteRoomResponse> Handle(CompleteRoomRequest request, CancellationToken cancellationToken)
        {
            // find support ticket -> throw exception if not found
            Guid supportTicketId;
            try
            {
                supportTicketId = new Guid(request.SupportTicketId);
            }
            catch (Exception)
            {
                throw new SupportTicketIdInvalidException();
            }

            var supportTicket = transaction.Entities<SupportTicket>().WithSupportTicketId(new Guid(request.SupportTicketId)).SingleOrDefault();

            if (supportTicket == null)
                throw new SupportTicketNotFoundException(supportTicketId);

            // update CallEndedAt
            supportTicket.CallEndedAt = currentDateTimeService.CurrentDateTime();
            await transaction.SaveChangesAsync();

            // send messages via SignalR to other technicians + customer
            await customerHub.TechnicianHasEndedCall(new TechnicianHasEndedCallMessage(request.SupportTicketId));
            await technicianHub.SupportTicketDeleted(new SupportTicketDeletedMessage(request.SupportTicketId));

            // call Twilio to complete room (if room is already completed it might ran out of time)
            var roomAlreadyCompleted = await twilioService.IsRoomCompleted(supportTicket.TwilioRoomSid);
            if (!roomAlreadyCompleted)
            {
                await twilioService.CompleteRoom(supportTicket.TwilioRoomName);
            }

            return new CompleteRoomResponse();
        }
    }
}
