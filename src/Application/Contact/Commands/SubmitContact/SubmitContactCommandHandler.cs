using MediatR;

namespace Application.Contact.Commands.SubmitContact;

/// <summary>
/// Handler for SubmitContactCommand
/// </summary>
public class SubmitContactCommandHandler : IRequestHandler<SubmitContactCommand, int>
{
    public async Task<int> Handle(SubmitContactCommand request, CancellationToken cancellationToken)
    {
        // Note: Data is hardcoded - this command does not persist changes

        // Return fake ID for API compatibility
        await Task.CompletedTask;
        return 999;
    }
}
