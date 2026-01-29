using Application.Settings.Queries.DTOs;
using System.Net.Http.Json;

namespace BlazorApp.Services;

public class SettingsService : ISettingsService
{
    private readonly HttpClient _httpClient;

    public SettingsService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<SettingsDto>> GetAllSettingsAsync()
    {
        try
        {
            var settings = await _httpClient.GetFromJsonAsync<List<SettingsDto>>("api/settings");
            return settings ?? new List<SettingsDto>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching settings: {ex.Message}");
            return new List<SettingsDto>();
        }
    }

    public async Task<List<SettingsDto>> GetSettingsByCategoryAsync(string category)
    {
        try
        {
            var settings = await _httpClient.GetFromJsonAsync<List<SettingsDto>>($"api/settings/category/{category}");
            return settings ?? new List<SettingsDto>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching settings for category {category}: {ex.Message}");
            return new List<SettingsDto>();
        }
    }
}
