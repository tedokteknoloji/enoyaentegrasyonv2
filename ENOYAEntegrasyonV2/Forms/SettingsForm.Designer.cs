namespace ENOYAEntegrasyonV2.Forms
{
    partial class SettingsForm
    {
        private System.ComponentModel.IContainer components = null;
        
        // Database kontrolleri
        private System.Windows.Forms.GroupBox grpDatabase;
        private System.Windows.Forms.Label lblDbServer;
        private System.Windows.Forms.TextBox txtDbServer;
        private System.Windows.Forms.Label lblDbDatabase;
        private System.Windows.Forms.TextBox txtDbDatabase;
        private System.Windows.Forms.Label lblDbUser;
        private System.Windows.Forms.TextBox txtDbUser;
        private System.Windows.Forms.Label lblDbPassword;
        private System.Windows.Forms.TextBox txtDbPassword;
        private System.Windows.Forms.CheckBox chkDbIntegratedSecurity;
        private System.Windows.Forms.Button btnTestDatabase;
        
        // API kontrolleri
        private System.Windows.Forms.GroupBox grpApi;
        private System.Windows.Forms.Label lblApiBaseUrl;
        private System.Windows.Forms.TextBox txtApiBaseUrl;
        private System.Windows.Forms.Label lblApiClientId;
        private System.Windows.Forms.TextBox txtApiClientId;
        private System.Windows.Forms.Label lblApiClientSecret;
        private System.Windows.Forms.TextBox txtApiClientSecret;
        private System.Windows.Forms.Label lblApiContract;
        private System.Windows.Forms.TextBox txtApiContract;
        private System.Windows.Forms.Button btnTestApi;
        
        // Genel kontroller
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.grpDatabase = new System.Windows.Forms.GroupBox();
            this.btnTestDatabase = new System.Windows.Forms.Button();
            this.chkDbIntegratedSecurity = new System.Windows.Forms.CheckBox();
            this.lblDbPassword = new System.Windows.Forms.Label();
            this.txtDbPassword = new System.Windows.Forms.TextBox();
            this.lblDbUser = new System.Windows.Forms.Label();
            this.txtDbUser = new System.Windows.Forms.TextBox();
            this.lblDbDatabase = new System.Windows.Forms.Label();
            this.txtDbDatabase = new System.Windows.Forms.TextBox();
            this.lblDbServer = new System.Windows.Forms.Label();
            this.txtDbServer = new System.Windows.Forms.TextBox();
            this.grpApi = new System.Windows.Forms.GroupBox();
            this.btnTestApi = new System.Windows.Forms.Button();
            this.lblApiContract = new System.Windows.Forms.Label();
            this.txtApiContract = new System.Windows.Forms.TextBox();
            this.lblApiClientSecret = new System.Windows.Forms.Label();
            this.txtApiClientSecret = new System.Windows.Forms.TextBox();
            this.lblApiClientId = new System.Windows.Forms.Label();
            this.txtApiClientId = new System.Windows.Forms.TextBox();
            this.lblApiBaseUrl = new System.Windows.Forms.Label();
            this.txtApiBaseUrl = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.grpSettings = new System.Windows.Forms.GroupBox();
            this.teAlternativeRoute = new System.Windows.Forms.TextBox();
            this.chkUseAlternativeRoute = new System.Windows.Forms.CheckBox();
            this.lblInterval = new System.Windows.Forms.Label();
            this.numInterval = new System.Windows.Forms.NumericUpDown();
            this.chkMinimizeToTray = new System.Windows.Forms.CheckBox();
            this.chkAutoStart = new System.Windows.Forms.CheckBox();
            this.tabcontSettings = new System.Windows.Forms.TabControl();
            this.tabpageGlobal = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.btnSiloKaydet = new System.Windows.Forms.Button();
            this.ıDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dEFINITIONDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ıDCOLUMNNAMEDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nAMEISTDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nAMEITMDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cONTYPEDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ıFSDATADataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ıSSUEQUANTITYDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.hUMIDITYDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rECYCLEDENSITYDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rEVISEDHUMIDITYDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rADJUSTQUANTITYDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cALCDENSITYDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cALCADJUSTQUANTITYDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cALCHUMIDITYDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pERCENTFIELDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ıTMPARTNODataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tRFIELDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ıFSPARTNODataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.siloAdlariBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.txtTokenEndPoint = new System.Windows.Forms.TextBox();
            this.grpDatabase.SuspendLayout();
            this.grpApi.SuspendLayout();
            this.grpSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numInterval)).BeginInit();
            this.tabcontSettings.SuspendLayout();
            this.tabpageGlobal.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.siloAdlariBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // grpDatabase
            // 
            this.grpDatabase.BackColor = System.Drawing.Color.LightGray;
            this.grpDatabase.Controls.Add(this.btnTestDatabase);
            this.grpDatabase.Controls.Add(this.chkDbIntegratedSecurity);
            this.grpDatabase.Controls.Add(this.lblDbPassword);
            this.grpDatabase.Controls.Add(this.txtDbPassword);
            this.grpDatabase.Controls.Add(this.lblDbUser);
            this.grpDatabase.Controls.Add(this.txtDbUser);
            this.grpDatabase.Controls.Add(this.lblDbDatabase);
            this.grpDatabase.Controls.Add(this.txtDbDatabase);
            this.grpDatabase.Controls.Add(this.lblDbServer);
            this.grpDatabase.Controls.Add(this.txtDbServer);
            this.grpDatabase.Location = new System.Drawing.Point(6, 6);
            this.grpDatabase.Name = "grpDatabase";
            this.grpDatabase.Size = new System.Drawing.Size(460, 150);
            this.grpDatabase.TabIndex = 0;
            this.grpDatabase.TabStop = false;
            this.grpDatabase.Text = "MSSQL Veritabanı Ayarları";
            // 
            // btnTestDatabase
            // 
            this.btnTestDatabase.Location = new System.Drawing.Point(340, 110);
            this.btnTestDatabase.Name = "btnTestDatabase";
            this.btnTestDatabase.Size = new System.Drawing.Size(100, 25);
            this.btnTestDatabase.TabIndex = 9;
            this.btnTestDatabase.Text = "Test Et";
            this.btnTestDatabase.UseVisualStyleBackColor = true;
            this.btnTestDatabase.Click += new System.EventHandler(this.BtnTestDatabase_Click);
            // 
            // chkDbIntegratedSecurity
            // 
            this.chkDbIntegratedSecurity.AutoSize = true;
            this.chkDbIntegratedSecurity.Location = new System.Drawing.Point(120, 112);
            this.chkDbIntegratedSecurity.Name = "chkDbIntegratedSecurity";
            this.chkDbIntegratedSecurity.Size = new System.Drawing.Size(154, 17);
            this.chkDbIntegratedSecurity.TabIndex = 8;
            this.chkDbIntegratedSecurity.Text = "Windows Kimlik Doğrulama";
            this.chkDbIntegratedSecurity.UseVisualStyleBackColor = true;
            this.chkDbIntegratedSecurity.CheckedChanged += new System.EventHandler(this.chkDbIntegratedSecurity_CheckedChanged);
            // 
            // lblDbPassword
            // 
            this.lblDbPassword.AutoSize = true;
            this.lblDbPassword.Location = new System.Drawing.Point(15, 90);
            this.lblDbPassword.Name = "lblDbPassword";
            this.lblDbPassword.Size = new System.Drawing.Size(31, 13);
            this.lblDbPassword.TabIndex = 6;
            this.lblDbPassword.Text = "Şifre:";
            // 
            // txtDbPassword
            // 
            this.txtDbPassword.Location = new System.Drawing.Point(120, 87);
            this.txtDbPassword.Name = "txtDbPassword";
            this.txtDbPassword.PasswordChar = '*';
            this.txtDbPassword.Size = new System.Drawing.Size(320, 20);
            this.txtDbPassword.TabIndex = 7;
            this.txtDbPassword.UseSystemPasswordChar = true;
            // 
            // lblDbUser
            // 
            this.lblDbUser.AutoSize = true;
            this.lblDbUser.Location = new System.Drawing.Point(15, 67);
            this.lblDbUser.Name = "lblDbUser";
            this.lblDbUser.Size = new System.Drawing.Size(49, 13);
            this.lblDbUser.TabIndex = 4;
            this.lblDbUser.Text = "Kullanıcı:";
            // 
            // txtDbUser
            // 
            this.txtDbUser.Location = new System.Drawing.Point(120, 64);
            this.txtDbUser.Name = "txtDbUser";
            this.txtDbUser.Size = new System.Drawing.Size(320, 20);
            this.txtDbUser.TabIndex = 5;
            // 
            // lblDbDatabase
            // 
            this.lblDbDatabase.AutoSize = true;
            this.lblDbDatabase.Location = new System.Drawing.Point(15, 44);
            this.lblDbDatabase.Name = "lblDbDatabase";
            this.lblDbDatabase.Size = new System.Drawing.Size(56, 13);
            this.lblDbDatabase.TabIndex = 2;
            this.lblDbDatabase.Text = "Database:";
            // 
            // txtDbDatabase
            // 
            this.txtDbDatabase.Location = new System.Drawing.Point(120, 41);
            this.txtDbDatabase.Name = "txtDbDatabase";
            this.txtDbDatabase.Size = new System.Drawing.Size(320, 20);
            this.txtDbDatabase.TabIndex = 3;
            // 
            // lblDbServer
            // 
            this.lblDbServer.AutoSize = true;
            this.lblDbServer.Location = new System.Drawing.Point(15, 21);
            this.lblDbServer.Name = "lblDbServer";
            this.lblDbServer.Size = new System.Drawing.Size(41, 13);
            this.lblDbServer.TabIndex = 0;
            this.lblDbServer.Text = "Server:";
            // 
            // txtDbServer
            // 
            this.txtDbServer.Location = new System.Drawing.Point(120, 18);
            this.txtDbServer.Name = "txtDbServer";
            this.txtDbServer.Size = new System.Drawing.Size(320, 20);
            this.txtDbServer.TabIndex = 1;
            // 
            // grpApi
            // 
            this.grpApi.BackColor = System.Drawing.Color.LightGray;
            this.grpApi.Controls.Add(this.label1);
            this.grpApi.Controls.Add(this.txtTokenEndPoint);
            this.grpApi.Controls.Add(this.btnTestApi);
            this.grpApi.Controls.Add(this.lblApiContract);
            this.grpApi.Controls.Add(this.txtApiContract);
            this.grpApi.Controls.Add(this.lblApiClientSecret);
            this.grpApi.Controls.Add(this.txtApiClientSecret);
            this.grpApi.Controls.Add(this.lblApiClientId);
            this.grpApi.Controls.Add(this.txtApiClientId);
            this.grpApi.Controls.Add(this.lblApiBaseUrl);
            this.grpApi.Controls.Add(this.txtApiBaseUrl);
            this.grpApi.Location = new System.Drawing.Point(6, 162);
            this.grpApi.Name = "grpApi";
            this.grpApi.Size = new System.Drawing.Size(460, 169);
            this.grpApi.TabIndex = 1;
            this.grpApi.TabStop = false;
            this.grpApi.Text = "REST API Ayarları (IFS)";
            // 
            // btnTestApi
            // 
            this.btnTestApi.Location = new System.Drawing.Point(340, 138);
            this.btnTestApi.Name = "btnTestApi";
            this.btnTestApi.Size = new System.Drawing.Size(100, 25);
            this.btnTestApi.TabIndex = 8;
            this.btnTestApi.Text = "Test Et";
            this.btnTestApi.UseVisualStyleBackColor = true;
            this.btnTestApi.Click += new System.EventHandler(this.BtnTestApi_Click);
            // 
            // lblApiContract
            // 
            this.lblApiContract.AutoSize = true;
            this.lblApiContract.Location = new System.Drawing.Point(15, 117);
            this.lblApiContract.Name = "lblApiContract";
            this.lblApiContract.Size = new System.Drawing.Size(50, 13);
            this.lblApiContract.TabIndex = 6;
            this.lblApiContract.Text = "Contract:";
            // 
            // txtApiContract
            // 
            this.txtApiContract.Location = new System.Drawing.Point(120, 114);
            this.txtApiContract.Name = "txtApiContract";
            this.txtApiContract.Size = new System.Drawing.Size(320, 20);
            this.txtApiContract.TabIndex = 7;
            // 
            // lblApiClientSecret
            // 
            this.lblApiClientSecret.AutoSize = true;
            this.lblApiClientSecret.Location = new System.Drawing.Point(15, 94);
            this.lblApiClientSecret.Name = "lblApiClientSecret";
            this.lblApiClientSecret.Size = new System.Drawing.Size(70, 13);
            this.lblApiClientSecret.TabIndex = 4;
            this.lblApiClientSecret.Text = "Client Secret:";
            // 
            // txtApiClientSecret
            // 
            this.txtApiClientSecret.Location = new System.Drawing.Point(120, 91);
            this.txtApiClientSecret.Name = "txtApiClientSecret";
            this.txtApiClientSecret.PasswordChar = '*';
            this.txtApiClientSecret.Size = new System.Drawing.Size(320, 20);
            this.txtApiClientSecret.TabIndex = 5;
            this.txtApiClientSecret.UseSystemPasswordChar = true;
            // 
            // lblApiClientId
            // 
            this.lblApiClientId.AutoSize = true;
            this.lblApiClientId.Location = new System.Drawing.Point(15, 71);
            this.lblApiClientId.Name = "lblApiClientId";
            this.lblApiClientId.Size = new System.Drawing.Size(50, 13);
            this.lblApiClientId.TabIndex = 2;
            this.lblApiClientId.Text = "Client ID:";
            // 
            // txtApiClientId
            // 
            this.txtApiClientId.Location = new System.Drawing.Point(120, 68);
            this.txtApiClientId.Name = "txtApiClientId";
            this.txtApiClientId.Size = new System.Drawing.Size(320, 20);
            this.txtApiClientId.TabIndex = 3;
            // 
            // lblApiBaseUrl
            // 
            this.lblApiBaseUrl.AutoSize = true;
            this.lblApiBaseUrl.Location = new System.Drawing.Point(15, 25);
            this.lblApiBaseUrl.Name = "lblApiBaseUrl";
            this.lblApiBaseUrl.Size = new System.Drawing.Size(59, 13);
            this.lblApiBaseUrl.TabIndex = 0;
            this.lblApiBaseUrl.Text = "Base URL:";
            // 
            // txtApiBaseUrl
            // 
            this.txtApiBaseUrl.Location = new System.Drawing.Point(120, 22);
            this.txtApiBaseUrl.Name = "txtApiBaseUrl";
            this.txtApiBaseUrl.Size = new System.Drawing.Size(320, 20);
            this.txtApiBaseUrl.TabIndex = 1;
            // 
            // btnSave
            // 
            this.btnSave.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnSave.Location = new System.Drawing.Point(264, 448);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 30);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "Kaydet";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(366, 448);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 30);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "İptal";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // grpSettings
            // 
            this.grpSettings.BackColor = System.Drawing.Color.LightGray;
            this.grpSettings.Controls.Add(this.teAlternativeRoute);
            this.grpSettings.Controls.Add(this.chkUseAlternativeRoute);
            this.grpSettings.Controls.Add(this.lblInterval);
            this.grpSettings.Controls.Add(this.numInterval);
            this.grpSettings.Controls.Add(this.chkMinimizeToTray);
            this.grpSettings.Controls.Add(this.chkAutoStart);
            this.grpSettings.Location = new System.Drawing.Point(6, 337);
            this.grpSettings.Name = "grpSettings";
            this.grpSettings.Size = new System.Drawing.Size(460, 105);
            this.grpSettings.TabIndex = 5;
            this.grpSettings.TabStop = false;
            this.grpSettings.Text = "Ayarlar";
            // 
            // teAlternativeRoute
            // 
            this.teAlternativeRoute.Enabled = false;
            this.teAlternativeRoute.Location = new System.Drawing.Point(154, 67);
            this.teAlternativeRoute.Name = "teAlternativeRoute";
            this.teAlternativeRoute.Size = new System.Drawing.Size(100, 20);
            this.teAlternativeRoute.TabIndex = 5;
            // 
            // chkUseAlternativeRoute
            // 
            this.chkUseAlternativeRoute.AutoSize = true;
            this.chkUseAlternativeRoute.Location = new System.Drawing.Point(15, 71);
            this.chkUseAlternativeRoute.Name = "chkUseAlternativeRoute";
            this.chkUseAlternativeRoute.Size = new System.Drawing.Size(125, 17);
            this.chkUseAlternativeRoute.TabIndex = 4;
            this.chkUseAlternativeRoute.Text = "Alternatif Rota Kullan";
            this.chkUseAlternativeRoute.UseVisualStyleBackColor = true;
            this.chkUseAlternativeRoute.CheckedChanged += new System.EventHandler(this.chkUseAlternativeRoute_CheckedChanged);
            // 
            // lblInterval
            // 
            this.lblInterval.AutoSize = true;
            this.lblInterval.Location = new System.Drawing.Point(153, 26);
            this.lblInterval.Name = "lblInterval";
            this.lblInterval.Size = new System.Drawing.Size(97, 13);
            this.lblInterval.TabIndex = 3;
            this.lblInterval.Text = "Çalışma Aralığı (sn):";
            // 
            // numInterval
            // 
            this.numInterval.Location = new System.Drawing.Point(258, 24);
            this.numInterval.Maximum = new decimal(new int[] {
            3600,
            0,
            0,
            0});
            this.numInterval.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numInterval.Name = "numInterval";
            this.numInterval.Size = new System.Drawing.Size(100, 20);
            this.numInterval.TabIndex = 2;
            this.numInterval.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            // 
            // chkMinimizeToTray
            // 
            this.chkMinimizeToTray.AutoSize = true;
            this.chkMinimizeToTray.Location = new System.Drawing.Point(15, 48);
            this.chkMinimizeToTray.Name = "chkMinimizeToTray";
            this.chkMinimizeToTray.Size = new System.Drawing.Size(157, 17);
            this.chkMinimizeToTray.TabIndex = 1;
            this.chkMinimizeToTray.Text = "Kapatıldığında Tray\'a Küçült";
            this.chkMinimizeToTray.UseVisualStyleBackColor = true;
            // 
            // chkAutoStart
            // 
            this.chkAutoStart.AutoSize = true;
            this.chkAutoStart.Location = new System.Drawing.Point(15, 25);
            this.chkAutoStart.Name = "chkAutoStart";
            this.chkAutoStart.Size = new System.Drawing.Size(100, 17);
            this.chkAutoStart.TabIndex = 0;
            this.chkAutoStart.Text = "Otomatik Başlat";
            this.chkAutoStart.UseVisualStyleBackColor = true;
            // 
            // tabcontSettings
            // 
            this.tabcontSettings.Appearance = System.Windows.Forms.TabAppearance.Buttons;
            this.tabcontSettings.Controls.Add(this.tabpageGlobal);
            this.tabcontSettings.Controls.Add(this.tabPage2);
            this.tabcontSettings.Location = new System.Drawing.Point(12, 12);
            this.tabcontSettings.Name = "tabcontSettings";
            this.tabcontSettings.SelectedIndex = 0;
            this.tabcontSettings.Size = new System.Drawing.Size(491, 513);
            this.tabcontSettings.TabIndex = 6;
            // 
            // tabpageGlobal
            // 
            this.tabpageGlobal.Controls.Add(this.grpDatabase);
            this.tabpageGlobal.Controls.Add(this.grpSettings);
            this.tabpageGlobal.Controls.Add(this.grpApi);
            this.tabpageGlobal.Controls.Add(this.btnCancel);
            this.tabpageGlobal.Controls.Add(this.btnSave);
            this.tabpageGlobal.Location = new System.Drawing.Point(4, 25);
            this.tabpageGlobal.Name = "tabpageGlobal";
            this.tabpageGlobal.Padding = new System.Windows.Forms.Padding(3);
            this.tabpageGlobal.Size = new System.Drawing.Size(483, 484);
            this.tabpageGlobal.TabIndex = 0;
            this.tabpageGlobal.Text = "Genel Bağlantı Bilgileri";
            this.tabpageGlobal.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.LightGray;
            this.tabPage2.Controls.Add(this.dataGridView1);
            this.tabPage2.Controls.Add(this.btnSiloKaydet);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(483, 465);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Bunker ve Silo Tanımları";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ıDDataGridViewTextBoxColumn,
            this.dEFINITIONDataGridViewTextBoxColumn,
            this.ıDCOLUMNNAMEDataGridViewTextBoxColumn,
            this.nAMEISTDataGridViewTextBoxColumn,
            this.nAMEITMDataGridViewTextBoxColumn,
            this.cONTYPEDataGridViewTextBoxColumn,
            this.ıFSDATADataGridViewTextBoxColumn,
            this.ıSSUEQUANTITYDataGridViewTextBoxColumn,
            this.hUMIDITYDataGridViewTextBoxColumn,
            this.rECYCLEDENSITYDataGridViewTextBoxColumn,
            this.rEVISEDHUMIDITYDataGridViewTextBoxColumn,
            this.rADJUSTQUANTITYDataGridViewTextBoxColumn,
            this.cALCDENSITYDataGridViewTextBoxColumn,
            this.cALCADJUSTQUANTITYDataGridViewTextBoxColumn,
            this.cALCHUMIDITYDataGridViewTextBoxColumn,
            this.pERCENTFIELDDataGridViewTextBoxColumn,
            this.ıTMPARTNODataGridViewTextBoxColumn,
            this.tRFIELDDataGridViewTextBoxColumn,
            this.ıFSPARTNODataGridViewTextBoxColumn});
            this.dataGridView1.DataSource = this.siloAdlariBindingSource;
            this.dataGridView1.Location = new System.Drawing.Point(3, 6);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(474, 422);
            this.dataGridView1.TabIndex = 2;
            // 
            // btnSiloKaydet
            // 
            this.btnSiloKaydet.Location = new System.Drawing.Point(6, 434);
            this.btnSiloKaydet.Name = "btnSiloKaydet";
            this.btnSiloKaydet.Size = new System.Drawing.Size(100, 30);
            this.btnSiloKaydet.TabIndex = 1;
            this.btnSiloKaydet.Text = "Silo Kaydet";
            this.btnSiloKaydet.UseVisualStyleBackColor = true;
            this.btnSiloKaydet.Click += new System.EventHandler(this.btnSiloKaydet_Click);
            // 
            // ıDDataGridViewTextBoxColumn
            // 
            this.ıDDataGridViewTextBoxColumn.DataPropertyName = "ID";
            this.ıDDataGridViewTextBoxColumn.HeaderText = "ID";
            this.ıDDataGridViewTextBoxColumn.Name = "ıDDataGridViewTextBoxColumn";
            // 
            // dEFINITIONDataGridViewTextBoxColumn
            // 
            this.dEFINITIONDataGridViewTextBoxColumn.DataPropertyName = "DEFINITION_";
            this.dEFINITIONDataGridViewTextBoxColumn.HeaderText = "DEFINITION_";
            this.dEFINITIONDataGridViewTextBoxColumn.Name = "dEFINITIONDataGridViewTextBoxColumn";
            // 
            // ıDCOLUMNNAMEDataGridViewTextBoxColumn
            // 
            this.ıDCOLUMNNAMEDataGridViewTextBoxColumn.DataPropertyName = "IDCOLUMNNAME";
            this.ıDCOLUMNNAMEDataGridViewTextBoxColumn.HeaderText = "IDCOLUMNNAME";
            this.ıDCOLUMNNAMEDataGridViewTextBoxColumn.Name = "ıDCOLUMNNAMEDataGridViewTextBoxColumn";
            // 
            // nAMEISTDataGridViewTextBoxColumn
            // 
            this.nAMEISTDataGridViewTextBoxColumn.DataPropertyName = "NAMEIST";
            this.nAMEISTDataGridViewTextBoxColumn.HeaderText = "NAMEIST";
            this.nAMEISTDataGridViewTextBoxColumn.Name = "nAMEISTDataGridViewTextBoxColumn";
            // 
            // nAMEITMDataGridViewTextBoxColumn
            // 
            this.nAMEITMDataGridViewTextBoxColumn.DataPropertyName = "NAMEITM";
            this.nAMEITMDataGridViewTextBoxColumn.HeaderText = "NAMEITM";
            this.nAMEITMDataGridViewTextBoxColumn.Name = "nAMEITMDataGridViewTextBoxColumn";
            // 
            // cONTYPEDataGridViewTextBoxColumn
            // 
            this.cONTYPEDataGridViewTextBoxColumn.DataPropertyName = "CONTYPE";
            this.cONTYPEDataGridViewTextBoxColumn.HeaderText = "CONTYPE";
            this.cONTYPEDataGridViewTextBoxColumn.Name = "cONTYPEDataGridViewTextBoxColumn";
            // 
            // ıFSDATADataGridViewTextBoxColumn
            // 
            this.ıFSDATADataGridViewTextBoxColumn.DataPropertyName = "IFSDATA";
            this.ıFSDATADataGridViewTextBoxColumn.HeaderText = "IFSDATA";
            this.ıFSDATADataGridViewTextBoxColumn.Name = "ıFSDATADataGridViewTextBoxColumn";
            // 
            // ıSSUEQUANTITYDataGridViewTextBoxColumn
            // 
            this.ıSSUEQUANTITYDataGridViewTextBoxColumn.DataPropertyName = "ISSUEQUANTITY";
            this.ıSSUEQUANTITYDataGridViewTextBoxColumn.HeaderText = "ISSUEQUANTITY";
            this.ıSSUEQUANTITYDataGridViewTextBoxColumn.Name = "ıSSUEQUANTITYDataGridViewTextBoxColumn";
            // 
            // hUMIDITYDataGridViewTextBoxColumn
            // 
            this.hUMIDITYDataGridViewTextBoxColumn.DataPropertyName = "HUMIDITY";
            this.hUMIDITYDataGridViewTextBoxColumn.HeaderText = "HUMIDITY";
            this.hUMIDITYDataGridViewTextBoxColumn.Name = "hUMIDITYDataGridViewTextBoxColumn";
            // 
            // rECYCLEDENSITYDataGridViewTextBoxColumn
            // 
            this.rECYCLEDENSITYDataGridViewTextBoxColumn.DataPropertyName = "RECYCLEDENSITY";
            this.rECYCLEDENSITYDataGridViewTextBoxColumn.HeaderText = "RECYCLEDENSITY";
            this.rECYCLEDENSITYDataGridViewTextBoxColumn.Name = "rECYCLEDENSITYDataGridViewTextBoxColumn";
            // 
            // rEVISEDHUMIDITYDataGridViewTextBoxColumn
            // 
            this.rEVISEDHUMIDITYDataGridViewTextBoxColumn.DataPropertyName = "REVISEDHUMIDITY";
            this.rEVISEDHUMIDITYDataGridViewTextBoxColumn.HeaderText = "REVISEDHUMIDITY";
            this.rEVISEDHUMIDITYDataGridViewTextBoxColumn.Name = "rEVISEDHUMIDITYDataGridViewTextBoxColumn";
            // 
            // rADJUSTQUANTITYDataGridViewTextBoxColumn
            // 
            this.rADJUSTQUANTITYDataGridViewTextBoxColumn.DataPropertyName = "RADJUSTQUANTITY";
            this.rADJUSTQUANTITYDataGridViewTextBoxColumn.HeaderText = "RADJUSTQUANTITY";
            this.rADJUSTQUANTITYDataGridViewTextBoxColumn.Name = "rADJUSTQUANTITYDataGridViewTextBoxColumn";
            // 
            // cALCDENSITYDataGridViewTextBoxColumn
            // 
            this.cALCDENSITYDataGridViewTextBoxColumn.DataPropertyName = "CALCDENSITY";
            this.cALCDENSITYDataGridViewTextBoxColumn.HeaderText = "CALCDENSITY";
            this.cALCDENSITYDataGridViewTextBoxColumn.Name = "cALCDENSITYDataGridViewTextBoxColumn";
            // 
            // cALCADJUSTQUANTITYDataGridViewTextBoxColumn
            // 
            this.cALCADJUSTQUANTITYDataGridViewTextBoxColumn.DataPropertyName = "CALCADJUSTQUANTITY";
            this.cALCADJUSTQUANTITYDataGridViewTextBoxColumn.HeaderText = "CALCADJUSTQUANTITY";
            this.cALCADJUSTQUANTITYDataGridViewTextBoxColumn.Name = "cALCADJUSTQUANTITYDataGridViewTextBoxColumn";
            // 
            // cALCHUMIDITYDataGridViewTextBoxColumn
            // 
            this.cALCHUMIDITYDataGridViewTextBoxColumn.DataPropertyName = "CALCHUMIDITY";
            this.cALCHUMIDITYDataGridViewTextBoxColumn.HeaderText = "CALCHUMIDITY";
            this.cALCHUMIDITYDataGridViewTextBoxColumn.Name = "cALCHUMIDITYDataGridViewTextBoxColumn";
            // 
            // pERCENTFIELDDataGridViewTextBoxColumn
            // 
            this.pERCENTFIELDDataGridViewTextBoxColumn.DataPropertyName = "PERCENTFIELD";
            this.pERCENTFIELDDataGridViewTextBoxColumn.HeaderText = "PERCENTFIELD";
            this.pERCENTFIELDDataGridViewTextBoxColumn.Name = "pERCENTFIELDDataGridViewTextBoxColumn";
            // 
            // ıTMPARTNODataGridViewTextBoxColumn
            // 
            this.ıTMPARTNODataGridViewTextBoxColumn.DataPropertyName = "ITMPARTNO";
            this.ıTMPARTNODataGridViewTextBoxColumn.HeaderText = "ITMPARTNO";
            this.ıTMPARTNODataGridViewTextBoxColumn.Name = "ıTMPARTNODataGridViewTextBoxColumn";
            // 
            // tRFIELDDataGridViewTextBoxColumn
            // 
            this.tRFIELDDataGridViewTextBoxColumn.DataPropertyName = "TRFIELD";
            this.tRFIELDDataGridViewTextBoxColumn.HeaderText = "TRFIELD";
            this.tRFIELDDataGridViewTextBoxColumn.Name = "tRFIELDDataGridViewTextBoxColumn";
            // 
            // ıFSPARTNODataGridViewTextBoxColumn
            // 
            this.ıFSPARTNODataGridViewTextBoxColumn.DataPropertyName = "IFSPARTNO";
            this.ıFSPARTNODataGridViewTextBoxColumn.HeaderText = "IFSPARTNO";
            this.ıFSPARTNODataGridViewTextBoxColumn.Name = "ıFSPARTNODataGridViewTextBoxColumn";
            // 
            // siloAdlariBindingSource
            // 
            this.siloAdlariBindingSource.DataSource = typeof(ENOYAEntegrasyonV2.Business.SiloAdlari);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Token EndPoint";
            // 
            // txtTokenEndPoint
            // 
            this.txtTokenEndPoint.Location = new System.Drawing.Point(120, 45);
            this.txtTokenEndPoint.Name = "txtTokenEndPoint";
            this.txtTokenEndPoint.Size = new System.Drawing.Size(320, 20);
            this.txtTokenEndPoint.TabIndex = 10;
            // 
            // SettingsForm
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(510, 537);
            this.Controls.Add(this.tabcontSettings);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Ayarlar";
            this.grpDatabase.ResumeLayout(false);
            this.grpDatabase.PerformLayout();
            this.grpApi.ResumeLayout(false);
            this.grpApi.PerformLayout();
            this.grpSettings.ResumeLayout(false);
            this.grpSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numInterval)).EndInit();
            this.tabcontSettings.ResumeLayout(false);
            this.tabpageGlobal.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.siloAdlariBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.GroupBox grpSettings;
        private System.Windows.Forms.TextBox teAlternativeRoute;
        private System.Windows.Forms.CheckBox chkUseAlternativeRoute;
        private System.Windows.Forms.Label lblInterval;
        private System.Windows.Forms.NumericUpDown numInterval;
        private System.Windows.Forms.CheckBox chkMinimizeToTray;
        private System.Windows.Forms.CheckBox chkAutoStart;
        private System.Windows.Forms.TabControl tabcontSettings;
        private System.Windows.Forms.TabPage tabpageGlobal;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button btnSiloKaydet;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.BindingSource siloAdlariBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn ıDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dEFINITIONDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ıDCOLUMNNAMEDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nAMEISTDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nAMEITMDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cONTYPEDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ıFSDATADataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ıSSUEQUANTITYDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn hUMIDITYDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn rECYCLEDENSITYDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn rEVISEDHUMIDITYDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn rADJUSTQUANTITYDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cALCDENSITYDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cALCADJUSTQUANTITYDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cALCHUMIDITYDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn pERCENTFIELDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ıTMPARTNODataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn tRFIELDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ıFSPARTNODataGridViewTextBoxColumn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtTokenEndPoint;
    }
}
