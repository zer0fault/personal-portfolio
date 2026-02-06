using Application.Skills.Commands.UpdateSkill;
using Domain.Enums;
using FluentAssertions;
using Xunit;

namespace Application.Tests.Skills.Commands.UpdateSkill;

public class UpdateSkillCommandHandlerTests
{
    private readonly UpdateSkillCommandHandler _handler;

    public UpdateSkillCommandHandlerTests()
    {
        _handler = new UpdateSkillCommandHandler();
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_WithValidData()
    {
        // Arrange
        var command = new UpdateSkillCommand
        {
            Id = 1,
            Name = "Updated Skill",
            Category = SkillCategory.Framework,
            DisplayOrder = 2,
            IconUrl = "https://example.com/new-icon.svg"
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_Should_ThrowException_WhenNameIsEmpty()
    {
        // Arrange
        var command = new UpdateSkillCommand
        {
            Id = 1,
            Name = "",
            Category = SkillCategory.Language,
            DisplayOrder = 1,
            IconUrl = null
        };

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("Skill name is required");
    }

    [Fact]
    public async Task Handle_Should_ThrowException_WhenNameIsWhitespace()
    {
        // Arrange
        var command = new UpdateSkillCommand
        {
            Id = 1,
            Name = "   ",
            Category = SkillCategory.Framework,
            DisplayOrder = 1,
            IconUrl = null
        };

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("Skill name is required");
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_WithNullIconUrl()
    {
        // Arrange
        var command = new UpdateSkillCommand
        {
            Id = 1,
            Name = "Vue.js",
            Category = SkillCategory.Framework,
            DisplayOrder = 1,
            IconUrl = null
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
    }
}
