namespace POS
{
    partial class Setting
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Setting));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.ConfigurationPage = new System.Windows.Forms.TabPage();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.chkCustomSKU = new System.Windows.Forms.CheckBox();
            this.gbIdleLogout = new System.Windows.Forms.GroupBox();
            this.chkIdleDetect = new System.Windows.Forms.CheckBox();
            this.label20 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.txtIdleTime = new System.Windows.Forms.TextBox();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.cboLanguage = new System.Windows.Forms.ComboBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.chkUseStockAutoGenerate = new System.Windows.Forms.CheckBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.txtDefaultSalesRow = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.btnNewCity = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.cboCity = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.lblCity = new System.Windows.Forms.Label();
            this.lbl = new System.Windows.Forms.Label();
            this.txtOpeningHours = new System.Windows.Forms.Label();
            this.txtPhoneNo = new System.Windows.Forms.Label();
            this.txtBranchName = new System.Windows.Forms.Label();
            this.txtShopName = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.cboShopList = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.dpCompanyStartDate = new System.Windows.Forms.DateTimePicker();
            this.PrinterSelection = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnadvanced = new System.Windows.Forms.Button();
            this.pnlAdvanced = new System.Windows.Forms.Panel();
            this.chkBO = new System.Windows.Forms.CheckBox();
            this.chkProductImage = new System.Windows.Forms.CheckBox();
            this.chkUpper = new System.Windows.Forms.CheckBox();
            this.chkTopMost = new System.Windows.Forms.CheckBox();
            this.chkAllowMinimize = new System.Windows.Forms.CheckBox();
            this.chkTicketSale = new System.Windows.Forms.CheckBox();
            this.chkSourceCode = new System.Windows.Forms.CheckBox();
            this.label18 = new System.Windows.Forms.Label();
            this.txtPOSID = new System.Windows.Forms.TextBox();
            this.txtNoOfCopy = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.ptImage = new System.Windows.Forms.PictureBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.txtImagePath = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.txtFooterPage = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cboBarcodePrinter = new System.Windows.Forms.ComboBox();
            this.cboA4Printer = new System.Windows.Forms.ComboBox();
            this.cboSlipPrinter = new System.Windows.Forms.ComboBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.rdoSlipPrinter = new System.Windows.Forms.RadioButton();
            this.rdoA4Printer = new System.Windows.Forms.RadioButton();
            this.panel4 = new System.Windows.Forms.Panel();
            this.lblDefaultPrinter = new System.Windows.Forms.Label();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.ConfigurationPage.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.gbIdleLogout.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.panel5.SuspendLayout();
            this.PrinterSelection.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.pnlAdvanced.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ptImage)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.ConfigurationPage);
            this.tabControl1.Controls.Add(this.PrinterSelection);
            this.tabControl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.tabControl1.Location = new System.Drawing.Point(4, 15);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1060, 664);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // ConfigurationPage
            // 
            this.ConfigurationPage.AutoScroll = true;
            this.ConfigurationPage.BackColor = System.Drawing.SystemColors.Control;
            this.ConfigurationPage.Controls.Add(this.groupBox9);
            this.ConfigurationPage.Controls.Add(this.gbIdleLogout);
            this.ConfigurationPage.Controls.Add(this.groupBox8);
            this.ConfigurationPage.Controls.Add(this.groupBox7);
            this.ConfigurationPage.Controls.Add(this.groupBox4);
            this.ConfigurationPage.Controls.Add(this.groupBox5);
            this.ConfigurationPage.Controls.Add(this.groupBox2);
            this.ConfigurationPage.Location = new System.Drawing.Point(4, 27);
            this.ConfigurationPage.Margin = new System.Windows.Forms.Padding(4);
            this.ConfigurationPage.Name = "ConfigurationPage";
            this.ConfigurationPage.Padding = new System.Windows.Forms.Padding(4);
            this.ConfigurationPage.Size = new System.Drawing.Size(1052, 633);
            this.ConfigurationPage.TabIndex = 0;
            this.ConfigurationPage.Text = "Configuration";
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.chkCustomSKU);
            this.groupBox9.Location = new System.Drawing.Point(782, 355);
            this.groupBox9.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox9.Size = new System.Drawing.Size(206, 76);
            this.groupBox9.TabIndex = 2;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "CustomSKU";
            // 
            // chkCustomSKU
            // 
            this.chkCustomSKU.AutoSize = true;
            this.chkCustomSKU.Location = new System.Drawing.Point(35, 34);
            this.chkCustomSKU.Margin = new System.Windows.Forms.Padding(4);
            this.chkCustomSKU.Name = "chkCustomSKU";
            this.chkCustomSKU.Size = new System.Drawing.Size(149, 22);
            this.chkCustomSKU.TabIndex = 0;
            this.chkCustomSKU.Text = "Use Custom SKU";
            this.chkCustomSKU.UseVisualStyleBackColor = true;
            // 
            // gbIdleLogout
            // 
            this.gbIdleLogout.Controls.Add(this.chkIdleDetect);
            this.gbIdleLogout.Controls.Add(this.label20);
            this.gbIdleLogout.Controls.Add(this.label21);
            this.gbIdleLogout.Controls.Add(this.txtIdleTime);
            this.gbIdleLogout.Location = new System.Drawing.Point(21, 528);
            this.gbIdleLogout.Margin = new System.Windows.Forms.Padding(4);
            this.gbIdleLogout.Name = "gbIdleLogout";
            this.gbIdleLogout.Padding = new System.Windows.Forms.Padding(4);
            this.gbIdleLogout.Size = new System.Drawing.Size(527, 87);
            this.gbIdleLogout.TabIndex = 52;
            this.gbIdleLogout.TabStop = false;
            this.gbIdleLogout.Text = "Idle Logout";
            // 
            // chkIdleDetect
            // 
            this.chkIdleDetect.AutoSize = true;
            this.chkIdleDetect.Location = new System.Drawing.Point(13, 32);
            this.chkIdleDetect.Margin = new System.Windows.Forms.Padding(4);
            this.chkIdleDetect.Name = "chkIdleDetect";
            this.chkIdleDetect.Size = new System.Drawing.Size(174, 22);
            this.chkIdleDetect.TabIndex = 10;
            this.chkIdleDetect.Text = "Detect Application Idle";
            this.chkIdleDetect.UseVisualStyleBackColor = true;
            this.chkIdleDetect.CheckedChanged += new System.EventHandler(this.chkIdleDetect_CheckedChanged);
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(463, 32);
            this.label20.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(40, 18);
            this.label20.TabIndex = 51;
            this.label20.Text = "Mins";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(216, 33);
            this.label21.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(91, 18);
            this.label21.TabIndex = 50;
            this.label21.Text = "Log out after";
            // 
            // txtIdleTime
            // 
            this.txtIdleTime.Location = new System.Drawing.Point(377, 28);
            this.txtIdleTime.Margin = new System.Windows.Forms.Padding(4);
            this.txtIdleTime.Name = "txtIdleTime";
            this.txtIdleTime.Size = new System.Drawing.Size(76, 24);
            this.txtIdleTime.TabIndex = 49;
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.cboLanguage);
            this.groupBox8.Location = new System.Drawing.Point(512, 434);
            this.groupBox8.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox8.Size = new System.Drawing.Size(343, 86);
            this.groupBox8.TabIndex = 8;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Default Language";
            // 
            // cboLanguage
            // 
            this.cboLanguage.FormattingEnabled = true;
            this.cboLanguage.Items.AddRange(new object[] {
            "English",
            "ZawGyi",
            "Myanmar3",
            "Other1",
            "Other2"});
            this.cboLanguage.Location = new System.Drawing.Point(62, 30);
            this.cboLanguage.Margin = new System.Windows.Forms.Padding(4);
            this.cboLanguage.Name = "cboLanguage";
            this.cboLanguage.Size = new System.Drawing.Size(252, 26);
            this.cboLanguage.TabIndex = 7;
            this.cboLanguage.Text = "English";
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.chkUseStockAutoGenerate);
            this.groupBox7.Location = new System.Drawing.Point(21, 355);
            this.groupBox7.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox7.Size = new System.Drawing.Size(363, 76);
            this.groupBox7.TabIndex = 1;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Product Auto Generate Code";
            // 
            // chkUseStockAutoGenerate
            // 
            this.chkUseStockAutoGenerate.AutoSize = true;
            this.chkUseStockAutoGenerate.Location = new System.Drawing.Point(35, 34);
            this.chkUseStockAutoGenerate.Margin = new System.Windows.Forms.Padding(4);
            this.chkUseStockAutoGenerate.Name = "chkUseStockAutoGenerate";
            this.chkUseStockAutoGenerate.Size = new System.Drawing.Size(252, 22);
            this.chkUseStockAutoGenerate.TabIndex = 0;
            this.chkUseStockAutoGenerate.Text = "Use Product Auto Generate Code";
            this.chkUseStockAutoGenerate.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.txtDefaultSalesRow);
            this.groupBox4.Controls.Add(this.label9);
            this.groupBox4.Location = new System.Drawing.Point(438, 355);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox4.Size = new System.Drawing.Size(293, 76);
            this.groupBox4.TabIndex = 4;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Default Row For Top Sales Report";
            // 
            // txtDefaultSalesRow
            // 
            this.txtDefaultSalesRow.Location = new System.Drawing.Point(196, 29);
            this.txtDefaultSalesRow.Margin = new System.Windows.Forms.Padding(4);
            this.txtDefaultSalesRow.Name = "txtDefaultSalesRow";
            this.txtDefaultSalesRow.Size = new System.Drawing.Size(69, 24);
            this.txtDefaultSalesRow.TabIndex = 1;
            this.txtDefaultSalesRow.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNO_KeyPress);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(31, 32);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(94, 18);
            this.label9.TabIndex = 0;
            this.label9.Text = "Row Amount";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.btnNewCity);
            this.groupBox5.Controls.Add(this.label10);
            this.groupBox5.Controls.Add(this.cboCity);
            this.groupBox5.Location = new System.Drawing.Point(21, 434);
            this.groupBox5.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox5.Size = new System.Drawing.Size(460, 86);
            this.groupBox5.TabIndex = 5;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Default City Selection";
            // 
            // btnNewCity
            // 
            this.btnNewCity.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(223)))), ((int)(((byte)(223)))));
            this.btnNewCity.FlatAppearance.BorderSize = 0;
            this.btnNewCity.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnNewCity.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnNewCity.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNewCity.Image = global::POS.Properties.Resources.add_small;
            this.btnNewCity.Location = new System.Drawing.Point(351, 32);
            this.btnNewCity.Margin = new System.Windows.Forms.Padding(4);
            this.btnNewCity.Name = "btnNewCity";
            this.btnNewCity.Size = new System.Drawing.Size(100, 33);
            this.btnNewCity.TabIndex = 1;
            this.btnNewCity.UseVisualStyleBackColor = true;
            this.btnNewCity.Click += new System.EventHandler(this.btnNewCity_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(8, 36);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(77, 18);
            this.label10.TabIndex = 0;
            this.label10.Text = "City Name";
            // 
            // cboCity
            // 
            this.cboCity.FormattingEnabled = true;
            this.cboCity.Location = new System.Drawing.Point(112, 32);
            this.cboCity.Margin = new System.Windows.Forms.Padding(4);
            this.cboCity.Name = "cboCity";
            this.cboCity.Size = new System.Drawing.Size(229, 26);
            this.cboCity.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tableLayoutPanel2);
            this.groupBox2.Location = new System.Drawing.Point(21, 7);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox2.Size = new System.Drawing.Size(993, 340);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Zoo Information";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 36.22047F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 63.77953F));
            this.tableLayoutPanel2.Controls.Add(this.lblCity, 1, 5);
            this.tableLayoutPanel2.Controls.Add(this.lbl, 0, 5);
            this.tableLayoutPanel2.Controls.Add(this.txtOpeningHours, 1, 4);
            this.tableLayoutPanel2.Controls.Add(this.txtPhoneNo, 1, 3);
            this.tableLayoutPanel2.Controls.Add(this.txtBranchName, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.txtShopName, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.label7, 0, 4);
            this.tableLayoutPanel2.Controls.Add(this.label5, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.label4, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.label6, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.label16, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.panel5, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.label11, 0, 6);
            this.tableLayoutPanel2.Controls.Add(this.dpCompanyStartDate, 1, 6);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(31, 32);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 7;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.81481F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 13.33333F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28531F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28531F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28531F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28531F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28816F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(912, 292);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // lblCity
            // 
            this.lblCity.AutoSize = true;
            this.lblCity.Location = new System.Drawing.Point(334, 205);
            this.lblCity.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCity.Name = "lblCity";
            this.lblCity.Size = new System.Drawing.Size(33, 18);
            this.lblCity.TabIndex = 11;
            this.lblCity.Text = "City";
            // 
            // lbl
            // 
            this.lbl.Location = new System.Drawing.Point(4, 205);
            this.lbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl.Name = "lbl";
            this.lbl.Size = new System.Drawing.Size(51, 22);
            this.lbl.TabIndex = 10;
            this.lbl.Text = "City";
            // 
            // txtOpeningHours
            // 
            this.txtOpeningHours.AutoSize = true;
            this.txtOpeningHours.Location = new System.Drawing.Point(334, 164);
            this.txtOpeningHours.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.txtOpeningHours.Name = "txtOpeningHours";
            this.txtOpeningHours.Size = new System.Drawing.Size(108, 18);
            this.txtOpeningHours.TabIndex = 46;
            this.txtOpeningHours.Text = "Opening Hours";
            // 
            // txtPhoneNo
            // 
            this.txtPhoneNo.AutoSize = true;
            this.txtPhoneNo.Location = new System.Drawing.Point(334, 123);
            this.txtPhoneNo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.txtPhoneNo.Name = "txtPhoneNo";
            this.txtPhoneNo.Size = new System.Drawing.Size(51, 18);
            this.txtPhoneNo.TabIndex = 45;
            this.txtPhoneNo.Text = "Phone";
            // 
            // txtBranchName
            // 
            this.txtBranchName.AutoSize = true;
            this.txtBranchName.Location = new System.Drawing.Point(334, 82);
            this.txtBranchName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.txtBranchName.Name = "txtBranchName";
            this.txtBranchName.Size = new System.Drawing.Size(178, 18);
            this.txtBranchName.TabIndex = 44;
            this.txtBranchName.Text = "Branch Name Or Address";
            // 
            // txtShopName
            // 
            this.txtShopName.AutoSize = true;
            this.txtShopName.Location = new System.Drawing.Point(334, 43);
            this.txtShopName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.txtShopName.Name = "txtShopName";
            this.txtShopName.Size = new System.Drawing.Size(79, 18);
            this.txtShopName.TabIndex = 43;
            this.txtShopName.Text = "Zoo Name";
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(4, 164);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(105, 22);
            this.label7.TabIndex = 6;
            this.label7.Text = "Opening Hours";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(4, 82);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(237, 22);
            this.label5.TabIndex = 2;
            this.label5.Text = "Branch Name Or Address";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(4, 43);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(156, 22);
            this.label4.TabIndex = 0;
            this.label4.Text = "Zoo Name        ";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(4, 123);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(72, 22);
            this.label6.TabIndex = 4;
            this.label6.Text = "Phone";
            // 
            // label16
            // 
            this.label16.Location = new System.Drawing.Point(4, 0);
            this.label16.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(171, 22);
            this.label16.TabIndex = 42;
            this.label16.Text = "Default Zoo   ";
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.cboShopList);
            this.panel5.Location = new System.Drawing.Point(334, 4);
            this.panel5.Margin = new System.Windows.Forms.Padding(4);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(574, 35);
            this.panel5.TabIndex = 47;
            // 
            // cboShopList
            // 
            this.cboShopList.FormattingEnabled = true;
            this.cboShopList.Location = new System.Drawing.Point(0, 5);
            this.cboShopList.Margin = new System.Windows.Forms.Padding(4);
            this.cboShopList.Name = "cboShopList";
            this.cboShopList.Size = new System.Drawing.Size(293, 26);
            this.cboShopList.TabIndex = 41;
            this.cboShopList.SelectedIndexChanged += new System.EventHandler(this.cboShopList_SelectedIndexChanged);
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(4, 246);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(188, 22);
            this.label11.TabIndex = 8;
            this.label11.Text = "Company Start Date";
            // 
            // dpCompanyStartDate
            // 
            this.dpCompanyStartDate.Location = new System.Drawing.Point(334, 250);
            this.dpCompanyStartDate.Margin = new System.Windows.Forms.Padding(4);
            this.dpCompanyStartDate.Name = "dpCompanyStartDate";
            this.dpCompanyStartDate.Size = new System.Drawing.Size(265, 24);
            this.dpCompanyStartDate.TabIndex = 9;
            // 
            // PrinterSelection
            // 
            this.PrinterSelection.BackColor = System.Drawing.SystemColors.Control;
            this.PrinterSelection.Controls.Add(this.groupBox1);
            this.PrinterSelection.Location = new System.Drawing.Point(4, 27);
            this.PrinterSelection.Margin = new System.Windows.Forms.Padding(4);
            this.PrinterSelection.Name = "PrinterSelection";
            this.PrinterSelection.Padding = new System.Windows.Forms.Padding(4);
            this.PrinterSelection.Size = new System.Drawing.Size(1052, 633);
            this.PrinterSelection.TabIndex = 1;
            this.PrinterSelection.Text = "Printer Selection";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnadvanced);
            this.groupBox1.Controls.Add(this.pnlAdvanced);
            this.groupBox1.Controls.Add(this.txtNoOfCopy);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.ptImage);
            this.groupBox1.Controls.Add(this.btnBrowse);
            this.groupBox1.Controls.Add(this.txtImagePath);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.txtFooterPage);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.tableLayoutPanel1);
            this.groupBox1.Location = new System.Drawing.Point(8, 7);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(1007, 692);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // btnadvanced
            // 
            this.btnadvanced.Location = new System.Drawing.Point(8, 476);
            this.btnadvanced.Margin = new System.Windows.Forms.Padding(4);
            this.btnadvanced.Name = "btnadvanced";
            this.btnadvanced.Size = new System.Drawing.Size(135, 33);
            this.btnadvanced.TabIndex = 0;
            this.btnadvanced.Text = "Advanced";
            this.btnadvanced.UseVisualStyleBackColor = true;
            this.btnadvanced.Click += new System.EventHandler(this.btnadvanced_Click);
            // 
            // pnlAdvanced
            // 
            this.pnlAdvanced.AutoScroll = true;
            this.pnlAdvanced.BackColor = System.Drawing.SystemColors.Control;
            this.pnlAdvanced.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlAdvanced.Controls.Add(this.chkBO);
            this.pnlAdvanced.Controls.Add(this.chkProductImage);
            this.pnlAdvanced.Controls.Add(this.chkUpper);
            this.pnlAdvanced.Controls.Add(this.chkTopMost);
            this.pnlAdvanced.Controls.Add(this.chkAllowMinimize);
            this.pnlAdvanced.Controls.Add(this.chkTicketSale);
            this.pnlAdvanced.Controls.Add(this.chkSourceCode);
            this.pnlAdvanced.Controls.Add(this.label18);
            this.pnlAdvanced.Controls.Add(this.txtPOSID);
            this.pnlAdvanced.Location = new System.Drawing.Point(8, 517);
            this.pnlAdvanced.Margin = new System.Windows.Forms.Padding(4);
            this.pnlAdvanced.Name = "pnlAdvanced";
            this.pnlAdvanced.Size = new System.Drawing.Size(962, 101);
            this.pnlAdvanced.TabIndex = 40;
            this.pnlAdvanced.Visible = false;
            // 
            // chkBO
            // 
            this.chkBO.AutoSize = true;
            this.chkBO.Location = new System.Drawing.Point(411, 25);
            this.chkBO.Margin = new System.Windows.Forms.Padding(4);
            this.chkBO.Name = "chkBO";
            this.chkBO.Size = new System.Drawing.Size(114, 22);
            this.chkBO.TabIndex = 53;
            this.chkBO.Text = "IsBackOffice";
            this.chkBO.UseVisualStyleBackColor = true;
            // 
            // chkProductImage
            // 
            this.chkProductImage.AutoSize = true;
            this.chkProductImage.Location = new System.Drawing.Point(280, 70);
            this.chkProductImage.Margin = new System.Windows.Forms.Padding(4);
            this.chkProductImage.Name = "chkProductImage";
            this.chkProductImage.Size = new System.Drawing.Size(257, 22);
            this.chkProductImage.TabIndex = 52;
            this.chkProductImage.Text = "Show ProductImage in A4 Reports";
            this.chkProductImage.UseVisualStyleBackColor = true;
            // 
            // chkUpper
            // 
            this.chkUpper.AutoSize = true;
            this.chkUpper.Location = new System.Drawing.Point(30, 70);
            this.chkUpper.Margin = new System.Windows.Forms.Padding(4);
            this.chkUpper.Name = "chkUpper";
            this.chkUpper.Size = new System.Drawing.Size(201, 22);
            this.chkUpper.TabIndex = 51;
            this.chkUpper.Text = "UpperCase ProductName";
            this.chkUpper.UseVisualStyleBackColor = true;
            // 
            // chkTopMost
            // 
            this.chkTopMost.AutoSize = true;
            this.chkTopMost.Location = new System.Drawing.Point(821, 27);
            this.chkTopMost.Margin = new System.Windows.Forms.Padding(4);
            this.chkTopMost.Name = "chkTopMost";
            this.chkTopMost.Size = new System.Drawing.Size(94, 22);
            this.chkTopMost.TabIndex = 50;
            this.chkTopMost.Text = "Top Most";
            this.chkTopMost.UseVisualStyleBackColor = true;
            // 
            // chkAllowMinimize
            // 
            this.chkAllowMinimize.AutoSize = true;
            this.chkAllowMinimize.Location = new System.Drawing.Point(673, 27);
            this.chkAllowMinimize.Margin = new System.Windows.Forms.Padding(4);
            this.chkAllowMinimize.Name = "chkAllowMinimize";
            this.chkAllowMinimize.Size = new System.Drawing.Size(128, 22);
            this.chkAllowMinimize.TabIndex = 49;
            this.chkAllowMinimize.Text = "Allow Minimize";
            this.chkAllowMinimize.UseVisualStyleBackColor = true;
            // 
            // chkTicketSale
            // 
            this.chkTicketSale.AutoSize = true;
            this.chkTicketSale.Location = new System.Drawing.Point(547, 27);
            this.chkTicketSale.Margin = new System.Windows.Forms.Padding(4);
            this.chkTicketSale.Name = "chkTicketSale";
            this.chkTicketSale.Size = new System.Drawing.Size(103, 22);
            this.chkTicketSale.TabIndex = 48;
            this.chkTicketSale.Text = "Ticket Sale";
            this.chkTicketSale.UseVisualStyleBackColor = true;
            // 
            // chkSourceCode
            // 
            this.chkSourceCode.AutoSize = true;
            this.chkSourceCode.Location = new System.Drawing.Point(32, 25);
            this.chkSourceCode.Margin = new System.Windows.Forms.Padding(4);
            this.chkSourceCode.Name = "chkSourceCode";
            this.chkSourceCode.Size = new System.Drawing.Size(125, 22);
            this.chkSourceCode.TabIndex = 43;
            this.chkSourceCode.Text = "IsSourceCode";
            this.chkSourceCode.UseVisualStyleBackColor = true;
            this.chkSourceCode.CheckedChanged += new System.EventHandler(this.chkSourceCode_CheckedChanged);
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(211, 27);
            this.label18.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(62, 18);
            this.label18.TabIndex = 42;
            this.label18.Text = "POS_ID";
            // 
            // txtPOSID
            // 
            this.txtPOSID.Location = new System.Drawing.Point(283, 23);
            this.txtPOSID.Margin = new System.Windows.Forms.Padding(4);
            this.txtPOSID.Name = "txtPOSID";
            this.txtPOSID.ReadOnly = true;
            this.txtPOSID.Size = new System.Drawing.Size(115, 24);
            this.txtPOSID.TabIndex = 1;
            // 
            // txtNoOfCopy
            // 
            this.txtNoOfCopy.Location = new System.Drawing.Point(364, 277);
            this.txtNoOfCopy.Margin = new System.Windows.Forms.Padding(4);
            this.txtNoOfCopy.Name = "txtNoOfCopy";
            this.txtNoOfCopy.Size = new System.Drawing.Size(99, 24);
            this.txtNoOfCopy.TabIndex = 39;
            this.txtNoOfCopy.Text = "1";
            this.txtNoOfCopy.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNO_KeyPress);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(35, 277);
            this.label15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(137, 18);
            this.label15.TabIndex = 38;
            this.label15.Text = "Number of Copies :";
            // 
            // ptImage
            // 
            this.ptImage.BackColor = System.Drawing.Color.Transparent;
            this.ptImage.Location = new System.Drawing.Point(363, 362);
            this.ptImage.Margin = new System.Windows.Forms.Padding(4);
            this.ptImage.Name = "ptImage";
            this.ptImage.Size = new System.Drawing.Size(148, 122);
            this.ptImage.TabIndex = 37;
            this.ptImage.TabStop = false;
            // 
            // btnBrowse
            // 
            this.btnBrowse.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(223)))), ((int)(((byte)(223)))));
            this.btnBrowse.FlatAppearance.BorderSize = 0;
            this.btnBrowse.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(223)))), ((int)(((byte)(223)))));
            this.btnBrowse.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(223)))), ((int)(((byte)(223)))));
            this.btnBrowse.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBrowse.Image = global::POS.Properties.Resources.browse;
            this.btnBrowse.Location = new System.Drawing.Point(632, 319);
            this.btnBrowse.Margin = new System.Windows.Forms.Padding(4);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(116, 33);
            this.btnBrowse.TabIndex = 35;
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // txtImagePath
            // 
            this.txtImagePath.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtImagePath.Location = new System.Drawing.Point(363, 319);
            this.txtImagePath.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.txtImagePath.Name = "txtImagePath";
            this.txtImagePath.ReadOnly = true;
            this.txtImagePath.Size = new System.Drawing.Size(248, 24);
            this.txtImagePath.TabIndex = 34;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.label14.Location = new System.Drawing.Point(33, 319);
            this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(50, 18);
            this.label14.TabIndex = 36;
            this.label14.Text = "Logo :";
            // 
            // txtFooterPage
            // 
            this.txtFooterPage.Location = new System.Drawing.Point(364, 204);
            this.txtFooterPage.Margin = new System.Windows.Forms.Padding(4);
            this.txtFooterPage.Multiline = true;
            this.txtFooterPage.Name = "txtFooterPage";
            this.txtFooterPage.Size = new System.Drawing.Size(437, 57);
            this.txtFooterPage.TabIndex = 2;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(35, 204);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(110, 18);
            this.label12.TabIndex = 1;
            this.label12.Text = "Footer Page    :";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 36.22F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 63.78F));
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.cboBarcodePrinter, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.cboA4Printer, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.cboSlipPrinter, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.panel3, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.panel4, 0, 3);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(31, 27);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.00062F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.00062F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.00062F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 24.99813F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(912, 165);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 82);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 18);
            this.label3.TabIndex = 4;
            this.label3.Text = "Slip Printer";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(111, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "Barcode Printer";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 41);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 18);
            this.label2.TabIndex = 2;
            this.label2.Text = "A4 Printer";
            // 
            // cboBarcodePrinter
            // 
            this.cboBarcodePrinter.FormattingEnabled = true;
            this.cboBarcodePrinter.Location = new System.Drawing.Point(334, 4);
            this.cboBarcodePrinter.Margin = new System.Windows.Forms.Padding(4);
            this.cboBarcodePrinter.Name = "cboBarcodePrinter";
            this.cboBarcodePrinter.Size = new System.Drawing.Size(441, 26);
            this.cboBarcodePrinter.TabIndex = 1;
            // 
            // cboA4Printer
            // 
            this.cboA4Printer.FormattingEnabled = true;
            this.cboA4Printer.Location = new System.Drawing.Point(334, 45);
            this.cboA4Printer.Margin = new System.Windows.Forms.Padding(4);
            this.cboA4Printer.Name = "cboA4Printer";
            this.cboA4Printer.Size = new System.Drawing.Size(441, 26);
            this.cboA4Printer.TabIndex = 3;
            // 
            // cboSlipPrinter
            // 
            this.cboSlipPrinter.FormattingEnabled = true;
            this.cboSlipPrinter.Location = new System.Drawing.Point(334, 86);
            this.cboSlipPrinter.Margin = new System.Windows.Forms.Padding(4);
            this.cboSlipPrinter.Name = "cboSlipPrinter";
            this.cboSlipPrinter.Size = new System.Drawing.Size(441, 26);
            this.cboSlipPrinter.TabIndex = 5;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.rdoSlipPrinter);
            this.panel3.Controls.Add(this.rdoA4Printer);
            this.panel3.Location = new System.Drawing.Point(334, 127);
            this.panel3.Margin = new System.Windows.Forms.Padding(4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(443, 30);
            this.panel3.TabIndex = 7;
            // 
            // rdoSlipPrinter
            // 
            this.rdoSlipPrinter.AutoSize = true;
            this.rdoSlipPrinter.Checked = true;
            this.rdoSlipPrinter.Location = new System.Drawing.Point(4, 4);
            this.rdoSlipPrinter.Margin = new System.Windows.Forms.Padding(4);
            this.rdoSlipPrinter.Name = "rdoSlipPrinter";
            this.rdoSlipPrinter.Size = new System.Drawing.Size(100, 22);
            this.rdoSlipPrinter.TabIndex = 0;
            this.rdoSlipPrinter.TabStop = true;
            this.rdoSlipPrinter.Text = "Slip Printer";
            this.rdoSlipPrinter.UseVisualStyleBackColor = true;
            // 
            // rdoA4Printer
            // 
            this.rdoA4Printer.AutoSize = true;
            this.rdoA4Printer.Location = new System.Drawing.Point(161, 4);
            this.rdoA4Printer.Margin = new System.Windows.Forms.Padding(4);
            this.rdoA4Printer.Name = "rdoA4Printer";
            this.rdoA4Printer.Size = new System.Drawing.Size(93, 22);
            this.rdoA4Printer.TabIndex = 1;
            this.rdoA4Printer.Text = "A4 Printer";
            this.rdoA4Printer.UseVisualStyleBackColor = true;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.lblDefaultPrinter);
            this.panel4.Location = new System.Drawing.Point(4, 127);
            this.panel4.Margin = new System.Windows.Forms.Padding(4);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(139, 30);
            this.panel4.TabIndex = 6;
            // 
            // lblDefaultPrinter
            // 
            this.lblDefaultPrinter.AutoSize = true;
            this.lblDefaultPrinter.Location = new System.Drawing.Point(0, 6);
            this.lblDefaultPrinter.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDefaultPrinter.Name = "lblDefaultPrinter";
            this.lblDefaultPrinter.Size = new System.Drawing.Size(101, 18);
            this.lblDefaultPrinter.TabIndex = 0;
            this.lblDefaultPrinter.Text = "Default Printer";
            // 
            // btnSubmit
            // 
            this.btnSubmit.FlatAppearance.BorderSize = 0;
            this.btnSubmit.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnSubmit.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnSubmit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSubmit.Image = global::POS.Properties.Resources.save_big;
            this.btnSubmit.Location = new System.Drawing.Point(869, 687);
            this.btnSubmit.Margin = new System.Windows.Forms.Padding(4);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(153, 52);
            this.btnSubmit.TabIndex = 1;
            this.btnSubmit.UseVisualStyleBackColor = true;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // Setting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1068, 759);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.btnSubmit);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Setting";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Setting";
            this.Load += new System.EventHandler(this.Setting_Load);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Setting_MouseMove);
            this.tabControl1.ResumeLayout(false);
            this.ConfigurationPage.ResumeLayout(false);
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            this.gbIdleLogout.ResumeLayout(false);
            this.gbIdleLogout.PerformLayout();
            this.groupBox8.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.PrinterSelection.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.pnlAdvanced.ResumeLayout(false);
            this.pnlAdvanced.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ptImage)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage ConfigurationPage;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.CheckBox chkUseStockAutoGenerate;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox txtDefaultSalesRow;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button btnNewCity;
        private System.Windows.Forms.ComboBox cboCity;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DateTimePicker dpCompanyStartDate;
        private System.Windows.Forms.TabPage PrinterSelection;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtFooterPage;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboBarcodePrinter;
        private System.Windows.Forms.ComboBox cboA4Printer;
        private System.Windows.Forms.ComboBox cboSlipPrinter;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.RadioButton rdoSlipPrinter;
        private System.Windows.Forms.RadioButton rdoA4Printer;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label lblDefaultPrinter;
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.PictureBox ptImage;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.TextBox txtImagePath;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txtNoOfCopy;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label txtOpeningHours;
        private System.Windows.Forms.Label txtPhoneNo;
        private System.Windows.Forms.Label txtBranchName;
        private System.Windows.Forms.Label txtShopName;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.ComboBox cboShopList;
        private System.Windows.Forms.Label lblCity;
        private System.Windows.Forms.Label lbl;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.ComboBox cboLanguage;
        private System.Windows.Forms.Button btnadvanced;
        private System.Windows.Forms.Panel pnlAdvanced;
        private System.Windows.Forms.CheckBox chkSourceCode;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox txtPOSID;
        private System.Windows.Forms.GroupBox gbIdleLogout;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.TextBox txtIdleTime;
        private System.Windows.Forms.CheckBox chkIdleDetect;
        private System.Windows.Forms.CheckBox chkTicketSale;
        private System.Windows.Forms.CheckBox chkAllowMinimize;
        private System.Windows.Forms.CheckBox chkTopMost;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.CheckBox chkCustomSKU;
        private System.Windows.Forms.CheckBox chkUpper;
        private System.Windows.Forms.CheckBox chkProductImage;
        private System.Windows.Forms.CheckBox chkBO;
    }
}