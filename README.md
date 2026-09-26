# vulu Teknik Servis Yönetim Sistemi

Teknik servis süreçlerini yönetmek için geliştirilen C# / .NET tabanlı masaüstü uygulaması.

Müşteri, teknisyen, servis kaydı, randevu ve fatura gibi işlemleri tek bir sistem üzerinden yönetmeyi hedefliyor.

## Özellikler

* Müşteri kayıtları
* Teknisyen kayıtları
* Servis kaydı oluşturma ve takip
* Randevu yönetimi
* Fatura işlemleri
* Kullanıcı yönetimi
* Admin / Teknisyen rolleri

## Kullanılan Teknolojiler

* C# / .NET 10
* Windows Forms
* ASP.NET Core Web API
* Entity Framework Core 10
* SQL Server
* JWT
* BCrypt.Net
* Swagger / OpenAPI

## Proje Yapısı

```text
vuluYonetimSistemi
    └── Windows Forms Client

vulu-yonetimsistemi-api
    ├── Controllers
    ├── Models
    ├── Data
    └── Migrations
```

Client ve API ayrı projeler olarak geliştiriliyor.

## Durum

* [x] Kullanıcı girişi
* [x] Kullanıcı yönetimi
* [x] Müşteri yönetimi
* [x] Teknisyen yönetimi
* [x] Servis kaydı API
* [x] Randevu API
* [x] Fatura API
* [ ] Servis kaydı arayüzü
* [ ] Randevu arayüzü
* [ ] Fatura arayüzü

Şu anda WinForms tarafındaki yönetim ekranları geliştiriliyor. Müşteri, teknisyen ve kullanıcı ekranlarının temel listeleme, ekleme, düzenleme ve silme işlemleri çalışıyor.

> Proje geliştirme aşamasındadır.
