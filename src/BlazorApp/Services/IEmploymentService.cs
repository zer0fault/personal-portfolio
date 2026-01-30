using Application.Employment.Queries.DTOs;
using Application.Employment.Commands.CreateEmployment;
using Application.Employment.Commands.UpdateEmployment;

namespace BlazorApp.Services;

public interface IEmploymentService
{
    Task<List<EmploymentDto>> GetAllEmploymentAsync();
    Task<EmploymentDto?> GetEmploymentByIdAsync(int id);
    Task<int?> CreateEmploymentAsync(CreateEmploymentCommand command);
    Task<bool> UpdateEmploymentAsync(UpdateEmploymentCommand command);
    Task<bool> DeleteEmploymentAsync(int id);
}
