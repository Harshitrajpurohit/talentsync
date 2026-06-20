using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Domain.Common;
using TalentSync.Domain.Entities.Recruitment;
using TalentSync.Domain.Enums.User;

namespace TalentSync.Domain.Entities.User
{
    public class User : BaseEntity
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }    

        public UserStatus Status { get; set; }

        public string? Phone { get; set; }

        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

        public ICollection<Job> Jobs { get; set; } = new List<Job>();

        public ICollection<ApplicationEntity> Applications { get; set; } = new List<ApplicationEntity>();

        public ICollection<Resume> Resumes { get; set; } = new List<Resume>();

        public ICollection<Screening> Screenings { get; set; } = new List<Screening>();
        public ICollection<Interview> Interviews { get; set; } = new List<Interview>();

    }
}
