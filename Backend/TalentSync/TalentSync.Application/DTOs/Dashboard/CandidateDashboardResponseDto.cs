using System;
using System.Collections.Generic;
using System.Text;

namespace TalentSync.Application.DTOs.Dashboard
{
    public class CandidateDashboardResponseDto
    {
        public int TotalApplications { get; set; }

        public int ActiveApplications { get; set; }

        public int UpcomingInterviewsCount { get; set; }

        public int SelectedApplications { get; set; }

        public int ProfileCompletion { get; set; }

        public bool ResumeUploaded { get; set; }

        public List<DashboardApplicationDto> RecentApplications { get; set; } = [];

        public List<DashboardInterviewDto> UpcomingInterviews { get; set; } = [];
    }
}