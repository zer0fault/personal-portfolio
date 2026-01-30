using MediatR;

namespace Application.Settings.Commands.CreateSetting;

/// <summary>
/// Command to create a new setting
/// </summary>
public class CreateSettingCommand : IRequest<int>
{
    public string Key { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
}
