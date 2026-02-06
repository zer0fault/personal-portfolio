using MediatR;

namespace Application.Settings.Commands.DeleteSetting;

/// <summary>
/// Handler for DeleteSettingCommand
/// </summary>
public class DeleteSettingCommandHandler : IRequestHandler<DeleteSettingCommand, Unit>
{
    public async Task<Unit> Handle(DeleteSettingCommand request, CancellationToken cancellationToken)
    {
        // Note: Data is hardcoded - this command does not persist changes

        // Return success for API compatibility
        await Task.CompletedTask;
        return Unit.Value;
    }
}
