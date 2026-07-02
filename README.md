# Bilsoft Anket Yönetim Platformu

ASP.NET Core MVC, Razor Pages, Entity Framework Core, MySQL ve ASP.NET Core Identity kullanılarak hazırlanmış staj projesidir.

## Mimari karar

Teknik dokümanda manuel `app_user` ve `app_role` tabloları önerilmiştir. Bu projede kullanıcı, rol, parola hashleme, oturum ve yetki yönetimi için ASP.NET Core Identity kullanıldı. Mevcut anket tabloları korunur; Identity tabloları, `Personel` ve `AppUserPersonel` tabloları tek `ApplicationDbContext` üzerinden yönetilir.

## Kullanılan teknolojiler

- .NET 8
- ASP.NET Core MVC
- Razor Pages
- ASP.NET Core Identity
- Entity Framework Core
- Pomelo.EntityFrameworkCore.MySql
- MySQL / SQLyog
- Bootstrap / jQuery
- ClosedXML

## Test kullanıcıları

- Admin: `admin@bilsoft.com` / `Admin123`
- Kullanıcı: `ayse@bilsoft.com` / `Kullanici123`

## Kurulum

1. `AnketDB.sql` dosyasını SQLyog ile `bilsoft_site` veritabanına import edin.
2. Gerekliyse `anket_cevaplar` ve `anket_musteri` tablolarına `personelID` kolonlarını ekleyin.
3. `appsettings.Development.json` içindeki connection string değerini kendi MySQL bilgilerinize göre kontrol edin.
4. Proje klasöründe migrationları uygulayın:

```bash
dotnet ef database update
```

5. Projeyi çalıştırın:

```bash
dotnet run
```


