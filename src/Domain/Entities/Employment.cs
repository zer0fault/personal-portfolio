using Domain.Common;

namespace Domain.Entities;

/// <summary>
/// Represents employment history entry
/// </summary>
public class Employment : BaseEntity, ISoftDeletable
{
    /// <summary>
    /// Company name
    /// </summary>
    public string CompanyName { get; set; } = string.Empty;

    /// <summary>
    /// Job title/position
    /// </summary>
    public string JobTitle { get; set; } = string.Empty;

    /// <summary>
    /// Employment start date
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// Employment end date (null if current position)
    /// </summary>
    public DateTime? EndDate { get; set; }

    /// <summary>
    /// Key responsibilities (stored as JSON array)
    /// </summary>
    public string Responsibilities { get; set; } = "[]";

    /// <summary>
    /// Notable achievements (stored as JSON array)
    /// </summary>
    public string Achievements { get; set; } = "[]";

    /// <summary>
    /// Technologies used in this role (stored as JSON array)
    /// </summary>
    public string Technologies { get; set; } = "[]";

    /// <summary>
    /// Display order for sorting employment history
    /// </summary>
    public int DisplayOrder { get; set; }

    /// <summary>
    /// Indicates whether the employment entry has been soft deleted
    /// </summary>
    public bool IsDeleted { get; set; }
}
