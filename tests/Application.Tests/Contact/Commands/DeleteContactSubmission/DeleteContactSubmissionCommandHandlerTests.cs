using Application.Contact.Commands.DeleteContactSubmission;
using FluentAssertions;
using Xunit;

namespace Application.Tests.Contact.Commands.DeleteContactSubmission;

public class DeleteContactSubmissionCommandHandlerTests
{
    private readonly DeleteContactSubmissionCommandHandler _handler;

    public DeleteContactSubmissionCommandHandlerTests()
    {
        _handler = new DeleteContactSubmissionCommandHandler();
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess()
    {
        // Arrange
        var command = new DeleteContactSubmissionCommand { Id = 1 };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_ForAnyId()
    {
        // Arrange
        var command = new DeleteContactSubmissionCommand { Id = 999 };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
    }
}
