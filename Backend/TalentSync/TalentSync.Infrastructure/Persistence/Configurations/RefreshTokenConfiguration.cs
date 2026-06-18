using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Domain.Entities.Auth;

namespace TalentSync.Infrastructure.Persistence.Configurations
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {

            builder.ToTable("RefreshTokens");

            builder.HasKey(rt => rt.Id);
            builder.Property(rt => rt.Token).IsRequired().HasMaxLength(88);
            builder.Property(rt => rt.IsRevoked).IsRequired();
            builder.HasIndex(rt => rt.UserId);
            builder.HasIndex(rt => rt.Token)
                .IsUnique();
            builder.HasOne(rt => rt.User)
               .WithMany()
               .HasForeignKey(rt => rt.UserId)
               .OnDelete(DeleteBehavior.Restrict);

            builder.Property(rt => rt.ExpiresAt).IsRequired();

            builder.Property(rt => rt.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            builder.Property(rt => rt.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");
            builder.Property(rt => rt.IsDeleted).HasDefaultValue(false);
        }
    }
}
