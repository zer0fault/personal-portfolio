using Application.Common.Data;
using Application.Settings.Queries.DTOs;

namespace BlazorApp.Services.Static;

public class StaticSettingsService : ISettingsService
{
    public Task<List<SettingsDto>> GetAllSettingsAsync()
    {
        return Task.FromResult(StaticDataProvider.GetAllSettingsDtos());
    }

    public Task<List<SettingsDto>> GetSettingsByCategoryAsync(string category)
    {
        var filtered = StaticDataProvider.GetAllSettingsDtos()
            .Where(s => s.Category == category)
            .ToList();
        return Task.FromResult(filtered);
    }

    public Task<SettingsDto?> GetSettingByIdAsync(int id)
    {
        var setting = StaticDataProvider.GetAllSettingsDtos().FirstOrDefault(s => s.Id == id);
        return Task.FromResult(setting);
    }

    // Admin methods - not supported in static mode
    public Task<int> CreateSettingAsync(Application.Settings.Commands.CreateSetting.CreateSettingCommand command)
    {
        Console.WriteLine("Create operations are not supported in static mode");
        return Task.FromResult(0);
    }

    public Task<bool> UpdateSettingAsync(int id, Application.Settings.Commands.UpdateSetting.UpdateSettingCommand command)
    {
        Console.WriteLine("Update operations are not supported in static mode");
        return Task.FromResult(false);
    }

    public Task<bool> DeleteSettingAsync(int id)
    {
        Console.WriteLine("Delete operations are not supported in static mode");
        return Task.FromResult(false);
    }
}
