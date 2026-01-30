using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using System.Text.Json;

namespace Application.Projects.Commands.CreateProject;

/// <summary>
/// Handler for creating a new project
/// </summary>
public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateProjectCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
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

        // Create the project entity
        var project = new Project
        {
            Title = request.Title,
            ShortDescription = request.ShortDescription,
            FullDescription = request.FullDescription,
            Technologies = JsonSerializer.Serialize(request.Technologies),
            GitHubUrl = request.GitHubUrl,
            LiveDemoUrl = request.LiveDemoUrl,
            DisplayOrder = request.DisplayOrder,
            Status = request.Status,
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow
        };

        _context.Projects.Add(project);
        await _context.SaveChangesAsync(cancellationToken);

        return project.Id;
    }
}
