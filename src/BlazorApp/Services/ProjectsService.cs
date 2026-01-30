using Application.Projects.Queries.DTOs;
using Application.Projects.Commands.CreateProject;
using Application.Projects.Commands.UpdateProject;
using System.Net.Http.Json;

namespace BlazorApp.Services;

public class ProjectsService : IProjectsService
{
    private readonly HttpClient _httpClient;

    public ProjectsService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<ProjectDto>> GetAllProjectsAsync()
    {
        try
        {
            var projects = await _httpClient.GetFromJsonAsync<List<ProjectDto>>("api/projects");
            return projects ?? new List<ProjectDto>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching projects: {ex.Message}");
            return new List<ProjectDto>();
        }
    }

    public async Task<List<ProjectDto>> GetAllProjectsForAdminAsync()
    {
        try
        {
            var projects = await _httpClient.GetFromJsonAsync<List<ProjectDto>>("api/projects-admin");
            return projects ?? new List<ProjectDto>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching projects for admin: {ex.Message}");
            return new List<ProjectDto>();
        }
    }

    public async Task<ProjectDetailDto?> GetProjectByIdAsync(int id)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<ProjectDetailDto>($"api/projects/{id}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching project {id}: {ex.Message}");
            return null;
        }
    }

    public async Task<ProjectDetailDto?> GetProjectByIdForAdminAsync(int id)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<ProjectDetailDto>($"api/projects-admin/{id}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching project {id} for admin: {ex.Message}");
            return null;
        }
    }

    public async Task<int?> CreateProjectAsync(CreateProjectCommand command)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/projects", command);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<CreateProjectResponse>();
                return result?.Id;
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Error creating project: {response.StatusCode} - {errorContent}");
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating project: {ex.Message}");
            return null;
        }
    }

    public async Task<bool> UpdateProjectAsync(UpdateProjectCommand command)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"api/projects/{command.Id}", command);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Error updating project: {response.StatusCode} - {errorContent}");
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating project: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> DeleteProjectAsync(int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"api/projects/{id}");
            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Error deleting project: {response.StatusCode} - {errorContent}");
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting project: {ex.Message}");
            return false;
        }
    }

    private class CreateProjectResponse
    {
        public int Id { get; set; }
    }
}
