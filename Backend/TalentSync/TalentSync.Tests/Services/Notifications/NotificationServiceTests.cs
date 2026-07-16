using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Application.Common.Pagination;
using TalentSync.Application.DTOs.Notifications;
using TalentSync.Application.Interfaces;
using TalentSync.Application.Interfaces.Notifications;
using TalentSync.Application.Interfaces.Repositories;
using TalentSync.Application.Services.Notifications;
using TalentSync.Domain.Entities.Notifications;
using TalentSync.Domain.Enums.Notifications;

namespace TalentSync.Tests.Services.Notifications
{
    public class NotificationServiceTests
    {

        private readonly Mock<INotificationRepository> _notificationRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<INotificationSender> _notificationSenderMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ILogger<NotificationService>> _loggerMock;

        private readonly NotificationService _notificationService;

        public NotificationServiceTests()
        {
            _notificationRepositoryMock = new Mock<INotificationRepository>();
            _mapperMock = new Mock<IMapper>();
            _notificationSenderMock = new Mock<INotificationSender>();

            _notificationSenderMock
               .Setup(x => x.Channel)
               .Returns(NotificationChannel.InApp);

            var senders = new List<INotificationSender>
            {
                _notificationSenderMock.Object
            };

            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _loggerMock = new Mock<ILogger<NotificationService>>();

            _notificationService = new NotificationService(
                _notificationRepositoryMock.Object,
                _mapperMock.Object,
                senders,
                _unitOfWorkMock.Object,
                _loggerMock.Object
                );

        }

        [Fact]
        public async Task SendAsync_Should_Send_Notification_Successfully()
        {
           
            var dto = new CreateNotificationDto
            {
                UserId = Guid.NewGuid(),
                Title = "Test",
                Message = "Test Message",
                Category = NotificationCategory.Recruitment,
                Channel = NotificationChannel.InApp
            };

            var notification = new Notification
            {
                Id = Guid.NewGuid(),
                UserId = dto.UserId,
                Title = dto.Title,
                Message = dto.Message,
                Category = dto.Category,
                Channel = dto.Channel
            };

            var realtimeDto = new NotificationRealtimeDto
            {
                Id = notification.Id,
                UserId = notification.UserId,
                Title = notification.Title,
                Message = notification.Message,
                Category = notification.Category
            };

            _mapperMock.Setup(x => x.Map<Notification>(dto)).Returns(notification);

            _notificationRepositoryMock.Setup(x => x.AddAsync(notification, It.IsAny<CancellationToken>()));

            _mapperMock.Setup(x => x.Map<NotificationRealtimeDto>(notification)).Returns(realtimeDto);

            _notificationSenderMock
                .Setup(x => x.SendAsync(realtimeDto, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            

            await _notificationService.SendAsync(dto, CancellationToken.None);

            Assert.Equal(NotificationStatus.Sent, notification.Status);
            Assert.False(notification.IsRead);
            Assert.NotNull(notification.UpdatedAt);

            _mapperMock.Verify(x => x.Map<Notification>(dto), Times.Once);

            _mapperMock.Verify(x => x.Map<NotificationRealtimeDto>(notification), Times.Once);

            _notificationRepositoryMock.Verify(x => x.AddAsync(notification, It.IsAny<CancellationToken>()), Times.Once);

            _notificationRepositoryMock.Verify( x => x.Update(notification),Times.Once);

            _notificationSenderMock.Verify(x => x.SendAsync(realtimeDto, It.IsAny<CancellationToken>()), Times.Once);

            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Exactly(2));
        }


        [Fact]
        public async Task SendAsync_Should_Throw_When_SendAsync_Throw_Exception()
        {

            var dto = new CreateNotificationDto
            {
                UserId = Guid.NewGuid(),
                Title = "Test",
                Message = "Test Message",
                Category = NotificationCategory.Recruitment,
                Channel = NotificationChannel.InApp
            };

            var notification = new Notification
            {
                Id = Guid.NewGuid(),
                UserId = dto.UserId,
                Title = dto.Title,
                Message = dto.Message,
                Category = dto.Category,
                Channel = dto.Channel
            };

            var realtimeDto = new NotificationRealtimeDto
            {
                Id = notification.Id,
                UserId = notification.UserId,
                Title = notification.Title,
                Message = notification.Message,
                Category = notification.Category
            };

            _mapperMock.Setup(x => x.Map<Notification>(dto)).Returns(notification);

            _notificationRepositoryMock.Setup(x => x.AddAsync(notification, It.IsAny<CancellationToken>()));

            _mapperMock.Setup(x => x.Map<NotificationRealtimeDto>(notification)).Returns(realtimeDto);

            _notificationSenderMock
                .Setup(x => x.SendAsync(realtimeDto, It.IsAny<CancellationToken>())).Throws<InvalidOperationException>();


            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _notificationService.SendAsync(dto, CancellationToken.None));

            Assert.Equal(NotificationStatus.Failed, notification.Status);
            Assert.False(notification.IsRead);
            Assert.NotNull(notification.UpdatedAt);

            _mapperMock.Verify(x => x.Map<Notification>(dto), Times.Once);

            _mapperMock.Verify(x => x.Map<NotificationRealtimeDto>(notification), Times.Once);

            _notificationRepositoryMock.Verify(x => x.AddAsync(notification, It.IsAny<CancellationToken>()), Times.Once);

            _notificationRepositoryMock.Verify(x => x.Update(notification), Times.Once);

            _notificationSenderMock.Verify(x => x.SendAsync(realtimeDto, It.IsAny<CancellationToken>()), Times.Once);

            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Exactly(2));

        }


