using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Domain.Entities.Recruitment;
using TalentSync.Domain.Enums.Recruitment;

namespace TalentSync.Application.DTOs.Recruitment
{
    public class ScreeningResponseDto
    {
        public Guid Id { get; set; }
        public Guid ApplicationId { get; set; }
        public string? CandidateName { get; set; }
        public string? JobTitle { get; set; }
        public Guid ScreenedById { get; set; }
        public string? ScreenedName { get; set; }
        public ScreeningResult Result { get; set; }
        public string? Notes { get; set; }
        public DateTime ScreeningDate { get; set; }
    }
}
