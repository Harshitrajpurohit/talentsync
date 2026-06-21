using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Application.Common.Pagination;
using TalentSync.Application.DTOs.Recruitment;

namespace TalentSync.Application.Interfaces.Services
{
    public interface IApplicationService
    {
        Task<ApplicationResponseDto> CreateApplicationAsync(CreateApplicationDto createApplicationDto, Guid candidateId, CancellationToken cancellationToken);
        Task<ApplicationWithDetailsResponseDto> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<PaginationResponse<ApplicationWithDetailsResponseDto>> GetAllAsync(PaginationRequest paginationRequest, CancellationToken cancellationToken);
        Task<List<ApplicationWithDetailsResponseDto>> GetByJobIdAsync(Guid jobId, CancellationToken cancellationToken);
        Task<List<ApplicationWithDetailsResponseDto>> GetByCandidateIdAsync(Guid candidateId, CancellationToken cancellationToken);
        Task<ApplicationResponseDto?> UpdateApplicationAsync(Guid id, UpdateApplicationRequestDto updateApplicationRequestDto, CancellationToken cancellationToken);
        Task<bool> DeleteApplicationAsync(Guid id, CancellationToken cancellationToken);
    }
}
