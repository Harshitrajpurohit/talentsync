using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TalentSync.Api.Extensions;
using TalentSync.Application.DTOs.Recruitment;
using TalentSync.Application.Interfaces.Services;
using TalentSync.Domain.Enums.Recruitment;

namespace TalentSync.Api.Controllers.Recruitment
{
    [Route("api/[controller]")]
    [ApiController]
    public class InterviewsController : ControllerBase
    {
        private readonly IInterviewService _interviewService;
        private readonly ILogger<InterviewsController> _logger;

        public InterviewsController(IInterviewService interviewService, ILogger<InterviewsController> logger)
        {
            _interviewService = interviewService;
            _logger = logger;
        }

        [Authorize(Roles = "HR,Manager,Recruiter")]
        [HttpPost]
        public async Task<IActionResult> ScheduleInterview([FromBody] ScheduleInterviewDto scheduleInterviewDto, CancellationToken cancellationToken)
        {
            InterviewResponseDto interview = await _interviewService.ScheduleInterviewAsync(scheduleInterviewDto, cancellationToken);

            return Ok(interview);
        }

        [Authorize(Roles = "HR,Recruiter")]
        [HttpPatch("{id}/Cancel")]
        public async Task<IActionResult> CancelInterview(Guid id,[FromBody] UpdateInterviewStatusDto updateInterviewStatus, CancellationToken cancellationToken)
        {
            var allowedStatus = new[] { InterviewStatus.Cancelled };
            if (!allowedStatus.Contains(updateInterviewStatus.Status))
            {
                return BadRequest(new
                {
                    message = "Status Can only Cancelled."
                });
            }

            InterviewResponseDto interview = await _interviewService.UpdateInterviewStatusAsync(id, updateInterviewStatus, cancellationToken);

            return Ok(interview);
        }

        [Authorize(Roles = "HR,Recruiter")]
        [HttpPatch("{id}/reschedule")]
        public async Task<IActionResult> RescheduleInterview(Guid id, [FromBody] RescheduleInterviewDto rescheduleInterview, CancellationToken cancellationToken)
        {

            InterviewResponseDto interview = await _interviewService.RescheduleInterviewAsync(id, rescheduleInterview, cancellationToken);

            return Ok(interview);
        }

        [Authorize(Roles = "Manager,HR")]
        [HttpPatch("{id}/outcome")]
        public async Task<IActionResult> RecordOutcome(Guid id, [FromBody] UpdateInterviewStatusDto updateInterviewStatus, CancellationToken cancellationToken)
        {
            var allowed = new[] { InterviewStatus.Passed, InterviewStatus.Failed };
            if (!allowed.Contains(updateInterviewStatus.Status))
                return BadRequest(new
                {
                    message = "Candidate Status can only Pass or Fail."
                });

            InterviewResponseDto interviewResponse = await _interviewService.UpdateInterviewStatusAsync(id, updateInterviewStatus, cancellationToken);

            return Ok(interviewResponse);
        }

        [Authorize(Roles = "Admin,HR,Recruiter,Manager,Candidate")]
        [HttpGet("application/{applicationId}")]
        public async Task<IActionResult> GetByApplicationId(Guid applicationId, CancellationToken cancellationToken)
        {
            List<InterviewDetailedResponseDto> interviews = await _interviewService.GetByApplicationIdAsync(applicationId, cancellationToken);
            return Ok(interviews);
        }


        [Authorize(Roles = "Manager,HR,Recruiter")]
        [HttpGet("my")]
        public async Task<IActionResult> InterviewsAssignedToInterviwer(CancellationToken cancellationToken)
        {
            Guid interviewerId = User.GetUserId();

            List<InterviewDetailedResponseDto> interviews = await _interviewService.InterviewsAssignedToInterviwerAsync(interviewerId, cancellationToken);

            return Ok(interviews);
        }

        [Authorize(Roles = "Manager,HR,Recruiter")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            InterviewDetailedResponseDto interview = await _interviewService.GetByIdWithDetailsAsync(id, cancellationToken);

            return Ok(interview);
        }

    }
}
