using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for Skill entity
/// </summary>
public class SkillConfiguration : IEntityTypeConfiguration<Skill>
{
    public void Configure(EntityTypeBuilder<Skill> builder)
    {
        builder.ToTable("Skills");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(s => s.Category)
            .IsRequired();

        builder.Property(s => s.ProficiencyLevel)
            .IsRequired();

        builder.Property(s => s.DisplayOrder)
            .IsRequired();

        builder.Property(s => s.IconUrl)
            .HasMaxLength(500);

        builder.Property(s => s.CreatedDate)
            .IsRequired();

        builder.Property(s => s.ModifiedDate)
            .IsRequired();

        // Indexes
        builder.HasIndex(s => s.Category);
        builder.HasIndex(s => s.DisplayOrder);
        builder.HasIndex(s => new { s.Category, s.DisplayOrder });
    }
}
