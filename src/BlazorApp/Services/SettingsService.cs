using Application.Settings.Commands.CreateSetting;
using Application.Settings.Commands.UpdateSetting;
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

    // Admin methods
    public async Task<SettingsDto?> GetSettingByIdAsync(int id)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<SettingsDto>($"api/settings/{id}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching setting {id}: {ex.Message}");
            return null;
        }
    }

    public async Task<int> CreateSettingAsync(CreateSettingCommand command)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/settings", command);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<CreateSettingResponse>();
                return result?.Id ?? 0;
            }
            return 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating setting: {ex.Message}");
            return 0;
        }
    }

    public async Task<bool> UpdateSettingAsync(int id, UpdateSettingCommand command)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"api/settings/{id}", command);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating setting {id}: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> DeleteSettingAsync(int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"api/settings/{id}");
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting setting {id}: {ex.Message}");
            return false;
        }
    }

    private class CreateSettingResponse
    {
        public int Id { get; set; }
    }
}
