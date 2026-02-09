using Application.Common.Data;
using Application.Settings.Queries.DTOs;
using MediatR;

namespace Application.Settings.Queries.GetAllSettings;

/// <summary>
/// Handler for GetAllSettingsQuery
/// </summary>
public class GetAllSettingsQueryHandler : IRequestHandler<GetAllSettingsQuery, List<SettingsDto>>
{
    public async Task<List<SettingsDto>> Handle(GetAllSettingsQuery request, CancellationToken cancellationToken)
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

        return await Task.FromResult(settings);
    }
}
