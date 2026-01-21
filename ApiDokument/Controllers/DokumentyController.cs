using ApiDokument.Data;
using ApiDokument.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiDokument.Controllers
{
    [Authorize]
    [Route("api/dokumenty")]
    [ApiController]
    public class DokumentyController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DokumentyController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Dokument>>> GetDokumenty()
        {
            return await _context.Dokumenty.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Dokument>> GetDokument(Guid id)
        {
            var dokument = await _context.Dokumenty.FindAsync(id);
            if (dokument == null) return NotFound();
            return Ok(dokument);
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Dokument>>> Search([FromQuery] string? query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return await _context.Dokumenty.ToListAsync();
            }

            var result = await _context.Dokumenty
                .Where(d => d.Nazwa.Contains(query) || (d.Opis != null && d.Opis.Contains(query)))
                .ToListAsync();

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<Dokument>> PostDokument(Dokument dokument)
        {
            _context.Dokumenty.Add(dokument);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetDokument), new { id = dokument.Id }, dokument);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutDokument(Guid id, Dokument dokument)
        {
            if (id != dokument.Id) return BadRequest();

            _context.Entry(dokument).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Dokumenty.Any(e => e.Id == id)) return NotFound();
                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDokument(Guid id)
        {
            var dokument = await _context.Dokumenty.FindAsync(id);
            if (dokument == null) return NotFound();

            _context.Dokumenty.Remove(dokument);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}