using Application.Skills.Queries.DTOs;
using Application.Skills.Commands.CreateSkill;
using Application.Skills.Commands.UpdateSkill;
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

    public async Task<SkillDto?> GetSkillByIdAsync(int id)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<SkillDto>($"api/skills/{id}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching skill {id}: {ex.Message}");
            return null;
        }
    }

    public async Task<int?> CreateSkillAsync(CreateSkillCommand command)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/skills", command);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<CreateSkillResponse>();
                return result?.Id;
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Error creating skill: {response.StatusCode} - {errorContent}");
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating skill: {ex.Message}");
            return null;
        }
    }

    public async Task<bool> UpdateSkillAsync(UpdateSkillCommand command)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"api/skills/{command.Id}", command);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Error updating skill: {response.StatusCode} - {errorContent}");
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating skill: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> DeleteSkillAsync(int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"api/skills/{id}");
            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Error deleting skill: {response.StatusCode} - {errorContent}");
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting skill: {ex.Message}");
            return false;
        }
    }

    private class CreateSkillResponse
    {
        public int Id { get; set; }
    }
}
