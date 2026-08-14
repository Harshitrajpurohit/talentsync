using System;
using System.Collections.Generic;
using System.Text;

namespace TalentSync.Application.DTOs.Dashboard
{
    public class AdminDashboardDto
    {
        public int TotalUsers { get; set; }
        public int ActiveUsers { get; set; }
        public int InactiveUsers { get; set; }
        public int SuspendedUsers { get; set; }
        public List<AdminRoleCountDto> UsersByRole { get; set; } = [];

    }
}
