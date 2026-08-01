using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TalentSync.Application.Common.Pagination;
using TalentSync.Application.Interfaces.Services;

namespace TalentSync.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {

        private readonly IEmployeeService _employeeService;

        public EmployeesController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }


        [Authorize(Roles = "HR")]
        [HttpGet]
        public async Task<IActionResult> GetEmployeesAsync([FromQuery] PaginationRequest paginationRequest, CancellationToken cancellationToken)
        {
            var response = await _employeeService.GetEmployeesAsync(
                paginationRequest,
                cancellationToken);

            return Ok(response);
        }
    }
}
