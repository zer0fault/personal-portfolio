using MediatR;

namespace Application.Contact.Commands.SubmitContact;

/// <summary>
/// Command to submit a contact form
/// </summary>
public record SubmitContactCommand(
    string Name,
    string Email,
    string Subject,
    string Message
) : IRequest<int>;
