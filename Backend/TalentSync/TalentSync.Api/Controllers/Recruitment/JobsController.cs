using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TalentSync.Api.Extensions;
using TalentSync.Application.Common.Pagination;
using TalentSync.Application.DTOs.Recruitment;
using TalentSync.Application.DTOs.User;
using TalentSync.Application.Interfaces.Services;
using TalentSync.Domain.Entities.Recruitment;
using TalentSync.Domain.Entities.User;

namespace TalentSync.Api.Controllers.Recruitment
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        private readonly IJobService _jobService;
        private readonly ILogger<JobsController> _logger;

        public JobsController(IJobService jobService, ILogger<JobsController> logger)
        {
            _jobService = jobService;
            _logger = logger;
        }

        [Authorize(Roles = "HR,Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateJobAsync([FromBody] CreateJobDto jobDto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userId = User.GetUserId();
            _logger.LogInformation("Creating job by HR {UserId}", userId);
            JobResponseDto job = await _jobService.CreateJobAsync(jobDto, userId, cancellationToken);

            return StatusCode(StatusCodes.Status201Created, job);
        }

        [Authorize]
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetJobByIdAsync(Guid Id, CancellationToken cancellationToken)
        {
            JobResponseDto jobResponse = await _jobService.GetJobByIdAsync(Id, cancellationToken);
            return Ok(jobResponse);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllJobsAsync([FromQuery]PaginationRequest paginationRequest, CancellationToken cancellationToken)
        {
            PaginationResponse<JobListDto> jobLists = await _jobService.GetAllJobsAsync(paginationRequest, cancellationToken);
            return Ok(jobLists);
        }

        [Authorize(Roles = "HR,Admin")]
        [HttpPut("{Id}")]
        public async Task<IActionResult> UpdateJobAsync(Guid Id,UpdateJobRequestDto updateJobRequestDto , CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            JobResponseDto jobResponse = await _jobService.UpdateJobAsync(Id, userId, updateJobRequestDto, cancellationToken);

            return Ok(jobResponse);
        }

        [Authorize(Roles = "HR,Admin")]
        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteJobAsync(Guid Id, CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            bool isDeleted = await _jobService.DeleteJobAsync(Id, userId, cancellationToken);

            return NoContent();
        }

    }
}
