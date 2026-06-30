using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Application.Common.Pagination;
using TalentSync.Domain.Entities.Notifications;

namespace TalentSync.Application.Interfaces.Repositories
{
    public interface INotificationRepository
    {
        Task<Notification> AddAsync(Notification notification, CancellationToken cancellationToken);
        Task<Notification?> GetByIdAsync(Guid Id, CancellationToken cancellationToken);
        Task<List<Notification>?> GetByUserIdAsync(Guid userId, PaginationRequest paginationRequest, CancellationToken cancellationToken);
        Task<List<Notification>> GetUnreadByUserIdAsync(Guid userId, CancellationToken cancellationToken);
        Task<List<Notification>> GetUnreadPagedByUserIdAsync(Guid userId, PaginationRequest paginationRequest, CancellationToken cancellationToken);
        Task<(List<Notification> Items, int TotalCount)> GetPagedByUserIdAsync(Guid userId, PaginationRequest pagination, CancellationToken cancellationToken);
        Task<int> GetUnreadCountAsync(Guid userId, CancellationToken cancellationToken);
        Task<int> GetTotalCountAsync(Guid userId, CancellationToken cancellationToken);
        Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken);
        void Update(Notification notification);
    }
}
