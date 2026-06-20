using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Domain.Entities.Recruitment;
using TalentSync.Domain.Enums.Recruitment;

namespace TalentSync.Infrastructure.Persistence.Configurations
{
    public class InterviewConfiguration : IEntityTypeConfiguration<Interview>
    {
        public void Configure(EntityTypeBuilder<Interview> builder)
        {

            builder.ToTable("Interviews");

            builder.HasKey(i => i.Id);

            builder.Property(i => i.ApplicationId)
                   .IsRequired();

            builder.Property(i => i.InterviewerId)
                   .IsRequired();

            builder.Property(i => i.ScheduledAt)
                   .HasColumnType("datetime2")
                   .IsRequired();

            builder.Property(i => i.Location)
                   .HasMaxLength(500);

            builder.Property(i => i.Status)
                   .HasConversion<string>()
                   .HasDefaultValue(InterviewStatus.Pending)
                   .IsRequired();

            builder.Property(i => i.Feedback)
                   .HasMaxLength(2000);

            // Application -> Interviews (1:M)
            builder.HasOne(i => i.Application)
                   .WithMany(a => a.Interviews)
                   .HasForeignKey(i => i.ApplicationId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Interviewer -> Interviews (1:M)
            builder.HasOne(i => i.Interviewer)
                   .WithMany(u => u.Interviews)
                   .HasForeignKey(i => i.InterviewerId)
                   .OnDelete(DeleteBehavior.Restrict);


            builder.HasIndex(i => i.ApplicationId);
            builder.HasIndex(i => i.InterviewerId);
            builder.HasIndex(i => i.Status);
            builder.HasIndex(i => i.ScheduledAt);

            builder.Property(i => i.CreatedAt)
                   .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(i => i.UpdatedAt)
                   .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(i => i.IsDeleted)
                   .HasDefaultValue(false);

            builder.HasIndex(i => i.CreatedAt);
        }
    }
}
