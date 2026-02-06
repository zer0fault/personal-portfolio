using MediatR;

namespace Application.Employment.Commands.DeleteEmployment;

/// <summary>
/// Handler for soft deleting an employment entry
/// </summary>
public class DeleteEmploymentCommandHandler : IRequestHandler<DeleteEmploymentCommand, bool>
{
    public async Task<bool> Handle(DeleteEmploymentCommand request, CancellationToken cancellationToken)
    {
        // Note: Data is hardcoded - this command does not persist changes

        // Return success for API compatibility
        await Task.CompletedTask;
        return true;
    }
}
