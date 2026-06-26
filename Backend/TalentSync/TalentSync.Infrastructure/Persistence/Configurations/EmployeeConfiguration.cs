using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Domain.Entities.HumanResources;
using TalentSync.Domain.Enums.Employees;

namespace TalentSync.Infrastructure.Persistence.Configurations
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.ToTable("Employees");

            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.EmployeeCode).IsUnique();

            builder.Property(x => x.EmployeeCode).HasMaxLength(20).IsRequired();

            builder.HasIndex(x => x.UserId).IsUnique();
            builder.Property(x => x.DepartmentName).HasMaxLength(100).IsRequired();
            builder.Property(x => x.Position).HasMaxLength(100).IsRequired();
            builder.Property(x => x.JoinDate).IsRequired();

            builder.Property(x => x.Status).HasConversion<string>().HasDefaultValue(EmployeeStatus.Active).IsRequired();
            builder.HasIndex(x => x.ManagerId);
            builder.HasIndex(x => x.CreatedAt);
            builder.HasIndex(x => x.JoinDate);
            builder.HasIndex(x => x.DepartmentName);
            builder.HasIndex(x => x.Status);

            builder.HasOne(x => x.User)
                 .WithOne(u => u.Employee)
                 .HasForeignKey<Employee>(x => x.UserId)
                 .OnDelete(DeleteBehavior.Restrict);


            builder.HasOne(x => x.Manager)
                .WithMany()
                .HasForeignKey(x => x.ManagerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
