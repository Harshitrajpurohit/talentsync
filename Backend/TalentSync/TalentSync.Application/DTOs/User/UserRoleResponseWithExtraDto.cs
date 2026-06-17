using System;
using System.Collections.Generic;
using System.Text;

namespace TalentSync.Application.DTOs.User
{
    public class UserRoleResponseWithExtraDto
    {
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
        public Guid Id { get; set; }
        public string? RoleName { get; set; } = null;
        public string? UserName { get; set; } = null;
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
