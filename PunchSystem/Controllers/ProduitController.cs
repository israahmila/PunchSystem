using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PunchSystem.Contracts;
using PunchSystem.Models;
using PunchSystem.Services;

[ApiController]
[Route("api/[controller]")]
public class ProduitController : ControllerBase
{
    private readonly IProduitService _service;

    public ProduitController(IProduitService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var produits = await _service.GetAllAsync();
        return Ok(produits);
    }

    [HttpPost]
    [Authorize(Policy = "CreateProduit")]
    public async Task<IActionResult> Create(Produit produit)
    {
        var result = await _service.CreateAsync(produit);
        return CreatedAtAction(nameof(GetAll), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    [Authorize(Policy = "EditProduit")]
    public async Task<IActionResult> Update(string id, [FromBody] Produit updated, [FromQuery] string raison)
    {
        if (string.IsNullOrWhiteSpace(raison)) return BadRequest("La raison est obligatoire.");
        var ok = await _service.UpdateAsync(id, updated, raison);
        if (!ok) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "DeleteProduit")]
    public async Task<IActionResult> SoftDelete(string id, [FromQuery] string raison)
    {
        if (string.IsNullOrWhiteSpace(raison)) return BadRequest("La raison est obligatoire.");
        var ok = await _service.SoftDeleteAsync(id, raison);
        if (!ok) return NotFound();
        return NoContent();
    }
}
