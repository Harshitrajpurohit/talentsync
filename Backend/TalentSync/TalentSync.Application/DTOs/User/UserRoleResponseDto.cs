using System;
using System.Collections.Generic;
using System.Text;

namespace TalentSync.Application.DTOs.User
{
    public class UserRoleResponseDto
    {
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
        public Guid Id { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
