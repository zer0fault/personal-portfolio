using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Contact.Commands.DeleteContactSubmission;

/// <summary>
/// Handler for deleting a contact submission
/// </summary>
public class DeleteContactSubmissionCommandHandler : IRequestHandler<DeleteContactSubmissionCommand, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<DeleteContactSubmissionCommandHandler> _logger;

    public DeleteContactSubmissionCommandHandler(
        IApplicationDbContext context,
        ILogger<DeleteContactSubmissionCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<bool> Handle(DeleteContactSubmissionCommand request, CancellationToken cancellationToken)
    {
        var submission = await _context.ContactSubmissions
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (submission == null)
        {
            throw new KeyNotFoundException("Contact submission not found");
        }

        // Log deletion for audit trail (without PII)
        _logger.LogInformation("Deleting contact submission {SubmissionId} from {SubmittedDate}",
            request.Id, submission.SubmittedDate);

        _context.ContactSubmissions.Remove(submission);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
