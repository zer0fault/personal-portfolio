using Domain.Common;

namespace Domain.Entities;

/// <summary>
/// Represents a configuration setting key-value pair
/// </summary>
public class Settings : BaseEntity
{
    /// <summary>
    /// Unique setting key (e.g., "HeroHeadline", "AboutBio")
    /// </summary>
    public string Key { get; set; } = string.Empty;

    /// <summary>
    /// Setting value
    /// </summary>
    public string Value { get; set; } = string.Empty;

    /// <summary>
    /// Category for grouping settings (e.g., "Hero", "About", "Social")
    /// </summary>
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// Date and time when the setting was last modified (UTC)
    /// </summary>
    public DateTime LastModified { get; set; }
}
