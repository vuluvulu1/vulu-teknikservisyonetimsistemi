using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vulu_yonetimsistemi_api.Data;
using vulu_yonetimsistemi_api.Models;

namespace vulu_yonetimsistemi_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TeknisyenController : ControllerBase
    {
        private readonly VeriTabaniBaglami _db;
        public TeknisyenController(VeriTabaniBaglami db) => _db = db;

        [HttpGet]
        public async Task<IActionResult> Listele() => Ok(await _db.Teknisyenler.ToListAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> Getir(int id)
        {
            var teknisyen = await _db.Teknisyenler.FindAsync(id);
            if (teknisyen is null) return NotFound(new { hata = "Teknisyen bulunamadı." });
            return Ok(teknisyen);
        }

        [HttpPost]
        public async Task<IActionResult> Ekle([FromBody] Teknisyen teknisyen)
        {
            _db.Teknisyenler.Add(teknisyen);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(Getir), new { id = teknisyen.Id }, teknisyen);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Guncelle(int id, [FromBody] Teknisyen teknisyen)
        {
            var mevcut = await _db.Teknisyenler.FindAsync(id);
            if (mevcut is null) return NotFound(new { hata = "Teknisyen bulunamadı." });

            mevcut.AdSoyad = teknisyen.AdSoyad;
            mevcut.Telefon = teknisyen.Telefon;
            mevcut.Uzmanlik = teknisyen.Uzmanlik;
            mevcut.KullaniciId = teknisyen.KullaniciId;

            await _db.SaveChangesAsync();
            return Ok(mevcut);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Sil(int id)
        {
            var mevcut = await _db.Teknisyenler.FindAsync(id);
            if (mevcut is null) return NotFound(new { hata = "Teknisyen bulunamadı." });

            _db.Teknisyenler.Remove(mevcut);
            await _db.SaveChangesAsync();
            return Ok(new { basarili = true });
        }
    }
}