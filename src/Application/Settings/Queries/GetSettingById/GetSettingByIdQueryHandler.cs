using Application.Common.Data;
using Application.Settings.Queries.DTOs;
using AutoMapper;
using MediatR;

namespace Application.Settings.Queries.GetSettingById;

/// <summary>
/// Handler for GetSettingByIdQuery
/// </summary>
public class GetSettingByIdQueryHandler : IRequestHandler<GetSettingByIdQuery, SettingsDto>
{
    private readonly IMapper _mapper;

    public GetSettingByIdQueryHandler(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<SettingsDto> Handle(GetSettingByIdQuery request, CancellationToken cancellationToken)
    {
        var setting = StaticDataProvider.GetSettings()
            .FirstOrDefault(s => !s.IsDeleted && s.Id == request.Id);

        if (setting == null)
        {
            return await Task.FromResult<SettingsDto>(null!);
        }

        return await Task.FromResult(_mapper.Map<SettingsDto>(setting));
    }
}
