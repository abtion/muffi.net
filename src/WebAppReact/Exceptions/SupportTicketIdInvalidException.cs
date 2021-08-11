using System;

namespace WebAppReact.Exceptions
{
    public class SupportTicketIdInvalidException : Exception
    {
        public SupportTicketIdInvalidException() : base("SupportTicketId format is invalid")
        {
            // skip
        }
    }
}