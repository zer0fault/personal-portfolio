using Application.Common.Data;
using Application.Projects.Queries.DTOs;
using AutoMapper;
using MediatR;

namespace Application.Projects.Queries.GetProjectByIdForAdmin;

/// <summary>
/// Handler for GetProjectByIdForAdminQuery - returns project regardless of status
/// </summary>
public class GetProjectByIdForAdminQueryHandler : IRequestHandler<GetProjectByIdForAdminQuery, ProjectDetailDto?>
{
    private readonly IMapper _mapper;

    public GetProjectByIdForAdminQueryHandler(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<ProjectDetailDto?> Handle(GetProjectByIdForAdminQuery request, CancellationToken cancellationToken)
    {
        var project = StaticDataProvider.GetProjects()
            .FirstOrDefault(p => p.Id == request.Id && !p.IsDeleted);

        return await Task.FromResult(project == null ? null : _mapper.Map<ProjectDetailDto>(project));
    }
}
