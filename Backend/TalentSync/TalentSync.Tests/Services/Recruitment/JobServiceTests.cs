using AutoMapper;
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
using TalentSync.Application.Services.Recruitment;
using TalentSync.Domain.Entities.Recruitment;
using TalentSync.Domain.Entities.User;
using TalentSync.Domain.Enums.Notifications;
using TalentSync.Domain.Enums.Recruitment;
using TalentSync.Infrastructure.Repositories.Recruitment;

namespace TalentSync.Tests.Services.Recruitment
{
    public class JobServiceTests
    {
        private readonly Mock<IJobRepository> _jobRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<INotificationService> _notificationServiceMock;
        private readonly Mock<ILogger<JobService>> _loggerMock;

        private readonly JobService _jobService;

        public JobServiceTests()
        {
            _jobRepositoryMock = new Mock<IJobRepository>();
            _mapperMock = new Mock<IMapper>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _notificationServiceMock = new Mock<INotificationService>();
            _loggerMock = new Mock<ILogger<JobService>>();

            _jobService = new JobService(_jobRepositoryMock.Object, _mapperMock.Object, _userRepositoryMock.Object, _notificationServiceMock.Object, _loggerMock.Object);
        }


        [Fact]
        public async Task CreateJobAsync_Should_Create_Job_Successfully()
        {
            var hrId = Guid.NewGuid();

            var request = new CreateJobDto
            {
                Title = "Title",
                Department = "Healthcare",
                Description = "Description",
                Requirements = "c#"
            };

            var user = new User
            {
                Id = hrId,
                Name = "hr",
                Email = "hr@gmail.com",
                IsDeleted = false,
                Status = Domain.Enums.User.UserStatus.Active

            };

            var mappedJob = new Job
            {
                Title = "Title",
                Department = "Healthcare",
                Description = "Description",
                Requirements = "c#",
                HRId = hrId,
                PostedDate = DateTime.UtcNow,
                Status = Domain.Enums.Recruitment.JobStatus.Open,
                UpdatedAt = DateTime.Now,
                CreatedAt = DateTime.Now,

            };

            var savedJob = new Job
            {
                Id = Guid.NewGuid(),
                Title = "Title",
                Department = "Healthcare",
                Description = "Description",
                Requirements = "c#",
                HRId = hrId,
                PostedDate = DateTime.UtcNow,
                Status = Domain.Enums.Recruitment.JobStatus.Open,
                UpdatedAt = DateTime.Now,
                CreatedAt = DateTime.Now,

            };

            var createJobNotification = new CreateNotificationDto
            {
                UserId = hrId,
                Title = savedJob.Title,
                Message = $"Your job '{savedJob.Title}' has been published successfully.",
                Category = NotificationCategory.Recruitment,
                Channel = NotificationChannel.InApp
            };

            var response = new JobResponseDto
            {
                Id = savedJob.Id,
                Title = savedJob.Title,
                Department = savedJob.Department,
                Description = savedJob.Description,
                Requirements = savedJob.Requirements,
                HRId = savedJob.HRId,
                PostedDate = savedJob.PostedDate,
                Status = savedJob.Status,
            };

            _userRepositoryMock.Setup(x => x.GetUserByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(user);
            _mapperMock.Setup(x => x.Map<Job>(It.IsAny<CreateJobDto>())).Returns(mappedJob);
            _jobRepositoryMock.Setup(x => x.AddAsync(mappedJob, It.IsAny<CancellationToken>())).ReturnsAsync(savedJob);
            _notificationServiceMock.Setup(x => x.SendAsync(createJobNotification, It.IsAny<CancellationToken>()));
            _mapperMock.Setup(x => x.Map<JobResponseDto>(It.IsAny<Job>())).Returns(response);

            var result = await _jobService.CreateJobAsync(request, hrId, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(savedJob.Id, result.Id);
            Assert.Equal(savedJob.Title, result.Title);


            _userRepositoryMock.Verify(x => x.GetUserByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once());
            _jobRepositoryMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);

        }

        [Fact]
        public async Task CreateJobAsync_Should_Throw_When_User_NotFound()
        {
            var hrId = Guid.NewGuid();
            var request = new CreateJobDto
            {
                Title = "Title",
                Department = "Healthcare",
                Description = "Description",
                Requirements = "c#"
            };

            _userRepositoryMock.Setup(x => x.GetUserByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync((User?)null);

            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _jobService.CreateJobAsync(request, hrId, CancellationToken.None));

            Assert.Equal("User not found.", exception.Message);

            _userRepositoryMock.Verify(x => x.GetUserByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);

            _jobRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Job>(), It.IsAny<CancellationToken>()), Times.Never);

