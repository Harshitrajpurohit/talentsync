using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Application.Common.Pagination;
using TalentSync.Application.DTOs.Recruitment;
using TalentSync.Application.DTOs.User;
using TalentSync.Domain.Entities.Recruitment;

namespace TalentSync.Application.Interfaces.Services
{
    public interface IJobService
    {
        Task<JobResponseDto> CreateJobAsync(CreateJobDto jobDto, Guid HrId, CancellationToken cancellationToken);
        Task<bool> DeleteJobAsync(Guid id, Guid userId, CancellationToken cancellationToken);
        Task<PaginationResponse<JobListDto>> GetAllJobsAsync(PaginationRequest paginationRequest, CancellationToken cancellationToken);
        Task<JobResponseDto> GetJobByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<JobResponseDto> UpdateJobAsync(Guid id, Guid userId,UpdateJobRequestDto updateJobRequestDto, CancellationToken cancellationToken);
    }
}
