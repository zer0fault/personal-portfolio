using Application.Common.Data;
using Application.Settings.Queries.DTOs;
using AutoMapper;
using MediatR;

namespace Application.Settings.Queries.GetSettingsByCategory;

/// <summary>
/// Handler for GetSettingsByCategoryQuery
/// </summary>
public class GetSettingsByCategoryQueryHandler : IRequestHandler<GetSettingsByCategoryQuery, List<SettingsDto>>
{
    private readonly IMapper _mapper;

    public GetSettingsByCategoryQueryHandler(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<List<SettingsDto>> Handle(GetSettingsByCategoryQuery request, CancellationToken cancellationToken)
    {
        var settings = StaticDataProvider.GetSettings()
            .Where(s => !s.IsDeleted && s.Category == request.Category)
            .OrderBy(s => s.Key)
            .ToList();

        return await Task.FromResult(_mapper.Map<List<SettingsDto>>(settings));
    }
}
