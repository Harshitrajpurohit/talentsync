using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Application.DTOs.Recruitment;

namespace TalentSync.Application.DTOs.Dashboard
{
    public class HrDashboardResponseDto
    {
        public int TotalJobs { get; set; }

        public int OpenJobs { get; set; }

        public int TotalCandidates { get; set; }

        public int TotalApplications { get; set; }

        public int InterviewsToday { get; set; }

        public List<ApplicationWithDetailsResponseDto> RecentApplications { get; set; } = [];

        public List<InterviewDetailedResponseDto> UpcomingInterviews { get; set; } = [];

        public List<JobResponseDto> RecentJobs { get; set; } = [];
    }
}
