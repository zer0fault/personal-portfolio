using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for Employment entity
/// </summary>
public class EmploymentConfiguration : IEntityTypeConfiguration<Employment>
{
    public void Configure(EntityTypeBuilder<Employment> builder)
    {
        builder.ToTable("Employment");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.CompanyName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.JobTitle)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.StartDate)
            .IsRequired();

        builder.Property(e => e.EndDate);

        builder.Property(e => e.Responsibilities)
            .IsRequired()
            .HasMaxLength(4000)
            .HasDefaultValue("[]");

        builder.Property(e => e.Achievements)
            .IsRequired()
            .HasMaxLength(4000)
            .HasDefaultValue("[]");

        builder.Property(e => e.Technologies)
            .IsRequired()
            .HasMaxLength(2000)
            .HasDefaultValue("[]");

        builder.Property(e => e.DisplayOrder)
            .IsRequired();

        builder.Property(e => e.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(e => e.CreatedDate)
            .IsRequired();

        builder.Property(e => e.ModifiedDate)
            .IsRequired();

        // Indexes
        builder.HasIndex(e => e.IsDeleted);
        builder.HasIndex(e => e.DisplayOrder);
        builder.HasIndex(e => e.StartDate);
    }
}
