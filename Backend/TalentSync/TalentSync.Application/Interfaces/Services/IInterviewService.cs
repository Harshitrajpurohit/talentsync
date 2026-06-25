using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Application.DTOs.Recruitment;

namespace TalentSync.Application.Interfaces.Services
{
    public interface IInterviewService
    {
        Task<InterviewResponseDto> ScheduleInterviewAsync(ScheduleInterviewDto scheduleInterview, CancellationToken cancellationToken);
        Task<List<InterviewDetailedResponseDto>> GetByApplicationIdAsync(Guid applicationId, CancellationToken cancellationToken);
        Task<InterviewResponseDto> UpdateInterviewStatusAsync(Guid id, UpdateInterviewStatusDto updateInterviewStatus, CancellationToken cancellationToken);
        Task<InterviewResponseDto> RescheduleInterviewAsync(Guid id, RescheduleInterviewDto rescheduleInterview, CancellationToken cancellationToken);
        Task<List<InterviewDetailedResponseDto>> InterviewsAssignedToInterviwerAsync(Guid interviewerId, CancellationToken cancellationToken);
        Task<InterviewDetailedResponseDto> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken);
    }
}
