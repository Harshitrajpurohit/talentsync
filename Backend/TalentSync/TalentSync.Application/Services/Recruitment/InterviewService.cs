using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;
using TalentSync.Application.Common.Pagination;
using TalentSync.Application.Common.Workflow;
using TalentSync.Application.DTOs.Notifications;
using TalentSync.Application.DTOs.Recruitment;
using TalentSync.Application.Interfaces;
using TalentSync.Application.Interfaces.Repositories;
using TalentSync.Application.Interfaces.Services;
using TalentSync.Domain.Entities.Recruitment;
using TalentSync.Domain.Entities.User;
using TalentSync.Domain.Enums.Notifications;
using TalentSync.Domain.Enums.Recruitment;
using TalentSync.Domain.Enums.User;

namespace TalentSync.Application.Services.Recruitment
{
    public class InterviewService : IInterviewService
    {
        private readonly IInterviewRepository _interviewRepository;
        private readonly IMapper _mapper;
        private readonly IApplicationRepository _applicationRepository;
        private readonly IScreeningRepository _screeningRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly INotificationService _notificationService;
        private readonly ILogger<InterviewService> _logger;

        public InterviewService(IInterviewRepository interviewRepository, 
            IMapper mapper, 
            IApplicationRepository applicationRepository, 
            IScreeningRepository screeningRepository, 
            IUnitOfWork unitOfWork, 
            IUserRepository userRepository, 
            IUserRoleRepository userRoleRepository, 
            INotificationService notificationService,
            ILogger<InterviewService> logger
            )
        {
            _interviewRepository = interviewRepository;
            _mapper = mapper;
            _applicationRepository = applicationRepository;
            _screeningRepository = screeningRepository;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            _notificationService = notificationService;
            _logger = logger;
        }

        public async Task<InterviewResponseDto> ScheduleInterviewAsync(ScheduleInterviewDto scheduleInterview, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Scheduling interview for application {ApplicationId} with interviewer {InterviewerId} at {ScheduledAt}.", scheduleInterview.ApplicationId, scheduleInterview.InterviewerId, scheduleInterview.ScheduledAt);

            ValidateInterviewScheduleTime(scheduleInterview.ScheduledAt);

            ApplicationEntity? application = await _applicationRepository.GetByIdAsync(scheduleInterview.ApplicationId, cancellationToken)
                ?? throw new KeyNotFoundException($"Application not found.");


            if (!ApplicationStatusValidator.IsValidTransition(application.Status, ApplicationStatus.InterviewScheduled))
            {
                _logger.LogWarning("Invalid application status transition from {CurrentStatus} to {NewStatus} for application {ApplicationId}.", application.Status, ApplicationStatus.InterviewScheduled, application.Id);
                throw new InvalidOperationException(
                    $"Cannot schedule interview from '{application.Status}'.");
            }

            bool? hasPassedScreening = await _screeningRepository.HasPassedScreeningAsync(scheduleInterview.ApplicationId, cancellationToken);

            if(hasPassedScreening != true)
            {
                throw new InvalidOperationException(
                    "Cannot schedule an interview: this application has not passed screening yet. " +
                    "Please complete screening with a Pass result first.");
            }

            Interview? existingInterview = await _interviewRepository.GetByApplicationIdAsync(scheduleInterview.ApplicationId, cancellationToken);


            ValidateExistingInterviewStatus(existingInterview);     

            User? interviewer = await _userRepository.GetUserByIdAsync(scheduleInterview.InterviewerId, cancellationToken);

            ValidateInterviewer(interviewer);

            UserRole? userRole = await _userRoleRepository.GetByUserIdWithRoleAsync(interviewer.Id, cancellationToken);

            ValidateInterviewerRole(userRole);     


            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            Interview newInterview;
            try
            {
                newInterview = _mapper.Map<Interview>(scheduleInterview);
                newInterview.Status = InterviewStatus.Scheduled;

                await _interviewRepository.AddAsync(newInterview, cancellationToken);

                application.Status = ApplicationStatus.InterviewScheduled;
                application.UpdatedAt = DateTime.UtcNow;
                _applicationRepository.Update(application);

                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await _unitOfWork.CommitTransactionAsync(cancellationToken);
                _logger.LogInformation("Interview scheduled successfully for application {ApplicationId} with interviewer {InterviewerId} at {ScheduledAt}.", scheduleInterview.ApplicationId, scheduleInterview.InterviewerId, scheduleInterview.ScheduledAt);
            }
            catch
            {
                _logger.LogError("Error occurred while scheduling interview for application {ApplicationId} with interviewer {InterviewerId} at {ScheduledAt}. Rolling back transaction.", scheduleInterview.ApplicationId, scheduleInterview.InterviewerId, scheduleInterview.ScheduledAt);
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                throw;
            }
            await SendInterviewScheduledNotificationsAsync(application, interviewer.Id, scheduleInterview.ScheduledAt, cancellationToken);

            return _mapper.Map<InterviewResponseDto>(newInterview);
        }

