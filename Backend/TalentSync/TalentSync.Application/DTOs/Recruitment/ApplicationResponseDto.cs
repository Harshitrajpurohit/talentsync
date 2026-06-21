using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Domain.Entities.Recruitment;
using TalentSync.Domain.Enums.Recruitment;

namespace TalentSync.Application.DTOs.Recruitment
{
    public class ApplicationResponseDto
    {
        public Guid Id { get; set; }
        public Guid JobId { get; set; }
        public Guid CandidateId { get; set; }
        public DateTime SubmittedDate { get; set; }
        public ApplicationStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
