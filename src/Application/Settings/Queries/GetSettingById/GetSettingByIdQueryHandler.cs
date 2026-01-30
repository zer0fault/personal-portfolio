using Application.Common.Interfaces;
using Application.Settings.Queries.DTOs;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Settings.Queries.GetSettingById;

/// <summary>
/// Handler for GetSettingByIdQuery
/// </summary>
public class GetSettingByIdQueryHandler : IRequestHandler<GetSettingByIdQuery, SettingsDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetSettingByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<SettingsDto> Handle(GetSettingByIdQuery request, CancellationToken cancellationToken)
    {
        var setting = await _context.Settings
            .Where(s => !s.IsDeleted)
            .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);

        if (setting == null)
        {
            throw new KeyNotFoundException($"Setting with ID {request.Id} not found");
        }

        return _mapper.Map<SettingsDto>(setting);
    }
}
