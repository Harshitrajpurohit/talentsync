using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Application.DTOs.Notifications;
using TalentSync.Application.DTOs.Recruitment;
using TalentSync.Application.Interfaces;
using TalentSync.Application.Interfaces.Repositories;
using TalentSync.Application.Interfaces.Services;
using TalentSync.Application.Services.Recruitment;
using TalentSync.Domain.Entities.HumanResources;
using TalentSync.Domain.Entities.Recruitment;
using TalentSync.Domain.Entities.User;
using TalentSync.Domain.Enums.Recruitment;
using TalentSync.Domain.Enums.User;
using static System.Net.Mime.MediaTypeNames;

namespace TalentSync.Tests.Services.Recruitment
{
    public class SelectionServiceTests
    {

        private readonly Mock<ISelectionRepository> _selectionRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IApplicationRepository> _applicationRepositoryMock;
        private readonly Mock<IInterviewRepository> _interviewRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IEmployeeRepository> _employeeRepositoryMock;
        private readonly Mock<IUserRoleRepository> _userRoleRepositoryMock;
        private readonly Mock<IRoleRepository> _roleRepositoryMock;
        private readonly Mock<INotificationService> _notificationServiceMock;
        private readonly Mock<ILogger<SelectionService>> _loggerMock;

        private readonly SelectionService _selectionService;

        public SelectionServiceTests()
        {
            _selectionRepositoryMock = new Mock<ISelectionRepository>();
            _mapperMock = new Mock<IMapper>();
            _applicationRepositoryMock = new Mock<IApplicationRepository>();
            _interviewRepositoryMock = new Mock<IInterviewRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _employeeRepositoryMock = new Mock<IEmployeeRepository>();
            _userRoleRepositoryMock = new Mock<IUserRoleRepository>();
            _roleRepositoryMock = new Mock<IRoleRepository>();
            _notificationServiceMock = new Mock<INotificationService>();
            _loggerMock = new Mock<ILogger<SelectionService>>();

            _selectionService = new SelectionService(
                _selectionRepositoryMock.Object,
                _mapperMock.Object,
                _applicationRepositoryMock.Object,
                _interviewRepositoryMock.Object,
                _unitOfWorkMock.Object,
                _employeeRepositoryMock.Object,
                _userRoleRepositoryMock.Object,
                _roleRepositoryMock.Object,
                _notificationServiceMock.Object,
                _loggerMock.Object
                );
        }


        [Fact]
        public async Task MakeDecisionAsync_Should_Select_Candidate_Successfully()
        {

            CreateSelectionDecisionDto createSelectionDecision = new CreateSelectionDecisionDto
            {
                ApplicationId = Guid.NewGuid(),
                Decision = SelectionDecision.Selected,
                Notes = "selected",
                Department = "department",
                Position = "position"
            };

            Job job = new Job
            {
                Id = Guid.NewGuid(),
                Title = "Test",
            };

            ApplicationEntity application = new ApplicationEntity
            {
                Id = createSelectionDecision.ApplicationId,
                CandidateId = Guid.NewGuid(),
                JobId = job.Id,
                Job = job,
                Status = ApplicationStatus.InterviewCompleted,

            };

            List<Interview> interviews = new List<Interview>
            {
                 new Interview
                {
                    Application = application,
                    ApplicationId = createSelectionDecision.ApplicationId,
                    Status = InterviewStatus.Passed,
                }
            };

            Selection selection = new Selection
            {
                ApplicationId = createSelectionDecision.ApplicationId,
                Decision = createSelectionDecision.Decision,
                Notes = createSelectionDecision.Notes,
            };


            Role existingRole = new Role
            {
                Id = Guid.NewGuid(),
                Name = RoleName.Candidate,
            };

            UserRole userRole = new UserRole
            {
                UserId = application.CandidateId,
                RoleId = existingRole.Id,
                Role = existingRole
            };

            Role role = new Role
            {
                Id = Guid.NewGuid(),
                Name = RoleName.Employee,
            };

            SelectionResponseDto selectionResponse = new SelectionResponseDto
            {
                ApplicationId = selection.ApplicationId,
                Decision = selection.Decision,
                Notes = selection.Notes,
            };

            _applicationRepositoryMock.Setup(x => x.GetByIdWithDetailsAsync(createSelectionDecision.ApplicationId, It.IsAny<CancellationToken>())).ReturnsAsync(application);
            _selectionRepositoryMock.Setup(x => x.GetByApplicationIdAsync(createSelectionDecision.ApplicationId, It.IsAny<CancellationToken>())).ReturnsAsync((Selection?)null);
            _interviewRepositoryMock.Setup(x => x.GetByApplicationIdAsync(createSelectionDecision.ApplicationId, It.IsAny<CancellationToken>())).ReturnsAsync(interviews);
            _mapperMock.Setup(x => x.Map<Selection>(createSelectionDecision)).Returns(selection);

            _employeeRepositoryMock.Setup(x => x.GetByUserIdAsync(application.CandidateId, It.IsAny<CancellationToken>())).ReturnsAsync((Employee?)null);
            _employeeRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Employee>(), It.IsAny<CancellationToken>()));

