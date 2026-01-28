using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for Project entity
/// </summary>
public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.ToTable("Projects");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.ShortDescription)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(p => p.FullDescription)
            .IsRequired();

        builder.Property(p => p.Technologies)
            .IsRequired()
            .HasMaxLength(2000)
            .HasDefaultValue("[]");

        builder.Property(p => p.GitHubUrl)
            .HasMaxLength(500);

        builder.Property(p => p.LiveDemoUrl)
            .HasMaxLength(500);

        builder.Property(p => p.DisplayOrder)
            .IsRequired();

        builder.Property(p => p.Status)
            .IsRequired();

        builder.Property(p => p.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(p => p.CreatedDate)
            .IsRequired();

        builder.Property(p => p.ModifiedDate)
            .IsRequired();

        // Relationships
        builder.HasMany(p => p.Images)
            .WithOne(i => i.Project)
            .HasForeignKey(i => i.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        builder.HasIndex(p => p.Status);
        builder.HasIndex(p => p.IsDeleted);
        builder.HasIndex(p => p.DisplayOrder);
    }
}
