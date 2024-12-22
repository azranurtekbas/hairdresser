using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using haircare.Models;

namespace haircare.Controllers
{
    [Route("[controller]")]
    public class IslemlerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public IslemlerController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Islems
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Islemler.ToListAsync());
        }

        // GET: Islems/Details/5
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var islem = await _context.Islemler
                .FirstOrDefaultAsync(m => m.IslemsId == id);
            if (islem == null)
            {
                return NotFound();
            }

            return View(islem);
        }

        // GET: Islems/Create
        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Islems/Create
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IslemsId,IslemAd,IslemTur,IslemSuresi,Fiyat")] Islem islem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(islem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(islem);
        }

        // GET: Islems/Edit/5
        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var islem = await _context.Islemler.FindAsync(id);
            if (islem == null)
            {
                return NotFound();
            }
            return View(islem);
        }

        // POST: Islems/Edit/5
        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IslemsId,IslemAd,IslemTur,IslemSuresi,Fiyat")] Islem islem)
        {
            if (id != islem.IslemsId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(islem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IslemExists(islem.IslemsId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(islem);
        }

        // GET: Islems/Delete/5
        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var islem = await _context.Islemler
                .FirstOrDefaultAsync(m => m.IslemsId == id);
            if (islem == null)
            {
                return NotFound();
            }

            return View(islem);
        }

        // POST: Islems/Delete/5
        [HttpPost("Delete/{id}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var islem = await _context.Islemler.FindAsync(id);
            if (islem != null)
            {
                _context.Islemler.Remove(islem);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IslemExists(int id)
        {
            return _context.Islemler.Any(e => e.IslemsId == id);
        }

        // API Methods
        // GET: api/Islemler
        [HttpGet("api")]
        public async Task<ActionResult<IEnumerable<Islem>>> GetIslemler()
        {
            return await _context.Islemler.ToListAsync();
        }

        // GET: api/Islemler/5
        [HttpGet("api/{id}")]
        public async Task<ActionResult<Islem>> GetIslem(int id)
        {
            var islem = await _context.Islemler.FindAsync(id);

            if (islem == null)
            {
                return NotFound();
            }

            return islem;
        }

        // PUT: api/Islemler/5
        [HttpPut("api/{id}")]
        public async Task<IActionResult> PutIslem(int id, Islem islem)
        {
            if (id != islem.IslemsId)
            {
                return BadRequest();
            }

            _context.Entry(islem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IslemExists(id))
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

        // POST: api/Islemler
        [HttpPost("api")]
        public async Task<ActionResult<Islem>> PostIslem(Islem islem)
        {
            _context.Islemler.Add(islem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetIslem", new { id = islem.IslemsId }, islem);
        }

        // DELETE: api/Islemler/5
        [HttpDelete("api/{id}")]
        public async Task<IActionResult> DeleteIslem(int id)
        {
            var islem = await _context.Islemler.FindAsync(id);
            if (islem == null)
            {
                return NotFound();
            }

            _context.Islemler.Remove(islem);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
