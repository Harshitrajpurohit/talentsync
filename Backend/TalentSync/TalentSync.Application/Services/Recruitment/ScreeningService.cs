using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Application.Common.Workflow;
using TalentSync.Application.DTOs.Notifications;
using TalentSync.Application.DTOs.Recruitment;
using TalentSync.Application.Interfaces;
using TalentSync.Application.Interfaces.Repositories;
using TalentSync.Application.Interfaces.Services;
using TalentSync.Application.Services.Notifications;
using TalentSync.Domain.Entities.Recruitment;
using TalentSync.Domain.Enums.Notifications;
using TalentSync.Domain.Enums.Recruitment;

namespace TalentSync.Application.Services.Recruitment
{
    public class ScreeningService : IScreeningService
    {
        private readonly IScreeningRepository _screeningRepository;
        private readonly IMapper _mapper;
        private readonly IApplicationRepository _applicationRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotificationService _notificationService;
        private readonly ILogger<ScreeningService> _logger;
        public ScreeningService(IScreeningRepository screeningRepository, IMapper mapper, IApplicationRepository applicationRepository, IUnitOfWork unitOfWork, INotificationService notificationService, ILogger<ScreeningService> logger)
        {
            _screeningRepository = screeningRepository;
            _mapper = mapper;
            _applicationRepository = applicationRepository;
            _unitOfWork = unitOfWork;
            _notificationService = notificationService;
            _logger = logger;
        }

        public async Task<ScreeningResponseDto> CreateScreeningAsync(CreateScreeningRequestDto screeningRequestDto, Guid screenedById, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creating screening for application {ApplicationId} by user {ScreenedById}", screeningRequestDto.ApplicationId, screenedById);

            ApplicationEntity application = await _applicationRepository.GetByIdAsync(screeningRequestDto.ApplicationId, cancellationToken) ?? throw new KeyNotFoundException($"Application not found.");


            if (!ApplicationStatusValidator.IsValidTransition(application.Status, ApplicationStatus.Screening))
            {
                _logger.LogWarning("Invalid status transition from {CurrentStatus} to {NewStatus} for application {ApplicationId}", application.Status, ApplicationStatus.Screening, screeningRequestDto.ApplicationId);
                throw new InvalidOperationException(
                    $"Cannot screen an application with status '{application.Status}'.");
            }

            bool alreadyScreened = await _screeningRepository.ExistsByApplicationIdAsync(screeningRequestDto.ApplicationId, cancellationToken);
            if (alreadyScreened)
            {
                throw new InvalidOperationException(
                    "Application has already been screened.");
            }

            Screening screening = _mapper.Map<Screening>(screeningRequestDto);
            screening.ScreeningDate = DateTime.UtcNow;
            screening.ScreenedById = screenedById;

            Screening newScreening = new Screening();
            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                newScreening = await _screeningRepository.AddAsync(screening, cancellationToken);
                if (screeningRequestDto.Result == ScreeningResult.Pass)
                {
                    application.Status = ApplicationStatus.Screening;
                    application.UpdatedAt = DateTime.UtcNow;
                    _applicationRepository.Update(application);

                }
                else if (screeningRequestDto.Result == ScreeningResult.Fail)
                {
                    application.Status = ApplicationStatus.Rejected;
                    application.UpdatedAt = DateTime.UtcNow;
                    _applicationRepository.Update(application);

                }
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await _unitOfWork.CommitTransactionAsync(cancellationToken);


                _logger.LogInformation("Screening created successfully for application {ApplicationId} with result {Result}", screeningRequestDto.ApplicationId, screeningRequestDto.Result);
            }
            catch
            {
                _logger.LogError("Error occurred while creating screening for application {ApplicationId}. Rolling back transaction.", screeningRequestDto.ApplicationId);
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                throw;
            }

            await SendScreeningNotificationAsync(application, screeningRequestDto.Result, cancellationToken);
            return _mapper.Map<ScreeningResponseDto>(newScreening);
        }