            _jobRepositoryMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);

        }

        [Fact]
        public async Task CreateJobAsync_Should_Throw_When_User_Is_InActive()
        {
            var hrId = Guid.NewGuid();
            var request = new CreateJobDto
            {
                Title = "Title",
                Department = "Healthcare",
                Description = "Description",
                Requirements = "c#"
            };
            var user = new User
            {
                Id = hrId,
                Name = "hr",
                Email = "hr@gmail.com",
                IsDeleted = false,
                Status = Domain.Enums.User.UserStatus.Inactive

            };
            _userRepositoryMock.Setup(x => x.GetUserByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(user);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _jobService.CreateJobAsync(request, hrId, CancellationToken.None));

            Assert.Equal("User account is not active.", exception.Message);

            _userRepositoryMock.Verify(x => x.GetUserByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);

            _jobRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Job>(), It.IsAny<CancellationToken>()), Times.Never);

            _jobRepositoryMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);

        }

        [Fact]
        public async Task GetAllJobsAsync_Should_Get_All_Jobs_Successfully()
        {
            var paginationRequest = new PaginationRequest
            {
                PageNumber = 1,
                PageSize = 10,
            };

            var job1 = new Job
            {
                Title = "Title",
                Department = "Healthcare",
                Description = "Description",
                Requirements = "c#",
                HRId = Guid.NewGuid(),
                PostedDate = DateTime.UtcNow,
                Status = Domain.Enums.Recruitment.JobStatus.Open,
                UpdatedAt = DateTime.Now,
                CreatedAt = DateTime.Now,
            };

            List<Job> jobs = new List<Job>
            {
                job1
            };

            List<JobListDto> jobLists = new List<JobListDto>
            {
                new JobListDto
                {
                    Title = job1.Title,
                    Department= job1.Department,
                }
            };

            PaginationResponse <JobListDto> response =  new PaginationResponse<JobListDto>
            (
                pageNumber: paginationRequest.PageNumber,
                pageSize: paginationRequest.PageSize,
                totalRecords: 1,
                data: jobLists
            );

            _jobRepositoryMock.Setup(x => x.GetPagedJobsAsync(paginationRequest, It.IsAny<CancellationToken>())).ReturnsAsync(jobs);
            _jobRepositoryMock.Setup(x => x.CountAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
            _mapperMock.Setup(x => x.Map<List<JobListDto>>(It.IsAny<List<Job>>())).Returns(jobLists);

            var result = await _jobService.GetAllJobsAsync(paginationRequest, It.IsAny<CancellationToken>());

            Assert.Equal(response.Data[0].Title, result.Data[0].Title);

            _jobRepositoryMock.Verify(x => x.GetPagedJobsAsync(It.IsAny<PaginationRequest>(), It.IsAny<CancellationToken>()), Times.Once);

        }

        [Fact]
        public async Task GetJobByIdAsync_Should_Get_Job_Successfully()
        {
            var jobId = Guid.NewGuid();
            var job = new Job
            {
                Title = "Title",
                Department = "Healthcare",
                Description = "Description",
                Requirements = "c#",
                HRId = Guid.NewGuid(),
                PostedDate = DateTime.UtcNow,
                Status = Domain.Enums.Recruitment.JobStatus.Open,
                UpdatedAt = DateTime.Now,
                CreatedAt = DateTime.Now,
            };

            var jobResponse = new JobResponseDto
            {
                Title = "Title",
                Department = "Healthcare",
                Description = "Description",
                Requirements = "c#",
                HRId = Guid.NewGuid(),
                PostedDate = DateTime.UtcNow,
                Status = Domain.Enums.Recruitment.JobStatus.Open
            };

            _jobRepositoryMock.Setup(x => x.GetJobByIdAsync(jobId, It.IsAny<CancellationToken>())).ReturnsAsync(job);
            _mapperMock.Setup(x => x.Map<JobResponseDto>(job)).Returns(jobResponse);

            var result = await _jobService.GetJobByIdAsync(jobId, CancellationToken.None);

            Assert.Equal(jobResponse.Title, result.Title);
            _jobRepositoryMock.Verify(x => x.GetJobByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetJobByIdAsync_Should_Throw_When_Job_NotFound()
        {
            var jobId = Guid.NewGuid();

            _jobRepositoryMock.Setup(x => x.GetJobByIdAsync(jobId, It.IsAny<CancellationToken>())).ReturnsAsync((Job?)null);

            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _jobService.GetJobByIdAsync(jobId, It.IsAny<CancellationToken>()));

            Assert.Equal("Job Not Found.", exception.Message);
            _jobRepositoryMock.Verify(x => x.GetJobByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(x => x.Map<JobResponseDto>(It.IsAny<Job>()), Times.Never);
        }

        [Fact]
        public async Task UpdateJobAsync_Should_Update_Job_Successfully()
        {
            var jobId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            UpdateJobRequestDto updateJob = new UpdateJobRequestDto
            {
                Title = "Title",
                Department = "Department",
                Description = "Description",
                Requirements = "Requirements",
                Status = JobStatus.Open
            };

            Job job = new Job
            {
                Id = jobId,
                Title = "Title",
                Department = "Department",
                Description = "Description",
                Requirements = "Requirements",
                Status = JobStatus.Open,
                HRId = userId,
            };

            JobResponseDto response = new JobResponseDto
            {
                Title = job.Title,
                Department = job.Department,
                Description = job.Description,
                Id = jobId,
                HRId = job.HRId,
            };

            _jobRepositoryMock.Setup(x => x.GetJobByIdAsync(jobId, It.IsAny<CancellationToken>())).ReturnsAsync(job);
            _mapperMock.Setup(x => x.Map(updateJob, job)).Returns(job);
            _jobRepositoryMock.Setup(x => x.UpdateJob(job));
            _mapperMock.Setup(x => x.Map<JobResponseDto>(job)).Returns(response);

            var result = await _jobService.UpdateJobAsync(jobId, userId, updateJob, CancellationToken.None);

            Assert.Equal(job.Title, result.Title);
            Assert.Equal(job.HRId, result.HRId);

            _jobRepositoryMock.Verify(x => x.GetJobByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            _jobRepositoryMock.Verify(x => x.UpdateJob(It.IsAny<Job>()), Times.Once);
        }

        [Fact]
        public async Task UpdateJobAsync_Should_Throw_When_Job_NotFound()
        {
            var jobId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            UpdateJobRequestDto updateJob = new UpdateJobRequestDto
            {
                Title = "Title",
                Department = "Department",
                Description = "Description",
                Requirements = "Requirements",
                Status = JobStatus.Open
            };

            _jobRepositoryMock.Setup(x => x.GetJobByIdAsync(jobId, It.IsAny<CancellationToken>())).ReturnsAsync((Job?)null);

            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _jobService.UpdateJobAsync(jobId, userId, updateJob, CancellationToken.None));

            Assert.Equal("Job Not Found.", exception.Message);

            _jobRepositoryMock.Verify(x => x.GetJobByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            _jobRepositoryMock.Verify(x => x.UpdateJob(It.IsAny<Job>()), Times.Never);
        }

        [Fact]
        public async Task UpdateJobAsync_Should_Throw_When_HrId_Not_Match()
        {
            var jobId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            UpdateJobRequestDto updateJob = new UpdateJobRequestDto
            {
                Title = "Title",
                Department = "Department",
                Description = "Description",
                Requirements = "Requirements",
                Status = JobStatus.Open
            };

            Job job = new Job
            {
                Id = jobId,
                Title = "Title",
                Department = "Department",
                Description = "Description",
                Requirements = "Requirements",
                Status = JobStatus.Open,
                HRId = Guid.NewGuid(),
            };

            _jobRepositoryMock.Setup(x => x.GetJobByIdAsync(jobId, It.IsAny<CancellationToken>())).ReturnsAsync(job);

            var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _jobService.UpdateJobAsync(jobId, userId, updateJob, CancellationToken.None));

            Assert.Equal("You can only update your own jobs.", exception.Message);

            _jobRepositoryMock.Verify(x => x.GetJobByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            _jobRepositoryMock.Verify(x => x.UpdateJob(It.IsAny<Job>()), Times.Never);
        }


        [Fact]
        public async Task DeleteJobAsync_Should_Delete_Job_Successfully()
        {
            var jobId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            Job job = new Job
            {
                Id = jobId,
                Title = "Title",
                Department = "Department",
                Description = "Description",
                Requirements = "Requirements",
                Status = JobStatus.Open,
                HRId = userId,
            };

            _jobRepositoryMock.Setup(x => x.GetJobByIdAsync(jobId, It.IsAny<CancellationToken>())).ReturnsAsync(job);
            _jobRepositoryMock.Setup(x => x.DeleteJob(job));

            var result = await _jobService.DeleteJobAsync(jobId, userId, CancellationToken.None);

            Assert.True(result);

            _jobRepositoryMock.Verify(x => x.GetJobByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            _jobRepositoryMock.Verify(x => x.DeleteJob(It.IsAny<Job>()), Times.Once);
        }

        [Fact]
        public async Task DeleteJobAsync_Should_Throw_When_Job_NotFound()
        {
            var jobId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            _jobRepositoryMock.Setup(x => x.GetJobByIdAsync(jobId, It.IsAny<CancellationToken>())).ReturnsAsync((Job?)null);

            var result = await Assert.ThrowsAsync<KeyNotFoundException>(() => _jobService.DeleteJobAsync(jobId, userId, CancellationToken.None));

            Assert.Equal("Job Not Found.", result.Message);

            _jobRepositoryMock.Verify(x => x.GetJobByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            _jobRepositoryMock.Verify(x => x.DeleteJob(It.IsAny<Job>()), Times.Never);
        }

        [Fact]
        public async Task DeleteJobAsync_Should_Throw_When_HrId_Not_Match()
        {
            var jobId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            Job job = new Job
            {
                Id = jobId,
                Title = "Title",
                Department = "Department",
                Description = "Description",
                Requirements = "Requirements",
                Status = JobStatus.Open,
                HRId = Guid.NewGuid(),
            };

            _jobRepositoryMock.Setup(x => x.GetJobByIdAsync(jobId, It.IsAny<CancellationToken>())).ReturnsAsync(job);

            var result = await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _jobService.DeleteJobAsync(jobId, userId, CancellationToken.None));

            Assert.Equal("You can only delete your own jobs.", result.Message);

            _jobRepositoryMock.Verify(x => x.GetJobByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            _jobRepositoryMock.Verify(x => x.DeleteJob(It.IsAny<Job>()), Times.Never);
        }
    }
}
