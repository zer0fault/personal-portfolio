using Application.Settings.Commands.UpdateSetting;
using FluentAssertions;
using Xunit;

namespace Application.Tests.Settings.Commands.UpdateSetting;

public class UpdateSettingCommandHandlerTests
{
    private readonly UpdateSettingCommandHandler _handler;

    public UpdateSettingCommandHandlerTests()
    {
        _handler = new UpdateSettingCommandHandler();
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_WithValidData()
    {
        var command = new UpdateSettingCommand { Id = 1, Key = "UpdatedKey", Value = "UpdatedValue", Category = "UpdatedCategory" };
        var result = await _handler.Handle(command, CancellationToken.None);
        result.Should().Be(MediatR.Unit.Value); // Command is a no-op
    }
}
