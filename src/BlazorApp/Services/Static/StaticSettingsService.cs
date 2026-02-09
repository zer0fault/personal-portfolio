using Application.Common.Data;
using Application.Settings.Queries.DTOs;

namespace BlazorApp.Services.Static;

public class StaticSettingsService : ISettingsService
{
    public Task<List<SettingsDto>> GetAllSettingsAsync()
    {
        var settings = new List<SettingsDto>();
        var currentId = 1;

        foreach (var (key, value) in StaticDataProvider.GetHeroSettings())
        {
            settings.Add(new SettingsDto
            {
                Id = currentId++,
                Key = key,
                Value = value,
                Category = "Hero",
                LastModified = DateTime.UtcNow
            });
        }

        foreach (var (key, value) in StaticDataProvider.GetAboutSettings())
        {
            settings.Add(new SettingsDto
            {
                Id = currentId++,
                Key = key,
                Value = value,
                Category = "About",
                LastModified = DateTime.UtcNow
            });
        }

        return Task.FromResult(settings);
    }

    public Task<List<SettingsDto>> GetSettingsByCategoryAsync(string category)
    {
        var allSettings = GetAllSettingsAsync().Result;
        var filtered = allSettings.Where(s => s.Category == category).ToList();
        return Task.FromResult(filtered);
    }

    public Task<SettingsDto?> GetSettingByIdAsync(int id)
    {
        var allSettings = GetAllSettingsAsync().Result;
        var setting = allSettings.FirstOrDefault(s => s.Id == id);
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
