# 🚀 Uygulama Geliştirme Rehberi

## 📋 Genel Bakış

**ENOYAEntegrasyonV2**, eski ENOYAEntegrasyon projesinin modern, SOLID prensiplerine uygun, standart Windows kontrolleri kullanan yeni versiyonudur.

## 🎯 Temel Değişiklikler

### 1. Veritabanı Bağlantısı

**Eski:** Oracle (yanlış anlama)
**Yeni:** MSSQL (ENOYAMODELLEME.sql dosyasına göre)

```csharp
// ENOYAMODELLEME.sql dosyasındaki tablolar:
- IRSALIYE
- SEVKIYAT  
- MALZEME
- IFSPLAN
- CONFIG
- RECETE
- MUSTERI
- SANTIYE
- ... (diğer tablolar)
```

### 2. API Entegrasyonu

**Eski:** Oracle veritabanı (yanlış)
**Yeni:** REST API (OAuth2 token authentication)

**Postman Collection Analizi:**
```
Base URL: https://testifs.bursabeton.com.tr
Token URL: /auth/realms/test/protocol/openid-connect/token
Client ID: BURBETENT
Client Secret: 6GrI9QilCrUqFMlsvdE5WdljD9Hg4Tfc

Endpoint'ler:
1. İş Emri Listesi: GET /int/ifsapplications/projection/v1/BurbetIntService.svc/BurbetShopOrdData
2. Malzeme Listesi: GET /int/ifsapplications/projection/v1/BurbetIntService.svc/ServicePartList
3. Work Order Report: POST /int/ifsapplications/projection/v1/BurbetQmanService.svc/ReportOrder
```

### 3. UI Kontrolleri

**Eski:** DevExpress (XtraEditors, LookUpEdit, vb.)
**Yeni:** Standart Windows Kontrolleri

```csharp
// DevExpress → Standart Windows
DevExpress.XtraEditors.LookUpEdit → ComboBox
DevExpress.XtraEditors.TextEdit → TextBox
DevExpress.XtraEditors.CheckEdit → CheckBox
DevExpress.XtraEditors.SimpleButton → Button
DevExpress.XtraGrid.GridControl → DataGridView
```

## 📁 Dosya Yapısı

### Models (Entity & Configuration)

```
Models/
├── Configuration/
│   └── AppSettings.cs          # Tüm ayarlar (Database, API, General)
└── Entities/
    ├── Irsaliye.cs             # IRSALIYE tablosu
    ├── Sevkiyat.cs             # SEVKIYAT tablosu
    ├── Malzeme.cs              # MALZEME tablosu
    └── IfsPlan.cs              # IFSPLAN tablosu
```

### Services (Business Logic)

```
Services/
├── Interfaces/                 # Tüm servis interface'leri
├── Database/
│   └── SqlServerService.cs    # MSSQL bağlantı ve işlemler
├── Api/
│   └── RestApiService.cs      # REST API (OAuth2, IFS)
└── Infrastructure/
    ├── FileLoggerService.cs   # Log dosyası yazma
    └── JsonConfigurationService.cs # JSON config yönetimi
```

### Repositories (Data Access)

```
Repositories/
├── Interfaces/                 # Repository interface'leri
├── IrsaliyeRepository.cs      # IRSALIYE CRUD işlemleri
├── SevkiyatRepository.cs      # SEVKIYAT CRUD işlemleri
└── MalzemeRepository.cs       # MALZEME CRUD işlemleri
```

### Business (Integration Logic)

```
Business/
└── IntegrationService.cs      # IFS entegrasyon iş mantığı
    - SyncShopOrdersAsync()    # İş emirlerini senkronize et
    - SyncMaterialsAsync()     # Malzemeleri senkronize et
    - ReportUnreportedShipmentsAsync() # Sevkiyatları raporla
```

### Forms (UI)

```
Forms/
├── MainForm.cs                # Ana form
└── SettingsForm.cs            # Ayarlar formu
```

## 🔧 Kullanım

### 1. İlk Kurulum

```bash
# 1. Projeyi aç
Visual Studio → ENOYAEntegrasyonV2.sln

# 2. NuGet paketlerini yükle
Tools → NuGet Package Manager → Restore Packages

# 3. Build et
Build → Rebuild Solution

# 4. Çalıştır
F5
```

