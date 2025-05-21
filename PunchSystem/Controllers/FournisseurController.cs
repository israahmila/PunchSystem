using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PunchSystem.Contracts;
using PunchSystem.Models;
using PunchSystem.Security;
using PunchSystem.Services;
[Authorize(Policy = PermissionPolicies.ManageFournisseurs)]
[ApiController]
[Route("api/[controller]")]
public class FournisseurController : ControllerBase
{
    private readonly IFournisseurService _service;

    public FournisseurController(IFournisseurService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var fournisseurs = await _service.GetAllAsync();
        return Ok(fournisseurs);
    }

    [HttpPost]
    [Authorize(Policy = "CreateFournisseur")]
    public async Task<IActionResult> Create(Fournisseur fournisseur)
    {
        var result = await _service.CreateAsync(fournisseur);
        return CreatedAtAction(nameof(GetAll), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    [Authorize(Policy = "EditFournisseur")]
    public async Task<IActionResult> Update(string id, Fournisseur updated, [FromQuery] string raison)
    {
        if (string.IsNullOrWhiteSpace(raison)) return BadRequest("La raison est obligatoire.");
        var ok = await _service.UpdateAsync(id, updated, raison);
        if (!ok) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "DeleteFournisseur")]
    public async Task<IActionResult> SoftDelete(string id, [FromQuery] string raison)
    {
        if (string.IsNullOrWhiteSpace(raison)) return BadRequest("La raison est obligatoire.");
        var ok = await _service.SoftDeleteAsync(id, raison);
        if (!ok) return NotFound();
        return NoContent();
    }
}
