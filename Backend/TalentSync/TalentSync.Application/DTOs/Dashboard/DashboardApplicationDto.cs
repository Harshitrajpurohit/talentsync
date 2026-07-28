using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Domain.Enums.Recruitment;

namespace TalentSync.Application.DTOs.Dashboard
{
    public class DashboardApplicationDto
    {
        public Guid Id { get; set; }

        public Guid JobId { get; set; }

        public string JobTitle { get; set; } = string.Empty;

        public ApplicationStatus Status { get; set; }

        public DateTime SubmittedDate { get; set; }
    }
}
