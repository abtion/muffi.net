using System.Diagnostics.CodeAnalysis;

namespace DomainModel.Shared;

[ExcludeFromCodeCoverage]
public class CurrentDateTimeService : ICurrentDateTimeService
{
    public DateTime CurrentDateTime()
    {
        return DateTime.UtcNow;
    }
}
