using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Application.Common.Pagination;
using TalentSync.Application.DTOs.Recruitment;

namespace TalentSync.Application.Interfaces.Services
{
    public interface IInterviewService
    {
        Task<InterviewResponseDto> ScheduleInterviewAsync(ScheduleInterviewDto scheduleInterview, CancellationToken cancellationToken);
        Task<InterviewResponseDto> GetByApplicationIdAsync(Guid applicationId, CancellationToken cancellationToken);
        Task<InterviewResponseDto> UpdateInterviewStatusAsync(Guid id, UpdateInterviewStatusDto updateInterviewStatus, CancellationToken cancellationToken);
        Task<InterviewResponseDto> RescheduleInterviewAsync(Guid id, RescheduleInterviewDto rescheduleInterview, CancellationToken cancellationToken);
        Task<PaginationResponse<InterviewDetailedResponseDto>> InterviewsAssignedToInterviwerAsync(Guid interviewerId, InterviewPaginationRequest paginationRequest, CancellationToken cancellationToken);
        Task<InterviewDetailedResponseDto> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken);
        Task<PaginationResponse<InterviewDetailedResponseDto>> GetPagedByCandidateIdAsync(Guid candidateId, InterviewPaginationRequest paginationRequest, CancellationToken cancellationToken);
        Task<PaginationResponse<CandidateInterviewResponseDto>> GetByCandidateIdAsync(Guid candidateId, InterviewPaginationRequest paginationRequest, CancellationToken cancellationToken);
    }
}
