using MediatR;

namespace Application.Contact.Commands.DeleteContactSubmission;

/// <summary>
/// Command to delete a contact submission
/// </summary>
public class DeleteContactSubmissionCommand : IRequest<bool>
{
    public int Id { get; set; }
}
