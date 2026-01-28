using Domain.Enums;

namespace Application.Skills.Queries.DTOs;

/// <summary>
/// DTO for skill display
/// </summary>
public class SkillDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public SkillCategory Category { get; set; }
    public ProficiencyLevel ProficiencyLevel { get; set; }
    public int DisplayOrder { get; set; }
    public string? IconUrl { get; set; }
}
