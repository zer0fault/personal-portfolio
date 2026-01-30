using Application.Common.Interfaces;
using Application.Contact.Queries.DTOs;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Contact.Queries.GetAllContactSubmissions;

/// <summary>
/// Handler for GetAllContactSubmissionsQuery
/// </summary>
public class GetAllContactSubmissionsQueryHandler : IRequestHandler<GetAllContactSubmissionsQuery, List<ContactSubmissionDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllContactSubmissionsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ContactSubmissionDto>> Handle(GetAllContactSubmissionsQuery request, CancellationToken cancellationToken)
    {
        var submissions = await _context.ContactSubmissions
            .OrderByDescending(c => c.SubmittedDate)
            .ToListAsync(cancellationToken);

        return _mapper.Map<List<ContactSubmissionDto>>(submissions);
    }
}
