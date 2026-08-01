using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Application.Common.Pagination;
using TalentSync.Application.DTOs.Employees;

namespace TalentSync.Application.Interfaces.Services
{
    public interface IEmployeeService
    {
        Task<PaginationResponse<EmployeeResponseDto>> GetEmployeesAsync(
        PaginationRequest paginationRequest,
        CancellationToken cancellationToken);
    }
}
