using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TalentSync.Api.Extensions;
using TalentSync.Application.Common.Pagination;
using TalentSync.Application.DTOs.Recruitment;
using TalentSync.Application.Interfaces.Services;
using TalentSync.Application.Services.Recruitment;
using TalentSync.Domain.Enums.Recruitment;

namespace TalentSync.Api.Controllers.Recruitment
{
    [Route("api/[controller]")]
    [ApiController]
    public class InterviewsController : ControllerBase
    {
        private readonly IInterviewService _interviewService;

        public InterviewsController(IInterviewService interviewService)
        {
            _interviewService = interviewService;
        }

        [Authorize(Roles = "HR,Manager,Recruiter")]
        [HttpPost]
        public async Task<IActionResult> ScheduleInterview([FromBody] ScheduleInterviewDto scheduleInterviewDto, CancellationToken cancellationToken)
        {
            InterviewResponseDto interview = await _interviewService.ScheduleInterviewAsync(scheduleInterviewDto, cancellationToken);

            return Ok(interview);
        }

        [Authorize(Roles = "HR")]
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

        [Authorize(Roles = "HR")]
        [HttpPatch("{id}/reschedule")]
        public async Task<IActionResult> RescheduleInterview(Guid id, [FromBody] RescheduleInterviewDto rescheduleInterview, CancellationToken cancellationToken)
        {

            InterviewResponseDto interview = await _interviewService.RescheduleInterviewAsync(id, rescheduleInterview, cancellationToken);

            return Ok(interview);
        }

        [Authorize(Roles = "Manager,HR")]
        [HttpPatch("{id}/outcome")]
        public async Task<IActionResult> UpdateInterviewOutcome(Guid id, [FromBody] UpdateInterviewStatusDto updateInterviewStatus, CancellationToken cancellationToken)
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
            InterviewResponseDto interview = await _interviewService.GetByApplicationIdAsync(applicationId, cancellationToken);
            return Ok(interview);
        }


        [Authorize(Roles = "Manager,HR,Recruiter")]
        [HttpGet("assigned")]
        public async Task<IActionResult> InterviewsAssignedToInterviewer([FromQuery] InterviewPaginationRequest paginationRequest, CancellationToken cancellationToken)
        {
            Guid interviewerId = User.GetUserId();

            PaginationResponse<InterviewDetailedResponseDto> interviews = await _interviewService.InterviewsAssignedToInterviwerAsync(interviewerId, paginationRequest, cancellationToken);

            return Ok(interviews);
        }

        [Authorize(Roles = "Manager,HR,Recruiter")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            InterviewDetailedResponseDto interview = await _interviewService.GetByIdWithDetailsAsync(id, cancellationToken);

            return Ok(interview);
        }

        [Authorize(Roles = "HR,Recruiter,Manager")]
        [HttpGet("candidate/{candidateId}")]
        public async Task<IActionResult> GetByCandidate(Guid candidateId, [FromQuery] InterviewPaginationRequest paginationRequest, CancellationToken cancellationToken)
        {

            PaginationResponse<InterviewDetailedResponseDto> interviews = await _interviewService.GetPagedByCandidateIdAsync(candidateId, paginationRequest, cancellationToken);
            return Ok(interviews);
        }

        [Authorize(Roles = "Candidate")]
        [HttpGet("candidate")]
        public async Task<IActionResult> GetMyInterviews([FromQuery] InterviewPaginationRequest paginationRequest, CancellationToken cancellationToken)
        {
            Guid candidateId = User.GetUserId();

            PaginationResponse<CandidateInterviewResponseDto> interviews = await _interviewService.GetByCandidateIdAsync(candidateId, paginationRequest, cancellationToken);

            return Ok(interviews);
        }
    }
}
