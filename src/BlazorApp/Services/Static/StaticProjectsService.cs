using Application.Common.Data;
using Application.Projects.Queries.DTOs;
using AutoMapper;
using Domain.Enums;

namespace BlazorApp.Services.Static;

public class StaticProjectsService : IProjectsService
{
    private readonly IMapper _mapper;

    public StaticProjectsService(IMapper mapper)
    {
        _mapper = mapper;
    }

    public Task<List<ProjectDto>> GetAllProjectsAsync()
    {
        var projects = StaticDataProvider.GetProjects()
            .Where(p => p.Status == ProjectStatus.Published && !p.IsDeleted)
            .OrderBy(p => p.DisplayOrder)
            .ToList();

        return Task.FromResult(_mapper.Map<List<ProjectDto>>(projects));
    }

    public Task<List<ProjectDto>> GetAllProjectsForAdminAsync()
    {
        return GetAllProjectsAsync();
    }

    public Task<ProjectDetailDto?> GetProjectByIdAsync(int id)
    {
        var project = StaticDataProvider.GetProjects()
            .FirstOrDefault(p => p.Id == id && p.Status == ProjectStatus.Published && !p.IsDeleted);

        if (project == null)
            return Task.FromResult<ProjectDetailDto?>(null);

        return Task.FromResult<ProjectDetailDto?>(_mapper.Map<ProjectDetailDto>(project));
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
