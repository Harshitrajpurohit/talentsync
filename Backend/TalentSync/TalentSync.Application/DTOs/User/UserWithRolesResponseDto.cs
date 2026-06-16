using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Domain.Enums.User;

namespace TalentSync.Application.DTOs.User
{
    public class UserWithRolesResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
        public UserStatus Status { get; set; }
        public List<string> Roles { get; set; } = [];
    }
}
