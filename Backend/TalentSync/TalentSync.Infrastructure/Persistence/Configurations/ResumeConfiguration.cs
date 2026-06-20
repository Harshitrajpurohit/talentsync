using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Domain.Entities.Recruitment;
using TalentSync.Domain.Enums.Recruitment;

namespace TalentSync.Infrastructure.Persistence.Configurations
{
    public class ResumeConfiguration : IEntityTypeConfiguration<Resume>
    {
        public void Configure(EntityTypeBuilder<Resume> builder)
        {
            builder.ToTable("Resumes");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.CandidateId)
                   .IsRequired();

            builder.Property(r => r.FilePath)
                   .HasMaxLength(500)
                   .IsRequired();

            builder.Property(r => r.UploadedDate)
                   .IsRequired();

            builder.Property(r => r.Status)
                   .HasConversion<string>()
                   .HasDefaultValue(ResumeStatus.Active)
                   .IsRequired();

            builder.HasOne(r => r.Candidate)
                   .WithMany(u => u.Resumes)
                   .HasForeignKey(r => r.CandidateId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(r => r.CandidateId);
            builder.HasIndex(r => r.Status);
            builder.HasIndex(r => r.UploadedDate);

            builder.Property(r => r.CreatedAt)
                   .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(r => r.UpdatedAt)
                   .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(r => r.IsDeleted)
                   .HasDefaultValue(false);

            builder.HasIndex(r => r.CreatedAt);
        }
    }
}
