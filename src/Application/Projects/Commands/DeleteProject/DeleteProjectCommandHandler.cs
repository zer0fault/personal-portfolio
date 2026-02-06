using MediatR;

namespace Application.Projects.Commands.DeleteProject;

/// <summary>
/// Handler for soft deleting a project
/// </summary>
public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand, bool>
{
    public async Task<bool> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
    {
        // Note: Data is hardcoded - this command does not persist changes

        // Return success for API compatibility
        await Task.CompletedTask;
        return true;
    }
}
