using Application.Common.Data;
using Application.Projects.Queries.DTOs;
using AutoMapper;
using Domain.Enums;
using MediatR;

namespace Application.Projects.Queries.GetAllProjects;

/// <summary>
/// Handler for GetAllProjectsQuery
/// </summary>
public class GetAllProjectsQueryHandler : IRequestHandler<GetAllProjectsQuery, List<ProjectDto>>
{
    private readonly IMapper _mapper;

    public GetAllProjectsQueryHandler(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<List<ProjectDto>> Handle(GetAllProjectsQuery request, CancellationToken cancellationToken)
    {
        var projects = StaticDataProvider.GetProjects()
            .Where(p => !p.IsDeleted && p.Status == ProjectStatus.Published)
            .OrderBy(p => p.DisplayOrder)
            .ToList();

        return await Task.FromResult(_mapper.Map<List<ProjectDto>>(projects));
    }
}
