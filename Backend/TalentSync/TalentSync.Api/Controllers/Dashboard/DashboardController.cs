using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TalentSync.Application.Interfaces.Services;
using TalentSync.Domain.Enums.User;

namespace TalentSync.Api.Controllers.Dashboard
{
    [Route("api/dashboard")]
    [ApiController]
    public class DashboardController : ControllerBase
    {

        private readonly ICandidateDashboardService _candidateDashboardService;

        public DashboardController(ICandidateDashboardService candidateDashboardService)
        {
            _candidateDashboardService = candidateDashboardService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetDashboardAsync(CancellationToken cancellationToken)
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Console.WriteLine(userId);
            if (!Guid.TryParse(userId, out var uId))
            {
                return Unauthorized();
            }

            var role = User.FindFirstValue(ClaimTypes.Role);
            Console.WriteLine(role);
            if (string.IsNullOrWhiteSpace(role))
            {
                return Unauthorized();
            }

            switch (role)
            {
                case nameof(RoleName.Candidate):
                    return Ok(await _candidateDashboardService.GetDashboardAsync(uId, cancellationToken));

                case nameof(RoleName.Recruiter):
                // return Ok(await _recruiterDashboardService.GetDashboardAsync(...));

                case nameof(RoleName.HR):
                // ...

                case nameof(RoleName.Manager):
                // ...

                case nameof(RoleName.Employee):
                // ...

                case nameof(RoleName.Admin):
                // ...

                default:
                    return Forbid();
            }
        }
    }
}