        public async Task<ScreeningResponseDto> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            Screening screening = await _screeningRepository.GetByIdAsync(id, cancellationToken) ?? throw new KeyNotFoundException("Screening Not Found");

            return _mapper.Map<ScreeningResponseDto>(screening);
        }

        public async Task<ScreeningResponseDto> GetByApplicationIdAsync(Guid applicationId, Guid authorizedUserId, CancellationToken cancellationToken)
        {

            Screening screening = await _screeningRepository.GetByApplicationIdAsync(applicationId, cancellationToken) ?? throw new KeyNotFoundException("Screening Not Found with Application");

            return _mapper.Map<ScreeningResponseDto>(screening);
        }

        public async Task<ScreeningResponseDto> UpdateScreeningAsync(Guid id, UpdateScreeningRequestDto updateScreeningRequest, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Updating screening {ScreeningId} with new result {NewResult}", id, updateScreeningRequest.Result);

            if (updateScreeningRequest.Result == ScreeningResult.Pending)
            {

                throw new InvalidOperationException(
                    "screenings cannot be reverted to Pending.");
            }

            Screening screening = await _screeningRepository.GetByIdAsync(id, cancellationToken) ?? throw new KeyNotFoundException("Screening Not Found");

            if (screening.Result == updateScreeningRequest.Result)
            {
                throw new InvalidOperationException(
                    "Screening already has this result.");
            }

            ApplicationEntity? application = await _applicationRepository.GetByIdAsync(screening.ApplicationId, cancellationToken);

            if (application == null)
            {
                throw new KeyNotFoundException("Application not found.");
            }

            if (application.Status is
                    ApplicationStatus.InterviewScheduled or
                    ApplicationStatus.InterviewCompleted or
                    ApplicationStatus.Selected or
                    ApplicationStatus.Rejected)
            {
                _logger.LogWarning("Cannot update screening for application {ApplicationId} because it is in '{CurrentStatus}' state", application.Id, application.Status);
                throw new InvalidOperationException(
                    $"Cannot update screening because application is in '{application.Status}' state.");
            }


            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                _mapper.Map(updateScreeningRequest, screening);
                screening.UpdatedAt = DateTime.UtcNow;
                if (updateScreeningRequest.Result == ScreeningResult.Pass)
                {
                    application.Status = ApplicationStatus.Screening;
                }
                else if (updateScreeningRequest.Result == ScreeningResult.Fail)
                {
                    application.Status = ApplicationStatus.Rejected;
                }
                application.UpdatedAt = DateTime.UtcNow;


                _screeningRepository.Update(screening);
                _applicationRepository.Update(application);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await _unitOfWork.CommitTransactionAsync(cancellationToken);
                _logger.LogInformation("Screening {ScreeningId} updated successfully with new result {NewResult}", id, updateScreeningRequest.Result);
            }
            catch
            {
                _logger.LogError("Error occurred while updating screening {ScreeningId}. Rolling back transaction.", id);
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                throw;
            }

            await SendScreeningNotificationAsync(application, updateScreeningRequest.Result, cancellationToken);
            return _mapper.Map<ScreeningResponseDto>(screening);
        }



        // private methods

        private async Task SendScreeningNotificationAsync(ApplicationEntity application, ScreeningResult result, CancellationToken cancellationToken)
        {
            await _notificationService.SendAsync(
                   new CreateNotificationDto
                   {
                       UserId = application.CandidateId,
                       Title = "Application Screening Result",
                       Message = result == ScreeningResult.Pass
                            ? $"Congratulations! Your application for '{application.Job.Title}' has passed the screening stage."
                            : $"Unfortunately, your application for '{application.Job.Title}' did not pass the screening stage.",
                       Category = NotificationCategory.Recruitment,
                       Channel = NotificationChannel.InApp
                   },
                   cancellationToken);
        }
    }
}
