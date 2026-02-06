using MediatR;

namespace Application.Skills.Commands.DeleteSkill;

/// <summary>
/// Handler for soft deleting a skill
/// </summary>
public class DeleteSkillCommandHandler : IRequestHandler<DeleteSkillCommand, bool>
{
    public async Task<bool> Handle(DeleteSkillCommand request, CancellationToken cancellationToken)
    {
        // Note: Data is hardcoded - this command does not persist changes

        // Return success for API compatibility
        await Task.CompletedTask;
        return true;
    }
}
