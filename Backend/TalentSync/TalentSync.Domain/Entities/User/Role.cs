using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Domain.Common;
using TalentSync.Domain.Enums.User;

namespace TalentSync.Domain.Entities.User
{
    public class Role : BaseEntity
    {
        public RoleName Name { get; set; }

        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}
