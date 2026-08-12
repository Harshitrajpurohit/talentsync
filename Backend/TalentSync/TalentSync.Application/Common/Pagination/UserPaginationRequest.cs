using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Domain.Enums.User;

namespace TalentSync.Application.Common.Pagination
{
    public class UserPaginationRequest : PaginationRequest
    {
        public string? Search { get; set; }

        public UserStatus? Status { get; set; }

        public RoleName? Role { get; set; }
    }
}
