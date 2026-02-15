using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ENOYAEntegrasyonV2.Business;
using ENOYAEntegrasyonV2.DbContxt;
using ENOYAEntegrasyonV2.Models.Configuration;
using ENOYAEntegrasyonV2.Models.Entities;
using ENOYAEntegrasyonV2.Services.Interfaces;
using Newtonsoft.Json;

namespace ENOYAEntegrasyonV2.Forms
{
    public partial class SettingsForm : Form
    {
        private readonly IConfigurationService _configService;
        private readonly ILoggerService _logger;
        List<MALZEME> malzemeler = new List<MALZEME>();
        // Form'un içinde field olarak tanımla
        private List<SiloAdlari> _siloList;
        List<KolonMalzemeEsleme> kolonEsleme = new List<KolonMalzemeEsleme>();

        public SettingsForm(IConfigurationService configService, ILoggerService logger)
        {
            _configService = configService ?? throw new ArgumentNullException(nameof(configService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            InitializeComponent();
            LoadSettings();
            BunkerDetaylar();
        }

        private void LoadSettings()
        {
            //_settings = _configService.GetSettings();

            // Database ayarları
            txtDbServer.Text = AppGlobals.appSettings.Database.Server;
            txtDbDatabase.Text = AppGlobals.appSettings.Database.Database;
            txtDbUser.Text = AppGlobals.appSettings.Database.UserId;
            txtDbPassword.Text = AppGlobals.appSettings.Database.Password;
            chkDbIntegratedSecurity.Checked = AppGlobals.appSettings.Database.IntegratedSecurity;
            txtDbUser.Enabled = !chkDbIntegratedSecurity.Checked;
            txtDbPassword.Enabled = !chkDbIntegratedSecurity.Checked;

            // API ayarları
            txtApiBaseUrl.Text = AppGlobals.appSettings.Api.BaseUrl;
            txtApiClientId.Text = AppGlobals.appSettings.Api.ClientId;
            txtApiClientSecret.Text = AppGlobals.appSettings.Api.ClientSecret;
            txtApiContract.Text = AppGlobals.appSettings.Api.Contract;
            txtTokenEndPoint.Text = AppGlobals.appSettings.Api.TokenUrl;

            //Genel Ayarlar
            teAlternativeRoute.Text = AppGlobals.appSettings.General.AlternativeRoute;
            chkUseAlternativeRoute.Checked = AppGlobals.appSettings.General.UseAlternativeRoute;
            chkAutoStart.Checked = AppGlobals.appSettings.General.AutoStartIntegration;
            numInterval.Value = Convert.ToDecimal(AppGlobals.appSettings.General.IntegrationIntervalSeconds);
            chkMinimizeToTray.Checked = AppGlobals.appSettings.General.MinimizeToTray;
        }

        async void BunkerDetaylar()
        {

            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.Columns.Clear();

            using var ctx = new TesisContext();

            malzemeler.Add(new MALZEME() { KOD = -1, AD = "Seçiniz...", PART_NO = "-1", PROD_FAMILY = "-1" });
            var list = await ctx.MALZEMEs.ToListAsync();
            foreach (var item in list)
            {
                malzemeler.Add(item);
            }

            // ID Kolonu
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colID",
                DataPropertyName = "ID",
                HeaderText = "ID",
                Width = 50,
                Frozen = true,
                ReadOnly = true,
            });

            // Tanım (DEFINITION_) kolonu
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colDefinition",
                DataPropertyName = "DEFINITION_",
                HeaderText = "Tanım",
                Width = 150,
                Frozen = true,
                ReadOnly = true,
            });

            // İstersen diğer property’ler için de benzer kolonlar ekleyebilirsin...

            // 3. adımda ekleyeceğimiz ComboBox kolonu:
            var colSiloCombo = new DataGridViewComboBoxColumn
            {
                Name = "colMalzemeSec",
                HeaderText = "Malzeme Seç",
                DataSource = malzemeler,          // item'ler
                DisplayMember = "AD",   // kullanıcıya görünen
                ValueMember = "PART_NO",               // hücre değeri
                DataPropertyName = "IFSPARTNO",      // DataPropertyName istersen başka bir property ile bağlanabilir
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            };

            dataGridView1.Columns.Add(colSiloCombo);

            if (File.Exists("SiloTanimlar.json"))
                _siloList = JsonConvert.DeserializeObject<List<SiloAdlari>>(File.ReadAllText("SiloTanimlar.json"));
            else
                _siloList = SiloAdlari.listSiloAdlari();

