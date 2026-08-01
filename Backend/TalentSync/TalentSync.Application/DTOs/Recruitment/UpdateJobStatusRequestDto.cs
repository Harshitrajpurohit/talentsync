using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Domain.Enums.Recruitment;

namespace TalentSync.Application.DTOs.Recruitment
{
    public class UpdateJobStatusRequestDto
    {
        public JobStatus Status { get; set; }
    }
}
