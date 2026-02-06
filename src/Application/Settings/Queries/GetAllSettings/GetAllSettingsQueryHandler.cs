using Application.Common.Data;
using Application.Settings.Queries.DTOs;
using AutoMapper;
using MediatR;

namespace Application.Settings.Queries.GetAllSettings;

/// <summary>
/// Handler for GetAllSettingsQuery
/// </summary>
public class GetAllSettingsQueryHandler : IRequestHandler<GetAllSettingsQuery, List<SettingsDto>>
{
    private readonly IMapper _mapper;

    public GetAllSettingsQueryHandler(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<List<SettingsDto>> Handle(GetAllSettingsQuery request, CancellationToken cancellationToken)
    {
        var settings = StaticDataProvider.GetSettings()
            .Where(s => !s.IsDeleted)
            .OrderBy(s => s.Category)
            .ThenBy(s => s.Key)
            .ToList();

        return await Task.FromResult(_mapper.Map<List<SettingsDto>>(settings));
    }
}
