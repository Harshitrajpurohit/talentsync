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
using TalentSync.Domain.Entities.HumanResources;
using TalentSync.Domain.Entities.Recruitment;
using TalentSync.Domain.Entities.User;
using TalentSync.Domain.Enums.Employees;
using TalentSync.Domain.Enums.Notifications;
using TalentSync.Domain.Enums.Recruitment;

namespace TalentSync.Application.Services.Recruitment
{
    public class SelectionService : ISelectionService
    {
        private readonly ISelectionRepository _selectionRepository;
        private readonly IMapper _mapper;
        private readonly IApplicationRepository _applicationRepository;
        private readonly IInterviewRepository _interviewRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly INotificationService _notificationService;
        private readonly ILogger<SelectionService> _logger;


        public SelectionService(ISelectionRepository selectionRepository,
            IMapper mapper,
            IApplicationRepository applicationRepository,
            IInterviewRepository interviewRepository,
            IUnitOfWork unitOfWork,
            IEmployeeRepository employeeRepository,
            IUserRoleRepository userRoleRepository,
            IRoleRepository roleRepository,
            INotificationService notificationService,
            ILogger<SelectionService> logger
            )
        {
            _selectionRepository = selectionRepository;
            _mapper = mapper;
            _applicationRepository = applicationRepository;
            _interviewRepository = interviewRepository;
            _unitOfWork = unitOfWork;
            _employeeRepository = employeeRepository;
            _userRoleRepository = userRoleRepository;
            _roleRepository = roleRepository;
            _notificationService = notificationService;
            _logger = logger;
        }

        public async Task<SelectionResponseDto> MakeDecisionAsync(CreateSelectionDecisionDto createSelectionDecision, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Making selection decision for application with ID: {ApplicationId}", createSelectionDecision.ApplicationId);
            if (createSelectionDecision.Decision == SelectionDecision.Pending)
            {
                throw new InvalidOperationException(
                    "Selection decision cannot be Pending.");
            }

            ApplicationEntity application = await _applicationRepository
                .GetByIdWithDetailsAsync(createSelectionDecision.ApplicationId, cancellationToken)
                ?? throw new KeyNotFoundException("Application not found.");

            if (application.Status == ApplicationStatus.Rejected)
            {
                _logger.LogWarning("Attempted to make a decision on a rejected application with ID: {ApplicationId}", createSelectionDecision.ApplicationId);
                throw new InvalidOperationException(
                    "Application is already rejected and cannot receive a new decision.");
            }

            Selection? existingSelection = await _selectionRepository
                .GetByApplicationIdAsync(application.Id, cancellationToken);

            if (existingSelection != null)
            {
                throw new InvalidOperationException(
                    "A selection decision already exists for this application.");
            }

            bool hasPassedInterview = (await _interviewRepository
                .GetByApplicationIdAsync(application.Id, cancellationToken))
                .Any(i => i.Status == InterviewStatus.Passed);

            if (!hasPassedInterview)
            {
                throw new InvalidOperationException(
                    "Candidate has not passed the interview.");
            }

            ApplicationStatus targetStatus =
                createSelectionDecision.Decision == SelectionDecision.Selected
                    ? ApplicationStatus.Selected
                    : ApplicationStatus.Rejected;

            if (!ApplicationStatusValidator.IsValidTransition(application.Status, targetStatus))
            {
                throw new InvalidOperationException(
                    $"Cannot change application status from '{application.Status}' to '{targetStatus}'.");
            }

            Selection selection = _mapper.Map<Selection>(createSelectionDecision);
            selection.SelectionDate = DateTime.UtcNow;

            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            Selection newSelection;
            try
            {
                if (createSelectionDecision.Decision == SelectionDecision.Selected)
                {
                    application.Status = ApplicationStatus.Selected;

                    await ConvertCandidateToEmployeeAsync(
                        application,
                        createSelectionDecision,
                        cancellationToken);

                    await AssignRoleToEmployee(application.CandidateId, cancellationToken);
                }
                else
                {
                    application.Status = ApplicationStatus.Rejected;
                }

                application.UpdatedAt = DateTime.UtcNow;

                newSelection = await _selectionRepository.AddAsync(
                    selection,
                    cancellationToken);

                _applicationRepository.Update(application);

                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await _unitOfWork.CommitTransactionAsync(cancellationToken);

                _logger.LogInformation("Selection decision made successfully for application with ID: {ApplicationId}", createSelectionDecision.ApplicationId);
            }
            catch
            {
                _logger.LogError("An error occurred while making selection decision for application with ID: {ApplicationId}. Rolling back transaction.", createSelectionDecision.ApplicationId);
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                throw;
            }

            await SendSelectionNotificationAsync(application, createSelectionDecision.Decision, cancellationToken);
            return _mapper.Map<SelectionResponseDto>(newSelection);
        }

