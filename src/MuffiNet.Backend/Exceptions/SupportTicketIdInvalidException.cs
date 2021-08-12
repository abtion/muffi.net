using System;

namespace MuffiNet.Backend.Exceptions
{
    public class SupportTicketIdInvalidException : Exception
    {
        public SupportTicketIdInvalidException() : base("SupportTicketId format is invalid")
        {
            // skip
        }
    }
}