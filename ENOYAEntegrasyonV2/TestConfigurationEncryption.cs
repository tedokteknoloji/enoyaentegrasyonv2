using System;
using System.IO;
using System.Windows.Forms;
using ENOYAEntegrasyonV2.Models.Configuration;
using ENOYAEntegrasyonV2.Services.Infrastructure;
using ENOYAEntegrasyonV2.Services.Interfaces;

namespace ENOYAEntegrasyonV2
{
    /// <summary>
    /// Configuration şifreleme test sınıfı
    /// </summary>
    public static class TestConfigurationEncryption
    {
        public static void RunTest()
        {
            try
            {
                string testConfigPath = Path.Combine(Path.GetTempPath(), "ENOYAEntegrasyonV2_Test_Config.json");
                
                // Test için geçici bir encryption service oluştur
                IEncryptionService encryptionService = new AesEncryptionService();
                
                // Test için geçici bir config service oluştur
                // (JsonConfigurationService'in constructor'ını değiştirmemiz gerekebilir)
                
                // Test ayarları oluştur
                var testSettings = new AppSettings
                {
                    Database = new DatabaseSettings
                    {
                        Server = "localhost",
                        Database = "TestDB",
                        UserId = "testuser",
                        Password = "TestPassword123!",
                        IntegratedSecurity = false
                    },
                    Api = new ApiSettings
                    {
                        BaseUrl = "https://test.example.com",
                        ClientId = "TestClient",
                        ClientSecret = "TestSecret123!",
                        Contract = "TEST"
                    }
                };

                // Şifreleme servisi ile config service oluştur
                IConfigurationService configService = new JsonConfigurationService(encryptionService);
                
                // Ayarları kaydet
                configService.SaveSettings(testSettings);
                
                // Ayarları yükle
                var loadedSettings = configService.LoadSettings();
                
                // Test sonuçlarını kontrol et
                bool passwordMatch = testSettings.Database.Password == loadedSettings.Database.Password;
                bool secretMatch = testSettings.Api.ClientSecret == loadedSettings.Api.ClientSecret;
                
                string result = $"Configuration Şifreleme Testi:\n\n";
                result += $"SQL Şifresi Eşleşiyor: {(passwordMatch ? "BAŞARILI ✓" : "BAŞARISIZ ✗")}\n";
                result += $"Client Secret Eşleşiyor: {(secretMatch ? "BAŞARILI ✓" : "BAŞARISIZ ✗")}\n";
                result += $"\nOrijinal SQL Şifresi: {testSettings.Database.Password}\n";
                result += $"Yüklenen SQL Şifresi: {loadedSettings.Database.Password}\n";
                result += $"\nOrijinal Client Secret: {testSettings.Api.ClientSecret}\n";
                result += $"Yüklenen Client Secret: {loadedSettings.Api.ClientSecret}";
                
                // JSON dosyasını kontrol et (şifrelenmiş olmalı)
                if (File.Exists(configService.GetType().GetField("_configFilePath", 
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(configService)?.ToString() ?? ""))
                {
                    string jsonContent = File.ReadAllText(
                        configService.GetType().GetField("_configFilePath", 
                        System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(configService)?.ToString() ?? "");
                    
                    result += $"\n\nJSON İçeriği (şifrelenmiş olmalı):\n";
                    result += jsonContent.Contains("TestPassword123!") ? "UYARI: Şifre düz metin olarak görünüyor! ✗" : "Şifre şifrelenmiş görünüyor ✓";
                    result += jsonContent.Contains("TestSecret123!") ? "\nUYARI: Client Secret düz metin olarak görünüyor! ✗" : "\nClient Secret şifrelenmiş görünüyor ✓";
                }
                
                MessageBox.Show(result, "Test Sonucu", MessageBoxButtons.OK, 
                    (passwordMatch && secretMatch) ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Test hatası:\n\n{ex.Message}\n\n{ex.StackTrace}", 
                    "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