        public async Task<InterviewResponseDto> GetByApplicationIdAsync(Guid applicationId, CancellationToken cancellationToken)
        {
            ApplicationEntity? application = await _applicationRepository.GetByIdAsync(applicationId, cancellationToken)
                ?? throw new KeyNotFoundException($"Application not found.");

            Interview interview = await _interviewRepository.GetByApplicationIdAsync(applicationId, cancellationToken)
                ?? throw new KeyNotFoundException($"Interview not found.");

            return _mapper.Map<InterviewResponseDto>(interview);

        }

        public async Task<InterviewResponseDto> UpdateInterviewStatusAsync(Guid id, UpdateInterviewStatusDto updateInterviewStatus, CancellationToken cancellationToken)
        {

            _logger.LogInformation("Updating interview status for interview {InterviewId} to {NewStatus}.", id, updateInterviewStatus.Status);

            Interview interview = await _interviewRepository.GetByIdAsync(id, cancellationToken) ?? throw new KeyNotFoundException("Interview Not Found");
            var now = DateTime.UtcNow;
            if (interview.ScheduledAt > now && (updateInterviewStatus.Status == InterviewStatus.Passed || updateInterviewStatus.Status == InterviewStatus.Failed))
            {
                _logger.LogWarning("Attempted to update interview status to {NewStatus} for interview {InterviewId} which is scheduled in the future.", updateInterviewStatus.Status, id);
                throw new InvalidOperationException(
                    $"Cannot mark interview as '{updateInterviewStatus.Status}' before it has occurred.");
            }

            if (!InterviewStatusValidator.IsValidTransition(interview.Status, updateInterviewStatus.Status)) {
                _logger.LogWarning("Invalid interview status transition from {CurrentStatus} to {NewStatus} for interview {InterviewId}.", interview.Status, updateInterviewStatus.Status, id);
                throw new InvalidOperationException(
                    $"Cannot change status from '{interview.Status}' to '{updateInterviewStatus.Status}'. ");
            }

            interview.Status = updateInterviewStatus.Status;
            interview.Feedback = updateInterviewStatus.Feedback;
            interview.UpdatedAt = now;

            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            ApplicationEntity application;
            try
            {

                _interviewRepository.Update(interview);

                application  = await _applicationRepository.GetByIdAsync(interview.ApplicationId, cancellationToken)
                    ?? throw new KeyNotFoundException($"Application not found.");

                switch (updateInterviewStatus.Status)
                {
                    case InterviewStatus.Passed:
                        application.Status = ApplicationStatus.InterviewCompleted;
                        application.UpdatedAt = now;
                        break;
                    case InterviewStatus.Failed:
                        application.Status = ApplicationStatus.Rejected;
                        application.UpdatedAt = now;
                        break;
                }

                _applicationRepository.Update(application);

                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await _unitOfWork.CommitTransactionAsync(cancellationToken);

                _logger.LogInformation("Interview status for interview {InterviewId} updated successfully to {NewStatus}.", id, updateInterviewStatus.Status);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating interview {InterviewId}. Rolling back transaction.", id);

                await _unitOfWork.RollbackTransactionAsync(cancellationToken);

                throw;
            }
            await SendInterviewResultNotificationAsync(application, interview, cancellationToken);

            return _mapper.Map<InterviewResponseDto>(interview);
        }

