using Domain.Enums;
using MediatR;

namespace Application.Skills.Commands.UpdateSkill;

/// <summary>
/// Command to update an existing skill
/// </summary>
public class UpdateSkillCommand : IRequest<bool>
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public SkillCategory Category { get; set; }
    public int DisplayOrder { get; set; }
    public string? IconUrl { get; set; }
}
