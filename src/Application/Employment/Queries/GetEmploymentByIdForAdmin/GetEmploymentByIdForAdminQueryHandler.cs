using Application.Common.Data;
using Application.Employment.Queries.DTOs;
using MediatR;

namespace Application.Employment.Queries.GetEmploymentByIdForAdmin;

/// <summary>
/// Handler for GetEmploymentByIdForAdminQuery
/// </summary>
public class GetEmploymentByIdForAdminQueryHandler : IRequestHandler<GetEmploymentByIdForAdminQuery, EmploymentDto?>
{
    public async Task<EmploymentDto?> Handle(GetEmploymentByIdForAdminQuery request, CancellationToken cancellationToken)
    {
        var employmentData = StaticDataProvider.GetEmploymentData();

        if (request.Id < 1 || request.Id > employmentData.Count)
            return await Task.FromResult<EmploymentDto?>(null);

        var emp = employmentData[request.Id - 1];

        var dto = new EmploymentDto
        {
            Id = request.Id,
            CompanyName = emp.CompanyName,
            JobTitle = emp.JobTitle,
            StartDate = emp.StartDate,
            EndDate = emp.EndDate,
            Responsibilities = emp.Responsibilities,
            Achievements = emp.Achievements,
            Technologies = emp.Technologies,
            DisplayOrder = request.Id
        };

        return await Task.FromResult<EmploymentDto?>(dto);
    }
}
