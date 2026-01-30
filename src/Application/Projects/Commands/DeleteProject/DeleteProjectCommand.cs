using MediatR;

namespace Application.Projects.Commands.DeleteProject;

/// <summary>
/// Command to soft delete a project
/// </summary>
public class DeleteProjectCommand : IRequest<bool>
{
    public int Id { get; set; }
}
