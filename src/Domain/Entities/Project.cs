using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;

/// <summary>
/// Represents a portfolio project
/// </summary>
public class Project : BaseEntity, ISoftDeletable
{
    /// <summary>
    /// Project title
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Short description for project cards (max 150 characters)
    /// </summary>
    public string ShortDescription { get; set; } = string.Empty;

    /// <summary>
    /// Full detailed description of the project
    /// </summary>
    public string FullDescription { get; set; } = string.Empty;

    /// <summary>
    /// Technologies used in the project (stored as JSON array)
    /// </summary>
    public string Technologies { get; set; } = "[]";

    /// <summary>
    /// URL to GitHub repository (optional)
    /// </summary>
    public string? GitHubUrl { get; set; }

    /// <summary>
    /// URL to live demo (optional)
    /// </summary>
    public string? LiveDemoUrl { get; set; }

    /// <summary>
    /// Display order for sorting projects
    /// </summary>
    public int DisplayOrder { get; set; }

    /// <summary>
    /// Current status of the project
    /// </summary>
    public ProjectStatus Status { get; set; }

    /// <summary>
    /// Indicates whether the project has been soft deleted
    /// </summary>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// Collection of project images
    /// </summary>
    public ICollection<ProjectImage> Images { get; set; } = new List<ProjectImage>();
}
