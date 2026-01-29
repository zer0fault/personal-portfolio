using Application.Employment.Queries.DTOs;
using System.Net.Http.Json;

namespace BlazorApp.Services;

public class EmploymentService : IEmploymentService
{
    private readonly HttpClient _httpClient;

    public EmploymentService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<EmploymentDto>> GetAllEmploymentAsync()
    {
        try
        {
            var employment = await _httpClient.GetFromJsonAsync<List<EmploymentDto>>("api/employment");
            return employment ?? new List<EmploymentDto>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching employment: {ex.Message}");
            return new List<EmploymentDto>();
        }
    }
}
