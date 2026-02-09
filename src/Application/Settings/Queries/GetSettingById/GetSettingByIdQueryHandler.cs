using Application.Common.Data;
using Application.Settings.Queries.DTOs;
using MediatR;

namespace Application.Settings.Queries.GetSettingById;

/// <summary>
/// Handler for GetSettingByIdQuery
/// </summary>
public class GetSettingByIdQueryHandler : IRequestHandler<GetSettingByIdQuery, SettingsDto>
{
    public async Task<SettingsDto> Handle(GetSettingByIdQuery request, CancellationToken cancellationToken)
    {
        var allSettings = new List<SettingsDto>();
        var currentId = 1;

        foreach (var (key, value) in StaticDataProvider.GetHeroSettings())
        {
            allSettings.Add(new SettingsDto
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
            allSettings.Add(new SettingsDto
            {
                Id = currentId++,
                Key = key,
                Value = value,
                Category = "About",
                LastModified = DateTime.UtcNow
            });
        }

        var setting = allSettings.FirstOrDefault(s => s.Id == request.Id);
        return await Task.FromResult(setting!);
    }
}
