using Application.Common.Data;
using Application.Skills.Queries.DTOs;
using AutoMapper;
using MediatR;

namespace Application.Skills.Queries.GetSkillByIdForAdmin;

/// <summary>
/// Handler for GetSkillByIdForAdminQuery
/// </summary>
public class GetSkillByIdForAdminQueryHandler : IRequestHandler<GetSkillByIdForAdminQuery, SkillDto?>
{
    private readonly IMapper _mapper;

    public GetSkillByIdForAdminQueryHandler(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<SkillDto?> Handle(GetSkillByIdForAdminQuery request, CancellationToken cancellationToken)
    {
        var skill = StaticDataProvider.GetSkills()
            .FirstOrDefault(s => s.Id == request.Id);

        return await Task.FromResult(skill == null ? null : _mapper.Map<SkillDto>(skill));
    }
}
