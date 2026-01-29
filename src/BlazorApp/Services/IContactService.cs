using Application.Contact.Commands;

namespace BlazorApp.Services;

public interface IContactService
{
    Task<bool> SubmitContactAsync(ContactSubmissionDto submission);
}
