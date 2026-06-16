using System;
using System.Collections.Generic;
using System.Text;

namespace TalentSync.Application.Common.Pagination
{
    public class PaginationRequest
    {
        private const int MaxPageSize = 100;

        public int PageNumber { get; set; } = 1;

        private int _pageSize = 10;

        public int PageSize
        {
            get { return _pageSize; }
            set => _pageSize = value <= 0 ? 10 :
                   value > MaxPageSize ? MaxPageSize : value;
        }

    }
}
