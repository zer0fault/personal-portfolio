using Application.Settings.Queries.DTOs;
using MediatR;

namespace Application.Settings.Queries.GetAllSettings;

/// <summary>
/// Query to get all settings
/// </summary>
public record GetAllSettingsQuery : IRequest<List<SettingsDto>>;
