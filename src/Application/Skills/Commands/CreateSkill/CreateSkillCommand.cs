using Domain.Enums;
using MediatR;

namespace Application.Skills.Commands.CreateSkill;

/// <summary>
/// Command to create a new skill
/// </summary>
public class CreateSkillCommand : IRequest<int>
{
    public string Name { get; set; } = string.Empty;
    public SkillCategory Category { get; set; }
    public int DisplayOrder { get; set; }
    public string? IconUrl { get; set; }
}
