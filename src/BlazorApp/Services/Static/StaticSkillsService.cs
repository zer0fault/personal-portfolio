using Application.Common.Data;
using Application.Skills.Queries.DTOs;
using AutoMapper;

namespace BlazorApp.Services.Static;

public class StaticSkillsService : ISkillsService
{
    private readonly IMapper _mapper;

    public StaticSkillsService(IMapper mapper)
    {
        _mapper = mapper;
    }

    public Task<List<SkillDto>> GetAllSkillsAsync()
    {
        var skills = StaticDataProvider.GetSkills()
            .OrderBy(s => s.Category)
            .ThenBy(s => s.DisplayOrder)
            .ToList();

        return Task.FromResult(_mapper.Map<List<SkillDto>>(skills));
    }

    public Task<List<SkillDto>> GetSkillsByCategoryAsync(string category)
    {
        var skills = StaticDataProvider.GetSkills()
            .Where(s => s.Category.ToString() == category)
            .OrderBy(s => s.DisplayOrder)
            .ToList();

        return Task.FromResult(_mapper.Map<List<SkillDto>>(skills));
    }

    public Task<SkillDto?> GetSkillByIdAsync(int id)
    {
        var skill = StaticDataProvider.GetSkills()
            .FirstOrDefault(s => s.Id == id);

        if (skill == null)
            return Task.FromResult<SkillDto?>(null);

        return Task.FromResult<SkillDto?>(_mapper.Map<SkillDto>(skill));
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
}
