using MediatR;

namespace Application.Settings.Commands.CreateSetting;

/// <summary>
/// Handler for CreateSettingCommand
/// </summary>
public class CreateSettingCommandHandler : IRequestHandler<CreateSettingCommand, int>
{
    public async Task<int> Handle(CreateSettingCommand request, CancellationToken cancellationToken)
    {
        // Note: Data is hardcoded - this command does not persist changes

        // Return fake ID for API compatibility
        await Task.CompletedTask;
        return 999;
    }
}
