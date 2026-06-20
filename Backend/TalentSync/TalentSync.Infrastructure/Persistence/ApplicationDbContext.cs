using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Domain.Entities.Auth;
using TalentSync.Domain.Entities.Recruitment;
using TalentSync.Domain.Entities.User;

namespace TalentSync.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public DbSet<Job> Jobs { get; set; }
        public DbSet<Resume> Resumes { get; set; }
        public DbSet<ApplicationEntity> Applications { get; set; }
        public DbSet<Screening> Screenings { get; set; }
        public DbSet<Interview> Interviews { get; set; }
        public DbSet<Selection> Selections { get; set; }

    }
}
