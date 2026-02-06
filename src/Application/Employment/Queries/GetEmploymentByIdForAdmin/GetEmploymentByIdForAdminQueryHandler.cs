using Application.Common.Data;
using Application.Employment.Queries.DTOs;
using AutoMapper;
using MediatR;

namespace Application.Employment.Queries.GetEmploymentByIdForAdmin;

/// <summary>
/// Handler for GetEmploymentByIdForAdminQuery
/// </summary>
public class GetEmploymentByIdForAdminQueryHandler : IRequestHandler<GetEmploymentByIdForAdminQuery, EmploymentDto?>
{
    private readonly IMapper _mapper;

    public GetEmploymentByIdForAdminQueryHandler(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<EmploymentDto?> Handle(GetEmploymentByIdForAdminQuery request, CancellationToken cancellationToken)
    {
        var employment = StaticDataProvider.GetEmployment()
            .FirstOrDefault(e => e.Id == request.Id && !e.IsDeleted);

        return await Task.FromResult(employment == null ? null : _mapper.Map<EmploymentDto>(employment));
    }
}
