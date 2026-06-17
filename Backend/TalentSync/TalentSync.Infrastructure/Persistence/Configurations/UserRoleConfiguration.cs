using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Domain.Entities.User;

namespace TalentSync.Infrastructure.Persistence.Configurations
{
    internal class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.ToTable("UserRoles");

            builder.HasKey(u => u.Id);
            builder.HasIndex(u => new { u.UserId, u.RoleId })
                .IsUnique();

            builder.Property(u => u.Id).HasDefaultValueSql("NEWSEQUENTIALID()").ValueGeneratedOnAdd();

            builder.Property(ur => ur.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            builder.Property(ur => ur.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");
            builder.Property(ur => ur.IsDeleted).HasDefaultValue(false);


            builder.HasOne(ur => ur.User).WithMany().HasForeignKey(ur => ur.UserId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(ur => ur.Role).WithMany().HasForeignKey(ur => ur.RoleId).OnDelete(DeleteBehavior.Restrict);

        }
    }
}
