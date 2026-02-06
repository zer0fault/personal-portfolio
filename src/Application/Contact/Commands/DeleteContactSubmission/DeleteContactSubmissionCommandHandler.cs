using MediatR;

namespace Application.Contact.Commands.DeleteContactSubmission;

/// <summary>
/// Handler for deleting a contact submission
/// </summary>
public class DeleteContactSubmissionCommandHandler : IRequestHandler<DeleteContactSubmissionCommand, bool>
{
    public async Task<bool> Handle(DeleteContactSubmissionCommand request, CancellationToken cancellationToken)
    {
        // Note: Data is hardcoded - this command does not persist changes

        // Return success for API compatibility
        await Task.CompletedTask;
        return true;
    }
}
