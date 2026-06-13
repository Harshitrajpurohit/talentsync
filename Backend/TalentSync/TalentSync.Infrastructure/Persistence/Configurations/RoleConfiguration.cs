using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Domain.Entities.User;
using TalentSync.Domain.Enums.User;

namespace TalentSync.Infrastructure.Persistence.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {

            builder.ToTable("Roles");
            builder.HasKey(r => r.Id);

            builder.Property(r => r.Id).HasDefaultValueSql("NEWSEQUENTIALID()").ValueGeneratedOnAdd();

            builder.Property(r => r.Name)
                   .HasConversion<string>()
                   .IsRequired()
                   .HasMaxLength(50);

            builder.HasIndex(r => r.Name)
                .IsUnique();

            builder.Property(r => r.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            builder.Property(r => r.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");
            builder.Property(ur => ur.IsDeleted).HasDefaultValue(false);

        }
    }
}
