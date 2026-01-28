using Domain.Enums;

namespace Application.Projects.Queries.DTOs;

/// <summary>
/// DTO for project list view
/// </summary>
public class ProjectDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string ShortDescription { get; set; } = string.Empty;
    public List<string> Technologies { get; set; } = new();
    public string? GitHubUrl { get; set; }
    public string? LiveDemoUrl { get; set; }
    public string ThumbnailPath { get; set; } = string.Empty;
    public ProjectStatus Status { get; set; }
    public int DisplayOrder { get; set; }
}
