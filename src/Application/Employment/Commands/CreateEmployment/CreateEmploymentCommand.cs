using MediatR;

namespace Application.Employment.Commands.CreateEmployment;

/// <summary>
/// Command to create a new employment entry
/// </summary>
public class CreateEmploymentCommand : IRequest<int>
{
    public string CompanyName { get; set; } = string.Empty;
    public string JobTitle { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public List<string> Responsibilities { get; set; } = new();
    public List<string> Achievements { get; set; } = new();
    public List<string> Technologies { get; set; } = new();
    public int DisplayOrder { get; set; }
}
