using Application.Projects.Queries.DTOs;
using MediatR;

namespace Application.Projects.Queries.GetProjectById;

/// <summary>
/// Query to get a project by its ID
/// </summary>
public record GetProjectByIdQuery(int Id) : IRequest<ProjectDetailDto?>;
