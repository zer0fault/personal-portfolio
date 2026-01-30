using Application.Settings.Queries.DTOs;
using MediatR;

namespace Application.Settings.Queries.GetSettingById;

/// <summary>
/// Query to get a setting by ID (for admin)
/// </summary>
public class GetSettingByIdQuery : IRequest<SettingsDto>
{
    public int Id { get; set; }

    public GetSettingByIdQuery(int id)
    {
        Id = id;
    }
}
