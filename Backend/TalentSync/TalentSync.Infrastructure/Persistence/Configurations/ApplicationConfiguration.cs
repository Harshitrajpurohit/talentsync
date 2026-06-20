using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Domain.Entities.Recruitment;
using TalentSync.Domain.Enums.Recruitment;

namespace TalentSync.Infrastructure.Persistence.Configurations
{
    public class ApplicationConfiguration : IEntityTypeConfiguration<ApplicationEntity>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<ApplicationEntity> builder)
        {
            builder.ToTable("Applications");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.JobId)
                   .IsRequired();

            builder.Property(a => a.CandidateId)
                   .IsRequired();

            builder.Property(a => a.SubmittedDate)
                   .IsRequired();


            builder.Property(a => a.Status)
                   .HasConversion<string>()
                   .HasDefaultValue(ApplicationStatus.Pending)
                   .IsRequired();

            // Job -> Applications (1:M)
            builder.HasOne(a => a.Job)
                   .WithMany(j => j.Applications)
                   .HasForeignKey(a => a.JobId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Candidate -> Applications (1:M)
            builder.HasOne(a => a.Candidate)
                   .WithMany(u => u.Applications)
                   .HasForeignKey(a => a.CandidateId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(a => new { a.JobId, a.CandidateId })
                   .IsUnique();

            builder.HasIndex(a => a.JobId);
            builder.HasIndex(a => a.CandidateId);
            builder.HasIndex(a => a.Status);
            builder.HasIndex(a => a.SubmittedDate);
            builder.HasIndex(a => a.CreatedAt);

            builder.Property(a => a.CreatedAt)
                   .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(a => a.UpdatedAt)
                   .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(a => a.IsDeleted)
                   .HasDefaultValue(false);

            

        }
    }
}
