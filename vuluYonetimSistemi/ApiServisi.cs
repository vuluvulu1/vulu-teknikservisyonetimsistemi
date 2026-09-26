using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace vuluYonetimSistemi
{
    public static class ApiServisi
    {
        private static readonly HttpClient _client;

        public static string? Token { get; set; }
        public static string? KullaniciAdi { get; set; }
        public static string? Rol { get; set; }

        static ApiServisi()
        {
            var handler = new HttpClientHandler
            {
                // Geliştirme ortamında self-signed sertifikayı kabul et.
                // Production'da gerçek sertifika kullanılacaksa bu satır kaldırılmalı.
                ServerCertificateCustomValidationCallback = (msg, cert, chain, errors) => true
            };

            _client = new HttpClient(handler)
            {
                BaseAddress = new Uri("https://localhost:7053/")
            };
        }

        public static async Task<(bool Basarili, string Mesaj)> GirisYap(string kullaniciAdi, string sifre)
        {
            try
            {
                var istek = new { KullaniciAdi = kullaniciAdi, Sifre = sifre };
                var yanit = await _client.PostAsJsonAsync("api/Kullanici/giris", istek);

                if (!yanit.IsSuccessStatusCode)
                {
                    var hata = await yanit.Content.ReadFromJsonAsync<HataYaniti>();
                    return (false, hata?.Hata ?? "Giriş başarısız.");
                }

                var sonuc = await yanit.Content.ReadFromJsonAsync<GirisYaniti>();
                if (sonuc is null) return (false, "Sunucudan geçersiz yanıt.");

                Token = sonuc.Token;
                KullaniciAdi = sonuc.KullaniciAdi;
                Rol = sonuc.Rol;

                _client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", Token);

                return (true, "Giriş başarılı.");
            }
            catch (HttpRequestException)
            {
                return (false, "Sunucuya bağlanılamadı. API çalışıyor mu kontrol et.");
            }
        }

        public static HttpClient Client => _client;

        private record GirisYaniti(string Token, string KullaniciAdi, string Rol);
        private record HataYaniti(string Hata);
    }
}