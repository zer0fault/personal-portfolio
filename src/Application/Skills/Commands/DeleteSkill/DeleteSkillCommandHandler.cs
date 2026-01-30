using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Skills.Commands.DeleteSkill;

/// <summary>
/// Handler for soft deleting a skill
/// </summary>
public class DeleteSkillCommandHandler : IRequestHandler<DeleteSkillCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public DeleteSkillCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(DeleteSkillCommand request, CancellationToken cancellationToken)
    {
        var skill = await _context.Skills
            .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);

        if (skill == null)
        {
            throw new KeyNotFoundException("Skill not found");
        }

        // Hard delete (since Skill doesn't implement ISoftDeletable)
        _context.Skills.Remove(skill);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