            // Şimdi DataGridView’in satırlarını listen ile doldur:
            dataGridView1.DataSource = _siloList;

        }

        private void chkDbIntegratedSecurity_CheckedChanged(object sender, System.EventArgs e)
        {
            txtDbUser.Enabled = !chkDbIntegratedSecurity.Checked;
            txtDbPassword.Enabled = !chkDbIntegratedSecurity.Checked;
        }

        private async void BtnTestDatabase_Click(object sender, EventArgs e)
        {
            try
            {
                var testSettings = new DatabaseSettings
                {
                    Server = txtDbServer.Text,
                    Database = txtDbDatabase.Text,
                    UserId = txtDbUser.Text,
                    Password = txtDbPassword.Text,
                    IntegratedSecurity = chkDbIntegratedSecurity.Checked
                };

                using (var dbService = new Services.Database.SqlServerService(testSettings))
                {
                    var result = await dbService.TestConnectionAsync();
                    if (result)
                    {
                        MessageBox.Show("MSSQL bağlantı testi başarılı!", "Başarılı",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        _logger.LogInfo("MSSQL bağlantı testi başarılı");
                    }
                    else
                    {
                        MessageBox.Show("MSSQL bağlantı testi başarısız!", "Hata",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        _logger.LogWarning("MSSQL bağlantı testi başarısız");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bağlantı hatası:\n\n{ex.Message}", "Hata",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                _logger.LogError("MSSQL bağlantı testi hatası", ex);
            }
        }

        private async void BtnTestApi_Click(object sender, EventArgs e)
        {
            try
            {
                var testSettings = new ApiSettings
                {
                    BaseUrl = txtApiBaseUrl.Text,
                    ClientId = txtApiClientId.Text,
                    ClientSecret = txtApiClientSecret.Text,
                    Contract = txtApiContract.Text,
                    TokenUrl = txtTokenEndPoint.Text
                };

                var apiService = new Services.Api.RestApiService(testSettings, _logger);
                var token = await apiService.GetAccessTokenAsync();

                if (!string.IsNullOrEmpty(token))
                {
                    MessageBox.Show("REST API bağlantı testi başarılı!", "Başarılı",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _logger.LogInfo("REST API bağlantı testi başarılı");
                }
                else
                {
                    MessageBox.Show("REST API bağlantı testi başarısız!", "Hata",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"API bağlantı hatası:\n\n{ex.Message}", "Hata",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                _logger.LogError("REST API bağlantı testi hatası", ex);
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            bool Okmi = false;
            try
            {
                // Ayarları güncelle
                AppGlobals.appSettings.Database.Server = txtDbServer.Text;
                AppGlobals.appSettings.Database.Database = txtDbDatabase.Text;
                AppGlobals.appSettings.Database.UserId = txtDbUser.Text;
                AppGlobals.appSettings.Database.Password = txtDbPassword.Text;
                AppGlobals.appSettings.Database.IntegratedSecurity = chkDbIntegratedSecurity.Checked;

                AppGlobals.appSettings.Api.BaseUrl = txtApiBaseUrl.Text;
                AppGlobals.appSettings.Api.ClientId = txtApiClientId.Text;
                AppGlobals.appSettings.Api.ClientSecret = txtApiClientSecret.Text;
                AppGlobals.appSettings.Api.Contract = txtApiContract.Text;
                AppGlobals.appSettings.Api.TokenUrl = txtTokenEndPoint.Text;

                AppGlobals.appSettings.General.AlternativeRoute = teAlternativeRoute.Text;
                AppGlobals.appSettings.General.UseAlternativeRoute = chkUseAlternativeRoute.Checked;
                AppGlobals.appSettings.General.AutoStartIntegration = chkAutoStart.Checked;
                AppGlobals.appSettings.General.IntegrationIntervalSeconds = Convert.ToInt32(numInterval.Value);
                AppGlobals.appSettings.General.MinimizeToTray = chkMinimizeToTray.Checked;

                // Kaydet
                _configService.SaveSettings(AppGlobals.appSettings);
                _logger.LogInfo("Ayarlar kaydedildi");

                // Şifreleme testi - JSON dosyasını kontrol et
                string configPath = System.IO.Path.Combine(
                    System.AppDomain.CurrentDomain.BaseDirectory,
                    "AppSettings.json");

                if (System.IO.File.Exists(configPath))
                {
                    string jsonContent = System.IO.File.ReadAllText(configPath);
                    bool passwordEncrypted = !jsonContent.Contains(txtDbPassword.Text) || string.IsNullOrEmpty(txtDbPassword.Text);
                    bool secretEncrypted = !jsonContent.Contains(txtApiClientSecret.Text) || string.IsNullOrEmpty(txtApiClientSecret.Text);

                    string message = "Ayarlar başarıyla kaydedildi!";
                    if (passwordEncrypted && secretEncrypted && (!string.IsNullOrEmpty(txtDbPassword.Text) || !string.IsNullOrEmpty(txtApiClientSecret.Text)))
                    {
                        message += "\n\n✓ Şifreleme başarılı: SQL şifresi ve Client Secret şifrelenmiş olarak kaydedildi.";
                    }
                    AppGlobals.appSettings.General.MinimizeToTray = false;
                    MessageBox.Show(message+ "\r\n"
                        + "Uygulama yeniden başlatılıyor...", "Başarılı",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Okmi = true;
                }
                else
                {
                    MessageBox.Show("Ayarları kayıt işleminde sorun oluştu.", "Hatalı işlem",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ayarlar kaydedilemedi:\n\n{ex.Message}", "Hata",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                _logger.LogError("Ayarlar kaydetme hatası", ex);
            }
            if (Okmi)
                Application.Restart();
        }

        private void chkUseAlternativeRoute_CheckedChanged(object sender, EventArgs e)
        {
            teAlternativeRoute.Enabled = chkUseAlternativeRoute.Checked;
        }

        private void btnSiloKaydet_Click(object sender, EventArgs e)
        {
            if (File.Exists("SiloTanimlar.json"))
            {
                File.Delete("SiloTanimlar.json");
            }
            foreach (var item in _siloList)
            {
                if (!item.IFSPARTNO.Equals("-1"))
                {
                    MALZEME sm = malzemeler.Where(x => x.PART_NO.Equals(item.IFSPARTNO)).FirstOrDefault();
                    if (item.ID >= 1 && item.ID <= 6)//(sm != null && sm.PROD_FAMILY.)
                    {
                        if (sm.PROD_FAMILY.ToUpper().Contains("KUM") || sm.PROD_FAMILY.ToUpper().Contains("MICIR"))
                        {
                            int prodCount = _siloList.Where(x => x.IFSPARTNO.ToUpper().Equals(item.IFSPARTNO)).Count();
                            kolonEsleme.Add(new KolonMalzemeEsleme()
                            {
                                IFSPARTNO = item.IFSPARTNO,
                                ALAN = item.IDCOLUMNNAME.ToString(),
                                YUZDE = 100,
                                SAYI = prodCount
                            });
                        }
                        else
                        {
                            MessageBox.Show("Agrega, Kum veya Mıcır Bunkerlerine " + sm.PROD_FAMILY + " türü eklenemez.", "Bunker Eşleşme Hatası");
                            return;
                        }
                    }
                    if (item.ID >= 7 && item.ID <= 10)//(sm != null && sm.PROD_FAMILY.)
                    {
                        if (sm.PROD_FAMILY.ToUpper().Contains("MENTO") || sm.PROD_FAMILY.ToUpper().Contains("KUL") || sm.PROD_FAMILY.ToUpper().Contains("NERAL"))
                        {
                        }
                        else
                        {
                            MessageBox.Show("Çimento Bunkerlerine " + sm.PROD_FAMILY + " türü eklenemez.", "Bunker Eşleşme Hatası");
                            return;
                        }
                    }
                    if (item.ID >= 11 && item.ID <= 12)//(sm != null && sm.PROD_FAMILY.)
                    {
                        if (sm.PROD_FAMILY.ToUpper().Contains("SU"))
                        {
                        }
                        else
                        {
                            MessageBox.Show("Su Bunkerlerine " + sm.PROD_FAMILY + " türü eklenemez.", "Bunker Eşleşme Hatası");
                            return;
                        }
                    }
                    if (item.ID >= 13 && item.ID <= 16)//(sm != null && sm.PROD_FAMILY.)
                    {
                        if (sm.PROD_FAMILY.ToUpper().Contains("KATK"))
                        {
                        }
                        else
                        {
                            MessageBox.Show("Katkı Bunkerlerine " + sm.PROD_FAMILY + " türü eklenemez.", "Bunker Eşleşme Hatası");
                            return;
                        }
                    }
                }
            }

            File.WriteAllText("SiloTanimlar.json", JsonConvert.SerializeObject(_siloList));

            //string FieldNames = "", FieldValues = "", UpdateString = "";
            //DataTable dtTemporaryG = new DataTable();
            //dtTemporaryG.Columns.Add("KOD", typeof(System.String));
            //dtTemporaryG.Columns.Add("ALAN", typeof(System.String));
            //dtTemporaryG.Columns.Add("YUZDE", typeof(System.Int16));
            //dtTemporaryG.Columns.Add("SAYI", typeof(System.Int16));

            try
            {
                using var ctx = new TesisContext();
                string removeQuery = "";
                string fieldName = "";
                string valueName = "";
                string updateQuery = "";

                foreach (var item in _siloList)
                {
                    if (!String.IsNullOrEmpty(item.IFSPARTNO) && !item.IFSPARTNO.Equals("-1"))
                    {
                        fieldName = String.IsNullOrEmpty(fieldName) ? item.IDCOLUMNNAME.ToString() : fieldName + " ," + item.IDCOLUMNNAME.ToString();
                        valueName = String.IsNullOrEmpty(valueName) ?
                            "'" + item.IFSPARTNO.ToString() + "'" :
                            valueName + " ,'" + item.IFSPARTNO.ToString() + "'";
                        updateQuery = String.IsNullOrEmpty(updateQuery) ?
                            item.IDCOLUMNNAME.ToString() + " = '" + item.IFSPARTNO.ToString() + "'" :
                            updateQuery + " ," + item.IDCOLUMNNAME.ToString() + " = '" + item.IFSPARTNO.ToString() + "'";
                    }
                    else
                    {
                        fieldName = String.IsNullOrEmpty(fieldName) ? item.IDCOLUMNNAME.ToString() : fieldName + " ," + item.IDCOLUMNNAME.ToString();
                        valueName = String.IsNullOrEmpty(valueName) ?
                            "'0'" :
                            valueName + " ,'0'";
                        updateQuery = String.IsNullOrEmpty(updateQuery) ?
                            item.IDCOLUMNNAME.ToString() + " = '0'" :
                            updateQuery + " ," + item.IDCOLUMNNAME.ToString() + " = '0'";
                    }
                }
                string sql = "IF NOT EXISTS(SELECT * FROM SILO_AD WHERE KOD = 1)\r\n"
                        + "BEGIN\r\n"
                        + "   DELETE SILO_AD \r\n"
                        + "   INSERT INTO SILO_AD (KOD," + fieldName + ") VALUES (1," + valueName + ")\r\n"
                        + "END\r\n"
                        + "ELSE\r\n"
                        + "BEGIN\r\n"
                        + "    UPDATE SILO_AD SET " + updateQuery + " WHERE KOD = 1\r\n"
                        + "END\r\n"
                        + "";
                int affected = ctx.Database.ExecuteSqlCommand(sql);
                if (affected <= 0)
                {
                    MessageBox.Show("SILO_AD Güncelleme sorunu", "Sistem Parametre Sorunu");
                    return;
                }
                affected = ctx.Database.ExecuteSqlCommand("UPDATE dbo.CONFIG SET BNKGUNCEL = 1, CIMENTOSILO = 0, KULSILO = 0 ");
                if (affected <= 0)
                {
                    MessageBox.Show("dbo.CONFIG Güncelleme sorunu", "Sistem Parametre Sorunu");
                    return;
                }
                MessageBox.Show("SILO_AD Güncelleme işlemi tamamlandı.\r\n"
                    +"Agrega bunkerlerindeki bölüm yüzdelik değerlerini düzeltiniz.", "Sistem Parametre Uyarısı");
                this.Close();

                //var esleme = _siloList.Select(x => new
                //        {
                //            x.IFSPARTNO
                //        }
                //    ).ToList();

                //    for (int dI = 0; dI < gviewMatchItems.RowCount; dI++)
                //    {
                //        if (gviewMatchItems.GetRowCellDisplayText(dI, "SILOID").ToString().Trim() != "")
                //        {
                //            string ItemCode = gviewMatchItems.GetRowCellDisplayText(dI, "HMKODU").ToString().Trim();
                //            int CodeCount = 0;
                //            for (int dP = 0; dP < gviewMatchItems.RowCount; dP++)
                //            {
                //                if (gviewMatchItems.GetRowCellDisplayText(dP, "HMKODU").ToString() == ItemCode)
                //                {
                //                    CodeCount = CodeCount + 1;
                //                }
                //            }

                //            switch (gviewMatchItems.GetRowCellDisplayText(dI, "SILOID").ToString())
                //            {
                //                case "AGREGA1":
                //                    dtTemporaryG.Rows.Add(gviewMatchItems.GetRowCellDisplayText(dI, "HMKODU").ToString(), "AG1BLMYZDE", 100, CodeCount);
                //                    break;
                //                case "AGREGA2":
                //                    dtTemporaryG.Rows.Add(gviewMatchItems.GetRowCellDisplayText(dI, "HMKODU").ToString(), "AG2BLMYZDE", 100, CodeCount);
                //                    break;
                //                case "AGREGA3":
                //                    dtTemporaryG.Rows.Add(gviewMatchItems.GetRowCellDisplayText(dI, "HMKODU").ToString(), "AG3BLMYZDE", 100, CodeCount);
                //                    break;
                //                case "AGREGA4":
                //                    dtTemporaryG.Rows.Add(gviewMatchItems.GetRowCellDisplayText(dI, "HMKODU").ToString(), "AG4BLMYZDE", 100, CodeCount);
                //                    break;
                //                case "AGREGA5":
                //                    dtTemporaryG.Rows.Add(gviewMatchItems.GetRowCellDisplayText(dI, "HMKODU").ToString(), "AG5BLMYZDE", 100, CodeCount);
                //                    break;
                //                case "AGREGA6":
                //                    dtTemporaryG.Rows.Add(gviewMatchItems.GetRowCellDisplayText(dI, "HMKODU").ToString(), "AG6BLMYZDE", 100, CodeCount);
                //                    break;
                //                default:
                //                    break;
                //            }

                //            if (UpdateString == "")
                //                UpdateString = gviewMatchItems.GetRowCellDisplayText(dI, "SILOID").ToString() + " = " + EGM.FieldReturnValues(gviewMatchItems.GetRowCellDisplayText(dI, "HMKODU").ToString(), 1);
                //            else
                //                UpdateString = UpdateString + "," + gviewMatchItems.GetRowCellDisplayText(dI, "SILOID").ToString() + " = " + EGM.FieldReturnValues(gviewMatchItems.GetRowCellDisplayText(dI, "HMKODU").ToString(), 1);

                //            if (FieldNames == "")
                //                FieldNames = gviewMatchItems.GetRowCellDisplayText(dI, "SILOID").ToString();
                //            else
                //                FieldNames = FieldNames + "," + gviewMatchItems.GetRowCellDisplayText(dI, "SILOID").ToString();

                //            if (FieldValues == "")
                //                FieldValues = EGM.FieldReturnValues(gviewMatchItems.GetRowCellDisplayText(dI, "HMKODU").ToString(), 1);
                //            else
                //                FieldValues = FieldValues + "," + EGM.FieldReturnValues(gviewMatchItems.GetRowCellDisplayText(dI, "HMKODU").ToString(), 1);
                //        }
                //    }
                //    FieldNames = "DELETE FROM SILO_AD; \r\n INSERT INTO SILO_AD (KOD," + FieldNames + ") VALUES (1," + FieldValues + ")";
                //    UpdateString = "UPDATE SILO_AD SET \r\n " + UpdateString + "\r\n WHERE KOD = 1 ";//\r\n UPDATE SYSTEM.CONFIG SET BNKGUNCEL = 1; ";
                //    if (ctx.RecordProcess(UpdateString, 0) == false)
                //    {
                //        MessageBox.Show("Silo tanımları güncellenemedi.");
                //    }
                //    else
                //    {
                //        UpdateString = "UPDATE SYSTEM.CONFIG SET BNKGUNCEL = 1 ";
                //        for (int dtY = 0; dtY < dtTemporaryG.Rows.Count; dtY++)
                //        {
                //            UpdateString = UpdateString + ", " + dtTemporaryG.Rows[dtY]["ALAN"] + " = "
                //                + EGM.FieldReturnValues(Math.Round((Convert.ToDouble(dtTemporaryG.Rows[dtY]["YUZDE"]) / Convert.ToDouble(dtTemporaryG.Rows[dtY]["SAYI"])), 0).ToString().Replace(',', '.'), 1);
                //        }

                //        ctx.RecordProcess(UpdateString, 0);
                //        MessageBox.Show("Silo tanımları güncellendi.");
                //    }
            }
            catch (Exception exc)
            {
                string aa = exc.Message;
            }

        }
    }
}

