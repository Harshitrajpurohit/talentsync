using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Domain.Enums.Notifications;

namespace TalentSync.Application.DTOs.Notifications
{
    public class NotificationResponseDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = default!;
        public string Message { get; set; } = default!;
        public bool IsRead { get; set; }
        public DateTime? ReadAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
