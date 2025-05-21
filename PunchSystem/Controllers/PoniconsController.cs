using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PunchSystem.Contracts;
using PunchSystem.Models;
using PunchSystem.Security;
using PunchSystem.Services;
[Authorize(Policy = PermissionPolicies.ManagePoincons)]
[ApiController]
[Route("api/[controller]")]
public class PoinconController : ControllerBase
{
    private readonly IPoinconService _service;

    public PoinconController(IPoinconService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var poincons = await _service.GetAllAsync();
        return Ok(poincons);
    }

    [HttpPost]
    [Authorize(Policy = "CreatePoincon")]
    public async Task<IActionResult> Create(Poincon poincon)
    {
        var result = await _service.CreateAsync(poincon);
        return CreatedAtAction(nameof(GetAll), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    [Authorize(Policy = "EditPoincon")]
    public async Task<IActionResult> Update(string id, Poincon updated, [FromQuery] string raison)
    {
        if (string.IsNullOrWhiteSpace(raison)) return BadRequest("La raison est obligatoire.");
        var ok = await _service.UpdateAsync(id, updated, raison);
        if (!ok) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "DeletePoincon")]
    public async Task<IActionResult> SoftDelete(string id, [FromQuery] string raison)
    {
        if (string.IsNullOrWhiteSpace(raison)) return BadRequest("La raison est obligatoire.");
        var ok = await _service.SoftDeleteAsync(id, raison);
        if (!ok) return NotFound();
        return NoContent();
    }
}
