using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Contact.Commands.SubmitContact;

/// <summary>
/// Handler for SubmitContactCommand
/// </summary>
public class SubmitContactCommandHandler : IRequestHandler<SubmitContactCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly IDateTime _dateTime;

    public SubmitContactCommandHandler(IApplicationDbContext context, IDateTime dateTime)
    {
        _context = context;
        _dateTime = dateTime;
    }

    public async Task<int> Handle(SubmitContactCommand request, CancellationToken cancellationToken)
    {
        var submission = new ContactSubmission
        {
            Name = request.Name,
            Email = request.Email,
            Subject = request.Subject,
            Message = request.Message,
            SubmittedDate = _dateTime.UtcNow,
            IsRead = false,
            CreatedDate = _dateTime.UtcNow,
            ModifiedDate = _dateTime.UtcNow
        };

        _context.ContactSubmissions.Add(submission);
        await _context.SaveChangesAsync(cancellationToken);

        return submission.Id;
    }
}
