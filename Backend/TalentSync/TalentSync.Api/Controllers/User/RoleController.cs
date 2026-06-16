using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TalentSync.Application.Interfaces.Services;

namespace TalentSync.Api.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly ILogger<RoleController> _logger;
        private readonly IRoleService _roleService;
        public RoleController(ILogger<RoleController> logger, IRoleService roleService)
        {
            _logger = logger;
            _roleService = roleService;
        }
        [HttpGet]
        public async  Task<IActionResult> GetAllRolesAsync()
        {
            List<string> roles = await _roleService.GetAllRolesAsync();
            return Ok(roles);
        }
    }
}
