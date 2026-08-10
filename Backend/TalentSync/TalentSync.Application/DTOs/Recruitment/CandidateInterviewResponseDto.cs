using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Domain.Enums.Recruitment;

namespace TalentSync.Application.DTOs.Recruitment
{
    public class CandidateInterviewResponseDto
    {
        public Guid Id { get; set; }

        public Guid JobId { get; set; }

        public string JobTitle { get; set; } = string.Empty;

        public DateTimeOffset ScheduledAt { get; set; }

        public string InterviewerName { get; set; } = string.Empty;

        public string? Location { get; set; }

        public InterviewStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
