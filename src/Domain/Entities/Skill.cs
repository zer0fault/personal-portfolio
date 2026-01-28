using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;

/// <summary>
/// Represents a technical skill
/// </summary>
public class Skill : BaseEntity
{
    /// <summary>
    /// Name of the skill (e.g., "C#", "ASP.NET Core", "Azure")
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Category of the skill
    /// </summary>
    public SkillCategory Category { get; set; }

    /// <summary>
    /// Proficiency level for this skill
    /// </summary>
    public ProficiencyLevel ProficiencyLevel { get; set; }

    /// <summary>
    /// Display order for sorting skills
    /// </summary>
    public int DisplayOrder { get; set; }

    /// <summary>
    /// Optional URL to an icon representing the skill
    /// </summary>
    public string? IconUrl { get; set; }
}
