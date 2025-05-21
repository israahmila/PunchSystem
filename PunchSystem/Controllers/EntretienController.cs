using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PunchSystem.Contracts;
using PunchSystem.Models;
using PunchSystem.Services;

[ApiController]
[Route("api/[controller]")]
public class EntretienController : ControllerBase
{
    private readonly IEntretienService _service;

    public EntretienController(IEntretienService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var entretiens = await _service.GetAllAsync();
        return Ok(entretiens);
    }

    [HttpPost]
    [Authorize(Policy = "CreateEntretien")]
    public async Task<IActionResult> Create(Entretien entretien)
    {
        var created = await _service.CreateAsync(entretien);
        return CreatedAtAction(nameof(GetAll), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    [Authorize(Policy = "EditEntretien")]
    public async Task<IActionResult> Update(string id, Entretien updated, [FromQuery] string raison)
    {
        if (string.IsNullOrWhiteSpace(raison)) return BadRequest("Raison requise");
        var ok = await _service.UpdateAsync(id, updated, raison);
        if (!ok) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "DeleteEntretien")]
    public async Task<IActionResult> Delete(string id, [FromQuery] string raison)
    {
        if (string.IsNullOrWhiteSpace(raison)) return BadRequest("Raison requise");
        var ok = await _service.DeleteAsync(id, raison);
        if (!ok) return NotFound();
        return NoContent();
    }
}
