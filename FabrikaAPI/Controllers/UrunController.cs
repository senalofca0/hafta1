using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FabrikaAPI.Data;
using FabrikaAPI.Models;

namespace FabrikaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UrunController : ControllerBase
    {
        private readonly FabrikaContext _context;

        public UrunController(FabrikaContext context)
        {
            _context = context;
        }

        // GET: api/Urun
        /// <summary>
        /// Tüm ürünleri getirir
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Urun>>> GetUrunler()
        {
            return await _context.Urunler.ToListAsync();
        }

        // GET: api/Urun/5
        /// <summary>
        /// Belirtilen ID'ye sahip ürünü getirir
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Urun>> GetUrun(int id)
        {
            var urun = await _context.Urunler.FindAsync(id);

            if (urun == null)
            {
                return NotFound();
            }

            return urun;
        }

        // POST: api/Urun
        /// <summary>
        /// Yeni bir ürün ekler
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Urun>> PostUrun(Urun urun)
        {
            _context.Urunler.Add(urun);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUrun), new { id = urun.UrunID }, urun);
        }

        // PUT: api/Urun/5
        /// <summary>
        /// Belirtilen ID'ye sahip ürünü günceller
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUrun(int id, Urun urun)
        {
            if (id != urun.UrunID)
            {
                return BadRequest();
            }

            _context.Entry(urun).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UrunExists(id))
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

        // DELETE: api/Urun/5
        /// <summary>
        /// Belirtilen ID'ye sahip ürünü siler
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUrun(int id)
        {
            var urun = await _context.Urunler.FindAsync(id);
            if (urun == null)
            {
                return NotFound();
            }

            _context.Urunler.Remove(urun);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UrunExists(int id)
        {
            return _context.Urunler.Any(e => e.UrunID == id);
        }
    }
}