using System;

namespace MuffiNet.FrontendReact.Exceptions
{
    public class TwilioRoomNotFoundException : Exception
    {
        public TwilioRoomNotFoundException(string message) : base(message)
        {
            // skip
        }
    }
}
