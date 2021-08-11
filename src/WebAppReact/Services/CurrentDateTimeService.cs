using System;

namespace WebAppReact.Services
{
    public interface ICurrentDateTimeService
    {
        DateTime CurrentDateTime();
    }

    public class CurrentDateTimeService : ICurrentDateTimeService
    {
        public DateTime CurrentDateTime()
        {
            return DateTime.UtcNow;
        }
    }
}