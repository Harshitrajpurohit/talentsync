using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Identity.Client;
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

            builder.Property(r => r.FileUrl)
               .HasMaxLength(500)
               .IsRequired();

            builder.Property(r => r.FileName)
                   .HasMaxLength(1000)
                   .IsRequired();

            builder.Property(r => r.PublicId)
                   .HasMaxLength(255)
                   .IsRequired();

            builder.Property(r => r.ContentType)
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(r => r.FileSize)
               .IsRequired();

            builder.Property(r => r.UploadedDate)
                   .IsRequired()
                   .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(r => r.CreatedAt)
                   .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(r => r.UpdatedAt)
                   .HasDefaultValueSql("GETUTCDATE()");

            builder.HasOne(r => r.Candidate)
               .WithOne(u => u.Resume)
               .HasForeignKey<Resume>(r => r.CandidateId)
               .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(r => r.CandidateId)
                   .IsUnique();

            builder.HasIndex(r => r.PublicId).IsUnique();
            builder.HasIndex(r => r.UploadedDate);
        }
    }
}
