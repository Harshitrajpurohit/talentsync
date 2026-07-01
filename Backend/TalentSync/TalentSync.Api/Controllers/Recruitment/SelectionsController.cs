using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TalentSync.Application.DTOs.Recruitment;
using TalentSync.Application.Interfaces.Services;

namespace TalentSync.Api.Controllers.Recruitment
{
    [Route("api/[controller]")]
    [ApiController]
    public class SelectionsController : ControllerBase
    {
        private readonly ISelectionService _selectionService;
        public SelectionsController(ISelectionService selectionService)
        {
            _selectionService = selectionService;
        }

        [Authorize(Roles = "Admin,HR")]
        [HttpPost]
        public async Task<IActionResult> MakeDecision([FromBody] CreateSelectionDecisionDto createSelectionDecision, CancellationToken cancellationToken)
        {
            SelectionResponseDto selectionResponse = await _selectionService.MakeDecisionAsync(createSelectionDecision, cancellationToken);
            return Ok(selectionResponse);
        }

        [Authorize(Roles = "Admin,HR,Candidate")]
        [HttpGet("application/{applicationId}")]
        public async Task<IActionResult> GetByApplicationId(Guid applicationId, CancellationToken cancellationToken)
        {
            SelectionWithDetailsResponseDto? selection = await _selectionService.GetByApplicationIdAsync(applicationId, cancellationToken);
            return Ok(selection);
        }
    }
}
