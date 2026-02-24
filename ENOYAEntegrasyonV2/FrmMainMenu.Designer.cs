namespace ENOYAEntegrasyonV2
{
    partial class FrmMainMenu
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing && (components != null))
        //    {
        //        components.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMainMenu));
            this.tabcontMainMenu = new DevExpress.XtraTab.XtraTabControl();
            this.tabpageMalzeme = new DevExpress.XtraTab.XtraTabPage();
            this.deTarih = new DevExpress.XtraEditors.DateEdit();
            this.btnMalzemeleriDByeAktar = new System.Windows.Forms.Button();
            this.btnMalzemeListesiGetir = new System.Windows.Forms.Button();
            this.grdMalzeme = new DevExpress.XtraGrid.GridControl();
            this.bindMalzeme = new System.Windows.Forms.BindingSource(this.components);
            this.gviewMalzeme = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colKOD = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colAD = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSTOK_MIKTARI = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSTOK_KONTROL = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colACIKLAMA = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMINIMUM_MIKTAR = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSILO_KAPASITE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colAGREGA = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCIMENTO = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colKUL = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTOZ = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colKUM = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSU = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colKATKI = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colEKSINEMLIMIT = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colPART_NO = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colPROD_FAMILY = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colGKS_YOG = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colGKS_INCE_YOG = new DevExpress.XtraGrid.Columns.GridColumn();
            this.tabpageSiparis = new DevExpress.XtraTab.XtraTabPage();
            this.ceYerelSiparisOku = new DevExpress.XtraEditors.CheckEdit();
            this.btnSiparisAktar = new System.Windows.Forms.Button();
            this.btnSiparisOku = new System.Windows.Forms.Button();
            this.grdSiparis = new DevExpress.XtraGrid.GridControl();
            this.bindIFSPlanline = new System.Windows.Forms.BindingSource(this.components);
            this.gviewSiparis = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colLuname = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colOrderNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colReleaseNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSequenceNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colOrderCode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colProcessType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDateEntered = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colContract = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colPartNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colPartDesc = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colRevisedQtyDue = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colEngChgLevel = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colStructureAlternative = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colAlternativeDesc = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colLineItemNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colComponentPartNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCompPartDesc = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colProductFamily = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colQtyRequired = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colQtyPerAssembly = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMikser = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colRecycleUsageFactor = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colRecycleUsageFactorV = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMixingTime = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colRoutingAlternative = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colEarliestStartDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCustomerId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCustomerName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colA = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colB = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colC = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colD = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colAdressId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colAdressDesc = new DevExpress.XtraGrid.Columns.GridColumn();
            this.tabpagePerDetaylar = new DevExpress.XtraTab.XtraTabPage();
            this.btnPeriyotlarMerkezeGonder = new System.Windows.Forms.Button();
            this.btnPeriyotListesiGetir = new System.Windows.Forms.Button();
            this.grdPeriyotlar = new DevExpress.XtraGrid.GridControl();
            this.bindPerDetay = new System.Windows.Forms.BindingSource(this.components);
            this.gviewPeriyotlar = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.ıFSPLANBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.grpContIslemler = new DevExpress.XtraEditors.GroupControl();
            this.lblStatus = new System.Windows.Forms.Label();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.grpActions = new System.Windows.Forms.GroupBox();
            this.btnSyncNow = new System.Windows.Forms.Button();
            this.btnSettings = new System.Windows.Forms.Button();
            this.btnStartStop = new System.Windows.Forms.Button();
            this.grpSettings = new System.Windows.Forms.GroupBox();
            this.teAlternativeRoute = new System.Windows.Forms.TextBox();
            this.chkUseAlternativeRoute = new System.Windows.Forms.CheckBox();
            this.lblInterval = new System.Windows.Forms.Label();
            this.numInterval = new System.Windows.Forms.NumericUpDown();
            this.chkMinimizeToTray = new System.Windows.Forms.CheckBox();
            this.chkAutoStart = new System.Windows.Forms.CheckBox();
            this.grpConnection = new System.Windows.Forms.GroupBox();
            this.lblApiStatus = new System.Windows.Forms.Label();
            this.lblDatabaseStatus = new System.Windows.Forms.Label();
            this.contextMalzeme = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.contextSiparis = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.exceleAktarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exceleAktarToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.tabcontMainMenu)).BeginInit();
            this.tabcontMainMenu.SuspendLayout();
            this.tabpageMalzeme.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.deTarih.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deTarih.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdMalzeme)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindMalzeme)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gviewMalzeme)).BeginInit();
            this.tabpageSiparis.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ceYerelSiparisOku.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdSiparis)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindIFSPlanline)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gviewSiparis)).BeginInit();
            this.tabpagePerDetaylar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdPeriyotlar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindPerDetay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gviewPeriyotlar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ıFSPLANBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpContIslemler)).BeginInit();
            this.grpContIslemler.SuspendLayout();
            this.grpActions.SuspendLayout();
            this.grpSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numInterval)).BeginInit();
            this.grpConnection.SuspendLayout();
            this.contextMalzeme.SuspendLayout();
            this.contextSiparis.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabcontMainMenu
            // 
            this.tabcontMainMenu.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabcontMainMenu.Location = new System.Drawing.Point(0, 240);
            this.tabcontMainMenu.Name = "tabcontMainMenu";
            this.tabcontMainMenu.SelectedTabPage = this.tabpageMalzeme;
            this.tabcontMainMenu.Size = new System.Drawing.Size(933, 260);
            this.tabcontMainMenu.TabIndex = 0;
            this.tabcontMainMenu.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.tabpageMalzeme,
            this.tabpageSiparis,
            this.tabpagePerDetaylar});
            // 
            // tabpageMalzeme
            // 
            this.tabpageMalzeme.Controls.Add(this.deTarih);
            this.tabpageMalzeme.Controls.Add(this.btnMalzemeleriDByeAktar);
            this.tabpageMalzeme.Controls.Add(this.btnMalzemeListesiGetir);
            this.tabpageMalzeme.Controls.Add(this.grdMalzeme);
            this.tabpageMalzeme.Name = "tabpageMalzeme";
            this.tabpageMalzeme.Size = new System.Drawing.Size(927, 232);
            this.tabpageMalzeme.Text = "Malzeme Listesi";
            // 
            // deTarih
            // 
            this.deTarih.EditValue = null;
            this.deTarih.Location = new System.Drawing.Point(385, 8);
            this.deTarih.Name = "deTarih";
            this.deTarih.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.deTarih.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.deTarih.Size = new System.Drawing.Size(100, 20);
            this.deTarih.TabIndex = 3;
            // 
            // btnMalzemeleriDByeAktar
            // 
            this.btnMalzemeleriDByeAktar.BackColor = System.Drawing.Color.Orange;
            this.btnMalzemeleriDByeAktar.ForeColor = System.Drawing.Color.Black;
            this.btnMalzemeleriDByeAktar.Location = new System.Drawing.Point(169, 4);
            this.btnMalzemeleriDByeAktar.Name = "btnMalzemeleriDByeAktar";
            this.btnMalzemeleriDByeAktar.Size = new System.Drawing.Size(200, 27);
            this.btnMalzemeleriDByeAktar.TabIndex = 2;
            this.btnMalzemeleriDByeAktar.Text = "Malzemeleri Veritabanına Aktar";
            this.btnMalzemeleriDByeAktar.UseVisualStyleBackColor = false;
            this.btnMalzemeleriDByeAktar.Click += new System.EventHandler(this.btnMalzemeleriDByeAktar_Click);
            // 
            // btnMalzemeListesiGetir
            // 
            this.btnMalzemeListesiGetir.BackColor = System.Drawing.Color.Orange;
            this.btnMalzemeListesiGetir.ForeColor = System.Drawing.Color.Black;
            this.btnMalzemeListesiGetir.Location = new System.Drawing.Point(7, 4);
            this.btnMalzemeListesiGetir.Name = "btnMalzemeListesiGetir";
            this.btnMalzemeListesiGetir.Size = new System.Drawing.Size(156, 27);
            this.btnMalzemeListesiGetir.TabIndex = 1;
            this.btnMalzemeListesiGetir.Text = "Malzeme Listesini Getir";
            this.btnMalzemeListesiGetir.UseVisualStyleBackColor = false;
            this.btnMalzemeListesiGetir.Click += new System.EventHandler(this.btnMalzemeListesiGetir_Click);
            // 
            // grdMalzeme
            // 
            this.grdMalzeme.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdMalzeme.DataSource = this.bindMalzeme;
            this.grdMalzeme.Location = new System.Drawing.Point(6, 33);
            this.grdMalzeme.MainView = this.gviewMalzeme;
            this.grdMalzeme.Name = "grdMalzeme";
            this.grdMalzeme.Size = new System.Drawing.Size(918, 197);
            this.grdMalzeme.TabIndex = 0;
            this.grdMalzeme.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gviewMalzeme});
            // 
            // bindMalzeme
            // 
            this.bindMalzeme.DataSource = typeof(ENOYAEntegrasyonV2.Models.Entities.MALZEME);
            // 
            // gviewMalzeme
            // 
            this.gviewMalzeme.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colKOD,
            this.colAD,
            this.colSTOK_MIKTARI,
            this.colSTOK_KONTROL,
            this.colACIKLAMA,
            this.colMINIMUM_MIKTAR,
            this.colSILO_KAPASITE,
            this.colAGREGA,
            this.colCIMENTO,
            this.colKUL,
            this.colTOZ,
            this.colKUM,
            this.colSU,
            this.colKATKI,
            this.colEKSINEMLIMIT,
            this.colPART_NO,
            this.colPROD_FAMILY,
            this.colGKS_YOG,
            this.colGKS_INCE_YOG});
            this.gviewMalzeme.GridControl = this.grdMalzeme;
            this.gviewMalzeme.Name = "gviewMalzeme";
            this.gviewMalzeme.OptionsBehavior.Editable = false;
            this.gviewMalzeme.OptionsView.BestFitMode = DevExpress.XtraGrid.Views.Grid.GridBestFitMode.Full;
            this.gviewMalzeme.OptionsView.ColumnAutoWidth = false;
            this.gviewMalzeme.OptionsView.ShowGroupPanel = false;
            // 
            // colKOD
            // 
            this.colKOD.FieldName = "KOD";
            this.colKOD.Name = "colKOD";
            this.colKOD.Visible = true;
            this.colKOD.VisibleIndex = 0;
            // 
            // colAD
            // 
            this.colAD.FieldName = "AD";
            this.colAD.Name = "colAD";
            this.colAD.Visible = true;
            this.colAD.VisibleIndex = 1;
            // 
            // colSTOK_MIKTARI
            // 
            this.colSTOK_MIKTARI.FieldName = "STOK_MIKTARI";
            this.colSTOK_MIKTARI.Name = "colSTOK_MIKTARI";
            this.colSTOK_MIKTARI.Visible = true;
            this.colSTOK_MIKTARI.VisibleIndex = 2;
            // 
            // colSTOK_KONTROL
            // 
            this.colSTOK_KONTROL.FieldName = "STOK_KONTROL";
            this.colSTOK_KONTROL.Name = "colSTOK_KONTROL";
            this.colSTOK_KONTROL.Visible = true;
            this.colSTOK_KONTROL.VisibleIndex = 3;
            // 
            // colACIKLAMA
            // 
            this.colACIKLAMA.FieldName = "ACIKLAMA";
            this.colACIKLAMA.Name = "colACIKLAMA";
            this.colACIKLAMA.Visible = true;
            this.colACIKLAMA.VisibleIndex = 4;
            // 
            // colMINIMUM_MIKTAR
            // 
            this.colMINIMUM_MIKTAR.FieldName = "MINIMUM_MIKTAR";
            this.colMINIMUM_MIKTAR.Name = "colMINIMUM_MIKTAR";
            this.colMINIMUM_MIKTAR.Visible = true;
            this.colMINIMUM_MIKTAR.VisibleIndex = 5;
            // 
            // colSILO_KAPASITE
            // 
            this.colSILO_KAPASITE.FieldName = "SILO_KAPASITE";
            this.colSILO_KAPASITE.Name = "colSILO_KAPASITE";
            this.colSILO_KAPASITE.Visible = true;
            this.colSILO_KAPASITE.VisibleIndex = 6;
            // 
            // colAGREGA
            // 
            this.colAGREGA.FieldName = "AGREGA";
            this.colAGREGA.Name = "colAGREGA";
            this.colAGREGA.Visible = true;
            this.colAGREGA.VisibleIndex = 7;
            // 
            // colCIMENTO
            // 
            this.colCIMENTO.FieldName = "CIMENTO";
            this.colCIMENTO.Name = "colCIMENTO";
            this.colCIMENTO.Visible = true;
            this.colCIMENTO.VisibleIndex = 8;
            // 
            // colKUL
            // 
            this.colKUL.FieldName = "KUL";
            this.colKUL.Name = "colKUL";
            this.colKUL.Visible = true;
            this.colKUL.VisibleIndex = 9;
            // 
            // colTOZ
            // 
            this.colTOZ.FieldName = "TOZ";
            this.colTOZ.Name = "colTOZ";
            this.colTOZ.Visible = true;
            this.colTOZ.VisibleIndex = 10;
            // 
            // colKUM
            // 
            this.colKUM.FieldName = "KUM";
            this.colKUM.Name = "colKUM";
            this.colKUM.Visible = true;
            this.colKUM.VisibleIndex = 11;
            // 
            // colSU
            // 
            this.colSU.FieldName = "SU";
            this.colSU.Name = "colSU";
            this.colSU.Visible = true;
            this.colSU.VisibleIndex = 12;
            // 
            // colKATKI
            // 
            this.colKATKI.FieldName = "KATKI";
            this.colKATKI.Name = "colKATKI";
            this.colKATKI.Visible = true;
            this.colKATKI.VisibleIndex = 13;
            // 
            // colEKSINEMLIMIT
            // 
            this.colEKSINEMLIMIT.FieldName = "EKSINEMLIMIT";
            this.colEKSINEMLIMIT.Name = "colEKSINEMLIMIT";
            this.colEKSINEMLIMIT.Visible = true;
            this.colEKSINEMLIMIT.VisibleIndex = 14;
            // 
            // colPART_NO
            // 
            this.colPART_NO.FieldName = "PART_NO";
            this.colPART_NO.Name = "colPART_NO";
            this.colPART_NO.Visible = true;
            this.colPART_NO.VisibleIndex = 15;
            // 
            // colPROD_FAMILY
            // 
            this.colPROD_FAMILY.FieldName = "PROD_FAMILY";
            this.colPROD_FAMILY.Name = "colPROD_FAMILY";
            this.colPROD_FAMILY.Visible = true;
            this.colPROD_FAMILY.VisibleIndex = 16;
            // 
            // colGKS_YOG
            // 
            this.colGKS_YOG.FieldName = "GKS_YOG";
            this.colGKS_YOG.Name = "colGKS_YOG";
            this.colGKS_YOG.Visible = true;
            this.colGKS_YOG.VisibleIndex = 17;
            // 
            // colGKS_INCE_YOG
            // 
            this.colGKS_INCE_YOG.FieldName = "GKS_INCE_YOG";
            this.colGKS_INCE_YOG.Name = "colGKS_INCE_YOG";
            this.colGKS_INCE_YOG.Visible = true;
            this.colGKS_INCE_YOG.VisibleIndex = 18;
            // 
            // tabpageSiparis
            // 
            this.tabpageSiparis.Controls.Add(this.ceYerelSiparisOku);
            this.tabpageSiparis.Controls.Add(this.btnSiparisAktar);
            this.tabpageSiparis.Controls.Add(this.btnSiparisOku);
            this.tabpageSiparis.Controls.Add(this.grdSiparis);
            this.tabpageSiparis.Name = "tabpageSiparis";
            this.tabpageSiparis.Size = new System.Drawing.Size(927, 232);
            this.tabpageSiparis.Text = "Sipariş Listesi";
            // 
            // ceYerelSiparisOku
            // 
            this.ceYerelSiparisOku.Location = new System.Drawing.Point(373, 7);
            this.ceYerelSiparisOku.Name = "ceYerelSiparisOku";
            this.ceYerelSiparisOku.Properties.Caption = "Yerel Sipariş Oku";
            this.ceYerelSiparisOku.Size = new System.Drawing.Size(159, 19);
            this.ceYerelSiparisOku.TabIndex = 6;
            // 
            // btnSiparisAktar
            // 
            this.btnSiparisAktar.BackColor = System.Drawing.Color.Orange;
            this.btnSiparisAktar.ForeColor = System.Drawing.Color.Black;
            this.btnSiparisAktar.Location = new System.Drawing.Point(167, 3);
            this.btnSiparisAktar.Name = "btnSiparisAktar";
            this.btnSiparisAktar.Size = new System.Drawing.Size(200, 27);
            this.btnSiparisAktar.TabIndex = 5;
            this.btnSiparisAktar.Text = "Siparişleri Veritabanına Aktar";
            this.btnSiparisAktar.UseVisualStyleBackColor = false;
            this.btnSiparisAktar.Click += new System.EventHandler(this.btnSiparisAktar_Click);
            // 
            // btnSiparisOku
            // 
            this.btnSiparisOku.BackColor = System.Drawing.Color.Orange;
            this.btnSiparisOku.ForeColor = System.Drawing.Color.Black;
            this.btnSiparisOku.Location = new System.Drawing.Point(5, 3);
            this.btnSiparisOku.Name = "btnSiparisOku";
            this.btnSiparisOku.Size = new System.Drawing.Size(156, 27);
            this.btnSiparisOku.TabIndex = 4;
            this.btnSiparisOku.Text = "Sipariş Listesini Getir";
            this.btnSiparisOku.UseVisualStyleBackColor = false;
            this.btnSiparisOku.Click += new System.EventHandler(this.btnSiparisOku_Click);
            // 
            // grdSiparis
            // 
            this.grdSiparis.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdSiparis.DataSource = this.bindIFSPlanline;
            this.grdSiparis.Location = new System.Drawing.Point(4, 32);
            this.grdSiparis.MainView = this.gviewSiparis;
            this.grdSiparis.Name = "grdSiparis";
            this.grdSiparis.Size = new System.Drawing.Size(918, 197);
            this.grdSiparis.TabIndex = 3;
            this.grdSiparis.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gviewSiparis});
            // 
            // bindIFSPlanline
            // 
            this.bindIFSPlanline.DataSource = typeof(ENOYAEntegrasyonV2.Models.Entities.IFSPLANLine);
            // 
            // gviewSiparis
            // 
            this.gviewSiparis.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colLuname,
            this.colOrderNo,
            this.colReleaseNo,
            this.colSequenceNo,
            this.colOrderCode,
            this.colProcessType,
            this.colDateEntered,
            this.colContract,
            this.colPartNo,
            this.colPartDesc,
            this.colRevisedQtyDue,
            this.colEngChgLevel,
            this.colStructureAlternative,
            this.colAlternativeDesc,
            this.colLineItemNo,
            this.colComponentPartNo,
            this.colCompPartDesc,
            this.colProductFamily,
            this.colQtyRequired,
            this.colQtyPerAssembly,
            this.colMikser,
            this.colRecycleUsageFactor,
            this.colRecycleUsageFactorV,
            this.colMixingTime,
            this.colRoutingAlternative,
            this.colEarliestStartDate,
            this.colCustomerId,
            this.colCustomerName,
            this.colA,
            this.colB,
            this.colC,
            this.colD,
            this.colAdressId,
            this.colAdressDesc});
            this.gviewSiparis.GridControl = this.grdSiparis;
            this.gviewSiparis.Name = "gviewSiparis";
            this.gviewSiparis.OptionsBehavior.Editable = false;
            this.gviewSiparis.OptionsView.BestFitMode = DevExpress.XtraGrid.Views.Grid.GridBestFitMode.Full;
            this.gviewSiparis.OptionsView.ColumnAutoWidth = false;
            this.gviewSiparis.OptionsView.ShowGroupPanel = false;
            // 
            // colLuname
            // 
            this.colLuname.FieldName = "Luname";
            this.colLuname.Name = "colLuname";
            this.colLuname.Visible = true;
            this.colLuname.VisibleIndex = 0;
            // 
            // colOrderNo
            // 
            this.colOrderNo.FieldName = "OrderNo";
            this.colOrderNo.Name = "colOrderNo";
            this.colOrderNo.Visible = true;
            this.colOrderNo.VisibleIndex = 1;
            // 
            // colReleaseNo
            // 
            this.colReleaseNo.FieldName = "ReleaseNo";
            this.colReleaseNo.Name = "colReleaseNo";
            this.colReleaseNo.Visible = true;
            this.colReleaseNo.VisibleIndex = 2;
            // 
            // colSequenceNo
            // 
            this.colSequenceNo.FieldName = "SequenceNo";
            this.colSequenceNo.Name = "colSequenceNo";
            this.colSequenceNo.Visible = true;
            this.colSequenceNo.VisibleIndex = 3;
            // 
            // colOrderCode
            // 
            this.colOrderCode.FieldName = "OrderCode";
            this.colOrderCode.Name = "colOrderCode";
            this.colOrderCode.Visible = true;
            this.colOrderCode.VisibleIndex = 4;
            // 
            // colProcessType
            // 
            this.colProcessType.FieldName = "ProcessType";
            this.colProcessType.Name = "colProcessType";
            this.colProcessType.Visible = true;
            this.colProcessType.VisibleIndex = 5;
            // 
            // colDateEntered
            // 
            this.colDateEntered.FieldName = "DateEntered";
            this.colDateEntered.Name = "colDateEntered";
            this.colDateEntered.Visible = true;
            this.colDateEntered.VisibleIndex = 6;
            // 
            // colContract
            // 
            this.colContract.FieldName = "Contract";
            this.colContract.Name = "colContract";
            this.colContract.Visible = true;
            this.colContract.VisibleIndex = 7;
            // 
            // colPartNo
            // 
            this.colPartNo.FieldName = "PartNo";
            this.colPartNo.Name = "colPartNo";
            this.colPartNo.Visible = true;
            this.colPartNo.VisibleIndex = 8;
            // 
            // colPartDesc
            // 
            this.colPartDesc.FieldName = "PartDesc";
            this.colPartDesc.Name = "colPartDesc";
            this.colPartDesc.Visible = true;
            this.colPartDesc.VisibleIndex = 9;
            // 
            // colRevisedQtyDue
            // 
            this.colRevisedQtyDue.FieldName = "RevisedQtyDue";
            this.colRevisedQtyDue.Name = "colRevisedQtyDue";
            this.colRevisedQtyDue.Visible = true;
            this.colRevisedQtyDue.VisibleIndex = 10;
            // 
            // colEngChgLevel
            // 
            this.colEngChgLevel.FieldName = "EngChgLevel";
            this.colEngChgLevel.Name = "colEngChgLevel";
            this.colEngChgLevel.Visible = true;
            this.colEngChgLevel.VisibleIndex = 11;
            // 
            // colStructureAlternative
            // 
            this.colStructureAlternative.FieldName = "StructureAlternative";
            this.colStructureAlternative.Name = "colStructureAlternative";
            this.colStructureAlternative.Visible = true;
            this.colStructureAlternative.VisibleIndex = 12;
            // 
            // colAlternativeDesc
            // 
            this.colAlternativeDesc.FieldName = "AlternativeDesc";
            this.colAlternativeDesc.Name = "colAlternativeDesc";
            this.colAlternativeDesc.Visible = true;
            this.colAlternativeDesc.VisibleIndex = 13;
            // 
            // colLineItemNo
            // 
            this.colLineItemNo.FieldName = "LineItemNo";
            this.colLineItemNo.Name = "colLineItemNo";
            this.colLineItemNo.Visible = true;
            this.colLineItemNo.VisibleIndex = 14;
            // 
            // colComponentPartNo
            // 
            this.colComponentPartNo.FieldName = "ComponentPartNo";
            this.colComponentPartNo.Name = "colComponentPartNo";
            this.colComponentPartNo.Visible = true;
            this.colComponentPartNo.VisibleIndex = 15;
            // 
            // colCompPartDesc
            // 
            this.colCompPartDesc.FieldName = "CompPartDesc";
            this.colCompPartDesc.Name = "colCompPartDesc";
            this.colCompPartDesc.Visible = true;
            this.colCompPartDesc.VisibleIndex = 16;
            // 
            // colProductFamily
            // 
            this.colProductFamily.FieldName = "ProductFamily";
            this.colProductFamily.Name = "colProductFamily";
            this.colProductFamily.Visible = true;
            this.colProductFamily.VisibleIndex = 17;
            // 
            // colQtyRequired
            // 
            this.colQtyRequired.FieldName = "QtyRequired";
            this.colQtyRequired.Name = "colQtyRequired";
            this.colQtyRequired.Visible = true;
            this.colQtyRequired.VisibleIndex = 18;
            // 
            // colQtyPerAssembly
            // 
            this.colQtyPerAssembly.FieldName = "QtyPerAssembly";
            this.colQtyPerAssembly.Name = "colQtyPerAssembly";
            this.colQtyPerAssembly.Visible = true;
            this.colQtyPerAssembly.VisibleIndex = 19;
            // 
            // colMikser
            // 
            this.colMikser.FieldName = "Mikser";
            this.colMikser.Name = "colMikser";
            this.colMikser.Visible = true;
            this.colMikser.VisibleIndex = 20;
            // 
            // colRecycleUsageFactor
            // 
            this.colRecycleUsageFactor.FieldName = "RecycleUsageFactor";
            this.colRecycleUsageFactor.Name = "colRecycleUsageFactor";
            this.colRecycleUsageFactor.Visible = true;
            this.colRecycleUsageFactor.VisibleIndex = 21;
            // 
            // colRecycleUsageFactorV
            // 
            this.colRecycleUsageFactorV.FieldName = "RecycleUsageFactorV";
            this.colRecycleUsageFactorV.Name = "colRecycleUsageFactorV";
            this.colRecycleUsageFactorV.Visible = true;
            this.colRecycleUsageFactorV.VisibleIndex = 22;
            // 
            // colMixingTime
            // 
            this.colMixingTime.FieldName = "MixingTime";
            this.colMixingTime.Name = "colMixingTime";
            this.colMixingTime.Visible = true;
            this.colMixingTime.VisibleIndex = 23;
            // 
            // colRoutingAlternative
            // 
            this.colRoutingAlternative.FieldName = "RoutingAlternative";
            this.colRoutingAlternative.Name = "colRoutingAlternative";
            this.colRoutingAlternative.Visible = true;
            this.colRoutingAlternative.VisibleIndex = 24;
            // 
            // colEarliestStartDate
            // 
            this.colEarliestStartDate.FieldName = "EarliestStartDate";
            this.colEarliestStartDate.Name = "colEarliestStartDate";
            this.colEarliestStartDate.Visible = true;
            this.colEarliestStartDate.VisibleIndex = 25;
            // 
            // colCustomerId
            // 
            this.colCustomerId.FieldName = "CustomerId";
            this.colCustomerId.Name = "colCustomerId";
            this.colCustomerId.Visible = true;
            this.colCustomerId.VisibleIndex = 26;
            // 
            // colCustomerName
            // 
            this.colCustomerName.FieldName = "CustomerName";
            this.colCustomerName.Name = "colCustomerName";
            this.colCustomerName.Visible = true;
            this.colCustomerName.VisibleIndex = 27;
            // 
            // colA
            // 
            this.colA.FieldName = "A";
            this.colA.Name = "colA";
            this.colA.Visible = true;
            this.colA.VisibleIndex = 28;
            // 
            // colB
            // 
            this.colB.FieldName = "B";
            this.colB.Name = "colB";
            this.colB.Visible = true;
            this.colB.VisibleIndex = 29;
            // 
            // colC
            // 
            this.colC.FieldName = "C";
            this.colC.Name = "colC";
            this.colC.Visible = true;
            this.colC.VisibleIndex = 30;
            // 
            // colD
            // 
            this.colD.FieldName = "D";
            this.colD.Name = "colD";
            this.colD.Visible = true;
            this.colD.VisibleIndex = 31;
            // 
            // colAdressId
            // 
            this.colAdressId.FieldName = "AdressId";
            this.colAdressId.Name = "colAdressId";
            this.colAdressId.Visible = true;
            this.colAdressId.VisibleIndex = 32;
            // 
            // colAdressDesc
            // 
            this.colAdressDesc.FieldName = "AdressDesc";
            this.colAdressDesc.Name = "colAdressDesc";
            this.colAdressDesc.Visible = true;
            this.colAdressDesc.VisibleIndex = 33;
            // 
            // tabpagePerDetaylar
            // 
            this.tabpagePerDetaylar.Controls.Add(this.btnPeriyotlarMerkezeGonder);
            this.tabpagePerDetaylar.Controls.Add(this.btnPeriyotListesiGetir);
            this.tabpagePerDetaylar.Controls.Add(this.grdPeriyotlar);
            this.tabpagePerDetaylar.Name = "tabpagePerDetaylar";
            this.tabpagePerDetaylar.Size = new System.Drawing.Size(927, 232);
            this.tabpagePerDetaylar.Text = "Periyot Sevkiyatları";
            // 
            // btnPeriyotlarMerkezeGonder
            // 
            this.btnPeriyotlarMerkezeGonder.BackColor = System.Drawing.Color.Orange;
            this.btnPeriyotlarMerkezeGonder.ForeColor = System.Drawing.Color.Black;
            this.btnPeriyotlarMerkezeGonder.Location = new System.Drawing.Point(167, 3);
            this.btnPeriyotlarMerkezeGonder.Name = "btnPeriyotlarMerkezeGonder";
            this.btnPeriyotlarMerkezeGonder.Size = new System.Drawing.Size(200, 27);
            this.btnPeriyotlarMerkezeGonder.TabIndex = 8;
            this.btnPeriyotlarMerkezeGonder.Text = "Periyotları Merkeze Gönder";
            this.btnPeriyotlarMerkezeGonder.UseVisualStyleBackColor = false;
            this.btnPeriyotlarMerkezeGonder.Click += new System.EventHandler(this.btnPeriyotlarMerkezeGonder_Click);
            // 
            // btnPeriyotListesiGetir
            // 
            this.btnPeriyotListesiGetir.BackColor = System.Drawing.Color.Orange;
            this.btnPeriyotListesiGetir.ForeColor = System.Drawing.Color.Black;
            this.btnPeriyotListesiGetir.Location = new System.Drawing.Point(5, 3);
            this.btnPeriyotListesiGetir.Name = "btnPeriyotListesiGetir";
            this.btnPeriyotListesiGetir.Size = new System.Drawing.Size(156, 27);
            this.btnPeriyotListesiGetir.TabIndex = 7;
            this.btnPeriyotListesiGetir.Text = "Periyot Listesini Getir";
            this.btnPeriyotListesiGetir.UseVisualStyleBackColor = false;
            this.btnPeriyotListesiGetir.Click += new System.EventHandler(this.btnPeriyotListesiGetir_Click);
            // 
            // grdPeriyotlar
            // 
            this.grdPeriyotlar.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdPeriyotlar.DataSource = this.bindPerDetay;
            this.grdPeriyotlar.Location = new System.Drawing.Point(4, 32);
            this.grdPeriyotlar.MainView = this.gviewPeriyotlar;
            this.grdPeriyotlar.Name = "grdPeriyotlar";
            this.grdPeriyotlar.Size = new System.Drawing.Size(918, 197);
            this.grdPeriyotlar.TabIndex = 6;
            this.grdPeriyotlar.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gviewPeriyotlar});
            // 
            // bindPerDetay
            // 
            this.bindPerDetay.DataSource = typeof(ENOYAEntegrasyonV2.Models.Entities.PERDETAY);
            // 
            // gviewPeriyotlar
            // 
            this.gviewPeriyotlar.GridControl = this.grdPeriyotlar;
            this.gviewPeriyotlar.Name = "gviewPeriyotlar";
            this.gviewPeriyotlar.OptionsBehavior.Editable = false;
            this.gviewPeriyotlar.OptionsView.BestFitMode = DevExpress.XtraGrid.Views.Grid.GridBestFitMode.Full;
            this.gviewPeriyotlar.OptionsView.ColumnAutoWidth = false;
            this.gviewPeriyotlar.OptionsView.ShowGroupPanel = false;
            this.gviewPeriyotlar.DoubleClick += new System.EventHandler(this.gviewPeriyotlar_DoubleClick);
            // 
            // ıFSPLANBindingSource
            // 
            this.ıFSPLANBindingSource.DataSource = typeof(ENOYAEntegrasyonV2.Models.Entities.IFSPLAN);
            // 
            // grpContIslemler
            // 
            this.grpContIslemler.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpContIslemler.Controls.Add(this.lblStatus);
            this.grpContIslemler.Controls.Add(this.txtLog);
            this.grpContIslemler.Controls.Add(this.grpActions);
            this.grpContIslemler.Controls.Add(this.grpSettings);
            this.grpContIslemler.Controls.Add(this.grpConnection);
            this.grpContIslemler.Location = new System.Drawing.Point(0, 0);
            this.grpContIslemler.Name = "grpContIslemler";
            this.grpContIslemler.Size = new System.Drawing.Size(933, 234);
            this.grpContIslemler.TabIndex = 1;
            this.grpContIslemler.Text = "İşlem İzleme";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(430, 90);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(31, 13);
            this.lblStatus.TabIndex = 7;
            this.lblStatus.Text = "Hazır";
            // 
            // txtLog
            // 
            this.txtLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLog.BackColor = System.Drawing.Color.Black;
            this.txtLog.Font = new System.Drawing.Font("Consolas", 9F);
            this.txtLog.ForeColor = System.Drawing.Color.Lime;
            this.txtLog.Location = new System.Drawing.Point(418, 106);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(503, 119);
            this.txtLog.TabIndex = 6;
            // 
            // grpActions
            // 
            this.grpActions.Controls.Add(this.btnSyncNow);
            this.grpActions.Controls.Add(this.btnSettings);
            this.grpActions.Controls.Add(this.btnStartStop);
            this.grpActions.Location = new System.Drawing.Point(418, 23);
            this.grpActions.Name = "grpActions";
            this.grpActions.Size = new System.Drawing.Size(400, 60);
            this.grpActions.TabIndex = 5;
            this.grpActions.TabStop = false;
            this.grpActions.Text = "İşlemler";
            // 
            // btnSyncNow
            // 
            this.btnSyncNow.Location = new System.Drawing.Point(245, 20);
            this.btnSyncNow.Name = "btnSyncNow";
            this.btnSyncNow.Size = new System.Drawing.Size(140, 30);
            this.btnSyncNow.TabIndex = 2;
            this.btnSyncNow.Text = "Şimdi Senkronize Et";
            this.btnSyncNow.UseVisualStyleBackColor = true;
            this.btnSyncNow.Click += new System.EventHandler(this.btnSyncNow_Click);
            // 
            // btnSettings
            // 
            this.btnSettings.Location = new System.Drawing.Point(130, 20);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(100, 30);
            this.btnSettings.TabIndex = 1;
            this.btnSettings.Text = "Ayarlar";
            this.btnSettings.UseVisualStyleBackColor = true;
            this.btnSettings.Click += new System.EventHandler(this.btnSettings_Click);
            // 
            // btnStartStop
            // 
            this.btnStartStop.BackColor = System.Drawing.Color.Green;
            this.btnStartStop.ForeColor = System.Drawing.Color.White;
            this.btnStartStop.Location = new System.Drawing.Point(15, 20);
            this.btnStartStop.Name = "btnStartStop";
            this.btnStartStop.Size = new System.Drawing.Size(100, 30);
            this.btnStartStop.TabIndex = 0;
            this.btnStartStop.Text = "BAŞLAT";
            this.btnStartStop.UseVisualStyleBackColor = false;
            this.btnStartStop.Click += new System.EventHandler(this.btnStartStop_Click);
            // 
            // grpSettings
            // 
            this.grpSettings.Controls.Add(this.teAlternativeRoute);
            this.grpSettings.Controls.Add(this.chkUseAlternativeRoute);
            this.grpSettings.Controls.Add(this.lblInterval);
            this.grpSettings.Controls.Add(this.numInterval);
            this.grpSettings.Controls.Add(this.chkMinimizeToTray);
            this.grpSettings.Controls.Add(this.chkAutoStart);
            this.grpSettings.Location = new System.Drawing.Point(12, 106);
            this.grpSettings.Name = "grpSettings";
            this.grpSettings.Size = new System.Drawing.Size(400, 119);
            this.grpSettings.TabIndex = 4;
            this.grpSettings.TabStop = false;
            this.grpSettings.Text = "Ayarlar";
            // 
            // teAlternativeRoute
            // 
            this.teAlternativeRoute.Location = new System.Drawing.Point(154, 67);
            this.teAlternativeRoute.Name = "teAlternativeRoute";
            this.teAlternativeRoute.Size = new System.Drawing.Size(100, 21);
            this.teAlternativeRoute.TabIndex = 5;
            // 
            // chkUseAlternativeRoute
            // 
            this.chkUseAlternativeRoute.AutoSize = true;
            this.chkUseAlternativeRoute.Location = new System.Drawing.Point(15, 71);
            this.chkUseAlternativeRoute.Name = "chkUseAlternativeRoute";
            this.chkUseAlternativeRoute.Size = new System.Drawing.Size(128, 17);
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
            this.lblInterval.Size = new System.Drawing.Size(101, 13);
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
            this.numInterval.Size = new System.Drawing.Size(100, 21);
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
            this.chkAutoStart.Size = new System.Drawing.Size(101, 17);
            this.chkAutoStart.TabIndex = 0;
            this.chkAutoStart.Text = "Otomatik Başlat";
            this.chkAutoStart.UseVisualStyleBackColor = true;
            // 
            // grpConnection
            // 
            this.grpConnection.Controls.Add(this.lblApiStatus);
            this.grpConnection.Controls.Add(this.lblDatabaseStatus);
            this.grpConnection.Location = new System.Drawing.Point(12, 23);
            this.grpConnection.Name = "grpConnection";
            this.grpConnection.Size = new System.Drawing.Size(400, 80);
            this.grpConnection.TabIndex = 3;
            this.grpConnection.TabStop = false;
            this.grpConnection.Text = "Bağlantı Durumu";
            // 
            // lblApiStatus
            // 
            this.lblApiStatus.AutoSize = true;
            this.lblApiStatus.Location = new System.Drawing.Point(15, 50);
            this.lblApiStatus.Name = "lblApiStatus";
            this.lblApiStatus.Size = new System.Drawing.Size(177, 13);
            this.lblApiStatus.TabIndex = 1;
            this.lblApiStatus.Text = "REST API Bağlantısı: Test ediliyor...";
            // 
            // lblDatabaseStatus
            // 
            this.lblDatabaseStatus.AutoSize = true;
            this.lblDatabaseStatus.Location = new System.Drawing.Point(15, 25);
            this.lblDatabaseStatus.Name = "lblDatabaseStatus";
            this.lblDatabaseStatus.Size = new System.Drawing.Size(165, 13);
            this.lblDatabaseStatus.TabIndex = 0;
            this.lblDatabaseStatus.Text = "MSSQL Bağlantısı: Test ediliyor...";
            // 
            // contextMalzeme
            // 
            this.contextMalzeme.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exceleAktarToolStripMenuItem});
            this.contextMalzeme.Name = "contextMalzeme";
            this.contextMalzeme.Size = new System.Drawing.Size(138, 26);
            this.contextMalzeme.Opening += new System.ComponentModel.CancelEventHandler(this.contextMalzeme_Opening);
            // 
            // contextSiparis
            // 
            this.contextSiparis.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exceleAktarToolStripMenuItem1});
            this.contextSiparis.Name = "contextMalzeme";
            this.contextSiparis.Size = new System.Drawing.Size(181, 48);
            // 
            // exceleAktarToolStripMenuItem
            // 
            this.exceleAktarToolStripMenuItem.Name = "exceleAktarToolStripMenuItem";
            this.exceleAktarToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.exceleAktarToolStripMenuItem.Text = "Excele Aktar";
            // 
            // exceleAktarToolStripMenuItem1
            // 
            this.exceleAktarToolStripMenuItem1.Name = "exceleAktarToolStripMenuItem1";
            this.exceleAktarToolStripMenuItem1.Size = new System.Drawing.Size(180, 22);
            this.exceleAktarToolStripMenuItem1.Text = "Excele Aktar";
            this.exceleAktarToolStripMenuItem1.Click += new System.EventHandler(this.exceleAktarToolStripMenuItem1_Click);
            // 
            // FrmMainMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(933, 499);
            this.Controls.Add(this.grpContIslemler);
            this.Controls.Add(this.tabcontMainMenu);
            this.IconOptions.Icon = ((System.Drawing.Icon)(resources.GetObject("FrmMainMenu.IconOptions.Icon")));
            this.Name = "FrmMainMenu";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Bursa Beton IFS-Enoya Aktarım Uygulaması v1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmMainMenu_FormClosing);
            this.Load += new System.EventHandler(this.FrmMainMenu_Load);
            ((System.ComponentModel.ISupportInitialize)(this.tabcontMainMenu)).EndInit();
            this.tabcontMainMenu.ResumeLayout(false);
            this.tabpageMalzeme.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.deTarih.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deTarih.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdMalzeme)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindMalzeme)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gviewMalzeme)).EndInit();
            this.tabpageSiparis.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ceYerelSiparisOku.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdSiparis)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindIFSPlanline)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gviewSiparis)).EndInit();
            this.tabpagePerDetaylar.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdPeriyotlar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindPerDetay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gviewPeriyotlar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ıFSPLANBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpContIslemler)).EndInit();
            this.grpContIslemler.ResumeLayout(false);
            this.grpContIslemler.PerformLayout();
            this.grpActions.ResumeLayout(false);
            this.grpSettings.ResumeLayout(false);
            this.grpSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numInterval)).EndInit();
            this.grpConnection.ResumeLayout(false);
            this.grpConnection.PerformLayout();
            this.contextMalzeme.ResumeLayout(false);
            this.contextSiparis.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraTab.XtraTabControl tabcontMainMenu;
        private DevExpress.XtraTab.XtraTabPage tabpageMalzeme;
        private DevExpress.XtraTab.XtraTabPage tabpageSiparis;
        private DevExpress.XtraEditors.GroupControl grpContIslemler;
        private System.Windows.Forms.GroupBox grpSettings;
        private System.Windows.Forms.Label lblInterval;
        private System.Windows.Forms.NumericUpDown numInterval;
        private System.Windows.Forms.CheckBox chkMinimizeToTray;
        private System.Windows.Forms.CheckBox chkAutoStart;
        private System.Windows.Forms.GroupBox grpConnection;
        private System.Windows.Forms.Label lblApiStatus;
        private System.Windows.Forms.Label lblDatabaseStatus;
        private System.Windows.Forms.GroupBox grpActions;
        private System.Windows.Forms.Button btnSyncNow;
        private System.Windows.Forms.Button btnSettings;
        private System.Windows.Forms.Button btnStartStop;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.TextBox teAlternativeRoute;
        private System.Windows.Forms.CheckBox chkUseAlternativeRoute;
        private System.Windows.Forms.Button btnMalzemeListesiGetir;
        private DevExpress.XtraGrid.GridControl grdMalzeme;
        private System.Windows.Forms.BindingSource bindMalzeme;
        private DevExpress.XtraGrid.Views.Grid.GridView gviewMalzeme;
        private DevExpress.XtraGrid.Columns.GridColumn colKOD;
        private DevExpress.XtraGrid.Columns.GridColumn colAD;
        private DevExpress.XtraGrid.Columns.GridColumn colSTOK_MIKTARI;
        private DevExpress.XtraGrid.Columns.GridColumn colSTOK_KONTROL;
        private DevExpress.XtraGrid.Columns.GridColumn colACIKLAMA;
        private DevExpress.XtraGrid.Columns.GridColumn colMINIMUM_MIKTAR;
        private DevExpress.XtraGrid.Columns.GridColumn colSILO_KAPASITE;
        private DevExpress.XtraGrid.Columns.GridColumn colAGREGA;
        private DevExpress.XtraGrid.Columns.GridColumn colCIMENTO;
        private DevExpress.XtraGrid.Columns.GridColumn colKUL;
        private DevExpress.XtraGrid.Columns.GridColumn colTOZ;
        private DevExpress.XtraGrid.Columns.GridColumn colKUM;
        private DevExpress.XtraGrid.Columns.GridColumn colSU;
        private DevExpress.XtraGrid.Columns.GridColumn colKATKI;
        private DevExpress.XtraGrid.Columns.GridColumn colEKSINEMLIMIT;
        private DevExpress.XtraGrid.Columns.GridColumn colPART_NO;
        private DevExpress.XtraGrid.Columns.GridColumn colPROD_FAMILY;
        private DevExpress.XtraGrid.Columns.GridColumn colGKS_YOG;
        private DevExpress.XtraGrid.Columns.GridColumn colGKS_INCE_YOG;
        private System.Windows.Forms.Button btnMalzemeleriDByeAktar;
        private System.Windows.Forms.Button btnSiparisAktar;
        private System.Windows.Forms.Button btnSiparisOku;
        private DevExpress.XtraGrid.GridControl grdSiparis;
        private System.Windows.Forms.BindingSource ıFSPLANBindingSource;
        private DevExpress.XtraGrid.Views.Grid.GridView gviewSiparis;
        private System.Windows.Forms.BindingSource bindIFSPlanline;
        private DevExpress.XtraGrid.Columns.GridColumn colLuname;
        private DevExpress.XtraGrid.Columns.GridColumn colOrderNo;
        private DevExpress.XtraGrid.Columns.GridColumn colReleaseNo;
        private DevExpress.XtraGrid.Columns.GridColumn colSequenceNo;
        private DevExpress.XtraGrid.Columns.GridColumn colOrderCode;
        private DevExpress.XtraGrid.Columns.GridColumn colProcessType;
        private DevExpress.XtraGrid.Columns.GridColumn colDateEntered;
        private DevExpress.XtraGrid.Columns.GridColumn colContract;
        private DevExpress.XtraGrid.Columns.GridColumn colPartNo;
        private DevExpress.XtraGrid.Columns.GridColumn colPartDesc;
        private DevExpress.XtraGrid.Columns.GridColumn colRevisedQtyDue;
        private DevExpress.XtraGrid.Columns.GridColumn colEngChgLevel;
        private DevExpress.XtraGrid.Columns.GridColumn colStructureAlternative;
        private DevExpress.XtraGrid.Columns.GridColumn colAlternativeDesc;
        private DevExpress.XtraGrid.Columns.GridColumn colLineItemNo;
        private DevExpress.XtraGrid.Columns.GridColumn colComponentPartNo;
        private DevExpress.XtraGrid.Columns.GridColumn colCompPartDesc;
        private DevExpress.XtraGrid.Columns.GridColumn colProductFamily;
        private DevExpress.XtraGrid.Columns.GridColumn colQtyRequired;
        private DevExpress.XtraGrid.Columns.GridColumn colQtyPerAssembly;
        private DevExpress.XtraGrid.Columns.GridColumn colMikser;
        private DevExpress.XtraGrid.Columns.GridColumn colRecycleUsageFactor;
        private DevExpress.XtraGrid.Columns.GridColumn colRecycleUsageFactorV;
        private DevExpress.XtraGrid.Columns.GridColumn colMixingTime;
        private DevExpress.XtraGrid.Columns.GridColumn colRoutingAlternative;
        private DevExpress.XtraGrid.Columns.GridColumn colEarliestStartDate;
        private DevExpress.XtraGrid.Columns.GridColumn colCustomerId;
        private DevExpress.XtraGrid.Columns.GridColumn colCustomerName;
        private DevExpress.XtraGrid.Columns.GridColumn colA;
        private DevExpress.XtraGrid.Columns.GridColumn colB;
        private DevExpress.XtraGrid.Columns.GridColumn colC;
        private DevExpress.XtraGrid.Columns.GridColumn colD;
        private DevExpress.XtraGrid.Columns.GridColumn colAdressId;
        private DevExpress.XtraGrid.Columns.GridColumn colAdressDesc;
        private DevExpress.XtraTab.XtraTabPage tabpagePerDetaylar;
        private System.Windows.Forms.Button btnPeriyotlarMerkezeGonder;
        private System.Windows.Forms.Button btnPeriyotListesiGetir;
        private DevExpress.XtraGrid.GridControl grdPeriyotlar;
        private System.Windows.Forms.BindingSource bindPerDetay;
        private DevExpress.XtraGrid.Views.Grid.GridView gviewPeriyotlar;
        private DevExpress.XtraEditors.DateEdit deTarih;
        private DevExpress.XtraEditors.CheckEdit ceYerelSiparisOku;
        private System.Windows.Forms.ContextMenuStrip contextMalzeme;
        private System.Windows.Forms.ToolStripMenuItem exceleAktarToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextSiparis;
        private System.Windows.Forms.ToolStripMenuItem exceleAktarToolStripMenuItem1;
    }
}