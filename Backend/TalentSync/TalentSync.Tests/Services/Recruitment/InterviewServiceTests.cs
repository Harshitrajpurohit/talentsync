using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;
using TalentSync.Application.DTOs.Notifications;
using TalentSync.Application.DTOs.Recruitment;
using TalentSync.Application.Interfaces;
using TalentSync.Application.Interfaces.Repositories;
using TalentSync.Application.Interfaces.Services;
using TalentSync.Application.Services.Recruitment;
using TalentSync.Domain.Entities.Recruitment;
using TalentSync.Domain.Entities.User;
using TalentSync.Domain.Enums.Notifications;
using TalentSync.Domain.Enums.Recruitment;
using TalentSync.Domain.Enums.User;
using TalentSync.Infrastructure.Repositories.Recruitment;
using static System.Net.Mime.MediaTypeNames;

namespace TalentSync.Tests.Services.Recruitment
{
    public class InterviewServiceTests
    {
        private readonly Mock<IInterviewRepository> _interviewRepositoryMock;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IApplicationRepository> _applicationRepositoryMock;
        private readonly Mock<IScreeningRepository> _screeningRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IUserRoleRepository> _userRoleRepositoryMock;
        private readonly Mock<INotificationService> _notificationServiceMock;
        private readonly Mock<ILogger<InterviewService>> _loggerMock;

        private readonly InterviewService _interviewService;

        public InterviewServiceTests()
        {
            _interviewRepositoryMock = new Mock<IInterviewRepository>();
            _mapper = new Mock<IMapper>();
            _applicationRepositoryMock = new Mock<IApplicationRepository>();
            _screeningRepositoryMock = new Mock<IScreeningRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _userRoleRepositoryMock = new Mock<IUserRoleRepository>();
            _notificationServiceMock = new Mock<INotificationService>();
            _loggerMock = new Mock<ILogger<InterviewService>>();

            _interviewService = new InterviewService(
                _interviewRepositoryMock.Object,
                _mapper.Object,
                _applicationRepositoryMock.Object,
                _screeningRepositoryMock.Object,
                _unitOfWorkMock.Object,
                _userRepositoryMock.Object,
                _userRoleRepositoryMock.Object,
                _notificationServiceMock.Object,
                _loggerMock.Object
                );

        }


        [Fact]
        public async Task ScheduleInterviewAsync_Should_Schedule_Interview_Successfully()
        {
            ScheduleInterviewDto interviewDto = new ScheduleInterviewDto
            {
                ApplicationId = Guid.NewGuid(),
                ScheduledAt = DateTime.UtcNow.AddHours(5),
                InterviewerId = Guid.NewGuid(),
                Location = "location"
            };
            Job job = new Job
            {
                Id = Guid.NewGuid(),
                Title = "Software Engineer"
            };

            ApplicationEntity application = new ApplicationEntity
            {

                Id = interviewDto.ApplicationId,
                CandidateId = Guid.NewGuid(),
                JobId = job.Id,
                Job = job,
                Status = ApplicationStatus.Screening
            };

            List<Interview> scheduledInterviews = new List<Interview>
            {

            };
            
            User interviewer = new User
            {
                Id = interviewDto.InterviewerId,
                Name = "name",
                Status = UserStatus.Active
            };
            Role role = new Role
            {
                Id = Guid.NewGuid(),
                Name = RoleName.HR
            };
            UserRole interviewerRole = new UserRole
            {
                UserId = interviewer.Id,
                RoleId = role.Id,
                Role = role

            };

            Interview interview = new Interview
            {
                ApplicationId = interviewDto.ApplicationId,
                ScheduledAt = interviewDto.ScheduledAt,
                InterviewerId = interviewDto.InterviewerId,
                Location = interviewDto.Location,
                Status = InterviewStatus.Scheduled
            };

            InterviewResponseDto interviewResponse = new InterviewResponseDto
            {
                Id = interview.Id,
                ApplicationId = interview.ApplicationId,
                InterviewerId = interview.InterviewerId,
                Status = interview.Status,
            };

            _applicationRepositoryMock.Setup(x => x.GetByIdAsync(interviewDto.ApplicationId, It.IsAny<CancellationToken>())).ReturnsAsync(application);

            _screeningRepositoryMock.Setup(x => x.HasPassedScreeningAsync(interviewDto.ApplicationId, It.IsAny<CancellationToken>())).ReturnsAsync(true);

            _interviewRepositoryMock.Setup(x => x.GetByApplicationIdAsync(interviewDto.ApplicationId, It.IsAny<CancellationToken>())).ReturnsAsync(scheduledInterviews);

            _userRepositoryMock.Setup(x => x.GetUserByIdAsync(interviewDto.InterviewerId, It.IsAny<CancellationToken>())).ReturnsAsync(interviewer);

            _userRoleRepositoryMock.Setup(x => x.GetByUserIdWithRoleAsync(interviewDto.InterviewerId, It.IsAny<CancellationToken>())).ReturnsAsync(interviewerRole);

            _unitOfWorkMock.Setup(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            _mapper.Setup(x => x.Map<Interview>(interviewDto)).Returns(interview);

            _interviewRepositoryMock.Setup(x => x.AddAsync(interview, It.IsAny<CancellationToken>()));
            _applicationRepositoryMock.Setup(x => x.Update(application));

            _unitOfWorkMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()));
            _unitOfWorkMock.Setup(x => x.CommitTransactionAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            _notificationServiceMock.Setup(x => x.SendAsync(It.IsAny<CreateNotificationDto>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            _mapper.Setup(x => x.Map<InterviewResponseDto>(interview)).Returns(interviewResponse);

            var result = await _interviewService.ScheduleInterviewAsync(interviewDto, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(interviewDto.ApplicationId, result.ApplicationId);
            Assert.Equal(Domain.Enums.Recruitment.InterviewStatus.Scheduled, result.Status);
            Assert.Equal(ApplicationStatus.InterviewScheduled, application.Status);

            _applicationRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);

            _interviewRepositoryMock.Verify(x => x.GetByApplicationIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);

            _unitOfWorkMock.Verify(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
            _notificationServiceMock.Verify(x => x.SendAsync(It.IsAny<CreateNotificationDto>(), It.IsAny<CancellationToken>()), Times.Exactly(2));
            _unitOfWorkMock.Verify(x => x.RollbackTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);

        }

        [Fact]
        public async Task ScheduleInterviewAsync_Should_Throw_When_ScheduledAt_NotValid()
        {
            ScheduleInterviewDto interviewDto = new ScheduleInterviewDto
            {
                ApplicationId = Guid.NewGuid(),
                ScheduledAt = DateTime.UtcNow,
                InterviewerId = Guid.NewGuid(),
                Location = "location"
            };

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _interviewService.ScheduleInterviewAsync(interviewDto, CancellationToken.None));

            Assert.Equal("Interview must be scheduled in the future.", exception.Message);

            _applicationRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);

            _interviewRepositoryMock.Verify(x => x.GetByApplicationIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);

            _unitOfWorkMock.Verify(x => x.RollbackTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task ScheduleInterviewAsync_Should_Throw_When_Application_NotFound()
        {
            ScheduleInterviewDto interviewDto = new ScheduleInterviewDto
            {
                ApplicationId = Guid.NewGuid(),
                ScheduledAt = DateTime.UtcNow.AddHours(5),
                InterviewerId = Guid.NewGuid(),
                Location = "location"
            };

            _applicationRepositoryMock.Setup(x => x.GetByIdAsync(interviewDto.ApplicationId, It.IsAny<CancellationToken>())).ReturnsAsync((ApplicationEntity?)null);

            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _interviewService.ScheduleInterviewAsync(interviewDto, CancellationToken.None));

            Assert.Equal("Application not found.", exception.Message);


            _applicationRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);

            _interviewRepositoryMock.Verify(x => x.GetByApplicationIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);

            _unitOfWorkMock.Verify(x => x.RollbackTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);

        }

        [Fact]
        public async Task ScheduleInterviewAsync_Should_Throw_When_Application_Status_NotValid()
        {
            ScheduleInterviewDto interviewDto = new ScheduleInterviewDto
            {
                ApplicationId = Guid.NewGuid(),
                ScheduledAt = DateTime.UtcNow.AddHours(5),
                InterviewerId = Guid.NewGuid(),
                Location = "location"
            };

            Job job = new Job
            {
                Id = Guid.NewGuid(),
                Title = "Software Engineer"
            };

            ApplicationEntity application = new ApplicationEntity
            {

                Id = interviewDto.ApplicationId,
                CandidateId = Guid.NewGuid(),
                JobId = job.Id,
                Job = job,
                Status = Domain.Enums.Recruitment.ApplicationStatus.Submitted
            };

            _applicationRepositoryMock.Setup(x => x.GetByIdAsync(interviewDto.ApplicationId, It.IsAny<CancellationToken>())).ReturnsAsync(application);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _interviewService.ScheduleInterviewAsync(interviewDto, CancellationToken.None));

            Assert.Equal($"Cannot schedule interview from '{application.Status}'.", exception.Message);


            _applicationRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);

