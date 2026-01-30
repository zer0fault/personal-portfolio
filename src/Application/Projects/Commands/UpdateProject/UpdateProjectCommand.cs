using Domain.Enums;
using MediatR;

namespace Application.Projects.Commands.UpdateProject;

/// <summary>
/// Command to update an existing project
/// </summary>
public class UpdateProjectCommand : IRequest<bool>
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string ShortDescription { get; set; } = string.Empty;
    public string FullDescription { get; set; } = string.Empty;
    public List<string> Technologies { get; set; } = new();
    public string? GitHubUrl { get; set; }
    public string? LiveDemoUrl { get; set; }
    public int DisplayOrder { get; set; }
    public ProjectStatus Status { get; set; }
}
