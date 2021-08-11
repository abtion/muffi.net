using System;

namespace WebAppReact.Exceptions
{
    public class TwilioRoomNotFoundException : Exception
    {
        public TwilioRoomNotFoundException(string message) : base(message)
        {
            // skip
        }
    }
}
