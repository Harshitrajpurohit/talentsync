using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TalentSync.Api.Extensions;
using TalentSync.Application.DTOs.Recruitment;
using TalentSync.Application.Interfaces.Services;
using TalentSync.Domain.Entities.Recruitment;
using static System.Net.Mime.MediaTypeNames;

namespace TalentSync.Api.Controllers.Recruitment
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResumesController : ControllerBase
    {
        private readonly IResumeService _resumeService;

        public ResumesController(IResumeService resumeService)
        {
            _resumeService = resumeService;
        }

        [Authorize(Roles = "Candidate")]
        [HttpPost]
        public async Task<IActionResult> UploadResumeAsync([FromForm] IFormFile resume, CancellationToken cancellationToken)
        {
            if (resume == null || resume.Length == 0)
                return BadRequest("No file provided.");

            var candidateId = User.GetUserId();

            using var stream = resume.OpenReadStream();

            ResumeResponseDto resumeResponse = await _resumeService.UploadResumeAsync(candidateId, stream, resume.FileName, resume.ContentType, resume.Length, cancellationToken);

            return StatusCode(StatusCodes.Status201Created, resumeResponse);
        }

        [Authorize(Roles = "Candidate")]
        [HttpGet("me")]
        public async Task<IActionResult> GetMyResume(CancellationToken cancellationToken)
        {
            var candidateId = User.GetUserId();

            var result = await _resumeService.GetByCandidateIdAsync(candidateId, cancellationToken);

            return Ok(result);
        }

        [Authorize(Roles = "HR,Manager,Recruiter")]
        [HttpGet("candidate/{candidateId:guid}")]
        public async Task<IActionResult> GetCandidateResume(Guid candidateId, CancellationToken cancellationToken)
        {
            var result = await _resumeService.GetByCandidateIdAsync(candidateId, cancellationToken);

            return Ok(result);
        }

        [Authorize(Roles = "Candidate")]
        [HttpPut("{id}")]
        public async Task<IActionResult> ReplaceResumeAsync(Guid id, [FromForm] IFormFile resume, CancellationToken cancellationToken)
        {
            if (resume == null || resume.Length == 0)
                return BadRequest("No file provided.");

            var candidateId = User.GetUserId();

            using var stream = resume.OpenReadStream();

            ResumeResponseDto resumeResponse = await _resumeService.ReplaceResumeAsync(id, stream, resume.FileName, resume.ContentType, resume.Length, cancellationToken);

            return Ok(resumeResponse);
        }

    }
}
