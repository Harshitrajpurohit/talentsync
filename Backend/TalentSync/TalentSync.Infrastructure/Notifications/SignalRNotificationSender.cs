using Microsoft.AspNetCore.SignalR;
using TalentSync.Application.DTOs.Notifications;
using TalentSync.Application.Interfaces.Notifications;
using TalentSync.Domain.Entities.Notifications;
using TalentSync.Domain.Enums.Notifications;
using TalentSync.Infrastructure.Notifications.SignalR.Hubs;

namespace TalentSync.Infrastructure.Notifications
{
    public class SignalRNotificationSender : INotificationSender
    {

        private readonly IHubContext<NotificationHub> _hubContext;

        public SignalRNotificationSender(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public NotificationChannel Channel => NotificationChannel.InApp;

        public async Task SendAsync(NotificationRealtimeDto notification, CancellationToken cancellationToken)
        {
            // signalR
            await _hubContext.Clients
                .User(notification.UserId.ToString())
                .SendAsync("ReceiveNotification", notification, cancellationToken);
        }
    }
}
