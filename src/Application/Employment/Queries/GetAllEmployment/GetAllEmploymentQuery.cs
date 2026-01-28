using Application.Employment.Queries.DTOs;
using MediatR;

namespace Application.Employment.Queries.GetAllEmployment;

/// <summary>
/// Query to get all employment history ordered by display order
/// </summary>
public record GetAllEmploymentQuery : IRequest<List<EmploymentDto>>;
