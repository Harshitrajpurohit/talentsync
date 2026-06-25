using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Domain.Enums.Recruitment;

namespace TalentSync.Application.DTOs.Recruitment
{
    public class InterviewDetailedResponseDto
    {
        public Guid Id { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid JobId { get; set; }
        public string JobTitle { get; set; } = string.Empty;
        public Guid CandidateId { get; set; }
        public string CandidateName { get; set; } = string.Empty;
        public string CandidateEmail { get; set; } = string.Empty;
        public DateTime ScheduledAt { get; set; }
        public string? Location { get; set; }
        public Guid InterviewerId { get; set; }
        public string InterviewerName { get; set; } = string.Empty;
        public InterviewStatus Status { get; set; }
        public string? Feedback { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
