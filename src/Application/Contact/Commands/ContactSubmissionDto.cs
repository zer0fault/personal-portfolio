namespace Application.Contact.Commands;

/// <summary>
/// DTO for contact form submission
/// </summary>
public class ContactSubmissionDto
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}
