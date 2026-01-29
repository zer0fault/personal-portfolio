using Application.Skills.Queries.DTOs;
using System.Net.Http.Json;

namespace BlazorApp.Services;

public class SkillsService : ISkillsService
{
    private readonly HttpClient _httpClient;

    public SkillsService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<SkillDto>> GetAllSkillsAsync()
    {
        try
        {
            var skills = await _httpClient.GetFromJsonAsync<List<SkillDto>>("api/skills");
            return skills ?? new List<SkillDto>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching skills: {ex.Message}");
            return new List<SkillDto>();
        }
    }
}
