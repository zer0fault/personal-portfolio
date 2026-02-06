using Application.Common.Data;
using Application.Employment.Queries.DTOs;
using AutoMapper;

namespace BlazorApp.Services.Static;

public class StaticEmploymentService : IEmploymentService
{
    private readonly IMapper _mapper;

    public StaticEmploymentService(IMapper mapper)
    {
        _mapper = mapper;
    }

    public Task<List<EmploymentDto>> GetAllEmploymentAsync()
    {
        var employment = StaticDataProvider.GetEmployment()
            .Where(e => !e.IsDeleted)
            .OrderBy(e => e.DisplayOrder)
            .ToList();

        return Task.FromResult(_mapper.Map<List<EmploymentDto>>(employment));
    }


    public Task<EmploymentDto?> GetEmploymentByIdAsync(int id)
    {
        var employment = StaticDataProvider.GetEmployment()
            .FirstOrDefault(e => e.Id == id && !e.IsDeleted);

        if (employment == null)
            return Task.FromResult<EmploymentDto?>(null);

        return Task.FromResult<EmploymentDto?>(_mapper.Map<EmploymentDto>(employment));
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
