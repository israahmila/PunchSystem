using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PunchSystem.DTOs;
using PunchSystem.Models;
using PunchSystem.Services;

namespace PunchSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UtilisataionController :ControllerBase
    {
        private readonly IUtilisationService _service;

        public UtilisataionController(IUtilisationService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UtilisationDto>>> GetAll()
        {
            var utilisations = await _service.GetAllAsync();
            return Ok(utilisations);
        }

        [HttpPost]
        public async Task<ActionResult<UtilisationDto>> Create([FromBody] CreateUtilisationDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var utilisation = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetAll), new { id = utilisation.Id }, utilisation);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var utilisation = await _service.GetByIdAsync(id);
            if (utilisation == null) return NotFound();
            return Ok(utilisation);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateUtilisationDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var updated = await _service.UpdateAsync(id, dto);
            if (!updated) return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted) return NotFound();

            return NoContent();
        }



    }
}
