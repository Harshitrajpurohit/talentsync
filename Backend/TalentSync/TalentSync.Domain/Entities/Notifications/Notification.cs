using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Domain.Common;
using TalentSync.Domain.Enums.Notifications;

namespace TalentSync.Domain.Entities.Notifications
{
    public class Notification : BaseEntity
    {
        public Guid UserId { get; set; }
        public User.User User { get; set; } = default!;
        public string Title { get; set; } = default!;
        public string Message { get; set; } = default!;
        public NotificationCategory Category { get; set; }
        public NotificationChannel Channel { get; set; }
        public NotificationStatus Status { get; set; }
        public bool IsRead { get; set; }
        public DateTime? ReadAt { get; set; }


    }
}
