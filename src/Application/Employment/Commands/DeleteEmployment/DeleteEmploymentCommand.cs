using MediatR;

namespace Application.Employment.Commands.DeleteEmployment;

/// <summary>
/// Command to soft delete an employment entry
/// </summary>
public class DeleteEmploymentCommand : IRequest<bool>
{
    public int Id { get; set; }
}
