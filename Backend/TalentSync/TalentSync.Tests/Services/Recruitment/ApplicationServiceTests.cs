using AutoMapper;
using Azure;
using Castle.Core.Logging;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Application.Common.Pagination;
using TalentSync.Application.DTOs.Notifications;
using TalentSync.Application.DTOs.Recruitment;
using TalentSync.Application.Interfaces.Repositories;
using TalentSync.Application.Interfaces.Services;
using TalentSync.Application.Services.Notifications;
using TalentSync.Application.Services.Recruitment;
using TalentSync.Domain.Entities.Recruitment;
using TalentSync.Domain.Entities.User;
using static System.Net.Mime.MediaTypeNames;

namespace TalentSync.Tests.Services.Recruitment
{
    public class ApplicationServiceTests
    {
        private readonly Mock<IApplicationRepository> _applicationRepositoryMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IJobRepository> _jobRepositoryMock;
        private readonly Mock<IResumeRepository> _resumeRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<INotificationService> _notificationServiceMock;
        private readonly Mock<ILogger<ApplicationService>> _loggerMock;

        private readonly ApplicationService _applicationService;

        public ApplicationServiceTests()
        {
            _applicationRepositoryMock = new Mock<IApplicationRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _jobRepositoryMock = new Mock<IJobRepository>();
            _resumeRepositoryMock = new Mock<IResumeRepository>();
            _mapperMock = new Mock<IMapper>();
            _notificationServiceMock = new Mock<INotificationService>();
            _loggerMock = new Mock<ILogger<ApplicationService>>();

            _applicationService = new ApplicationService(
                _applicationRepositoryMock.Object,
                _userRepositoryMock.Object,
                _jobRepositoryMock.Object,
                _resumeRepositoryMock.Object,
                _mapperMock.Object,
                _notificationServiceMock.Object,
                _loggerMock.Object);
        }


