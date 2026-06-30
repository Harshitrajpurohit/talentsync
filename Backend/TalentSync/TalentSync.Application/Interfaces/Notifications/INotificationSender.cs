using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Application.DTOs.Notifications;
using TalentSync.Domain.Entities.Notifications;
using TalentSync.Domain.Enums.Notifications;

namespace TalentSync.Application.Interfaces.Notifications
{
    public interface INotificationSender
    {
        NotificationChannel Channel { get; }
        Task SendAsync(NotificationRealtimeDto notification, CancellationToken cancellationToken);
    }
}
