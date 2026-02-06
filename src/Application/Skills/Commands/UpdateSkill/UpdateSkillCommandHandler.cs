using MediatR;

namespace Application.Skills.Commands.UpdateSkill;

/// <summary>
/// Handler for updating an existing skill
/// </summary>
public class UpdateSkillCommandHandler : IRequestHandler<UpdateSkillCommand, bool>
{
    public async Task<bool> Handle(UpdateSkillCommand request, CancellationToken cancellationToken)
    {
        // Note: Data is hardcoded - this command does not persist changes

        // Validate required fields
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            throw new ArgumentException("Skill name is required");
        }

        // Return success for API compatibility
        await Task.CompletedTask;
        return true;
    }
}
