using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Domain.Enums.User;

namespace TalentSync.Application.DTOs.User
{
    public class UserWithRolesDto
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public Guid RoleId { get; set; }

        public string Name { get; set; } = default!;

        public string Email { get; set; } = default!;

        public string? Phone { get; set; }

        public UserStatus Status { get; set; }

        public RoleName Role { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
