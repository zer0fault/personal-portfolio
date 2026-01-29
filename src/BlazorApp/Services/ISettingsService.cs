using Application.Settings.Queries.DTOs;

namespace BlazorApp.Services;

public interface ISettingsService
{
    Task<List<SettingsDto>> GetAllSettingsAsync();
    Task<List<SettingsDto>> GetSettingsByCategoryAsync(string category);
}
