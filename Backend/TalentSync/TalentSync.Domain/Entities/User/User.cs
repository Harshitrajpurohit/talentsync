using System;
using System.Collections.Generic;
using TalentSync.Domain.Common;
using TalentSync.Domain.Entities.HumanResources;
using TalentSync.Domain.Entities.Notifications;
using TalentSync.Domain.Entities.Recruitment;
using TalentSync.Domain.Enums.User;

namespace TalentSync.Domain.Entities.User
{
    public class User : BaseEntity
    {
        // Authentication
        public string Name { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public UserStatus Status { get; set; }

        // Profile
        public string? Phone { get; set; }

        public string? ProfilePictureUrl { get; set; }

        public DateOnly? DateOfBirth { get; set; }

        public string? Gender { get; set; }

        public string? Address { get; set; }

        public string? About { get; set; }

        public string? LinkedinUrl { get; set; }

        public string? GithubUrl { get; set; }

        public string? PortfolioUrl { get; set; }

        // Navigation Properties
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

        public ICollection<Job> Jobs { get; set; } = new List<Job>();

        public ICollection<ApplicationEntity> Applications { get; set; } = new List<ApplicationEntity>();

        public ICollection<Screening> Screenings { get; set; } = new List<Screening>();

        public ICollection<Interview> Interviews { get; set; } = new List<Interview>();

        public Resume? Resume { get; set; }

        public Employee? Employee { get; set; }

        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    }
}