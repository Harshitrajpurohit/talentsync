using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Domain.Enums.Notifications;

namespace TalentSync.Application.DTOs.Notifications
{
    public class NotificationRealtimeDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Title { get; set; } = default!;
        public string Message { get; set; } = default!;
        public NotificationCategory Category { get; set; }
        public bool IsRead { get; set; }
        public DateTime? ReadAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
