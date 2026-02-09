using Application.Common.Data;
using Application.Projects.Queries.DTOs;
using Domain.Enums;
using MediatR;

namespace Application.Projects.Queries.GetAllProjects;

/// <summary>
/// Handler for GetAllProjectsQuery
/// </summary>
public class GetAllProjectsQueryHandler : IRequestHandler<GetAllProjectsQuery, List<ProjectDto>>
{
    public async Task<List<ProjectDto>> Handle(GetAllProjectsQuery request, CancellationToken cancellationToken)
    {
        var projects = StaticDataProvider.GetProjectsData();
        var dtos = new List<ProjectDto>();
        var currentId = 1;

        foreach (var project in projects)
        {
            dtos.Add(new ProjectDto
            {
                Id = currentId,
                Title = project.Title,
                ShortDescription = project.ShortDescription,
                Technologies = project.Technologies,
                GitHubUrl = project.GitHubUrl,
                LiveDemoUrl = project.LiveDemoUrl,
                Status = ProjectStatus.Published,
                DisplayOrder = currentId++,
                ThumbnailPath = string.Empty
            });
        }

        return await Task.FromResult(dtos);
    }
}
