using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TalentSync.Api.Extensions;
using TalentSync.Application.Common.Pagination;
using TalentSync.Application.DTOs.Notifications;
using TalentSync.Application.Interfaces.Services;

namespace TalentSync.Api.Controllers.Notifications
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        private readonly ILogger<NotificationsController> _logger;

        public NotificationsController(INotificationService notificationService, ILogger<NotificationsController> logger)
        {
            _notificationService = notificationService;
            _logger = logger;
        }

        [Authorize]
        [HttpGet("my")]
        public async Task<IActionResult> GetMyNotificationsAsync([FromQuery] PaginationRequest pagination, CancellationToken cancellationToken)
        {
            Guid userId = User.GetUserId();
            return Ok(await _notificationService.GetMyNotificationsAsync(userId, pagination, cancellationToken));
        }


        [Authorize]
        [HttpGet("unread/count")]
        public async Task<IActionResult> GetUnreadCountAsync(CancellationToken cancellationToken)
        {
            Guid userId = User.GetUserId();
            int count = await _notificationService.GetUnreadCountAsync(userId, cancellationToken);
            return Ok(new { unreadCount = count });
        }

        [Authorize]
        [HttpPut("{notificationId:guid}")]
        public async Task<IActionResult> MarkAsReadAsync(Guid notificationId, CancellationToken cancellationToken)
        {
            Guid userId = User.GetUserId();
            await _notificationService.MarkAsReadAsync(notificationId, userId, cancellationToken);

            return NoContent();
        }

        [Authorize]
        [HttpPut("read-all")]
        public async Task<IActionResult> MarkAllAsReadAsync(CancellationToken cancellationToken)
        {
            Guid userId = User.GetUserId();
            await _notificationService.MarkAllAsReadAsync(userId, cancellationToken);

            return NoContent();
        }
    }
}
