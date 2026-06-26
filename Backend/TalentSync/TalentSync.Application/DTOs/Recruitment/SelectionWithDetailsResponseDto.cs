using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Domain.Enums.Recruitment;

namespace TalentSync.Application.DTOs.Recruitment
{
    public class SelectionWithDetailsResponseDto
    {
        public Guid Id { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid CandidateId { get; set; }
        public string CandidateName { get; set; } = string.Empty;
        public string CandidateEmail { get; set; } = string.Empty;
        public Guid JobId { get; set; }
        public string JobTitle {get; set;} = string.Empty;
        public SelectionDecision Decision { get; set; }
        public string? Notes { get; set; }
        public DateTime SelectionDate { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
