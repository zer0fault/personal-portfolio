using Application.Common.Data;
using Application.Settings.Queries.DTOs;

namespace BlazorApp.Services.Static;

public class StaticSettingsService : ISettingsService
{
    public Task<List<SettingsDto>> GetAllSettingsAsync()
    {
        var settings = StaticDataProvider.GetSettings()
            .Where(s => !s.IsDeleted)
            .Select(s => new SettingsDto
            {
                Id = s.Id,
                Key = s.Key,
                Value = s.Value,
                Category = s.Category
            })
            .ToList();

        return Task.FromResult(settings);
    }

    public Task<List<SettingsDto>> GetSettingsByCategoryAsync(string category)
    {
        var settings = StaticDataProvider.GetSettings()
            .Where(s => !s.IsDeleted && s.Category == category)
            .Select(s => new SettingsDto
            {
                Id = s.Id,
                Key = s.Key,
                Value = s.Value,
                Category = s.Category
            })
            .ToList();

        return Task.FromResult(settings);
    }

    public Task<SettingsDto?> GetSettingByIdAsync(int id)
    {
        var setting = StaticDataProvider.GetSettings()
            .FirstOrDefault(s => s.Id == id && !s.IsDeleted);

        if (setting == null)
            return Task.FromResult<SettingsDto?>(null);

        var dto = new SettingsDto
        {
            Id = setting.Id,
            Key = setting.Key,
            Value = setting.Value,
            Category = setting.Category
        };

        return Task.FromResult<SettingsDto?>(dto);
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
