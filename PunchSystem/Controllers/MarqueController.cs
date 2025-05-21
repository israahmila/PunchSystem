using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PunchSystem.Contracts;
using PunchSystem.Models;
using PunchSystem.Services;

[ApiController]
[Route("api/[controller]")]
public class MarqueController : ControllerBase
{
    private readonly IMarqueService _service;

    public MarqueController(IMarqueService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var marques = await _service.GetAllAsync();
        return Ok(marques);
    }

    [HttpPost]
    [Authorize(Policy = "CreateMarque")]
    public async Task<IActionResult> Create(Marque marque)
    {
        var result = await _service.CreateAsync(marque);
        return CreatedAtAction(nameof(GetAll), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    [Authorize(Policy = "EditMarque")]
    public async Task<IActionResult> Update(string id, Marque updated, [FromQuery] string raison)
    {
        if (string.IsNullOrWhiteSpace(raison)) return BadRequest("La raison est obligatoire.");
        var ok = await _service.UpdateAsync(id, updated, raison);
        if (!ok) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "DeleteMarque")]
    public async Task<IActionResult> SoftDelete(string id, [FromQuery] string raison)
    {
        if (string.IsNullOrWhiteSpace(raison)) return BadRequest("La raison est obligatoire.");
        var ok = await _service.SoftDeleteAsync(id, raison);
        if (!ok) return NotFound();
        return NoContent();
    }
}
