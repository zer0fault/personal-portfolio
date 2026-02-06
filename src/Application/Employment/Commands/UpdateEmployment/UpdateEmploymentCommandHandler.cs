using MediatR;

namespace Application.Employment.Commands.UpdateEmployment;

/// <summary>
/// Handler for updating an existing employment entry
/// </summary>
public class UpdateEmploymentCommandHandler : IRequestHandler<UpdateEmploymentCommand, bool>
{
    public async Task<bool> Handle(UpdateEmploymentCommand request, CancellationToken cancellationToken)
    {
        // Note: Data is hardcoded - this command does not persist changes

        // Validate required fields
        if (string.IsNullOrWhiteSpace(request.CompanyName))
        {
            throw new ArgumentException("Company name is required");
        }

        if (string.IsNullOrWhiteSpace(request.JobTitle))
        {
            throw new ArgumentException("Job title is required");
        }

        // Return success for API compatibility
        await Task.CompletedTask;
        return true;
    }
}