        public async Task<SelectionWithDetailsResponseDto?> GetByApplicationIdAsync(Guid applicationId, CancellationToken cancellationToken)
        {
            Selection? selection = await _selectionRepository.GetByApplicationIdAsync(applicationId, cancellationToken);
            if (selection == null)
            {
                throw new KeyNotFoundException("Selection not found for the given applicationId.");
            }
            return _mapper.Map<SelectionWithDetailsResponseDto>(selection);

        }


        // private

        private async Task ConvertCandidateToEmployeeAsync(ApplicationEntity application, CreateSelectionDecisionDto createSelectionDecision, CancellationToken cancellationToken)
        {
            Employee? existingEmployee = await _employeeRepository.GetByUserIdAsync(application.CandidateId, cancellationToken);

            if(existingEmployee != null)
            {
                return;
            }

            Employee employee = new Employee
            {
                UserId = application.CandidateId,
                EmployeeCode = $"EMP-{DateTime.UtcNow:yyyy}-{Guid.NewGuid().ToString("N")[..6].ToUpper()}",
                DepartmentName = createSelectionDecision.Department ?? application.Job.Department ?? "General",
                Position = createSelectionDecision.Position ?? application.Job.Title ?? "Employee",
                Status = EmployeeStatus.Active,
                JoinDate = DateTime.UtcNow,
            };

            await _employeeRepository.AddAsync(employee, cancellationToken);
        }

        private async Task AssignRoleToEmployee(Guid userId, CancellationToken cancellationToken)
        {
            UserRole? userRole = await _userRoleRepository.GetByUserIdAsync(userId, cancellationToken) ??
                throw new KeyNotFoundException("Role Not assigned to this user");
            Role? role = await _roleRepository.GetRoleByRoleNameAsync(Domain.Enums.User.RoleName.Employee, cancellationToken) ??
                throw new KeyNotFoundException("Role Not Found.");

            if(userRole.Role.Name == Domain.Enums.User.RoleName.Candidate)
            {
                userRole.RoleId = role.Id;
                userRole.UpdatedAt = DateTime.UtcNow;
                _userRoleRepository.Update(userRole);
            }
        }


        private async Task SendSelectionNotificationAsync(ApplicationEntity application, SelectionDecision decision, CancellationToken cancellationToken)
        {

            await _notificationService.SendAsync(
                new CreateNotificationDto
                {
                    UserId = application.CandidateId,
                    Title = decision == SelectionDecision.Selected
                        ? "Congratulations! You're Selected"
                        : "Application Update",

                    Message = decision == SelectionDecision.Selected
                        ? $"Congratulations! You have been selected for the position of '{application.Job.Title}'. Our HR team will reach out shortly regarding your offer and onboarding process."
                        : $"Thank you for your interest in '{application.Job.Title}'. Although you were not selected for this position, we appreciate the time you invested in the recruitment process and encourage you to apply for future opportunities.",

                    Category = NotificationCategory.Recruitment,
                    Channel = NotificationChannel.InApp
                },
                cancellationToken);
        }
    }
}
