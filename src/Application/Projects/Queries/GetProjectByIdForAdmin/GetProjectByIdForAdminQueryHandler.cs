using Application.Common.Interfaces;
using Application.Projects.Queries.DTOs;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Projects.Queries.GetProjectByIdForAdmin;

/// <summary>
/// Handler for GetProjectByIdForAdminQuery - returns project regardless of status
/// </summary>
public class GetProjectByIdForAdminQueryHandler : IRequestHandler<GetProjectByIdForAdminQuery, ProjectDetailDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetProjectByIdForAdminQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ProjectDetailDto?> Handle(GetProjectByIdForAdminQuery request, CancellationToken cancellationToken)
    {
        var project = await _context.Projects
            .Include(p => p.Images)
            .Where(p => p.Id == request.Id && !p.IsDeleted)
            .FirstOrDefaultAsync(cancellationToken);

        return project == null ? null : _mapper.Map<ProjectDetailDto>(project);
    }
}
