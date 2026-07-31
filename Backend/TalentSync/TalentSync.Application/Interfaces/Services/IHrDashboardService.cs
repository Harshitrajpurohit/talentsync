using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Application.DTOs.Dashboard;

namespace TalentSync.Application.Interfaces.Services
{
    public interface IHrDashboardService
    {
        Task<HrDashboardResponseDto> GetDashboardAsync(Guid hrId, CancellationToken cancellationToken);
    }
}
