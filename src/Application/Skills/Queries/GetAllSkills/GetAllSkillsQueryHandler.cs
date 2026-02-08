using Application.Common.Data;
using Application.Skills.Queries.DTOs;
using MediatR;

namespace Application.Skills.Queries.GetAllSkills;

/// <summary>
/// Handler for GetAllSkillsQuery
/// </summary>
public class GetAllSkillsQueryHandler : IRequestHandler<GetAllSkillsQuery, List<SkillDto>>
{
    public async Task<List<SkillDto>> Handle(GetAllSkillsQuery request, CancellationToken cancellationToken)
    {
        var skillsByCategory = StaticDataProvider.GetSkillsByCategory();
        var skills = new List<SkillDto>();
        var currentId = 1;

        foreach (var (category, skillNames) in skillsByCategory.OrderBy(x => x.Key))
        {
            var displayOrder = 1;
            foreach (var skillName in skillNames)
            {
                skills.Add(new SkillDto
                {
                    Id = currentId++,
                    Name = skillName,
                    Category = category,
                    DisplayOrder = displayOrder++,
                    IconUrl = null
                });
            }
        }

        return await Task.FromResult(skills);
    }
}
