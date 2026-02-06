using Application.Skills.Commands.CreateSkill;
using Domain.Enums;
using FluentAssertions;
using Xunit;

namespace Application.Tests.Skills.Commands.CreateSkill;

public class CreateSkillCommandHandlerTests
{
    private readonly CreateSkillCommandHandler _handler;

    public CreateSkillCommandHandlerTests()
    {
        _handler = new CreateSkillCommandHandler();
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_WithValidData()
    {
        // Arrange
        var command = new CreateSkillCommand
        {
            Name = "C#",
            Category = SkillCategory.Language,
            DisplayOrder = 1,
            IconUrl = "https://example.com/csharp-icon.svg"
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_WithoutIconUrl()
    {
        // Arrange
        var command = new CreateSkillCommand
        {
            Name = "TypeScript",
            Category = SkillCategory.Language,
            DisplayOrder = 7,
            IconUrl = null
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_WithZeroDisplayOrder()
    {
        // Arrange
        var command = new CreateSkillCommand
        {
            Name = "JavaScript",
            Category = SkillCategory.Language,
            DisplayOrder = 0,
            IconUrl = null
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task Handle_Should_ThrowException_WhenNameIsEmpty()
    {
        // Arrange
        var command = new CreateSkillCommand
        {
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
        var command = new CreateSkillCommand
        {
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
}
