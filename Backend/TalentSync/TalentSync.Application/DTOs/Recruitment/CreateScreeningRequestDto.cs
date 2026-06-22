using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TalentSync.Domain.Entities.Recruitment;
using TalentSync.Domain.Enums.Recruitment;

namespace TalentSync.Application.DTOs.Recruitment
{
    public class CreateScreeningRequestDto
    {
        [Required]
        public Guid ApplicationId { get; set; }
        [Required]
        public ScreeningResult Result { get; set; }
        [Required]
        [MaxLength(1000)]
        public string Notes { get; set; }
    }
}
