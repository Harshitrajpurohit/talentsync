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
using TalentSync.Domain.Entities.Recruitment;
using TalentSync.Domain.Enums.Recruitment;
using static System.Net.Mime.MediaTypeNames;

namespace TalentSync.Tests.Services.Recruitment
{
    public class ScreeningServiceTests
    {

        private readonly Mock<IScreeningRepository> _screeningRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IApplicationRepository> _applicationRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<INotificationService> _notificationServiceMock;
        private readonly Mock<ILogger<ScreeningService>> _loggerMock;

        private readonly ScreeningService _screeningService;

        public ScreeningServiceTests()
        {
            _screeningRepositoryMock = new Mock<IScreeningRepository>();
            _mapperMock = new Mock<IMapper>();
            _applicationRepositoryMock = new Mock<IApplicationRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _notificationServiceMock = new Mock<INotificationService>();
            _loggerMock = new Mock<ILogger<ScreeningService>>();

            _screeningService = new ScreeningService(
                _screeningRepositoryMock.Object,
                _mapperMock.Object,
                _applicationRepositoryMock.Object,
                _unitOfWorkMock.Object,
                _notificationServiceMock.Object,
                _loggerMock.Object
                );

        }

        [Fact]
        public async Task CreateScreeningAsync_Should_Create_Screening_With_Result_Pending_Successfully()
        {
            
            CreateScreeningRequestDto createScreeningRequest = new CreateScreeningRequestDto
            {
                ApplicationId = Guid.NewGuid(),
                Result = ScreeningResult.Pending,
                Notes = "notes"
            };
            Guid screenedById = Guid.NewGuid();

            Job job = new Job
            {
                Id = Guid.NewGuid(),
                Title = "title",
            };

            ApplicationEntity application = new ApplicationEntity{
                Id = createScreeningRequest.ApplicationId,
                CandidateId = Guid.NewGuid(),
                JobId = job.Id,
                Job = job
            };

            Screening screening = new Screening
            {
                ApplicationId = createScreeningRequest.ApplicationId,
                Result = ScreeningResult.Pending,
                Notes = "notes",
                ScreenedById = screenedById,
            };

            ScreeningResponseDto screeningResponse = new ScreeningResponseDto
            {
                ApplicationId = screening.ApplicationId,
                Result = screening.Result,
                Notes = screening.Notes,
                ScreenedById = screening.ScreenedById,
            };

            _applicationRepositoryMock.Setup(x => x.GetByIdAsync(createScreeningRequest.ApplicationId, It.IsAny<CancellationToken>())).ReturnsAsync(application);
            _screeningRepositoryMock.Setup(x => x.ExistsByApplicationIdAsync(createScreeningRequest.ApplicationId, It.IsAny<CancellationToken>())).ReturnsAsync(false);
            _mapperMock.Setup(x => x.Map<Screening>(createScreeningRequest)).Returns(screening);
            _unitOfWorkMock.Setup(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            _screeningRepositoryMock.Setup(x => x.AddAsync(screening, It.IsAny<CancellationToken>())).ReturnsAsync(screening);
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()));
            _unitOfWorkMock.Setup(x => x.CommitTransactionAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            _notificationServiceMock.Setup(x => x.SendAsync(It.IsAny<CreateNotificationDto>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            _mapperMock.Setup(x => x.Map<ScreeningResponseDto>(screening)).Returns(screeningResponse);
            
            var result = await _screeningService.CreateScreeningAsync(createScreeningRequest, screenedById, CancellationToken.None);

            Assert.Equal(screenedById, result.ScreenedById);
            Assert.Equal(ScreeningResult.Pending, result.Result);


            _applicationRepositoryMock.Verify(x => x.GetByIdAsync(createScreeningRequest.ApplicationId, It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
            _screeningRepositoryMock.Verify(x => x.AddAsync(screening, It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.CommitTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.RollbackTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
            _notificationServiceMock.Verify(x => x.SendAsync(It.IsAny<CreateNotificationDto>(), It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(x => x.Map<ScreeningResponseDto>(screening), Times.Once);
        }

        [Fact]
        public async Task CreateScreeningAsync_Should_Create_Screening_With_Result_Pass_Successfully()
        {

            CreateScreeningRequestDto createScreeningRequest = new CreateScreeningRequestDto
            {
                ApplicationId = Guid.NewGuid(),
                Result = ScreeningResult.Pass,
                Notes = "notes"
            };
            Guid screenedById = Guid.NewGuid();

            Job job = new Job
            {
                Id = Guid.NewGuid(),
                Title = "title",
            };

            ApplicationEntity application = new ApplicationEntity
            {
                Id = createScreeningRequest.ApplicationId,
                CandidateId = Guid.NewGuid(),
                JobId = job.Id,
                Job = job,
                Status = ApplicationStatus.Submitted,
            };

            Screening screening = new Screening
            {
                ApplicationId = createScreeningRequest.ApplicationId,
                Result = createScreeningRequest.Result,
                Notes = createScreeningRequest.Notes,
                ScreenedById = screenedById,
            };

            ScreeningResponseDto screeningResponse = new ScreeningResponseDto
            {
                ApplicationId = screening.ApplicationId,
                Result = screening.Result,
                Notes = screening.Notes,
                ScreenedById = screening.ScreenedById,
            };

            _applicationRepositoryMock.Setup(x => x.GetByIdAsync(createScreeningRequest.ApplicationId, It.IsAny<CancellationToken>())).ReturnsAsync(application);
            _screeningRepositoryMock.Setup(x => x.ExistsByApplicationIdAsync(createScreeningRequest.ApplicationId, It.IsAny<CancellationToken>())).ReturnsAsync(false);
            _mapperMock.Setup(x => x.Map<Screening>(createScreeningRequest)).Returns(screening);
            _unitOfWorkMock.Setup(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            _screeningRepositoryMock.Setup(x => x.AddAsync(screening, It.IsAny<CancellationToken>())).ReturnsAsync(screening);
            _applicationRepositoryMock.Setup(x => x.Update(application));
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()));
            _unitOfWorkMock.Setup(x => x.CommitTransactionAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            _notificationServiceMock.Setup(x => x.SendAsync(It.IsAny<CreateNotificationDto>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            _mapperMock.Setup(x => x.Map<ScreeningResponseDto>(screening)).Returns(screeningResponse);

            var result = await _screeningService.CreateScreeningAsync(createScreeningRequest, screenedById, CancellationToken.None);

            Assert.Equal(screenedById, result.ScreenedById);
            Assert.Equal(ScreeningResult.Pass, result.Result);
            Assert.Equal(ApplicationStatus.Screening, application.Status);

            _applicationRepositoryMock.Verify(x => x.GetByIdAsync(createScreeningRequest.ApplicationId, It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
            _screeningRepositoryMock.Verify(x => x.AddAsync(screening, It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.CommitTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.RollbackTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
            _notificationServiceMock.Verify(x => x.SendAsync(It.IsAny<CreateNotificationDto>(), It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(x => x.Map<ScreeningResponseDto>(screening), Times.Once);
        }

        [Fact]
        public async Task CreateScreeningAsync_Should_Create_Screening_With_Result_Fail_Successfully()
        {

            CreateScreeningRequestDto createScreeningRequest = new CreateScreeningRequestDto
            {
                ApplicationId = Guid.NewGuid(),
                Result = ScreeningResult.Fail,
                Notes = "notes"
            };
            Guid screenedById = Guid.NewGuid();

            Job job = new Job
            {
                Id = Guid.NewGuid(),
                Title = "title",
            };

            ApplicationEntity application = new ApplicationEntity
            {
                Id = createScreeningRequest.ApplicationId,
                CandidateId = Guid.NewGuid(),
                JobId = job.Id,
                Job = job,
                Status = ApplicationStatus.Submitted,
            };

            Screening screening = new Screening
            {
                ApplicationId = createScreeningRequest.ApplicationId,
                Result = createScreeningRequest.Result,
                Notes = createScreeningRequest.Notes,
                ScreenedById = screenedById,
            };

            ScreeningResponseDto screeningResponse = new ScreeningResponseDto
            {
                ApplicationId = screening.ApplicationId,
                Result = screening.Result,
                Notes = screening.Notes,
                ScreenedById = screening.ScreenedById,
            };

            _applicationRepositoryMock.Setup(x => x.GetByIdAsync(createScreeningRequest.ApplicationId, It.IsAny<CancellationToken>())).ReturnsAsync(application);
            _screeningRepositoryMock.Setup(x => x.ExistsByApplicationIdAsync(createScreeningRequest.ApplicationId, It.IsAny<CancellationToken>())).ReturnsAsync(false);
            _mapperMock.Setup(x => x.Map<Screening>(createScreeningRequest)).Returns(screening);
            _unitOfWorkMock.Setup(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            _screeningRepositoryMock.Setup(x => x.AddAsync(screening, It.IsAny<CancellationToken>())).ReturnsAsync(screening);
            _applicationRepositoryMock.Setup(x => x.Update(application));
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()));
            _unitOfWorkMock.Setup(x => x.CommitTransactionAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            _notificationServiceMock.Setup(x => x.SendAsync(It.IsAny<CreateNotificationDto>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            _mapperMock.Setup(x => x.Map<ScreeningResponseDto>(screening)).Returns(screeningResponse);

            var result = await _screeningService.CreateScreeningAsync(createScreeningRequest, screenedById, CancellationToken.None);

            Assert.Equal(screenedById, result.ScreenedById);
            Assert.Equal(ScreeningResult.Fail, result.Result);
            Assert.Equal(ApplicationStatus.Rejected, application.Status);


            _applicationRepositoryMock.Verify(x => x.GetByIdAsync(createScreeningRequest.ApplicationId, It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
            _screeningRepositoryMock.Verify(x => x.AddAsync(screening, It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.CommitTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.RollbackTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
            _notificationServiceMock.Verify(x => x.SendAsync(It.IsAny<CreateNotificationDto>(), It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(x => x.Map<ScreeningResponseDto>(screening), Times.Once);
        }

        [Fact]
        public async Task CreateScreeningAsync_Should_Throw_When_Application_NotFound()
        {
            CreateScreeningRequestDto createScreeningRequest = new CreateScreeningRequestDto
            {
                ApplicationId = Guid.NewGuid(),
                Result = ScreeningResult.Fail,
                Notes = "notes"
            };
            Guid screenedById = Guid.NewGuid();

            _applicationRepositoryMock.Setup(x => x.GetByIdAsync(createScreeningRequest.ApplicationId, It.IsAny<CancellationToken>())).ReturnsAsync((ApplicationEntity?)null);

            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _screeningService.CreateScreeningAsync(createScreeningRequest, screenedById, CancellationToken.None));

            Assert.Equal($"Application not found.", exception.Message);

            _applicationRepositoryMock.Verify(x => x.GetByIdAsync(createScreeningRequest.ApplicationId, It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
            _screeningRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Screening>(), It.IsAny<CancellationToken>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.CommitTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.RollbackTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
            _notificationServiceMock.Verify(x => x.SendAsync(It.IsAny<CreateNotificationDto>(), It.IsAny<CancellationToken>()), Times.Never);
            _mapperMock.Verify(x => x.Map<ScreeningResponseDto>(It.IsAny<Screening>()), Times.Never);

        }

        [Fact]
        public async Task CreateScreeningAsync_Should_Throw_When_Application_Status_NotValid()
        {
            CreateScreeningRequestDto createScreeningRequest = new CreateScreeningRequestDto
            {
                ApplicationId = Guid.NewGuid(),
                Result = ScreeningResult.Fail,
                Notes = "notes"
            };
            Guid screenedById = Guid.NewGuid();

            Job job = new Job
            {
                Id = Guid.NewGuid(),
                Title = "title",
            };

            ApplicationEntity application = new ApplicationEntity
            {
                Id = createScreeningRequest.ApplicationId,
                CandidateId = Guid.NewGuid(),
                JobId = job.Id,
                Job = job,
                Status = ApplicationStatus.Selected,
            };

            _applicationRepositoryMock.Setup(x => x.GetByIdAsync(createScreeningRequest.ApplicationId, It.IsAny<CancellationToken>())).ReturnsAsync(application);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _screeningService.CreateScreeningAsync(createScreeningRequest, screenedById, CancellationToken.None));

            Assert.Equal($"Cannot screen an application with status '{application.Status}'.", exception.Message);

            _applicationRepositoryMock.Verify(x => x.GetByIdAsync(createScreeningRequest.ApplicationId, It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
            _screeningRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Screening>(), It.IsAny<CancellationToken>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.CommitTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.RollbackTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
            _notificationServiceMock.Verify(x => x.SendAsync(It.IsAny<CreateNotificationDto>(), It.IsAny<CancellationToken>()), Times.Never);
            _mapperMock.Verify(x => x.Map<ScreeningResponseDto>(It.IsAny<Screening>()), Times.Never);
        }

        [Fact]
        public async Task CreateScreeningAsync_Should_Throw_When_Screening_Already_Created()
        {
            CreateScreeningRequestDto createScreeningRequest = new CreateScreeningRequestDto
            {
                ApplicationId = Guid.NewGuid(),
                Result = ScreeningResult.Fail,
                Notes = "notes"
            };
            Guid screenedById = Guid.NewGuid();

            Job job = new Job
            {
                Id = Guid.NewGuid(),
                Title = "title",
            };

            ApplicationEntity application = new ApplicationEntity
            {
                Id = createScreeningRequest.ApplicationId,
                CandidateId = Guid.NewGuid(),
                JobId = job.Id,
                Job = job,
                Status = ApplicationStatus.Submitted,
            };

            _applicationRepositoryMock.Setup(x => x.GetByIdAsync(createScreeningRequest.ApplicationId, It.IsAny<CancellationToken>())).ReturnsAsync(application);
            _screeningRepositoryMock.Setup(x => x.ExistsByApplicationIdAsync(createScreeningRequest.ApplicationId, It.IsAny<CancellationToken>())).ReturnsAsync(true);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _screeningService.CreateScreeningAsync(createScreeningRequest, screenedById, CancellationToken.None));

            Assert.Equal("Application has already been screened.", exception.Message);

            _applicationRepositoryMock.Verify(x => x.GetByIdAsync(createScreeningRequest.ApplicationId, It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
            _screeningRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Screening>(), It.IsAny<CancellationToken>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.CommitTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.RollbackTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
            _notificationServiceMock.Verify(x => x.SendAsync(It.IsAny<CreateNotificationDto>(), It.IsAny<CancellationToken>()), Times.Never);
            _mapperMock.Verify(x => x.Map<ScreeningResponseDto>(It.IsAny<Screening>()), Times.Never);

        }

        [Fact]
        public async Task CreateScreeningAsync_Should_Throw_When_Screening_NotAdded()
        {
            CreateScreeningRequestDto createScreeningRequest = new CreateScreeningRequestDto
            {
                ApplicationId = Guid.NewGuid(),
                Result = ScreeningResult.Fail,
                Notes = "notes"
            };
            Guid screenedById = Guid.NewGuid();

            Job job = new Job
            {
                Id = Guid.NewGuid(),
                Title = "title",
            };

            ApplicationEntity application = new ApplicationEntity
            {
                Id = createScreeningRequest.ApplicationId,
                CandidateId = Guid.NewGuid(),
                JobId = job.Id,
                Job = job,
                Status = ApplicationStatus.Submitted,
            };

            Screening screening = new Screening
            {
                ApplicationId = createScreeningRequest.ApplicationId,
                Result = createScreeningRequest.Result,
                Notes = createScreeningRequest.Notes,
                ScreenedById = screenedById,
            };


            _applicationRepositoryMock.Setup(x => x.GetByIdAsync(createScreeningRequest.ApplicationId, It.IsAny<CancellationToken>())).ReturnsAsync(application);
            _screeningRepositoryMock.Setup(x => x.ExistsByApplicationIdAsync(createScreeningRequest.ApplicationId, It.IsAny<CancellationToken>())).ReturnsAsync(false);
            _mapperMock.Setup(x => x.Map<Screening>(createScreeningRequest)).Returns(screening);
            _unitOfWorkMock.Setup(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            _screeningRepositoryMock.Setup(x => x.AddAsync(screening, It.IsAny<CancellationToken>())).Throws<InvalidOperationException>();

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _screeningService.CreateScreeningAsync(createScreeningRequest, screenedById, CancellationToken.None));


            _applicationRepositoryMock.Verify(x => x.GetByIdAsync(createScreeningRequest.ApplicationId, It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
            _screeningRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Screening>(), It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.CommitTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.RollbackTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);

            _notificationServiceMock.Verify(x => x.SendAsync(It.IsAny<CreateNotificationDto>(), It.IsAny<CancellationToken>()), Times.Never);
            _mapperMock.Verify(x => x.Map<ScreeningResponseDto>(It.IsAny<Screening>()), Times.Never);
        }

        [Fact]
        public async Task GetByIdAsync_Should_Get_Successfully()
        {
            Guid id = Guid.NewGuid();

            Screening screening = new Screening
            {
                Id = id,
                Result = ScreeningResult.Pending,
                ApplicationId = Guid.NewGuid()
            };

            ScreeningResponseDto screeningResponse = new ScreeningResponseDto
            {
                Id = id,
                Result = ScreeningResult.Pending,
                ApplicationId = Guid.NewGuid()
            };

            _screeningRepositoryMock.Setup(x => x.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(screening);
            _mapperMock.Setup(x => x.Map<ScreeningResponseDto>(screening)).Returns(screeningResponse);

            var result = await _screeningService.GetByIdAsync(id, CancellationToken.None);

            Assert.Equal(id, result.Id);

            _screeningRepositoryMock.Verify(x => x.GetByIdAsync(id, It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(x => x.Map<ScreeningResponseDto>(It.IsAny<Screening>()), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_Should_Throw_When_Screening_NotFound()
        {
            Guid id = Guid.NewGuid();

            _screeningRepositoryMock.Setup(x => x.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync((Screening?)null);
            
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _screeningService.GetByIdAsync(id, CancellationToken.None));

            Assert.Equal("Screening Not Found", exception.Message);

            _screeningRepositoryMock.Verify(x => x.GetByIdAsync(id, It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(x => x.Map<ScreeningResponseDto>(It.IsAny<Screening>()), Times.Never);
        }

        [Fact]
        public async Task GetByApplicationIdAsync_Should_Get_Successfully()
        {
            Guid applicationId = Guid.NewGuid();

            Screening screening = new Screening
            {
                Id = Guid.NewGuid(),
                Result = ScreeningResult.Pending,
                ApplicationId = applicationId
            };

            ScreeningResponseDto screeningResponse = new ScreeningResponseDto
            {
                Id = screening.Id,
                Result = ScreeningResult.Pending,
                ApplicationId = screening.ApplicationId
            };

            _screeningRepositoryMock.Setup(x => x.GetByApplicationIdAsync(applicationId, It.IsAny<CancellationToken>())).ReturnsAsync(screening);
            _mapperMock.Setup(x => x.Map<ScreeningResponseDto>(screening)).Returns(screeningResponse);

            var result = await _screeningService.GetByApplicationIdAsync(applicationId, It.IsAny<Guid>(), CancellationToken.None);

            Assert.Equal(applicationId, result.ApplicationId);

            _screeningRepositoryMock.Verify(x => x.GetByApplicationIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(x => x.Map<ScreeningResponseDto>(It.IsAny<Screening>()), Times.Once);
        }

        [Fact]
        public async Task GetByApplicationIdAsync_Should_Throw_When_Screening_NotFound()
        {
            Guid applicationId = Guid.NewGuid();


            _screeningRepositoryMock.Setup(x => x.GetByApplicationIdAsync(applicationId, It.IsAny<CancellationToken>())).ReturnsAsync((Screening?)null);

            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _screeningService.GetByApplicationIdAsync(applicationId, It.IsAny<Guid>(), CancellationToken.None));

            Assert.Equal("Screening Not Found with Application", exception.Message);

            _screeningRepositoryMock.Verify(x => x.GetByApplicationIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(x => x.Map<ScreeningResponseDto>(It.IsAny<Screening>()), Times.Never);
        }

        [Fact]
        public async Task UpdateScreeningAsync_Should_Update_Screening_Result_ToPass_Successfully()
        {
            Guid id = Guid.NewGuid();

            UpdateScreeningRequestDto updateScreeningRequest = new UpdateScreeningRequestDto
            {
                Result = ScreeningResult.Pass,
                Notes = "notes"
            };

            Screening screening = new Screening
            {
                Id = id,
                ApplicationId = Guid.NewGuid(),
                Result = ScreeningResult.Pending,
            };

            Job job = new Job
            {
                Id= Guid.NewGuid(),
                Title = "Test",
            };

            ApplicationEntity application = new ApplicationEntity
            {
                Id = screening.ApplicationId,
                Status = ApplicationStatus.Screening,
                JobId = job.Id,
                Job = job,
                
            };

            ScreeningResponseDto screeningResponse = new ScreeningResponseDto
            {
                Id = id,
                ApplicationId = screening.ApplicationId,
                Result = ScreeningResult.Pass
            };

            _screeningRepositoryMock.Setup(x => x.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(screening);
            _applicationRepositoryMock.Setup(x => x.GetByIdAsync(screening.ApplicationId, It.IsAny<CancellationToken>())).ReturnsAsync(application);
            _unitOfWorkMock.Setup(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            _mapperMock.Setup(x => x.Map(updateScreeningRequest, screening));
            _screeningRepositoryMock.Setup(x => x.Update(screening));
            _applicationRepositoryMock.Setup(x => x.Update(application));
            _unitOfWorkMock.Setup(x => x.CommitTransactionAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            _notificationServiceMock.Setup(x => x.SendAsync(It.IsAny<CreateNotificationDto>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            _mapperMock.Setup(x => x.Map<ScreeningResponseDto>(screening)).Returns(screeningResponse);

            var result = await _screeningService.UpdateScreeningAsync(id, updateScreeningRequest, CancellationToken.None);

            Assert.Equal(id, result.Id);
            Assert.Equal(ScreeningResult.Pass, result.Result);
            Assert.Equal(ApplicationStatus.Screening, application.Status);


            _screeningRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            _applicationRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
            _screeningRepositoryMock.Verify(x => x.Update(It.IsAny<Screening>()), Times.Once);
            _applicationRepositoryMock.Verify(x => x.Update(It.IsAny<ApplicationEntity>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.CommitTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.RollbackTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
            _notificationServiceMock.Verify(x => x.SendAsync(It.IsAny<CreateNotificationDto>(), It.IsAny<CancellationToken>()), Times.Once);  

        }

        [Fact]
        public async Task UpdateScreeningAsync_Should_Update_Screening_Result_ToFail_Successfully()
        {
            Guid id = Guid.NewGuid();

            UpdateScreeningRequestDto updateScreeningRequest = new UpdateScreeningRequestDto
            {
                Result = ScreeningResult.Fail,
                Notes = "notes"
            };

            Screening screening = new Screening
            {
                Id = id,
                ApplicationId = Guid.NewGuid(),
                Result = ScreeningResult.Pending,
            };

            Job job = new Job
            {
                Id = Guid.NewGuid(),
                Title = "Test",
            };

            ApplicationEntity application = new ApplicationEntity
            {
                Id = screening.ApplicationId,
                Status = ApplicationStatus.Screening,
                JobId = job.Id,
                Job = job,

            };

            ScreeningResponseDto screeningResponse = new ScreeningResponseDto
            {
                Id = id,
                ApplicationId = screening.ApplicationId,
                Result = ScreeningResult.Fail
            };

            _screeningRepositoryMock.Setup(x => x.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(screening);
            _applicationRepositoryMock.Setup(x => x.GetByIdAsync(screening.ApplicationId, It.IsAny<CancellationToken>())).ReturnsAsync(application);
            _unitOfWorkMock.Setup(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            _mapperMock.Setup(x => x.Map(updateScreeningRequest, screening));
            _screeningRepositoryMock.Setup(x => x.Update(screening));
            _applicationRepositoryMock.Setup(x => x.Update(application));
            _unitOfWorkMock.Setup(x => x.CommitTransactionAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            _notificationServiceMock.Setup(x => x.SendAsync(It.IsAny<CreateNotificationDto>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            _mapperMock.Setup(x => x.Map<ScreeningResponseDto>(screening)).Returns(screeningResponse);

            var result = await _screeningService.UpdateScreeningAsync(id, updateScreeningRequest, CancellationToken.None);

            Assert.Equal(id, result.Id);
            Assert.Equal(ScreeningResult.Fail, result.Result);
            Assert.Equal(ApplicationStatus.Rejected, application.Status);


            _screeningRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            _applicationRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
            _screeningRepositoryMock.Verify(x => x.Update(It.IsAny<Screening>()), Times.Once);
            _applicationRepositoryMock.Verify(x => x.Update(It.IsAny<ApplicationEntity>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.CommitTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.RollbackTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
            _notificationServiceMock.Verify(x => x.SendAsync(It.IsAny<CreateNotificationDto>(), It.IsAny<CancellationToken>()), Times.Once);

        }

        [Fact]
        public async Task UpdateScreeningAsync_Should_Throw_When_Screening_Result_IsPending()
        {
            Guid id = Guid.NewGuid();

            UpdateScreeningRequestDto updateScreeningRequest = new UpdateScreeningRequestDto
            {
                Result = ScreeningResult.Pending,
                Notes = "notes"
            };

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _screeningService.UpdateScreeningAsync(id, updateScreeningRequest, CancellationToken.None));

            Assert.Equal("screenings cannot be reverted to Pending.", exception.Message);

            _screeningRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
            _applicationRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
            _screeningRepositoryMock.Verify(x => x.Update(It.IsAny<Screening>()), Times.Never);
            _applicationRepositoryMock.Verify(x => x.Update(It.IsAny<ApplicationEntity>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.CommitTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.RollbackTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
            _notificationServiceMock.Verify(x => x.SendAsync(It.IsAny<CreateNotificationDto>(), It.IsAny<CancellationToken>()), Times.Never);

        }

        [Fact]
        public async Task UpdateScreeningAsync_Should_Throw_When_Screening_NotFound()
        {
            Guid id = Guid.NewGuid();

            UpdateScreeningRequestDto updateScreeningRequest = new UpdateScreeningRequestDto
            {
                Result = ScreeningResult.Fail,
                Notes = "notes"
            };


            _screeningRepositoryMock.Setup(x => x.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync((Screening?)null);


            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _screeningService.UpdateScreeningAsync(id, updateScreeningRequest, CancellationToken.None));

            Assert.Equal("Screening Not Found", exception.Message);

            _screeningRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            _applicationRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
            _screeningRepositoryMock.Verify(x => x.Update(It.IsAny<Screening>()), Times.Never);
            _applicationRepositoryMock.Verify(x => x.Update(It.IsAny<ApplicationEntity>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.CommitTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.RollbackTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
            _notificationServiceMock.Verify(x => x.SendAsync(It.IsAny<CreateNotificationDto>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task UpdateScreeningAsync_Should_Throw_When_Update_And_Existing_Screening_Matched()
        {
            Guid id = Guid.NewGuid();

            UpdateScreeningRequestDto updateScreeningRequest = new UpdateScreeningRequestDto
            {
                Result = ScreeningResult.Fail,
                Notes = "notes"
            };

            Screening screening = new Screening
            {
                Id = id,
                ApplicationId = Guid.NewGuid(),
                Result = ScreeningResult.Fail,
            };


            _screeningRepositoryMock.Setup(x => x.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(screening);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _screeningService.UpdateScreeningAsync(id, updateScreeningRequest, CancellationToken.None));

            Assert.Equal("Screening already has this result.", exception.Message);

            _screeningRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            _applicationRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
            _screeningRepositoryMock.Verify(x => x.Update(It.IsAny<Screening>()), Times.Never);
            _applicationRepositoryMock.Verify(x => x.Update(It.IsAny<ApplicationEntity>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.CommitTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.RollbackTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
            _notificationServiceMock.Verify(x => x.SendAsync(It.IsAny<CreateNotificationDto>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task UpdateScreeningAsync_Should_Throw_When_Application_NotFound()
        {
            Guid id = Guid.NewGuid();

            UpdateScreeningRequestDto updateScreeningRequest = new UpdateScreeningRequestDto
            {
                Result = ScreeningResult.Fail,
                Notes = "notes"
            };

            Screening screening = new Screening
            {
                Id = id,
                ApplicationId = Guid.NewGuid(),
                Result = ScreeningResult.Pending,
            };

            _screeningRepositoryMock.Setup(x => x.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(screening);
            _applicationRepositoryMock.Setup(x => x.GetByIdAsync(screening.ApplicationId, It.IsAny<CancellationToken>())).ReturnsAsync((ApplicationEntity?)null);

            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _screeningService.UpdateScreeningAsync(id, updateScreeningRequest, CancellationToken.None));

            Assert.Equal("Application not found.", exception.Message);

            _screeningRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            _applicationRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
            _screeningRepositoryMock.Verify(x => x.Update(It.IsAny<Screening>()), Times.Never);
            _applicationRepositoryMock.Verify(x => x.Update(It.IsAny<ApplicationEntity>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.CommitTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.RollbackTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
            _notificationServiceMock.Verify(x => x.SendAsync(It.IsAny<CreateNotificationDto>(), It.IsAny<CancellationToken>()), Times.Never);

        }

        [Fact]
        public async Task UpdateScreeningAsync_Should_Throw_When_Application_Status_NotValid()
        {
            Guid id = Guid.NewGuid();

            UpdateScreeningRequestDto updateScreeningRequest = new UpdateScreeningRequestDto
            {
                Result = ScreeningResult.Fail,
                Notes = "notes"
            };

            Screening screening = new Screening
            {
                Id = id,
                ApplicationId = Guid.NewGuid(),
                Result = ScreeningResult.Pending,
            };

            Job job = new Job
            {
                Id = Guid.NewGuid(),
                Title = "Test",
            };

            ApplicationEntity application = new ApplicationEntity
            {
                Id = screening.ApplicationId,
                Status = ApplicationStatus.Rejected,
                JobId = job.Id,
                Job = job,

            };

            _screeningRepositoryMock.Setup(x => x.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(screening);
            _applicationRepositoryMock.Setup(x => x.GetByIdAsync(screening.ApplicationId, It.IsAny<CancellationToken>())).ReturnsAsync(application);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _screeningService.UpdateScreeningAsync(id, updateScreeningRequest, CancellationToken.None));

            Assert.Equal($"Cannot update screening because application is in '{application.Status}' state.", exception.Message);

            _screeningRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            _applicationRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
            _screeningRepositoryMock.Verify(x => x.Update(It.IsAny<Screening>()), Times.Never);
            _applicationRepositoryMock.Verify(x => x.Update(It.IsAny<ApplicationEntity>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.CommitTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.RollbackTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
            _notificationServiceMock.Verify(x => x.SendAsync(It.IsAny<CreateNotificationDto>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task UpdateScreeningAsync_Should_Throw_When_Screening_Update_Failed()
        {
            Guid id = Guid.NewGuid();

            UpdateScreeningRequestDto updateScreeningRequest = new UpdateScreeningRequestDto
            {
                Result = ScreeningResult.Fail,
                Notes = "notes"
            };

            Screening screening = new Screening
            {
                Id = id,
                ApplicationId = Guid.NewGuid(),
                Result = ScreeningResult.Pending,
            };

            Job job = new Job
            {
                Id = Guid.NewGuid(),
                Title = "Test",
            };

            ApplicationEntity application = new ApplicationEntity
            {
                Id = screening.ApplicationId,
                Status = ApplicationStatus.Screening,
                JobId = job.Id,
                Job = job,

            };

            _screeningRepositoryMock.Setup(x => x.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(screening);
            _applicationRepositoryMock.Setup(x => x.GetByIdAsync(screening.ApplicationId, It.IsAny<CancellationToken>())).ReturnsAsync(application);
            _unitOfWorkMock.Setup(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            _mapperMock.Setup(x => x.Map(updateScreeningRequest, screening));
            _screeningRepositoryMock.Setup(x => x.Update(screening)).Throws<InvalidOperationException>();

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _screeningService.UpdateScreeningAsync(id, updateScreeningRequest, CancellationToken.None));

            _screeningRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            _applicationRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
            _screeningRepositoryMock.Verify(x => x.Update(It.IsAny<Screening>()), Times.Once);
            _applicationRepositoryMock.Verify(x => x.Update(It.IsAny<ApplicationEntity>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.CommitTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.RollbackTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
            _notificationServiceMock.Verify(x => x.SendAsync(It.IsAny<CreateNotificationDto>(), It.IsAny<CancellationToken>()), Times.Never);

        }

    }
}
