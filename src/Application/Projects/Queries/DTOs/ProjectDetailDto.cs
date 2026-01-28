using Domain.Enums;

namespace Application.Projects.Queries.DTOs;

/// <summary>
/// DTO for detailed project view
/// </summary>
public class ProjectDetailDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string ShortDescription { get; set; } = string.Empty;
    public string FullDescription { get; set; } = string.Empty;
    public List<string> Technologies { get; set; } = new();
    public string? GitHubUrl { get; set; }
    public string? LiveDemoUrl { get; set; }
    public ProjectStatus Status { get; set; }
    public List<ProjectImageDto> Images { get; set; } = new();
}

/// <summary>
/// DTO for project images
/// </summary>
public class ProjectImageDto
{
    public int Id { get; set; }
    public string ImagePath { get; set; } = string.Empty;
    public string AltText { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
    public bool IsThumbnail { get; set; }
}
