using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TalentSync.Domain.Enums.Recruitment;

namespace TalentSync.Application.DTOs.Recruitment
{
    public class UpdateJobRequestDto
    {
        [MaxLength(100)]
        public string? Title { get; set; }

        [MaxLength(2000)]
        public string? Description { get; set; }

        [MaxLength(2000)]
        public string? Requirements { get; set; }

        [MaxLength(100)]
        public string? Department { get; set; }
        public JobStatus? Status { get; set; }
    }
}
