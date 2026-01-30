using Application.Common.Interfaces;
using MediatR;
using System.Text.Json;

namespace Application.Employment.Commands.CreateEmployment;

/// <summary>
/// Handler for creating a new employment entry
/// </summary>
public class CreateEmploymentCommandHandler : IRequestHandler<CreateEmploymentCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateEmploymentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateEmploymentCommand request, CancellationToken cancellationToken)
    {
        // Validate required fields
        if (string.IsNullOrWhiteSpace(request.CompanyName))
        {
            throw new ArgumentException("Company name is required");
        }

        if (string.IsNullOrWhiteSpace(request.JobTitle))
        {
            throw new ArgumentException("Job title is required");
        }

        // Create the employment entity
        var employment = new Domain.Entities.Employment
        {
            CompanyName = request.CompanyName,
            JobTitle = request.JobTitle,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            Responsibilities = JsonSerializer.Serialize(request.Responsibilities),
            Achievements = JsonSerializer.Serialize(request.Achievements),
            Technologies = JsonSerializer.Serialize(request.Technologies),
            DisplayOrder = request.DisplayOrder,
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow
        };

        _context.EmploymentHistory.Add(employment);
        await _context.SaveChangesAsync(cancellationToken);

        return employment.Id;
    }
}
