using Application.Employment.Queries.DTOs;
using Application.Employment.Commands.CreateEmployment;
using Application.Employment.Commands.UpdateEmployment;
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

    public async Task<EmploymentDto?> GetEmploymentByIdAsync(int id)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<EmploymentDto>($"api/employment/{id}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching employment {id}: {ex.Message}");
            return null;
        }
    }

    public async Task<int?> CreateEmploymentAsync(CreateEmploymentCommand command)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/employment", command);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<CreateEmploymentResponse>();
                return result?.Id;
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Error creating employment: {response.StatusCode} - {errorContent}");
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating employment: {ex.Message}");
            return null;
        }
    }

    public async Task<bool> UpdateEmploymentAsync(UpdateEmploymentCommand command)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"api/employment/{command.Id}", command);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Error updating employment: {response.StatusCode} - {errorContent}");
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating employment: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> DeleteEmploymentAsync(int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"api/employment/{id}");
            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Error deleting employment: {response.StatusCode} - {errorContent}");
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting employment: {ex.Message}");
            return false;
        }
    }

    private class CreateEmploymentResponse
    {
        public int Id { get; set; }
    }
}
