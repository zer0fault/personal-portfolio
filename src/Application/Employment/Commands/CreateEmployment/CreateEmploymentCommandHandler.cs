using MediatR;

namespace Application.Employment.Commands.CreateEmployment;

/// <summary>
/// Handler for creating a new employment entry
/// </summary>
public class CreateEmploymentCommandHandler : IRequestHandler<CreateEmploymentCommand, int>
{
    public async Task<int> Handle(CreateEmploymentCommand request, CancellationToken cancellationToken)
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

        // Return fake ID for API compatibility
        await Task.CompletedTask;
        return 999;
    }
}
