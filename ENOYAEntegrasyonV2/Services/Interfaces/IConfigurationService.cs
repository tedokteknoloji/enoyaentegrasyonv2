using ENOYAEntegrasyonV2.Models.Configuration;

namespace ENOYAEntegrasyonV2.Services.Interfaces
{
    /// <summary>
    /// Configuration servisi interface
    /// </summary>
    public interface IConfigurationService
    {
        /// <summary>
        /// Ayarları yükle
        /// </summary>
        AppSettings LoadSettings();

        /// <summary>
        /// Ayarları kaydet
        /// </summary>
        void SaveSettings(AppSettings settings);

        /// <summary>
        /// Mevcut ayarları getir
        /// </summary>
        AppSettings GetSettings();
    }
}

