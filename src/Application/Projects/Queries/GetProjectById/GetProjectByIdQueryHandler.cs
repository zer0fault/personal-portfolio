using Application.Common.Interfaces;
using Application.Projects.Queries.DTOs;
using AutoMapper;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Projects.Queries.GetProjectById;

/// <summary>
/// Handler for GetProjectByIdQuery
/// </summary>
public class GetProjectByIdQueryHandler : IRequestHandler<GetProjectByIdQuery, ProjectDetailDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetProjectByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ProjectDetailDto?> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
    {
        var project = await _context.Projects
            .Include(p => p.Images)
            .Where(p => p.Id == request.Id && !p.IsDeleted && p.Status == ProjectStatus.Published)
            .FirstOrDefaultAsync(cancellationToken);

        return project == null ? null : _mapper.Map<ProjectDetailDto>(project);
    }
}
