using Application.Projects.Queries.DTOs;
using Application.Projects.Commands.CreateProject;
using Application.Projects.Commands.UpdateProject;

namespace BlazorApp.Services;

public interface IProjectsService
{
    Task<List<ProjectDto>> GetAllProjectsAsync();
    Task<List<ProjectDto>> GetAllProjectsForAdminAsync();
    Task<ProjectDetailDto?> GetProjectByIdAsync(int id);
    Task<ProjectDetailDto?> GetProjectByIdForAdminAsync(int id);
    Task<int?> CreateProjectAsync(CreateProjectCommand command);
    Task<bool> UpdateProjectAsync(UpdateProjectCommand command);
    Task<bool> DeleteProjectAsync(int id);
}
