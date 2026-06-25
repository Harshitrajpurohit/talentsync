using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TalentSync.Application.DTOs.Recruitment
{
    public class RescheduleInterviewDto
    {
        [Required]
        public DateTime ScheduledAt { get; set; }

        [Required]
        [MaxLength(100)]
        public string Location { get; set; } = string.Empty;
    }
}
