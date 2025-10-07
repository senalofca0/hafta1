using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FabrikaAPI.Data;
using FabrikaAPI.Models;

namespace FabrikaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UrunTipiController : ControllerBase
    {
        private readonly FabrikaContext _context;

        public UrunTipiController(FabrikaContext context)
        {
            _context = context;
        }

        // GET: api/UrunTipi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UrunTipi>>> GetUrunTipleri()
        {
            return await _context.UrunTipleri.ToListAsync();
        }

        // GET: api/UrunTipi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UrunTipi>> GetUrunTipi(int id)
        {
            var urunTipi = await _context.UrunTipleri
                .Include(u => u.Urunler)
                .FirstOrDefaultAsync(u => u.UrunTipiID == id);

            if (urunTipi == null)
            {
                return NotFound();
            }

            return urunTipi;
        }

        // POST: api/UrunTipi
        [HttpPost]
        public async Task<ActionResult<UrunTipi>> PostUrunTipi(UrunTipi urunTipi)
        {
            _context.UrunTipleri.Add(urunTipi);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUrunTipi), new { id = urunTipi.UrunTipiID }, urunTipi);
        }

        // PUT: api/UrunTipi/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUrunTipi(int id, UrunTipi urunTipi)
        {
            if (id != urunTipi.UrunTipiID)
            {
                return BadRequest();
            }

            _context.Entry(urunTipi).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UrunTipiExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/UrunTipi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUrunTipi(int id)
        {
            var urunTipi = await _context.UrunTipleri.FindAsync(id);
            if (urunTipi == null)
            {
                return NotFound();
            }

            _context.UrunTipleri.Remove(urunTipi);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UrunTipiExists(int id)
        {
            return _context.UrunTipleri.Any(e => e.UrunTipiID == id);
        }
    }
}