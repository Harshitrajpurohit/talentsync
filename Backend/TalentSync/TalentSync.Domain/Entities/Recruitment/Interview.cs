using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Domain.Common;
using TalentSync.Domain.Enums.Recruitment;

namespace TalentSync.Domain.Entities.Recruitment
{
    public class Interview : BaseEntity
    {
        public Guid ApplicationId { get; set; }

        public ApplicationEntity Application { get; set; } = null!;

        public DateTime ScheduledAt { get; set; }

        public string? Location { get; set; }

        public Guid InterviewerId { get; set; }

        public User.User Interviewer { get; set; } = null!;

        public InterviewStatus Status { get; set; }

        public string? Feedback { get; set; }

    }
}
