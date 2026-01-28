using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for ContactSubmission entity
/// </summary>
public class ContactSubmissionConfiguration : IEntityTypeConfiguration<ContactSubmission>
{
    public void Configure(EntityTypeBuilder<ContactSubmission> builder)
    {
        builder.ToTable("ContactSubmissions");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.Email)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(c => c.Subject)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.Message)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(c => c.SubmittedDate)
            .IsRequired();

        builder.Property(c => c.IsRead)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(c => c.CreatedDate)
            .IsRequired();

        builder.Property(c => c.ModifiedDate)
            .IsRequired();

        // Indexes
        builder.HasIndex(c => c.IsRead);
        builder.HasIndex(c => c.SubmittedDate);
    }
}
