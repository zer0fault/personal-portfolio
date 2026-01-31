using Domain.Entities;
using Domain.Enums;
using FluentAssertions;

namespace Domain.Tests.Entities;

public class SkillTests
{
    [Fact]
    public void Skill_ShouldInitialize_WithDefaultValues()
    {
        // Arrange & Act
        var skill = new Skill();

        // Assert
        skill.Id.Should().Be(0);
        skill.Name.Should().BeEmpty();
        skill.Category.Should().Be(SkillCategory.Language);
        skill.ProficiencyLevel.Should().Be(ProficiencyLevel.Beginner);
        skill.DisplayOrder.Should().Be(0);
        skill.IconUrl.Should().BeNull();
    }

    [Fact]
    public void Skill_ShouldAllowSettingProperties()
    {
        // Arrange
        var skill = new Skill();
        var createdDate = DateTime.UtcNow;
        var modifiedDate = DateTime.UtcNow.AddMinutes(5);

        // Act
        skill.Id = 1;
        skill.Name = "C#";
        skill.Category = SkillCategory.Language;
        skill.ProficiencyLevel = ProficiencyLevel.Advanced;
        skill.DisplayOrder = 1;
        skill.IconUrl = "https://cdn.example.com/csharp.svg";
        skill.CreatedDate = createdDate;
        skill.ModifiedDate = modifiedDate;

        // Assert
        skill.Id.Should().Be(1);
        skill.Name.Should().Be("C#");
        skill.Category.Should().Be(SkillCategory.Language);
        skill.ProficiencyLevel.Should().Be(ProficiencyLevel.Advanced);
        skill.DisplayOrder.Should().Be(1);
        skill.IconUrl.Should().Be("https://cdn.example.com/csharp.svg");
        skill.CreatedDate.Should().Be(createdDate);
        skill.ModifiedDate.Should().Be(modifiedDate);
    }

    [Theory]
    [InlineData(SkillCategory.Language)]
    [InlineData(SkillCategory.Framework)]
    [InlineData(SkillCategory.Cloud)]
    [InlineData(SkillCategory.Architecture)]
    [InlineData(SkillCategory.Practice)]
    [InlineData(SkillCategory.Domain)]
    public void Skill_ShouldSupportAllCategories(SkillCategory category)
    {
        // Arrange
        var skill = new Skill();

        // Act
        skill.Category = category;

        // Assert
        skill.Category.Should().Be(category);
    }

    [Theory]
    [InlineData(ProficiencyLevel.Beginner)]
    [InlineData(ProficiencyLevel.Intermediate)]
    [InlineData(ProficiencyLevel.Advanced)]
    [InlineData(ProficiencyLevel.Expert)]
    public void Skill_ShouldSupportAllProficiencyLevels(ProficiencyLevel level)
    {
        // Arrange
        var skill = new Skill();

        // Act
        skill.ProficiencyLevel = level;

        // Assert
        skill.ProficiencyLevel.Should().Be(level);
    }

    [Fact]
    public void Skill_IconUrl_CanBeNull()
    {
        // Arrange & Act
        var skill = new Skill
        {
            Name = "C#",
            Category = SkillCategory.Language,
            IconUrl = null
        };

        // Assert
        skill.IconUrl.Should().BeNull();
    }

    [Fact]
    public void Skill_DisplayOrder_ShouldDetermineListingOrder()
    {
        // Arrange
        var skill1 = new Skill { Name = "Skill 1", DisplayOrder = 2 };
        var skill2 = new Skill { Name = "Skill 2", DisplayOrder = 1 };
        var skill3 = new Skill { Name = "Skill 3", DisplayOrder = 3 };
        var skills = new List<Skill> { skill1, skill2, skill3 };

        // Act
        var orderedSkills = skills.OrderBy(s => s.DisplayOrder).ToList();

        // Assert
        orderedSkills[0].Name.Should().Be("Skill 2");
        orderedSkills[1].Name.Should().Be("Skill 1");
        orderedSkills[2].Name.Should().Be("Skill 3");
    }
}
