using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Application.Projects.Commands.UpdateProject;

/// <summary>
/// Handler for updating an existing project
/// </summary>
public class UpdateProjectCommandHandler : IRequestHandler<UpdateProjectCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public UpdateProjectCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
    {
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

        // Find the project
        var project = await _context.Projects
            .FirstOrDefaultAsync(p => p.Id == request.Id && !p.IsDeleted, cancellationToken);

        if (project == null)
        {
            throw new KeyNotFoundException("Project not found");
        }

        // Update the project
        project.Title = request.Title;
        project.ShortDescription = request.ShortDescription;
        project.FullDescription = request.FullDescription;
        project.Technologies = JsonSerializer.Serialize(request.Technologies);
        project.GitHubUrl = request.GitHubUrl;
        project.LiveDemoUrl = request.LiveDemoUrl;
        project.DisplayOrder = request.DisplayOrder;
        project.Status = request.Status;
        project.ModifiedDate = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
