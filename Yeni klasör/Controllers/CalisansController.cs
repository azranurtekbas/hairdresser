using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using haircare.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace haircare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalisansApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CalisansApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/CalisansApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Calisan>>> GetCalisanlar()
        {
            return await _context.Calisanlar.ToListAsync();
        }

        // GET: api/CalisansApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Calisan>> GetCalisan(int id)
        {
            var calisan = await _context.Calisanlar.FindAsync(id);

            if (calisan == null)
            {
                return NotFound(new { message = "Çalışan bulunamadı." });
            }

            return Ok(calisan);
        }

        // PUT: api/CalisansApi/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCalisan(int id, [FromBody] Calisan calisan)
        {
            // Veriyi doğrula, eğer geçersizse BadRequest döndür
            if (calisan == null)
            {
                return BadRequest(new { message = "Geçersiz çalışan verisi." });
            }

            if (id != calisan.Id)
            {
                return BadRequest(new { message = "Gönderilen ID ile çalışan ID'si uyuşmuyor." });
            }

            // Çalışan var mı diye kontrol et
            var existingCalisan = await _context.Calisanlar.FindAsync(id);
            if (existingCalisan == null)
            {
                return NotFound(new { message = "Güncellenmek istenen çalışan bulunamadı." });
            }

            // Veriyi güncelle
            existingCalisan.CalısanAd = calisan.CalısanAd;
            existingCalisan.CalısanSoyad = calisan.CalısanSoyad;
            existingCalisan.UzmanlikAlani = calisan.UzmanlikAlani;
            existingCalisan.UygunlukSaati = calisan.UygunlukSaati;

            try
            {
                // Değişiklikleri kaydet
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, new { message = "Veritabanı güncellenemedi, lütfen tekrar deneyin." });
            }

            return Ok(new { message = "Çalışan başarıyla güncellendi." });
        }

        // POST: api/CalisansApi
        [HttpPost]
        public async Task<ActionResult<Calisan>> PostCalisan([FromBody] Calisan calisan)
        {
            if (calisan == null)
            {
                return BadRequest(new { message = "Geçersiz çalışan verisi." });
            }

            _context.Calisanlar.Add(calisan);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCalisan", new { id = calisan.Id }, new { message = "Çalışan başarıyla oluşturuldu.", data = calisan });
        }

        // DELETE: api/CalisansApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCalisan(int id)
        {
            var calisan = await _context.Calisanlar.FindAsync(id);
            if (calisan == null)
            {
                return NotFound(new { message = "Çalışan bulunamadı." });
            }

            _context.Calisanlar.Remove(calisan);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Çalışan başarıyla silindi." });
        }

        private bool CalisanExists(int id)
        {
            return _context.Calisanlar.Any(e => e.Id == id);
        }
    }
}
