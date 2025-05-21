using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PunchSystem.Contracts;
using PunchSystem.Security;
using PunchSystem.Services;
[Authorize(Policy = PermissionPolicies.ViewAudit)]
[ApiController]
[Route("api/[controller]")]
public class AuditTrailController : ControllerBase
{
    private readonly IAuditTrailService _service;

    public AuditTrailController(IAuditTrailService service)
    {
        _service = service;
    }

    [HttpGet]
    [Authorize(Policy = "ViewAudit")]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? module,
        [FromQuery] string? utilisateur,
        [FromQuery] DateTime? from,
        [FromQuery] DateTime? to)
    {
        var result = await _service.GetAllAsync(module, utilisateur, from, to);
        return Ok(result);
    }
}