        public async Task<InterviewResponseDto> RescheduleInterviewAsync(Guid id, RescheduleInterviewDto rescheduleInterview, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Rescheduling interview {InterviewId} to new time {NewScheduledAt}.", id, rescheduleInterview.ScheduledAt);
            
            ValidateInterviewScheduleTime(rescheduleInterview.ScheduledAt);

            Interview interview = await _interviewRepository.GetByIdAsync(id, cancellationToken)
                ?? throw new KeyNotFoundException("Interview not found.");

            if (!InterviewStatusValidator.IsValidTransition(
                interview.Status,
                InterviewStatus.Scheduled))
            {
                _logger.LogWarning("Invalid interview status transition from {CurrentStatus} to {NewStatus} for interview {InterviewId}.", interview.Status, InterviewStatus.Scheduled, id);
                throw new InvalidOperationException(
                    $"Cannot reschedule interview from '{interview.Status}'.");
            }

            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            ApplicationEntity? application = null;
            try
            {
                interview.Status = InterviewStatus.Scheduled;
                interview.ScheduledAt = rescheduleInterview.ScheduledAt;
                interview.Location = rescheduleInterview.Location;
                interview.InterviewerId = rescheduleInterview.InterviewerId;
                interview.UpdatedAt = DateTime.UtcNow;

                _interviewRepository.Update(interview);

                application =
                    await _applicationRepository.GetByIdAsync(
                        interview.ApplicationId,
                        cancellationToken) ?? throw new KeyNotFoundException("Application not found.");

                application.Status = ApplicationStatus.InterviewScheduled;
                application.UpdatedAt = DateTime.UtcNow;

                _applicationRepository.Update(application);

                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await _unitOfWork.CommitTransactionAsync(cancellationToken);

                _logger.LogInformation("Interview {InterviewId} rescheduled successfully to new time {NewScheduledAt}.", id, rescheduleInterview.ScheduledAt);
            }
            catch
            {
                _logger.LogError("Error occurred while rescheduling interview {InterviewId} to new time {NewScheduledAt}. Rolling back transaction.", id, rescheduleInterview.ScheduledAt);
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                throw;
            }

            await SendInterviewRescheduledNotificationsAsync(application, interview.InterviewerId, rescheduleInterview.ScheduledAt, cancellationToken);

            return _mapper.Map<InterviewResponseDto>(interview);
        }

        public async Task<PaginationResponse<InterviewDetailedResponseDto>> InterviewsAssignedToInterviwerAsync(Guid interviewerId, InterviewPaginationRequest paginationRequest, CancellationToken cancellationToken)
        {
            User? interviewer = await _userRepository.GetUserByIdAsync(interviewerId, cancellationToken);

            ValidateInterviewer(interviewer);


            UserRole? userRole = await _userRoleRepository.GetByUserIdWithRoleAsync(interviewer.Id, cancellationToken);

            ValidateInterviewerRole(userRole);
            int count = await _interviewRepository.CountByInterviewerIdAsync(interviewerId, paginationRequest, cancellationToken);

            List<Interview> interviews = await _interviewRepository.GetByInterviewerIdAsync(interviewerId, paginationRequest, cancellationToken);
            return new PaginationResponse<InterviewDetailedResponseDto>(
                paginationRequest.PageNumber,
                paginationRequest.PageSize,
                count,
                _mapper.Map<List<InterviewDetailedResponseDto>>(interviews)
            );
        }

        public async Task<InterviewDetailedResponseDto> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken)
        {
            Interview? interview = await _interviewRepository.GetByIdWithDetailsAsync(id, cancellationToken);
            if(interview == null)
            {
                throw new KeyNotFoundException("Interview not found.");
            }

            return _mapper.Map<InterviewDetailedResponseDto>(interview);
        }

