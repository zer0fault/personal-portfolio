using Application.Common.Interfaces;
using Application.Employment.Queries.DTOs;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Employment.Queries.GetAllEmployment;

/// <summary>
/// Handler for GetAllEmploymentQuery
/// </summary>
public class GetAllEmploymentQueryHandler : IRequestHandler<GetAllEmploymentQuery, List<EmploymentDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllEmploymentQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<EmploymentDto>> Handle(GetAllEmploymentQuery request, CancellationToken cancellationToken)
    {
        return await _context.EmploymentHistory
            .Where(e => !e.IsDeleted)
            .OrderBy(e => e.DisplayOrder)
            .ProjectTo<EmploymentDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}
