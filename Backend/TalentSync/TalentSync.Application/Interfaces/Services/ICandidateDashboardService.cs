using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Application.DTOs.Dashboard;

namespace TalentSync.Application.Interfaces.Services
{
    public interface ICandidateDashboardService
    {
         Task<CandidateDashboardResponseDto> GetDashboardAsync(Guid candidateId, CancellationToken cancellationToken);
    }
}
