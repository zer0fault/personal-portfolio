using Application.Employment.Queries.DTOs;
using MediatR;

namespace Application.Employment.Queries.GetEmploymentByIdForAdmin;

/// <summary>
/// Query to get an employment entry by ID for admin
/// </summary>
public record GetEmploymentByIdForAdminQuery(int Id) : IRequest<EmploymentDto?>;
