namespace Application.Settings.Queries.DTOs;

/// <summary>
/// DTO for settings
/// </summary>
public class SettingsDto
{
    public int Id { get; set; }
    public string Key { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public DateTime LastModified { get; set; }
}
