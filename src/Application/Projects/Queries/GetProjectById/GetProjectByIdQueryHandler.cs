using Application.Common.Data;
using Application.Projects.Queries.DTOs;
using Domain.Enums;
using MediatR;

namespace Application.Projects.Queries.GetProjectById;

/// <summary>
/// Handler for GetProjectByIdQuery
/// </summary>
public class GetProjectByIdQueryHandler : IRequestHandler<GetProjectByIdQuery, ProjectDetailDto?>
{
    public async Task<ProjectDetailDto?> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
    {
        var projects = StaticDataProvider.GetProjectsData();

        if (request.Id < 1 || request.Id > projects.Count)
            return await Task.FromResult<ProjectDetailDto?>(null);

        var project = projects[request.Id - 1];

        var dto = new ProjectDetailDto
        {
            Id = request.Id,
            Title = project.Title,
            ShortDescription = project.ShortDescription,
            FullDescription = project.FullDescription,
            Technologies = project.Technologies,
            GitHubUrl = project.GitHubUrl,
            LiveDemoUrl = project.LiveDemoUrl,
            Status = ProjectStatus.Published,
            Images = new()
        };

        return await Task.FromResult<ProjectDetailDto?>(dto);
    }
}
