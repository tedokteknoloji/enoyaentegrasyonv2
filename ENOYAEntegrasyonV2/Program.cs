using System;
using System.Threading;
using System.Windows.Forms;
using ENOYAEntegrasyonV2.Forms;
using ENOYAEntegrasyonV2.Services.Infrastructure;
using ENOYAEntegrasyonV2.Services.Interfaces;

namespace ENOYAEntegrasyonV2
{
    static class Program
    {
        private static Mutex? _mutex;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            bool createdNew;
            // Uygulama adına özel bir mutex ismi ver
            _mutex = new Mutex(true, "Global\\ENOYAEntegrasyonV2", out createdNew);

            if (!createdNew)
            {
                // Zaten açık
                MessageBox.Show("Uygulama zaten açık.", "Bilgi",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                Application.Exit();
            }
            else
            {

                // Logger'ı başlat
                ILoggerService logger = new FileLoggerService();
                logger.LogInfo("Uygulama başlatıldı");

                try
                {
                    // Configuration servisini başlat
                    IConfigurationService configService = new JsonConfigurationService();
                    var settings = configService.LoadSettings();

                    // Ana formu göster
                    Application.Run(new FrmMainMenu(logger, configService, args));
                    // İsteğe bağlı - uygulama kapanırken bırak
                    _mutex.ReleaseMutex();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Uygulama başlatılamadı:\n\n{ex.Message}",
                        "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}

