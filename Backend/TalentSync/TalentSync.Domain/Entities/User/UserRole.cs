using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Domain.Common;

namespace TalentSync.Domain.Entities.User
{
    public class UserRole : BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
        public User User { get; set; }
        public Role Role { get; set; }

    }
}
