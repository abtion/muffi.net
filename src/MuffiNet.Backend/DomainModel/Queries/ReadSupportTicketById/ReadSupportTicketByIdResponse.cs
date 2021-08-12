using System;

namespace MuffiNet.Backend.DomainModel.Queries.ReadSupportTicketById
{
    public class ReadSupportTicketByIdResponse
    {
        public SupportTicketRecord SupportTicket { get; set; }

        public record SupportTicketRecord(
            string CustomerName,
            string CustomerEmail,
            string CustomerPhone,
            Guid SupportTicketId,
            DateTime CreatedAt,
            string Brand,
            string TwilioVideoGrantForCustomerToken,
            string TwilioVideoGrantForTechnicianToken,
            string OssId
        );
    }
}