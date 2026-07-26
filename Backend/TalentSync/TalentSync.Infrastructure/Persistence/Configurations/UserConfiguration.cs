using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
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

            builder.Property(u => u.Id)
                .HasDefaultValueSql("NEWSEQUENTIALID()")
                .ValueGeneratedOnAdd();

            builder.Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(320)
                .IsUnicode(false);

            builder.HasIndex(u => u.Email)
                .IsUnique();

            builder.Property(u => u.PasswordHash)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(u => u.Phone)
                .HasMaxLength(20)
                .IsRequired(false);

            builder.HasIndex(u => u.Phone)
                .IsUnique()
                .HasFilter("[Phone] IS NOT NULL");

            // Profile Fields

            builder.Property(u => u.ProfilePictureUrl)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property(u => u.DateOfBirth)
                .IsRequired(false);

            builder.Property(u => u.Gender)
                .HasMaxLength(20)
                .IsRequired(false);

            builder.Property(u => u.Address)
                .HasMaxLength(300)
                .IsRequired(false);

            builder.Property(u => u.About)
                .HasMaxLength(1000)
                .IsRequired(false);

            builder.Property(u => u.LinkedinUrl)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property(u => u.GithubUrl)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property(u => u.PortfolioUrl)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property(u => u.Status)
                .HasConversion<string>()
                .HasDefaultValue(UserStatus.Active)
                .IsRequired();

            builder.Property(u => u.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.HasIndex(u => u.CreatedAt);

            builder.Property(u => u.UpdatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(u => u.IsDeleted)
                .HasDefaultValue(false);
        }
    }
}