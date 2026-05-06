using Application.Common.Data;
using Application.Skills.Queries.DTOs;
using Application.Skills.Queries.GetSkillByIdForAdmin;
using Domain.Enums;
using FluentAssertions;
using Xunit;

namespace Application.Tests.Skills.Queries.GetSkillByIdForAdmin;

public class GetSkillByIdForAdminQueryHandlerTests
{
    private readonly GetSkillByIdForAdminQueryHandler _handler;

    public GetSkillByIdForAdminQueryHandlerTests()
    {
        _handler = new GetSkillByIdForAdminQueryHandler();
    }

    [Fact]
    public async Task Handle_Should_Return_Skill_When_Exists()
    {
        // Arrange - StaticDataProvider has skill with Id = 1 (C#)
        var query = new GetSkillByIdForAdminQuery(1);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(1);
        result.Name.Should().Be("C#");
        result.Category.Should().Be(SkillCategory.Language);
    }

    [Fact]
    public async Task Handle_Should_Return_Null_When_Skill_Not_Found()
    {
        // Arrange
        var query = new GetSkillByIdForAdminQuery(999);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task Handle_Should_Return_Skill_For_Different_Categories()
    {
        var skillsByCategory = StaticDataProvider.GetSkillsByCategory();

        // IDs are assigned in enum order; compute start ID for each category
        var firstFrameworkId = skillsByCategory
            .Where(kvp => (int)kvp.Key < (int)SkillCategory.Framework)
            .Sum(kvp => kvp.Value.Count) + 1;
        var firstCloudId = skillsByCategory
            .Where(kvp => (int)kvp.Key < (int)SkillCategory.Cloud)
            .Sum(kvp => kvp.Value.Count) + 1;

        var languageResult = await _handler.Handle(new GetSkillByIdForAdminQuery(1), CancellationToken.None);
        var frameworkResult = await _handler.Handle(new GetSkillByIdForAdminQuery(firstFrameworkId), CancellationToken.None);
        var cloudResult = await _handler.Handle(new GetSkillByIdForAdminQuery(firstCloudId), CancellationToken.None);

        languageResult!.Category.Should().Be(SkillCategory.Language);
        languageResult.Name.Should().Be("C#");

        frameworkResult!.Category.Should().Be(SkillCategory.Framework);
        frameworkResult.Name.Should().Be(skillsByCategory[SkillCategory.Framework][0]);

        cloudResult!.Category.Should().Be(SkillCategory.Cloud);
        cloudResult.Name.Should().Be(skillsByCategory[SkillCategory.Cloud][0]);
    }

    [Fact]
    public async Task Handle_Should_Map_All_Properties_Correctly()
    {
        // Arrange - ID 3 is TypeScript (Language category, display order 3)
        var query = new GetSkillByIdForAdminQuery(3);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(3);
        result.Name.Should().Be("TypeScript");
        result.Category.Should().Be(SkillCategory.Language);
        result.DisplayOrder.Should().Be(3);
    }
}
