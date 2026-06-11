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
        var setting = StaticDataProvider.GetAllSettingsDtos().FirstOrDefault(s => s.Id == request.Id);
        return await Task.FromResult(setting!);
    }
}
