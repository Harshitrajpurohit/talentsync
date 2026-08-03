using System;
using System.Collections.Generic;
using System.Text;

namespace TalentSync.Application.DTOs.Recruitment
{
    public class JobSummaryResponseDto
    {
        public int TotalApplications { get; set; }

        public int Submitted { get; set; }

        public int Screening { get; set; }

        public int Interview { get; set; }

        public int Selected { get; set; }

        public int Rejected { get; set; }
    }
}
