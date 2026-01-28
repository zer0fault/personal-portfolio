using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for ProjectImage entity
/// </summary>
public class ProjectImageConfiguration : IEntityTypeConfiguration<ProjectImage>
{
    public void Configure(EntityTypeBuilder<ProjectImage> builder)
    {
        builder.ToTable("ProjectImages");

        builder.HasKey(pi => pi.Id);

        builder.Property(pi => pi.ImagePath)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(pi => pi.AltText)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(pi => pi.DisplayOrder)
            .IsRequired();

        builder.Property(pi => pi.IsThumbnail)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(pi => pi.CreatedDate)
            .IsRequired();

        builder.Property(pi => pi.ModifiedDate)
            .IsRequired();

        // Indexes
        builder.HasIndex(pi => pi.ProjectId);
        builder.HasIndex(pi => new { pi.ProjectId, pi.IsThumbnail });
    }
}
