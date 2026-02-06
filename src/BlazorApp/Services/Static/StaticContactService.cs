using Application.Contact.Commands;
using QueryDto = Application.Contact.Queries.DTOs.ContactSubmissionDto;
using CommandDto = Application.Contact.Commands.ContactSubmissionDto;

namespace BlazorApp.Services.Static;

public class StaticContactService : IContactService
{
    public Task<bool> SubmitContactAsync(CommandDto submission)
    {
        // For static site, contact form can log to console or be disabled
        // In a real scenario, this could integrate with a third-party service like Formspree, EmailJS, etc.
        Console.WriteLine($"Contact form submitted (static mode - not persisted):");
        Console.WriteLine($"Name: {submission.Name}");
        Console.WriteLine($"Email: {submission.Email}");
        Console.WriteLine($"Subject: {submission.Subject}");
        Console.WriteLine($"Message: {submission.Message}");

        // Return true to provide user feedback, but inform them it's not actually sent
        return Task.FromResult(true);
    }

    // Admin methods - not supported in static mode
    public Task<List<QueryDto>> GetAllContactSubmissionsAsync()
    {
        Console.WriteLine("Admin operations are not supported in static mode");
        return Task.FromResult(new List<QueryDto>());
    }

    public Task<QueryDto?> GetContactSubmissionByIdAsync(int id)
    {
        Console.WriteLine("Admin operations are not supported in static mode");
        return Task.FromResult<QueryDto?>(null);
    }

    public Task<bool> MarkAsReadAsync(int id, bool isRead)
    {
        Console.WriteLine("Admin operations are not supported in static mode");
        return Task.FromResult(false);
    }

    public Task<bool> DeleteSubmissionAsync(int id)
    {
        Console.WriteLine("Admin operations are not supported in static mode");
        return Task.FromResult(false);
    }
}
