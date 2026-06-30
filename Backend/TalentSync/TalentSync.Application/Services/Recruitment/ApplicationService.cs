using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Application.Common.Pagination;
using TalentSync.Application.Common.Workflow;
using TalentSync.Application.DTOs.Notifications;
using TalentSync.Application.DTOs.Recruitment;
using TalentSync.Application.Interfaces.Repositories;
using TalentSync.Application.Interfaces.Services;
using TalentSync.Domain.Entities.Recruitment;
using TalentSync.Domain.Entities.User;
using TalentSync.Domain.Enums.Recruitment;
using TalentSync.Domain.Enums.User;

namespace TalentSync.Application.Services.Recruitment
{
    public class ApplicationService : IApplicationService
    {
        private readonly IApplicationRepository _applicationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IJobRepository _jobRepository;
        private readonly IMapper _mapper;
        private readonly IResumeRepository _resumeRepository;
        
        private readonly INotificationService _notificationService;

        public ApplicationService(IApplicationRepository applicationRepository,
            IUserRepository userRepository,
            IMapper mapper,
            IJobRepository jobRepository,
            IResumeRepository resumeRepository, 
            INotificationService notificationService)
        {
            _applicationRepository = applicationRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _jobRepository = jobRepository;
            _resumeRepository = resumeRepository;
            _notificationService = notificationService;
        }

        public async Task<ApplicationResponseDto> CreateApplicationAsync(CreateApplicationDto createApplicationDto,Guid candidateId, CancellationToken cancellationToken)
        {
            //Resume? resume = await _resumeRepository.GetByCandidateId(candidateId, cancellationToken);
            //if (resume == null)
            //{
            //    throw new InvalidOperationException("Resume Not Uploaded, Upload it First.");
            //}

            ApplicationEntity application = _mapper.Map<ApplicationEntity>(createApplicationDto);

            Job? job = await _jobRepository.GetJobByIdAsync(createApplicationDto.JobId, cancellationToken);
            if (job == null || job.IsDeleted)
            {
                throw new KeyNotFoundException("Job Not Available");
            }
            if(job.Status == JobStatus.Closed)
            {
                throw new InvalidOperationException($"Job '{job.Title}' is not accepting applications.");
            }

            User? user = await _userRepository.GetUserByIdAsync(candidateId, cancellationToken);
            if (user == null || user.IsDeleted)
            {
                throw new KeyNotFoundException("User Not Available");
            }
            if(user.Status != UserStatus.Active)
            {
                throw new InvalidOperationException("Candidate account is not active.");
            }

            bool alreadyApplied = await _applicationRepository.ExistsAsync(createApplicationDto.JobId, candidateId, cancellationToken);
            if (alreadyApplied)
            {
                throw new InvalidOperationException(
                    "You have already applied for this job.");
            }
            application.CandidateId = candidateId;
            application.SubmittedDate = DateTime.UtcNow;
            application.CreatedAt = DateTime.UtcNow;
            application.Status = ApplicationStatus.Submitted;

            ApplicationEntity newApplication = await _applicationRepository.AddAsync(application, cancellationToken);
            await _applicationRepository.SaveChangesAsync(cancellationToken);

            CreateNotificationDto createNotification = new CreateNotificationDto
            {
                UserId = user.Id,
                Title = $"Application Submitted.",
                Message = $"Application Submitted for {job.Title}",
                Category = Domain.Enums.Notifications.NotificationCategory.Recruitment,
                Channel = Domain.Enums.Notifications.NotificationChannel.InApp
            };
            
            await _notificationService.SendAsync(createNotification, cancellationToken);

            return _mapper.Map<ApplicationResponseDto>(newApplication);
        }

        public async Task<ApplicationWithDetailsResponseDto> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            ApplicationEntity? entity = await _applicationRepository.GetByIdWithDetailsAsync(id, cancellationToken);
            if (entity == null || entity.IsDeleted)
            {
                throw new KeyNotFoundException("Application Not Available");
            }
            return _mapper.Map<ApplicationWithDetailsResponseDto>(entity);
        }

        public async Task<PaginationResponse<ApplicationWithDetailsResponseDto>> GetAllAsync(PaginationRequest paginationRequest ,CancellationToken cancellationToken)
        {
            int count = await _applicationRepository.CountAsync(cancellationToken);
            List<ApplicationEntity> list = await _applicationRepository.GetPagedApplicationsAsync(paginationRequest, cancellationToken);

            List<ApplicationWithDetailsResponseDto> applicationWithDetails = _mapper.Map<List<ApplicationWithDetailsResponseDto>>(list);

            return new PaginationResponse<ApplicationWithDetailsResponseDto>
            (
                pageNumber : paginationRequest.PageNumber,
                pageSize : paginationRequest.PageSize,
                totalRecords : count,
                data : applicationWithDetails
            );
        }


        public async Task<List<ApplicationWithDetailsResponseDto>> GetByJobIdAsync(Guid jobId, CancellationToken cancellationToken)
        {
            Job? job = await _jobRepository.GetJobByIdAsync(jobId, cancellationToken);
            if (job == null || job.IsDeleted)
            {
                throw new KeyNotFoundException("Job Not Found");
            }

            List<ApplicationEntity> list = await _applicationRepository.GetByJobIdAsync(jobId, cancellationToken);
            List<ApplicationWithDetailsResponseDto> applications = _mapper.Map<List<ApplicationWithDetailsResponseDto>>(list);
            return applications;
        }

        public async Task<List<ApplicationWithDetailsResponseDto>> GetByCandidateIdAsync(Guid candidateId, CancellationToken cancellationToken)
        {
            User? candidate = await _userRepository.GetUserByIdAsync(candidateId, cancellationToken);
            if (candidate == null || candidate.IsDeleted)
            {
                throw new KeyNotFoundException("Candidate Not Found");
            }

            List<ApplicationEntity> list = await _applicationRepository.GetByCandidateIdAsync(candidateId, cancellationToken);
            List<ApplicationWithDetailsResponseDto> applications = _mapper.Map<List<ApplicationWithDetailsResponseDto>>(list);
            return applications;
        }

        public async Task<ApplicationResponseDto?> UpdateApplicationAsync(Guid id, UpdateApplicationRequestDto updateApplicationRequestDto, CancellationToken cancellationToken)
        {
            var application = await _applicationRepository.GetByIdAsync(id, cancellationToken);
            if (application == null){
                throw new KeyNotFoundException("Application Not Available");
            }

            if(!ApplicationStatusValidator.IsValidTransition(application.Status, updateApplicationRequestDto.Status))
            {
                throw new InvalidOperationException($"Cannot change application status from {application.Status} to {updateApplicationRequestDto.Status}");
            }

            _mapper.Map(updateApplicationRequestDto, application);
            application.UpdatedAt = DateTime.UtcNow;
            _applicationRepository.Update(application);
            await _applicationRepository.SaveChangesAsync(cancellationToken);
            return _mapper.Map<ApplicationResponseDto>(application);
        }

        public async Task<bool> DeleteApplicationAsync(Guid id, CancellationToken cancellationToken)
        {
            var application = await _applicationRepository.GetByIdAsync(id, cancellationToken);
            if (application == null)
            {
                throw new KeyNotFoundException("Application Not Available");
            }
            _applicationRepository.Delete(application);
            application.UpdatedAt = DateTime.UtcNow;
            await _applicationRepository.SaveChangesAsync(cancellationToken);
            return true;
        }

    }
}
