using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Application.DTOs.Recruitment;

namespace TalentSync.Application.DTOs.Dashboard
{
    public class RecruiterDashboardResponseDto
    {
        public int OpenJobs { get; set; }

        public int TotalApplications { get; set; }

        public int PendingScreenings { get; set; }

        public int ApplicationsToday { get; set; }

        public int InterviewsScheduled { get; set; }

        public int ClosedJobs { get; set; }

        public List<ApplicationWithDetailsResponseDto> RecentApplications { get; set; } = [];

        public List<DashboardJobDto> RecentJobs { get; set; } = [];
    }
}
