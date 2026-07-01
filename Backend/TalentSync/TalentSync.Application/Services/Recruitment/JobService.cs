using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Application.Common.Pagination;
using TalentSync.Application.DTOs.Notifications;
using TalentSync.Application.DTOs.Recruitment;
using TalentSync.Application.DTOs.User;
using TalentSync.Application.Interfaces.Repositories;
using TalentSync.Application.Interfaces.Services;
using TalentSync.Domain.Entities.Recruitment;
using TalentSync.Domain.Entities.User;
using TalentSync.Domain.Enums.Notifications;
using TalentSync.Domain.Enums.Recruitment;
using TalentSync.Domain.Enums.User;

namespace TalentSync.Application.Services.Recruitment
{
    public class JobService : IJobService
    {
        private readonly IJobRepository _jobRepository;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly INotificationService _notificationService;
        private readonly ILogger<JobService> _logger;

        public JobService(IJobRepository jobRepository, IMapper mapper, IUserRepository userRepository, INotificationService notificationService, ILogger<JobService> logger)
        {
            _jobRepository = jobRepository;
            _mapper = mapper;
            _userRepository = userRepository;
            _notificationService = notificationService;
            _logger = logger;
        }

        public async Task<JobResponseDto> CreateJobAsync(CreateJobDto jobDto, Guid hrId, CancellationToken cancellationToken)
        {

            _logger.LogInformation("Creating a new job for HR with ID: {HRId}", hrId);
            User? user = await _userRepository.GetUserByIdAsync(hrId, cancellationToken);


            ValidateUserStatus(user);

            Job job = _mapper.Map<Job>(jobDto);
            job.PostedDate = DateTime.UtcNow;
            job.CreatedAt = DateTime.UtcNow;
            job.Status = JobStatus.Open;
            job.HRId = hrId;

            Job newJob = await _jobRepository.AddAsync(job, cancellationToken);
            await _jobRepository.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Job created successfully with ID: {JobId}", newJob.Id);
            await _notificationService.SendAsync(
                        new CreateNotificationDto
                        {
                            UserId = hrId,
                            Title = "Job Posted",
                            Message = $"Your job '{job.Title}' has been published successfully.",
                            Category = NotificationCategory.Recruitment,
                            Channel = NotificationChannel.InApp
                        },
                        cancellationToken);

            return _mapper.Map<JobResponseDto>(newJob);
        }

        public async Task<PaginationResponse<JobListDto>> GetAllJobsAsync(PaginationRequest paginationRequest, CancellationToken cancellationToken)
        {
            List<Job> jobs = await _jobRepository.GetPagedJobsAsync(paginationRequest, cancellationToken);
            int totalCount = await _jobRepository.CountAsync(cancellationToken);

            List<JobListDto> result = _mapper.Map<List<JobListDto>>(jobs);

            return new PaginationResponse<JobListDto>
            (
                pageNumber : paginationRequest.PageNumber,
                pageSize : paginationRequest.PageSize,
                totalRecords : totalCount,
                data : result
            );

        }

        public async Task<JobResponseDto> GetJobByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            Job job = await _jobRepository.GetJobByIdAsync(id, cancellationToken) ?? throw new KeyNotFoundException("Job Not Found.");

            return _mapper.Map<JobResponseDto>(job);
        }

        public async Task<JobResponseDto> UpdateJobAsync(Guid id, Guid userId,UpdateJobRequestDto updateJobRequestDto, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Updating job with ID: {JobId} for user with ID: {UserId}", id, userId);
            Job job = await _jobRepository.GetJobByIdAsync(id, cancellationToken) ?? throw new KeyNotFoundException("Job Not Found.");

            if (job.HRId != userId)
            {
                _logger.LogWarning("Unauthorized update attempt for job with ID: {JobId} by user with ID: {UserId}", id, userId);
                throw new UnauthorizedAccessException(
                    "You can only update your own jobs.");
            }

            _mapper.Map(updateJobRequestDto, job);
            job.UpdatedAt = DateTime.UtcNow;
            
            _jobRepository.UpdateJob(job);
            await _jobRepository.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Job with ID: {JobId} updated successfully.", id);
            return _mapper.Map<JobResponseDto>(job);

        }

        public async Task<bool> DeleteJobAsync(Guid id,Guid userId, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting job with ID: {JobId} for user with ID: {UserId}", id, userId);
            Job job = await _jobRepository.GetJobByIdAsync(id, cancellationToken) ?? throw new KeyNotFoundException("Job Not Found.");

            if (job.HRId != userId)
            {
                throw new UnauthorizedAccessException(
                    "You can only delete your own jobs.");
            }
            job.UpdatedAt = DateTime.UtcNow;
            _jobRepository.DeleteJob(job);

            await _jobRepository.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Job with ID: {JobId} deleted successfully.", id);
            return true;

        }


        // private 
        private static void ValidateUserStatus(User? user)
        {

            if (user == null || user.IsDeleted)
            {
                throw new KeyNotFoundException("User not found.");
            }
            if (user.Status != UserStatus.Active)
            {
                throw new InvalidOperationException("User account is not active.");
            }
        }
    }
}
