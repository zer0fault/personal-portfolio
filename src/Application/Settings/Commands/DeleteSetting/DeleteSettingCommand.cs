using MediatR;

namespace Application.Settings.Commands.DeleteSetting;

/// <summary>
/// Command to soft delete a setting
/// </summary>
public class DeleteSettingCommand : IRequest<Unit>
{
    public int Id { get; set; }

    public DeleteSettingCommand(int id)
    {
        Id = id;
    }
}
