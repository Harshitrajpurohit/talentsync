using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Domain.Enums.User;

namespace TalentSync.Application.DTOs.User
{
    public class RoleResponseDto
    {
        public Guid Id { get; set; }
        public RoleName Name { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
