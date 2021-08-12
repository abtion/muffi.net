using System;

namespace MuffiNet.Backend.Exceptions
{
    public class ExampleException : Exception
    {
        public ExampleException() : base("Example exception thrown")
        {
            // skip
        }
    }
}