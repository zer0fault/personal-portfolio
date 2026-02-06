using Application.Employment.Commands.DeleteEmployment;
using FluentAssertions;
using MediatR;
using Xunit;

namespace Application.Tests.Employment.Commands.DeleteEmployment;

public class DeleteEmploymentCommandHandlerTests
{
    private readonly DeleteEmploymentCommandHandler _handler;

    public DeleteEmploymentCommandHandlerTests()
    {
        _handler = new DeleteEmploymentCommandHandler();
    }

    [Fact]
    public async Task Handle_Should_Return_Unit()
    {
        // Arrange
        var command = new DeleteEmploymentCommand { Id = 1 };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
    }
}
