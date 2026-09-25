# Tech Service Management System (TSMS)

Teknik servis süreçlerini (müşteri, teknisyen, servis kaydı, randevu, fatura)
yönetmek için geliştirilen, C# .NET tabanlı masaüstü uygulaması.

## Mimari

```
vuluYonetimSistemi (WinForms Client)  →  HTTPS/JWT  →  vulu-yonetimsistemi-api  →  SQL Server
```

İstemci veritabanına asla doğrudan bağlanmaz; tüm veri erişimi HTTPS üzerinden,
JWT ile doğrulanan Web API üzerinden yapılır.

## Teknoloji Yığını

- **.NET 10** (LTS)
- **WinForms** — masaüstü istemci
- **ASP.NET Core Web API** (controller-based) — backend
- **Entity Framework Core 10** — ORM
- **SQL Server** (geliştirmede LocalDB)
- **JWT Bearer Authentication**
- **BCrypt.Net** — şifre hash'leme

## Proje Yapısı

```
/vuluYonetimSistemi          → WinForms client projesi
/vulu-yonetimsistemi-api     → ASP.NET Core Web API
  /Models                    → Kullanici, Musteri, Teknisyen, ServisKaydi, Randevu, Fatura
  /Data                      → VeriTabaniBaglami (DbContext)
  /Controllers               → Kullanici, Musteri, Teknisyen, ServisKaydi, Randevu, Fatura
  /Migrations                → EF Core migration'ları
```

## Veri Modeli

- **Kullanici** — sistem girişi yapan kullanıcılar (Admin / Teknisyen rolleri)
- **Musteri** — servise cihaz getiren müşteriler
- **Teknisyen** — servisi gerçekleştiren personel
- **ServisKaydi** — bir müşterinin cihazı için açılan servis kaydı (Beklemede / Devam / Tamamlandı / İptal)
- **Randevu** — müşteri/teknisyen için planlanan randevular
- **Fatura** — tamamlanan servis kayıtları için oluşturulan faturalar

## API Endpoint'leri

Tüm endpoint'ler `/api/Kullanici/giris` hariç JWT token gerektirir
(`Authorization: Bearer <token>`).

| Yöntem | Endpoint | Açıklama |
|---|---|---|
| POST | `/api/Kullanici/giris` | Giriş yapar, JWT token döner |
| POST | `/api/Kullanici/olustur` | Yeni kullanıcı oluşturur (admin anahtarı gerekir) |
| GET/POST/PUT/DELETE | `/api/Musteri` | Müşteri CRUD |
| GET/POST/PUT/DELETE | `/api/Teknisyen` | Teknisyen CRUD |
| GET/POST/PUT/DELETE | `/api/ServisKaydi` | Servis kaydı CRUD |
| GET/POST/PUT/DELETE | `/api/Randevu` | Randevu CRUD |
| GET/POST/PUT/DELETE | `/api/Fatura` | Fatura CRUD |

## Kurulum

1. `vulu-yonetimsistemi-api/appsettings.json` içinde `ConnectionStrings:DefaultConnection`,
   `Jwt:Key`, `Jwt:Issuer`, `Jwt:Audience` ve `AdminKey` değerlerini kendi ortamına göre ayarla.
2. Package Manager Console'da (startup project: `vulu-yonetimsistemi-api`):
   ```
   Update-Database
   ```
3. `vulu-yonetimsistemi-api`'yi çalıştır (API ayağa kalkar).
4. `vuluYonetimSistemi`'yi startup project yapıp çalıştır (client).

## Güvenlik Notları

- `appsettings.json` içindeki `Jwt:Key` ve `AdminKey` gerçek/production değerleri
  repoya commit edilmemeli — `.gitignore`'a eklenmeli veya `dotnet user-secrets` kullanılmalı.
- İstemci hiçbir zaman veritabanı bağlantı bilgisi taşımaz, sadece API'den aldığı JWT token'ı taşır.

## Durum

Şu an API tarafı (auth + tüm CRUD controller'lar) tamamlandı ve test edildi.
Sırada WinForms client tarafının (login ekranı ve diğer arayüzler) geliştirilmesi var.
