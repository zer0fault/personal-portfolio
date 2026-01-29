using Application.Common.Interfaces;
using Application.Settings.Queries.DTOs;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Settings.Queries.GetAllSettings;

/// <summary>
/// Handler for GetAllSettingsQuery
/// </summary>
public class GetAllSettingsQueryHandler : IRequestHandler<GetAllSettingsQuery, List<SettingsDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllSettingsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<SettingsDto>> Handle(GetAllSettingsQuery request, CancellationToken cancellationToken)
    {
        return await _context.Settings
            .Where(s => !s.IsDeleted)
            .OrderBy(s => s.Category)
            .ThenBy(s => s.Key)
            .ProjectTo<SettingsDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}
