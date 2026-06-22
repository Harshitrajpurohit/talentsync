using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TalentSync.Api.Extensions;
using TalentSync.Application.DTOs.Recruitment;
using TalentSync.Application.Interfaces.Services;
using TalentSync.Domain.Entities.User;

namespace TalentSync.Api.Controllers.Recruitment
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScreeningsController : ControllerBase
    {
        private readonly IScreeningService _screeningService;
        private readonly ILogger<ScreeningsController> _logger;

        public ScreeningsController(IScreeningService screeningService, ILogger<ScreeningsController> logger)
        {
            _screeningService = screeningService;
            _logger = logger;
        }

        [HttpPost]
        [Authorize(Roles = "Admin,HR,Recruiter")]
        public async Task<IActionResult> CreateAsync([FromBody] CreateScreeningRequestDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Guid userId = User.GetUserId();

            ScreeningResponseDto screeningResponse = await _screeningService.CreateScreeningAsync(dto, userId, cancellationToken);

            return StatusCode(StatusCodes.Status201Created, screeningResponse);
        }

        [HttpGet("{id:Guid}")]
        [Authorize(Roles = "Admin,HR,Recruiter")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            ScreeningResponseDto screening = await _screeningService.GetByIdAsync(id, cancellationToken);
            return Ok(screening);
        }

        [HttpGet("application/{applicationId:Guid}")]
        [Authorize(Roles = "Admin,HR,Recruiter,Candidate")]
        public async Task<IActionResult> GetByApplicationId(Guid applicationId, CancellationToken cancellationToken)
        {
            Guid userId = User.GetUserId();
            ScreeningResponseDto screening = await _screeningService.GetByApplicationIdAsync(applicationId, userId, cancellationToken);

            return Ok(screening);
        }

        [HttpPut("{id:Guid}")]
        [Authorize(Roles = "Admin,HR,Recruiter")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateScreeningRequestDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            ScreeningResponseDto updated = await _screeningService.UpdateScreeningAsync(id, dto, cancellationToken);

            return Ok(updated);
        }

    }
}
