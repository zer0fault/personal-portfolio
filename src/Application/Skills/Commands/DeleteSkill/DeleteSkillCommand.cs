using MediatR;

namespace Application.Skills.Commands.DeleteSkill;

/// <summary>
/// Command to soft delete a skill
/// </summary>
public class DeleteSkillCommand : IRequest<bool>
{
    public int Id { get; set; }
}
