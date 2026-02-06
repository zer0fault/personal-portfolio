using MediatR;

namespace Application.Skills.Commands.CreateSkill;

/// <summary>
/// Handler for creating a new skill
/// </summary>
public class CreateSkillCommandHandler : IRequestHandler<CreateSkillCommand, int>
{
    public async Task<int> Handle(CreateSkillCommand request, CancellationToken cancellationToken)
    {
        // Note: Data is hardcoded - this command does not persist changes

        // Validate required fields
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            throw new ArgumentException("Skill name is required");
        }

        // Return fake ID for API compatibility
        await Task.CompletedTask;
        return 999;
    }
}
