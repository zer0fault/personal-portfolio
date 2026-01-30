using Application.Projects.Queries.DTOs;
using MediatR;

namespace Application.Projects.Queries.GetProjectByIdForAdmin;

/// <summary>
/// Query to get a project by ID for admin (any status)
/// </summary>
public record GetProjectByIdForAdminQuery(int Id) : IRequest<ProjectDetailDto?>;
