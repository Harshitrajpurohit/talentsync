using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Application.Common.Workflow;
using TalentSync.Application.DTOs.Recruitment;
using TalentSync.Application.Interfaces;
using TalentSync.Application.Interfaces.Repositories;
using TalentSync.Application.Interfaces.Services;
using TalentSync.Domain.Entities.HumanResources;
using TalentSync.Domain.Entities.Recruitment;
using TalentSync.Domain.Entities.User;
using TalentSync.Domain.Enums.Employees;
using TalentSync.Domain.Enums.Recruitment;

namespace TalentSync.Application.Services.Recruitment
{
    public class SelectionService : ISelectionService
    {
        private readonly ISelectionRepository _selectionRepository;
        private readonly IMapper _mapper;
        private readonly IApplicationRepository _applicationRepository;
        private readonly IInterviewRepository _interviewRepository;
        private readonly IScreeningRepository _screeningRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IRoleRepository _roleRepository;

        public SelectionService(ISelectionRepository selectionRepository,
            IMapper mapper,
            IApplicationRepository applicationRepository,
            IInterviewRepository interviewRepository,
            IScreeningRepository screeningRepository,
            IUnitOfWork unitOfWork,
            IEmployeeRepository employeeRepository,
            IUserRoleRepository userRoleRepository,
            IRoleRepository roleRepository
            )
        {
            _selectionRepository = selectionRepository;
            _mapper = mapper;
            _applicationRepository = applicationRepository;
            _interviewRepository = interviewRepository;
            _screeningRepository = screeningRepository;
            _unitOfWork = unitOfWork;
            _employeeRepository = employeeRepository;
            _userRoleRepository = userRoleRepository;
            _roleRepository = roleRepository;
        }

        public async Task<SelectionResponseDto> MakeDecisionAsync(CreateSelectionDecisionDto createSelectionDecision, CancellationToken cancellationToken)
        {
            ApplicationEntity application = await _applicationRepository
                .GetByIdWithDetailsAsync(createSelectionDecision.ApplicationId, cancellationToken)
                ?? throw new KeyNotFoundException("Application not found.");

            if (application.Status == ApplicationStatus.Rejected)
            {
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

                Selection newSelection = await _selectionRepository.AddAsync(
                    selection,
                    cancellationToken);

                _applicationRepository.Update(application);

                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await _unitOfWork.CommitTransactionAsync(cancellationToken);

                return _mapper.Map<SelectionResponseDto>(newSelection);
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                throw;
            }
        }

        public async Task<SelectionWithDetailsResponseDto?> GetByApplicationIdAsync(Guid applicationId, CancellationToken cancellationToken)
        {
            Selection? selection = await _selectionRepository.GetByApplicationIdAsync(applicationId, cancellationToken);
            if (selection == null)
            {
                throw new KeyNotFoundException("Selection Not Found.");
            }
            return _mapper.Map<SelectionWithDetailsResponseDto>(selection);

        }

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
    }
}