        [Fact]
        public async Task GetMyNotificationsAsync_Should_Return_Notifications_Successfully()
        {
            Guid userId = Guid.NewGuid();
            PaginationRequest paginationRequest = new PaginationRequest
            {
                PageNumber = 1,
                PageSize = 10,
            };

            List<Notification> notifications = new List<Notification>
            {
                new Notification
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                }

            };

            int totalCount = 1;

            List<NotificationResponseDto> notificationResponses = new List<NotificationResponseDto>
            {
                new NotificationResponseDto
                {
                    Id = Guid.NewGuid(),
                }

            };

            PaginationResponse<NotificationResponseDto> response = new PaginationResponse<NotificationResponseDto>(
                paginationRequest.PageNumber,
                paginationRequest.PageSize,
                totalCount,
                notificationResponses
                );

            _notificationRepositoryMock.Setup(x => x.GetPagedByUserIdAsync(userId, paginationRequest, It.IsAny<CancellationToken>())).ReturnsAsync((notifications, totalCount));
            _mapperMock.Setup(x => x.Map<List<NotificationResponseDto>>(notifications)).Returns(notificationResponses);

            var result = await _notificationService.GetMyNotificationsAsync(userId, paginationRequest, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(totalCount, result.TotalRecords);

            _notificationRepositoryMock.Verify(x => x.GetPagedByUserIdAsync(userId, paginationRequest, It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(x => x.Map<List<NotificationResponseDto>>(notifications), Times.Once);


        }


        [Fact]
        public async Task GetUnreadCountAsync_Should_Return_Unreaded_Count_Successfully()
        {
            Guid userId = Guid.NewGuid();
            int totalCount = 1;

            _notificationRepositoryMock.Setup(x => x.GetUnreadCountAsync(userId, It.IsAny<CancellationToken>())).ReturnsAsync(totalCount);

            var result = await _notificationService.GetUnreadCountAsync(userId, CancellationToken.None);

            Assert.Equal(totalCount, result);

            _notificationRepositoryMock.Verify(x => x.GetUnreadCountAsync(userId, It.IsAny<CancellationToken>()), Times.Once);

        }

        [Fact]
        public async Task MarkAsReadAsync_Should_Mark_Notification_As_Read_Successfully()
        {
            Guid notificationId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();

            Notification notification = new Notification
            {
                Id = notificationId,
                UserId = userId,
            };
            _notificationRepositoryMock.Setup(x => x.GetByIdAsync(notificationId, It.IsAny<CancellationToken>())).ReturnsAsync(notification);
            _notificationRepositoryMock.Setup(x => x.Update(notification));

            await _notificationService.MarkAsReadAsync(notificationId, userId, CancellationToken.None);

            Assert.Equal(notification.Id, notificationId);
            Assert.True(notification.IsRead);

            _notificationRepositoryMock.Verify(x => x.GetByIdAsync(notificationId, It.IsAny<CancellationToken>()), Times.Once);
            _notificationRepositoryMock.Verify(x => x.Update(notification), Times.Once);

        }

        [Fact]
        public async Task MarkAsReadAsync_Should_Throw_When_Notification_NotFound()
        {
            Guid notificationId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();

            _notificationRepositoryMock.Setup(x => x.GetByIdAsync(notificationId, It.IsAny<CancellationToken>())).ReturnsAsync((Notification?)null);

            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _notificationService.MarkAsReadAsync(notificationId, userId, CancellationToken.None));

            Assert.Equal("Notification not found.", exception.Message);


            _notificationRepositoryMock.Verify(x => x.GetByIdAsync(notificationId, It.IsAny<CancellationToken>()), Times.Once);
            _notificationRepositoryMock.Verify(x => x.Update(It.IsAny<Notification>()), Times.Never);

        }

        [Fact]
        public async Task MarkAsReadAsync_Should_Throw_When_Notification_Already_Read()
        {
            Guid notificationId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();

            Notification notification = new Notification
            {
                Id = notificationId,
                UserId = userId,
                IsRead = true
            };

            _notificationRepositoryMock.Setup(x => x.GetByIdAsync(notificationId, It.IsAny<CancellationToken>())).ReturnsAsync(notification);

            await _notificationService.MarkAsReadAsync(notificationId, userId, CancellationToken.None);

            _notificationRepositoryMock.Verify(x => x.GetByIdAsync(notificationId, It.IsAny<CancellationToken>()), Times.Once);
            _notificationRepositoryMock.Verify(x => x.Update(It.IsAny<Notification>()), Times.Never);
        }

        [Fact]
        public async Task MarkAllAsReadAsync_Should_Mark_As_Read_Successfully()
        {
            Guid userId = Guid.NewGuid();

            List<Notification> notifications = new List<Notification>
            {
                new Notification
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    IsRead = false
                }
            };

            _notificationRepositoryMock.Setup(x => x.GetUnreadByUserIdAsync(userId, It.IsAny<CancellationToken>())).ReturnsAsync(notifications);

            await _notificationService.MarkAllAsReadAsync(userId, CancellationToken.None);

            Assert.True(notifications[0].IsRead);

            _notificationRepositoryMock.Verify(x => x.GetUnreadByUserIdAsync(userId, It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);

        }

        [Fact]
        public async Task MarkAllAsReadAsync_Should_Return_When_No_Unread_Notifications()
        {
            Guid userId = Guid.NewGuid();

            List<Notification> notifications = new List<Notification>
            {

            };

            _notificationRepositoryMock.Setup(x => x.GetUnreadByUserIdAsync(userId, It.IsAny<CancellationToken>())).ReturnsAsync(notifications);
            
            await _notificationService.MarkAllAsReadAsync(userId, CancellationToken.None);


            _notificationRepositoryMock.Verify(x => x.GetUnreadByUserIdAsync(userId, It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);

        }

    }
}
