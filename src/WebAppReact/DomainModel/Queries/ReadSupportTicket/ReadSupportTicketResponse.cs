using System;
using System.Collections.Generic;

namespace WebAppReact.DomainModel.Queries.ReadSupportTicket
{
    public class ReadSupportTicketResponse
    {
        public IList<SupportTicketRecord> SupportTickets { get; set; }

        public record SupportTicketRecord(
            string CustomerName,
            string CustomerEmail,
            string CustomerPhone,
            Guid SupportTicketId,
            DateTime CreatedAt,
            string Brand,
            DateTime? CallStartedAt,
            DateTime? CallEndedAt,
            string TechnicianUserId,
            string TechnicianFullName
        );
    }
}