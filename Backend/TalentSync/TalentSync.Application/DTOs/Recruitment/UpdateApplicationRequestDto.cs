using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TalentSync.Domain.Enums.Recruitment;

namespace TalentSync.Application.DTOs.Recruitment
{
    public class UpdateApplicationRequestDto
    {
        [Required]
        public ApplicationStatus Status { get; set; }
    }
}
