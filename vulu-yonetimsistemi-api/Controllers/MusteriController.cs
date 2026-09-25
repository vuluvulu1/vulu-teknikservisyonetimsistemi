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
    public class MusteriController : ControllerBase
    {
        private readonly VeriTabaniBaglami _db;

        public MusteriController(VeriTabaniBaglami db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> Listele()
        {
            var musteriler = await _db.Musteriler.ToListAsync();
            return Ok(musteriler);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Getir(int id)
        {
            var musteri = await _db.Musteriler.FindAsync(id);
            if (musteri is null) return NotFound(new { hata = "Müşteri bulunamadı." });
            return Ok(musteri);
        }

        [HttpPost]
        public async Task<IActionResult> Ekle([FromBody] Musteri musteri)
        {
            _db.Musteriler.Add(musteri);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(Getir), new { id = musteri.Id }, musteri);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Guncelle(int id, [FromBody] Musteri musteri)
        {
            var mevcut = await _db.Musteriler.FindAsync(id);
            if (mevcut is null) return NotFound(new { hata = "Müşteri bulunamadı." });

            mevcut.AdSoyad = musteri.AdSoyad;
            mevcut.Telefon = musteri.Telefon;
            mevcut.Email = musteri.Email;
            mevcut.Adres = musteri.Adres;

            await _db.SaveChangesAsync();
            return Ok(mevcut);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Sil(int id)
        {
            var mevcut = await _db.Musteriler.FindAsync(id);
            if (mevcut is null) return NotFound(new { hata = "Müşteri bulunamadı." });

            _db.Musteriler.Remove(mevcut);
            await _db.SaveChangesAsync();
            return Ok(new { basarili = true });
        }
    }
}