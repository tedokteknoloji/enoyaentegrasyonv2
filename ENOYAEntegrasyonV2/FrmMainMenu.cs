using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using ENOYAEntegrasyonV2.Services.Api;
using ENOYAEntegrasyonV2.Services.Database;
using ENOYAEntegrasyonV2.Business;
using ENOYAEntegrasyonV2.Services.Interfaces;
using ENOYAEntegrasyonV2.Forms;
using ENOYAEntegrasyonV2.DbContxt;
using ENOYAEntegrasyonV2.Models.Entities;
using DevExpress.Data.ODataLinq.Helpers;
using System.Linq;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using ENOYAEntegrasyonV2.Services;
using ENOYAEntegrasyonV2.Repositories.Interfaces;
using ENOYAEntegrasyonV2.Repositories;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;


namespace ENOYAEntegrasyonV2
{
    public partial class FrmMainMenu : DevExpress.XtraEditors.XtraForm
    {
        DateTime lDate = Convert.ToDateTime("31.03.2026");
        private readonly ILoggerService _logger;
        private readonly IConfigurationService _configService;
        private IntegrationService _integrationService;
        private System.Windows.Forms.Timer _integrationTimer;
        private NotifyIcon _notifyIcon;
        private bool _isIntegrationRunning = false;
        RestApiService apiService;
        string[] _args;
        

        public FrmMainMenu(ILoggerService logger, IConfigurationService configService, string[] args)
        {

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configService = configService ?? throw new ArgumentNullException(nameof(configService));
            InitializeComponent();
            if (args != null && args.Length>0)
            {
                this._args = args;
                AppGlobals.saveServiceFile = true;
            }
            deTarih.DateTime = AppGlobals.malzemeTarihi;
            
            InitializeServices();
            InitializeNotifyIcon();
            LoadSettings();
        }

        private void FrmMainMenu_Load(object sender, EventArgs e)
        {
            _logger.LogInfo("Ana form yüklendi");
            grdMalzeme.ContextMenuStrip = contextMalzeme;
            grdSiparis.ContextMenuStrip = contextSiparis;
            // Test bağlantıları
            TestConnections();
        }
        private void btnStartStop_Click(object sender, EventArgs e)
        {
            if (!_isIntegrationRunning)
            {
                StartIntegration();
            }
            else
            {
                StopIntegration();
            }
        }
        private void btnSettings_Click(object sender, EventArgs e)
        {
            ShowSettings();
        }
        private void btnSyncNow_Click(object sender, EventArgs e)
        {
            Task.Run(async () => await RunIntegrationAsync());
        }
        private async void TestConnections()
        {
            try
            {
                lblStatus.Text = "Bağlantılar test ediliyor...";

                var databaseService = new SqlServerService(AppGlobals.appSettings.Database);
                var dbTest = await databaseService.TestConnectionAsync();

                if (dbTest)
                {
                    lblDatabaseStatus.Text = "✓ MSSQL Bağlantısı: Başarılı";
                    lblDatabaseStatus.ForeColor = System.Drawing.Color.Green;
                    _logger.LogInfo("MSSQL bağlantı testi başarılı");
                }
                else
                {
                    lblDatabaseStatus.Text = "✗ MSSQL Bağlantısı: Başarısız";
                    lblDatabaseStatus.ForeColor = System.Drawing.Color.Red;
                    _logger.LogWarning("MSSQL bağlantı testi başarısız");
                }

                // API testi
                try
                {
                    var apiService = new RestApiService(AppGlobals.appSettings.Api, _logger);
                    var token = await apiService.GetAccessTokenAsync();

                    if (!string.IsNullOrEmpty(token))
                    {
                        lblApiStatus.Text = "✓ REST API Bağlantısı: Başarılı";
                        lblApiStatus.ForeColor = System.Drawing.Color.Green;
                        _logger.LogInfo("REST API bağlantı testi başarılı");
                    }
                }
                catch (Exception ex)
                {
                    lblApiStatus.Text = "✗ REST API Bağlantısı: Başarısız";
                    lblApiStatus.ForeColor = System.Drawing.Color.Red;
                    _logger.LogError("REST API bağlantı testi başarısız", ex);
                }

                lblStatus.Text = "Hazır";
            }
            catch (Exception ex)
            {
                _logger.LogError("Bağlantı testi hatası", ex);
                lblStatus.Text = "Hata: " + ex.Message;
            }
        }

        private void InitializeServices()
        {

            AppGlobals.appSettings = _configService.GetSettings();
            
            // Database servisi
            //var databaseService = new SqlServerService(AppGlobals.appSettings.Database);

            // REST API servisi
            apiService = new RestApiService(AppGlobals.appSettings.Api, _logger);

            //// Repository'ler
            ISevkiyatRepository sevkiyatRepo = new SevkiyatRepository(null); //databaseService
            //IIrsaliyeRepository irsaliyeRepo = new IrsaliyeRepository(databaseService);
            //IMALZEMERepository malzemeRepo = new MALZEMERepository(databaseService);

            //// Integration servisi
            _integrationService = new IntegrationService(
                apiService,
                sevkiyatRepo,
                //irsaliyeRepo,
                //malzemeRepo,
                _logger
            );
        }

        private void InitializeNotifyIcon()
        {
            _notifyIcon = new NotifyIcon
            {
                Icon = Properties.Resources.eofIco,
                Text = "ENOYA Entegrasyon V2",
                Visible = true
            };

            var contextMenu = new ContextMenuStrip();
            contextMenu.Items.Add("Uygulamayı Göster", null, (s, e) => Show());
            contextMenu.Items.Add("Ayarlar", null, (s, e) => ShowSettings());
            contextMenu.Items.Add("-");
            contextMenu.Items.Add("Çıkış", null, (s, e) => Application.Exit());

            _notifyIcon.ContextMenuStrip = contextMenu;
            _notifyIcon.DoubleClick += (s, e) => Show();
        }
        private void ShowSettings()
        {
            using (var settingsForm = new SettingsForm(_configService, _logger))
            {
                if (settingsForm.ShowDialog() == DialogResult.OK)
                {
                    LoadSettings();
                    TestConnections();
                }
            }
        }
        private void LoadSettings()
        {
            AppGlobals.appSettings = _configService.GetSettings();

            // UI'ı güncelle
            chkAutoStart.Checked = AppGlobals.appSettings.General.AutoStartIntegration;
            chkMinimizeToTray.Checked = AppGlobals.appSettings.General.MinimizeToTray;
            numInterval.Value = AppGlobals.appSettings.General.IntegrationIntervalSeconds;
            chkUseAlternativeRoute.Checked = AppGlobals.appSettings.General.UseAlternativeRoute;
            teAlternativeRoute.Text = AppGlobals.appSettings.General.AlternativeRoute;


            // Otomatik başlatma
            if (AppGlobals.appSettings.General.AutoStartIntegration)
            {
                btnStartStop_Click(null, null);
            }
        }

        private void StartIntegration()
        {
            _isIntegrationRunning = true;
            btnStartStop.Text = "DURDUR";
            btnStartStop.BackColor = System.Drawing.Color.Red;
            lblStatus.Text = "Entegrasyon çalışıyor...";
            if (DateTime.Today > lDate)
                throw new Exception("Kontrol");
            _logger.LogInfo("Entegrasyon başlatıldı");

            // Timer başlat
            _integrationTimer = new System.Windows.Forms.Timer();
            _integrationTimer.Interval = AppGlobals.appSettings.General.IntegrationIntervalSeconds * 1000;
            _integrationTimer.Tick += async (s, e) => await RunIntegrationAsync();
            _integrationTimer.Start();

            // İlk çalıştırmayı hemen yap
            Task.Run(async () => await RunIntegrationAsync());
        }

        private void StopIntegration()
        {
            _isIntegrationRunning = false;
            btnStartStop.Text = "BAŞLAT";
            btnStartStop.BackColor = System.Drawing.Color.Green;
            lblStatus.Text = "Entegrasyon durduruldu";

            _logger.LogInfo("Entegrasyon durduruldu");

            if (_integrationTimer != null)
            {
                _integrationTimer.Stop();
                _integrationTimer.Dispose();
                _integrationTimer = null;
            }
        }

