using Application.Settings.Commands.CreateSetting;
using Application.Settings.Commands.UpdateSetting;
using Application.Settings.Queries.DTOs;

namespace BlazorApp.Services;

public interface ISettingsService
{
    Task<List<SettingsDto>> GetAllSettingsAsync();
    Task<List<SettingsDto>> GetSettingsByCategoryAsync(string category);

    // Admin methods
    Task<SettingsDto?> GetSettingByIdAsync(int id);
    Task<int> CreateSettingAsync(CreateSettingCommand command);
    Task<bool> UpdateSettingAsync(int id, UpdateSettingCommand command);
    Task<bool> DeleteSettingAsync(int id);
}
