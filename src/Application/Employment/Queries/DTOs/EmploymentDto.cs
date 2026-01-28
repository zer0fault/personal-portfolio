namespace Application.Employment.Queries.DTOs;

/// <summary>
/// DTO for employment history
/// </summary>
public class EmploymentDto
{
    public int Id { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public string JobTitle { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public List<string> Responsibilities { get; set; } = new();
    public List<string> Achievements { get; set; } = new();
    public List<string> Technologies { get; set; } = new();
    public int DisplayOrder { get; set; }
}
