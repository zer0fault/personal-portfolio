using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces;

/// <summary>
/// Interface for the application database context
/// </summary>
public interface IApplicationDbContext
{
    /// <summary>
    /// DbSet for Project entities
    /// </summary>
    DbSet<Project> Projects { get; }

    /// <summary>
    /// DbSet for ProjectImage entities
    /// </summary>
    DbSet<ProjectImage> ProjectImages { get; }

    /// <summary>
    /// DbSet for Employment entities
    /// </summary>
    DbSet<Domain.Entities.Employment> EmploymentHistory { get; }

    /// <summary>
    /// DbSet for ContactSubmission entities
    /// </summary>
    DbSet<ContactSubmission> ContactSubmissions { get; }

    /// <summary>
    /// DbSet for Skill entities
    /// </summary>
    DbSet<Skill> Skills { get; }

    /// <summary>
    /// DbSet for Settings entities
    /// </summary>
    DbSet<Settings> Settings { get; }

    /// <summary>
    /// Saves changes asynchronously
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Number of state entries written to the database</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