            _interviewRepositoryMock.Verify(x => x.GetByApplicationIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);

            _unitOfWorkMock.Verify(x => x.RollbackTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);

        }

        [Fact]
        public async Task ScheduleInterviewAsync_Should_Throw_When_Screening_Not_Passed()
        {
            ScheduleInterviewDto interviewDto = new ScheduleInterviewDto
            {
                ApplicationId = Guid.NewGuid(),
                ScheduledAt = DateTime.UtcNow.AddHours(5),
                InterviewerId = Guid.NewGuid(),
                Location = "location"
            };
            Job job = new Job
            {
                Id = Guid.NewGuid(),
                Title = "Software Engineer"
            };
            ApplicationEntity application = new ApplicationEntity
            {
                Id = interviewDto.ApplicationId,
                CandidateId = Guid.NewGuid(),
                JobId = job.Id,
                Job = job,
                Status = ApplicationStatus.Screening
            };

            _applicationRepositoryMock.Setup(x => x.GetByIdAsync(interviewDto.ApplicationId, It.IsAny<CancellationToken>())).ReturnsAsync(application);
            _screeningRepositoryMock.Setup(x => x.HasPassedScreeningAsync(interviewDto.ApplicationId, It.IsAny<CancellationToken>())).ReturnsAsync(false);
            
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _interviewService.ScheduleInterviewAsync(interviewDto, CancellationToken.None));
            Assert.Equal("Cannot schedule an interview: this application has not passed screening yet. " +
                    "Please complete screening with a Pass result first.", exception.Message);


            _applicationRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            _screeningRepositoryMock.Verify(x => x.HasPassedScreeningAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);

            _interviewRepositoryMock.Verify(x => x.GetByApplicationIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);

