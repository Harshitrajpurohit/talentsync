using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Application.DTOs.Recruitment;

namespace TalentSync.Application.DTOs.Dashboard
{
    public class ManagerDashboardDto
    {
        public int OpenJobs { get; set; }
        public int TotalApplications { get; set; }
        public int InterviewsToday { get; set; }
        public int UpcomingInterviews { get; set; }
        public int CompletedInterviews { get; set; }

        public List<InterviewDetailedResponseDto> UpcomingInterviewsList { get; set; } = [];
        public List<ApplicationWithDetailsResponseDto> RecentApplications { get; set; } = [];
        public List<DashboardJobDto> RecentJobs { get; set; } = [];
    }
}
