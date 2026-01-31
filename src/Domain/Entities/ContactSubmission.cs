using Domain.Common;

namespace Domain.Entities;

/// <summary>
/// Represents a contact form submission
/// </summary>
public class ContactSubmission : BaseEntity
{
    /// <summary>
    /// Name of the person submitting the form
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Email address of the person submitting the form
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Optional subject line
    /// </summary>
    public string? Subject { get; set; }

    /// <summary>
    /// Message content
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Indicates whether the submission has been read by admin
    /// </summary>
    public bool IsRead { get; set; }

    /// <summary>
    /// Date and time when the form was submitted (UTC)
    /// </summary>
    public DateTime SubmittedDate { get; set; }
}
