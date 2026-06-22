using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TalentSync.Api.Extensions;
using TalentSync.Application.Common.Pagination;
using TalentSync.Application.DTOs.Recruitment;
using TalentSync.Application.Interfaces.Services;

namespace TalentSync.Api.Controllers.Recruitment
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationsController : ControllerBase
    {
        private readonly IApplicationService _applicationService;
        private readonly ILogger<ApplicationsController> _logger;

        public ApplicationsController(IApplicationService applicationService, ILogger<ApplicationsController> logger)
        {
            _applicationService = applicationService;
            _logger = logger;
        }

        [Authorize(Roles = "Candidate")]
        [HttpPost]
        public async Task<IActionResult> CreateApplicationAsync([FromBody] CreateApplicationDto dto, CancellationToken cancellationToken) {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var candidateId = User.GetUserId();
            _logger.LogInformation("Candidate {CandidateId} applied for Job {JobId}", candidateId, dto.JobId);

            ApplicationResponseDto application = await _applicationService.CreateApplicationAsync(dto, candidateId, cancellationToken);

            return StatusCode(StatusCodes.Status201Created, application);
        }

        [Authorize(Roles = "Admin,HR,Recruiter")]
        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] PaginationRequest paginationRequest, CancellationToken cancellationToken)
        {
            PaginationResponse<ApplicationWithDetailsResponseDto> applications = await _applicationService.GetAllAsync(paginationRequest, cancellationToken);
            return Ok(applications);
        }

        [Authorize(Roles = "Admin,HR,Recruiter")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var application = await _applicationService.GetByIdAsync(id, cancellationToken);
            return Ok(application);
        }

        [Authorize(Roles = "Admin,HR,Recruiter")]
        [HttpGet("job/{jobId}")]
        public async Task<IActionResult> GetByJob(Guid jobId, CancellationToken cancellationToken)
        {
            var applications = await _applicationService.GetByJobIdAsync(jobId, cancellationToken);
            return Ok(applications);
        }

        [Authorize(Roles = "Candidate,Admin")]
        [HttpGet("candidate")]
        public async Task<IActionResult> GetByCandidate(CancellationToken cancellationToken)
        {
            Guid candidateId = User.GetUserId();
            var applications = await _applicationService.GetByCandidateIdAsync(candidateId, cancellationToken);
            return Ok(applications);
        }

        [Authorize(Roles = "Admin,HR,Recruiter")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateApplicationRequestDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var updated = await _applicationService.UpdateApplicationAsync(id, dto, cancellationToken);
            if (updated == null) return NotFound(new { message = $"Application {id} not found." });

            return Ok(updated);
        }

        [Authorize(Roles = "Admin,Candidate,Recruiter")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var deleted = await _applicationService.DeleteApplicationAsync(id, cancellationToken);
            if (!deleted) return NotFound(new { message = $"Application {id} not found." });

            return NoContent();
        }

    }
}
