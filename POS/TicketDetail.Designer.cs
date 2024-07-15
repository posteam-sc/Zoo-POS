namespace POS
{
    partial class TicketDetail
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TicketDetail));
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cboTiType = new System.Windows.Forms.ComboBox();
            this.chkTiType = new System.Windows.Forms.CheckBox();
            this.chkCounter = new System.Windows.Forms.CheckBox();
            this.chkCashier = new System.Windows.Forms.CheckBox();
            this.cboCounter = new System.Windows.Forms.ComboBox();
            this.cboCashier = new System.Windows.Forms.ComboBox();
            this.gbTransactionList = new System.Windows.Forms.GroupBox();
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.label3 = new System.Windows.Forms.Label();
            this.lblPeriod = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cboTTP = new System.Windows.Forms.ComboBox();
            this.cboTP = new System.Windows.Forms.ComboBox();
            this.cboFP = new System.Windows.Forms.ComboBox();
            this.cboFTP = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.chkTime = new System.Windows.Forms.CheckBox();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.groupBox3.SuspendLayout();
            this.gbTransactionList.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.cboTiType);
            this.groupBox3.Controls.Add(this.chkTiType);
            this.groupBox3.Controls.Add(this.chkCounter);
            this.groupBox3.Controls.Add(this.chkCashier);
            this.groupBox3.Controls.Add(this.cboCounter);
            this.groupBox3.Controls.Add(this.cboCashier);
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.groupBox3.Location = new System.Drawing.Point(16, 6);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox3.Size = new System.Drawing.Size(1049, 107);
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Filter Types";
            // 
            // cboTiType
            // 
            this.cboTiType.Enabled = false;
            this.cboTiType.FormattingEnabled = true;
            this.cboTiType.Location = new System.Drawing.Point(785, 65);
            this.cboTiType.Margin = new System.Windows.Forms.Padding(4);
            this.cboTiType.Name = "cboTiType";
            this.cboTiType.Size = new System.Drawing.Size(200, 26);
            this.cboTiType.TabIndex = 8;
            this.cboTiType.SelectedValueChanged += new System.EventHandler(this.cboTiType_SelectedValueChanged);
            // 
            // chkTiType
            // 
            this.chkTiType.AutoSize = true;
            this.chkTiType.Location = new System.Drawing.Point(785, 24);
            this.chkTiType.Margin = new System.Windows.Forms.Padding(4);
            this.chkTiType.Name = "chkTiType";
            this.chkTiType.Size = new System.Drawing.Size(83, 22);
            this.chkTiType.TabIndex = 6;
            this.chkTiType.Text = "By Type";
            this.chkTiType.UseVisualStyleBackColor = true;
            this.chkTiType.CheckedChanged += new System.EventHandler(this.chkTiType_CheckedChanged);
            // 
            // chkCounter
            // 
            this.chkCounter.AutoSize = true;
            this.chkCounter.Location = new System.Drawing.Point(480, 24);
            this.chkCounter.Margin = new System.Windows.Forms.Padding(4);
            this.chkCounter.Name = "chkCounter";
            this.chkCounter.Size = new System.Drawing.Size(104, 22);
            this.chkCounter.TabIndex = 3;
            this.chkCounter.Text = "By Counter";
            this.chkCounter.UseVisualStyleBackColor = true;
            this.chkCounter.CheckedChanged += new System.EventHandler(this.chkCounter_CheckedChanged);
            // 
            // chkCashier
            // 
            this.chkCashier.AutoSize = true;
            this.chkCashier.Location = new System.Drawing.Point(114, 24);
            this.chkCashier.Margin = new System.Windows.Forms.Padding(4);
            this.chkCashier.Name = "chkCashier";
            this.chkCashier.Size = new System.Drawing.Size(102, 22);
            this.chkCashier.TabIndex = 0;
            this.chkCashier.Text = "By Cashier";
            this.chkCashier.UseVisualStyleBackColor = true;
            this.chkCashier.CheckedChanged += new System.EventHandler(this.chkCashier_CheckedChanged);
            // 
            // cboCounter
            // 
            this.cboCounter.Enabled = false;
            this.cboCounter.FormattingEnabled = true;
            this.cboCounter.Location = new System.Drawing.Point(480, 65);
            this.cboCounter.Margin = new System.Windows.Forms.Padding(4);
            this.cboCounter.Name = "cboCounter";
            this.cboCounter.Size = new System.Drawing.Size(185, 26);
            this.cboCounter.TabIndex = 5;
            this.cboCounter.SelectedIndexChanged += new System.EventHandler(this.cboCounter_SelectedIndexChanged);
            this.cboCounter.SelectedValueChanged += new System.EventHandler(this.cboCounter_SelectedValueChanged);
            // 
            // cboCashier
            // 
            this.cboCashier.Enabled = false;
            this.cboCashier.FormattingEnabled = true;
            this.cboCashier.Location = new System.Drawing.Point(114, 63);
            this.cboCashier.Margin = new System.Windows.Forms.Padding(4);
            this.cboCashier.Name = "cboCashier";
            this.cboCashier.Size = new System.Drawing.Size(275, 26);
            this.cboCashier.TabIndex = 2;
            this.cboCashier.SelectedIndexChanged += new System.EventHandler(this.cboCashier_SelectedIndexChanged);
            this.cboCashier.SelectedValueChanged += new System.EventHandler(this.cboCashier_SelectedValueChanged);
            // 
            // gbTransactionList
            // 
            this.gbTransactionList.Controls.Add(this.reportViewer1);
            this.gbTransactionList.Controls.Add(this.label3);
            this.gbTransactionList.Controls.Add(this.lblPeriod);
            this.gbTransactionList.Location = new System.Drawing.Point(16, 248);
            this.gbTransactionList.Margin = new System.Windows.Forms.Padding(4);
            this.gbTransactionList.Name = "gbTransactionList";
            this.gbTransactionList.Padding = new System.Windows.Forms.Padding(4);
            this.gbTransactionList.Size = new System.Drawing.Size(1049, 447);
            this.gbTransactionList.TabIndex = 8;
            this.gbTransactionList.TabStop = false;
            // 
            // reportViewer1
            // 
            this.reportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "POS.Reports.TransactionReport.rdlc";
            this.reportViewer1.Location = new System.Drawing.Point(4, 19);
            this.reportViewer1.Margin = new System.Windows.Forms.Padding(4);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.Size = new System.Drawing.Size(1041, 424);
            this.reportViewer1.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(477, 20);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 16);
            this.label3.TabIndex = 4;
            this.label3.Text = "Period";
            // 
            // lblPeriod
            // 
            this.lblPeriod.AutoSize = true;
            this.lblPeriod.Location = new System.Drawing.Point(544, 20);
            this.lblPeriod.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPeriod.Name = "lblPeriod";
            this.lblPeriod.Size = new System.Drawing.Size(11, 16);
            this.lblPeriod.TabIndex = 1;
            this.lblPeriod.Text = "-";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cboTTP);
            this.groupBox2.Controls.Add(this.cboTP);
            this.groupBox2.Controls.Add(this.cboFP);
            this.groupBox2.Controls.Add(this.cboFTP);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.chkTime);
            this.groupBox2.Controls.Add(this.dtpTo);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.dtpFrom);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.groupBox2.Location = new System.Drawing.Point(16, 123);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox2.Size = new System.Drawing.Size(725, 117);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Filter By";
            // 
            // cboTTP
            // 
            this.cboTTP.Enabled = false;
            this.cboTTP.FormattingEnabled = true;
            this.cboTTP.Location = new System.Drawing.Point(436, 76);
            this.cboTTP.Margin = new System.Windows.Forms.Padding(4);
            this.cboTTP.Name = "cboTTP";
            this.cboTTP.Size = new System.Drawing.Size(57, 26);
            this.cboTTP.TabIndex = 11;
            this.cboTTP.SelectedIndexChanged += new System.EventHandler(this.cboTTP_SelectedIndexChanged);
            // 
            // cboTP
            // 
            this.cboTP.Enabled = false;
            this.cboTP.FormattingEnabled = true;
            this.cboTP.Location = new System.Drawing.Point(498, 76);
            this.cboTP.Margin = new System.Windows.Forms.Padding(4);
            this.cboTP.Name = "cboTP";
            this.cboTP.Size = new System.Drawing.Size(57, 26);
            this.cboTP.TabIndex = 10;
            this.cboTP.SelectedIndexChanged += new System.EventHandler(this.cboTP_SelectedIndexChanged);
            // 
            // cboFP
            // 
            this.cboFP.Enabled = false;
            this.cboFP.FormattingEnabled = true;
            this.cboFP.Location = new System.Drawing.Point(322, 76);
            this.cboFP.Margin = new System.Windows.Forms.Padding(4);
            this.cboFP.Name = "cboFP";
            this.cboFP.Size = new System.Drawing.Size(57, 26);
            this.cboFP.TabIndex = 9;
            this.cboFP.SelectedIndexChanged += new System.EventHandler(this.cboFP_SelectedIndexChanged);
            // 
            // cboFTP
            // 
            this.cboFTP.Enabled = false;
            this.cboFTP.FormattingEnabled = true;
            this.cboFTP.Location = new System.Drawing.Point(260, 76);
            this.cboFTP.Margin = new System.Windows.Forms.Padding(4);
            this.cboFTP.Name = "cboFTP";
            this.cboFTP.Size = new System.Drawing.Size(57, 26);
            this.cboFTP.TabIndex = 8;
            this.cboFTP.SelectedIndexChanged += new System.EventHandler(this.cboFTP_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Enabled = false;
            this.label4.Location = new System.Drawing.Point(402, 79);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(26, 18);
            this.label4.TabIndex = 7;
            this.label4.Text = "To";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Enabled = false;
            this.label5.Location = new System.Drawing.Point(207, 79);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(44, 18);
            this.label5.TabIndex = 6;
            this.label5.Text = "From";
            // 
            // chkTime
            // 
            this.chkTime.AutoSize = true;
            this.chkTime.Location = new System.Drawing.Point(117, 78);
            this.chkTime.Margin = new System.Windows.Forms.Padding(4);
            this.chkTime.Name = "chkTime";
            this.chkTime.Size = new System.Drawing.Size(84, 22);
            this.chkTime.TabIndex = 5;
            this.chkTime.Text = "By Time";
            this.chkTime.UseVisualStyleBackColor = true;
            this.chkTime.CheckedChanged += new System.EventHandler(this.chkTime_CheckedChanged);
            // 
            // dtpTo
            // 
            this.dtpTo.CustomFormat = "dd/MMM/yyyy";
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTo.Location = new System.Drawing.Point(399, 24);
            this.dtpTo.Margin = new System.Windows.Forms.Padding(4);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(156, 24);
            this.dtpTo.TabIndex = 3;
            this.dtpTo.ValueChanged += new System.EventHandler(this.dtpTo_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(359, 26);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(26, 18);
            this.label2.TabIndex = 2;
            this.label2.Text = "To";
            // 
            // dtpFrom
            // 
            this.dtpFrom.CustomFormat = "dd/MMM/yyyy";
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFrom.Location = new System.Drawing.Point(170, 24);
            this.dtpFrom.Margin = new System.Windows.Forms.Padding(4);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(156, 24);
            this.dtpFrom.TabIndex = 1;
            this.dtpFrom.ValueChanged += new System.EventHandler(this.dtpFrom_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(114, 26);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "From";
            // 
            // btnSearch
            // 
            this.btnSearch.BackgroundImage = global::POS.Properties.Resources.search_big;
            this.btnSearch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSearch.Location = new System.Drawing.Point(923, 171);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(142, 52);
            this.btnSearch.TabIndex = 21;
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // TicketDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1082, 708);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.gbTransactionList);
            this.Controls.Add(this.groupBox2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "TicketDetail";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ticket Detail Report";
            this.Load += new System.EventHandler(this.TicketDetail_Load);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.gbTransactionList.ResumeLayout(false);
            this.gbTransactionList.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox cboTiType;
        private System.Windows.Forms.CheckBox chkTiType;
        private System.Windows.Forms.CheckBox chkCounter;
        private System.Windows.Forms.CheckBox chkCashier;
        private System.Windows.Forms.ComboBox cboCounter;
        private System.Windows.Forms.ComboBox cboCashier;
        private System.Windows.Forms.GroupBox gbTransactionList;
        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblPeriod;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox cboTTP;
        private System.Windows.Forms.ComboBox cboTP;
        private System.Windows.Forms.ComboBox cboFP;
        private System.Windows.Forms.ComboBox cboFTP;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox chkTime;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSearch;
    }
}