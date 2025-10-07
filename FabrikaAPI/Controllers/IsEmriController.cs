using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FabrikaAPI.Data;
using FabrikaAPI.Models;

namespace FabrikaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IsEmriController : ControllerBase
    {
        private readonly FabrikaContext _context;

        public IsEmriController(FabrikaContext context)
        {
            _context = context;
        }

        // GET: api/IsEmri
        [HttpGet]
        public async Task<ActionResult<IEnumerable<IsEmri>>> GetIsEmirleri()
        {
            return await _context.IsEmirleri.Include(i => i.Urun).ToListAsync();
        }

        // GET: api/IsEmri/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IsEmri>> GetIsEmri(int id)
        {
            var isEmri = await _context.IsEmirleri
                .Include(i => i.Urun)
                .FirstOrDefaultAsync(i => i.IsEmriID == id);

            if (isEmri == null)
            {
                return NotFound();
            }

            return isEmri;
        }

        // POST: api/IsEmri
        [HttpPost]
        public async Task<ActionResult<IsEmri>> PostIsEmri(IsEmri isEmri)
        {
            _context.IsEmirleri.Add(isEmri);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetIsEmri), new { id = isEmri.IsEmriID }, isEmri);
        }

        // PUT: api/IsEmri/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIsEmri(int id, IsEmri isEmri)
        {
            if (id != isEmri.IsEmriID)
            {
                return BadRequest();
            }

            _context.Entry(isEmri).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IsEmriExists(id))
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

        // DELETE: api/IsEmri/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIsEmri(int id)
        {
            var isEmri = await _context.IsEmirleri.FindAsync(id);
            if (isEmri == null)
            {
                return NotFound();
            }

            _context.IsEmirleri.Remove(isEmri);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool IsEmriExists(int id)
        {
            return _context.IsEmirleri.Any(e => e.IsEmriID == id);
        }
    }
}