# Changelog

## [2.0.0] - 2025-01-XX

### 🎉 Yeni Proje Oluşturuldu

#### ✨ Yeni Özellikler

- **Modern Mimari**: SOLID prensipleri, Repository Pattern, Service Layer
- **Standart Windows Kontrolleri**: DevExpress yerine System.Windows.Forms
- **MSSQL Entegrasyonu**: ENOYAMODELLEME.sql tablolarına göre
- **REST API Entegrasyonu**: IFS API (OAuth2 token authentication)
- **JSON Configuration**: AppSettings.json dosyası
- **File Logging**: Detaylı log dosyası
- **Async/Await**: Asenkron programlama
- **System Tray**: Arka planda çalışma desteği

#### 🔄 Değişiklikler

- **UI**: DevExpress → Standart Windows kontrolleri
- **Veritabanı**: Oracle (yanlış) → MSSQL (doğru)
- **API**: Yok → REST API (OAuth2)
- **Mimari**: Monolitik → SOLID + Repository
- **Config**: INI/JSON → JSON (merkezi)

#### 🐛 Düzeltmeler

- SQL Injection koruması (basit escape, production'da SqlParameter gerekli)
- Exception handling iyileştirildi
- Logging merkezileştirildi

#### 📝 Notlar

- SQL Injection: Şu an basit escape kullanılıyor, production'da SqlParameter kullanılmalı
- JSON Mapping: REST API response mapping'i eksik, implementasyon gerekli
- Transaction: Transaction desteği var ama kullanılmıyor

---

## [1.0.0] - Eski Versiyon

### Eski Yapı (ENOYAEntegrasyon)

- DevExpress kontrolleri
- Oracle veritabanı (yanlış anlama)
- Statik bağımlılıklar
- Karmaşık kod yapısı

