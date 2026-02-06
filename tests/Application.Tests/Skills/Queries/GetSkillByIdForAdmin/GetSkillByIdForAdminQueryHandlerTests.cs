using Application.Common.Data;
using Application.Common.Mappings;
using Application.Skills.Queries.DTOs;
using Application.Skills.Queries.GetSkillByIdForAdmin;
using AutoMapper;
using Domain.Enums;
using FluentAssertions;
using Xunit;

namespace Application.Tests.Skills.Queries.GetSkillByIdForAdmin;

public class GetSkillByIdForAdminQueryHandlerTests
{
    private readonly IMapper _mapper;
    private readonly GetSkillByIdForAdminQueryHandler _handler;

    public GetSkillByIdForAdminQueryHandlerTests()
    {
        // Use the actual MappingProfile
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });
        _mapper = configuration.CreateMapper();

        _handler = new GetSkillByIdForAdminQueryHandler(_mapper);
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
        // Arrange & Act
        var languageResult = await _handler.Handle(new GetSkillByIdForAdminQuery(1), CancellationToken.None);
        var frameworkResult = await _handler.Handle(new GetSkillByIdForAdminQuery(3), CancellationToken.None);
        var cloudResult = await _handler.Handle(new GetSkillByIdForAdminQuery(5), CancellationToken.None);

        // Assert
        languageResult!.Category.Should().Be(SkillCategory.Language);
        frameworkResult!.Category.Should().Be(SkillCategory.Framework);
        cloudResult!.Category.Should().Be(SkillCategory.Cloud);
    }

    [Fact]
    public async Task Handle_Should_Map_All_Properties_Correctly()
    {
        // Arrange - StaticDataProvider has skill with Id = 3 (ASP.NET Core)
        var query = new GetSkillByIdForAdminQuery(3);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(3);
        result.Name.Should().Be("ASP.NET Core");
        result.Category.Should().Be(SkillCategory.Framework);
        result.DisplayOrder.Should().Be(1);
    }
}
