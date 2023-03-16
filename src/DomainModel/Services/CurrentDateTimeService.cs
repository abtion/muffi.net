using System;
using System.Diagnostics.CodeAnalysis;

namespace DomainModel.Services;

[ExcludeFromCodeCoverage]
public class CurrentDateTimeService : ICurrentDateTimeService
{
    public DateTime CurrentDateTime()
    {
        return DateTime.UtcNow;
    }
}