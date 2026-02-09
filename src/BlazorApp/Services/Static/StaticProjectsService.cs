using Application.Common.Data;
using Application.Projects.Queries.DTOs;
using Domain.Enums;

namespace BlazorApp.Services.Static;

public class StaticProjectsService : IProjectsService
{
    public Task<List<ProjectDto>> GetAllProjectsAsync()
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

        return Task.FromResult(dtos);
    }

    public Task<List<ProjectDto>> GetAllProjectsForAdminAsync()
    {
        return GetAllProjectsAsync();
    }

    public Task<ProjectDetailDto?> GetProjectByIdAsync(int id)
    {
        var projects = StaticDataProvider.GetProjectsData();

        if (id < 1 || id > projects.Count)
            return Task.FromResult<ProjectDetailDto?>(null);

        var project = projects[id - 1];

        var dto = new ProjectDetailDto
        {
            Id = id,
            Title = project.Title,
            ShortDescription = project.ShortDescription,
            FullDescription = project.FullDescription,
            Technologies = project.Technologies,
            GitHubUrl = project.GitHubUrl,
            LiveDemoUrl = project.LiveDemoUrl,
            Status = ProjectStatus.Published,
            Images = new()
        };

        return Task.FromResult<ProjectDetailDto?>(dto);
    }

    public Task<ProjectDetailDto?> GetProjectByIdForAdminAsync(int id)
    {
        return GetProjectByIdAsync(id);
    }

    // Admin methods - not supported in static mode
    public Task<int?> CreateProjectAsync(Application.Projects.Commands.CreateProject.CreateProjectCommand command)
    {
        Console.WriteLine("Create operations are not supported in static mode");
        return Task.FromResult<int?>(null);
    }

    public Task<bool> UpdateProjectAsync(Application.Projects.Commands.UpdateProject.UpdateProjectCommand command)
    {
        Console.WriteLine("Update operations are not supported in static mode");
        return Task.FromResult(false);
    }

    public Task<bool> DeleteProjectAsync(int id)
    {
        Console.WriteLine("Delete operations are not supported in static mode");
        return Task.FromResult(false);
    }
}
