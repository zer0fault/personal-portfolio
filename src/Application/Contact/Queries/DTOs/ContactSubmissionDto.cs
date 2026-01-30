namespace Application.Contact.Queries.DTOs;

/// <summary>
/// DTO for contact submission display (admin view)
/// </summary>
public class ContactSubmissionDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Subject { get; set; }
    public string Message { get; set; } = string.Empty;
    public bool IsRead { get; set; }
    public DateTime SubmittedDate { get; set; }
}
