using Application.Contact.Queries.DTOs;
using MediatR;

namespace Application.Contact.Queries.GetAllContactSubmissions;

/// <summary>
/// Query to get all contact submissions for admin
/// </summary>
public record GetAllContactSubmissionsQuery : IRequest<List<ContactSubmissionDto>>;
