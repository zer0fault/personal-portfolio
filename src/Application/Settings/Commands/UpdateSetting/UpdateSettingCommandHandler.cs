using MediatR;

namespace Application.Settings.Commands.UpdateSetting;

/// <summary>
/// Handler for UpdateSettingCommand
/// </summary>
public class UpdateSettingCommandHandler : IRequestHandler<UpdateSettingCommand, Unit>
{
    public async Task<Unit> Handle(UpdateSettingCommand request, CancellationToken cancellationToken)
    {
        // Note: Data is hardcoded - this command does not persist changes

        // Return success for API compatibility
        await Task.CompletedTask;
        return Unit.Value;
    }
}
