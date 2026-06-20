using System.Security.Claims;

namespace TalentSync.Api.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static Guid GetUserId(this ClaimsPrincipal user)
        {
            var userIdClaim =
                user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!Guid.TryParse(userIdClaim, out var userId))
                throw new UnauthorizedAccessException();

            return userId;
        }
    }
}
