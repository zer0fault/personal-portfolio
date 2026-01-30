using Application.Projects.Queries.DTOs;
using MediatR;

namespace Application.Projects.Queries.GetAllProjectsForAdmin;

/// <summary>
/// Query to get all projects for admin (includes Draft, Published, Archived)
/// </summary>
public class GetAllProjectsForAdminQuery : IRequest<List<ProjectDto>>
{
}
