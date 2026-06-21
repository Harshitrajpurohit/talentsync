using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Domain.Enums.Recruitment;

namespace TalentSync.Application.DTOs.Recruitment
{
    public class ApplicationWithDetailsResponseDto
    {
        public Guid Id { get; set; }
        public Guid JobId { get; set; }
        public string? JobTitle { get; set; }
        public Guid CandidateId { get; set; }
        public string? CandidateName { get; set; }
        public DateTime SubmittedDate { get; set; }
        public ApplicationStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
