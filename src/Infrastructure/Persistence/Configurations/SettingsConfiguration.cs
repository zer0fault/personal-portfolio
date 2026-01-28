using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for Settings entity
/// </summary>
public class SettingsConfiguration : IEntityTypeConfiguration<Settings>
{
    public void Configure(EntityTypeBuilder<Settings> builder)
    {
        builder.ToTable("Settings");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Key)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(s => s.Value)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(s => s.Category)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(s => s.LastModified)
            .IsRequired();

        builder.Property(s => s.CreatedDate)
            .IsRequired();

        builder.Property(s => s.ModifiedDate)
            .IsRequired();

        // Indexes
        builder.HasIndex(s => s.Key)
            .IsUnique();

        builder.HasIndex(s => s.Category);
    }
}
