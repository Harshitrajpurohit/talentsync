using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TalentSync.Application.DTOs.Recruitment
{
    public class ScheduleInterviewDto
    {
        [Required]
        public Guid ApplicationId { get; set; }

        [Required]
        public DateTime ScheduledAt { get; set; }

        [Required]
        public Guid InterviewerId { get; set; }

        [MaxLength(200)]
        public string? Location { get; set; }
    }
}
