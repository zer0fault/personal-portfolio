namespace Domain.Common;

/// <summary>
/// Interface for entities that support soft delete functionality
/// </summary>
public interface ISoftDeletable
{
    /// <summary>
    /// Indicates whether the entity has been soft deleted
    /// </summary>
    bool IsDeleted { get; set; }
}