        [Fact]
        public async Task CreateApplicationAsync_Should_Create_Application_Successfully()
        {
            Guid candidateId = Guid.NewGuid();
            CreateApplicationDto dto = new CreateApplicationDto
            {
                JobId = Guid.NewGuid(),
            };

            Resume resume = new Resume
            {
                FileUrl = "url",
                CandidateId = candidateId,
                FileName = "filename",
            };

            var application = new ApplicationEntity
            {
                CandidateId = candidateId,
                JobId = dto.JobId,
            };
            var job = new Job
            {
                Id = Guid.NewGuid(),
                Title = "title",
                Status = Domain.Enums.Recruitment.JobStatus.Open,
            };

            var user = new User
            {
                Id = candidateId,
                Name = "user",
                Email = "email",
                Status = Domain.Enums.User.UserStatus.Active
            };

            var response = new ApplicationResponseDto
            {
                CandidateId = application.CandidateId,
                JobId = application.JobId,
            };

            _resumeRepositoryMock.Setup(x => x.GetByCandidateId(candidateId, It.IsAny<CancellationToken>())).ReturnsAsync(resume);
            _mapperMock.Setup(x => x.Map<ApplicationEntity>(dto)).Returns(application);
            _jobRepositoryMock.Setup(x => x.GetJobByIdAsync(dto.JobId, It.IsAny<CancellationToken>())).ReturnsAsync(job);
            _userRepositoryMock.Setup(x => x.GetUserByIdAsync(candidateId, It.IsAny<CancellationToken>())).ReturnsAsync(user);
            _applicationRepositoryMock.Setup(x => x.ExistsAsync(dto.JobId, candidateId, It.IsAny<CancellationToken>())).ReturnsAsync(false);
            _applicationRepositoryMock.Setup(x => x.AddAsync(application, It.IsAny<CancellationToken>())).ReturnsAsync(application);
            _notificationServiceMock.Setup(x => x.SendAsync(It.IsAny<CreateNotificationDto>(), It.IsAny<CancellationToken>()));
            _mapperMock.Setup(x => x.Map<ApplicationResponseDto>(application)).Returns(response);


            var result = await _applicationService.CreateApplicationAsync(dto, candidateId, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(result.CandidateId, candidateId);

            _resumeRepositoryMock.Verify(x => x.GetByCandidateId(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            _jobRepositoryMock.Verify(x => x.GetJobByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            _applicationRepositoryMock.Verify(x => x.AddAsync(It.IsAny<ApplicationEntity>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task CreateApplicationAsync_Should_Throw_When_Resume_NotFound()
        {
            Guid candidateId = Guid.NewGuid();
            CreateApplicationDto dto = new CreateApplicationDto
            {
                JobId = Guid.NewGuid(),
            };
            _resumeRepositoryMock.Setup(x => x.GetByCandidateId(candidateId, It.IsAny<CancellationToken>())).ReturnsAsync((Resume?)null);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _applicationService.CreateApplicationAsync(dto, candidateId, CancellationToken.None));

            Assert.Equal("Resume Not Uploaded, Upload it First.", exception.Message);

            _resumeRepositoryMock.Verify(x => x.GetByCandidateId(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            _jobRepositoryMock.Verify(x => x.GetJobByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
        }


        [Fact]
        public async Task CreateApplicationAsync_Should_Throw_When_Job_NotFound()
        {
            Guid candidateId = Guid.NewGuid();
            CreateApplicationDto dto = new CreateApplicationDto
            {
                JobId = Guid.NewGuid(),
            };

            Resume resume = new Resume
            {
                FileUrl = "url",
                CandidateId = candidateId,
                FileName = "filename",
            };

            var application = new ApplicationEntity
            {
                CandidateId = candidateId,
                JobId = dto.JobId,
            };

            _resumeRepositoryMock.Setup(x => x.GetByCandidateId(candidateId, It.IsAny<CancellationToken>())).ReturnsAsync(resume);
            _mapperMock.Setup(x => x.Map<ApplicationEntity>(dto)).Returns(application);
            _jobRepositoryMock.Setup(x => x.GetJobByIdAsync(dto.JobId, It.IsAny<CancellationToken>())).ReturnsAsync((Job?)null);

            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _applicationService.CreateApplicationAsync(dto, candidateId, CancellationToken.None));

            Assert.Equal("Job Not Available", exception.Message);

            _resumeRepositoryMock.Verify(x => x.GetByCandidateId(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            _jobRepositoryMock.Verify(x => x.GetJobByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            _applicationRepositoryMock.Verify(x => x.AddAsync(It.IsAny<ApplicationEntity>(), It.IsAny<CancellationToken>()), Times.Never);

        }

        [Fact]
        public async Task CreateApplicationAsync_Should_Throw_When_Job_Closed()
        {
            Guid candidateId = Guid.NewGuid();
            CreateApplicationDto dto = new CreateApplicationDto
            {
                JobId = Guid.NewGuid(),
            };

            Resume resume = new Resume
            {
                FileUrl = "url",
                CandidateId = candidateId,
                FileName = "filename",
            };

            var application = new ApplicationEntity
            {
                CandidateId = candidateId,
                JobId = dto.JobId,
            };
            var job = new Job
            {
                Id = Guid.NewGuid(),
                Title = "title",
                Status = Domain.Enums.Recruitment.JobStatus.Closed,
            };

            _resumeRepositoryMock.Setup(x => x.GetByCandidateId(candidateId, It.IsAny<CancellationToken>())).ReturnsAsync(resume);
            _mapperMock.Setup(x => x.Map<ApplicationEntity>(dto)).Returns(application);
            _jobRepositoryMock.Setup(x => x.GetJobByIdAsync(dto.JobId, It.IsAny<CancellationToken>())).ReturnsAsync(job);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _applicationService.CreateApplicationAsync(dto, candidateId, CancellationToken.None));

            Assert.Equal($"Job '{job.Title}' is not accepting applications.", exception.Message);

            _resumeRepositoryMock.Verify(x => x.GetByCandidateId(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            _jobRepositoryMock.Verify(x => x.GetJobByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            _applicationRepositoryMock.Verify(x => x.AddAsync(It.IsAny<ApplicationEntity>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task CreateApplicationAsync_Should_Throw_When_User_NotFound(){
            Guid candidateId = Guid.NewGuid();
            CreateApplicationDto dto = new CreateApplicationDto
            {
                JobId = Guid.NewGuid(),
            };

            Resume resume = new Resume
            {
                FileUrl = "url",
                CandidateId = candidateId,
                FileName = "filename",
            };

            var application = new ApplicationEntity
            {
                CandidateId = candidateId,
                JobId = dto.JobId,
            };
            var job = new Job
            {
                Id = Guid.NewGuid(),
                Title = "title",
                Status = Domain.Enums.Recruitment.JobStatus.Open,
            };
           

            _resumeRepositoryMock.Setup(x => x.GetByCandidateId(candidateId, It.IsAny<CancellationToken>())).ReturnsAsync(resume);
            _mapperMock.Setup(x => x.Map<ApplicationEntity>(dto)).Returns(application);
            _jobRepositoryMock.Setup(x => x.GetJobByIdAsync(dto.JobId, It.IsAny<CancellationToken>())).ReturnsAsync(job);
            _userRepositoryMock.Setup(x => x.GetUserByIdAsync(candidateId, It.IsAny<CancellationToken>())).ReturnsAsync((User?)null);


            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _applicationService.CreateApplicationAsync(dto, candidateId, CancellationToken.None));

            Assert.Equal("User Not Available", exception.Message);

            _resumeRepositoryMock.Verify(x => x.GetByCandidateId(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            _jobRepositoryMock.Verify(x => x.GetJobByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            _userRepositoryMock.Verify(x => x.GetUserByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            _applicationRepositoryMock.Verify(x => x.AddAsync(It.IsAny<ApplicationEntity>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task CreateApplicationAsync_Should_Throw_When_User_InActive()
        {
            Guid candidateId = Guid.NewGuid();
            CreateApplicationDto dto = new CreateApplicationDto
            {
                JobId = Guid.NewGuid(),
            };

            Resume resume = new Resume
            {
                FileUrl = "url",
                CandidateId = candidateId,
                FileName = "filename",
            };

            var application = new ApplicationEntity
            {
                CandidateId = candidateId,
                JobId = dto.JobId,
            };
            var job = new Job
            {
                Id = Guid.NewGuid(),
                Title = "title",
                Status = Domain.Enums.Recruitment.JobStatus.Open,
            };

            var user = new User
            {
                Id = candidateId,
                Name = "user",
                Email = "email",
                Status = Domain.Enums.User.UserStatus.Inactive
            };

            _resumeRepositoryMock.Setup(x => x.GetByCandidateId(candidateId, It.IsAny<CancellationToken>())).ReturnsAsync(resume);
            _mapperMock.Setup(x => x.Map<ApplicationEntity>(dto)).Returns(application);
            _jobRepositoryMock.Setup(x => x.GetJobByIdAsync(dto.JobId, It.IsAny<CancellationToken>())).ReturnsAsync(job);
            _userRepositoryMock.Setup(x => x.GetUserByIdAsync(candidateId, It.IsAny<CancellationToken>())).ReturnsAsync(user);


            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _applicationService.CreateApplicationAsync(dto, candidateId, CancellationToken.None));

            Assert.Equal("Candidate account is not active.", exception.Message);

            _resumeRepositoryMock.Verify(x => x.GetByCandidateId(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            _jobRepositoryMock.Verify(x => x.GetJobByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            _userRepositoryMock.Verify(x => x.GetUserByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            _applicationRepositoryMock.Verify(x => x.AddAsync(It.IsAny<ApplicationEntity>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task CreateApplicationAsync_Shoud_Throw_When_Application_Already_Exists()
        {
            Guid candidateId = Guid.NewGuid();
            CreateApplicationDto dto = new CreateApplicationDto
            {
                JobId = Guid.NewGuid(),
            };

            Resume resume = new Resume
            {
                FileUrl = "url",
                CandidateId = candidateId,
                FileName = "filename",
            };

            var application = new ApplicationEntity
            {
                CandidateId = candidateId,
                JobId = dto.JobId,
            };
            var job = new Job
            {
                Id = Guid.NewGuid(),
                Title = "title",
                Status = Domain.Enums.Recruitment.JobStatus.Open,
            };

            var user = new User
            {
                Id = candidateId,
                Name = "user",
                Email = "email",
                Status = Domain.Enums.User.UserStatus.Active
            };

            _resumeRepositoryMock.Setup(x => x.GetByCandidateId(candidateId, It.IsAny<CancellationToken>())).ReturnsAsync(resume);
            _mapperMock.Setup(x => x.Map<ApplicationEntity>(dto)).Returns(application);
            _jobRepositoryMock.Setup(x => x.GetJobByIdAsync(dto.JobId, It.IsAny<CancellationToken>())).ReturnsAsync(job);
            _userRepositoryMock.Setup(x => x.GetUserByIdAsync(candidateId, It.IsAny<CancellationToken>())).ReturnsAsync(user);
            _applicationRepositoryMock.Setup(x => x.ExistsAsync(dto.JobId, candidateId, It.IsAny<CancellationToken>())).ReturnsAsync(true);


            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _applicationService.CreateApplicationAsync(dto, candidateId, CancellationToken.None));

            Assert.Equal("You have already applied for this job.", exception.Message);

            _resumeRepositoryMock.Verify(x => x.GetByCandidateId(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            _jobRepositoryMock.Verify(x => x.GetJobByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            _userRepositoryMock.Verify(x => x.GetUserByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            _applicationRepositoryMock.Verify(x => x.AddAsync(It.IsAny<ApplicationEntity>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task GetByIdAsync_Shoud_Get_Application_By_Id_Successfully()
        {
            Guid applicationId = Guid.NewGuid();

            ApplicationEntity application = new ApplicationEntity
            {
                CandidateId = Guid.NewGuid(),
                JobId = Guid.NewGuid(),
                Status = Domain.Enums.Recruitment.ApplicationStatus.Submitted
            };

            ApplicationWithDetailsResponseDto detailsResponseDto = new ApplicationWithDetailsResponseDto
            {
                CandidateId = application.CandidateId,
                JobId = application.JobId,
            };

            _applicationRepositoryMock.Setup(x => x.GetByIdWithDetailsAsync(applicationId, It.IsAny<CancellationToken>())).ReturnsAsync(application);
            _mapperMock.Setup(x => x.Map<ApplicationWithDetailsResponseDto>(application)).Returns(detailsResponseDto);

            var result = await _applicationService.GetByIdAsync(applicationId, CancellationToken.None);

            Assert.Equal(result.CandidateId, application.CandidateId);

            _applicationRepositoryMock.Verify(x => x.GetByIdWithDetailsAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(x => x.Map<ApplicationWithDetailsResponseDto>(application), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_Shoud_Throw_When_Application_NotFound()
        {
            Guid applicationId = Guid.NewGuid();

            _applicationRepositoryMock.Setup(x => x.GetByIdWithDetailsAsync(applicationId, It.IsAny<CancellationToken>())).ReturnsAsync((ApplicationEntity?)null);
    
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _applicationService.GetByIdAsync(applicationId, CancellationToken.None));

            Assert.Equal("Application Not Available", exception.Message);

            _applicationRepositoryMock.Verify(x => x.GetByIdWithDetailsAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(x => x.Map<ApplicationWithDetailsResponseDto>(It.IsAny<ApplicationEntity>()), Times.Never);
        }


        [Fact]
        public async Task GetAllAsync_Should_Get_All_Applications_Successfully()
        {

            PaginationRequest paginationRequest = new PaginationRequest
            {
                PageNumber = 1,
                PageSize = 10,
            };

            int count = 1;

            List<ApplicationEntity> applications = new List<ApplicationEntity>
            {
                new ApplicationEntity
                {
                    CandidateId = Guid.NewGuid(),
                    JobId = Guid.NewGuid(),
                }
            };

            List<ApplicationWithDetailsResponseDto> applicationWithDetailsResponses = new List<ApplicationWithDetailsResponseDto>
            {
                new ApplicationWithDetailsResponseDto
                {
                    CandidateId =applications[0].CandidateId,
                    JobId = applications[0].JobId,
                }
            };

            _applicationRepositoryMock.Setup(x => x.CountAsync(It.IsAny<CancellationToken>())).ReturnsAsync(count);
            _applicationRepositoryMock.Setup(x => x.GetPagedApplicationsAsync(It.IsAny<PaginationRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(applications);
            _mapperMock.Setup(x => x.Map<List<ApplicationWithDetailsResponseDto>>(applications)).Returns(applicationWithDetailsResponses);

            await _applicationService.GetAllAsync(paginationRequest, CancellationToken.None);

            _applicationRepositoryMock.Verify(x => x.CountAsync(It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(x => x.Map<List<ApplicationWithDetailsResponseDto>>(applications), Times.Once);

        }

        [Fact]
        public async Task GetByIdAsync_Shoud_Get_Application_By_JobId_Successfully()
        {
            Guid jobId = Guid.NewGuid();
            Job job = new Job
            {
                Id = jobId,
                Title = "Title",
                IsDeleted = false,
            };

            List<ApplicationEntity> applications = new List<ApplicationEntity>()
            {
                new ApplicationEntity
                {
                    JobId = jobId,
                    CandidateId = Guid.NewGuid(),
                }
            };

            List<ApplicationWithDetailsResponseDto> applicationWithDetailsResponses = new List<ApplicationWithDetailsResponseDto> { 
                
                new ApplicationWithDetailsResponseDto
                {
                    JobId = applications[0].JobId,
                    CandidateId = applications[0].CandidateId
                }
            };

            _jobRepositoryMock.Setup(x => x.GetJobByIdAsync(jobId, It.IsAny<CancellationToken>())).ReturnsAsync(job);
            _applicationRepositoryMock.Setup(x => x.GetByJobIdAsync(jobId, It.IsAny<CancellationToken>())).ReturnsAsync(applications);
            _mapperMock.Setup(x => x.Map<List<ApplicationWithDetailsResponseDto>>(applications)).Returns(applicationWithDetailsResponses);

            var result = await _applicationService.GetByJobIdAsync(jobId, CancellationToken.None);

            Assert.Equal(jobId, result[0].JobId);

            _jobRepositoryMock.Verify(x => x.GetJobByIdAsync(jobId, It.IsAny<CancellationToken>()), Times.Once);
            _applicationRepositoryMock.Verify(x => x.GetByJobIdAsync(jobId, It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(x => x.Map<List<ApplicationWithDetailsResponseDto>>(applications), Times.Once);

        }

        [Fact]
        public async Task GetByIdAsync_Shoud_Throw_When_JobId_NotFound()
        {
            Guid jobId = Guid.NewGuid();

            _jobRepositoryMock.Setup(x => x.GetJobByIdAsync(jobId, It.IsAny<CancellationToken>())).ReturnsAsync((Job?)null);

            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _applicationService.GetByJobIdAsync(jobId, CancellationToken.None));

            Assert.Equal("Job Not Found", exception.Message);

            _jobRepositoryMock.Verify(x => x.GetJobByIdAsync(jobId, It.IsAny<CancellationToken>()), Times.Once);
            _applicationRepositoryMock.Verify(x => x.GetByJobIdAsync(jobId, It.IsAny<CancellationToken>()), Times.Never);
            _mapperMock.Verify(x => x.Map<List<ApplicationWithDetailsResponseDto>>(It.IsAny<ApplicationEntity>()), Times.Never);

        }


        [Fact]
        public async Task GetByIdAsync_Shoud_Get_Application_By_CandidateId_Successfully()
        {
            Guid candidateId = Guid.NewGuid();
            User user = new User
            {
                Id = candidateId,
                Name = "Test",
                Status =  Domain.Enums.User.UserStatus.Active
            };


            List<ApplicationEntity> applications = new List<ApplicationEntity>()
            {
                new ApplicationEntity
                {
                    JobId = Guid.NewGuid(),
                    CandidateId = user.Id,
                }
            };

            List<ApplicationWithDetailsResponseDto> applicationWithDetailsResponses = new List<ApplicationWithDetailsResponseDto> {

                new ApplicationWithDetailsResponseDto
                {
                    JobId = applications[0].JobId,
                    CandidateId = applications[0].CandidateId
                }
            };

            _userRepositoryMock.Setup(x => x.GetUserByIdAsync(candidateId, It.IsAny<CancellationToken>())).ReturnsAsync(user);
            _applicationRepositoryMock.Setup(x => x.GetByCandidateIdAsync(candidateId, It.IsAny<CancellationToken>())).ReturnsAsync(applications);
            _mapperMock.Setup(x => x.Map<List<ApplicationWithDetailsResponseDto>>(applications)).Returns(applicationWithDetailsResponses);

            var result = await _applicationService.GetByCandidateIdAsync(candidateId, CancellationToken.None);

            Assert.Equal(candidateId, result[0].CandidateId);

            _userRepositoryMock.Verify(x => x.GetUserByIdAsync(candidateId, It.IsAny<CancellationToken>()), Times.Once);
            _applicationRepositoryMock.Verify(x => x.GetByCandidateIdAsync(candidateId, It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(x => x.Map<List<ApplicationWithDetailsResponseDto>>(applications), Times.Once);

        }

        [Fact]
        public async Task GetByIdAsync_Shoud_Throw_When_CandidateId_NotFound()
        {
            Guid candidateId = Guid.NewGuid();


            _userRepositoryMock.Setup(x => x.GetUserByIdAsync(candidateId, It.IsAny<CancellationToken>())).ReturnsAsync((User?)null);

            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _applicationService.GetByCandidateIdAsync(candidateId, CancellationToken.None));

            Assert.Equal("Candidate Not Found", exception.Message);

            _userRepositoryMock.Verify(x => x.GetUserByIdAsync(candidateId, It.IsAny<CancellationToken>()), Times.Once);
            _applicationRepositoryMock.Verify(x => x.GetByCandidateIdAsync(candidateId, It.IsAny<CancellationToken>()), Times.Never);

        }


        [Fact]
        public async Task UpdateApplicationAsync_Should_Update_Application_Successfully()
        {
            Guid applicationId = Guid.NewGuid();
            UpdateApplicationRequestDto update = new UpdateApplicationRequestDto
            {
                Status = Domain.Enums.Recruitment.ApplicationStatus.Screening
            };


            ApplicationEntity application = new ApplicationEntity
            {
                Id = applicationId,
                Status = Domain.Enums.Recruitment.ApplicationStatus.Submitted
            };


            ApplicationResponseDto applicationResponse = new ApplicationResponseDto
            {
                Id = application.Id,
                Status = update.Status,
            };

            _applicationRepositoryMock.Setup(x => x.GetByIdAsync(applicationId, It.IsAny<CancellationToken>())).ReturnsAsync(application);
            _mapperMock.Setup(x => x.Map(update, application));
            _applicationRepositoryMock.Setup(x => x.Update(application));
            _mapperMock.Setup(x => x.Map<ApplicationResponseDto>(application)).Returns(applicationResponse);

            var result = await _applicationService.UpdateApplicationAsync(applicationId, update, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(applicationId, result.Id);
            Assert.Equal(update.Status, result.Status);

            _applicationRepositoryMock.Verify(x => x.GetByIdAsync(applicationId, It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(x => x.Map(update, application), Times.Once);
            _applicationRepositoryMock.Verify(x => x.Update(application), Times.Once);

        }

        [Fact]
        public async Task UpdateApplicationAsync_Should_Throw_When_Application_NotFound()
        {
            Guid applicationId = Guid.NewGuid();
            UpdateApplicationRequestDto update = new UpdateApplicationRequestDto
            {
                Status = Domain.Enums.Recruitment.ApplicationStatus.Screening
            };

            _applicationRepositoryMock.Setup(x => x.GetByIdAsync(applicationId, It.IsAny<CancellationToken>())).ReturnsAsync((ApplicationEntity?)null);

            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _applicationService.UpdateApplicationAsync(applicationId, update, CancellationToken.None));

            Assert.Equal("Application Not Available", exception.Message);

            _applicationRepositoryMock.Verify(x => x.GetByIdAsync(applicationId, It.IsAny<CancellationToken>()), Times.Once);
            _applicationRepositoryMock.Verify(x => x.Update(It.IsAny<ApplicationEntity>()), Times.Never);

        }

        [Fact]
        public async Task UpdateApplicationAsync_Should_Throw_When_Application_Status_NotMatch()
        {
            Guid applicationId = Guid.NewGuid();
            UpdateApplicationRequestDto update = new UpdateApplicationRequestDto
            {
                Status = Domain.Enums.Recruitment.ApplicationStatus.Screening
            };

            ApplicationEntity application = new ApplicationEntity
            {
                Id = applicationId,
                Status = Domain.Enums.Recruitment.ApplicationStatus.Screening
            };

            _applicationRepositoryMock.Setup(x => x.GetByIdAsync(applicationId, It.IsAny<CancellationToken>())).ReturnsAsync(application);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _applicationService.UpdateApplicationAsync(applicationId, update, CancellationToken.None));

            Assert.Equal($"Cannot change application status from {application.Status} to {update.Status}", exception.Message);

            _applicationRepositoryMock.Verify(x => x.GetByIdAsync(applicationId, It.IsAny<CancellationToken>()), Times.Once);
            _applicationRepositoryMock.Verify(x => x.Update(It.IsAny<ApplicationEntity>()), Times.Never);

        }

        [Fact]
        public async Task DeleteApplicationAsync_Should_Delete_Application_Successfully()
        {
            Guid applicationId = Guid.NewGuid();
            ApplicationEntity application = new ApplicationEntity
            {
                Id = applicationId,
                Status = Domain.Enums.Recruitment.ApplicationStatus.Screening
            };

            _applicationRepositoryMock.Setup(x => x.GetByIdAsync(applicationId, It.IsAny<CancellationToken>())).ReturnsAsync(application);
            _applicationRepositoryMock.Setup(x => x.Delete(application));

            var result = await _applicationService.DeleteApplicationAsync(applicationId, CancellationToken.None);

            Assert.True(result);

            _applicationRepositoryMock.Verify(x => x.GetByIdAsync(applicationId, It.IsAny<CancellationToken>()), Times.Once);
            _applicationRepositoryMock.Verify(x => x.Delete(It.IsAny<ApplicationEntity>()), Times.Once);
        }

        [Fact]
        public async Task DeleteApplicationAsync_Should_Throw_When_Application_NotFound()
        {
            Guid applicationId = Guid.NewGuid();

            _applicationRepositoryMock.Setup(x => x.GetByIdAsync(applicationId, It.IsAny<CancellationToken>())).ReturnsAsync((ApplicationEntity?)null);

            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _applicationService.DeleteApplicationAsync(applicationId, CancellationToken.None));

            Assert.Equal("Application Not Available", exception.Message);

            _applicationRepositoryMock.Verify(x => x.GetByIdAsync(applicationId, It.IsAny<CancellationToken>()), Times.Once);
            _applicationRepositoryMock.Verify(x => x.Delete(It.IsAny<ApplicationEntity>()), Times.Never);
        }

    }
}
