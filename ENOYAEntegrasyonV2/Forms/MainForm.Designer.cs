namespace ENOYAEntegrasyonV2.Forms
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblDatabaseStatus;
        private System.Windows.Forms.Label lblApiStatus;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Button btnStartStop;
        private System.Windows.Forms.Button btnSettings;
        private System.Windows.Forms.Button btnSyncNow;
        private System.Windows.Forms.CheckBox chkAutoStart;
        private System.Windows.Forms.CheckBox chkMinimizeToTray;
        private System.Windows.Forms.NumericUpDown numInterval;
        private System.Windows.Forms.Label lblInterval;
        private System.Windows.Forms.GroupBox grpConnection;
        private System.Windows.Forms.GroupBox grpSettings;
        private System.Windows.Forms.GroupBox grpActions;
        private System.Windows.Forms.TextBox txtLog;

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing && (components != null))
        //    {
        //        components.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}

        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.grpConnection = new System.Windows.Forms.GroupBox();
            this.lblDatabaseStatus = new System.Windows.Forms.Label();
            this.lblApiStatus = new System.Windows.Forms.Label();
            this.grpSettings = new System.Windows.Forms.GroupBox();
            this.lblInterval = new System.Windows.Forms.Label();
            this.numInterval = new System.Windows.Forms.NumericUpDown();
            this.chkMinimizeToTray = new System.Windows.Forms.CheckBox();
            this.chkAutoStart = new System.Windows.Forms.CheckBox();
            this.grpActions = new System.Windows.Forms.GroupBox();
            this.btnSyncNow = new System.Windows.Forms.Button();
            this.btnSettings = new System.Windows.Forms.Button();
            this.btnStartStop = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.grpConnection.SuspendLayout();
            this.grpSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numInterval)).BeginInit();
            this.grpActions.SuspendLayout();
            this.SuspendLayout();
            
            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(12, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(250, 24);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "ENOYA Entegrasyon V2";
            
            // grpConnection
            this.grpConnection.Controls.Add(this.lblApiStatus);
            this.grpConnection.Controls.Add(this.lblDatabaseStatus);
            this.grpConnection.Location = new System.Drawing.Point(12, 45);
            this.grpConnection.Name = "grpConnection";
            this.grpConnection.Size = new System.Drawing.Size(400, 80);
            this.grpConnection.TabIndex = 1;
            this.grpConnection.TabStop = false;
            this.grpConnection.Text = "Bağlantı Durumu";
            
            // lblDatabaseStatus
            this.lblDatabaseStatus.AutoSize = true;
            this.lblDatabaseStatus.Location = new System.Drawing.Point(15, 25);
            this.lblDatabaseStatus.Name = "lblDatabaseStatus";
            this.lblDatabaseStatus.Size = new System.Drawing.Size(150, 13);
            this.lblDatabaseStatus.TabIndex = 0;
            this.lblDatabaseStatus.Text = "MSSQL Bağlantısı: Test ediliyor...";
            
            // lblApiStatus
            this.lblApiStatus.AutoSize = true;
            this.lblApiStatus.Location = new System.Drawing.Point(15, 50);
            this.lblApiStatus.Name = "lblApiStatus";
            this.lblApiStatus.Size = new System.Drawing.Size(140, 13);
            this.lblApiStatus.TabIndex = 1;
            this.lblApiStatus.Text = "REST API Bağlantısı: Test ediliyor...";
            
            // grpSettings
            this.grpSettings.Controls.Add(this.lblInterval);
            this.grpSettings.Controls.Add(this.numInterval);
            this.grpSettings.Controls.Add(this.chkMinimizeToTray);
            this.grpSettings.Controls.Add(this.chkAutoStart);
            this.grpSettings.Location = new System.Drawing.Point(12, 135);
            this.grpSettings.Name = "grpSettings";
            this.grpSettings.Size = new System.Drawing.Size(400, 100);
            this.grpSettings.TabIndex = 2;
            this.grpSettings.TabStop = false;
            this.grpSettings.Text = "Ayarlar";
            
            // chkAutoStart
            this.chkAutoStart.AutoSize = true;
            this.chkAutoStart.Location = new System.Drawing.Point(15, 25);
            this.chkAutoStart.Name = "chkAutoStart";
            this.chkAutoStart.Size = new System.Drawing.Size(150, 17);
            this.chkAutoStart.TabIndex = 0;
            this.chkAutoStart.Text = "Otomatik Başlat";
            this.chkAutoStart.UseVisualStyleBackColor = true;
            
            // chkMinimizeToTray
            this.chkMinimizeToTray.AutoSize = true;
            this.chkMinimizeToTray.Location = new System.Drawing.Point(15, 50);
            this.chkMinimizeToTray.Name = "chkMinimizeToTray";
            this.chkMinimizeToTray.Size = new System.Drawing.Size(180, 17);
            this.chkMinimizeToTray.TabIndex = 1;
            this.chkMinimizeToTray.Text = "Kapatıldığında Tray'a Küçült";
            this.chkMinimizeToTray.UseVisualStyleBackColor = true;
            
            // numInterval
            this.numInterval.Location = new System.Drawing.Point(120, 75);
            this.numInterval.Minimum = new decimal(new int[] { 10, 0, 0, 0 });
            this.numInterval.Maximum = new decimal(new int[] { 3600, 0, 0, 0 });
            this.numInterval.Name = "numInterval";
            this.numInterval.Size = new System.Drawing.Size(100, 20);
            this.numInterval.TabIndex = 2;
            this.numInterval.Value = new decimal(new int[] { 60, 0, 0, 0 });
            
            // lblInterval
            this.lblInterval.AutoSize = true;
            this.lblInterval.Location = new System.Drawing.Point(15, 77);
            this.lblInterval.Name = "lblInterval";
            this.lblInterval.Size = new System.Drawing.Size(99, 13);
            this.lblInterval.TabIndex = 3;
            this.lblInterval.Text = "Çalışma Aralığı (sn):";
            
            // grpActions
            this.grpActions.Controls.Add(this.btnSyncNow);
            this.grpActions.Controls.Add(this.btnSettings);
            this.grpActions.Controls.Add(this.btnStartStop);
            this.grpActions.Location = new System.Drawing.Point(12, 245);
            this.grpActions.Name = "grpActions";
            this.grpActions.Size = new System.Drawing.Size(400, 60);
            this.grpActions.TabIndex = 3;
            this.grpActions.TabStop = false;
            this.grpActions.Text = "İşlemler";
            
            // btnStartStop
            this.btnStartStop.BackColor = System.Drawing.Color.Green;
            this.btnStartStop.ForeColor = System.Drawing.Color.White;
            this.btnStartStop.Location = new System.Drawing.Point(15, 20);
            this.btnStartStop.Name = "btnStartStop";
            this.btnStartStop.Size = new System.Drawing.Size(100, 30);
            this.btnStartStop.TabIndex = 0;
            this.btnStartStop.Text = "BAŞLAT";
            this.btnStartStop.UseVisualStyleBackColor = false;
            //this.btnStartStop.Click += new System.EventHandler(this.btnStartStop_Click);
            
            // btnSettings
            this.btnSettings.Location = new System.Drawing.Point(130, 20);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(100, 30);
            this.btnSettings.TabIndex = 1;
            this.btnSettings.Text = "Ayarlar";
            this.btnSettings.UseVisualStyleBackColor = true;
            //this.btnSettings.Click += new System.EventHandler(this.btnSettings_Click);
            
            // btnSyncNow
            this.btnSyncNow.Location = new System.Drawing.Point(245, 20);
            this.btnSyncNow.Name = "btnSyncNow";
            this.btnSyncNow.Size = new System.Drawing.Size(140, 30);
            this.btnSyncNow.TabIndex = 2;
            this.btnSyncNow.Text = "Şimdi Senkronize Et";
            this.btnSyncNow.UseVisualStyleBackColor = true;
            //this.btnSyncNow.Click += new System.EventHandler(this.btnSyncNow_Click);
            
            // lblStatus
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(12, 320);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(42, 13);
            this.lblStatus.TabIndex = 4;
            this.lblStatus.Text = "Hazır";
            
            // txtLog
            this.txtLog.Location = new System.Drawing.Point(12, 345);
            this.txtLog.Multiline = true;
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(400, 150);
            this.txtLog.TabIndex = 5;
            this.txtLog.BackColor = System.Drawing.Color.Black;
            this.txtLog.ForeColor = System.Drawing.Color.Lime;
            this.txtLog.Font = new System.Drawing.Font("Consolas", 9F);
            
            // MainForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(424, 507);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.grpActions);
            this.Controls.Add(this.grpSettings);
            this.Controls.Add(this.grpConnection);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ENOYA Entegrasyon V2";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.grpConnection.ResumeLayout(false);
            this.grpConnection.PerformLayout();
            this.grpSettings.ResumeLayout(false);
            this.grpSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numInterval)).EndInit();
            this.grpActions.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}

