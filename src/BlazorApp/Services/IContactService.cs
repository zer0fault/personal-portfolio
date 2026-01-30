namespace BlazorApp.Services;

public interface IContactService
{
    Task<bool> SubmitContactAsync(Application.Contact.Commands.ContactSubmissionDto submission);

    // Admin methods
    Task<List<Application.Contact.Queries.DTOs.ContactSubmissionDto>> GetAllContactSubmissionsAsync();
    Task<Application.Contact.Queries.DTOs.ContactSubmissionDto?> GetContactSubmissionByIdAsync(int id);
    Task<bool> MarkAsReadAsync(int id, bool isRead);
    Task<bool> DeleteSubmissionAsync(int id);
}
