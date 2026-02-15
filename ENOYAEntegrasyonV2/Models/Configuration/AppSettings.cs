using System;

namespace ENOYAEntegrasyonV2.Models.Configuration
{
    /// <summary>
    /// Uygulama ayarları modeli
    /// </summary>
    public class AppSettings
    {
        /// <summary>
        /// MSSQL veritabanı bağlantı ayarları
        /// </summary>
        public DatabaseSettings Database { get; set; } = new DatabaseSettings();

        /// <summary>
        /// REST API bağlantı ayarları (IFS)
        /// </summary>
        public ApiSettings Api { get; set; } = new ApiSettings();

        /// <summary>
        /// Uygulama genel ayarları
        /// </summary>
        public GeneralSettings General { get; set; } = new GeneralSettings();
    }

    /// <summary>
    /// MSSQL veritabanı ayarları
    /// </summary>
    public class DatabaseSettings
    {
        public string Server { get; set; } = "localhost";
        public string Database { get; set; } = "ENOYAMODELLEME";
        public string UserId { get; set; } = "sa";
        public string Password { get; set; } = "";
        public bool IntegratedSecurity { get; set; } = true;
        public int ConnectionTimeout { get; set; } = 30;
        public int CommandTimeout { get; set; } = 300;

        /// <summary>
        /// Connection string oluşturur
        /// </summary>
        public string GetConnectionString()
        {
            if (IntegratedSecurity)
            {
                return $"Server={Server};Database={Database};Integrated Security=true;Connection Timeout={ConnectionTimeout};";
            }
            else
            {
                return $"Server={Server};Database={Database};User Id={UserId};Password={Password};Connection Timeout={ConnectionTimeout};";
            }
        }
    }

    /// <summary>
    /// REST API ayarları (IFS)
    /// </summary>
    public class ApiSettings
    {
        public string BaseUrl { get; set; } = "https://test2ifs.bursabeton.com.tr";
        public string TokenUrl { get; set; } = "/auth/realms/test2/protocol/openid-connect/token"; // https://preprodifs.bursabeton.com.tr/auth/realms/cfg/protocol/openid-connect/token
        public string ClientId { get; set; } = "BURBET_MES_ENT";//"BURBETENT";
        public string ClientSecret { get; set; } = "FweYdBYmbVjdEs4JWg00TA9ZRuiGhtqw";//"6GrI9QilCrUqFMlsvdE5WdljD9Hg4Tfc";
        public string Contract { get; set; } = "DEMRT";
        public int TimeoutSeconds { get; set; } = 60;
        public int TokenRefreshMinutes { get; set; } = 50; // Token 1 saat geçerli, 50 dakikada yenile

        /// <summary>
        /// Tam token URL'i
        /// </summary>
        public string GetFullTokenUrl()
        {
            return $"{BaseUrl}{TokenUrl}";
            //return BaseUrl;
        }
    }

    /// <summary>
    /// Genel uygulama ayarları
    /// </summary>
    public class GeneralSettings
    {
        public bool AutoStartIntegration { get; set; } = false;
        public bool MinimizeToTray { get; set; } = true;
        public int IntegrationIntervalSeconds { get; set; } = 60;
        public bool UseAlternativeRoute { get; set; } = false;
        public string AlternativeRoute { get; set; } = "*";
        public string LogLevel { get; set; } = "Info"; // Debug, Info, Warning, Error
    }
}

