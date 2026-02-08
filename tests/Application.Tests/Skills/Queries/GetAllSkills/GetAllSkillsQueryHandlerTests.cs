using Application.Skills.Queries.DTOs;
using Application.Skills.Queries.GetAllSkills;
using Domain.Enums;
using FluentAssertions;
using Xunit;

namespace Application.Tests.Skills.Queries.GetAllSkills;

public class GetAllSkillsQueryHandlerTests
{
    private readonly GetAllSkillsQueryHandler _handler;

    public GetAllSkillsQueryHandlerTests()
    {
        _handler = new GetAllSkillsQueryHandler();
    }

    [Fact]
    public async Task Handle_Should_Return_All_Skills()
    {
        // Arrange
        var query = new GetAllSkillsQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(32); // StaticDataProvider has 32 skills across all categories
    }

    [Fact]
    public async Task Handle_Should_Order_Skills_By_Category_Then_DisplayOrder()
    {
        // Arrange
        var query = new GetAllSkillsQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(32);

        // Verify ordering: Language(0) < Framework(1) < Cloud(2) < Architecture(3) < Practice(4)
        // First skills should be from Language category
        result[0].Category.Should().Be(SkillCategory.Language);
        result[0].Name.Should().Be("C#");
        result[0].DisplayOrder.Should().Be(1);

        // Language category has 7 skills, so Framework starts at index 7
        result[7].Category.Should().Be(SkillCategory.Framework);
        result[7].Name.Should().Be(".NET Framework");
        result[7].DisplayOrder.Should().Be(1);

        // Categories should be ordered: Language -> Framework -> Cloud -> Architecture -> Practice
        var categories = result.Select(s => s.Category).Distinct().ToList();
        categories.Should().ContainInOrder(
            SkillCategory.Language,
            SkillCategory.Framework,
            SkillCategory.Cloud,
            SkillCategory.Architecture,
            SkillCategory.Practice
        );
    }

    [Fact]
    public async Task Handle_Should_Include_Skill_Names()
    {
        // Arrange
        var query = new GetAllSkillsQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().Contain(s => s.Name == "C#");
        result.Should().Contain(s => s.Name == "JavaScript");
        result.Should().Contain(s => s.Name == "ASP.NET");
        result.Should().Contain(s => s.Name == "Blazor");
        result.Should().Contain(s => s.Name == "Microsoft Azure");
        result.Should().Contain(s => s.Name == "Clean Architecture");
        result.Should().Contain(s => s.Name == "Unit Testing");
    }
}
