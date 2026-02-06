using Application.Common.Data;
using Application.Projects.Queries.DTOs;
using AutoMapper;
using MediatR;

namespace Application.Projects.Queries.GetAllProjectsForAdmin;

/// <summary>
/// Handler for GetAllProjectsForAdminQuery - returns all projects regardless of status
/// </summary>
public class GetAllProjectsForAdminQueryHandler : IRequestHandler<GetAllProjectsForAdminQuery, List<ProjectDto>>
{
    private readonly IMapper _mapper;

    public GetAllProjectsForAdminQueryHandler(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<List<ProjectDto>> Handle(GetAllProjectsForAdminQuery request, CancellationToken cancellationToken)
    {
        var projects = StaticDataProvider.GetProjects()
            .Where(p => !p.IsDeleted)
            .OrderBy(p => p.DisplayOrder)
            .ToList();

        return await Task.FromResult(_mapper.Map<List<ProjectDto>>(projects));
    }
}
