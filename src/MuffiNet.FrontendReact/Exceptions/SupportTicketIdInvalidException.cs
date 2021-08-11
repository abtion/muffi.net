using System;

namespace MuffiNet.FrontendReact.Exceptions
{
    public class SupportTicketIdInvalidException : Exception
    {
        public SupportTicketIdInvalidException() : base("SupportTicketId format is invalid")
        {
            // skip
        }
    }
}