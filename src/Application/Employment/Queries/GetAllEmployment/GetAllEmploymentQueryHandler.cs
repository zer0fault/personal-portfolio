using Application.Common.Data;
using Application.Employment.Queries.DTOs;
using MediatR;

namespace Application.Employment.Queries.GetAllEmployment;

/// <summary>
/// Handler for GetAllEmploymentQuery
/// </summary>
public class GetAllEmploymentQueryHandler : IRequestHandler<GetAllEmploymentQuery, List<EmploymentDto>>
{
    public async Task<List<EmploymentDto>> Handle(GetAllEmploymentQuery request, CancellationToken cancellationToken)
    {
        var employmentData = StaticDataProvider.GetEmploymentData();
        var dtos = new List<EmploymentDto>();
        var currentId = 1;

        foreach (var emp in employmentData)
        {
            dtos.Add(new EmploymentDto
            {
                Id = currentId++,
                CompanyName = emp.CompanyName,
                JobTitle = emp.JobTitle,
                StartDate = emp.StartDate,
                EndDate = emp.EndDate,
                Responsibilities = emp.Responsibilities,
                Achievements = emp.Achievements,
                Technologies = emp.Technologies,
                DisplayOrder = currentId - 1
            });
        }

        return await Task.FromResult(dtos);
    }
}