        public async Task<PaginationResponse<InterviewDetailedResponseDto>> GetPagedByCandidateIdAsync(Guid candidateId, InterviewPaginationRequest paginationRequest, CancellationToken cancellationToken)
        {
            User? candidate = await _userRepository.GetUserByIdAsync(candidateId, cancellationToken);
            if (candidate == null || candidate.IsDeleted)
            {
                throw new KeyNotFoundException("Candidate Not Found");
            }

            int totalRecords = await _interviewRepository.CountByCandidateIdAsync(candidateId, paginationRequest, cancellationToken);

            List<Interview> interviews =
                await _interviewRepository.GetPagedByCandidateIdAsync(
                    candidateId,
                    paginationRequest,
                    cancellationToken);

            List<InterviewDetailedResponseDto> data = _mapper.Map<List<InterviewDetailedResponseDto>>(interviews);

            return new PaginationResponse<InterviewDetailedResponseDto>(   
                paginationRequest.PageNumber,
                paginationRequest.PageSize,
                totalRecords,
                data
                );
        }

        public async Task<PaginationResponse<CandidateInterviewResponseDto>> GetByCandidateIdAsync(Guid candidateId, InterviewPaginationRequest paginationRequest, CancellationToken cancellationToken)
        {

            int totalRecords = await _interviewRepository.CountByCandidateIdAsync(candidateId, paginationRequest, cancellationToken);

            List<Interview> interviews =
                await _interviewRepository.GetPagedByCandidateIdAsync(
                    candidateId,
                    paginationRequest,
                    cancellationToken);

            List<CandidateInterviewResponseDto> data = _mapper.Map<List<CandidateInterviewResponseDto>>(interviews);

            return new PaginationResponse<CandidateInterviewResponseDto>(
                paginationRequest.PageNumber,
                paginationRequest.PageSize,
                totalRecords,
                data
                );
        }


        // private 

        // validations

        private static void ValidateInterviewScheduleTime(DateTimeOffset scheduledAt)
        {
            DateTimeOffset minimumAllowedTime = DateTimeOffset.UtcNow.AddMinutes(5);

            if (scheduledAt <= minimumAllowedTime)
            {
                throw new InvalidOperationException(
                    "Interview must be scheduled in the future.");
            }
        }

        private static void ValidateExistingInterviewStatus(Interview existingInterview)
        {
            //var activeInterview = existingInterviews.FirstOrDefault(i =>
            //    i.Status == InterviewStatus.Scheduled ||
            //    i.Status == InterviewStatus.Passed ||
            //    i.Status == InterviewStatus.Completed
            //    );
            if (existingInterview == null) {
                return;
            }
            switch (existingInterview?.Status)
            {
                case InterviewStatus.Passed:
                    throw new InvalidOperationException(
                        "Cannot schedule another interview — this candidate has already passed. Proceed to final selection.");


                case InterviewStatus.Scheduled:
                    throw new InvalidOperationException(
                      "Cannot schedule another interview — an active interview already exists for this application. Cancel it first.");


                case InterviewStatus.Completed:
                    throw new InvalidOperationException(
                      "Cannot schedule another interview — this candidate has already Completed Interview. First give the result than try again.");

            }
        }

        private static void ValidateInterviewer(User? interviewer)
        {
            if (interviewer == null)
            {
                throw new KeyNotFoundException("Interviewer not found.");
            }
            if (interviewer.Status != UserStatus.Active)
            {
                throw new InvalidOperationException(
                    "Interviewer account is not active.");
            }
        }

        private static void ValidateInterviewerRole(UserRole? userRole)
        {
            if (userRole == null)
            {
                throw new KeyNotFoundException("Role Not Assigned.");
            }
            if (userRole.Role.Name != RoleName.Manager && userRole.Role.Name != RoleName.HR)
            {
                throw new InvalidOperationException("Selected user cannot conduct interviews.");
            }
        }


        //notifications