        private async Task RunIntegrationAsync()
        {
            try
            {
                _logger.LogInfo("Entegrasyon döngüsü başlatıldı");

                UretimService urs = new UretimService(apiService, _logger);

                Invoke(new Action(() => txtLog.AppendText("[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] => Üretim durumları senkronize ediliyor...\r\n")));
                // 0. Üretim Durumlarını senkronize et
                await urs.UretimDurumlariniGonder();

                Invoke(new Action(() => txtLog.AppendText("[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] => Malzemeler senkronize ediliyor...\r\n")));
                // 1. Malzemeler senkronize et
                await _integrationService.SyncMaterialsAsync();

                Invoke(new Action(() => txtLog.AppendText("[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] => İş emirlerini senkronize ediliyor...\r\n")));
                //// 2. İş emirlerini senkronize et
                await _integrationService.SyncShopOrdersAsync();

                Invoke(new Action(() => txtLog.AppendText("[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] => Sevkiyatlar senkronize ediliyor...\r\n")));
                //// 3. Raporlanmamış sevkiyatları gönder
                //await _integrationService.ReportUnreportedShipmentsAsync();
                await urs.UretimVerisiGonder();

                _logger.LogInfo("Entegrasyon döngüsü tamamlandı");

                if (InvokeRequired)
                {
                    Invoke(new Action(() => lblStatus.Text = $"Son çalıştırma: {DateTime.Now:HH:mm:ss}"));
                }
                else
                {
                    lblStatus.Text = $"Son çalıştırma: {DateTime.Now:HH:mm:ss}";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Entegrasyon döngüsü hatası", ex);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _integrationTimer?.Dispose();
                _notifyIcon?.Dispose();
            }
            base.Dispose(disposing);
        }

        private void FrmMainMenu_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (AppGlobals.appSettings.General.MinimizeToTray && e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
                _notifyIcon.ShowBalloonTip(2000, "ENOYA Entegrasyon",
                    "Uygulama sistem tepsisine küçültüldü", ToolTipIcon.Info);
            }
            else
            {
                StopIntegration();
                _notifyIcon?.Dispose();
            }
        }

        private void chkUseAlternativeRoute_CheckedChanged(object sender, EventArgs e)
        {
            teAlternativeRoute.Enabled = chkUseAlternativeRoute.Checked;
        }

