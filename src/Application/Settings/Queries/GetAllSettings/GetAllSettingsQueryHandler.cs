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
        return await Task.FromResult(StaticDataProvider.GetAllSettingsDtos());
    }
}
