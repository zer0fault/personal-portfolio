using Application.Projects.Queries.DTOs;
using MediatR;

namespace Application.Projects.Queries.GetAllProjects;

/// <summary>
/// Query to get all published projects ordered by display order
/// </summary>
public record GetAllProjectsQuery : IRequest<List<ProjectDto>>;