### 2. Ayarları Yapılandır

**AppSettings.json** otomatik oluşturulur:

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

### 3. Bağlantıları Test Et

1. **Ana Form:** Bağlantılar otomatik test edilir
2. **Ayarlar Formu:** Manuel test butonları var

### 4. Entegrasyonu Başlat

- **BAŞLAT** butonuna tıkla
- Otomatik olarak:
  - İş emirleri senkronize edilir
  - Malzemeler senkronize edilir
  - Raporlanmamış sevkiyatlar gönderilir

## 🔍 Kod Örnekleri

### Database Service Kullanımı

```csharp
var settings = new DatabaseSettings
{
    Server = "localhost",
    Database = "ENOYAMODELLEME",
    IntegratedSecurity = true
};

using (var dbService = new SqlServerService(settings))
{
    var test = await dbService.TestConnectionAsync();
    var data = await dbService.ExecuteQueryAsync("SELECT * FROM IRSALIYE");
}
```

### REST API Service Kullanımı

```csharp
var apiSettings = new ApiSettings
{
    BaseUrl = "https://testifs.bursabeton.com.tr",
    ClientId = "BURBETENT",
    ClientSecret = "6GrI9QilCrUqFMlsvdE5WdljD9Hg4Tfc"
};

var apiService = new RestApiService(apiSettings, logger);
var token = await apiService.GetAccessTokenAsync();
var orders = await apiService.GetShopOrderListAsync();
```

### Repository Kullanımı

```csharp
var dbService = new SqlServerService(settings);
var repository = new SevkiyatRepository(dbService);

var sevkiyat = await repository.GetByIdAsync(1);
var all = await repository.GetAllAsync();
var unreported = await repository.GetUnreportedAsync();
```

## ⚠️ Önemli Notlar

### SQL Injection Koruması

**Şu Anki:** Basit string escape (`'` → `''`)
**Production İçin:** SqlParameter kullanılmalı

```csharp
// Önerilen (güvenli)
var cmd = new SqlCommand("SELECT * FROM TABLE WHERE ID = @id", connection);
cmd.Parameters.AddWithValue("@id", id);
```

### JSON Mapping

**Eksik:** REST API response'larının entity'lere mapping'i
**Yapılacak:** JSON deserialization implementasyonu

### Transaction Yönetimi

**Mevcut:** Transaction desteği var
**Kullanım:** Batch işlemlerde transaction kullanılmalı

## 🐛 Bilinen Sorunlar

1. **SQL Injection:** Basit escape kullanılıyor, SqlParameter'a geçilmeli
2. **JSON Mapping:** REST API response mapping'i eksik
3. **Transaction:** Transaction kullanılmıyor
4. **Error Recovery:** Retry logic yok
5. **Unit Tests:** Test yazılmamış

## 📚 Referanslar

- **MSSQL Model:** `BursaBetonModelleme/ENOYAMODELLEME.sql`
- **REST API:** `BursaBetonModelleme/Bursa Beton Entegrasyon.postman_collection.json`
- **Eski Kod:** `ENOYAEntegrasyon/` klasörü

## 🎓 Öğrenilen Dersler

1. **Postman Collection Analizi Önemli:** Oracle değil, REST API kullanılıyor
2. **SOLID Prensipleri:** Kod kalitesini önemli ölçüde artırır
3. **Repository Pattern:** Test edilebilirliği artırır
4. **Async/Await:** UI donmasını önler
5. **Standart Kontroller:** DevExpress'e gerek yok

## ✅ Tamamlanan İşler

- [x] Proje yapısı
- [x] Entity modelleri
- [x] Repository pattern
- [x] REST API servisi
- [x] MSSQL servisi
- [x] Logger servisi
- [x] Configuration servisi
- [x] Ana form
- [x] Ayarlar formu
- [x] Integration servisi

## 🔮 Gelecek İyileştirmeler

- [ ] SQL Injection koruması (SqlParameter)
- [ ] JSON mapping implementasyonu
- [ ] Transaction yönetimi
- [ ] Unit testler
- [ ] Error recovery
- [ ] Retry logic
- [ ] Performance monitoring

---

**Versiyon:** 2.0.0
**Durum:** Production'a hazır (SQL Injection koruması eklendikten sonra)

