using Application.Common.Data;
using Application.Projects.Queries.DTOs;
using AutoMapper;
using Domain.Enums;
using MediatR;

namespace Application.Projects.Queries.GetProjectById;

/// <summary>
/// Handler for GetProjectByIdQuery
/// </summary>
public class GetProjectByIdQueryHandler : IRequestHandler<GetProjectByIdQuery, ProjectDetailDto?>
{
    private readonly IMapper _mapper;

    public GetProjectByIdQueryHandler(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<ProjectDetailDto?> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
    {
        var project = StaticDataProvider.GetProjects()
            .FirstOrDefault(p => p.Id == request.Id && !p.IsDeleted && p.Status == ProjectStatus.Published);

        return await Task.FromResult(project == null ? null : _mapper.Map<ProjectDetailDto>(project));
    }
}
