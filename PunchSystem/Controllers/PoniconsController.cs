using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PunchSystem.Data;
using PunchSystem.DTOs;
using PunchSystem.Models;
using Newtonsoft.Json;

namespace PunchSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PoinconsController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _environment;

    public PoinconsController(AppDbContext context, IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
    }

    // 📥 BATCH CREATE
    [HttpPost("batch")]
    public async Task<IActionResult> BatchCreate([FromBody] List<PoinconCreateDto> poincons)
    {
        if (!ModelState.IsValid)
        {
            Console.WriteLine("[VALIDATION ERROR] " + JsonConvert.SerializeObject(ModelState));
            return BadRequest(ModelState);
        }

        if (poincons == null || poincons.Count == 0)
            return BadRequest(new { message = "No poinçons provided" });
        if (poincons == null || poincons.Count == 0)
            return BadRequest(new { message = "No poinçons provided" });

        var newPoincons = poincons.Select(dto => new Poincon
        {
            CodeFormat = dto.CodeFormat,
            Forme = dto.Forme,
            MarqueId = dto.MarqueId,
            CodeGMAO = dto.CodeGMAO,
            FournisseurId = dto.FournisseurId,
            GravureSup = dto.GravureSup,
            GravureInf = dto.GravureInf,
            Secabilite = dto.Secabilite,
            Clavetage = dto.Clavetage,
            EmplacementReception = dto.EmplacementReception,
            DateReception = dto.DateReception,
            DateFabrication = dto.DateFabrication,
            DateMiseEnService = dto.DateMiseEnService,
            Commentaire = dto.Commentaire,
            FicheTechniqueUrl = dto.FicheTechniqueUrl,
            Matrice = dto.Matrice,
            Largeur = dto.Largeur,
            Longueur = dto.Longueur,
            RefSup = dto.RefSup,
            RefInf = dto.RefInf,
            Diametre = dto.Diametre,
            ChAdm = dto.ChAdm,
            Statut = dto.Status
        }).ToList();

        await _context.Poincons.AddRangeAsync(newPoincons);
        await _context.SaveChangesAsync();

        return StatusCode(201); // 201 Created
    }

    [HttpPost("upload")]
    [Consumes("multipart/form-data")] // ✅ Required for Swagger to understand file uploads
    public async Task<IActionResult> UploadFile([FromForm] FileUploadDto dto)
    {
        var file = dto.File;

        if (file == null || file.Length == 0)
            return BadRequest(new { message = "No file uploaded" });

        var uploadsPath = Path.Combine(_environment.WebRootPath, "uploads");

        if (!Directory.Exists(uploadsPath))
            Directory.CreateDirectory(uploadsPath);

        var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
        var filePath = Path.Combine(uploadsPath, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var fileUrl = $"{Request.Scheme}://{Request.Host}/uploads/{fileName}";

        return Ok(new { url = fileUrl });
    }


    // 📄 GET ALL POINCONS
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var poincons = await _context.Poincons.ToListAsync();
        return Ok(poincons);
    }

    // 📄 GET SINGLE POINCON
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var poincon = await _context.Poincons.FindAsync(id);

        if (poincon == null)
            return NotFound();

        return Ok(poincon);
    }

    // ✏️ UPDATE POINCON
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] PoinconUpdateDto dto)
    {
        var poincon = await _context.Poincons.FindAsync(id);

        if (poincon == null)
            return NotFound();

        poincon.CodeFormat = dto.CodeFormat;
        poincon.Forme = dto.Forme;
        poincon.MarqueId = dto.MarqueId;
        poincon.CodeGMAO = dto.CodeGMAO;
        poincon.FournisseurId = dto.FournisseurId;
        poincon.GravureSup = dto.GravureSup;
        poincon.GravureInf = dto.GravureInf;
        poincon.Secabilite = dto.Secabilite;
        poincon.Clavetage = dto.Clavetage;
        poincon.EmplacementReception = dto.EmplacementReception;
        poincon.DateReception = dto.DateReception;
        poincon.DateFabrication = dto.DateFabrication;
        poincon.DateMiseEnService = dto.DateMiseEnService;
        poincon.Commentaire = dto.Commentaire;
        poincon.Matrice = dto.Matrice;
        poincon.Largeur = dto.Largeur;
        poincon.Longueur = dto.Longueur;
        poincon.RefSup = dto.RefSup;
        poincon.RefInf = dto.RefInf;
        poincon.Diametre = dto.Diametre;
        poincon.ChAdm = dto.ChAdm;
        poincon.Statut = dto.Status;
        if (!string.IsNullOrWhiteSpace(dto.FicheTechniqueUrl))
            poincon.FicheTechniqueUrl = dto.FicheTechniqueUrl;

        await _context.SaveChangesAsync();
        return NoContent(); // 204
    }

    // ❌ No real delete, just soft-delete (Status = "inactive")
    [HttpDelete("{id}")]
    public async Task<IActionResult> SoftDelete(int id)
    {
        var poincon = await _context.Poincons.FindAsync(id);

        if (poincon == null)
            return NotFound();

        poincon.Statut = "inactive"; // ✅ Soft delete by setting status
        await _context.SaveChangesAsync();

        return NoContent(); // 204
    }

}
