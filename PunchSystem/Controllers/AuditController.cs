using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PunchSystem.Contracts;
using PunchSystem.Models;
using PunchSystem.Security;

namespace PunchSystem.Controllers
{
    [Authorize(Policy = PermissionPolicies.ViewAudit)]
    [ApiController]
    [Route("api/[controller]")]
    public class AuditController : ControllerBase
    {
        private readonly IAuditTrailService _service;

        public AuditController(IAuditTrailService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuditTrail>>> GetAll(
            [FromQuery] string? module,
            [FromQuery] string? utilisateur,
            [FromQuery] DateTime? from,
            [FromQuery] DateTime? to)
        {
            var result = await _service.GetAllAsync(module, utilisateur, from, to);
            return Ok(result);
        }
    }
}
