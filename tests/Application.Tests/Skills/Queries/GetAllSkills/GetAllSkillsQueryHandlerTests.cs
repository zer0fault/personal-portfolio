using Application.Common.Data;
using Application.Common.Mappings;
using Application.Skills.Queries.DTOs;
using Application.Skills.Queries.GetAllSkills;
using AutoMapper;
using Domain.Enums;
using FluentAssertions;
using Xunit;

namespace Application.Tests.Skills.Queries.GetAllSkills;

public class GetAllSkillsQueryHandlerTests
{
    private readonly IMapper _mapper;
    private readonly GetAllSkillsQueryHandler _handler;

    public GetAllSkillsQueryHandlerTests()
    {
        // Use the actual MappingProfile
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });
        _mapper = configuration.CreateMapper();

        _handler = new GetAllSkillsQueryHandler(_mapper);
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
        result.Should().HaveCount(7); // StaticDataProvider has 7 skills
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
        result.Should().HaveCount(7);

        // Verify ordering: Language(0) < Framework(1) < Cloud(2) < Architecture(3) < Practice(4)
        result[0].Category.Should().Be(SkillCategory.Language);
        result[1].Category.Should().Be(SkillCategory.Language);
        result[2].Category.Should().Be(SkillCategory.Framework);
        result[3].Category.Should().Be(SkillCategory.Framework);
        result[4].Category.Should().Be(SkillCategory.Cloud);
        result[5].Category.Should().Be(SkillCategory.Architecture);
        result[6].Category.Should().Be(SkillCategory.Practice);
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
        result.Should().Contain(s => s.Name == "ASP.NET Core");
        result.Should().Contain(s => s.Name == "Blazor");
        result.Should().Contain(s => s.Name == "Azure");
        result.Should().Contain(s => s.Name == "Clean Architecture");
        result.Should().Contain(s => s.Name == "Unit Testing");
    }
}
