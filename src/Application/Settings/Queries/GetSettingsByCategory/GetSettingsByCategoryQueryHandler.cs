using Application.Common.Data;
using Application.Settings.Queries.DTOs;
using MediatR;

namespace Application.Settings.Queries.GetSettingsByCategory;

/// <summary>
/// Handler for GetSettingsByCategoryQuery
/// </summary>
public class GetSettingsByCategoryQueryHandler : IRequestHandler<GetSettingsByCategoryQuery, List<SettingsDto>>
{
    public async Task<List<SettingsDto>> Handle(GetSettingsByCategoryQuery request, CancellationToken cancellationToken)
    {
        var settings = new List<SettingsDto>();
        var currentId = 1;

        if (request.Category == "Hero")
        {
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
        }
        else if (request.Category == "About")
        {
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
        }

        return await Task.FromResult(settings);
    }
}
