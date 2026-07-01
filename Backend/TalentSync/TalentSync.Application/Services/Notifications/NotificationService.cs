using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Application.Common.Pagination;
using TalentSync.Application.DTOs.Notifications;
using TalentSync.Application.Interfaces;
using TalentSync.Application.Interfaces.Notifications;
using TalentSync.Application.Interfaces.Repositories;
using TalentSync.Application.Interfaces.Services;
using TalentSync.Domain.Entities.Notifications;
using TalentSync.Domain.Enums.Notifications;

namespace TalentSync.Application.Services.Notifications
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IMapper _mapper;
        private readonly IEnumerable<INotificationSender> _notificationSenders;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(INotificationRepository notificationRepository, 
            IMapper mapper, 
            IEnumerable<INotificationSender> notificationSenders,
            IUnitOfWork unitOfWork,
            ILogger<NotificationService> logger
            )
        {
            _notificationRepository = notificationRepository;
            _mapper = mapper;
            _notificationSenders = notificationSenders;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task SendAsync(CreateNotificationDto createNotification, CancellationToken cancellationToken)
        {


            Notification notification = _mapper.Map<Notification>(createNotification);

            notification.Status = NotificationStatus.Pending;
            notification.IsRead = false;

            await _notificationRepository.AddAsync(notification, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var sender = _notificationSenders.FirstOrDefault(x => x.Channel == notification.Channel);

            if (sender is null)
            {
                _logger.LogError(
                        "No notification sender registered for channel {Channel}.",
                        notification.Channel);
                throw new InvalidOperationException(
                        $"No notification sender registered for channel '{notification.Channel}'.");

            }

            NotificationRealtimeDto notificationRealtime = _mapper.Map<NotificationRealtimeDto>(notification);

            try
            {
                await sender.SendAsync(notificationRealtime, cancellationToken);

                notification.Status = NotificationStatus.Sent;
                notification.UpdatedAt = DateTime.UtcNow;
            }
            catch
            {
                notification.Status = NotificationStatus.Failed;
                notification.UpdatedAt = DateTime.UtcNow;
                throw;
            }
            finally
            {
                _logger.LogInformation("Notification {NotificationId} sent with status {Status}", notification.Id, notification.Status);
                _notificationRepository.Update(notification);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task<PaginationResponse<NotificationResponseDto>> GetMyNotificationsAsync(Guid userId, PaginationRequest pagination, CancellationToken cancellationToken)
        {

            var (notifications, totalCount) = await _notificationRepository.GetPagedByUserIdAsync(userId, pagination, cancellationToken);

            List<NotificationResponseDto> result = _mapper.Map<List<NotificationResponseDto>>(notifications);

            PaginationResponse<NotificationResponseDto> paginationResponse = new PaginationResponse<NotificationResponseDto>(
                pageNumber: pagination.PageNumber,
                pageSize: pagination.PageSize,
                totalRecords: totalCount,
                data: result
                );

            return paginationResponse;
        }

        public async Task<int> GetUnreadCountAsync(Guid userId,CancellationToken cancellationToken)
        {
            return await _notificationRepository.GetUnreadCountAsync(userId, cancellationToken);
        }

        public async Task MarkAsReadAsync(Guid notificationId, Guid userId, CancellationToken cancellationToken)
        {
            Notification? notification = await _notificationRepository.GetByIdAsync(notificationId, cancellationToken);
            if (notification == null || notification.UserId != userId)
            {
                throw new KeyNotFoundException("Notification not found.");
            }
            if (notification.IsRead)
            {
                return;
            }
            notification.IsRead = true;
            notification.ReadAt = DateTime.UtcNow;
            notification.UpdatedAt = DateTime.UtcNow;
            _notificationRepository.Update(notification);
            _logger.LogInformation("Notification {NotificationId} marked as read by user {UserId}", notification.Id, userId);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task MarkAllAsReadAsync(Guid userId, CancellationToken cancellationToken)
        {
            List<Notification> notifications = await _notificationRepository.GetUnreadByUserIdAsync(userId, cancellationToken);
            if(notifications.Count<=0)
            {
                return;
            }

            foreach (var notification in notifications)
            {
                notification.IsRead = true;
                notification.ReadAt = DateTime.UtcNow;
                notification.UpdatedAt = DateTime.UtcNow;
            }
            _logger.LogInformation("All notifications marked as read by user {UserId}", userId);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

    }
}
