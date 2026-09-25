# vulu Teknik Servis Yönetim Sistemi

Teknik servis süreçlerini yönetmek için geliştirilen C# / .NET tabanlı masaüstü uygulaması.

Müşteri, teknisyen, servis kaydı, randevu ve fatura gibi işlemleri tek bir sistem üzerinden yönetmeyi hedefliyor.

## Özellikler

* Müşteri kayıtları
* Teknisyen kayıtları
* Servis kaydı oluşturma ve takip
* Randevu yönetimi
* Fatura işlemleri
* Kullanıcı girişi ve rol sistemi

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

Client ve API ayrı projeler olarak geliştiriliyor. Veritabanı işlemleri API tarafında tutuluyor.

## Durum

API tarafı büyük ölçüde tamamlandı.

* [x] Kullanıcı girişi
* [x] JWT authentication
* [x] Müşteri CRUD
* [x] Teknisyen CRUD
* [x] Servis kaydı CRUD
* [x] Randevu CRUD
* [x] Fatura CRUD
* [ ] WinForms arayüzü

Şu anda WinForms tarafına geçiliyor. İlk olarak login ekranı, sonrasında diğer yönetim ekranları geliştirilecek.

> Proje geliştirme aşamasındadır.
