using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vulu_yonetimsistemi_api.Data;
using vulu_yonetimsistemi_api.Models;

namespace vulu_yonetimsistemi_api.Controllers
{
    public record ServisKaydiIstek(int MusteriId, int? TeknisyenId, string CihazBilgisi, string Sorun, ServisDurumu Durum);

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ServisKaydiController : ControllerBase
    {
        private readonly VeriTabaniBaglami _db;
        public ServisKaydiController(VeriTabaniBaglami db) => _db = db;

        [HttpGet]
        public async Task<IActionResult> Listele() =>
            Ok(await _db.ServisKayitlari.Include(s => s.Musteri).Include(s => s.Teknisyen).ToListAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> Getir(int id)
        {
            var kayit = await _db.ServisKayitlari.Include(s => s.Musteri).Include(s => s.Teknisyen)
                .FirstOrDefaultAsync(s => s.Id == id);
            if (kayit is null) return NotFound(new { hata = "Servis kaydı bulunamadı." });
            return Ok(kayit);
        }

        [HttpPost]
        public async Task<IActionResult> Ekle([FromBody] ServisKaydiIstek istek)
        {
            var musteriVarMi = await _db.Musteriler.AnyAsync(m => m.Id == istek.MusteriId);
            if (!musteriVarMi) return BadRequest(new { hata = "Belirtilen müşteri bulunamadı." });

            var kayit = new ServisKaydi
            {
                MusteriId = istek.MusteriId,
                TeknisyenId = istek.TeknisyenId,
                CihazBilgisi = istek.CihazBilgisi,
                Sorun = istek.Sorun,
                Durum = istek.Durum
            };

            _db.ServisKayitlari.Add(kayit);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(Getir), new { id = kayit.Id }, kayit);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Guncelle(int id, [FromBody] ServisKaydiIstek istek)
        {
            var mevcut = await _db.ServisKayitlari.FindAsync(id);
            if (mevcut is null) return NotFound(new { hata = "Servis kaydı bulunamadı." });

            mevcut.MusteriId = istek.MusteriId;
            mevcut.TeknisyenId = istek.TeknisyenId;
            mevcut.CihazBilgisi = istek.CihazBilgisi;
            mevcut.Sorun = istek.Sorun;

            if (istek.Durum == ServisDurumu.Tamamlandi && mevcut.Durum != ServisDurumu.Tamamlandi)
                mevcut.TamamlanmaTarihi = DateTime.UtcNow;

            mevcut.Durum = istek.Durum;

            await _db.SaveChangesAsync();
            return Ok(mevcut);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Sil(int id)
        {
            var mevcut = await _db.ServisKayitlari.FindAsync(id);
            if (mevcut is null) return NotFound(new { hata = "Servis kaydı bulunamadı." });

            _db.ServisKayitlari.Remove(mevcut);
            await _db.SaveChangesAsync();
            return Ok(new { basarili = true });
        }
    }
}