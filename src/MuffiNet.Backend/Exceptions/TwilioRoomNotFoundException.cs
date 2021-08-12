using System;

namespace MuffiNet.Backend.Exceptions
{
    public class TwilioRoomNotFoundException : Exception
    {
        public TwilioRoomNotFoundException(string message) : base(message)
        {
            // skip
        }
    }
}
