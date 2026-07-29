using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Domain.Enums.Recruitment;

namespace TalentSync.Application.DTOs.Recruitment
{
    public class CandidateJobListDto
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Department { get; set; } = string.Empty;

        public DateTime PostedDate { get; set; }

        public JobStatus Status { get; set; }

        public bool HasApplied { get; set; }
    }
}
