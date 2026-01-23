using ApiDokument.Data;
using ApiDokument.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public async Task<ActionResult<IEnumerable<Dokument>>> GetDokumenty([FromQuery] bool? maWieleWersji)
        {
            var query = _context.Dokumenty.AsQueryable();

            if (maWieleWersji == true)
            {
                query = query.Where(d => _context.DocumentVersions.Count(v => v.DokumentId == d.Id) >= 2);
            }

            return await query.ToListAsync();
        }

        [HttpPost("{id}/wersje")]
        public async Task<ActionResult<DokumentVersion>> PostVersion(Guid id, [FromBody] NewVersionRequest request)
        {
            var dokument = await _context.Dokumenty.FindAsync(id);
            if (dokument == null) return NotFound();

            var ostatniNumer = await _context.DocumentVersions
                .Where(v => v.DokumentId == id)
                .OrderByDescending(v => v.NumerWersji)
                .Select(v => v.NumerWersji)
                .FirstOrDefaultAsync();

            var nowaWersja = new DokumentVersion
            {
                DokumentId = id,
                NumerWersji = ostatniNumer + 1,
                SciezkaPliku = request.SciezkaPliku,
                TypPliku = request.TypPliku,
                Opis = request.Opis,
                DataDodania = DateTime.Now
            };

            dokument.SciezkaPliku = request.SciezkaPliku;
            dokument.TypPliku = request.TypPliku;

            _context.DocumentVersions.Add(nowaWersja);
            await _context.SaveChangesAsync();

            return Ok(nowaWersja);
        }

        [HttpGet("{id}/wersje")]
        public async Task<ActionResult<IEnumerable<DokumentVersion>>> GetVersions(Guid id)
        {
            var dokumentExists = await _context.Dokumenty.AnyAsync(d => d.Id == id);
            if (!dokumentExists) return NotFound();

            return await _context.DocumentVersions
                .Where(v => v.DokumentId == id)
                .OrderByDescending(v => v.NumerWersji)
                .ToListAsync();
        }

        [HttpGet("{id}/wersje/{numer}")]
        public async Task<ActionResult<DokumentVersion>> GetVersionByNumber(Guid id, int numer)
        {
            var wersja = await _context.DocumentVersions
                .FirstOrDefaultAsync(v => v.DokumentId == id && v.NumerWersji == numer);

            if (wersja == null) return NotFound();
            return Ok(wersja);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Dokument>> GetDokument(Guid id)
        {
            var dokument = await _context.Dokumenty.FindAsync(id);
            if (dokument == null) return NotFound();
            return Ok(dokument);
        }

        [HttpPost]
        public async Task<ActionResult<Dokument>> PostDokument(Dokument dokument)
        {
            _context.Dokumenty.Add(dokument);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetDokument), new { id = dokument.Id }, dokument);
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Dokument>>> Search([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query)) return await _context.Dokumenty.ToListAsync();

            return await _context.Dokumenty
                .Where(d => d.Nazwa.Contains(query) || (d.Opis != null && d.Opis.Contains(query)))
                .ToListAsync();
        }
    }

    public class NewVersionRequest
    {
        public string SciezkaPliku { get; set; } = string.Empty;
        public string TypPliku { get; set; } = string.Empty;
        public string? Opis { get; set; }
    }
}