using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Domain.Common;
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

    }
}
