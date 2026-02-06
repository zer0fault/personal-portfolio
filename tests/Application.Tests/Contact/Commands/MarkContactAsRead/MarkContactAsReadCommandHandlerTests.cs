using Application.Contact.Commands.MarkContactAsRead;
using FluentAssertions;
using Xunit;

namespace Application.Tests.Contact.Commands.MarkContactAsRead;

public class MarkContactAsReadCommandHandlerTests
{
    private readonly MarkContactAsReadCommandHandler _handler;

    public MarkContactAsReadCommandHandlerTests()
    {
        _handler = new MarkContactAsReadCommandHandler();
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess()
    {
        // Arrange
        var command = new MarkContactAsReadCommand { Id = 1 };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_ForAnyId()
    {
        // Arrange
        var command = new MarkContactAsReadCommand { Id = 999 };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
    }
}
