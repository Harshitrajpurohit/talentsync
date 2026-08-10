using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Domain.Enums.Recruitment;

namespace TalentSync.Application.Common.Pagination
{
    public class InterviewPaginationRequest : PaginationRequest
    {
        public string? Search { get; set; }

        public InterviewStatus? Status { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }
    }
}
