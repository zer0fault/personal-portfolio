using Application.Skills.Queries.DTOs;
using MediatR;

namespace Application.Skills.Queries.GetAllSkills;

/// <summary>
/// Query to get all skills grouped by category and ordered by display order
/// </summary>
public record GetAllSkillsQuery : IRequest<List<SkillDto>>;