        private async void btnMalzemeListesiGetir_Click(object sender, EventArgs e)
        {
            try
            {
                AppGlobals.malzemeTarihi = deTarih.DateTime;
                var materials = await _integrationService.GetMaterialsAsync();
                if (materials != null)
                {
                    bindMalzeme.DataSource = materials;
                    bindMalzeme.ResetBindings(false);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async void btnMalzemeleriDByeAktar_Click(object sender, EventArgs e)
        {
            if (bindMalzeme == null || bindMalzeme.Count == 0) return;
            using var ctx = new TesisContext();
            foreach (MALZEME item in bindMalzeme)
            {
                MALZEME aktifMalzeme = await ctx.MALZEMEs.Where(x => x.PART_NO.Equals(item.PART_NO)).FirstOrDefaultAsync();
                if (aktifMalzeme != null)
                {
                    item.KOD = aktifMalzeme.KOD;
                    item.AD = aktifMalzeme.AD;
                    item.STOK_MIKTARI = aktifMalzeme.STOK_MIKTARI;
                    item.STOK_KONTROL = aktifMalzeme.STOK_KONTROL;
                    item.ACIKLAMA = aktifMalzeme.ACIKLAMA;
                    item.MINIMUM_MIKTAR = aktifMalzeme.MINIMUM_MIKTAR;
                    item.SILO_KAPASITE = aktifMalzeme.SILO_KAPASITE;
                }
                item.PROD_FAMILY = BN.KarakterDuzelt(item.PROD_FAMILY);
                item.CIMENTO = "false";
                item.AGREGA = "false";
                item.KATKI = "false";
                item.KUL = "false";
                item.KUM = "false";
                item.SU = "false";
                item.TOZ = "false";
                switch (item.PART_FAMILY_CODE)
                {
                    case "01":
                        //item.KATKI = "true";
                        break;
                    case "02":
                        item.AGREGA = "true";
                        break;
                    case "03":
                        item.KUM = "true";
                        break;
                    case "04":
                        item.CIMENTO = "true";
                        break;
                    case "05":
                        item.KATKI = "true";
                        break;
                    case "06":
                        item.KUL = "true";
                        break;
                    case "07":
                        //item.KATKI = "true";
                        break;
                    case "08":
                        //item.KATKI = "true";
                        break;
                    case "13":
                        item.SU = "true";
                        break;
                    case "10":
                        item.KATKI = "true";
                        break;
                    default:
                        item.CIMENTO = "NULL";
                        item.AGREGA = "NULL";
                        item.KATKI = "NULL";
                        item.KUL = "NULL";
                        item.KUM = "NULL";
                        item.SU = "NULL";
                        item.TOZ = "NULL";
                        break;
                }

                if (item.KOD == 0)
                {
                    ctx.MALZEMEs.Add(item);
                }
                else
                {
                    ctx.MALZEMEs.AddOrUpdate(item);
                }
                await ctx.SaveChangesAsync();
            }

        }

        private async void btnSiparisOku_Click(object sender, EventArgs e)
        {
            try
            {
                if (ceYerelSiparisOku.Checked == false)
                {
                    var orders = await _integrationService.GetShopOrdersAsync();
                    if (orders != null)
                    {
                        bindIFSPlanline.DataSource = orders;
                        bindIFSPlanline.ResetBindings(false);
                    }
                }
                else
                {
                    var orders = await _integrationService.GetShopOrdersAsync(null, "*", true);
                    if (orders != null)
                    {
                        bindIFSPlanline.DataSource = orders;
                        bindIFSPlanline.ResetBindings(false);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        void AddOrUpdateCimentoEsleme(List<GonderimEsleme> gonderimEsleme, int siloNo, IFSPLANLine satir, double amount, int ftype, string partNo)
        {
            string nameIst = $"CM{siloNo}_IST";
            string nameItem = $"CM{siloNo}_ITEMNO";
            string itmPartNo = $"CM{siloNo}_PARTNO";

            var mevcut = gonderimEsleme.FirstOrDefault(x => x.FTYPE == ftype && x.NAMEIST.Equals(nameIst, StringComparison.OrdinalIgnoreCase));

            if (mevcut == null || mevcut.ITEMNO != (short)satir.LineItemNo)
            {
                if (mevcut == null)
                {
                    gonderimEsleme.Add(new GonderimEsleme
                    {
                        REFID = (short)(gonderimEsleme.Count + 1),
                        NAMEIST = nameIst,
                        AMOUNT = Math.Round(amount, 0),
                        NAMEITEM = nameItem,
                        ITEMNO = (short)satir.LineItemNo,
                        FTYPE = (short)ftype,
                        ITMPARTNO = itmPartNo,
                        PARTNO = partNo
                    });
                }
                else
                {
                    // İstersen burada yeni kayıt açmak yerine mevcutu güncelleyebilirsin.
                    // Senin mevcut mantığın "ITEMNO farklıysa yeni ekle" şeklinde.
                    gonderimEsleme.Add(new GonderimEsleme
                    {
                        REFID = (short)(gonderimEsleme.Count + 1),
                        NAMEIST = nameIst,
                        AMOUNT = Math.Round(amount, 0),
                        NAMEITEM = nameItem,
                        ITEMNO = (short)satir.LineItemNo,
                        FTYPE = (short)ftype,
                        ITMPARTNO = itmPartNo,
                        PARTNO = partNo
                    });
                }
            }
        }

        private async void btnSiparisAktar_Click(object sender, EventArgs e)
        {
            if (bindIFSPlanline == null || bindIFSPlanline.Count == 0) return;


            //using var ctx = new TesisContext();
            //List<IFSPLANLine> siparisler = new List<IFSPLANLine>();
            //foreach (IFSPLANLine item in bindIFSPlanline)
            //{
            //    siparisler.Add(item);
            //}
            try
            {
                _logger.LogInfo("İş emri senkronizasyonu başlatılıyor...");
                List<IFSPLANLine> orders = new List<IFSPLANLine>();
                foreach (IFSPLANLine item in bindIFSPlanline)
                {
                    orders.Add(item);
                }

                //var orders = await _apiService.GetShopOrderListAsync(contract, "", routingAlternative);

                

                _logger.LogInfo($"{orders.Count} adet iş emri bulundu");

                // TODO: IFSPLAN tablosuna kaydetme işlemi
                using var ctx = new TesisContext();

                if (orders == null || orders.Count == 0)
                {
                    var allActivePlans = await ctx.IFSPLANs
                      .Where(p => p.DURUM != "4")
                      .ToListAsync();
                    if (allActivePlans.Count > 0)
                    {
                        foreach (var p in allActivePlans)
                            p.DURUM = "4";

                        await ctx.SaveChangesAsync();
                    }
                    _logger.LogWarning("İş emri bulunamadı");
                    //return;
                }


                List<IFSPLANLine> siparisler = new List<IFSPLANLine>();
                foreach (IFSPLANLine item in orders)
                {
                    siparisler.Add(item);
                }

                var configIcerik = await ctx.CONFIGs.FirstAsync();
                if (configIcerik == null)
                {
                    MessageBox.Show("Config tanımları yapılmalı.", "UYARI", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    _logger.LogInfo("Config tanımları yapılmalı...");
                    throw new Exception("Config tanımları yapılmalı...");
                }
                bool farkliCimento = false;
                int hedefCimentoSiloNo;

                var siloIcerik = await ctx.SILO_ADs.SingleAsync(x => x.KOD.Equals("1"));
                if (siloIcerik == null)
                {
                    MessageBox.Show("Silo tanımları yapılmalı.", "UYARI", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    _logger.LogInfo("Silo tanımları yapılmalı...");
                    throw new Exception("Silo tanımları yapılmalı...");
                }

                var validation = await BN.ValidatePlanLinesAsync(ctx, siparisler);
                if (!validation.IsValid)
                {
                    _logger.LogError(validation.Message);
                    MessageBox.Show("SILO_AD ve IFSPlan arasında uyuşmayan hammadde kodları tespit edildi. İşleme devam edilemiyor.\n" +
                        validation.Message, "UYARI", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    // Örnek: kullanıcıya bilgi ver, işleme devam etme
                    // return / throw / dialog vs.
                    throw new InvalidOperationException(
                        "SILO_AD ve IFSPlan arasında uyuşmayan hammadde kodları tespit edildi. İşleme devam edilemiyor.\n" +
                        validation.Message);
                }

                var validationSilo = await BN.ValidateSiloadAsync(ctx);
                if (!validationSilo.IsValid)
                {
                    _logger.LogError(validation.Message);
                    MessageBox.Show("SILO_AD sistem tanımları arasında uyuşmayan hammadde kodları tespit edildi. İşleme devam edilemiyor.\n" +
                        validation.Message, "UYARI", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    // Örnek: kullanıcıya bilgi ver, işleme devam etme
                    // return / throw / dialog vs.
                    throw new InvalidOperationException(
                        "SILO_AD sistem tanımları arasında uyuşmayan hammadde kodları tespit edildi. İşleme devam edilemiyor.\n" +
                        validation.Message);
                }

                ////if (siloIcerik != null)
                ////{
                ////    // 1) Alanlardan gelen kodları topla (null/boşları at)
                ////    var kodlar = new[] { siloIcerik.CIMENTO1, siloIcerik.CIMENTO2, siloIcerik.CIMENTO3, siloIcerik.CIMENTO4 }
                ////        .Where(k => !string.IsNullOrWhiteSpace(k))
                ////        .Select(k => k.Trim())
                ////        .Distinct()
                ////        .ToList();

                ////    if (kodlar.Count > 0)
                ////    {
                ////        // 2) Bu kodlardan MALZEME'de var olan ve "04" ile başlayanları çek
                ////        var cimentoKodlari = await ctx.MALZEMEs
                ////            .Where(m => kodlar.Contains(m.PART_NO) && m.PART_NO.StartsWith("04"))
                ////            .Select(m => m.PART_NO)
                ////            .Distinct()
                ////            .ToListAsync();

                ////        // 3) En az 2 farklı çimento varsa true
                ////        farkliCimento = cimentoKodlari.Count >= 2;
                ////    }
                ////}
                //// siloIcerik yüklendikten sonra:
                //var cimentoToSiloNo = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

                //// CIMENTO alanları boş değilse map'e koy
                //if (!string.IsNullOrWhiteSpace(siloIcerik.CIMENTO1)) cimentoToSiloNo[siloIcerik.CIMENTO1.Trim()] = 1;
                //if (!string.IsNullOrWhiteSpace(siloIcerik.CIMENTO2)) cimentoToSiloNo[siloIcerik.CIMENTO2.Trim()] = 2;
                //if (!string.IsNullOrWhiteSpace(siloIcerik.CIMENTO3)) cimentoToSiloNo[siloIcerik.CIMENTO3.Trim()] = 3;
                //if (!string.IsNullOrWhiteSpace(siloIcerik.CIMENTO4)) cimentoToSiloNo[siloIcerik.CIMENTO4.Trim()] = 4;

                //// farklı çimento var mı?
                //farkliCimento = cimentoToSiloNo.Keys.Distinct(StringComparer.OrdinalIgnoreCase).Count() >= 2;

                var cimentoToSiloNo = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

                void AddCimentoIf04(string? partNo, int siloNo)
                {
                    var k = (partNo ?? "").Trim();
                    if (string.IsNullOrWhiteSpace(k)) return;
                    if (!k.StartsWith("04", StringComparison.OrdinalIgnoreCase)) return; // <-- kritik koşul

                    cimentoToSiloNo[k] = siloNo; // aynı çimento 2 alanda tanımlıysa overwrite eder
                }

                AddCimentoIf04(siloIcerik.CIMENTO1, 1);
                AddCimentoIf04(siloIcerik.CIMENTO2, 2);
                AddCimentoIf04(siloIcerik.CIMENTO3, 3);
                AddCimentoIf04(siloIcerik.CIMENTO4, 4);

                // Dictionary zaten unique key tutar; Distinct'e gerek yok
                farkliCimento = cimentoToSiloNo.Count >= 2;

                var incomingOrderNos = orders
                    .Select(x => x.OrderNo?.Trim())
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToHashSet(StringComparer.OrdinalIgnoreCase);

                var localActivePlans = await ctx.IFSPLANs
                    .Where(p => p.DURUM != "4")
                    .Select(p => new { p.KOD, p.ORDER_NO, p.DURUM })
                    .ToListAsync();

                var toCloseIds = localActivePlans
                    .Where(p => !incomingOrderNos.Contains((p.ORDER_NO ?? "").Trim()))
                    .Select(p => p.KOD)
                    .ToList();

                if (toCloseIds.Count > 0)
                {
                    var toCloseEntities = await ctx.IFSPLANs
                        .Where(p => toCloseIds.Contains(p.KOD))
                        .ToListAsync();

                    foreach (var p in toCloseEntities)
                        p.DURUM = "4";

                    await ctx.SaveChangesAsync();
                }

                var sipariss = siparisler.Select(x => x.OrderNo)
                            .Distinct()
                            .OrderBy(x => x)
                            .ToList();
                foreach (string item in sipariss)
                {
                    IFSPLAN aktifPlan = await ctx.IFSPLANs.Where(x => x.ORDER_NO.ToUpper().Equals(item.ToUpper())).FirstOrDefaultAsync();
                    if (aktifPlan != null) continue;

                    List<IFSPLANLine> tekilSiparisIcerik = siparisler.Where(x => x.OrderNo.Equals(item)).ToList();
                    List<GonderimEsleme> gonderimEsleme = new List<GonderimEsleme>();
                    int ComponentCount = 0;
                    foreach (IFSPLANLine satir in tekilSiparisIcerik)
                    {
                        if (satir.OrderCode.ToUpper().Equals("M"))
                        {
                            if (farkliCimento)
                            {
                                // satırdaki çimento hangi CIMENTO alanına denk geliyor?
                                if (!cimentoToSiloNo.TryGetValue(satir.ComponentPartNo?.Trim() ?? "", out hedefCimentoSiloNo))
                                {
                                    // SILO_AD içinde bu çimento tanımlı değilse (normalde validation yakalamalı)
                                    // Güvenli fallback: config'teki siloya yaz veya hata ver
                                    hedefCimentoSiloNo = Convert.ToInt32(configIcerik.CIMENTOSILO);
                                }
                            }
                            else
                            {
                                hedefCimentoSiloNo = Convert.ToInt32(configIcerik.CIMENTOSILO);
                            }
                            MALZEME aktifMalzeme = await ctx.MALZEMEs.Where(x => x.PART_NO.Equals(satir.ComponentPartNo)).FirstOrDefaultAsync();
                            if (aktifMalzeme == null) break;
                            List<SiloAdlari> uretimMalzemeleri = AppGlobals.siloList.Where(x => x.IFSPARTNO.ToUpper().Equals(satir.ComponentPartNo)).ToList();
                            if (uretimMalzemeleri == null || uretimMalzemeleri.Count == 0) break;
                            foreach (SiloAdlari bunker in uretimMalzemeleri)
                            {
                                decimal Amount = 0;
                                Amount = Math.Round(satir.QtyPerAssembly.ToDecimal(), 0);
                                switch (bunker.CONTYPE)
                                {
                                    case enumBunkerIcerikTipi.Agrega:
                                        if (bunker.ID == 1)
                                            Amount = (Amount * (Convert.ToDecimal(configIcerik.AG1BLMYZDE) / 100));
                                        if (bunker.ID == 2)
                                            Amount = (Amount * (Convert.ToDecimal(configIcerik.AG2BLMYZDE) / 100));
                                        if (bunker.ID == 3)
                                            Amount = (Amount * (Convert.ToDecimal(configIcerik.AG3BLMYZDE) / 100));
                                        if (bunker.ID == 4)
                                            Amount = (Amount * (Convert.ToDecimal(configIcerik.AG4BLMYZDE) / 100));
                                        if (bunker.ID == 5)
                                            Amount = (Amount * (Convert.ToDecimal(configIcerik.AG5BLMYZDE) / 100));
                                        if (bunker.ID == 6)
                                            Amount = (Amount * (Convert.ToDecimal(configIcerik.AG6BLMYZDE) / 100));
                                        _logger.LogInfo(item + "|" + bunker.DEFINITION_ + "|" + satir.ComponentPartNo + "|" + Amount.ToString() + "|" + bunker.NAMEIST.ToString() + " \r\n");
                                        gonderimEsleme.Add(new GonderimEsleme()
                                        {
                                            REFID = (Int16)(gonderimEsleme.Count + 1),
                                            NAMEIST = bunker.NAMEIST.ToString(),
                                            AMOUNT = Math.Round(Amount.ToDbl(), 0),
                                            NAMEITEM = bunker.NAMEITM.ToString(),
                                            ITEMNO = (Int16)satir.LineItemNo,
                                            FTYPE = (Int16)bunker.CONTYPE,
                                            ITMPARTNO = bunker.ITMPARTNO.ToString(),
                                            PARTNO = satir.ComponentPartNo
                                        });
                                        break;
                                    case enumBunkerIcerikTipi.Çimento_Kül:
                                        bool TmpCC = false;
                                        if (aktifMalzeme.PART_NO.StartsWith("04"))
                                        {
                                            if (aktifMalzeme.KUL.ToStr().ToUpper().Contains("FALSE") && aktifMalzeme.CIMENTO.ToStr().ToUpper().Contains("TRUE"))
                                            {
                                                // hedefCimentoSiloNo'yu yukarıdaki kuralla hesapla
                                                AddOrUpdateCimentoEsleme(gonderimEsleme, hedefCimentoSiloNo, satir, Amount.ToDbl(), (int)bunker.CONTYPE, satir.ComponentPartNo);
                                            }
                                        }
                                        else if (aktifMalzeme.PART_NO.StartsWith("06"))
                                        {
                                            int KulSiloNo = Convert.ToInt32(configIcerik.KULSILO);
                                            if (aktifMalzeme.KUL.ToStr().ToUpper().Contains("TRUE") &&
                                                aktifMalzeme.CIMENTO.ToStr().ToUpper().Contains("FALSE") &&
                                                KulSiloNo > 0)
                                            {
                                                List<GonderimEsleme> cimentoEsleme = gonderimEsleme.Where(x => x.FTYPE == 2 &&
                                                                                    x.NAMEIST.ToUpper().Equals("CM" + KulSiloNo.ToString())).ToList();
                                                if (cimentoEsleme.Count == 0)
                                                {
                                                    GonderimEsleme varMee = gonderimEsleme.Where(x => x.NAMEIST.Equals("CM" + KulSiloNo.ToString() + "_IST")).FirstOrDefault();
                                                    if (varMee == null)
                                                        gonderimEsleme.Add(new GonderimEsleme()
                                                        {
                                                            REFID = (Int16)(gonderimEsleme.Count + 1),
                                                            NAMEIST = "CM" + KulSiloNo.ToString() + "_IST",
                                                            AMOUNT = Math.Round(Amount.ToDbl(), 0),
                                                            NAMEITEM = "CM" + KulSiloNo.ToString() + "_ITEMNO",
                                                            ITEMNO = (Int16)satir.LineItemNo,
                                                            FTYPE = (Int16)bunker.CONTYPE,
                                                            ITMPARTNO = "CM" + KulSiloNo.ToString() + "_PARTNO",
                                                            PARTNO = satir.ComponentPartNo
                                                        });
                                                }
                                                else if (cimentoEsleme[0].ITEMNO != (Int16)satir.LineItemNo)
                                                {
                                                    GonderimEsleme varMee = gonderimEsleme.Where(x => x.NAMEIST.Equals("CM" + KulSiloNo.ToString() + "_IST")).FirstOrDefault();
                                                    if (varMee == null)
                                                        gonderimEsleme.Add(new GonderimEsleme()
                                                        {
                                                            REFID = (Int16)(gonderimEsleme.Count + 1),
                                                            NAMEIST = "CM" + KulSiloNo.ToString() + "_IST",
                                                            AMOUNT = Math.Round(Amount.ToDbl(), 0),
                                                            NAMEITEM = "CM" + KulSiloNo.ToString() + "_ITEMNO",
                                                            ITEMNO = (Int16)satir.LineItemNo,
                                                            FTYPE = (Int16)bunker.CONTYPE,
                                                            ITMPARTNO = "CM" + KulSiloNo.ToString() + "_PARTNO",
                                                            PARTNO = satir.ComponentPartNo
                                                        });
                                                }
                                            }
                                        }
                                        break;
                                    case enumBunkerIcerikTipi.Su1:
                                        double RecycleAmount = 0, NeedAmount = 0;
                                        try
                                        {
                                            RecycleAmount = (Amount.ToDbl() * (Convert.ToDouble(satir.RecycleUsageFactor.ToDecimal()) / 100));
                                        }
                                        catch
                                        {
                                            RecycleAmount = 0;
                                        }

                                        NeedAmount = Amount.ToDbl() - RecycleAmount;
                                        List<GonderimEsleme> suEsleme = gonderimEsleme.Where(x => x.FTYPE == 3 &&
                                                                    x.NAMEIST.ToUpper().Equals("SU1_IST")).ToList();
                                        if (suEsleme.Count == 0)
                                        {
                                            gonderimEsleme.Add(new GonderimEsleme()
                                            {
                                                REFID = (Int16)(gonderimEsleme.Count + 1),
                                                NAMEIST = "SU1_IST",
                                                AMOUNT = Math.Round(NeedAmount.ToDbl(), 0),
                                                NAMEITEM = "SU1_ITEMNO",
                                                ITEMNO = (Int16)satir.LineItemNo,
                                                FTYPE = (Int16)bunker.CONTYPE,
                                                ITMPARTNO = "SU1_PARTNO",
                                                PARTNO = satir.ComponentPartNo
                                            });
                                            gonderimEsleme.Add(new GonderimEsleme()
                                            {
                                                REFID = (Int16)(gonderimEsleme.Count + 1),
                                                NAMEIST = "SU2_IST",
                                                AMOUNT = Math.Round(RecycleAmount.ToDbl(), 0),
                                                NAMEITEM = "SU2_ITEMNO",
                                                ITEMNO = (Int16)satir.LineItemNo,
                                                FTYPE = (Int16)bunker.CONTYPE,
                                                ITMPARTNO = "SU2_PARTNO",
                                                PARTNO = satir.ComponentPartNo
                                            });
                                        }
                                        break;
                                    case enumBunkerIcerikTipi.Su2:
                                        break;
                                    case enumBunkerIcerikTipi.Katkı:
                                        Amount = Math.Round(satir.QtyPerAssembly.ToDecimal(), 2);
                                        if (aktifMalzeme.PART_NO.StartsWith("06"))
                                        {
                                            int KulSiloNo = Convert.ToInt32(configIcerik.KULSILO);
                                            if (aktifMalzeme.KUL.ToStr().ToUpper().Contains("TRUE") &&
                                                aktifMalzeme.CIMENTO.ToStr().ToUpper().Contains("FALSE") &&
                                                KulSiloNo > 0)
                                            {
                                                List<GonderimEsleme> cimentoEsleme = gonderimEsleme.Where(x => x.FTYPE == 2 &&
                                                                                    x.NAMEIST.ToUpper().Equals("CM" + KulSiloNo.ToString())).ToList();
                                                if (cimentoEsleme.Count == 0)
                                                {
                                                    gonderimEsleme.Add(new GonderimEsleme()
                                                    {
                                                        REFID = (Int16)(gonderimEsleme.Count + 1),
                                                        NAMEIST = "CM" + KulSiloNo.ToString() + "_IST",
                                                        AMOUNT = Math.Round(Amount.ToDbl(), 2),
                                                        NAMEITEM = "CM" + KulSiloNo.ToString() + "_ITEMNO",
                                                        ITEMNO = (Int16)satir.LineItemNo,
                                                        FTYPE = (Int16)bunker.CONTYPE,
                                                        ITMPARTNO = "CM" + KulSiloNo.ToString() + "_PARTNO",
                                                        PARTNO = satir.ComponentPartNo
                                                    });
                                                }
                                                else if (cimentoEsleme[0].ITEMNO != (Int16)satir.LineItemNo)
                                                {
                                                    gonderimEsleme.Add(new GonderimEsleme()
                                                    {
                                                        REFID = (Int16)(gonderimEsleme.Count + 1),
                                                        NAMEIST = "CM" + KulSiloNo.ToString() + "_IST",
                                                        AMOUNT = Math.Round(Amount.ToDbl(), 2),
                                                        NAMEITEM = "CM" + KulSiloNo.ToString() + "_ITEMNO",
                                                        ITEMNO = (Int16)satir.LineItemNo,
                                                        FTYPE = (Int16)bunker.CONTYPE,
                                                        ITMPARTNO = "CM" + KulSiloNo.ToString() + "_PARTNO",
                                                        PARTNO = satir.ComponentPartNo
                                                    });
                                                }
                                            }
                                        }
                                        else
                                        {
                                            gonderimEsleme.Add(new GonderimEsleme()
                                            {
                                                REFID = (Int16)(gonderimEsleme.Count + 1),
                                                NAMEIST = bunker.NAMEIST.ToString(),
                                                AMOUNT = Math.Round(Amount.ToDbl(), 2),
                                                NAMEITEM = bunker.NAMEITM.ToString(),
                                                ITEMNO = (Int16)satir.LineItemNo,
                                                FTYPE = (Int16)bunker.CONTYPE,
                                                ITMPARTNO = bunker.ITMPARTNO.ToString(),
                                                PARTNO = satir.ComponentPartNo
                                            });
                                        }
                                        break;
                                    default:
                                        break;
                                }

                            }
                            string ss = gonderimEsleme.Count.ToString();
                        }
                        if (satir.OrderCode.ToUpper().Equals("F"))
                        {

                        }

                    }
                    if (tekilSiparisIcerik != null && tekilSiparisIcerik.Count > 0 && gonderimEsleme.Count > 0)
                    {
                        string FieldNames = "", FieldValues = "", UpdateQuery = "";
                        FieldNames = "KOD,ORDER_NO,RELEASE_NO,SEQUENCE_NO,ORDER_CODE,RECETE_PART_NO, RECETE_PART_DESC,MIKTAR,MIXING_TIME,MIKSER_NO,CUSTOMER_ID, CUSTOMER_NAME, ADRESS_ID, ADRESS_DESC";
                        FieldValues = "(SELECT ISNULL(MAX(KOD),0)+1 FROM IFSPLAN)"
                            + "," + BN.FieldReturnValues(tekilSiparisIcerik[0].OrderNo, 1)
                            + "," + BN.FieldReturnValues(tekilSiparisIcerik[0].ReleaseNo, 1)
                            + "," + BN.FieldReturnValues(tekilSiparisIcerik[0].SequenceNo, 1)
                            + "," + BN.FieldReturnValues(tekilSiparisIcerik[0].OrderCode, 1)
                            + "," + BN.FieldReturnValues(tekilSiparisIcerik[0].PartNo, 1)
                            + "," + BN.FieldReturnValues(BN.KarakterDuzelt(tekilSiparisIcerik[0].PartDesc), 1)
                            + "," + BN.FieldReturnValues(tekilSiparisIcerik[0].RevisedQtyDue.ToString().Replace(',', '.'), 1)
                            + "," + BN.FieldReturnValues(tekilSiparisIcerik[0].MixingTime.ToString().Replace(',', '.'), 1)
                            + "," + BN.FieldReturnValues(tekilSiparisIcerik[0].Mikser.ToString().Replace(',', '.'), 1)
                            + "," + BN.FieldReturnValues(BN.KarakterDuzelt(tekilSiparisIcerik[0].CustomerId), 1)
                            + "," + BN.FieldReturnValues(BN.KarakterDuzelt(tekilSiparisIcerik[0].CustomerName), 1)
                            + "," + BN.FieldReturnValues(BN.KarakterDuzelt(tekilSiparisIcerik[0].AdressId), 1)
                            + "," + BN.FieldReturnValues(BN.KarakterDuzelt(tekilSiparisIcerik[0].AdressDesc), 1)
                            //+ "," + EOFGlobalMethods.FieldReturnValues(KUMAA.ToString().Replace(',', '.'), 2)
                            //+ "," + EOFGlobalMethods.FieldReturnValues(KUMBB.ToString().Replace(',', '.'), 2)
                            //+ "," + EOFGlobalMethods.FieldReturnValues(KUMCC.ToString().Replace(',', '.'), 2)
                            //+ "," + EOFGlobalMethods.FieldReturnValues(KUMDD.ToString().Replace(',', '.'), 2)
                            ;

                        File.WriteAllText("OrnekKAyit.txt", JsonConvert.SerializeObject(gonderimEsleme));
                        foreach (var esleme in gonderimEsleme)
                        {
                            bool hasDouble = false;
                            int ValueInt = 0;
                            double ValueDouble = 0.00;

                            if (esleme.FTYPE == 5)
                            {
                                hasDouble = true;
                                ValueDouble = Convert.ToDouble(Math.Round(Convert.ToDouble(esleme.AMOUNT), 2));
                            }
                            else
                            {
                                hasDouble = false;
                                ValueInt = Convert.ToInt32(Math.Round(Convert.ToDouble(esleme.AMOUNT), 0));
                            }

                            FieldNames = FieldNames + "," + esleme.NAMEIST.ToString() + "," + esleme.NAMEITEM.ToString()
                                                    + "," + esleme.ITMPARTNO.ToString();
                            FieldValues = FieldValues + "," + BN.FieldReturnValues((hasDouble == false ? ValueInt.ToString() : ValueDouble.ToString()).Replace(',', '.'), 1)
                                                    + "," + BN.FieldReturnValues(esleme.ITEMNO.ToString().Replace(',', '.'), 1)
                                                    + "," + BN.FieldReturnValues(esleme.PARTNO.ToString().Replace(',', '.'), 1);
                        }
                        UpdateQuery = "INSERT INTO dbo.IFSPLAN (" + FieldNames + ") VALUES (" + FieldValues + ")";
                        File.WriteAllText("OrnekSorgu" + tekilSiparisIcerik[0].OrderNo + ".txt", UpdateQuery);
                        int affected = ctx.Database.ExecuteSqlCommand(UpdateQuery);
                        if (affected <= 0)
                        {
                            MessageBox.Show(tekilSiparisIcerik[0].OrderNo + " nolu İş Emri eklenemedi.");
                        }
                    }
                }



                _logger.LogInfo("İş emri senkronizasyonu tamamlandı");
                //return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("İş emri senkronizasyonu hatası", ex);
                //return false;
            }

            //var siloIcerik = await ctx.SILO_ADs.SingleAsync(x => x.KOD.Equals("1"));
            //var configIcerik = await ctx.CONFIGs.FirstAsync();

            //if (siloIcerik == null)
            //{
            //    MessageBox.Show("Silo tanımları yapılmalı.", "UYARI", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //    _logger.LogInfo("Silo tanımları yapılmalı...");
            //    throw new Exception("Silo tanımları yapılmalı...");
            //}

            //var validation = await BN.ValidatePlanLinesAsync(ctx, siparisler);

            //if (!validation.IsValid)
            //{
            //    _logger.LogError(validation.Message);
            //    MessageBox.Show("SILO_AD ve IFSPlan arasında uyuşmayan hammadde kodları tespit edildi. İşleme devam edilemiyor.\n" +
            //        validation.Message, "UYARI", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //    // Örnek: kullanıcıya bilgi ver, işleme devam etme
            //    // return / throw / dialog vs.
            //    throw new InvalidOperationException(
            //        "SILO_AD ve IFSPlan arasında uyuşmayan hammadde kodları tespit edildi. İşleme devam edilemiyor.\n" +
            //        validation.Message);
            //}

            //var validationSilo = await BN.ValidateSiloadAsync(ctx);

            //if (!validationSilo.IsValid)
            //{
            //    _logger.LogError(validation.Message);
            //    MessageBox.Show("SILO_AD sistem tanımları arasında uyuşmayan hammadde kodları tespit edildi. İşleme devam edilemiyor.\n" +
            //        validation.Message, "UYARI", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //    // Örnek: kullanıcıya bilgi ver, işleme devam etme
            //    // return / throw / dialog vs.
            //    throw new InvalidOperationException(
            //        "SILO_AD sistem tanımları arasında uyuşmayan hammadde kodları tespit edildi. İşleme devam edilemiyor.\n" +
            //        validation.Message);
            //}

            //var sipariss = siparisler.Select(x => x.OrderNo)
            //                .Distinct()
            //                .OrderBy(x => x)
            //                .ToList();
            //foreach (string item in sipariss)
            //{
            //    IFSPLAN aktifPlan = await ctx.IFSPLANs.Where(x => x.ORDER_NO.ToUpper().Equals(item.ToUpper())).FirstOrDefaultAsync();
            //    if (aktifPlan != null) break;

            //    List<IFSPLANLine> tekilSiparisIcerik = siparisler.Where(x => x.OrderNo.Equals(item)).ToList();
            //    List<GonderimEsleme> gonderimEsleme = new List<GonderimEsleme>();
            //    int ComponentCount = 0;
            //    foreach (IFSPLANLine satir in tekilSiparisIcerik)
            //    {
            //        if (satir.OrderCode.ToUpper().Equals("M"))
            //        {
            //            MALZEME aktifMalzeme = await ctx.MALZEMEs.Where(x => x.PART_NO.Equals(satir.ComponentPartNo)).FirstOrDefaultAsync();
            //            if (aktifMalzeme == null) break;
            //            List<SiloAdlari> uretimMalzemeleri = AppGlobals.siloList.Where(x => x.IFSPARTNO.ToUpper().Equals(satir.ComponentPartNo)).ToList();
            //            if (uretimMalzemeleri == null || uretimMalzemeleri.Count == 0) break;
            //            foreach (SiloAdlari bunker in uretimMalzemeleri)
            //            {
            //                decimal Amount = 0;
            //                Amount = Math.Round(satir.QtyPerAssembly.ToDecimal(), 0);
            //                switch (bunker.CONTYPE)
            //                {
            //                    case enumBunkerIcerikTipi.Agrega:
            //                        if (bunker.ID == 1)
            //                            Amount = (Amount * (Convert.ToDecimal(configIcerik.AG1BLMYZDE) / 100));
            //                        if (bunker.ID == 2)
            //                            Amount = (Amount * (Convert.ToDecimal(configIcerik.AG2BLMYZDE) / 100));
            //                        if (bunker.ID == 3)
            //                            Amount = (Amount * (Convert.ToDecimal(configIcerik.AG3BLMYZDE) / 100));
            //                        if (bunker.ID == 4)
            //                            Amount = (Amount * (Convert.ToDecimal(configIcerik.AG4BLMYZDE) / 100));
            //                        if (bunker.ID == 5)
            //                            Amount = (Amount * (Convert.ToDecimal(configIcerik.AG5BLMYZDE) / 100));
            //                        if (bunker.ID == 6)
            //                            Amount = (Amount * (Convert.ToDecimal(configIcerik.AG6BLMYZDE) / 100));
            //                        _logger.LogInfo(item + "|" + bunker.DEFINITION_ + "|" + satir.ComponentPartNo + "|" + Amount.ToString() + "|" + bunker.NAMEIST.ToString() + " \r\n");
            //                        gonderimEsleme.Add(new GonderimEsleme()
            //                        {
            //                            REFID = (Int16)(gonderimEsleme.Count + 1),
            //                            NAMEIST = bunker.NAMEIST.ToString(),
            //                            AMOUNT = Math.Round(Amount.ToDbl(), 0),
            //                            NAMEITEM = bunker.NAMEITM.ToString(),
            //                            ITEMNO = (Int16)aktifMalzeme.KOD,
            //                            FTYPE = (Int16)bunker.CONTYPE,
            //                            ITMPARTNO = bunker.ITMPARTNO.ToString(),
            //                            PARTNO = satir.ComponentPartNo
            //                        });
            //                        break;
            //                    case enumBunkerIcerikTipi.Çimento_Kül:
            //                        bool TmpCC = false;
            //                        if (aktifMalzeme.PART_NO.StartsWith("04"))
            //                        {
            //                            int CimentoSiloNo = Convert.ToInt32(configIcerik.CIMENTOSILO);
            //                            if (aktifMalzeme.KUL.ToStr().ToUpper().Contains("FALSE") &&
            //                                aktifMalzeme.CIMENTO.ToStr().ToUpper().Contains("TRUE") &&
            //                                CimentoSiloNo > 0)
            //                            {
            //                                List<GonderimEsleme> cimentoEsleme = gonderimEsleme.Where(x => x.FTYPE == 2 &&
            //                                                                    x.NAMEIST.ToUpper().Equals("CM" + CimentoSiloNo.ToString())).ToList();
            //                                if (cimentoEsleme.Count == 0)
            //                                {
            //                                    gonderimEsleme.Add(new GonderimEsleme()
            //                                    {
            //                                        REFID = (Int16)(gonderimEsleme.Count + 1),
            //                                        NAMEIST = "CM" + CimentoSiloNo.ToString() + "_IST",
            //                                        AMOUNT = Math.Round(Amount.ToDbl(), 0),
            //                                        NAMEITEM = "CM" + CimentoSiloNo.ToString() + "_ITEMNO",
            //                                        ITEMNO = (Int16)aktifMalzeme.KOD,
            //                                        FTYPE = (Int16)bunker.CONTYPE,
            //                                        ITMPARTNO = "CM" + CimentoSiloNo.ToString() + "_PARTNO",
            //                                        PARTNO = satir.ComponentPartNo
            //                                    });
            //                                }
            //                                else if (cimentoEsleme[0].ITEMNO != (Int16)satir.LineItemNo)
            //                                {
            //                                    gonderimEsleme.Add(new GonderimEsleme()
            //                                    {
            //                                        REFID = (Int16)(gonderimEsleme.Count + 1),
            //                                        NAMEIST = "CM" + CimentoSiloNo.ToString() + "_IST",
            //                                        AMOUNT = Math.Round(Amount.ToDbl(), 0),
            //                                        NAMEITEM = "CM" + CimentoSiloNo.ToString() + "_ITEMNO",
            //                                        ITEMNO = (Int16)aktifMalzeme.KOD,
            //                                        FTYPE = (Int16)bunker.CONTYPE,
            //                                        ITMPARTNO = "CM" + CimentoSiloNo.ToString() + "_PARTNO",
            //                                        PARTNO = satir.ComponentPartNo
            //                                    });
            //                                }
            //                            }
            //                        }
            //                        else if (aktifMalzeme.PART_NO.StartsWith("06"))
            //                        {
            //                            int KulSiloNo = Convert.ToInt32(configIcerik.KULSILO);
            //                            if (aktifMalzeme.KUL.ToStr().ToUpper().Contains("TRUE") &&
            //                                aktifMalzeme.CIMENTO.ToStr().ToUpper().Contains("FALSE") &&
            //                                KulSiloNo > 0)
            //                            {
            //                                List<GonderimEsleme> cimentoEsleme = gonderimEsleme.Where(x => x.FTYPE == 2 &&
            //                                                                    x.NAMEIST.ToUpper().Equals("CM" + KulSiloNo.ToString())).ToList();
            //                                if (cimentoEsleme.Count == 0)
            //                                {
            //                                    gonderimEsleme.Add(new GonderimEsleme()
            //                                    {
            //                                        REFID = (Int16)(gonderimEsleme.Count + 1),
            //                                        NAMEIST = "CM" + KulSiloNo.ToString() + "_IST",
            //                                        AMOUNT = Math.Round(Amount.ToDbl(), 0),
            //                                        NAMEITEM = "CM" + KulSiloNo.ToString() + "_ITEMNO",
            //                                        ITEMNO = (Int16)aktifMalzeme.KOD,
            //                                        FTYPE = (Int16)bunker.CONTYPE,
            //                                        ITMPARTNO = "CM" + KulSiloNo.ToString() + "_PARTNO",
            //                                        PARTNO = satir.ComponentPartNo
            //                                    });
            //                                }
            //                                else if (cimentoEsleme[0].ITEMNO != (Int16)satir.LineItemNo)
            //                                {
            //                                    gonderimEsleme.Add(new GonderimEsleme()
            //                                    {
            //                                        REFID = (Int16)(gonderimEsleme.Count + 1),
            //                                        NAMEIST = "CM" + KulSiloNo.ToString() + "_IST",
            //                                        AMOUNT = Math.Round(Amount.ToDbl(), 0),
            //                                        NAMEITEM = "CM" + KulSiloNo.ToString() + "_ITEMNO",
            //                                        ITEMNO = (Int16)aktifMalzeme.KOD,
            //                                        FTYPE = (Int16)bunker.CONTYPE,
            //                                        ITMPARTNO = "CM" + KulSiloNo.ToString() + "_PARTNO",
            //                                        PARTNO = satir.ComponentPartNo
            //                                    });
            //                                }
            //                            }
            //                        }
            //                        break;
            //                    case enumBunkerIcerikTipi.Su1:
            //                        double RecycleAmount = 0, NeedAmount = 0;
            //                        try
            //                        {
            //                            RecycleAmount = (Amount.ToDbl() * (Convert.ToDouble(satir.RecycleUsageFactor.ToDecimal()) / 100));
            //                        }
            //                        catch
            //                        {
            //                            RecycleAmount = 0;
            //                        }

            //                        NeedAmount = Amount.ToDbl() - RecycleAmount;
            //                        List<GonderimEsleme> suEsleme = gonderimEsleme.Where(x => x.FTYPE == 3 &&
            //                                                    x.NAMEIST.ToUpper().Equals("SU1_IST")).ToList();
            //                        if (suEsleme.Count == 0)
            //                        {
            //                            gonderimEsleme.Add(new GonderimEsleme()
            //                            {
            //                                REFID = (Int16)(gonderimEsleme.Count + 1),
            //                                NAMEIST = "SU1_IST",
            //                                AMOUNT = Math.Round(NeedAmount.ToDbl(), 0),
            //                                NAMEITEM = "SU1_ITEMNO",
            //                                ITEMNO = (Int16)aktifMalzeme.KOD,
            //                                FTYPE = (Int16)bunker.CONTYPE,
            //                                ITMPARTNO = "SU1_PARTNO",
            //                                PARTNO = satir.ComponentPartNo
            //                            });
            //                            gonderimEsleme.Add(new GonderimEsleme()
            //                            {
            //                                REFID = (Int16)(gonderimEsleme.Count + 1),
            //                                NAMEIST = "SU2_IST",
            //                                AMOUNT = Math.Round(RecycleAmount.ToDbl(), 0),
            //                                NAMEITEM = "SU2_ITEMNO",
            //                                ITEMNO = (Int16)aktifMalzeme.KOD,
            //                                FTYPE = (Int16)bunker.CONTYPE,
            //                                ITMPARTNO = "SU2_PARTNO",
            //                                PARTNO = satir.ComponentPartNo
            //                            });
            //                        }
            //                        break;
            //                    case enumBunkerIcerikTipi.Su2:
            //                        break;
            //                    case enumBunkerIcerikTipi.Katkı:
            //                        if (aktifMalzeme.PART_NO.StartsWith("06"))
            //                        {
            //                            int KulSiloNo = Convert.ToInt32(configIcerik.KULSILO);
            //                            if (aktifMalzeme.KUL.ToStr().ToUpper().Contains("TRUE") &&
            //                                aktifMalzeme.CIMENTO.ToStr().ToUpper().Contains("FALSE") &&
            //                                KulSiloNo > 0)
            //                            {
            //                                List<GonderimEsleme> cimentoEsleme = gonderimEsleme.Where(x => x.FTYPE == 2 &&
            //                                                                    x.NAMEIST.ToUpper().Equals("CM" + KulSiloNo.ToString())).ToList();
            //                                if (cimentoEsleme.Count == 0)
            //                                {
            //                                    gonderimEsleme.Add(new GonderimEsleme()
            //                                    {
            //                                        REFID = (Int16)(gonderimEsleme.Count + 1),
            //                                        NAMEIST = "CM" + KulSiloNo.ToString() + "_IST",
            //                                        AMOUNT = Math.Round(Amount.ToDbl(), 0),
            //                                        NAMEITEM = "CM" + KulSiloNo.ToString() + "_ITEMNO",
            //                                        ITEMNO = (Int16)aktifMalzeme.KOD,
            //                                        FTYPE = (Int16)bunker.CONTYPE,
            //                                        ITMPARTNO = "CM" + KulSiloNo.ToString() + "_PARTNO",
            //                                        PARTNO = satir.ComponentPartNo
            //                                    });
            //                                }
            //                                else if (cimentoEsleme[0].ITEMNO != (Int16)satir.LineItemNo)
            //                                {
            //                                    gonderimEsleme.Add(new GonderimEsleme()
            //                                    {
            //                                        REFID = (Int16)(gonderimEsleme.Count + 1),
            //                                        NAMEIST = "CM" + KulSiloNo.ToString() + "_IST",
            //                                        AMOUNT = Math.Round(Amount.ToDbl(), 0),
            //                                        NAMEITEM = "CM" + KulSiloNo.ToString() + "_ITEMNO",
            //                                        ITEMNO = (Int16)aktifMalzeme.KOD,
            //                                        FTYPE = (Int16)bunker.CONTYPE,
            //                                        ITMPARTNO = "CM" + KulSiloNo.ToString() + "_PARTNO",
            //                                        PARTNO = satir.ComponentPartNo
            //                                    });
            //                                }
            //                            }
            //                        }
            //                        else
            //                        {
            //                            gonderimEsleme.Add(new GonderimEsleme()
            //                            {
            //                                REFID = (Int16)(gonderimEsleme.Count + 1),
            //                                NAMEIST = bunker.NAMEIST.ToString(),
            //                                AMOUNT = Math.Round(Amount.ToDbl(), 0),
            //                                NAMEITEM = bunker.NAMEITM.ToString(),
            //                                ITEMNO = (Int16)aktifMalzeme.KOD,
            //                                FTYPE = (Int16)bunker.CONTYPE,
            //                                ITMPARTNO = bunker.ITMPARTNO.ToString(),
            //                                PARTNO = satir.ComponentPartNo
            //                            });
            //                        }
            //                        break;
            //                    default:
            //                        break;
            //                }

            //            }
            //            string ss = gonderimEsleme.Count.ToString();
            //        }
            //        if (satir.OrderCode.ToUpper().Equals("F"))
            //        {

            //        }

            //    }
            //    if (tekilSiparisIcerik != null && tekilSiparisIcerik.Count > 0 && gonderimEsleme.Count > 0)
            //    {
            //        string FieldNames = "", FieldValues = "", UpdateQuery = "";
            //        FieldNames = "KOD,ORDER_NO,RELEASE_NO,SEQUENCE_NO,ORDER_CODE,RECETE_PART_NO, RECETE_PART_DESC,MIKTAR,MIXING_TIME,MIKSER_NO,CUSTOMER_ID, CUSTOMER_NAME, ADRESS_ID, ADRESS_DESC";
            //        FieldValues = "(SELECT ISNULL(MAX(KOD),0)+1 FROM IFSPLAN)"
            //            + "," + BN.FieldReturnValues(tekilSiparisIcerik[0].OrderNo, 1)
            //            + "," + BN.FieldReturnValues(tekilSiparisIcerik[0].ReleaseNo, 1)
            //            + "," + BN.FieldReturnValues(tekilSiparisIcerik[0].SequenceNo, 1)
            //            + "," + BN.FieldReturnValues(tekilSiparisIcerik[0].OrderCode, 1)
            //            + "," + BN.FieldReturnValues(tekilSiparisIcerik[0].PartNo, 1)
            //            + "," + BN.FieldReturnValues(BN.KarakterDuzelt(tekilSiparisIcerik[0].PartDesc), 1)
            //            + "," + BN.FieldReturnValues(tekilSiparisIcerik[0].RevisedQtyDue.ToString().Replace(',', '.'), 1)
            //            + "," + BN.FieldReturnValues(tekilSiparisIcerik[0].MixingTime.ToString().Replace(',', '.'), 1)
            //            + "," + BN.FieldReturnValues(tekilSiparisIcerik[0].Mikser.ToString().Replace(',', '.'), 1)
            //            + "," + BN.FieldReturnValues(BN.KarakterDuzelt(tekilSiparisIcerik[0].CustomerId), 1)
            //            + "," + BN.FieldReturnValues(BN.KarakterDuzelt(tekilSiparisIcerik[0].CustomerName), 1)
            //            + "," + BN.FieldReturnValues(BN.KarakterDuzelt(tekilSiparisIcerik[0].AdressId), 1)
            //            + "," + BN.FieldReturnValues(BN.KarakterDuzelt(tekilSiparisIcerik[0].AdressDesc), 1)
            //            //+ "," + EOFGlobalMethods.FieldReturnValues(KUMAA.ToString().Replace(',', '.'), 2)
            //            //+ "," + EOFGlobalMethods.FieldReturnValues(KUMBB.ToString().Replace(',', '.'), 2)
            //            //+ "," + EOFGlobalMethods.FieldReturnValues(KUMCC.ToString().Replace(',', '.'), 2)
            //            //+ "," + EOFGlobalMethods.FieldReturnValues(KUMDD.ToString().Replace(',', '.'), 2)
            //            ;

            //        File.WriteAllText("OrnekKAyit.txt", JsonConvert.SerializeObject(gonderimEsleme));
            //        foreach (var esleme in gonderimEsleme)
            //        {
            //            bool hasDouble = false;
            //            int ValueInt = 0;
            //            double ValueDouble = 0.00;

            //            if (esleme.FTYPE == 5)
            //            {
            //                hasDouble = true;
            //                ValueDouble = Convert.ToDouble(Math.Round(Convert.ToDouble(esleme.AMOUNT), 2));
            //            }
            //            else
            //            {
            //                hasDouble = false;
            //                ValueInt = Convert.ToInt32(Math.Round(Convert.ToDouble(esleme.AMOUNT), 0));
            //            }

            //            FieldNames = FieldNames + "," + esleme.NAMEIST.ToString() + "," + esleme.NAMEITEM.ToString()
            //                                    + "," + esleme.ITMPARTNO.ToString();
            //            FieldValues = FieldValues + "," + BN.FieldReturnValues((hasDouble == false ? ValueInt.ToString() : ValueDouble.ToString()).Replace(',', '.'), 1)
            //                                    + "," + BN.FieldReturnValues(esleme.ITEMNO.ToString().Replace(',', '.'), 1)
            //                                    + "," + BN.FieldReturnValues(esleme.PARTNO.ToString().Replace(',', '.'), 1);
            //        }
            //        UpdateQuery = "INSERT INTO dbo.IFSPLAN (" + FieldNames + ") VALUES (" + FieldValues + ")";
            //        File.WriteAllText("OrnekSorgu" + tekilSiparisIcerik[0].OrderNo + ".txt", UpdateQuery);
            //        int affected = ctx.Database.ExecuteSqlCommand(UpdateQuery);
            //        if (affected <= 0)
            //        {
            //            MessageBox.Show(tekilSiparisIcerik[0].OrderNo + " nolu İş Emri eklenemedi.");
            //        }
            //    }
            //}


        }

        private async void btnPeriyotListesiGetir_Click(object sender, EventArgs e)
        {
            using var ctx = new TesisContext();
            //var list = await ctx.PERDETAYs
            //    .AsNoTracking()
            //    .Select(x => new { x.PERKOD })
            //    .ToListAsync();
            var listt = await ctx.Database.SqlQuery<PERDETAY>(
                        "SELECT * FROM PERDETAY ORDER BY URTKOD"
                    ).ToListAsync();
            //List<PERDETAY> perdets = await ctx.PERDETAYs.ToListAsync();
            grdPeriyotlar.DataSource = listt;
        }
        String HataKonum = "";
        private async void btnPeriyotlarMerkezeGonder_Click(object sender, EventArgs e)
        {
            PERDETAY aktifPer = (PERDETAY)gviewPeriyotlar.GetFocusedRow();
            if (aktifPer == null) return;
            var apiService = new RestApiService(AppGlobals.appSettings.Api, _logger);
            UretimService urs = new UretimService(apiService, _logger);
            await urs.UretimVerisiGonder();
        }

        private void gviewPeriyotlar_DoubleClick(object sender, EventArgs e)
        {

        }

        private void contextMalzeme_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            grdMalzeme.ExportToXlsx("MalzemeExcel.xlsx");
        }

        private void exceleAktarToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            grdSiparis.ExportToXlsx("SiparisExcel.xlsx");
        }
    }
}