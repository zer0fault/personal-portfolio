using Application.Common.Data;
using Application.Employment.Queries.DTOs;

namespace BlazorApp.Services.Static;

public class StaticEmploymentService : IEmploymentService
{
    public Task<List<EmploymentDto>> GetAllEmploymentAsync()
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

        return Task.FromResult(dtos);
    }

    public Task<EmploymentDto?> GetEmploymentByIdAsync(int id)
    {
        var allEmployment = GetAllEmploymentAsync().Result;
        var employment = allEmployment.FirstOrDefault(e => e.Id == id);
        return Task.FromResult(employment);
    }

    // Admin methods - not supported in static mode
    public Task<int?> CreateEmploymentAsync(Application.Employment.Commands.CreateEmployment.CreateEmploymentCommand command)
    {
        Console.WriteLine("Create operations are not supported in static mode");
        return Task.FromResult<int?>(null);
    }

    public Task<bool> UpdateEmploymentAsync(Application.Employment.Commands.UpdateEmployment.UpdateEmploymentCommand command)
    {
        Console.WriteLine("Update operations are not supported in static mode");
        return Task.FromResult(false);
    }

    public Task<bool> DeleteEmploymentAsync(int id)
    {
        Console.WriteLine("Delete operations are not supported in static mode");
        return Task.FromResult(false);
    }
}
