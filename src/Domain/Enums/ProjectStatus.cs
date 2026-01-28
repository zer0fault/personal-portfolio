namespace Domain.Enums;

/// <summary>
/// Status of a portfolio project
/// </summary>
public enum ProjectStatus
{
    /// <summary>
    /// Project is in draft and not visible to public
    /// </summary>
    Draft = 0,

    /// <summary>
    /// Project is published and visible to public
    /// </summary>
    Published = 1,

    /// <summary>
    /// Project is archived and hidden from public view
    /// </summary>
    Archived = 2
}
