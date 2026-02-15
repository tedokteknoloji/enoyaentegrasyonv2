# ENOYA Entegrasyon V2

## Modern C# WinForms Uygulaması

Bu proje, ENOYA IFS entegrasyon uygulamasının modern, SOLID prensiplerine uygun, standart Windows kontrolleri kullanan versiyonudur.

## Özellikler

### ✅ Modern Mimari
- **SOLID Prensipleri**: Single Responsibility, Open/Closed, Liskov Substitution, Interface Segregation, Dependency Inversion
- **Repository Pattern**: Veri erişim katmanı soyutlaması
- **Service Layer**: İş mantığı katmanı
- **Dependency Injection**: Bağımlılık yönetimi
- **Async/Await**: Asenkron programlama

### ✅ Veritabanı
- **MSSQL**: ENOYAMODELLEME.sql dosyasındaki tablolara göre bağlantı
- **Entity Models**: IRSALIYE, SEVKIYAT, MALZEME, IFSPLAN
- **Repository Pattern**: Veri erişim soyutlaması

### ✅ REST API Entegrasyonu
- **OAuth2 Authentication**: Token tabanlı kimlik doğrulama
- **IFS API**: İş emri listesi, malzeme listesi, work order raporu
- **Postman Collection**: Bursa Beton Entegrasyon.postman_collection.json'a göre

### ✅ UI
- **Standart Windows Kontrolleri**: DevExpress yerine System.Windows.Forms
- **Modern Tasarım**: Temiz ve kullanıcı dostu arayüz
- **System Tray**: Arka planda çalışma desteği
- **Real-time Status**: Bağlantı durumu ve log görüntüleme

### ✅ Logging & Configuration
- **File Logging**: Detaylı log dosyası
- **JSON Configuration**: AppSettings.json dosyası
- **Error Handling**: Merkezi hata yönetimi

## Proje Yapısı

```
ENOYAEntegrasyonV2/
├── Models/
│   ├── Configuration/      # AppSettings, DatabaseSettings, ApiSettings
│   └── Entities/           # Irsaliye, Sevkiyat, Malzeme, IfsPlan
├── Services/
│   ├── Interfaces/         # IDatabaseService, IRestApiService, ILoggerService
│   ├── Database/           # SqlServerService
│   ├── Api/                # RestApiService
│   └── Infrastructure/     # FileLoggerService, JsonConfigurationService
├── Repositories/
│   ├── Interfaces/         # IIrsaliyeRepository, ISevkiyatRepository
│   └── ...                 # Repository implementasyonları
├── Business/
│   └── IntegrationService  # IFS entegrasyon iş mantığı
└── Forms/
    ├── MainForm            # Ana form
    └── SettingsForm        # Ayarlar formu
```

## Kullanım

### 1. Ayarları Yapılandır

İlk çalıştırmada `AppSettings.json` dosyası otomatik oluşturulur:

```json
{
  "Database": {
    "Server": "localhost",
    "Database": "ENOYAMODELLEME",
    "IntegratedSecurity": true
  },
  "Api": {
    "BaseUrl": "https://testifs.bursabeton.com.tr",
    "ClientId": "BURBETENT",
    "ClientSecret": "6GrI9QilCrUqFMlsvdE5WdljD9Hg4Tfc",
    "Contract": "DEMRT"
  },
  "General": {
    "AutoStartIntegration": false,
    "MinimizeToTray": true,
    "IntegrationIntervalSeconds": 60
  }
}
```

### 2. Bağlantıları Test Et

- Ana formda bağlantı durumları otomatik test edilir
- Ayarlar formundan manuel test yapılabilir

### 3. Entegrasyonu Başlat

- **BAŞLAT** butonuna tıklayın
- Otomatik olarak:
  - İş emirleri senkronize edilir
  - Malzemeler senkronize edilir
  - Raporlanmamış sevkiyatlar gönderilir

## Eski Yapıdan Farklar

### ❌ Eski Yapı (ENOYAEntegrasyon)
- DevExpress kontrolleri
- Oracle veritabanı (eski)
- Statik bağımlılıklar
- Karmaşık kod yapısı
- Base64 şifreleme (güvensiz)

### ✅ Yeni Yapı (ENOYAEntegrasyonV2)
- Standart Windows kontrolleri
- MSSQL veritabanı (ENOYAMODELLEME.sql)
- REST API entegrasyonu (OAuth2)
- SOLID prensipleri
- Repository Pattern
- Async/Await
- Modern logging
- JSON configuration

## Gereksinimler

- .NET Framework 4.8
- MSSQL Server (ENOYAMODELLEME veritabanı)
- IFS REST API erişimi
- Newtonsoft.Json NuGet paketi

## Lisans

Copyright © 2025

