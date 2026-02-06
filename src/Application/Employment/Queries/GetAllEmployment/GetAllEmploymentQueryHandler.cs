using Application.Common.Data;
using Application.Employment.Queries.DTOs;
using AutoMapper;
using MediatR;

namespace Application.Employment.Queries.GetAllEmployment;

/// <summary>
/// Handler for GetAllEmploymentQuery
/// </summary>
public class GetAllEmploymentQueryHandler : IRequestHandler<GetAllEmploymentQuery, List<EmploymentDto>>
{
    private readonly IMapper _mapper;

    public GetAllEmploymentQueryHandler(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<List<EmploymentDto>> Handle(GetAllEmploymentQuery request, CancellationToken cancellationToken)
    {
        var employment = StaticDataProvider.GetEmployment()
            .Where(e => !e.IsDeleted)
            .OrderBy(e => e.DisplayOrder)
            .ToList();

        return await Task.FromResult(_mapper.Map<List<EmploymentDto>>(employment));
    }
}
