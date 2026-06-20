using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Domain.Entities.Recruitment;
using TalentSync.Domain.Enums.Recruitment;

namespace TalentSync.Infrastructure.Persistence.Configurations
{
    public class SelectionConfiguration : IEntityTypeConfiguration<Selection>
    {
        public void Configure(EntityTypeBuilder<Selection> builder)
        {

            builder.ToTable("Selections");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.ApplicationId)
                   .IsRequired();

            builder.Property(s => s.Decision)
                   .HasConversion<string>()
                   .HasDefaultValue(SelectionDecision.Pending)
                   .IsRequired();

            builder.Property(s => s.Notes)
                   .HasMaxLength(2000);

            builder.Property(s => s.SelectionDate)
                   .HasColumnType("datetime2")
                   .IsRequired();

            // Application <-> Selection (1:1)
            builder.HasOne(s => s.Application)
                   .WithOne(a => a.Selection)
                   .HasForeignKey<Selection>(s => s.ApplicationId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Prevent multiple selections for same application
            builder.HasIndex(s => s.ApplicationId)
                   .IsUnique();

            builder.HasIndex(s => s.Decision);
            builder.HasIndex(s => s.SelectionDate);

            builder.Property(s => s.CreatedAt)
                   .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(s => s.UpdatedAt)
                   .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(s => s.IsDeleted)
                   .HasDefaultValue(false);

            builder.HasIndex(s => s.CreatedAt);
        }
    }
}
