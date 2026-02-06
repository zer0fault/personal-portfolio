using MediatR;

namespace Application.Projects.Commands.CreateProject;

/// <summary>
/// Handler for creating a new project
/// </summary>
public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, int>
{
    public async Task<int> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
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

        // Return fake ID for API compatibility
        await Task.CompletedTask;
        return 999;
    }
}
