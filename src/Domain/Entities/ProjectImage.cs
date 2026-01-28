using Domain.Common;

namespace Domain.Entities;

/// <summary>
/// Represents an image associated with a portfolio project
/// </summary>
public class ProjectImage : BaseEntity
{
    /// <summary>
    /// Foreign key to the parent project
    /// </summary>
    public int ProjectId { get; set; }

    /// <summary>
    /// Navigation property to the parent project
    /// </summary>
    public Project Project { get; set; } = null!;

    /// <summary>
    /// Relative path to the image from wwwroot (e.g., "/assets/projects/project1.webp")
    /// </summary>
    public string ImagePath { get; set; } = string.Empty;

    /// <summary>
    /// Alternative text for accessibility
    /// </summary>
    public string AltText { get; set; } = string.Empty;

    /// <summary>
    /// Display order for multiple images
    /// </summary>
    public int DisplayOrder { get; set; }

    /// <summary>
    /// Indicates if this is the thumbnail image for the project
    /// </summary>
    public bool IsThumbnail { get; set; }
}
