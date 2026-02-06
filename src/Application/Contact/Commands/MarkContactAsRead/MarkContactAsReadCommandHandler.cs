using MediatR;

namespace Application.Contact.Commands.MarkContactAsRead;

/// <summary>
/// Handler for marking a contact submission as read/unread
/// </summary>
public class MarkContactAsReadCommandHandler : IRequestHandler<MarkContactAsReadCommand, bool>
{
    public async Task<bool> Handle(MarkContactAsReadCommand request, CancellationToken cancellationToken)
    {
        // Note: Data is hardcoded - this command does not persist changes

        // Return success for API compatibility
        await Task.CompletedTask;
        return true;
    }
}
