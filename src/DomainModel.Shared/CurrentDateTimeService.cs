using System.Diagnostics.CodeAnalysis;

namespace Domain.Shared;

[ExcludeFromCodeCoverage]
public class CurrentDateTimeService : ICurrentDateTimeService
{
    public DateTime CurrentDateTime()
    {
        return DateTime.UtcNow;
    }
}
