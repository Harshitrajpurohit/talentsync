using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Domain.Enums.User;

namespace TalentSync.Application.DTOs.Dashboard
{
    public class AdminRoleCountDto
    {
        public RoleName Role { get; set; }
        public int Count { get; set; }
    }
}
