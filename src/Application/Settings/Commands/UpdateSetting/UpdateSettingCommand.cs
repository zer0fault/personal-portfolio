using MediatR;

namespace Application.Settings.Commands.UpdateSetting;

/// <summary>
/// Command to update an existing setting
/// </summary>
public class UpdateSettingCommand : IRequest<Unit>
{
    public int Id { get; set; }
    public string Key { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
}
