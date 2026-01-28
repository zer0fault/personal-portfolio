using Application.Common.Interfaces;

namespace Infrastructure.Services;

/// <summary>
/// Service for DateTime operations (enables testability)
/// </summary>
public class DateTimeService : IDateTime
{
    public DateTime UtcNow => DateTime.UtcNow;
}
