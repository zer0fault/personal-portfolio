using Application.Common.Interfaces;
using Application.Skills.Queries.DTOs;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Skills.Queries.GetSkillByIdForAdmin;

/// <summary>
/// Handler for GetSkillByIdForAdminQuery
/// </summary>
public class GetSkillByIdForAdminQueryHandler : IRequestHandler<GetSkillByIdForAdminQuery, SkillDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetSkillByIdForAdminQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<SkillDto?> Handle(GetSkillByIdForAdminQuery request, CancellationToken cancellationToken)
    {
        var skill = await _context.Skills
            .Where(s => s.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);

        return skill == null ? null : _mapper.Map<SkillDto>(skill);
    }
}
