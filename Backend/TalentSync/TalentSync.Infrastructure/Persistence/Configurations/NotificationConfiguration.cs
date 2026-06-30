using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Domain.Entities.Notifications;
using TalentSync.Domain.Enums.Notifications;

namespace TalentSync.Infrastructure.Persistence.Configurations
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.ToTable("Notifications");

            

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Title).HasMaxLength(100).IsRequired();
            builder.Property(x => x.Message).HasMaxLength(1000).IsRequired();
            builder.Property(x => x.Category).HasConversion<string>().HasDefaultValue(NotificationCategory.System).IsRequired();
            builder.Property(x => x.Channel).HasConversion<string>().HasDefaultValue(NotificationChannel.InApp).IsRequired();
            builder.Property(x => x.Status).HasConversion<string>().HasDefaultValue(NotificationStatus.Pending).IsRequired();

            builder.Property(x => x.IsRead).HasDefaultValue(false).IsRequired();

            builder.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()").IsRequired();
            builder.Property(x => x.UpdatedAt).HasDefaultValueSql("GETUTCDATE()").IsRequired();
            builder.Property(x => x.IsDeleted).HasDefaultValue(false).IsRequired();

            builder.HasOne(x => x.User)
                   .WithMany(u => u.Notifications)
                   .HasForeignKey(x => x.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => x.CreatedAt);
            builder.HasIndex(x => x.UserId);
            builder.HasIndex(x => new
            {
                x.UserId,
                x.IsRead
            });

        }
    }
}
