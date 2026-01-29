using Application.Common.Interfaces;
using Application.Settings.Queries.DTOs;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Settings.Queries.GetSettingsByCategory;

/// <summary>
/// Handler for GetSettingsByCategoryQuery
/// </summary>
public class GetSettingsByCategoryQueryHandler : IRequestHandler<GetSettingsByCategoryQuery, List<SettingsDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetSettingsByCategoryQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<SettingsDto>> Handle(GetSettingsByCategoryQuery request, CancellationToken cancellationToken)
    {
        return await _context.Settings
            .Where(s => !s.IsDeleted && s.Category == request.Category)
            .OrderBy(s => s.Key)
            .ProjectTo<SettingsDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}
