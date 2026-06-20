using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Domain.Entities.Recruitment;
using TalentSync.Domain.Enums.Recruitment;

namespace TalentSync.Infrastructure.Persistence.Configurations
{
    public class JobConfiguration : IEntityTypeConfiguration<Job>
    {
        public void Configure(EntityTypeBuilder<Job> builder)
        {

            builder.ToTable("Jobs");
            builder.HasKey(j => j.Id);
            builder.Property(j => j.Title)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(j => j.Department)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(j => j.Description)
                .HasMaxLength(2000)
                .IsRequired();

            builder.Property(j => j.Requirements)
                .HasMaxLength(2000)
                .IsRequired();

            builder.Property(j => j.Status)
                .HasConversion<string>()
                .HasDefaultValue(JobStatus.Open)
                .IsRequired();

            builder.Property(j => j.PostedDate)
                .IsRequired();

            builder.Property(j => j.HRId)
                .IsRequired();

            builder.HasOne(j => j.HR)
                .WithMany(u => u.Jobs)
                .HasForeignKey(j => j.HRId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(j => j.HRId);
            builder.HasIndex(j => j.Title);
            builder.HasIndex(j => j.Status);

            builder.HasIndex(j => j.PostedDate);

            builder.HasIndex(u => u.CreatedAt);
            
            builder.Property(u => u.CreatedAt)
                   .HasDefaultValueSql("GETUTCDATE()");
            
            builder.Property(u => u.UpdatedAt)
                   .HasDefaultValueSql("GETUTCDATE()");
            
            builder.Property(u => u.IsDeleted)
                   .HasDefaultValue(false);


        }
    }
}
