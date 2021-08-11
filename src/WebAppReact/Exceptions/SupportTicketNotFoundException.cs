using System;

namespace WebAppReact.Exceptions
{
    public class SupportTicketNotFoundException : Exception
    {
        public SupportTicketNotFoundException(Guid supportTicketId) : base($"Support Ticket with ID {supportTicketId} was not found")
        {
            // skip
        }
    }
}