            _userRoleRepositoryMock.Setup(x => x.GetByUserIdAsync(application.CandidateId, It.IsAny<CancellationToken>())).ReturnsAsync(userRole);
            _roleRepositoryMock.Setup(x => x.GetRoleByRoleNameAsync(RoleName.Employee, It.IsAny<CancellationToken>())).ReturnsAsync(role);
            _userRoleRepositoryMock.Setup(x => x.Update(userRole));

            _selectionRepositoryMock.Setup(x => x.AddAsync(selection, It.IsAny<CancellationToken>())).ReturnsAsync(selection);
            _applicationRepositoryMock.Setup(x => x.Update(application));
            _notificationServiceMock.Setup(x => x.SendAsync(It.IsAny<CreateNotificationDto>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            _mapperMock.Setup(x => x.Map<SelectionResponseDto>(selection)).Returns(selectionResponse);

            var result = await _selectionService.MakeDecisionAsync(createSelectionDecision, CancellationToken.None);

            Assert.Equal(createSelectionDecision.ApplicationId, result.ApplicationId);
            Assert.Equal(createSelectionDecision.Decision, result.Decision);
            Assert.Equal(ApplicationStatus.Selected, application.Status);


            _applicationRepositoryMock.Verify(x => x.GetByIdWithDetailsAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);

            _interviewRepositoryMock.Verify(x => x.GetByApplicationIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(x => x.Map<Selection>(It.IsAny<CreateSelectionDecisionDto>()), Times.Once);

            _employeeRepositoryMock.Verify(x => x.GetByUserIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            _employeeRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Employee>(), It.IsAny<CancellationToken>()), Times.Once);

            _userRoleRepositoryMock.Verify(x => x.GetByUserIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            _roleRepositoryMock.Verify(x => x.GetRoleByRoleNameAsync(RoleName.Employee, It.IsAny<CancellationToken>()));
            _userRoleRepositoryMock.Verify(x => x.Update(It.IsAny<UserRole>()), Times.Once);

            _selectionRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Selection>(), It.IsAny<CancellationToken>()), Times.Once);
            _applicationRepositoryMock.Verify(x => x.Update(It.IsAny<ApplicationEntity>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.RollbackTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
            _notificationServiceMock.Verify(x => x.SendAsync(It.IsAny<CreateNotificationDto>(), It.IsAny<CancellationToken>()), Times.Once);

        }

        [Fact]
        public async Task MakeDecisionAsync_Should_Reject_Candidate_Successfully()
        {

            CreateSelectionDecisionDto createSelectionDecision = new CreateSelectionDecisionDto
            {
                ApplicationId = Guid.NewGuid(),
                Decision = SelectionDecision.Rejected,
                Notes = "selected",
                Department = "department",
                Position = "position"
            };

            Job job = new Job
            {
                Id = Guid.NewGuid(),
                Title = "Test",
            };

            ApplicationEntity application = new ApplicationEntity
            {
                Id = createSelectionDecision.ApplicationId,
                CandidateId = Guid.NewGuid(),
                JobId = job.Id,
                Job = job,
                Status = ApplicationStatus.InterviewCompleted,

            };

            List<Interview> interviews = new List<Interview>
            {
                 new Interview
                {
                    Application = application,
                    ApplicationId = createSelectionDecision.ApplicationId,
                    Status = InterviewStatus.Passed,
                }
            };

            Selection selection = new Selection
            {
                ApplicationId = createSelectionDecision.ApplicationId,
                Decision = createSelectionDecision.Decision,
                Notes = createSelectionDecision.Notes,
            };


            Role existingRole = new Role
            {
                Id = Guid.NewGuid(),
                Name = RoleName.Candidate,
            };

            UserRole userRole = new UserRole
            {
                UserId = application.CandidateId,
                RoleId = existingRole.Id,
                Role = existingRole
            };

            Role role = new Role
            {
                Id = Guid.NewGuid(),
                Name = RoleName.Employee,
            };

            SelectionResponseDto selectionResponse = new SelectionResponseDto
            {
                ApplicationId = selection.ApplicationId,
                Decision = selection.Decision,
                Notes = selection.Notes,
            };

            _applicationRepositoryMock.Setup(x => x.GetByIdWithDetailsAsync(createSelectionDecision.ApplicationId, It.IsAny<CancellationToken>())).ReturnsAsync(application);
            _selectionRepositoryMock.Setup(x => x.GetByApplicationIdAsync(createSelectionDecision.ApplicationId, It.IsAny<CancellationToken>())).ReturnsAsync((Selection?)null);
            _interviewRepositoryMock.Setup(x => x.GetByApplicationIdAsync(createSelectionDecision.ApplicationId, It.IsAny<CancellationToken>())).ReturnsAsync(interviews);
            _mapperMock.Setup(x => x.Map<Selection>(createSelectionDecision)).Returns(selection);

            _selectionRepositoryMock.Setup(x => x.AddAsync(selection, It.IsAny<CancellationToken>())).ReturnsAsync(selection);
            _applicationRepositoryMock.Setup(x => x.Update(application));
            _notificationServiceMock.Setup(x => x.SendAsync(It.IsAny<CreateNotificationDto>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            _mapperMock.Setup(x => x.Map<SelectionResponseDto>(selection)).Returns(selectionResponse);

            var result = await _selectionService.MakeDecisionAsync(createSelectionDecision, CancellationToken.None);

            Assert.Equal(createSelectionDecision.ApplicationId, result.ApplicationId);
            Assert.Equal(createSelectionDecision.Decision, result.Decision);
            Assert.Equal(ApplicationStatus.Rejected, application.Status);


            _applicationRepositoryMock.Verify(x => x.GetByIdWithDetailsAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);

            _interviewRepositoryMock.Verify(x => x.GetByApplicationIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(x => x.Map<Selection>(It.IsAny<CreateSelectionDecisionDto>()), Times.Once);

            _employeeRepositoryMock.Verify(x => x.GetByUserIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
            _employeeRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Employee>(), It.IsAny<CancellationToken>()), Times.Never);

            _userRoleRepositoryMock.Verify(x => x.GetByUserIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
            _roleRepositoryMock.Verify(x => x.GetRoleByRoleNameAsync(RoleName.Employee, It.IsAny<CancellationToken>()), Times.Never);
            _userRoleRepositoryMock.Verify(x => x.Update(It.IsAny<UserRole>()), Times.Never);

            _selectionRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Selection>(), It.IsAny<CancellationToken>()), Times.Once);
            _applicationRepositoryMock.Verify(x => x.Update(It.IsAny<ApplicationEntity>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.RollbackTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
            _notificationServiceMock.Verify(x => x.SendAsync(It.IsAny<CreateNotificationDto>(), It.IsAny<CancellationToken>()), Times.Once);

        }

        [Fact]
        public async Task MakeDecisionAsync_Should_Throw_When_Selection_Decision_IsPending()
        {
            CreateSelectionDecisionDto createSelectionDecision = new CreateSelectionDecisionDto
            {
                ApplicationId = Guid.NewGuid(),
                Decision = SelectionDecision.Pending,
                Notes = "selected",
                Department = "department",
                Position = "position"
            };

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _selectionService.MakeDecisionAsync(createSelectionDecision, CancellationToken.None));

            Assert.Equal("Selection decision cannot be Pending.", exception.Message);


            _applicationRepositoryMock.Verify(x => x.GetByIdWithDetailsAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);

            _interviewRepositoryMock.Verify(x => x.GetByApplicationIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
            _mapperMock.Verify(x => x.Map<Selection>(It.IsAny<CreateSelectionDecisionDto>()), Times.Never);

            _employeeRepositoryMock.Verify(x => x.GetByUserIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
            _employeeRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Employee>(), It.IsAny<CancellationToken>()), Times.Never);

            _userRoleRepositoryMock.Verify(x => x.GetByUserIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
            _roleRepositoryMock.Verify(x => x.GetRoleByRoleNameAsync(RoleName.Employee, It.IsAny<CancellationToken>()), Times.Never);
            _userRoleRepositoryMock.Verify(x => x.Update(It.IsAny<UserRole>()), Times.Never);

            _selectionRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Selection>(), It.IsAny<CancellationToken>()), Times.Never);
            _applicationRepositoryMock.Verify(x => x.Update(It.IsAny<ApplicationEntity>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.RollbackTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
            _notificationServiceMock.Verify(x => x.SendAsync(It.IsAny<CreateNotificationDto>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task MakeDecisionAsync_Should_Throw_When_Application_NotFound()
        {
            CreateSelectionDecisionDto createSelectionDecision = new CreateSelectionDecisionDto
            {
                ApplicationId = Guid.NewGuid(),
                Decision = SelectionDecision.Selected,
                Notes = "selected",
                Department = "department",
                Position = "position"
            };

            _applicationRepositoryMock.Setup(x => x.GetByIdWithDetailsAsync(createSelectionDecision.ApplicationId, It.IsAny<CancellationToken>())).ReturnsAsync((ApplicationEntity?)null);


            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _selectionService.MakeDecisionAsync(createSelectionDecision, CancellationToken.None));

            Assert.Equal("Application not found.", exception.Message);


            _applicationRepositoryMock.Verify(x => x.GetByIdWithDetailsAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);

            _interviewRepositoryMock.Verify(x => x.GetByApplicationIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
            _mapperMock.Verify(x => x.Map<Selection>(It.IsAny<CreateSelectionDecisionDto>()), Times.Never);

            _employeeRepositoryMock.Verify(x => x.GetByUserIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
            _employeeRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Employee>(), It.IsAny<CancellationToken>()), Times.Never);

            _userRoleRepositoryMock.Verify(x => x.GetByUserIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
            _roleRepositoryMock.Verify(x => x.GetRoleByRoleNameAsync(RoleName.Employee, It.IsAny<CancellationToken>()), Times.Never);
            _userRoleRepositoryMock.Verify(x => x.Update(It.IsAny<UserRole>()), Times.Never);

            _selectionRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Selection>(), It.IsAny<CancellationToken>()), Times.Never);
            _applicationRepositoryMock.Verify(x => x.Update(It.IsAny<ApplicationEntity>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.RollbackTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
            _notificationServiceMock.Verify(x => x.SendAsync(It.IsAny<CreateNotificationDto>(), It.IsAny<CancellationToken>()), Times.Never);

        }



        [Fact]
        public async Task MakeDecisionAsync_Should_Throw_When_Application_Status_IsRejected()
        {
            CreateSelectionDecisionDto createSelectionDecision = new CreateSelectionDecisionDto
            {
                ApplicationId = Guid.NewGuid(),
                Decision = SelectionDecision.Selected,
                Notes = "selected",
                Department = "department",
                Position = "position"
            };

            Job job = new Job
            {
                Id = Guid.NewGuid(),
                Title = "Test",
            };

            ApplicationEntity application = new ApplicationEntity
            {
                Id = createSelectionDecision.ApplicationId,
                CandidateId = Guid.NewGuid(),
                JobId = job.Id,
                Job = job,
                Status = ApplicationStatus.Rejected,

            };


            _applicationRepositoryMock.Setup(x => x.GetByIdWithDetailsAsync(createSelectionDecision.ApplicationId, It.IsAny<CancellationToken>())).ReturnsAsync(application);


            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _selectionService.MakeDecisionAsync(createSelectionDecision, CancellationToken.None));

            Assert.Equal("Application is already rejected and cannot receive a new decision.", exception.Message);


            _applicationRepositoryMock.Verify(x => x.GetByIdWithDetailsAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);

            _interviewRepositoryMock.Verify(x => x.GetByApplicationIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
            _mapperMock.Verify(x => x.Map<Selection>(It.IsAny<CreateSelectionDecisionDto>()), Times.Never);

            _employeeRepositoryMock.Verify(x => x.GetByUserIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
            _employeeRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Employee>(), It.IsAny<CancellationToken>()), Times.Never);

            _userRoleRepositoryMock.Verify(x => x.GetByUserIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
            _roleRepositoryMock.Verify(x => x.GetRoleByRoleNameAsync(RoleName.Employee, It.IsAny<CancellationToken>()), Times.Never);
            _userRoleRepositoryMock.Verify(x => x.Update(It.IsAny<UserRole>()), Times.Never);

            _selectionRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Selection>(), It.IsAny<CancellationToken>()), Times.Never);
            _applicationRepositoryMock.Verify(x => x.Update(It.IsAny<ApplicationEntity>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.RollbackTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
            _notificationServiceMock.Verify(x => x.SendAsync(It.IsAny<CreateNotificationDto>(), It.IsAny<CancellationToken>()), Times.Never);

        }


        [Fact]
        public async Task MakeDecisionAsync_Should_Throw_When_Selection_Already_Exists()
        {
            CreateSelectionDecisionDto createSelectionDecision = new CreateSelectionDecisionDto
            {
                ApplicationId = Guid.NewGuid(),
                Decision = SelectionDecision.Selected,
                Notes = "selected",
                Department = "department",
                Position = "position"
            };

            Job job = new Job
            {
                Id = Guid.NewGuid(),
                Title = "Test",
            };

            ApplicationEntity application = new ApplicationEntity
            {
                Id = createSelectionDecision.ApplicationId,
                CandidateId = Guid.NewGuid(),
                JobId = job.Id,
                Job = job,
                Status = ApplicationStatus.InterviewCompleted,

            };

            Selection selection = new Selection
            {
                ApplicationId = createSelectionDecision.ApplicationId,
                Decision = createSelectionDecision.Decision,
                Notes = createSelectionDecision.Notes,
            };


            _applicationRepositoryMock.Setup(x => x.GetByIdWithDetailsAsync(createSelectionDecision.ApplicationId, It.IsAny<CancellationToken>())).ReturnsAsync(application);
            _selectionRepositoryMock.Setup(x => x.GetByApplicationIdAsync(createSelectionDecision.ApplicationId, It.IsAny<CancellationToken>())).ReturnsAsync(selection);


            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _selectionService.MakeDecisionAsync(createSelectionDecision, CancellationToken.None));

            Assert.Equal("A selection decision already exists for this application.", exception.Message);


            _applicationRepositoryMock.Verify(x => x.GetByIdWithDetailsAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);

            _interviewRepositoryMock.Verify(x => x.GetByApplicationIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
            _mapperMock.Verify(x => x.Map<Selection>(It.IsAny<CreateSelectionDecisionDto>()), Times.Never);

            _employeeRepositoryMock.Verify(x => x.GetByUserIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
            _employeeRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Employee>(), It.IsAny<CancellationToken>()), Times.Never);

            _userRoleRepositoryMock.Verify(x => x.GetByUserIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
            _roleRepositoryMock.Verify(x => x.GetRoleByRoleNameAsync(RoleName.Employee, It.IsAny<CancellationToken>()), Times.Never);
            _userRoleRepositoryMock.Verify(x => x.Update(It.IsAny<UserRole>()), Times.Never);

            _selectionRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Selection>(), It.IsAny<CancellationToken>()), Times.Never);
            _applicationRepositoryMock.Verify(x => x.Update(It.IsAny<ApplicationEntity>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.RollbackTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
            _notificationServiceMock.Verify(x => x.SendAsync(It.IsAny<CreateNotificationDto>(), It.IsAny<CancellationToken>()), Times.Never);

        }


        [Fact]
        public async Task MakeDecisionAsync_Should_Throw_When_Candidate_Has_Not_Passed_Interview()
        {
            CreateSelectionDecisionDto createSelectionDecision = new CreateSelectionDecisionDto
            {
                ApplicationId = Guid.NewGuid(),
                Decision = SelectionDecision.Selected,
                Notes = "selected",
                Department = "department",
                Position = "position"
            };

            Job job = new Job
            {
                Id = Guid.NewGuid(),
                Title = "Test",
            };

            ApplicationEntity application = new ApplicationEntity
            {
                Id = createSelectionDecision.ApplicationId,
                CandidateId = Guid.NewGuid(),
                JobId = job.Id,
                Job = job,
                Status = ApplicationStatus.InterviewCompleted,

            };

            List<Interview> interviews = new List<Interview>
                 {
                      new Interview
                     {
                         Application = application,
                         ApplicationId = createSelectionDecision.ApplicationId,
                         Status = InterviewStatus.Pending,
                     }
                 };

            _applicationRepositoryMock.Setup(x => x.GetByIdWithDetailsAsync(createSelectionDecision.ApplicationId, It.IsAny<CancellationToken>())).ReturnsAsync(application);
            _selectionRepositoryMock.Setup(x => x.GetByApplicationIdAsync(createSelectionDecision.ApplicationId, It.IsAny<CancellationToken>())).ReturnsAsync((Selection?)null);
            _interviewRepositoryMock.Setup(x => x.GetByApplicationIdAsync(createSelectionDecision.ApplicationId, It.IsAny<CancellationToken>())).ReturnsAsync(interviews);


            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _selectionService.MakeDecisionAsync(createSelectionDecision, CancellationToken.None));

            Assert.Equal("Candidate has not passed the interview.", exception.Message);


            _applicationRepositoryMock.Verify(x => x.GetByIdWithDetailsAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);

            _interviewRepositoryMock.Verify(x => x.GetByApplicationIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(x => x.Map<Selection>(It.IsAny<CreateSelectionDecisionDto>()), Times.Never);

            _employeeRepositoryMock.Verify(x => x.GetByUserIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
            _employeeRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Employee>(), It.IsAny<CancellationToken>()), Times.Never);

            _userRoleRepositoryMock.Verify(x => x.GetByUserIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
            _roleRepositoryMock.Verify(x => x.GetRoleByRoleNameAsync(RoleName.Employee, It.IsAny<CancellationToken>()), Times.Never);
            _userRoleRepositoryMock.Verify(x => x.Update(It.IsAny<UserRole>()), Times.Never);

            _selectionRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Selection>(), It.IsAny<CancellationToken>()), Times.Never);
            _applicationRepositoryMock.Verify(x => x.Update(It.IsAny<ApplicationEntity>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.RollbackTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
            _notificationServiceMock.Verify(x => x.SendAsync(It.IsAny<CreateNotificationDto>(), It.IsAny<CancellationToken>()), Times.Never);


        }

        [Fact]
        public async Task MakeDecisionAsync_Should_Throw_When_Application_Status_NotValid()
        {
            CreateSelectionDecisionDto createSelectionDecision = new CreateSelectionDecisionDto
            {
                ApplicationId = Guid.NewGuid(),
                Decision = SelectionDecision.Selected,
                Notes = "selected",
                Department = "department",
                Position = "position"
            };

            Job job = new Job
            {
                Id = Guid.NewGuid(),
                Title = "Test",
            };

            ApplicationEntity application = new ApplicationEntity
            {
                Id = createSelectionDecision.ApplicationId,
                CandidateId = Guid.NewGuid(),
                JobId = job.Id,
                Job = job,
                Status = ApplicationStatus.Screening,

            };

            List<Interview> interviews = new List<Interview>
                 {
                      new Interview
                     {
                         Application = application,
                         ApplicationId = createSelectionDecision.ApplicationId,
                         Status = InterviewStatus.Passed,
                     }
                 };

            _applicationRepositoryMock.Setup(x => x.GetByIdWithDetailsAsync(createSelectionDecision.ApplicationId, It.IsAny<CancellationToken>())).ReturnsAsync(application);
            _selectionRepositoryMock.Setup(x => x.GetByApplicationIdAsync(createSelectionDecision.ApplicationId, It.IsAny<CancellationToken>())).ReturnsAsync((Selection?)null);
            _interviewRepositoryMock.Setup(x => x.GetByApplicationIdAsync(createSelectionDecision.ApplicationId, It.IsAny<CancellationToken>())).ReturnsAsync(interviews);


            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _selectionService.MakeDecisionAsync(createSelectionDecision, CancellationToken.None));

            Assert.Equal($"Cannot change application status from '{application.Status}' to '{ApplicationStatus.Selected}'.", exception.Message);


            _applicationRepositoryMock.Verify(x => x.GetByIdWithDetailsAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);

            _interviewRepositoryMock.Verify(x => x.GetByApplicationIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(x => x.Map<Selection>(It.IsAny<CreateSelectionDecisionDto>()), Times.Never);

            _employeeRepositoryMock.Verify(x => x.GetByUserIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
            _employeeRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Employee>(), It.IsAny<CancellationToken>()), Times.Never);

            _userRoleRepositoryMock.Verify(x => x.GetByUserIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
            _roleRepositoryMock.Verify(x => x.GetRoleByRoleNameAsync(RoleName.Employee, It.IsAny<CancellationToken>()), Times.Never);
            _userRoleRepositoryMock.Verify(x => x.Update(It.IsAny<UserRole>()), Times.Never);

            _selectionRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Selection>(), It.IsAny<CancellationToken>()), Times.Never);
            _applicationRepositoryMock.Verify(x => x.Update(It.IsAny<ApplicationEntity>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.RollbackTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
            _notificationServiceMock.Verify(x => x.SendAsync(It.IsAny<CreateNotificationDto>(), It.IsAny<CancellationToken>()), Times.Never);


        }

        [Fact]
        public async Task MakeDecisionAsync_Should_Throw_When_UserRole_NotFound()
        {

            CreateSelectionDecisionDto createSelectionDecision = new CreateSelectionDecisionDto
            {
                ApplicationId = Guid.NewGuid(),
                Decision = SelectionDecision.Selected,
                Notes = "selected",
                Department = "department",
                Position = "position"
            };

            Job job = new Job
            {
                Id = Guid.NewGuid(),
                Title = "Test",
            };

            ApplicationEntity application = new ApplicationEntity
            {
                Id = createSelectionDecision.ApplicationId,
                CandidateId = Guid.NewGuid(),
                JobId = job.Id,
                Job = job,
                Status = ApplicationStatus.InterviewCompleted,

            };

            List<Interview> interviews = new List<Interview>
                {
                     new Interview
                    {
                        Application = application,
                        ApplicationId = createSelectionDecision.ApplicationId,
                        Status = InterviewStatus.Passed,
                    }
                };

            Selection selection = new Selection
            {
                ApplicationId = createSelectionDecision.ApplicationId,
                Decision = createSelectionDecision.Decision,
                Notes = createSelectionDecision.Notes,
            };

            _applicationRepositoryMock.Setup(x => x.GetByIdWithDetailsAsync(createSelectionDecision.ApplicationId, It.IsAny<CancellationToken>())).ReturnsAsync(application);
            _selectionRepositoryMock.Setup(x => x.GetByApplicationIdAsync(createSelectionDecision.ApplicationId, It.IsAny<CancellationToken>())).ReturnsAsync((Selection?)null);
            _interviewRepositoryMock.Setup(x => x.GetByApplicationIdAsync(createSelectionDecision.ApplicationId, It.IsAny<CancellationToken>())).ReturnsAsync(interviews);
            _mapperMock.Setup(x => x.Map<Selection>(createSelectionDecision)).Returns(selection);

            _employeeRepositoryMock.Setup(x => x.GetByUserIdAsync(application.CandidateId, It.IsAny<CancellationToken>())).ReturnsAsync((Employee?)null);
            _employeeRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Employee>(), It.IsAny<CancellationToken>()));

            _userRoleRepositoryMock.Setup(x => x.GetByUserIdAsync(application.CandidateId, It.IsAny<CancellationToken>())).ReturnsAsync((UserRole?)null);


            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _selectionService.MakeDecisionAsync(createSelectionDecision, CancellationToken.None));

            Assert.Equal("Role Not assigned to this user", exception.Message);


            _applicationRepositoryMock.Verify(x => x.GetByIdWithDetailsAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);

            _interviewRepositoryMock.Verify(x => x.GetByApplicationIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(x => x.Map<Selection>(It.IsAny<CreateSelectionDecisionDto>()), Times.Once);

            _employeeRepositoryMock.Verify(x => x.GetByUserIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            _employeeRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Employee>(), It.IsAny<CancellationToken>()), Times.Once);

            _userRoleRepositoryMock.Verify(x => x.GetByUserIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            _roleRepositoryMock.Verify(x => x.GetRoleByRoleNameAsync(RoleName.Employee, It.IsAny<CancellationToken>()), Times.Never);
            _userRoleRepositoryMock.Verify(x => x.Update(It.IsAny<UserRole>()), Times.Never);

            _selectionRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Selection>(), It.IsAny<CancellationToken>()), Times.Never);
            _applicationRepositoryMock.Verify(x => x.Update(It.IsAny<ApplicationEntity>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.RollbackTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
            _notificationServiceMock.Verify(x => x.SendAsync(It.IsAny<CreateNotificationDto>(), It.IsAny<CancellationToken>()), Times.Never);


        }

        [Fact]
        public async Task MakeDecisionAsync_Should_Throw_When_Role_NotFound()
        {

            CreateSelectionDecisionDto createSelectionDecision = new CreateSelectionDecisionDto
            {
                ApplicationId = Guid.NewGuid(),
                Decision = SelectionDecision.Selected,
                Notes = "selected",
                Department = "department",
                Position = "position"
            };

            Job job = new Job
            {
                Id = Guid.NewGuid(),
                Title = "Test",
            };

            ApplicationEntity application = new ApplicationEntity
            {
                Id = createSelectionDecision.ApplicationId,
                CandidateId = Guid.NewGuid(),
                JobId = job.Id,
                Job = job,
                Status = ApplicationStatus.InterviewCompleted,

            };

            List<Interview> interviews = new List<Interview>
                {
                     new Interview
                    {
                        Application = application,
                        ApplicationId = createSelectionDecision.ApplicationId,
                        Status = InterviewStatus.Passed,
                    }
                };

            Selection selection = new Selection
            {
                ApplicationId = createSelectionDecision.ApplicationId,
                Decision = createSelectionDecision.Decision,
                Notes = createSelectionDecision.Notes,
            };


            Role existingRole = new Role
            {
                Id = Guid.NewGuid(),
                Name = RoleName.Candidate,
            };

            UserRole userRole = new UserRole
            {
                UserId = application.CandidateId,
                RoleId = existingRole.Id,
                Role = existingRole
            };

            Role role = new Role
            {
                Id = Guid.NewGuid(),
                Name = RoleName.Employee,
            };

            _applicationRepositoryMock.Setup(x => x.GetByIdWithDetailsAsync(createSelectionDecision.ApplicationId, It.IsAny<CancellationToken>())).ReturnsAsync(application);
            _selectionRepositoryMock.Setup(x => x.GetByApplicationIdAsync(createSelectionDecision.ApplicationId, It.IsAny<CancellationToken>())).ReturnsAsync((Selection?)null);
            _interviewRepositoryMock.Setup(x => x.GetByApplicationIdAsync(createSelectionDecision.ApplicationId, It.IsAny<CancellationToken>())).ReturnsAsync(interviews);
            _mapperMock.Setup(x => x.Map<Selection>(createSelectionDecision)).Returns(selection);

            _employeeRepositoryMock.Setup(x => x.GetByUserIdAsync(application.CandidateId, It.IsAny<CancellationToken>())).ReturnsAsync((Employee?)null);
            _employeeRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Employee>(), It.IsAny<CancellationToken>()));

            _userRoleRepositoryMock.Setup(x => x.GetByUserIdAsync(application.CandidateId, It.IsAny<CancellationToken>())).ReturnsAsync(userRole);
            _roleRepositoryMock.Setup(x => x.GetRoleByRoleNameAsync(RoleName.Employee, It.IsAny<CancellationToken>())).ReturnsAsync((Role?)null);


            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _selectionService.MakeDecisionAsync(createSelectionDecision, CancellationToken.None));

            Assert.Equal("Role Not Found.", exception.Message);


            _applicationRepositoryMock.Verify(x => x.GetByIdWithDetailsAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);

            _interviewRepositoryMock.Verify(x => x.GetByApplicationIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(x => x.Map<Selection>(It.IsAny<CreateSelectionDecisionDto>()), Times.Once);

            _employeeRepositoryMock.Verify(x => x.GetByUserIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            _employeeRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Employee>(), It.IsAny<CancellationToken>()), Times.Once);

            _userRoleRepositoryMock.Verify(x => x.GetByUserIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            _roleRepositoryMock.Verify(x => x.GetRoleByRoleNameAsync(RoleName.Employee, It.IsAny<CancellationToken>()), Times.Once);
            _userRoleRepositoryMock.Verify(x => x.Update(It.IsAny<UserRole>()), Times.Never);

            _selectionRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Selection>(), It.IsAny<CancellationToken>()), Times.Never);
            _applicationRepositoryMock.Verify(x => x.Update(It.IsAny<ApplicationEntity>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.RollbackTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
            _notificationServiceMock.Verify(x => x.SendAsync(It.IsAny<CreateNotificationDto>(), It.IsAny<CancellationToken>()), Times.Never);


        }

        [Fact]
        public async Task GetByApplicationIdAsync_Should_Get_Selection_Successfully()
        {
            Guid applicationId = Guid.NewGuid();

            Selection selection = new Selection
            {
                ApplicationId = applicationId,
                Decision = SelectionDecision.Selected,
                Notes = "selected",
            };

            SelectionWithDetailsResponseDto selectionResponse = new SelectionWithDetailsResponseDto
            {
                ApplicationId = selection.ApplicationId,
                Decision = selection.Decision,
                Notes = selection.Notes,
            };

            _selectionRepositoryMock.Setup(x => x.GetByApplicationIdAsync(applicationId, It.IsAny<CancellationToken>())).ReturnsAsync(selection);
            _mapperMock.Setup(x => x.Map<SelectionWithDetailsResponseDto>(selection)).Returns(selectionResponse);

            var result = await _selectionService.GetByApplicationIdAsync(applicationId, CancellationToken.None);

            Assert.Equal(applicationId, result.ApplicationId);

            _selectionRepositoryMock.Verify(x => x.GetByApplicationIdAsync(applicationId, It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(x => x.Map<SelectionWithDetailsResponseDto>(selection), Times.Once);

        }

        [Fact]
        public async Task GetByApplicationIdAsync_Should_Throw_When_Selection_NotFound()
        {
            Guid applicationId = Guid.NewGuid();

            _selectionRepositoryMock.Setup(x => x.GetByApplicationIdAsync(applicationId, It.IsAny<CancellationToken>())).ReturnsAsync((Selection?)null);

            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _selectionService.GetByApplicationIdAsync(applicationId, CancellationToken.None));

            Assert.Equal("Selection not found for the given applicationId.", exception.Message);

            _selectionRepositoryMock.Verify(x => x.GetByApplicationIdAsync(applicationId, It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(x => x.Map<SelectionWithDetailsResponseDto>(It.IsAny<Selection>()), Times.Never);

        }

    }
}
