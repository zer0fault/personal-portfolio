using Application.Contact.Queries.DTOs;
using MediatR;

namespace Application.Contact.Queries.GetContactSubmissionById;

/// <summary>
/// Query to get a contact submission by ID for admin
/// </summary>
public record GetContactSubmissionByIdQuery(int Id) : IRequest<ContactSubmissionDto?>;
