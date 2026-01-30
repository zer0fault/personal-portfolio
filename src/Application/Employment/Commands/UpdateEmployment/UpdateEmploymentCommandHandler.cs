using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Application.Employment.Commands.UpdateEmployment;

/// <summary>
/// Handler for updating an existing employment entry
/// </summary>
public class UpdateEmploymentCommandHandler : IRequestHandler<UpdateEmploymentCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public UpdateEmploymentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UpdateEmploymentCommand request, CancellationToken cancellationToken)
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

        // Find the employment entry
        var employment = await _context.EmploymentHistory
            .FirstOrDefaultAsync(e => e.Id == request.Id && !e.IsDeleted, cancellationToken);

        if (employment == null)
        {
            throw new KeyNotFoundException("Employment entry not found");
        }

        // Update the employment entry
        employment.CompanyName = request.CompanyName;
        employment.JobTitle = request.JobTitle;
        employment.StartDate = request.StartDate;
        employment.EndDate = request.EndDate;
        employment.Responsibilities = JsonSerializer.Serialize(request.Responsibilities);
        employment.Achievements = JsonSerializer.Serialize(request.Achievements);
        employment.Technologies = JsonSerializer.Serialize(request.Technologies);
        employment.DisplayOrder = request.DisplayOrder;
        employment.ModifiedDate = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
