using Application.Skills.Commands.DeleteSkill;
using FluentAssertions;
using Xunit;

namespace Application.Tests.Skills.Commands.DeleteSkill;

public class DeleteSkillCommandHandlerTests
{
    private readonly DeleteSkillCommandHandler _handler;

    public DeleteSkillCommandHandlerTests()
    {
        _handler = new DeleteSkillCommandHandler();
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess()
    {
        // Arrange
        var command = new DeleteSkillCommand { Id = 1 };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_ForAnyId()
    {
        // Arrange
        var command = new DeleteSkillCommand { Id = 999 };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
    }
}
