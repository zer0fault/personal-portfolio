using Application.Common.Interfaces;
using Application.Contact.Queries.DTOs;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Contact.Queries.GetContactSubmissionById;

/// <summary>
/// Handler for GetContactSubmissionByIdQuery
/// </summary>
public class GetContactSubmissionByIdQueryHandler : IRequestHandler<GetContactSubmissionByIdQuery, ContactSubmissionDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetContactSubmissionByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ContactSubmissionDto?> Handle(GetContactSubmissionByIdQuery request, CancellationToken cancellationToken)
    {
        var submission = await _context.ContactSubmissions
            .Where(c => c.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);

        return submission == null ? null : _mapper.Map<ContactSubmissionDto>(submission);
    }
}
