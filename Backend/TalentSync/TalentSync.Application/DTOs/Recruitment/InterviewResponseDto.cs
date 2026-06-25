using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Domain.Enums.Recruitment;

namespace TalentSync.Application.DTOs.Recruitment
{
    public class InterviewResponseDto
    {
        public Guid Id { get; set; }
        public Guid ApplicationId { get; set; }
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
