using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Employment.Commands.DeleteEmployment;

/// <summary>
/// Handler for soft deleting an employment entry
/// </summary>
public class DeleteEmploymentCommandHandler : IRequestHandler<DeleteEmploymentCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public DeleteEmploymentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(DeleteEmploymentCommand request, CancellationToken cancellationToken)
    {
        var employment = await _context.EmploymentHistory
            .FirstOrDefaultAsync(e => e.Id == request.Id && !e.IsDeleted, cancellationToken);

        if (employment == null)
        {
            throw new KeyNotFoundException("Employment entry not found");
        }

        // Soft delete
        employment.IsDeleted = true;
        employment.ModifiedDate = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
