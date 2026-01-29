using Application.Employment.Queries.DTOs;

namespace BlazorApp.Services;

public interface IEmploymentService
{
    Task<List<EmploymentDto>> GetAllEmploymentAsync();
}
