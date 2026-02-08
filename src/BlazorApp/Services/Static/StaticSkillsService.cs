using Application.Common.Data;
using Application.Skills.Queries.DTOs;
using Domain.Enums;

namespace BlazorApp.Services.Static;

public class StaticSkillsService : ISkillsService
{
    public Task<List<SkillDto>> GetAllSkillsAsync()
    {
        var skillsByCategory = StaticDataProvider.GetSkillsByCategory();
        var skills = ConvertToSkillDtos(skillsByCategory);

        return Task.FromResult(skills);
    }

    public Task<List<SkillDto>> GetSkillsByCategoryAsync(string category)
    {
        if (!Enum.TryParse<SkillCategory>(category, out var categoryEnum))
            return Task.FromResult(new List<SkillDto>());

        var skillsByCategory = StaticDataProvider.GetSkillsByCategory();

        if (!skillsByCategory.ContainsKey(categoryEnum))
            return Task.FromResult(new List<SkillDto>());

        var skills = ConvertCategoryToSkillDtos(categoryEnum, skillsByCategory[categoryEnum]);

        return Task.FromResult(skills);
    }

    public Task<SkillDto?> GetSkillByIdAsync(int id)
    {
        var allSkills = GetAllSkillsAsync().Result;
        var skill = allSkills.FirstOrDefault(s => s.Id == id);

        return Task.FromResult(skill);
    }

    // Admin methods - not supported in static mode
    public Task<int?> CreateSkillAsync(Application.Skills.Commands.CreateSkill.CreateSkillCommand command)
    {
        Console.WriteLine("Create operations are not supported in static mode");
        return Task.FromResult<int?>(null);
    }

    public Task<bool> UpdateSkillAsync(Application.Skills.Commands.UpdateSkill.UpdateSkillCommand command)
    {
        Console.WriteLine("Update operations are not supported in static mode");
        return Task.FromResult(false);
    }

    public Task<bool> DeleteSkillAsync(int id)
    {
        Console.WriteLine("Delete operations are not supported in static mode");
        return Task.FromResult(false);
    }

    /// <summary>
    /// Converts the simplified skills dictionary into SkillDto objects with generated IDs and display orders
    /// </summary>
    private static List<SkillDto> ConvertToSkillDtos(Dictionary<SkillCategory, List<string>> skillsByCategory)
    {
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

        return skills;
    }

    /// <summary>
    /// Converts a single category's skills into SkillDto objects
    /// </summary>
    private static List<SkillDto> ConvertCategoryToSkillDtos(SkillCategory category, List<string> skillNames)
    {
        var skills = new List<SkillDto>();
        var displayOrder = 1;

        foreach (var skillName in skillNames)
        {
            skills.Add(new SkillDto
            {
                Id = displayOrder, // Use display order as ID for single category
                Name = skillName,
                Category = category,
                DisplayOrder = displayOrder++,
                IconUrl = null
            });
        }

        return skills;
    }
}
