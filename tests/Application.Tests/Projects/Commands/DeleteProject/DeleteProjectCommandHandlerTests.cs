using Application.Projects.Commands.DeleteProject;
using FluentAssertions;
using Xunit;

namespace Application.Tests.Projects.Commands.DeleteProject;

public class DeleteProjectCommandHandlerTests
{
    private readonly DeleteProjectCommandHandler _handler;

    public DeleteProjectCommandHandlerTests()
    {
        _handler = new DeleteProjectCommandHandler();
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess()
    {
        // Arrange
        var command = new DeleteProjectCommand { Id = 1 };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_ForAnyId()
    {
        // Arrange
        var command = new DeleteProjectCommand { Id = 999 };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
    }
}
