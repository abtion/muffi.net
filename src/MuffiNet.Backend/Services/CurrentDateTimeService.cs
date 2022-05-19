using System;
using System.Diagnostics.CodeAnalysis;

namespace MuffiNet.Backend.Services;

public interface ICurrentDateTimeService
{
    DateTime CurrentDateTime();
}
[ExcludeFromCodeCoverage]
public class CurrentDateTimeService : ICurrentDateTimeService
{
    public DateTime CurrentDateTime()
    {
        return DateTime.UtcNow;
    }
}