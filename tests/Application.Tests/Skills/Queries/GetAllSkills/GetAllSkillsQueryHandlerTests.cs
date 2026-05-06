using Application.Common.Data;
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
        var query = new GetAllSkillsQuery();

        var result = await _handler.Handle(query, CancellationToken.None);

        var expectedCount = StaticDataProvider.GetSkillsByCategory().Sum(x => x.Value.Count);
        result.Should().NotBeNull();
        result.Should().HaveCount(expectedCount);
    }

    [Fact]
    public async Task Handle_Should_Order_Skills_By_Category_Then_DisplayOrder()
    {
        var query = new GetAllSkillsQuery();

        var result = await _handler.Handle(query, CancellationToken.None);

        var skillsByCategory = StaticDataProvider.GetSkillsByCategory();
        var expectedCount = skillsByCategory.Sum(x => x.Value.Count);
        result.Should().NotBeNull();
        result.Should().HaveCount(expectedCount);

        result[0].Category.Should().Be(SkillCategory.Language);
        result[0].Name.Should().Be("C#");
        result[0].DisplayOrder.Should().Be(1);

        // First Framework skill falls immediately after all Language skills
        var firstFrameworkIndex = skillsByCategory[SkillCategory.Language].Count;
        result[firstFrameworkIndex].Category.Should().Be(SkillCategory.Framework);
        result[firstFrameworkIndex].Name.Should().Be(skillsByCategory[SkillCategory.Framework][0]);
        result[firstFrameworkIndex].DisplayOrder.Should().Be(1);

        // Categories should appear in enum order
        var categories = result.Select(s => s.Category).Distinct().ToList();
        var expectedCategories = skillsByCategory.Keys.OrderBy(k => k).ToList();
        categories.Should().ContainInOrder(expectedCategories);
    }

    [Fact]
    public async Task Handle_Should_Include_Skill_Names()
    {
        var query = new GetAllSkillsQuery();

        var result = await _handler.Handle(query, CancellationToken.None);

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
