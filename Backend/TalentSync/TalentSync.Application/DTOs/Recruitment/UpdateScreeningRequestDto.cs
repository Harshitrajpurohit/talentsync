using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TalentSync.Domain.Enums.Recruitment;

namespace TalentSync.Application.DTOs.Recruitment
{
    public class UpdateScreeningRequestDto
    {
        [Required]
        public ScreeningResult? Result { get; set; }
        [Required]
        [MaxLength(1000)]
        public string? Notes { get; set; }
    }
}
