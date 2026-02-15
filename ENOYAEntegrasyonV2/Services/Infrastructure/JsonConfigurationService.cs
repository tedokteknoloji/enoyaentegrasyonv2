using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using ENOYAEntegrasyonV2.Models.Configuration;
using ENOYAEntegrasyonV2.Services.Interfaces;
using Newtonsoft.Json;

namespace ENOYAEntegrasyonV2.Services.Infrastructure
{
    /// <summary>
    /// JSON tabanlı configuration servisi
    /// </summary>
    public class JsonConfigurationService : IConfigurationService
    {
        private readonly string _configFilePath;
        private AppSettings _settings;

        //public JsonConfigurationService() //: this(new AesEncryptionService())
        //{
        //}

        public JsonConfigurationService()//IEncryptionService encryptionService)
        {
            //_encryptionService = encryptionService ?? throw new ArgumentNullException(nameof(encryptionService));

            // Windows Forms bağımlılığını kaldırmak için AppDomain kullanıyoruz
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            _configFilePath = Path.Combine(basePath, "AppSettings.json");
        }

        public AppSettings LoadSettings()
        {
            try
            {
                if (File.Exists(_configFilePath))
                {
                    var json = File.ReadAllText(_configFilePath);
                    _settings = JsonConvert.DeserializeObject<AppSettings>(EncryptionService.AesDecryptString(GetAppGUID(), json));

                    //// Şifrelenmiş değerleri çöz
                    //if (_settings != null)
                    //{
                    //    if (!string.IsNullOrEmpty(_settings.Database.Password))
                    //    {
                    //        _settings.Database.Password = _encryptionService.Decrypt(_settings.Database.Password);
                    //    }
                    //    if (!string.IsNullOrEmpty(_settings.Api.ClientSecret))
                    //    {
                    //        _settings.Api.ClientSecret = _encryptionService.Decrypt(_settings.Api.ClientSecret);
                    //    }
                    //}
                }
                else
                {
                    // Varsayılan ayarları oluştur
                    _settings = new AppSettings();
                    SaveSettings(_settings);
                }
            }
            catch
            {
                // Hata durumunda varsayılan ayarları kullan
                _settings = new AppSettings();
            }

            return _settings;
        }

        public void SaveSettings(AppSettings settings)
        {
            try
            {
                _settings = settings;

                // Kaydetmeden önce şifreli bir kopya oluştur
                var settingsToSave = EncryptionService.AesEncryptString(GetAppGUID(), JsonConvert.SerializeObject(settings));

                //// Şifreleri şifrele
                //if (!string.IsNullOrEmpty(settingsToSave.Database.Password))
                //{
                //    settingsToSave.Database.Password = _encryptionService.Encrypt(settingsToSave.Database.Password);
                //}
                //if (!string.IsNullOrEmpty(settingsToSave.Api.ClientSecret))
                //{
                //    settingsToSave.Api.ClientSecret = _encryptionService.Encrypt(settingsToSave.Api.ClientSecret);
                //}

                //var json = JsonConvert.SerializeObject(settingsToSave, Formatting.Indented);
                File.WriteAllText(_configFilePath, settingsToSave);
            }
            catch
            {
                // Hata durumunda sessizce devam et
            }
        }

        public AppSettings GetSettings()
        {
            if (_settings == null)
            {
                LoadSettings();
            }
            return _settings;
        }

        public static string GetAppGUID()
        {
            string rv = "";
            Assembly assembly = Assembly.GetExecutingAssembly();
            var attribute = (GuidAttribute)assembly.GetCustomAttributes(typeof(GuidAttribute), true)[0];
            var id = attribute.Value;
            return rv = id.ToString();
        }
    }
}

