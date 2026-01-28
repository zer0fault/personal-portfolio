namespace Application.Common.Interfaces;

/// <summary>
/// Interface for DateTime abstraction (for testability)
/// </summary>
public interface IDateTime
{
    /// <summary>
    /// Gets the current UTC date and time
    /// </summary>
    DateTime UtcNow { get; }
}
