using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Domain.Enums.Recruitment;

namespace TalentSync.Application.DTOs.Dashboard
{
    public class DashboardInterviewDto
    {
        public Guid Id { get; set; }

        public string JobTitle { get; set; } = string.Empty;

        public DateTime ScheduledAt { get; set; }

        public string InterviewerName { get; set; } = string.Empty;

        public string? Location { get; set; }

        public InterviewStatus Status { get; set; }
    }
}
