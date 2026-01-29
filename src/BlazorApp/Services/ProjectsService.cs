using Application.Projects.Queries.DTOs;
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
}
