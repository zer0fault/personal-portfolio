using Application.Common.Interfaces;
using Application.Projects.Queries.DTOs;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Projects.Queries.GetAllProjectsForAdmin;

/// <summary>
/// Handler for GetAllProjectsForAdminQuery - returns all projects regardless of status
/// </summary>
public class GetAllProjectsForAdminQueryHandler : IRequestHandler<GetAllProjectsForAdminQuery, List<ProjectDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllProjectsForAdminQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ProjectDto>> Handle(GetAllProjectsForAdminQuery request, CancellationToken cancellationToken)
    {
        return await _context.Projects
            .Include(p => p.Images)
            .Where(p => !p.IsDeleted)
            .OrderBy(p => p.DisplayOrder)
            .ProjectTo<ProjectDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}
