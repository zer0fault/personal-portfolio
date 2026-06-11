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
        var settings = StaticDataProvider.GetAllSettingsDtos()
            .Where(s => s.Category == request.Category)
            .ToList();
        return await Task.FromResult(settings);
    }
}