        private async Task SendInterviewScheduledNotificationsAsync(ApplicationEntity application, Guid interviewerId, DateTimeOffset scheduledAt, CancellationToken cancellationToken)
        {
            await _notificationService.SendAsync(
                new CreateNotificationDto
                {
                    UserId = application.CandidateId,
                    Title = "Interview Scheduled",
                    Message =
                        $"Your interview for '{application.Job.Title}' has been scheduled on {scheduledAt:dd MMM yyyy hh:mm tt}.",
                    Category = NotificationCategory.Recruitment,
                    Channel = NotificationChannel.InApp
                },
                cancellationToken);

            await _notificationService.SendAsync(
                new CreateNotificationDto
                {
                    UserId = interviewerId,
                    Title = "Interview Assigned",
                    Message =
                        $"You have been assigned to interview a candidate for '{application.Job.Title}' on {scheduledAt:dd MMM yyyy hh:mm tt}.",
                    Category = NotificationCategory.Recruitment,
                    Channel = NotificationChannel.InApp
                },
                cancellationToken);
        }

        private async Task SendInterviewResultNotificationAsync(ApplicationEntity application, Interview interview, CancellationToken cancellationToken)
        {
            if (interview.Status == InterviewStatus.Passed)
            {
                await _notificationService.SendAsync(
                    new CreateNotificationDto
                    {
                        UserId = application.CandidateId,
                        Title = "Interview Result",
                        Message = $"Congratulations! You have successfully cleared the interview for '{application.Job.Title}'.",
                        Category = NotificationCategory.Recruitment,
                        Channel = NotificationChannel.InApp
                    },
                    cancellationToken);

                return;
            }

            if (interview.Status == InterviewStatus.Failed)
            {
                await _notificationService.SendAsync(
                    new CreateNotificationDto
                    {
                        UserId = application.CandidateId,
                        Title = "Interview Result",
                        Message = $"We appreciate your interest. Unfortunately, you were not selected after the interview for '{application.Job.Title}'.",
                        Category = NotificationCategory.Recruitment,
                        Channel = NotificationChannel.InApp
                    },
                    cancellationToken);

                return;
            }

            if (interview.Status == InterviewStatus.Cancelled)
            {
                await _notificationService.SendAsync(
                    new CreateNotificationDto
                    {
                        UserId = application.CandidateId,
                        Title = "Interview Cancelled",
                        Message = $"Your interview for '{application.Job.Title}' has been cancelled. A new schedule will be shared soon.",
                        Category = NotificationCategory.Recruitment,
                        Channel = NotificationChannel.InApp
                    },
                    cancellationToken);

                await _notificationService.SendAsync(
                    new CreateNotificationDto
                    {
                        UserId = interview.InterviewerId,
                        Title = "Interview Cancelled",
                        Message = $"Your interview with '{application.Candidate.Name}, {application.Candidate.Email}' for '{application.Job.Title}' has been cancelled.",
                        Category = NotificationCategory.Recruitment,
                        Channel = NotificationChannel.InApp
                    },
                    cancellationToken);
            }
        }

        private async Task SendInterviewRescheduledNotificationsAsync(ApplicationEntity application, Guid interviewerId, DateTime scheduledAt, CancellationToken cancellationToken)
        {
            await _notificationService.SendAsync(
                new CreateNotificationDto
                {
                    UserId = application.CandidateId,
                    Title = "Interview Rescheduled",
                    Message =
                        $"Your interview for '{application.Job.Title}' has been rescheduled to {scheduledAt:dd MMM yyyy hh:mm tt}.",
                    Category = NotificationCategory.Recruitment,
                    Channel = NotificationChannel.InApp
                },
                cancellationToken);

            await _notificationService.SendAsync(
                new CreateNotificationDto
                {
                    UserId = interviewerId,
                    Title = "Interview Rescheduled",
                    Message =
                        $"The interview assigned to you for '{application.Job.Title}' has been rescheduled to {scheduledAt:dd MMM yyyy hh:mm tt}.",
                    Category = NotificationCategory.Recruitment,
                    Channel = NotificationChannel.InApp
                },
                cancellationToken);
        }


    }
}
