namespace POS
{
    partial class MDIParent
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
         {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MDIParent));
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.statusToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.versiontoolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.accountToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.addNewUserToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.userListToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.roleManagementToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.productToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.productCategoryToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.productSubCategoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.brandToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.addNewProductToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.productListToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.productPriceChangeHistoryListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saleToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.transactionToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.transactionListToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.configurationSettingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.counterToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.addCityToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addShopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.localizationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.assignTicketToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reportsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.dailySummaryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.transactionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.transactionSummaryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.transactionDetailByItemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.averageMonthlyReportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.itemSummaryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.topToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.databaseExportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.databaseImportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logInToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.logOutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tSSBOOrPOS = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.ofdDBBackup = new System.Windows.Forms.OpenFileDialog();
            this.menuStrip.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileMenu,
            this.accountToolStripMenuItem1,
            this.productToolStripMenuItem,
            this.saleToolStripMenuItem,
            this.transactionToolStripMenuItem1,
            this.settingsToolStripMenuItem1,
            this.reportsToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.logInToolStripMenuItem1,
            this.logOutToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Padding = new System.Windows.Forms.Padding(6, 3, 0, 3);
            this.menuStrip.Size = new System.Drawing.Size(1370, 30);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "MenuStrip";
            this.menuStrip.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.menuStrip_ItemClicked);
            // 
            // fileMenu
            // 
            this.fileMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusToolStripMenuItem,
            this.toolStripSeparator5,
            this.versiontoolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileMenu.ImageTransparentColor = System.Drawing.SystemColors.ActiveBorder;
            this.fileMenu.Name = "fileMenu";
            this.fileMenu.Size = new System.Drawing.Size(70, 24);
            this.fileMenu.Text = "&System";
            // 
            // statusToolStripMenuItem
            // 
            this.statusToolStripMenuItem.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusToolStripMenuItem.ForeColor = System.Drawing.Color.DodgerBlue;
            this.statusToolStripMenuItem.Name = "statusToolStripMenuItem";
            this.statusToolStripMenuItem.Size = new System.Drawing.Size(170, 26);
            this.statusToolStripMenuItem.Text = "status";
            this.statusToolStripMenuItem.Click += new System.EventHandler(this.statusToolStripMenuItem_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(167, 6);
            // 
            // versiontoolStripMenuItem
            // 
            this.versiontoolStripMenuItem.Name = "versiontoolStripMenuItem";
            this.versiontoolStripMenuItem.Size = new System.Drawing.Size(170, 26);
            this.versiontoolStripMenuItem.Text = "Version Info";
            this.versiontoolStripMenuItem.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.versiontoolStripMenuItem.Click += new System.EventHandler(this.versiontoolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(170, 26);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.ExitToolsStripMenuItem_Click);
            // 
            // accountToolStripMenuItem1
            // 
            this.accountToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addNewUserToolStripMenuItem,
            this.userListToolStripMenuItem1,
            this.roleManagementToolStripMenuItem});
            this.accountToolStripMenuItem1.Name = "accountToolStripMenuItem1";
            this.accountToolStripMenuItem1.Size = new System.Drawing.Size(77, 24);
            this.accountToolStripMenuItem1.Text = "&Account";
            // 
            // addNewUserToolStripMenuItem
            // 
            this.addNewUserToolStripMenuItem.Name = "addNewUserToolStripMenuItem";
            this.addNewUserToolStripMenuItem.Size = new System.Drawing.Size(238, 26);
            this.addNewUserToolStripMenuItem.Text = "Add &New User";
            this.addNewUserToolStripMenuItem.Click += new System.EventHandler(this.addNewUserToolStripMenuItem_Click);
            // 
            // userListToolStripMenuItem1
            // 
            this.userListToolStripMenuItem1.Name = "userListToolStripMenuItem1";
            this.userListToolStripMenuItem1.Size = new System.Drawing.Size(238, 26);
            this.userListToolStripMenuItem1.Text = "&User List";
            this.userListToolStripMenuItem1.Click += new System.EventHandler(this.userListToolStripMenuItem1_Click);
            // 
            // roleManagementToolStripMenuItem
            // 
            this.roleManagementToolStripMenuItem.Name = "roleManagementToolStripMenuItem";
            this.roleManagementToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F7;
            this.roleManagementToolStripMenuItem.Size = new System.Drawing.Size(238, 26);
            this.roleManagementToolStripMenuItem.Text = "&Role Management";
            this.roleManagementToolStripMenuItem.Click += new System.EventHandler(this.roleManagementToolStripMenuItem_Click);
            // 
            // productToolStripMenuItem
            // 
            this.productToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.productCategoryToolStripMenuItem1,
            this.productSubCategoryToolStripMenuItem,
            this.brandToolStripMenuItem1,
            this.addNewProductToolStripMenuItem,
            this.productListToolStripMenuItem1,
            this.productPriceChangeHistoryListToolStripMenuItem});
            this.productToolStripMenuItem.Name = "productToolStripMenuItem";
            this.productToolStripMenuItem.Size = new System.Drawing.Size(74, 24);
            this.productToolStripMenuItem.Text = "&Product";
            // 
            // productCategoryToolStripMenuItem1
            // 
            this.productCategoryToolStripMenuItem1.Name = "productCategoryToolStripMenuItem1";
            this.productCategoryToolStripMenuItem1.Size = new System.Drawing.Size(310, 26);
            this.productCategoryToolStripMenuItem1.Text = "Add Product &Category";
            this.productCategoryToolStripMenuItem1.Click += new System.EventHandler(this.productCategoryToolStripMenuItem1_Click);
            // 
            // productSubCategoryToolStripMenuItem
            // 
            this.productSubCategoryToolStripMenuItem.Name = "productSubCategoryToolStripMenuItem";
            this.productSubCategoryToolStripMenuItem.Size = new System.Drawing.Size(310, 26);
            this.productSubCategoryToolStripMenuItem.Text = "Add Product &Sub Category";
            this.productSubCategoryToolStripMenuItem.Click += new System.EventHandler(this.productSubCategoryToolStripMenuItem_Click);
            // 
            // brandToolStripMenuItem1
            // 
            this.brandToolStripMenuItem1.Name = "brandToolStripMenuItem1";
            this.brandToolStripMenuItem1.Size = new System.Drawing.Size(310, 26);
            this.brandToolStripMenuItem1.Text = "Add &Brand ";
            this.brandToolStripMenuItem1.Click += new System.EventHandler(this.brandToolStripMenuItem1_Click);
            // 
            // addNewProductToolStripMenuItem
            // 
            this.addNewProductToolStripMenuItem.Name = "addNewProductToolStripMenuItem";
            this.addNewProductToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3;
            this.addNewProductToolStripMenuItem.Size = new System.Drawing.Size(310, 26);
            this.addNewProductToolStripMenuItem.Text = "Add &New Product";
            this.addNewProductToolStripMenuItem.Click += new System.EventHandler(this.addNewProductToolStripMenuItem_Click);
            // 
            // productListToolStripMenuItem1
            // 
            this.productListToolStripMenuItem1.Name = "productListToolStripMenuItem1";
            this.productListToolStripMenuItem1.ShortcutKeys = System.Windows.Forms.Keys.F4;
            this.productListToolStripMenuItem1.Size = new System.Drawing.Size(310, 26);
            this.productListToolStripMenuItem1.Text = "&Product List";
            this.productListToolStripMenuItem1.Click += new System.EventHandler(this.productListToolStripMenuItem1_Click);
            // 
            // productPriceChangeHistoryListToolStripMenuItem
            // 
            this.productPriceChangeHistoryListToolStripMenuItem.Name = "productPriceChangeHistoryListToolStripMenuItem";
            this.productPriceChangeHistoryListToolStripMenuItem.Size = new System.Drawing.Size(310, 26);
            this.productPriceChangeHistoryListToolStripMenuItem.Text = "Product Price Change &History List";
            this.productPriceChangeHistoryListToolStripMenuItem.Click += new System.EventHandler(this.productPriceChangeHistoryListToolStripMenuItem_Click);
            // 
            // saleToolStripMenuItem
            // 
            this.saleToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saleToolStripMenuItem1});
            this.saleToolStripMenuItem.Name = "saleToolStripMenuItem";
            this.saleToolStripMenuItem.Size = new System.Drawing.Size(57, 24);
            this.saleToolStripMenuItem.Text = "Sal&es";
            // 
            // saleToolStripMenuItem1
            // 
            this.saleToolStripMenuItem1.Name = "saleToolStripMenuItem1";
            this.saleToolStripMenuItem1.Size = new System.Drawing.Size(126, 26);
            this.saleToolStripMenuItem1.Text = "Sales";
            this.saleToolStripMenuItem1.Click += new System.EventHandler(this.saleToolStripMenuItem1_Click);
            // 
            // transactionToolStripMenuItem1
            // 
            this.transactionToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.transactionListToolStripMenuItem1,
            this.deleteLogToolStripMenuItem});
            this.transactionToolStripMenuItem1.Name = "transactionToolStripMenuItem1";
            this.transactionToolStripMenuItem1.Size = new System.Drawing.Size(98, 24);
            this.transactionToolStripMenuItem1.Text = "&Transaction";
            // 
            // transactionListToolStripMenuItem1
            // 
            this.transactionListToolStripMenuItem1.Name = "transactionListToolStripMenuItem1";
            this.transactionListToolStripMenuItem1.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.transactionListToolStripMenuItem1.Size = new System.Drawing.Size(217, 26);
            this.transactionListToolStripMenuItem1.Text = "Transaction &List";
            this.transactionListToolStripMenuItem1.Click += new System.EventHandler(this.transactionListToolStripMenuItem_Click);
            // 
            // deleteLogToolStripMenuItem
            // 
            this.deleteLogToolStripMenuItem.Name = "deleteLogToolStripMenuItem";
            this.deleteLogToolStripMenuItem.Size = new System.Drawing.Size(217, 26);
            this.deleteLogToolStripMenuItem.Text = "&Delete Log";
            this.deleteLogToolStripMenuItem.Click += new System.EventHandler(this.deleteLogToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem1
            // 
            this.settingsToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.configurationSettingToolStripMenuItem,
            this.counterToolStripMenuItem1,
            this.addCityToolStripMenuItem,
            this.addShopToolStripMenuItem,
            this.localizationToolStripMenuItem,
            this.assignTicketToolStripMenuItem});
            this.settingsToolStripMenuItem1.Name = "settingsToolStripMenuItem1";
            this.settingsToolStripMenuItem1.Size = new System.Drawing.Size(76, 24);
            this.settingsToolStripMenuItem1.Text = "&Settings";
            // 
            // configurationSettingToolStripMenuItem
            // 
            this.configurationSettingToolStripMenuItem.Name = "configurationSettingToolStripMenuItem";
            this.configurationSettingToolStripMenuItem.Size = new System.Drawing.Size(250, 26);
            this.configurationSettingToolStripMenuItem.Text = "Configuration (&Settings)";
            this.configurationSettingToolStripMenuItem.Click += new System.EventHandler(this.configurationSettingToolStripMenuItem_Click);
            // 
            // counterToolStripMenuItem1
            // 
            this.counterToolStripMenuItem1.Name = "counterToolStripMenuItem1";
            this.counterToolStripMenuItem1.Size = new System.Drawing.Size(250, 26);
            this.counterToolStripMenuItem1.Text = "&Add Counter";
            this.counterToolStripMenuItem1.Click += new System.EventHandler(this.counterToolStripMenuItem_Click);
            // 
            // addCityToolStripMenuItem
            // 
            this.addCityToolStripMenuItem.Name = "addCityToolStripMenuItem";
            this.addCityToolStripMenuItem.Size = new System.Drawing.Size(250, 26);
            this.addCityToolStripMenuItem.Text = "Add &City";
            this.addCityToolStripMenuItem.Click += new System.EventHandler(this.addCityToolStripMenuItem_Click);
            // 
            // addShopToolStripMenuItem
            // 
            this.addShopToolStripMenuItem.Name = "addShopToolStripMenuItem";
            this.addShopToolStripMenuItem.Size = new System.Drawing.Size(250, 26);
            this.addShopToolStripMenuItem.Text = "Add Zoo";
            this.addShopToolStripMenuItem.Click += new System.EventHandler(this.addShopToolStripMenuItem_Click);
            // 
            // localizationToolStripMenuItem
            // 
            this.localizationToolStripMenuItem.Name = "localizationToolStripMenuItem";
            this.localizationToolStripMenuItem.Size = new System.Drawing.Size(250, 26);
            this.localizationToolStripMenuItem.Text = "Localization";
            this.localizationToolStripMenuItem.Click += new System.EventHandler(this.localizationToolStripMenuItem_Click);
            // 
            // assignTicketToolStripMenuItem
            // 
            this.assignTicketToolStripMenuItem.Name = "assignTicketToolStripMenuItem";
            this.assignTicketToolStripMenuItem.Size = new System.Drawing.Size(250, 26);
            this.assignTicketToolStripMenuItem.Text = "Assign Ticket Buttons";
            this.assignTicketToolStripMenuItem.Click += new System.EventHandler(this.assignTicketToolStripMenuItem_Click);
            // 
            // reportsToolStripMenuItem
            // 
            this.reportsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.toolStripMenuItem2,
            this.dailySummaryToolStripMenuItem,
            this.transactionToolStripMenuItem,
            this.transactionSummaryToolStripMenuItem,
            this.transactionDetailByItemToolStripMenuItem,
            this.averageMonthlyReportToolStripMenuItem,
            this.itemSummaryToolStripMenuItem,
            this.topToolStripMenuItem});
            this.reportsToolStripMenuItem.Name = "reportsToolStripMenuItem";
            this.reportsToolStripMenuItem.Size = new System.Drawing.Size(74, 24);
            this.reportsToolStripMenuItem.Text = "&Reports";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(254, 26);
            this.toolStripMenuItem1.Text = "Ticket Summary Report";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(254, 26);
            this.toolStripMenuItem2.Text = "Ticket Details Report";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // dailySummaryToolStripMenuItem
            // 
            this.dailySummaryToolStripMenuItem.Name = "dailySummaryToolStripMenuItem";
            this.dailySummaryToolStripMenuItem.Size = new System.Drawing.Size(254, 26);
            this.dailySummaryToolStripMenuItem.Text = "Daily Sales Summary";
            this.dailySummaryToolStripMenuItem.Click += new System.EventHandler(this.dailySummaryToolStripMenuItem_Click);
            // 
            // transactionToolStripMenuItem
            // 
            this.transactionToolStripMenuItem.Name = "transactionToolStripMenuItem";
            this.transactionToolStripMenuItem.Size = new System.Drawing.Size(254, 26);
            this.transactionToolStripMenuItem.Text = "&Transactions";
            this.transactionToolStripMenuItem.Click += new System.EventHandler(this.transactionToolStripMenuItem_Click);
            // 
            // transactionSummaryToolStripMenuItem
            // 
            this.transactionSummaryToolStripMenuItem.Name = "transactionSummaryToolStripMenuItem";
            this.transactionSummaryToolStripMenuItem.Size = new System.Drawing.Size(254, 26);
            this.transactionSummaryToolStripMenuItem.Text = "Transaction &Summary";
            this.transactionSummaryToolStripMenuItem.Click += new System.EventHandler(this.transactionSummaryToolStripMenuItem_Click);
            // 
            // transactionDetailByItemToolStripMenuItem
            // 
            this.transactionDetailByItemToolStripMenuItem.Name = "transactionDetailByItemToolStripMenuItem";
            this.transactionDetailByItemToolStripMenuItem.Size = new System.Drawing.Size(254, 26);
            this.transactionDetailByItemToolStripMenuItem.Text = "Transaction &Detail";
            this.transactionDetailByItemToolStripMenuItem.Click += new System.EventHandler(this.transactionDetailByItemToolStripMenuItem_Click);
            // 
            // averageMonthlyReportToolStripMenuItem
            // 
            this.averageMonthlyReportToolStripMenuItem.Name = "averageMonthlyReportToolStripMenuItem";
            this.averageMonthlyReportToolStripMenuItem.Size = new System.Drawing.Size(254, 26);
            this.averageMonthlyReportToolStripMenuItem.Text = "&Average Monthly Report";
            this.averageMonthlyReportToolStripMenuItem.Click += new System.EventHandler(this.averageMonthlyReportToolStripMenuItem_Click);
            // 
            // itemSummaryToolStripMenuItem
            // 
            this.itemSummaryToolStripMenuItem.Name = "itemSummaryToolStripMenuItem";
            this.itemSummaryToolStripMenuItem.Size = new System.Drawing.Size(254, 26);
            this.itemSummaryToolStripMenuItem.Text = "&Item Sale Summary";
            this.itemSummaryToolStripMenuItem.Click += new System.EventHandler(this.itemSummaryToolStripMenuItem_Click);
            // 
            // topToolStripMenuItem
            // 
            this.topToolStripMenuItem.Name = "topToolStripMenuItem";
            this.topToolStripMenuItem.Size = new System.Drawing.Size(254, 26);
            this.topToolStripMenuItem.Text = "&Best Seller Items";
            this.topToolStripMenuItem.Click += new System.EventHandler(this.topToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.databaseExportToolStripMenuItem,
            this.databaseImportToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(58, 24);
            this.toolsToolStripMenuItem.Text = "T&ools";
            // 
            // databaseExportToolStripMenuItem
            // 
            this.databaseExportToolStripMenuItem.Name = "databaseExportToolStripMenuItem";
            this.databaseExportToolStripMenuItem.Size = new System.Drawing.Size(209, 26);
            this.databaseExportToolStripMenuItem.Text = "&Backup Database";
            this.databaseExportToolStripMenuItem.Click += new System.EventHandler(this.databaseExportToolStripMenuItem_Click);
            // 
            // databaseImportToolStripMenuItem
            // 
            this.databaseImportToolStripMenuItem.Name = "databaseImportToolStripMenuItem";
            this.databaseImportToolStripMenuItem.Size = new System.Drawing.Size(209, 26);
            this.databaseImportToolStripMenuItem.Text = "&Restore Database";
            this.databaseImportToolStripMenuItem.Click += new System.EventHandler(this.databaseImportToolStripMenuItem_Click);
            // 
            // logInToolStripMenuItem1
            // 
            this.logInToolStripMenuItem1.Name = "logInToolStripMenuItem1";
            this.logInToolStripMenuItem1.Size = new System.Drawing.Size(64, 24);
            this.logInToolStripMenuItem1.Text = "&Log In";
            this.logInToolStripMenuItem1.Click += new System.EventHandler(this.logInToolStripMenuItem1_Click);
            // 
            // logOutToolStripMenuItem
            // 
            this.logOutToolStripMenuItem.Name = "logOutToolStripMenuItem";
            this.logOutToolStripMenuItem.Size = new System.Drawing.Size(76, 24);
            this.logOutToolStripMenuItem.Text = "&Log Out";
            this.logOutToolStripMenuItem.Click += new System.EventHandler(this.logOutToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Checked = true;
            this.helpToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1;
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(55, 24);
            this.helpToolStripMenuItem.Text = "Help";
            this.helpToolStripMenuItem.Click += new System.EventHandler(this.helpToolStripMenuItem_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel,
            this.toolStripStatusLabel1,
            this.tSSBOOrPOS});
            this.statusStrip.Location = new System.Drawing.Point(0, 386);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1370, 26);
            this.statusStrip.TabIndex = 2;
            this.statusStrip.Text = "StatusStrip";
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(49, 20);
            this.toolStripStatusLabel.Text = "Status";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Margin = new System.Windows.Forms.Padding(400, 3, 0, 2);
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(49, 21);
            this.toolStripStatusLabel1.Text = "Status";
            // 
            // tSSBOOrPOS
            // 
            this.tSSBOOrPOS.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tSSBOOrPOS.IsLink = true;
            this.tSSBOOrPOS.Margin = new System.Windows.Forms.Padding(400, 3, 0, 2);
            this.tSSBOOrPOS.Name = "tSSBOOrPOS";
            this.tSSBOOrPOS.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.tSSBOOrPOS.Size = new System.Drawing.Size(63, 21);
            this.tSSBOOrPOS.Text = "Status";
            this.tSSBOOrPOS.Visible = false;
            this.tSSBOOrPOS.Click += new System.EventHandler(this.tSSBOOrPOS_Click);
            // 
            // ofdDBBackup
            // 
            this.ofdDBBackup.FileName = "ofdDBBackup";
            // 
            // MDIParent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1370, 412);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.menuStrip);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "MDIParent";
            this.Text = "mPOS";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MDIParent_FormClosing);
            this.Load += new System.EventHandler(this.MDIParent_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MDIParent_KeyDown);
            this.Resize += new System.EventHandler(this.MDIParent_Resize);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion


        public System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        public System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.ToolStripMenuItem fileMenu;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolTip toolTip;
        public System.Windows.Forms.ToolStripMenuItem logInToolStripMenuItem1;
        public System.Windows.Forms.ToolStripMenuItem logOutToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem transactionToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem itemSummaryToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem transactionDetailByItemToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem productToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem addNewProductToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem productCategoryToolStripMenuItem1;
        public System.Windows.Forms.ToolStripMenuItem productSubCategoryToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem brandToolStripMenuItem1;
        public System.Windows.Forms.ToolStripMenuItem productListToolStripMenuItem1;
        public System.Windows.Forms.ToolStripMenuItem accountToolStripMenuItem1;
        public System.Windows.Forms.ToolStripMenuItem addNewUserToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem userListToolStripMenuItem1;
        public System.Windows.Forms.ToolStripMenuItem roleManagementToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem1;
        public System.Windows.Forms.ToolStripMenuItem configurationSettingToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem counterToolStripMenuItem1;
        public System.Windows.Forms.ToolStripMenuItem transactionToolStripMenuItem1;
        public System.Windows.Forms.ToolStripMenuItem transactionListToolStripMenuItem1;
        public System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem databaseExportToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem databaseImportToolStripMenuItem;
        public System.Windows.Forms.OpenFileDialog ofdDBBackup;
        public System.Windows.Forms.ToolStripMenuItem topToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem deleteLogToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem addCityToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem transactionSummaryToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem productPriceChangeHistoryListToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem saleToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem saleToolStripMenuItem1;
        public System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem dailySummaryToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem averageMonthlyReportToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem reportsToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem addShopToolStripMenuItem;
        public System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        public System.Windows.Forms.ToolStripStatusLabel tSSBOOrPOS;
        public System.Windows.Forms.ToolStripMenuItem statusToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem localizationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem versiontoolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem assignTicketToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
    }
}



