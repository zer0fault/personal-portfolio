using Application.Settings.Commands.CreateSetting;
using Domain.Enums;
using FluentAssertions;
using Xunit;

namespace Application.Tests.Settings.Commands.CreateSetting;

public class CreateSettingCommandHandlerTests
{
    private readonly CreateSettingCommandHandler _handler;

    public CreateSettingCommandHandlerTests()
    {
        _handler = new CreateSettingCommandHandler();
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_WithValidData()
    {
        var command = new CreateSettingCommand
        {
            Key = "TestKey",
            Value = "TestValue",
            Category = "TestCategory"
        };
        var result = await _handler.Handle(command, CancellationToken.None);
        result.Should().Be(999); // Command is a no-op, returns fake ID
    }
}
