using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vulu_yonetimsistemi_api.Data;
using vulu_yonetimsistemi_api.Models;

namespace vulu_yonetimsistemi_api.Controllers
{
    public record FaturaIstek(int ServisKaydiId, decimal Tutar, bool OdendiMi);

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class FaturaController : ControllerBase
    {
        private readonly VeriTabaniBaglami _db;
        public FaturaController(VeriTabaniBaglami db) => _db = db;

        [HttpGet]
        public async Task<IActionResult> Listele() =>
            Ok(await _db.Faturalar.Include(f => f.ServisKaydi).ToListAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> Getir(int id)
        {
            var fatura = await _db.Faturalar.Include(f => f.ServisKaydi).FirstOrDefaultAsync(f => f.Id == id);
            if (fatura is null) return NotFound(new { hata = "Fatura bulunamadı." });
            return Ok(fatura);
        }

        [HttpPost]
        public async Task<IActionResult> Ekle([FromBody] FaturaIstek istek)
        {
            var servisVarMi = await _db.ServisKayitlari.AnyAsync(s => s.Id == istek.ServisKaydiId);
            if (!servisVarMi) return BadRequest(new { hata = "Belirtilen servis kaydı bulunamadı." });

            var faturaVarMi = await _db.Faturalar.AnyAsync(f => f.ServisKaydiId == istek.ServisKaydiId);
            if (faturaVarMi) return BadRequest(new { hata = "Bu servis kaydı için zaten fatura oluşturulmuş." });

            var fatura = new Fatura
            {
                ServisKaydiId = istek.ServisKaydiId,
                Tutar = istek.Tutar,
                OdendiMi = istek.OdendiMi,
                OdemeTarihi = istek.OdendiMi ? DateTime.UtcNow : null
            };

            _db.Faturalar.Add(fatura);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(Getir), new { id = fatura.Id }, fatura);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Guncelle(int id, [FromBody] FaturaIstek istek)
        {
            var mevcut = await _db.Faturalar.FindAsync(id);
            if (mevcut is null) return NotFound(new { hata = "Fatura bulunamadı." });

            mevcut.Tutar = istek.Tutar;

            if (istek.OdendiMi && !mevcut.OdendiMi)
                mevcut.OdemeTarihi = DateTime.UtcNow;
            else if (!istek.OdendiMi)
                mevcut.OdemeTarihi = null;

            mevcut.OdendiMi = istek.OdendiMi;

            await _db.SaveChangesAsync();
            return Ok(mevcut);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Sil(int id)
        {
            var mevcut = await _db.Faturalar.FindAsync(id);
            if (mevcut is null) return NotFound(new { hata = "Fatura bulunamadı." });

            _db.Faturalar.Remove(mevcut);
            await _db.SaveChangesAsync();
            return Ok(new { basarili = true });
        }
    }
}