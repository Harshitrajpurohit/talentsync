using System;
using System.Collections.Generic;
using System.Text;

namespace TalentSync.Application.Common.Pagination
{
    public class PaginationResponse<T>
    {
        public PaginationResponse(int pageNumber, int pageSize, int totalRecords, List<T> data)
        {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
            this.TotalRecords = totalRecords;
            this.Data = data;
        }

        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }

        public List<T> Data { get; set; } = [];
    }
}
