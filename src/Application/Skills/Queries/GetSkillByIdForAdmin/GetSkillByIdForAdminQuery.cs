using Application.Skills.Queries.DTOs;
using MediatR;

namespace Application.Skills.Queries.GetSkillByIdForAdmin;

/// <summary>
/// Query to get a skill by ID for admin
/// </summary>
public record GetSkillByIdForAdminQuery(int Id) : IRequest<SkillDto?>;
