using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Contact.Commands.MarkContactAsRead;

/// <summary>
/// Handler for marking a contact submission as read/unread
/// </summary>
public class MarkContactAsReadCommandHandler : IRequestHandler<MarkContactAsReadCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public MarkContactAsReadCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(MarkContactAsReadCommand request, CancellationToken cancellationToken)
    {
        var submission = await _context.ContactSubmissions
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (submission == null)
        {
            throw new KeyNotFoundException("Contact submission not found");
        }

        submission.IsRead = request.IsRead;
        submission.ModifiedDate = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
