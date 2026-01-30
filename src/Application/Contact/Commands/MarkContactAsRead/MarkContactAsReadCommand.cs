using MediatR;

namespace Application.Contact.Commands.MarkContactAsRead;

/// <summary>
/// Command to mark a contact submission as read
/// </summary>
public class MarkContactAsReadCommand : IRequest<bool>
{
    public int Id { get; set; }
    public bool IsRead { get; set; }
}
