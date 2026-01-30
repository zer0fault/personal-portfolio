using Domain.Enums;
using MediatR;

namespace Application.Projects.Commands.CreateProject;

/// <summary>
/// Command to create a new project
/// </summary>
public class CreateProjectCommand : IRequest<int>
{
    public string Title { get; set; } = string.Empty;
    public string ShortDescription { get; set; } = string.Empty;
    public string FullDescription { get; set; } = string.Empty;
    public List<string> Technologies { get; set; } = new();
    public string? GitHubUrl { get; set; }
    public string? LiveDemoUrl { get; set; }
    public int DisplayOrder { get; set; }
    public ProjectStatus Status { get; set; } = ProjectStatus.Draft;
}
