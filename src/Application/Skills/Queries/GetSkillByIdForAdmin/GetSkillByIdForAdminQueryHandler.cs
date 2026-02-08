using Application.Common.Data;
using Application.Skills.Queries.DTOs;
using MediatR;

namespace Application.Skills.Queries.GetSkillByIdForAdmin;

/// <summary>
/// Handler for GetSkillByIdForAdminQuery
/// </summary>
public class GetSkillByIdForAdminQueryHandler : IRequestHandler<GetSkillByIdForAdminQuery, SkillDto?>
{
    public async Task<SkillDto?> Handle(GetSkillByIdForAdminQuery request, CancellationToken cancellationToken)
    {
        var skillsByCategory = StaticDataProvider.GetSkillsByCategory();
        var currentId = 1;

        foreach (var (category, skillNames) in skillsByCategory.OrderBy(x => x.Key))
        {
            var displayOrder = 1;
            foreach (var skillName in skillNames)
            {
                if (currentId == request.Id)
                {
                    return await Task.FromResult(new SkillDto
                    {
                        Id = currentId,
                        Name = skillName,
                        Category = category,
                        DisplayOrder = displayOrder,
                        IconUrl = null
                    });
                }
                currentId++;
                displayOrder++;
            }
        }

        return await Task.FromResult<SkillDto?>(null);
    }
}
