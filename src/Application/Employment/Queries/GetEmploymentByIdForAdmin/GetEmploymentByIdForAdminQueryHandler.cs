using Application.Common.Interfaces;
using Application.Employment.Queries.DTOs;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Employment.Queries.GetEmploymentByIdForAdmin;

/// <summary>
/// Handler for GetEmploymentByIdForAdminQuery
/// </summary>
public class GetEmploymentByIdForAdminQueryHandler : IRequestHandler<GetEmploymentByIdForAdminQuery, EmploymentDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetEmploymentByIdForAdminQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<EmploymentDto?> Handle(GetEmploymentByIdForAdminQuery request, CancellationToken cancellationToken)
    {
        var employment = await _context.EmploymentHistory
            .Where(e => e.Id == request.Id && !e.IsDeleted)
            .FirstOrDefaultAsync(cancellationToken);

        return employment == null ? null : _mapper.Map<EmploymentDto>(employment);
    }
}
