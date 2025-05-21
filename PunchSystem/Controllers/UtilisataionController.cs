using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PunchSystem.DTOs;
using PunchSystem.Models;
using AutoMapper;
using PunchSystem.Contracts;
using PunchSystem.Security;

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
        [Authorize(Policy = PermissionPolicies.CreateUtilisation)]
        public async Task<ActionResult<UtilisationDto>> Create([FromBody] CreateUtilisationDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var utilisation = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetAll), new { id = utilisation.Id }, utilisation);
        }



        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var utilisation = await _service.GetByIdAsync(id);
            if (utilisation == null) return NotFound();
            return Ok(utilisation);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, UpdateUtilisationDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updated = await _service.UpdateAsync(id, dto); // ✅ méthode doit retourner bool

            if (!updated)
                return NotFound();

            return NoContent();
        }






    }
}
