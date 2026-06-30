using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Application.Common.Pagination;
using TalentSync.Application.DTOs.Notifications;

namespace TalentSync.Application.Interfaces.Services
{
    public interface INotificationService
    {
        Task SendAsync(CreateNotificationDto createNotification, CancellationToken cancellationToken);
        Task<PaginationResponse<NotificationResponseDto>> GetMyNotificationsAsync(Guid userId, PaginationRequest pagination, CancellationToken cancellationToken);
        Task<int> GetUnreadCountAsync(Guid userId, CancellationToken cancellationToken);
        Task MarkAsReadAsync(Guid notificationId, Guid userId, CancellationToken cancellationToken);
        Task MarkAllAsReadAsync(Guid userId, CancellationToken cancellationToken);
    }
}
