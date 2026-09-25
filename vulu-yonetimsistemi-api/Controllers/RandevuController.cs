using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vulu_yonetimsistemi_api.Data;
using vulu_yonetimsistemi_api.Models;

namespace vulu_yonetimsistemi_api.Controllers
{
    public record RandevuIstek(int MusteriId, int? TeknisyenId, int? ServisKaydiId, DateTime RandevuTarihi, string? Aciklama, RandevuDurumu Durum);

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RandevuController : ControllerBase
    {
        private readonly VeriTabaniBaglami _db;
        public RandevuController(VeriTabaniBaglami db) => _db = db;

        [HttpGet]
        public async Task<IActionResult> Listele() =>
            Ok(await _db.Randevular.Include(r => r.Musteri).Include(r => r.Teknisyen).ToListAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> Getir(int id)
        {
            var randevu = await _db.Randevular.Include(r => r.Musteri).Include(r => r.Teknisyen)
                .FirstOrDefaultAsync(r => r.Id == id);
            if (randevu is null) return NotFound(new { hata = "Randevu bulunamadı." });
            return Ok(randevu);
        }

        [HttpPost]
        public async Task<IActionResult> Ekle([FromBody] RandevuIstek istek)
        {
            var musteriVarMi = await _db.Musteriler.AnyAsync(m => m.Id == istek.MusteriId);
            if (!musteriVarMi) return BadRequest(new { hata = "Belirtilen müşteri bulunamadı." });

            var randevu = new Randevu
            {
                MusteriId = istek.MusteriId,
                TeknisyenId = istek.TeknisyenId,
                ServisKaydiId = istek.ServisKaydiId,
                RandevuTarihi = istek.RandevuTarihi,
                Aciklama = istek.Aciklama,
                Durum = istek.Durum
            };

            _db.Randevular.Add(randevu);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(Getir), new { id = randevu.Id }, randevu);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Guncelle(int id, [FromBody] RandevuIstek istek)
        {
            var mevcut = await _db.Randevular.FindAsync(id);
            if (mevcut is null) return NotFound(new { hata = "Randevu bulunamadı." });

            mevcut.MusteriId = istek.MusteriId;
            mevcut.TeknisyenId = istek.TeknisyenId;
            mevcut.ServisKaydiId = istek.ServisKaydiId;
            mevcut.RandevuTarihi = istek.RandevuTarihi;
            mevcut.Aciklama = istek.Aciklama;
            mevcut.Durum = istek.Durum;

            await _db.SaveChangesAsync();
            return Ok(mevcut);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Sil(int id)
        {
            var mevcut = await _db.Randevular.FindAsync(id);
            if (mevcut is null) return NotFound(new { hata = "Randevu bulunamadı." });

            _db.Randevular.Remove(mevcut);
            await _db.SaveChangesAsync();
            return Ok(new { basarili = true });
        }
    }
}