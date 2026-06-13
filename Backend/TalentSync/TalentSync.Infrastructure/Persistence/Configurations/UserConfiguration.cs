using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Domain.Entities.User;
using TalentSync.Domain.Enums.User;

namespace TalentSync.Infrastructure.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.Id).HasDefaultValueSql("NEWSEQUENTIALID()").ValueGeneratedOnAdd();

            builder.Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(u => u.Email)
               .IsRequired()
               .HasMaxLength(320).IsUnicode(false);

            builder.HasIndex(u => u.Email).IsUnique();

            builder.Property(u => u.PasswordHash)
               .IsRequired()
               .HasMaxLength(500);

            builder.Property(u => u.Phone)
                  .HasMaxLength(20)
                  .IsRequired(false);

            builder.HasIndex(u => u.Phone)
               .IsUnique()
               .HasFilter("[Phone] IS NOT NULL");


            builder.Property(u => u.Status)
                .HasConversion<string>()
                .HasDefaultValue(UserStatus.Active)
                .IsRequired();

            builder.Property(u => u.CreatedAt)
                   .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(u => u.UpdatedAt)
                   .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(u => u.IsDeleted)
                   .HasDefaultValue(false);

        }
    }
}
