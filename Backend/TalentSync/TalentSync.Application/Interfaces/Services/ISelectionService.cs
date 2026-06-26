using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Application.DTOs.Recruitment;

namespace TalentSync.Application.Interfaces.Services
{
    public interface ISelectionService
    {
        Task<SelectionResponseDto> MakeDecisionAsync(CreateSelectionDecisionDto createSelectionDecision, CancellationToken cancellationToken);
        Task<SelectionWithDetailsResponseDto?> GetByApplicationIdAsync(Guid applicationId, CancellationToken cancellationToken);

    }
}
