using Application.Settings.Queries.DTOs;
using MediatR;

namespace Application.Settings.Queries.GetSettingsByCategory;

/// <summary>
/// Query to get settings by category
/// </summary>
public record GetSettingsByCategoryQuery(string Category) : IRequest<List<SettingsDto>>;
