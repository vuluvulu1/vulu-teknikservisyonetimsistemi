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
    public record KullaniciOlusturIstegi(string KullaniciAdi, string Sifre, KullaniciRol Rol, string AdminKey);

    [ApiController]
    [Route("api/[controller]")]
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
        public async Task<IActionResult> Giris([FromBody] GirisIstegi istek)
        {
            if (string.IsNullOrWhiteSpace(istek.KullaniciAdi) || string.IsNullOrWhiteSpace(istek.Sifre))
                return BadRequest(new { hata = "Kullanıcı adı ve şifre gerekli." });

            var kullanici = await _db.Kullanicilar
                .FirstOrDefaultAsync(k => k.KullaniciAdi == istek.KullaniciAdi);

            if (kullanici is null)
                return Unauthorized(new { hata = "Kullanıcı bulunamadı." });

            if (!kullanici.AktifMi)
                return StatusCode(403, new { hata = "Hesabınız devre dışı." });

            bool gecerli = BCrypt.Net.BCrypt.Verify(istek.Sifre, kullanici.SifreHash);
            if (!gecerli)
                return Unauthorized(new { hata = "Şifre yanlış." });

            var token = TokenUret(kullanici);

            return Ok(new
            {
                token,
                kullaniciAdi = kullanici.KullaniciAdi,
                rol = kullanici.Rol.ToString()
            });
        }

        [HttpPost("olustur")]
        public async Task<IActionResult> KullaniciOlustur([FromBody] KullaniciOlusturIstegi istek)
        {
            if (istek.AdminKey != _config["AdminKey"])
                return StatusCode(403, new { hata = "Yetkisiz." });

            var varMi = await _db.Kullanicilar.AnyAsync(k => k.KullaniciAdi == istek.KullaniciAdi);
            if (varMi)
                return BadRequest(new { hata = "Bu kullanıcı adı zaten var." });

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