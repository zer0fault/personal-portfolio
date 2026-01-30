using System.Net.Http.Json;

namespace BlazorApp.Services;

public class ContactService : IContactService
{
    private readonly HttpClient _httpClient;

    public ContactService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> SubmitContactAsync(Application.Contact.Commands.ContactSubmissionDto submission)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/contact", submission);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error submitting contact form: {ex.Message}");
            return false;
        }
    }

    // Admin methods
    public async Task<List<Application.Contact.Queries.DTOs.ContactSubmissionDto>> GetAllContactSubmissionsAsync()
    {
        try
        {
            var submissions = await _httpClient.GetFromJsonAsync<List<Application.Contact.Queries.DTOs.ContactSubmissionDto>>("api/contact");
            return submissions ?? new List<Application.Contact.Queries.DTOs.ContactSubmissionDto>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching contact submissions: {ex.Message}");
            return new List<Application.Contact.Queries.DTOs.ContactSubmissionDto>();
        }
    }

    public async Task<Application.Contact.Queries.DTOs.ContactSubmissionDto?> GetContactSubmissionByIdAsync(int id)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<Application.Contact.Queries.DTOs.ContactSubmissionDto>($"api/contact/{id}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching contact submission {id}: {ex.Message}");
            return null;
        }
    }

    public async Task<bool> MarkAsReadAsync(int id, bool isRead)
    {
        try
        {
            var response = await _httpClient.PatchAsJsonAsync($"api/contact/{id}", new { isRead });
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error marking contact submission {id} as read: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> DeleteSubmissionAsync(int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"api/contact/{id}");
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting contact submission {id}: {ex.Message}");
            return false;
        }
    }
}
