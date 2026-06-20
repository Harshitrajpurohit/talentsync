using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Domain.Entities.Recruitment;
using TalentSync.Domain.Enums.Recruitment;

namespace TalentSync.Infrastructure.Persistence.Configurations
{
    public class ScreeningConfiguration : IEntityTypeConfiguration<Screening>
    {
        public void Configure(EntityTypeBuilder<Screening> builder)
        {
            builder.ToTable("Screenings");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.ApplicationId)
                   .IsRequired();

            builder.Property(s => s.RecruiterId)
                   .IsRequired();

            builder.Property(s => s.Result)
                   .HasConversion<string>()
                   .HasDefaultValue(ScreeningResult.Pending)
                   .IsRequired();

            builder.Property(s => s.Notes)
                   .HasMaxLength(2000);

            builder.Property(s => s.ScreeningDate)
                   .IsRequired();


            // Application -> Screenings (1:M)
            builder.HasOne(s => s.Application)
                   .WithMany(a => a.Screenings)
                   .HasForeignKey(s => s.ApplicationId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Recruiter -> Screenings (1:M)
            builder.HasOne(s => s.Recruiter)
                   .WithMany(u => u.Screenings)
                   .HasForeignKey(s => s.RecruiterId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(s => s.ApplicationId);
            builder.HasIndex(s => s.RecruiterId);
            builder.HasIndex(s => s.Result);
            builder.HasIndex(s => s.ScreeningDate);

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
