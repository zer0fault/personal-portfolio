using Domain.Enums;

namespace BlazorApp.Helpers;

public static class SkillCategoryExtensions
{
    public static string GetDisplayName(this SkillCategory category) => category switch
    {
        SkillCategory.Language => "Languages",
        SkillCategory.Framework => "Frameworks & Libraries",
        SkillCategory.Cloud => "Cloud & DevOps",
        SkillCategory.Architecture => "Architecture & Design",
        SkillCategory.Practice => "Practices & Tools",
        SkillCategory.Domain => "Domain Knowledge",
        SkillCategory.AI => "AI / LLM Integration",
        _ => category.ToString()
    };
}
