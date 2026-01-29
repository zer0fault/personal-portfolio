using Application.Projects.Queries.DTOs;

namespace BlazorApp.Services;

public interface IProjectsService
{
    Task<List<ProjectDto>> GetAllProjectsAsync();
    Task<ProjectDetailDto?> GetProjectByIdAsync(int id);
}
