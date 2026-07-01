using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<SignalRNotificationSender> _logger;

        public SignalRNotificationSender(IHubContext<NotificationHub> hubContext, ILogger<SignalRNotificationSender> logger)
        {
            _hubContext = hubContext;
            _logger = logger;
        }

        public NotificationChannel Channel => NotificationChannel.InApp;

        public async Task SendAsync(NotificationRealtimeDto notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Sending realtime notification to user: {UserId}", notification.UserId);
            // signalR
            await _hubContext.Clients
                .User(notification.UserId.ToString())
                .SendAsync("ReceiveNotification", notification, cancellationToken);
            _logger.LogInformation("Realtime notification sent to user: {UserId}", notification.UserId);
        }
    }
}
