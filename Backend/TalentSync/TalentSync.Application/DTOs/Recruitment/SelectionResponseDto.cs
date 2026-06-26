using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Domain.Enums.Recruitment;

namespace TalentSync.Application.DTOs.Recruitment
{
    public class SelectionResponseDto
    {
        public Guid Id { get; set; }
        public Guid ApplicationId { get; set; }
        public SelectionDecision Decision { get; set; }
        public string? Notes { get; set; }
        public DateTime SelectionDate { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
