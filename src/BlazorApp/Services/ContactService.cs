using Application.Contact.Commands;
using System.Net.Http.Json;

namespace BlazorApp.Services;

public class ContactService : IContactService
{
    private readonly HttpClient _httpClient;

    public ContactService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> SubmitContactAsync(ContactSubmissionDto submission)
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
}
