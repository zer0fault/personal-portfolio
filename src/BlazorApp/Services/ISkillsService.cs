using Application.Skills.Queries.DTOs;
using Application.Skills.Commands.CreateSkill;
using Application.Skills.Commands.UpdateSkill;

namespace BlazorApp.Services;

public interface ISkillsService
{
    Task<List<SkillDto>> GetAllSkillsAsync();
    Task<SkillDto?> GetSkillByIdAsync(int id);
    Task<int?> CreateSkillAsync(CreateSkillCommand command);
    Task<bool> UpdateSkillAsync(UpdateSkillCommand command);
    Task<bool> DeleteSkillAsync(int id);
}
