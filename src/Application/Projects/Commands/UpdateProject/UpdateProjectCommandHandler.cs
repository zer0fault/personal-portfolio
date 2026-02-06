using MediatR;

namespace Application.Projects.Commands.UpdateProject;

/// <summary>
/// Handler for updating an existing project
/// </summary>
public class UpdateProjectCommandHandler : IRequestHandler<UpdateProjectCommand, bool>
{
    public async Task<bool> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
    {
        // Note: Data is hardcoded - this command does not persist changes

        // Validate required fields
        if (string.IsNullOrWhiteSpace(request.Title))
        {
            throw new ArgumentException("Title is required");
        }

        if (string.IsNullOrWhiteSpace(request.ShortDescription))
        {
            throw new ArgumentException("Short description is required");
        }

        if (string.IsNullOrWhiteSpace(request.FullDescription))
        {
            throw new ArgumentException("Full description is required");
        }

        // Return success for API compatibility
        await Task.CompletedTask;
        return true;
    }
}
