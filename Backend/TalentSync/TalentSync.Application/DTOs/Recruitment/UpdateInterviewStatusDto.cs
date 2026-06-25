using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TalentSync.Domain.Enums.Recruitment;

namespace TalentSync.Application.DTOs.Recruitment
{
    public class UpdateInterviewStatusDto
    {
        [Required]
        public InterviewStatus Status { get; set; }

        [MaxLength(2000)]
        public string? Feedback { get; set; }
    }
}
