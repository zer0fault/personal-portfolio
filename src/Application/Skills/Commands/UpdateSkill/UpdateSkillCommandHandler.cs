using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Skills.Commands.UpdateSkill;

/// <summary>
/// Handler for updating an existing skill
/// </summary>
public class UpdateSkillCommandHandler : IRequestHandler<UpdateSkillCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public UpdateSkillCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UpdateSkillCommand request, CancellationToken cancellationToken)
    {
        // Validate required fields
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            throw new ArgumentException("Skill name is required");
        }

        // Find the skill
        var skill = await _context.Skills
            .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);

        if (skill == null)
        {
            throw new KeyNotFoundException("Skill not found");
        }

        // Update the skill
        skill.Name = request.Name;
        skill.Category = request.Category;
        skill.DisplayOrder = request.DisplayOrder;
        skill.IconUrl = request.IconUrl;
        skill.ModifiedDate = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