            _unitOfWorkMock.Verify(x => x.RollbackTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);

        }

        [Fact]
        public async Task ScheduleInterviewAsync_Should_Throw_When_Interview_Already_Scheduled()
        {
            ScheduleInterviewDto interviewDto = new ScheduleInterviewDto
            {
                ApplicationId = Guid.NewGuid(),
                ScheduledAt = DateTime.UtcNow.AddHours(5),
                InterviewerId = Guid.NewGuid(),
                Location = "location"
            };
            Job job = new Job
            {
                Id = Guid.NewGuid(),
                Title = "Software Engineer"
            };

            ApplicationEntity application = new ApplicationEntity
            {

                Id = interviewDto.ApplicationId,
                CandidateId = Guid.NewGuid(),
                JobId = job.Id,
                Job = job,
                Status = ApplicationStatus.Screening
            };

            List<Interview> scheduledInterviews = new List<Interview>
            {
                new Interview
                {
                    ApplicationId = interviewDto.ApplicationId,
                    ScheduledAt = DateTime.UtcNow.AddHours(1),
                    InterviewerId = Guid.NewGuid(),
                    Location = "location",
                    Status = InterviewStatus.Scheduled
                }
            };

            _applicationRepositoryMock.Setup(x => x.GetByIdAsync(interviewDto.ApplicationId, It.IsAny<CancellationToken>())).ReturnsAsync(application);

            _screeningRepositoryMock.Setup(x => x.HasPassedScreeningAsync(interviewDto.ApplicationId, It.IsAny<CancellationToken>())).ReturnsAsync(true);

            _interviewRepositoryMock.Setup(x => x.GetByApplicationIdAsync(interviewDto.ApplicationId, It.IsAny<CancellationToken>())).ReturnsAsync(scheduledInterviews);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _interviewService.ScheduleInterviewAsync(interviewDto, CancellationToken.None));

            Assert.Equal("Cannot schedule another interview — an active interview already exists for this application. Cancel it first.", exception.Message);


            _applicationRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            _screeningRepositoryMock.Verify(x => x.HasPassedScreeningAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);

            _interviewRepositoryMock.Verify(x => x.GetByApplicationIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);

            _interviewRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Interview>(), It.IsAny<CancellationToken>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.RollbackTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);

        }

        [Fact]
        public async Task ScheduleInterviewAsync_Should_Throw_When_Interview_Already_Passed()
        {
            ScheduleInterviewDto interviewDto = new ScheduleInterviewDto
            {
                ApplicationId = Guid.NewGuid(),
                ScheduledAt = DateTime.UtcNow.AddHours(5),
                InterviewerId = Guid.NewGuid(),
                Location = "location"
            };
            Job job = new Job
            {
                Id = Guid.NewGuid(),
                Title = "Software Engineer"
            };

            ApplicationEntity application = new ApplicationEntity
            {

                Id = interviewDto.ApplicationId,
                CandidateId = Guid.NewGuid(),
                JobId = job.Id,
                Job = job,
                Status = ApplicationStatus.Screening
            };

            List<Interview> scheduledInterviews = new List<Interview>
            {
                new Interview
                {
                    ApplicationId = interviewDto.ApplicationId,
                    ScheduledAt = DateTime.UtcNow.AddHours(1),
                    InterviewerId = Guid.NewGuid(),
                    Location = "location",
                    Status = InterviewStatus.Passed
                }
            };

            _applicationRepositoryMock.Setup(x => x.GetByIdAsync(interviewDto.ApplicationId, It.IsAny<CancellationToken>())).ReturnsAsync(application);

            _screeningRepositoryMock.Setup(x => x.HasPassedScreeningAsync(interviewDto.ApplicationId, It.IsAny<CancellationToken>())).ReturnsAsync(true);

            _interviewRepositoryMock.Setup(x => x.GetByApplicationIdAsync(interviewDto.ApplicationId, It.IsAny<CancellationToken>())).ReturnsAsync(scheduledInterviews);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _interviewService.ScheduleInterviewAsync(interviewDto, CancellationToken.None));

            Assert.Equal("Cannot schedule another interview — this candidate has already passed. Proceed to final selection.", exception.Message);


            _applicationRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            _screeningRepositoryMock.Verify(x => x.HasPassedScreeningAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);

            _interviewRepositoryMock.Verify(x => x.GetByApplicationIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);

            _interviewRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Interview>(), It.IsAny<CancellationToken>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.RollbackTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);

        }

        [Fact]
        public async Task ScheduleInterviewAsync_Should_Throw_When_Interview_Already_Completed()
        {
            ScheduleInterviewDto interviewDto = new ScheduleInterviewDto
            {
                ApplicationId = Guid.NewGuid(),
                ScheduledAt = DateTime.UtcNow.AddHours(5),
                InterviewerId = Guid.NewGuid(),
                Location = "location"
            };
            Job job = new Job
            {
                Id = Guid.NewGuid(),
                Title = "Software Engineer"
            };

            ApplicationEntity application = new ApplicationEntity
            {

                Id = interviewDto.ApplicationId,
                CandidateId = Guid.NewGuid(),
                JobId = job.Id,
                Job = job,
                Status = ApplicationStatus.Screening
            };

            List<Interview> scheduledInterviews = new List<Interview>
            {
                new Interview
                {
                    ApplicationId = interviewDto.ApplicationId,
                    ScheduledAt = DateTime.UtcNow.AddHours(1),
                    InterviewerId = Guid.NewGuid(),
                    Location = "location",
                    Status = InterviewStatus.Completed
                }
            };

            _applicationRepositoryMock.Setup(x => x.GetByIdAsync(interviewDto.ApplicationId, It.IsAny<CancellationToken>())).ReturnsAsync(application);

            _screeningRepositoryMock.Setup(x => x.HasPassedScreeningAsync(interviewDto.ApplicationId, It.IsAny<CancellationToken>())).ReturnsAsync(true);

            _interviewRepositoryMock.Setup(x => x.GetByApplicationIdAsync(interviewDto.ApplicationId, It.IsAny<CancellationToken>())).ReturnsAsync(scheduledInterviews);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _interviewService.ScheduleInterviewAsync(interviewDto, CancellationToken.None));

            Assert.Equal("Cannot schedule another interview — this candidate has already Completed Interview. First give the result than try again.", exception.Message);


            _applicationRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            _screeningRepositoryMock.Verify(x => x.HasPassedScreeningAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);

            _interviewRepositoryMock.Verify(x => x.GetByApplicationIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);

            _interviewRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Interview>(), It.IsAny<CancellationToken>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.RollbackTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);

        }

        [Fact]
        public async Task ScheduleInterviewAsync_Should_Throw_When_Interviewer_NotFound()
        {
            ScheduleInterviewDto interviewDto = new ScheduleInterviewDto
            {
                ApplicationId = Guid.NewGuid(),
                ScheduledAt = DateTime.UtcNow.AddHours(5),
                InterviewerId = Guid.NewGuid(),
                Location = "location"
            };
            Job job = new Job
            {
                Id = Guid.NewGuid(),
                Title = "Software Engineer"
            };

            ApplicationEntity application = new ApplicationEntity
            {

                Id = interviewDto.ApplicationId,
                CandidateId = Guid.NewGuid(),
                JobId = job.Id,
                Job = job,
                Status = ApplicationStatus.Screening
            };

            List<Interview> scheduledInterviews = new List<Interview>
            {

            };

            _applicationRepositoryMock.Setup(x => x.GetByIdAsync(interviewDto.ApplicationId, It.IsAny<CancellationToken>())).ReturnsAsync(application);

            _screeningRepositoryMock.Setup(x => x.HasPassedScreeningAsync(interviewDto.ApplicationId, It.IsAny<CancellationToken>())).ReturnsAsync(true);

            _interviewRepositoryMock.Setup(x => x.GetByApplicationIdAsync(interviewDto.ApplicationId, It.IsAny<CancellationToken>())).ReturnsAsync(scheduledInterviews);

            _userRepositoryMock.Setup(x => x.GetUserByIdAsync(interviewDto.InterviewerId, It.IsAny<CancellationToken>())).ReturnsAsync((User?)null);


            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _interviewService.ScheduleInterviewAsync(interviewDto, CancellationToken.None));

            Assert.Equal("Interviewer not found.", exception.Message);

            _applicationRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            _screeningRepositoryMock.Verify(x => x.HasPassedScreeningAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);

            _interviewRepositoryMock.Verify(x => x.GetByApplicationIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);

            _interviewRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Interview>(), It.IsAny<CancellationToken>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.RollbackTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task ScheduleInterviewAsync_Should_Throw_When_Interviewer_InActive()
        {
            ScheduleInterviewDto interviewDto = new ScheduleInterviewDto
            {
                ApplicationId = Guid.NewGuid(),
                ScheduledAt = DateTime.UtcNow.AddHours(5),
                InterviewerId = Guid.NewGuid(),
                Location = "location"
            };
            Job job = new Job
            {
                Id = Guid.NewGuid(),
                Title = "Software Engineer"
            };

            ApplicationEntity application = new ApplicationEntity
            {

                Id = interviewDto.ApplicationId,
                CandidateId = Guid.NewGuid(),
                JobId = job.Id,
                Job = job,
                Status = ApplicationStatus.Screening
            };

            List<Interview> scheduledInterviews = new List<Interview>
            {

            };

            User interviewer = new User
            {
                Id = interviewDto.InterviewerId,
                Name = "name",
                Status = UserStatus.Inactive
            };

            _applicationRepositoryMock.Setup(x => x.GetByIdAsync(interviewDto.ApplicationId, It.IsAny<CancellationToken>())).ReturnsAsync(application);

            _screeningRepositoryMock.Setup(x => x.HasPassedScreeningAsync(interviewDto.ApplicationId, It.IsAny<CancellationToken>())).ReturnsAsync(true);

            _interviewRepositoryMock.Setup(x => x.GetByApplicationIdAsync(interviewDto.ApplicationId, It.IsAny<CancellationToken>())).ReturnsAsync(scheduledInterviews);

            _userRepositoryMock.Setup(x => x.GetUserByIdAsync(interviewDto.InterviewerId, It.IsAny<CancellationToken>())).ReturnsAsync(interviewer);


            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _interviewService.ScheduleInterviewAsync(interviewDto, CancellationToken.None));

            Assert.Equal("Interviewer account is not active.", exception.Message);

            _applicationRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            _screeningRepositoryMock.Verify(x => x.HasPassedScreeningAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);

            _interviewRepositoryMock.Verify(x => x.GetByApplicationIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);

            _interviewRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Interview>(), It.IsAny<CancellationToken>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.RollbackTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
        }


        [Fact]
        public async Task ScheduleInterviewAsync_Should_Throw_When_UserRole_NotFound()
        {
            ScheduleInterviewDto interviewDto = new ScheduleInterviewDto
            {
                ApplicationId = Guid.NewGuid(),
                ScheduledAt = DateTime.UtcNow.AddHours(5),
                InterviewerId = Guid.NewGuid(),
                Location = "location"
            };
            Job job = new Job
            {
                Id = Guid.NewGuid(),
                Title = "Software Engineer"
            };

            ApplicationEntity application = new ApplicationEntity
            {

                Id = interviewDto.ApplicationId,
                CandidateId = Guid.NewGuid(),
                JobId = job.Id,
                Job = job,
                Status = ApplicationStatus.Screening
            };

            List<Interview> scheduledInterviews = new List<Interview>
            {

            };

            User interviewer = new User
            {
                Id = interviewDto.InterviewerId,
                Name = "name",
                Status = UserStatus.Active
            };

            _applicationRepositoryMock.Setup(x => x.GetByIdAsync(interviewDto.ApplicationId, It.IsAny<CancellationToken>())).ReturnsAsync(application);

            _screeningRepositoryMock.Setup(x => x.HasPassedScreeningAsync(interviewDto.ApplicationId, It.IsAny<CancellationToken>())).ReturnsAsync(true);

            _interviewRepositoryMock.Setup(x => x.GetByApplicationIdAsync(interviewDto.ApplicationId, It.IsAny<CancellationToken>())).ReturnsAsync(scheduledInterviews);

            _userRepositoryMock.Setup(x => x.GetUserByIdAsync(interviewDto.InterviewerId, It.IsAny<CancellationToken>())).ReturnsAsync(interviewer);

            _userRoleRepositoryMock.Setup(x => x.GetByUserIdWithRoleAsync(interviewDto.InterviewerId, It.IsAny<CancellationToken>())).ReturnsAsync((UserRole?)null);

            
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _interviewService.ScheduleInterviewAsync(interviewDto, CancellationToken.None));

            Assert.Equal("Role Not Assigned.", exception.Message);

            _applicationRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            _screeningRepositoryMock.Verify(x => x.HasPassedScreeningAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);

            _interviewRepositoryMock.Verify(x => x.GetByApplicationIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);

            _interviewRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Interview>(), It.IsAny<CancellationToken>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.RollbackTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);

        }

        [Fact]
        public async Task ScheduleInterviewAsync_Should_Throw_When_Role_NotMatch()
        {
            ScheduleInterviewDto interviewDto = new ScheduleInterviewDto
            {
                ApplicationId = Guid.NewGuid(),
                ScheduledAt = DateTime.UtcNow.AddHours(5),
                InterviewerId = Guid.NewGuid(),
                Location = "location"
            };
            Job job = new Job
            {
                Id = Guid.NewGuid(),
                Title = "Software Engineer"
            };

            ApplicationEntity application = new ApplicationEntity
            {

                Id = interviewDto.ApplicationId,
                CandidateId = Guid.NewGuid(),
                JobId = job.Id,
                Job = job,
                Status = ApplicationStatus.Screening
            };

            List<Interview> scheduledInterviews = new List<Interview>
            {

            };

            User interviewer = new User
            {
                Id = interviewDto.InterviewerId,
                Name = "name",
                Status = UserStatus.Active
            };
            Role role = new Role
            {
                Id = Guid.NewGuid(),
                Name = RoleName.Candidate
            };
            UserRole interviewerRole = new UserRole
            {
                UserId = interviewer.Id,
                RoleId = role.Id,
                Role = role
            };


            _applicationRepositoryMock.Setup(x => x.GetByIdAsync(interviewDto.ApplicationId, It.IsAny<CancellationToken>())).ReturnsAsync(application);

            _screeningRepositoryMock.Setup(x => x.HasPassedScreeningAsync(interviewDto.ApplicationId, It.IsAny<CancellationToken>())).ReturnsAsync(true);

            _interviewRepositoryMock.Setup(x => x.GetByApplicationIdAsync(interviewDto.ApplicationId, It.IsAny<CancellationToken>())).ReturnsAsync(scheduledInterviews);

            _userRepositoryMock.Setup(x => x.GetUserByIdAsync(interviewDto.InterviewerId, It.IsAny<CancellationToken>())).ReturnsAsync(interviewer);

            _userRoleRepositoryMock.Setup(x => x.GetByUserIdWithRoleAsync(interviewDto.InterviewerId, It.IsAny<CancellationToken>())).ReturnsAsync(interviewerRole);


            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _interviewService.ScheduleInterviewAsync(interviewDto, CancellationToken.None));

            Assert.Equal("Selected user cannot conduct interviews.", exception.Message);


            _applicationRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            _screeningRepositoryMock.Verify(x => x.HasPassedScreeningAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);

            _interviewRepositoryMock.Verify(x => x.GetByApplicationIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);

            _interviewRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Interview>(), It.IsAny<CancellationToken>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.RollbackTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);


        }

        [Fact]
        public async Task ScheduleInterviewAsync_Should_Throw_When_Application_Updated_Failed()
        {

            ScheduleInterviewDto interviewDto = new ScheduleInterviewDto
            {
                ApplicationId = Guid.NewGuid(),
                ScheduledAt = DateTime.UtcNow.AddHours(5),
                InterviewerId = Guid.NewGuid(),
                Location = "location"
            };
            Job job = new Job
            {
                Id = Guid.NewGuid(),
                Title = "Software Engineer"
            };

            ApplicationEntity application = new ApplicationEntity
            {

                Id = interviewDto.ApplicationId,
                CandidateId = Guid.NewGuid(),
                JobId = job.Id,
                Job = job,
                Status = ApplicationStatus.Screening
            };

            List<Interview> scheduledInterviews = new List<Interview>
            {

            };

            User interviewer = new User
            {
                Id = interviewDto.InterviewerId,
                Name = "name",
                Status = UserStatus.Active
            };
            Role role = new Role
            {
                Id = Guid.NewGuid(),
                Name = RoleName.HR
            };
            UserRole interviewerRole = new UserRole
            {
                UserId = interviewer.Id,
                RoleId = role.Id,
                Role = role

            };

            Interview interview = new Interview
            {
                ApplicationId = interviewDto.ApplicationId,
                ScheduledAt = interviewDto.ScheduledAt,
                InterviewerId = interviewDto.InterviewerId,
                Location = interviewDto.Location,
                Status = InterviewStatus.Scheduled
            };


            _applicationRepositoryMock.Setup(x => x.GetByIdAsync(interviewDto.ApplicationId, It.IsAny<CancellationToken>())).ReturnsAsync(application);

            _screeningRepositoryMock.Setup(x => x.HasPassedScreeningAsync(interviewDto.ApplicationId, It.IsAny<CancellationToken>())).ReturnsAsync(true);

            _interviewRepositoryMock.Setup(x => x.GetByApplicationIdAsync(interviewDto.ApplicationId, It.IsAny<CancellationToken>())).ReturnsAsync(scheduledInterviews);

            _userRepositoryMock.Setup(x => x.GetUserByIdAsync(interviewDto.InterviewerId, It.IsAny<CancellationToken>())).ReturnsAsync(interviewer);

            _userRoleRepositoryMock.Setup(x => x.GetByUserIdWithRoleAsync(interviewDto.InterviewerId, It.IsAny<CancellationToken>())).ReturnsAsync(interviewerRole);

            _unitOfWorkMock.Setup(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            _mapper.Setup(x => x.Map<Interview>(interviewDto)).Returns(interview);

            _interviewRepositoryMock.Setup(x => x.AddAsync(interview, It.IsAny<CancellationToken>()));
            _applicationRepositoryMock.Setup(x => x.Update(application)).Throws<InvalidOperationException>();
            _unitOfWorkMock.Setup(x => x.RollbackTransactionAsync(It.IsAny<CancellationToken>()));


            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _interviewService.ScheduleInterviewAsync(interviewDto, CancellationToken.None));


            _applicationRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            _screeningRepositoryMock.Verify(x => x.HasPassedScreeningAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);

            _interviewRepositoryMock.Verify(x => x.GetByApplicationIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);

            _interviewRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Interview>(), It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.CommitTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.RollbackTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);

        }

        [Fact]
        public async Task GetByApplicationIdAsync_Should_Get_Interview_Successfully()
        {
            Guid applicationId = Guid.NewGuid();

            ApplicationEntity application = new ApplicationEntity
            {
                Id = applicationId,
                CandidateId = Guid.NewGuid(),
                JobId = Guid.NewGuid()
            };

            List<Interview> interviews = new List<Interview>
            {
                new Interview
                {
                    ApplicationId = applicationId
                }
            };

            List<InterviewDetailedResponseDto> interviewDetailedResponses = new List<InterviewDetailedResponseDto>
            {
                new InterviewDetailedResponseDto
                {
                    ApplicationId = applicationId
                }
            };

            _applicationRepositoryMock.Setup(x => x.GetByIdAsync(applicationId, It.IsAny<CancellationToken>())).ReturnsAsync(application);
            _interviewRepositoryMock.Setup(x => x.GetByApplicationIdAsync(applicationId, It.IsAny<CancellationToken>())).ReturnsAsync(interviews);
            _mapper.Setup(x => x.Map<List<InterviewDetailedResponseDto>>(interviews)).Returns(interviewDetailedResponses);

            var result = await _interviewService.GetByApplicationIdAsync(applicationId, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(applicationId, result[0].ApplicationId);

            _applicationRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            _interviewRepositoryMock.Verify(x => x.GetByApplicationIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            _mapper.Verify(x => x.Map<List<InterviewDetailedResponseDto>>(It.IsAny<List<Interview>>()), Times.Once);

        }

        [Fact]
        public async Task GetByApplicationIdAsync_Should_Throw_When_Application_NotFound()
        {
            Guid applicationId = Guid.NewGuid();

            ApplicationEntity application = new ApplicationEntity
            {
                Id = applicationId,
                CandidateId = Guid.NewGuid(),
                JobId = Guid.NewGuid()
            };

            List<Interview> interviews = new List<Interview>
            {
                new Interview
                {
                    ApplicationId = applicationId
                }
            };

            List<InterviewDetailedResponseDto> interviewDetailedResponses = new List<InterviewDetailedResponseDto>
            {
                new InterviewDetailedResponseDto
                {
                    ApplicationId = applicationId
                }
            };

            _applicationRepositoryMock.Setup(x => x.GetByIdAsync(applicationId, It.IsAny<CancellationToken>())).ReturnsAsync((ApplicationEntity?)null);

            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _interviewService.GetByApplicationIdAsync(applicationId, CancellationToken.None));

            Assert.NotNull(exception);
            Assert.Equal("Application not found.", exception.Message);

            _applicationRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            _interviewRepositoryMock.Verify(x => x.GetByApplicationIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
            _mapper.Verify(x => x.Map<List<InterviewDetailedResponseDto>>(It.IsAny<List<Interview>>()), Times.Never);

        }


        [Fact]
        public async Task UpdateInterviewStatusAsync_Should_Update_Status_To_Passed_Successfully()
        {
            Guid interviewId = Guid.NewGuid();

            UpdateInterviewStatusDto updateInterviewStatus = new UpdateInterviewStatusDto
            {
                Status = InterviewStatus.Passed,
                Feedback = "feedback"
            };

            Interview interview = new Interview
            {
                Id = interviewId,
                ApplicationId = Guid.NewGuid(),
                InterviewerId = Guid.NewGuid(),
                Status = InterviewStatus.Scheduled
            };

            Job job = new Job
            {
                Id = Guid.NewGuid(),
                Title = "title",
            };

            ApplicationEntity application = new ApplicationEntity
            {
                Id = interview.ApplicationId,
                JobId = job.Id,
                Job = job,
                Status = ApplicationStatus.InterviewScheduled
            };

            InterviewResponseDto interviewResponse = new InterviewResponseDto
            {
                Id = interviewId,
                Status = InterviewStatus.Passed
            };

            _interviewRepositoryMock.Setup(x => x.GetByIdAsync(interviewId, It.IsAny<CancellationToken>())).ReturnsAsync(interview);
            _unitOfWorkMock.Setup(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            _interviewRepositoryMock.Setup(x => x.Update(interview));
            _applicationRepositoryMock.Setup(x => x.GetByIdAsync(interview.ApplicationId, It.IsAny<CancellationToken>())).ReturnsAsync(application);
            _applicationRepositoryMock.Setup(x => x.Update(application));
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()));
            _unitOfWorkMock.Setup(x => x.CommitTransactionAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            _notificationServiceMock.Setup(x => x.SendAsync(It.IsAny<CreateNotificationDto>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            _mapper.Setup(x => x.Map<InterviewResponseDto>(interview)).Returns(interviewResponse);

            var result = await _interviewService.UpdateInterviewStatusAsync(interviewId, updateInterviewStatus, CancellationToken.None);

            Assert.Equal(updateInterviewStatus.Status, result.Status);
            Assert.Equal(ApplicationStatus.InterviewCompleted, application.Status);

            _interviewRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
            _interviewRepositoryMock.Verify(x => x.Update(It.IsAny<Interview>()), Times.Once);
            _applicationRepositoryMock.Verify(x => x.Update(It.IsAny<ApplicationEntity>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.CommitTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.RollbackTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);

        }

        [Fact]
        public async Task UpdateInterviewStatusAsync_Should_Update_Status_To_Failed_Successfully()
        {
            Guid interviewId = Guid.NewGuid();

            UpdateInterviewStatusDto updateInterviewStatus = new UpdateInterviewStatusDto
            {
                Status = InterviewStatus.Failed,
                Feedback = "feedback"
            };

            Interview interview = new Interview
            {
                Id = interviewId,
                ApplicationId = Guid.NewGuid(),
                InterviewerId = Guid.NewGuid(),
                Status = InterviewStatus.Scheduled
            };

            Job job = new Job
            {
                Id = Guid.NewGuid(),
                Title = "title",
            };

            ApplicationEntity application = new ApplicationEntity
            {
                Id = interview.ApplicationId,
                JobId = job.Id,
                Job = job,
                Status = ApplicationStatus.InterviewScheduled
            };

            InterviewResponseDto interviewResponse = new InterviewResponseDto
            {
                Id = interviewId,
                Status = InterviewStatus.Failed
            };

            _interviewRepositoryMock.Setup(x => x.GetByIdAsync(interviewId, It.IsAny<CancellationToken>())).ReturnsAsync(interview);
            _unitOfWorkMock.Setup(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            _interviewRepositoryMock.Setup(x => x.Update(interview));
            _applicationRepositoryMock.Setup(x => x.GetByIdAsync(interview.ApplicationId, It.IsAny<CancellationToken>())).ReturnsAsync(application);
            _applicationRepositoryMock.Setup(x => x.Update(application));
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()));
            _unitOfWorkMock.Setup(x => x.CommitTransactionAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            _notificationServiceMock.Setup(x => x.SendAsync(It.IsAny<CreateNotificationDto>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            _mapper.Setup(x => x.Map<InterviewResponseDto>(interview)).Returns(interviewResponse);

            var result = await _interviewService.UpdateInterviewStatusAsync(interviewId, updateInterviewStatus, CancellationToken.None);

            Assert.Equal(updateInterviewStatus.Status, result.Status);
            Assert.Equal(ApplicationStatus.Rejected, application.Status);

            _interviewRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
            _interviewRepositoryMock.Verify(x => x.Update(It.IsAny<Interview>()), Times.Once);
            _applicationRepositoryMock.Verify(x => x.Update(It.IsAny<ApplicationEntity>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.CommitTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.RollbackTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);

        }

        [Fact]
        public async Task UpdateInterviewStatusAsync_Should_Throw_When_Interview_NotFound()
        {
            Guid interviewId = Guid.NewGuid();

            UpdateInterviewStatusDto updateInterviewStatus = new UpdateInterviewStatusDto
            {
                Status = InterviewStatus.Passed,
                Feedback = "feedback"
            };

            _interviewRepositoryMock.Setup(x => x.GetByIdAsync(interviewId, It.IsAny<CancellationToken>())).ReturnsAsync((Interview?)null);

            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _interviewService.UpdateInterviewStatusAsync(interviewId, updateInterviewStatus, CancellationToken.None));

            Assert.Equal("Interview Not Found", exception.Message);

            _interviewRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
            _interviewRepositoryMock.Verify(x => x.Update(It.IsAny<Interview>()), Times.Never);
            _applicationRepositoryMock.Verify(x => x.Update(It.IsAny<ApplicationEntity>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.CommitTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task UpdateInterviewStatusAsync_Should_Throw_When_Interview_Status_NotValid()
        {
            Guid interviewId = Guid.NewGuid();

            UpdateInterviewStatusDto updateInterviewStatus = new UpdateInterviewStatusDto
            {
                Status = InterviewStatus.Scheduled,
                Feedback = "feedback"
            };

            Interview interview = new Interview
            {
                Id = interviewId,
                ApplicationId = Guid.NewGuid(),
                InterviewerId = Guid.NewGuid(),
                Status = InterviewStatus.Scheduled
            };

            _interviewRepositoryMock.Setup(x => x.GetByIdAsync(interviewId, It.IsAny<CancellationToken>())).ReturnsAsync(interview);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _interviewService.UpdateInterviewStatusAsync(interviewId, updateInterviewStatus, CancellationToken.None));

            Assert.Equal($"Cannot change status from '{interview.Status}' to '{updateInterviewStatus.Status}'. ", exception.Message);

            _interviewRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
            _interviewRepositoryMock.Verify(x => x.Update(It.IsAny<Interview>()), Times.Never);
            _applicationRepositoryMock.Verify(x => x.Update(It.IsAny<ApplicationEntity>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.CommitTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task UpdateInterviewStatusAsync_Should_Throw_When_Interview_Updated_Failed()
        {
            Guid interviewId = Guid.NewGuid();

            UpdateInterviewStatusDto updateInterviewStatus = new UpdateInterviewStatusDto
            {
                Status = InterviewStatus.Failed,
                Feedback = "feedback"
            };

            Interview interview = new Interview
            {
                Id = interviewId,
                ApplicationId = Guid.NewGuid(),
                InterviewerId = Guid.NewGuid(),
                Status = InterviewStatus.Scheduled
            };

            _interviewRepositoryMock.Setup(x => x.GetByIdAsync(interviewId, It.IsAny<CancellationToken>())).ReturnsAsync(interview);
            _unitOfWorkMock.Setup(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            _interviewRepositoryMock.Setup(x => x.Update(interview)).Throws<InvalidOperationException>();

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _interviewService.UpdateInterviewStatusAsync(interviewId, updateInterviewStatus, CancellationToken.None));

            _interviewRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
            _interviewRepositoryMock.Verify(x => x.Update(It.IsAny<Interview>()), Times.Once);
            _applicationRepositoryMock.Verify(x => x.Update(It.IsAny<ApplicationEntity>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.CommitTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.RollbackTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);

        }

        [Fact]
        public async Task UpdateInterviewStatusAsync_Should_Throw_When_application_NotFound()
        {
            Guid interviewId = Guid.NewGuid();

            UpdateInterviewStatusDto updateInterviewStatus = new UpdateInterviewStatusDto
            {
                Status = InterviewStatus.Failed,
                Feedback = "feedback"
            };

            Interview interview = new Interview
            {
                Id = interviewId,
                ApplicationId = Guid.NewGuid(),
                InterviewerId = Guid.NewGuid(),
                Status = InterviewStatus.Scheduled
            };

            _interviewRepositoryMock.Setup(x => x.GetByIdAsync(interviewId, It.IsAny<CancellationToken>())).ReturnsAsync(interview);
            _unitOfWorkMock.Setup(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            _interviewRepositoryMock.Setup(x => x.Update(interview));
            _applicationRepositoryMock.Setup(x => x.GetByIdAsync(interview.ApplicationId, It.IsAny<CancellationToken>())).ReturnsAsync((ApplicationEntity?)null);

            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _interviewService.UpdateInterviewStatusAsync(interviewId, updateInterviewStatus, CancellationToken.None));

            Assert.Equal($"Application not found.", exception.Message);

            _interviewRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
            _interviewRepositoryMock.Verify(x => x.Update(It.IsAny<Interview>()), Times.Once);
            _applicationRepositoryMock.Verify(x => x.Update(It.IsAny<ApplicationEntity>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.CommitTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.RollbackTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);

        }




        [Fact]
        public async Task RescheduleInterviewAsync_Should_Reschedule_Interview_Successfully()
        {
            Guid interviewId = Guid.NewGuid();

            RescheduleInterviewDto rescheduleInterview = new RescheduleInterviewDto
            {
                ScheduledAt = DateTime.UtcNow.AddHours(7),
                Location = "meet"
            };

            Interview interview = new Interview
            {
                Id = interviewId,
                ApplicationId = Guid.NewGuid(),
                Status = InterviewStatus.Cancelled,
            };

            Job job = new Job
            {
                Id = Guid.NewGuid(),
                Title = "title",
            };

            ApplicationEntity application = new ApplicationEntity
            {
                Id = interview.ApplicationId,
                CandidateId = Guid.NewGuid(),
                JobId = job.Id,
                Job = job

            };

            InterviewResponseDto interviewResponse = new InterviewResponseDto
            {
                Id = interview.Id,
                ApplicationId =application.Id,
                Status = InterviewStatus.Scheduled,
                ScheduledAt = rescheduleInterview.ScheduledAt

            };

            _interviewRepositoryMock.Setup(x => x.GetByIdAsync(interviewId, It.IsAny<CancellationToken>())).ReturnsAsync(interview);
            _interviewRepositoryMock.Setup(x => x.Update(interview));
            _applicationRepositoryMock.Setup(x => x.GetByIdAsync(interview.ApplicationId, It.IsAny<CancellationToken>())).ReturnsAsync(application);
            _applicationRepositoryMock.Setup(x => x.Update(application));
            _notificationServiceMock.Setup(x => x.SendAsync(It.IsAny<CreateNotificationDto>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            _mapper.Setup(x => x.Map<InterviewResponseDto>(interview)).Returns(interviewResponse);

            var result = await _interviewService.RescheduleInterviewAsync(interviewId, rescheduleInterview, CancellationToken.None);

            Assert.Equal(interviewId, result.Id);
            Assert.Equal(rescheduleInterview.ScheduledAt, result.ScheduledAt);


            _interviewRepositoryMock.Verify(x => x.GetByIdAsync(interviewId, It.IsAny<CancellationToken>()), Times.Once);
            _interviewRepositoryMock.Verify(x => x.Update(interview), Times.Once);
            _applicationRepositoryMock.Verify(x => x.GetByIdAsync(interview.ApplicationId, It.IsAny<CancellationToken>()), Times.Once);
            _applicationRepositoryMock.Verify(x => x.Update(application), Times.Once);
            _notificationServiceMock.Verify(x => x.SendAsync(It.IsAny<CreateNotificationDto>(), It.IsAny<CancellationToken>()), Times.Exactly(2));
            _unitOfWorkMock.Verify(x => x.RollbackTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);


        }

        [Fact]
        public async Task RescheduleInterviewAsync_Should_Throw_When_Interview_NotFound()
        {
            Guid interviewId = Guid.NewGuid();

            RescheduleInterviewDto rescheduleInterview = new RescheduleInterviewDto
            {
                ScheduledAt = DateTime.UtcNow.AddHours(7),
                Location = "meet"
            };

            _interviewRepositoryMock.Setup(x => x.GetByIdAsync(interviewId, It.IsAny<CancellationToken>())).ReturnsAsync((Interview?)null);

            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _interviewService.RescheduleInterviewAsync(interviewId, rescheduleInterview, CancellationToken.None));

            Assert.Equal("Interview not found.", exception.Message);


            _interviewRepositoryMock.Verify(x => x.GetByIdAsync(interviewId, It.IsAny<CancellationToken>()), Times.Once);
            _interviewRepositoryMock.Verify(x => x.Update(It.IsAny<Interview>()), Times.Never);
            _applicationRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
            _applicationRepositoryMock.Verify(x => x.Update(It.IsAny<ApplicationEntity>()), Times.Never);
            _notificationServiceMock.Verify(x => x.SendAsync(It.IsAny<CreateNotificationDto>(), It.IsAny<CancellationToken>()), Times.Exactly(0));
            _unitOfWorkMock.Verify(x => x.RollbackTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);

        }

        [Fact]
        public async Task RescheduleInterviewAsync_Should_Throw_When_Interview_Status_NotValid()
        {
            Guid interviewId = Guid.NewGuid();

            RescheduleInterviewDto rescheduleInterview = new RescheduleInterviewDto
            {
                ScheduledAt = DateTime.UtcNow.AddHours(7),
                Location = "meet"
            };

            Interview interview = new Interview
            {
                Id = interviewId,
                ApplicationId = Guid.NewGuid(),
                Status = InterviewStatus.Completed,
            };

            _interviewRepositoryMock.Setup(x => x.GetByIdAsync(interviewId, It.IsAny<CancellationToken>())).ReturnsAsync(interview);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _interviewService.RescheduleInterviewAsync(interviewId, rescheduleInterview, CancellationToken.None));

            Assert.Equal($"Cannot reschedule interview from '{interview.Status}'.", exception.Message);


            _interviewRepositoryMock.Verify(x => x.GetByIdAsync(interviewId, It.IsAny<CancellationToken>()), Times.Once);
            _interviewRepositoryMock.Verify(x => x.Update(It.IsAny<Interview>()), Times.Never);
            _applicationRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
            _applicationRepositoryMock.Verify(x => x.Update(It.IsAny<ApplicationEntity>()), Times.Never);
            _notificationServiceMock.Verify(x => x.SendAsync(It.IsAny<CreateNotificationDto>(), It.IsAny<CancellationToken>()), Times.Exactly(0));
            _unitOfWorkMock.Verify(x => x.RollbackTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);

        }

        [Fact]
        public async Task RescheduleInterviewAsync_Should_Throw_When_ScheduledAt_NotValid()
        {
            Guid interviewId = Guid.NewGuid();

            RescheduleInterviewDto rescheduleInterview = new RescheduleInterviewDto
            {
                ScheduledAt = DateTime.UtcNow,
                Location = "meet"
            };


            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _interviewService.RescheduleInterviewAsync(interviewId, rescheduleInterview, CancellationToken.None));

            Assert.Equal("Interview must be scheduled in the future.", exception.Message);

            _interviewRepositoryMock.Verify(x => x.GetByIdAsync(interviewId, It.IsAny<CancellationToken>()), Times.Never);
            _interviewRepositoryMock.Verify(x => x.Update(It.IsAny<Interview>()), Times.Never);
            _applicationRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
            _applicationRepositoryMock.Verify(x => x.Update(It.IsAny<ApplicationEntity>()), Times.Never);
            _notificationServiceMock.Verify(x => x.SendAsync(It.IsAny<CreateNotificationDto>(), It.IsAny<CancellationToken>()), Times.Exactly(0));
            _unitOfWorkMock.Verify(x => x.RollbackTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);

        }


        [Fact]
        public async Task RescheduleInterviewAsync_Should_Throw_When_Application_NotFound()
        {
            Guid interviewId = Guid.NewGuid();

            RescheduleInterviewDto rescheduleInterview = new RescheduleInterviewDto
            {
                ScheduledAt = DateTime.UtcNow.AddHours(7),
                Location = "meet"
            };

            Interview interview = new Interview
            {
                Id = interviewId,
                ApplicationId = Guid.NewGuid(),
                Status = InterviewStatus.Cancelled,
            };

            Job job = new Job
            {
                Id = Guid.NewGuid(),
                Title = "title",
            };

            _interviewRepositoryMock.Setup(x => x.GetByIdAsync(interviewId, It.IsAny<CancellationToken>())).ReturnsAsync(interview);
            _interviewRepositoryMock.Setup(x => x.Update(interview));
            _applicationRepositoryMock.Setup(x => x.GetByIdAsync(interview.ApplicationId, It.IsAny<CancellationToken>())).ReturnsAsync((ApplicationEntity?)null);

            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _interviewService.RescheduleInterviewAsync(interviewId, rescheduleInterview, CancellationToken.None));

            Assert.Equal("Application not found.", exception.Message);

            _interviewRepositoryMock.Verify(x => x.GetByIdAsync(interviewId, It.IsAny<CancellationToken>()), Times.Once);
            _interviewRepositoryMock.Verify(x => x.Update(It.IsAny<Interview>()), Times.Once);
            _applicationRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            _applicationRepositoryMock.Verify(x => x.Update(It.IsAny<ApplicationEntity>()), Times.Never);
            _notificationServiceMock.Verify(x => x.SendAsync(It.IsAny<CreateNotificationDto>(), It.IsAny<CancellationToken>()), Times.Exactly(0));
            _unitOfWorkMock.Verify(x => x.RollbackTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);

        }

        [Fact]
        public async Task InterviewsAssignedToInterviwerAsync_Should_Get_Interviews_Successfully()
        {
            var interviewerId = Guid.NewGuid();

            User user = new User
            {
                Id = interviewerId,
                Name = "name",
                Status = UserStatus.Active
            };

            Role role = new Role
            {
                Id = Guid.NewGuid(),
                Name = RoleName.Manager
            };


            UserRole userRole = new UserRole
            {
                Id = Guid.NewGuid(),
                RoleId = role.Id,
                Role = role,
                UserId = user.Id,
                User = user
                
            };

            List<Interview> interviews = new List<Interview>
            {
                new Interview
                {
                    ApplicationId = Guid.NewGuid(),
                    InterviewerId = interviewerId,
                }
            };

            List<InterviewDetailedResponseDto> interviewDetailedResponses = new List<InterviewDetailedResponseDto>
            {
                new InterviewDetailedResponseDto
                {
                    ApplicationId = Guid.NewGuid(),
                    InterviewerId = interviewerId,
                }
            };

            _userRepositoryMock.Setup(x => x.GetUserByIdAsync(interviewerId, It.IsAny<CancellationToken>())).ReturnsAsync(user);
            _userRoleRepositoryMock.Setup(x => x.GetByUserIdWithRoleAsync(interviewerId, It.IsAny<CancellationToken>())).ReturnsAsync(userRole);
            _interviewRepositoryMock.Setup(x => x.GetByInterviewerIdAsync(interviewerId, It.IsAny<CancellationToken>())).ReturnsAsync(interviews);
            _mapper.Setup(x => x.Map<List<InterviewDetailedResponseDto>>(interviews)).Returns(interviewDetailedResponses);

            var result = await _interviewService.InterviewsAssignedToInterviwerAsync(interviewerId, CancellationToken.None);

            Assert.Equal(interviewerId, result[0].InterviewerId);

            _userRepositoryMock.Verify(x => x.GetUserByIdAsync(interviewerId, It.IsAny<CancellationToken>()), Times.Once);
            _userRoleRepositoryMock.Verify(x => x.GetByUserIdWithRoleAsync(interviewerId, It.IsAny<CancellationToken>()), Times.Once);
            _interviewRepositoryMock.Verify(x => x.GetByInterviewerIdAsync(interviewerId, It.IsAny<CancellationToken>()), Times.Once);
            _mapper.Verify(x => x.Map<List<InterviewDetailedResponseDto>>(interviews), Times.Once);


        }

        [Fact]
        public async Task InterviewsAssignedToInterviwerAsync_Should_Throw_When_Interviewer_NotFound()
        {
            var interviewerId = Guid.NewGuid();
            _userRepositoryMock.Setup(x => x.GetUserByIdAsync(interviewerId, It.IsAny<CancellationToken>())).ReturnsAsync((User?)null);

            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _interviewService.InterviewsAssignedToInterviwerAsync(interviewerId, CancellationToken.None));

            Assert.Equal("Interviewer not found.", exception.Message);

            _userRepositoryMock.Verify(x => x.GetUserByIdAsync(interviewerId, It.IsAny<CancellationToken>()), Times.Once);
            _userRoleRepositoryMock.Verify(x => x.GetByUserIdWithRoleAsync(interviewerId, It.IsAny<CancellationToken>()), Times.Never);
            _interviewRepositoryMock.Verify(x => x.GetByInterviewerIdAsync(interviewerId, It.IsAny<CancellationToken>()), Times.Never);
            _mapper.Verify(x => x.Map<List<InterviewDetailedResponseDto>>(It.IsAny<Interview>()), Times.Never);

        }

        [Fact]
        public async Task InterviewsAssignedToInterviwerAsync_Should_Throw_When_Interviewer_NotActive()
        {
            var interviewerId = Guid.NewGuid();

            User user = new User
            {
                Id = interviewerId,
                Name = "name",
                Status = UserStatus.Inactive
            };
            _userRepositoryMock.Setup(x => x.GetUserByIdAsync(interviewerId, It.IsAny<CancellationToken>())).ReturnsAsync(user);


            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _interviewService.InterviewsAssignedToInterviwerAsync(interviewerId, CancellationToken.None));

            Assert.Equal("Interviewer account is not active.", exception.Message);

            _userRepositoryMock.Verify(x => x.GetUserByIdAsync(interviewerId, It.IsAny<CancellationToken>()), Times.Once);
            _userRoleRepositoryMock.Verify(x => x.GetByUserIdWithRoleAsync(interviewerId, It.IsAny<CancellationToken>()), Times.Never);
            _interviewRepositoryMock.Verify(x => x.GetByInterviewerIdAsync(interviewerId, It.IsAny<CancellationToken>()), Times.Never);
            _mapper.Verify(x => x.Map<List<InterviewDetailedResponseDto>>(It.IsAny<Interview>()), Times.Never);

        }

        [Fact]
        public async Task InterviewsAssignedToInterviwerAsync_Should_Throw_When_Role_NotAssigned()
        {
            var interviewerId = Guid.NewGuid();

            User user = new User
            {
                Id = interviewerId,
                Name = "name",
                Status = UserStatus.Active
            };

            _userRepositoryMock.Setup(x => x.GetUserByIdAsync(interviewerId, It.IsAny<CancellationToken>())).ReturnsAsync(user);
            _userRoleRepositoryMock.Setup(x => x.GetByUserIdWithRoleAsync(interviewerId, It.IsAny<CancellationToken>())).ReturnsAsync((UserRole?)null);


            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _interviewService.InterviewsAssignedToInterviwerAsync(interviewerId, CancellationToken.None));

            Assert.Equal("Role Not Assigned.", exception.Message);

            _userRepositoryMock.Verify(x => x.GetUserByIdAsync(interviewerId, It.IsAny<CancellationToken>()), Times.Once);
            _userRoleRepositoryMock.Verify(x => x.GetByUserIdWithRoleAsync(interviewerId, It.IsAny<CancellationToken>()), Times.Once);
            _interviewRepositoryMock.Verify(x => x.GetByInterviewerIdAsync(interviewerId, It.IsAny<CancellationToken>()), Times.Never);
            _mapper.Verify(x => x.Map<List<InterviewDetailedResponseDto>>(It.IsAny<Interview>()), Times.Never);

        }

        [Fact]
        public async Task InterviewsAssignedToInterviwerAsync_Should_Throw_When_Role_NotValid()
        {
            var interviewerId = Guid.NewGuid();

            User user = new User
            {
                Id = interviewerId,
                Name = "name",
                Status = UserStatus.Active
            };

            Role role = new Role
            {
                Id = Guid.NewGuid(),
                Name = RoleName.Employee
            };


            UserRole userRole = new UserRole
            {
                Id = Guid.NewGuid(),
                RoleId = role.Id,
                Role = role,
                UserId = user.Id,
                User = user

            };

            _userRepositoryMock.Setup(x => x.GetUserByIdAsync(interviewerId, It.IsAny<CancellationToken>())).ReturnsAsync(user);
            _userRoleRepositoryMock.Setup(x => x.GetByUserIdWithRoleAsync(interviewerId, It.IsAny<CancellationToken>())).ReturnsAsync(userRole);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _interviewService.InterviewsAssignedToInterviwerAsync(interviewerId, CancellationToken.None));

            Assert.Equal("Selected user cannot conduct interviews.", exception.Message);

            _userRepositoryMock.Verify(x => x.GetUserByIdAsync(interviewerId, It.IsAny<CancellationToken>()), Times.Once);
            _userRoleRepositoryMock.Verify(x => x.GetByUserIdWithRoleAsync(interviewerId, It.IsAny<CancellationToken>()), Times.Once);
            _interviewRepositoryMock.Verify(x => x.GetByInterviewerIdAsync(interviewerId, It.IsAny<CancellationToken>()), Times.Never);
            _mapper.Verify(x => x.Map<List<InterviewDetailedResponseDto>>(It.IsAny<Interview>()), Times.Never);

        }

        [Fact]
        public async Task GetByIdWithDetailsAsync_Should_Get_Detailed_Interview_Successfully()
        {
            var interviewId = Guid.NewGuid();

            Interview interview = new Interview
            {
                Id = interviewId,
                ApplicationId = Guid.NewGuid(),
                
            };

            InterviewDetailedResponseDto interviewDetailedResponse = new InterviewDetailedResponseDto
            {
                Id = interviewId,
                ApplicationId = Guid.NewGuid(),
            };

            _interviewRepositoryMock.Setup(x => x.GetByIdWithDetailsAsync(interviewId, It.IsAny<CancellationToken>())).ReturnsAsync(interview);
            _mapper.Setup(x => x.Map<InterviewDetailedResponseDto>(interview)).Returns(interviewDetailedResponse);

            var result = await _interviewService.GetByIdWithDetailsAsync(interviewId, CancellationToken.None);

            Assert.Equal(interviewId, result.Id);

            _interviewRepositoryMock.Verify(x => x.GetByIdWithDetailsAsync(interviewId, It.IsAny<CancellationToken>()), Times.Once);
            _mapper.Verify(x => x.Map<InterviewDetailedResponseDto>(interview), Times.Once);

        }

        [Fact]
        public async Task GetByIdWithDetailsAsync_Should_Throw_When_Interview_NotFound()
        {
            var interviewId = Guid.NewGuid();


            _interviewRepositoryMock.Setup(x => x.GetByIdWithDetailsAsync(interviewId, It.IsAny<CancellationToken>())).ReturnsAsync((Interview?)null);

            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _interviewService.GetByIdWithDetailsAsync(interviewId, CancellationToken.None));

            Assert.Equal("Interview not found.", exception.Message);

            _interviewRepositoryMock.Verify(x => x.GetByIdWithDetailsAsync(interviewId, It.IsAny<CancellationToken>()), Times.Once);
            _mapper.Verify(x => x.Map<InterviewDetailedResponseDto>(It.IsAny<Interview>()), Times.Never);

        }

    }
}
