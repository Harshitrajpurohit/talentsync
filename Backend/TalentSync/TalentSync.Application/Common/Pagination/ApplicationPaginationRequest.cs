using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Domain.Enums.Recruitment;

namespace TalentSync.Application.Common.Pagination
{
    public class ApplicationPaginationRequest : PaginationRequest
    {
        public string? Search { get; set; }

        public ApplicationStatus? Status { get; set; }

        public Guid? JobId { get; set; }
    }
}
