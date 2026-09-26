using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using vulu_yonetimsistemi_api.Data;
using vulu_yonetimsistemi_api.Models;

namespace vulu_yonetimsistemi_api.Controllers
{
    public record GirisIstegi(string KullaniciAdi, string Sifre);
    public record KullaniciOlusturIstegi(string KullaniciAdi, string Sifre, KullaniciRol Rol);
    public record KullaniciGuncelleIstegi(string KullaniciAdi, KullaniciRol Rol, bool AktifMi, string? YeniSifre);
    public record KullaniciOzet(int Id, string KullaniciAdi, string Rol, bool AktifMi);

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class KullaniciController : ControllerBase
    {
        private readonly VeriTabaniBaglami _db;
        private readonly IConfiguration _config;

        public KullaniciController(VeriTabaniBaglami db, IConfiguration config)
        {
            _db = db;
            _config = config;
        }

        [HttpPost("giris")]
        [AllowAnonymous]
        public async Task<IActionResult> Giris([FromBody] GirisIstegi istek)
        {
            if (string.IsNullOrWhiteSpace(istek.KullaniciAdi) || string.IsNullOrWhiteSpace(istek.Sifre))
                return BadRequest(new { hata = "Kullanıcı adı ve şifre gerekli." });

            var kullanici = await _db.Kullanicilar.FirstOrDefaultAsync(k => k.KullaniciAdi == istek.KullaniciAdi);
            if (kullanici is null) return Unauthorized(new { hata = "Kullanıcı bulunamadı." });
            if (!kullanici.AktifMi) return StatusCode(403, new { hata = "Hesabınız devre dışı." });
            if (!BCrypt.Net.BCrypt.Verify(istek.Sifre, kullanici.SifreHash)) return Unauthorized(new { hata = "Şifre yanlış." });

            var token = TokenUret(kullanici);
            return Ok(new { token, kullaniciAdi = kullanici.KullaniciAdi, rol = kullanici.Rol.ToString() });
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Listele()
        {
            var kullanicilar = await _db.Kullanicilar
                .Select(k => new KullaniciOzet(k.Id, k.KullaniciAdi, k.Rol.ToString(), k.AktifMi))
                .ToListAsync();
            return Ok(kullanicilar);
        }

        [HttpPost("olustur")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> KullaniciOlustur([FromBody] KullaniciOlusturIstegi istek)
        {
            var varMi = await _db.Kullanicilar.AnyAsync(k => k.KullaniciAdi == istek.KullaniciAdi);
            if (varMi) return BadRequest(new { hata = "Bu kullanıcı adı zaten var." });

            var kullanici = new Kullanici
            {
                KullaniciAdi = istek.KullaniciAdi,
                SifreHash = BCrypt.Net.BCrypt.HashPassword(istek.Sifre),
                Rol = istek.Rol
            };

            _db.Kullanicilar.Add(kullanici);
            await _db.SaveChangesAsync();
            return Ok(new { basarili = true, kullaniciAdi = kullanici.KullaniciAdi });
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Guncelle(int id, [FromBody] KullaniciGuncelleIstegi istek)
        {
            var mevcut = await _db.Kullanicilar.FindAsync(id);
            if (mevcut is null) return NotFound(new { hata = "Kullanıcı bulunamadı." });

            var baskaVarMi = await _db.Kullanicilar.AnyAsync(k => k.KullaniciAdi == istek.KullaniciAdi && k.Id != id);
            if (baskaVarMi) return BadRequest(new { hata = "Bu kullanıcı adı başka bir hesapta kullanılıyor." });

            mevcut.KullaniciAdi = istek.KullaniciAdi;
            mevcut.Rol = istek.Rol;
            mevcut.AktifMi = istek.AktifMi;

            if (!string.IsNullOrWhiteSpace(istek.YeniSifre))
                mevcut.SifreHash = BCrypt.Net.BCrypt.HashPassword(istek.YeniSifre);

            await _db.SaveChangesAsync();
            return Ok(new { basarili = true });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Sil(int id)
        {
            var kullaniliyorMu = await _db.Teknisyenler.AnyAsync(t => t.KullaniciId == id);
            if (kullaniliyorMu)
                return BadRequest(new { hata = "Bu kullanıcı bir teknisyene bağlı, önce bağlantıyı kaldırın." });

            var mevcut = await _db.Kullanicilar.FindAsync(id);
            if (mevcut is null) return NotFound(new { hata = "Kullanıcı bulunamadı." });

            _db.Kullanicilar.Remove(mevcut);
            await _db.SaveChangesAsync();
            return Ok(new { basarili = true });
        }

        private string TokenUret(Kullanici kullanici)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, kullanici.KullaniciAdi),
                new(ClaimTypes.Role, kullanici.Rol.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}