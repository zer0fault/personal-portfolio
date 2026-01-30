using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Skills.Commands.CreateSkill;

/// <summary>
/// Handler for creating a new skill
/// </summary>
public class CreateSkillCommandHandler : IRequestHandler<CreateSkillCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateSkillCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateSkillCommand request, CancellationToken cancellationToken)
    {
        // Validate required fields
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            throw new ArgumentException("Skill name is required");
        }

        // Create the skill entity
        var skill = new Skill
        {
            Name = request.Name,
            Category = request.Category,
            ProficiencyLevel = Domain.Enums.ProficiencyLevel.Advanced, // Default value, not exposed to user
            DisplayOrder = request.DisplayOrder,
            IconUrl = request.IconUrl,
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow
        };

        _context.Skills.Add(skill);
        await _context.SaveChangesAsync(cancellationToken);

        return skill.Id;
    }
}
