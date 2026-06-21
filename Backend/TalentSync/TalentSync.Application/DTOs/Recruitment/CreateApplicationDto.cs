using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TalentSync.Domain.Entities.Recruitment;
using TalentSync.Domain.Enums.Recruitment;

namespace TalentSync.Application.DTOs.Recruitment
{
    public class CreateApplicationDto
    {
        [Required]
        public Guid JobId { get; set; }
    }
}
