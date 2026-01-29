using Application.Skills.Queries.DTOs;

namespace BlazorApp.Services;

public interface ISkillsService
{
    Task<List<SkillDto>> GetAllSkillsAsync();
}
