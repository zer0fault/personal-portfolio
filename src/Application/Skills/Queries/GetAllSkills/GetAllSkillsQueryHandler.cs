using Application.Common.Data;
using Application.Skills.Queries.DTOs;
using AutoMapper;
using MediatR;

namespace Application.Skills.Queries.GetAllSkills;

/// <summary>
/// Handler for GetAllSkillsQuery
/// </summary>
public class GetAllSkillsQueryHandler : IRequestHandler<GetAllSkillsQuery, List<SkillDto>>
{
    private readonly IMapper _mapper;

    public GetAllSkillsQueryHandler(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<List<SkillDto>> Handle(GetAllSkillsQuery request, CancellationToken cancellationToken)
    {
        var skills = StaticDataProvider.GetSkills()
            .OrderBy(s => s.Category)
            .ThenBy(s => s.DisplayOrder)
            .ToList();

        return await Task.FromResult(_mapper.Map<List<SkillDto>>(skills));
    }
}
