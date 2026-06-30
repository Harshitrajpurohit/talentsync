using AutoMapper;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;
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
using static System.Net.Mime.MediaTypeNames;

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

        public InterviewService(IInterviewRepository interviewRepository, 
            IMapper mapper, 
            IApplicationRepository applicationRepository, 
            IScreeningRepository screeningRepository, 
            IUnitOfWork unitOfWork, 
            IUserRepository userRepository, 
            IUserRoleRepository userRoleRepository, 
            INotificationService notificationService)
        {
            _interviewRepository = interviewRepository;
            _mapper = mapper;
            _applicationRepository = applicationRepository;
            _screeningRepository = screeningRepository;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            _notificationService = notificationService;
        }

        public async Task<InterviewResponseDto> ScheduleInterviewAsync(ScheduleInterviewDto scheduleInterview, CancellationToken cancellationToken)
        {

            if (scheduleInterview.ScheduledAt <= DateTime.UtcNow.AddMinutes(5))
            {
                throw new InvalidOperationException(
                    "Interview must be scheduled in the future.");
            }

            ApplicationEntity? application = await _applicationRepository.GetByIdAsync(scheduleInterview.ApplicationId, cancellationToken)
                ?? throw new KeyNotFoundException($"Application not found.");


            if (!ApplicationStatusValidator.IsValidTransition(application.Status, ApplicationStatus.InterviewScheduled))
            {
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

            List<Interview> existingInterviews = await _interviewRepository.GetByApplicationIdAsync(scheduleInterview.ApplicationId, cancellationToken);

            var activeInterview = existingInterviews.FirstOrDefault(i =>
                i.Status == InterviewStatus.Scheduled ||
                i.Status == InterviewStatus.Passed || 
                i.Status == InterviewStatus.Completed
                );

            switch (activeInterview?.Status)
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

            User? interviewer = await _userRepository.GetUserByIdAsync(scheduleInterview.InterviewerId, cancellationToken);

            if (interviewer == null)
            {
                throw new KeyNotFoundException("Interviewer not found.");
            }
            if (interviewer.Status != UserStatus.Active)
            {
                throw new InvalidOperationException(
                    "Interviewer account is not active.");
            }

            UserRole? userRole = await _userRoleRepository.GetByUserIdWithRoleAsync(interviewer.Id, cancellationToken);
            if (userRole == null)
            {
                throw new KeyNotFoundException("Role Not Assigned ");
            }
            if(userRole.Role.Name != RoleName.Manager && userRole.Role.Name != RoleName.HR && userRole.Role.Name != RoleName.Recruiter)
            {
                throw new InvalidOperationException("Selected user cannot conduct interviews.");
            }

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
                
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                throw;
            }

            await SendInterviewScheduledNotificationsAsync(application, interviewer.Id, scheduleInterview.ScheduledAt, cancellationToken);

            return _mapper.Map<InterviewResponseDto>(newInterview);
        }

        public async Task<List<InterviewDetailedResponseDto>> GetByApplicationIdAsync(Guid applicationId, CancellationToken cancellationToken)
        {
            ApplicationEntity? application = await _applicationRepository.GetByIdAsync(applicationId, cancellationToken)
                ?? throw new KeyNotFoundException($"Application not found.");

            List<Interview> interviews = await _interviewRepository.GetByApplicationIdAsync(applicationId, cancellationToken);

            return _mapper.Map<List<InterviewDetailedResponseDto>>(interviews);

        }

        public async Task<InterviewResponseDto> UpdateInterviewStatusAsync(Guid id, UpdateInterviewStatusDto updateInterviewStatus, CancellationToken cancellationToken)
        {
            Interview interview = await _interviewRepository.GetByIdAsync(id, cancellationToken) ?? throw new KeyNotFoundException("Interview Not Found");

            if (!InterviewStatusValidator.IsValidTransition(interview.Status, updateInterviewStatus.Status)) {
                throw new InvalidOperationException(
                    $"Cannot change status from '{interview.Status}' to '{updateInterviewStatus.Status}'. ");
            }

            interview.Status = updateInterviewStatus.Status;
            interview.Feedback = updateInterviewStatus.Feedback;
            interview.UpdatedAt = DateTime.UtcNow;

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
                        application.UpdatedAt = DateTime.UtcNow;
                        break;
                    case InterviewStatus.Failed:
                        application.Status = ApplicationStatus.Rejected;
                        application.UpdatedAt = DateTime.UtcNow;
                        break;
                }

                _applicationRepository.Update(application);

                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await _unitOfWork.CommitTransactionAsync(cancellationToken);

                
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                throw;
            }

            await SendInterviewResultNotificationAsync(application, updateInterviewStatus.Status, cancellationToken);

            return _mapper.Map<InterviewResponseDto>(interview);
        }

        public async Task<InterviewResponseDto> RescheduleInterviewAsync(Guid id, RescheduleInterviewDto rescheduleInterview, CancellationToken cancellationToken)
        {
            Interview interview = await _interviewRepository.GetByIdAsync(id, cancellationToken)
                ?? throw new KeyNotFoundException("Interview not found.");

            if (!InterviewStatusValidator.IsValidTransition(
                interview.Status,
                InterviewStatus.Scheduled))
            {
                throw new InvalidOperationException(
                    $"Cannot reschedule interview from '{interview.Status}'.");
            }

            if (rescheduleInterview.ScheduledAt <= DateTime.UtcNow.AddMinutes(5))
            {
                throw new InvalidOperationException(
                    "Interview must be scheduled at least 5 minutes in the future.");
            }

            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            ApplicationEntity? application = null;
            try
            {
                interview.Status = InterviewStatus.Scheduled;
                interview.ScheduledAt = rescheduleInterview.ScheduledAt;
                interview.Location = rescheduleInterview.Location;
                interview.UpdatedAt = DateTime.UtcNow;

                _interviewRepository.Update(interview);

                application =
                    await _applicationRepository.GetByIdAsync(
                        interview.ApplicationId,
                        cancellationToken) ?? throw new KeyNotFoundException("Application not found.");

                if (application != null)
                {
                    application.Status = ApplicationStatus.InterviewScheduled;
                    application.UpdatedAt = DateTime.UtcNow;

                    _applicationRepository.Update(application);
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await _unitOfWork.CommitTransactionAsync(cancellationToken);

                
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                throw;
            }

            await SendInterviewRescheduledNotificationsAsync(application, interview.InterviewerId, rescheduleInterview.ScheduledAt, cancellationToken);

            return _mapper.Map<InterviewResponseDto>(interview);
        }

        public async Task<List<InterviewDetailedResponseDto>> InterviewsAssignedToInterviwerAsync(Guid interviewerId, CancellationToken cancellationToken)
        {
            User? interviewer = await _userRepository.GetUserByIdAsync(interviewerId, cancellationToken);

            if (interviewer == null)
            {
                throw new KeyNotFoundException("Interviewer not found.");
            }
            if (interviewer.Status != UserStatus.Active)
            {
                throw new InvalidOperationException(
                    "Interviewer account is not active.");
            }

            UserRole? userRole = await _userRoleRepository.GetByUserIdWithRoleAsync(interviewer.Id, cancellationToken);
            if (userRole == null)
            {
                throw new KeyNotFoundException("Role Not Assigned ");
            }
            if (userRole.Role.Name != RoleName.Manager && userRole.Role.Name != RoleName.HR && userRole.Role.Name != RoleName.Recruiter)
            {
                throw new InvalidOperationException("Selected user cannot conduct interviews.");
            }


            List<Interview> interviews = await _interviewRepository.GetByInterviewerIdAsync(interviewerId, cancellationToken);


            return _mapper.Map<List<InterviewDetailedResponseDto>>(interviews);
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


        // private 

        private async Task SendInterviewScheduledNotificationsAsync(ApplicationEntity application, Guid interviewerId, DateTime scheduledAt, CancellationToken cancellationToken)
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

        private async Task SendInterviewResultNotificationAsync(ApplicationEntity application, InterviewStatus status, CancellationToken cancellationToken)
        {
            if (status != InterviewStatus.Passed &&
                status != InterviewStatus.Failed)
                return;

            await _notificationService.SendAsync(
                new CreateNotificationDto
                {
                    UserId = application.CandidateId,
                    Title = "Interview Result",
                    Message = status == InterviewStatus.Passed
                        ? $"Congratulations! You have successfully cleared the interview for '{application.Job.Title}'."
                        : $"We appreciate your interest. Unfortunately, you were not selected after the interview for '{application.Job.Title}'.",
                    Category = NotificationCategory.Recruitment,
                    Channel = NotificationChannel.InApp
                },
                cancellationToken);
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
