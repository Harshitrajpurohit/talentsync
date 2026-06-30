using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Application.Common.Pagination;
using TalentSync.Application.Interfaces.Repositories;
using TalentSync.Domain.Entities.Notifications;
using TalentSync.Domain.Enums.Notifications;
using TalentSync.Infrastructure.Persistence;

namespace TalentSync.Infrastructure.Repositories.Notifications
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly ApplicationDbContext _context;

        public NotificationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Notification> AddAsync(Notification notification, CancellationToken cancellationToken)
        {
            return (await _context.Notifications.AddAsync(notification, cancellationToken)).Entity;
        }

        public async Task<Notification?> GetByIdAsync(Guid Id , CancellationToken cancellationToken)
        {
            return await _context.Notifications
                .FirstOrDefaultAsync(x => x.Id == Id && !x.IsDeleted && x.Channel == NotificationChannel.InApp, cancellationToken);
        }

        public async Task<List<Notification>?> GetByUserIdAsync(Guid userId,PaginationRequest paginationRequest, CancellationToken cancellationToken)
        {
            return await _context.Notifications
                .AsNoTracking()
                .Where(x => x.UserId == userId && !x.IsDeleted && x.Channel == NotificationChannel.InApp)
                .OrderByDescending(x => x.CreatedAt)
                .Skip((paginationRequest.PageNumber - 1) * paginationRequest.PageSize)
                .Take(paginationRequest.PageSize)
                .ToListAsync(cancellationToken);

        }

        public async Task<List<Notification>> GetUnreadPagedByUserIdAsync(Guid userId, PaginationRequest paginationRequest, CancellationToken cancellationToken)
        {
            return await _context.Notifications
                .AsNoTracking()
                .Where(x => x.UserId == userId && !x.IsRead && !x.IsDeleted && x.Channel == NotificationChannel.InApp)
                .OrderByDescending(x => x.CreatedAt)
                .Skip((paginationRequest.PageNumber - 1) * paginationRequest.PageSize)
                .Take(paginationRequest.PageSize)
                .ToListAsync(cancellationToken);
        }
        public async Task<List<Notification>> GetUnreadByUserIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await _context.Notifications
                .Where(x => x.UserId == userId && !x.IsRead && !x.IsDeleted)
                .ToListAsync(cancellationToken);
        }

        public async Task<(List<Notification> Items, int TotalCount)> GetPagedByUserIdAsync(Guid userId, PaginationRequest pagination, CancellationToken cancellationToken)
        {
            var query = _context.Notifications
                .AsNoTracking()
                .Where(n => n.UserId == userId && !n.IsDeleted && n.Channel == NotificationChannel.InApp);

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderByDescending(n => n.CreatedAt)
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync(cancellationToken);

            return (items, totalCount);
        }

        public async Task<int> GetUnreadCountAsync(Guid userId,CancellationToken cancellationToken)
        {
            return await _context.Notifications
                .CountAsync( x => x.UserId == userId && !x.IsRead && !x.IsDeleted && x.Channel == NotificationChannel.InApp, cancellationToken);
        }

        public async Task<int> GetTotalCountAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await _context.Notifications
                .CountAsync(x => x.UserId == userId && !x.IsRead && !x.IsDeleted && x.Channel == NotificationChannel.InApp, cancellationToken);
        }

        public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Notifications
                .AnyAsync(x => x.Id == id && !x.IsDeleted, cancellationToken);
        }

        public void Update(Notification notification)
        {
            _context.Notifications.Update(notification);
        }

    }
}
