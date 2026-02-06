using Application.Settings.Commands.DeleteSetting;
using FluentAssertions;
using MediatR;
using Xunit;

namespace Application.Tests.Settings.Commands.DeleteSetting;

public class DeleteSettingCommandHandlerTests
{
    private readonly DeleteSettingCommandHandler _handler;

    public DeleteSettingCommandHandlerTests()
    {
        _handler = new DeleteSettingCommandHandler();
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess()
    {
        // Arrange
        var command = new DeleteSettingCommand(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(Unit.Value);
    }
}
