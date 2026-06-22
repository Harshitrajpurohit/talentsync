using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Application.DTOs.Recruitment;

namespace TalentSync.Application.Interfaces.Services
{
    public interface IScreeningService
    {
        Task<ScreeningResponseDto> CreateScreeningAsync(CreateScreeningRequestDto screeningRequestDto, Guid screenedById, CancellationToken cancellationToken);
        Task<ScreeningResponseDto> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<ScreeningResponseDto> GetByApplicationIdAsync(Guid applicationId,Guid authorizedUserId, CancellationToken cancellationToken);
        Task<ScreeningResponseDto> UpdateScreeningAsync(Guid id, UpdateScreeningRequestDto updateScreeningRequest, CancellationToken cancellationToken);
    }
}
