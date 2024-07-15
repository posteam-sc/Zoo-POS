namespace POS
{
    partial class ItemList
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ItemList));
            this.dgvItemList = new System.Windows.Forms.DataGridView();
            this.colId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colProductCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBarcode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colProductName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUnitPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDisPercent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEdit = new System.Windows.Forms.DataGridViewLinkColumn();
            this.ColDelete = new System.Windows.Forms.DataGridViewLinkColumn();
            this.colPriceChange = new System.Windows.Forms.DataGridViewLinkColumn();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cboMainCategory = new System.Windows.Forms.ComboBox();
            this.cboSubCategory = new System.Windows.Forms.ComboBox();
            this.cboBrand = new System.Windows.Forms.ComboBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtBarcode = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rdAll = new System.Windows.Forms.RadioButton();
            this.rdNonConsignment = new System.Windows.Forms.RadioButton();
            this.rdConsignment = new System.Windows.Forms.RadioButton();
            this.btnPBC = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItemList)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvItemList
            // 
            this.dgvItemList.AllowUserToAddRows = false;
            this.dgvItemList.BackgroundColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvItemList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvItemList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvItemList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colId,
            this.ColNo,
            this.colProductCode,
            this.colBarcode,
            this.colProductName,
            this.colUnitPrice,
            this.colDisPercent,
            this.colEdit,
            this.ColDelete,
            this.colPriceChange});
            this.dgvItemList.Location = new System.Drawing.Point(10, 198);
            this.dgvItemList.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dgvItemList.Name = "dgvItemList";
            this.dgvItemList.RowHeadersVisible = false;
            this.dgvItemList.RowHeadersWidth = 51;
            this.dgvItemList.Size = new System.Drawing.Size(1315, 484);
            this.dgvItemList.TabIndex = 3;
            this.dgvItemList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvItemList_CellClick);
            this.dgvItemList.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvItemList_DataBindingComplete);
            // 
            // colId
            // 
            this.colId.HeaderText = "Id";
            this.colId.MinimumWidth = 6;
            this.colId.Name = "colId";
            this.colId.Visible = false;
            this.colId.Width = 125;
            // 
            // ColNo
            // 
            this.ColNo.HeaderText = "No.";
            this.ColNo.MinimumWidth = 6;
            this.ColNo.Name = "ColNo";
            this.ColNo.Width = 40;
            // 
            // colProductCode
            // 
            this.colProductCode.HeaderText = "Product Code";
            this.colProductCode.MinimumWidth = 6;
            this.colProductCode.Name = "colProductCode";
            this.colProductCode.Width = 125;
            // 
            // colBarcode
            // 
            this.colBarcode.HeaderText = "Barcode";
            this.colBarcode.MinimumWidth = 6;
            this.colBarcode.Name = "colBarcode";
            this.colBarcode.Width = 125;
            // 
            // colProductName
            // 
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.colProductName.DefaultCellStyle = dataGridViewCellStyle2;
            this.colProductName.HeaderText = "Product Name";
            this.colProductName.MinimumWidth = 6;
            this.colProductName.Name = "colProductName";
            this.colProductName.Width = 250;
            // 
            // colUnitPrice
            // 
            this.colUnitPrice.HeaderText = "Unit Price";
            this.colUnitPrice.MinimumWidth = 6;
            this.colUnitPrice.Name = "colUnitPrice";
            this.colUnitPrice.Width = 150;
            // 
            // colDisPercent
            // 
            this.colDisPercent.HeaderText = "Discount Percent";
            this.colDisPercent.MinimumWidth = 6;
            this.colDisPercent.Name = "colDisPercent";
            this.colDisPercent.Width = 120;
            // 
            // colEdit
            // 
            this.colEdit.HeaderText = "";
            this.colEdit.MinimumWidth = 6;
            this.colEdit.Name = "colEdit";
            this.colEdit.Text = "Edit";
            this.colEdit.UseColumnTextForLinkValue = true;
            this.colEdit.Width = 60;
            // 
            // ColDelete
            // 
            this.ColDelete.HeaderText = "";
            this.ColDelete.MinimumWidth = 6;
            this.ColDelete.Name = "ColDelete";
            this.ColDelete.Text = "Delete";
            this.ColDelete.UseColumnTextForLinkValue = true;
            this.ColDelete.Width = 60;
            // 
            // colPriceChange
            // 
            this.colPriceChange.HeaderText = "";
            this.colPriceChange.MinimumWidth = 6;
            this.colPriceChange.Name = "colPriceChange";
            this.colPriceChange.Text = "Price Change History";
            this.colPriceChange.UseColumnTextForLinkValue = true;
            this.colPriceChange.Width = 160;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label2.Location = new System.Drawing.Point(15, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 18);
            this.label2.TabIndex = 2;
            this.label2.Text = "Category :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label3.Location = new System.Drawing.Point(275, 37);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(106, 18);
            this.label3.TabIndex = 4;
            this.label3.Text = "Sub Category :";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label4.Location = new System.Drawing.Point(901, 41);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(54, 17);
            this.label4.TabIndex = 0;
            this.label4.Text = "Brand :";
            this.label4.Visible = false;
            // 
            // cboMainCategory
            // 
            this.cboMainCategory.FormattingEnabled = true;
            this.cboMainCategory.Location = new System.Drawing.Point(103, 32);
            this.cboMainCategory.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cboMainCategory.Name = "cboMainCategory";
            this.cboMainCategory.Size = new System.Drawing.Size(139, 26);
            this.cboMainCategory.TabIndex = 3;
            this.cboMainCategory.SelectedIndexChanged += new System.EventHandler(this.cboMainCategory_SelectedIndexChanged);
            this.cboMainCategory.SelectedValueChanged += new System.EventHandler(this.cboMainCategory_SelectedValueChanged);
            // 
            // cboSubCategory
            // 
            this.cboSubCategory.FormattingEnabled = true;
            this.cboSubCategory.Location = new System.Drawing.Point(390, 32);
            this.cboSubCategory.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cboSubCategory.Name = "cboSubCategory";
            this.cboSubCategory.Size = new System.Drawing.Size(139, 26);
            this.cboSubCategory.TabIndex = 5;
            // 
            // cboBrand
            // 
            this.cboBrand.FormattingEnabled = true;
            this.cboBrand.Location = new System.Drawing.Point(971, 37);
            this.cboBrand.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cboBrand.Name = "cboBrand";
            this.cboBrand.Size = new System.Drawing.Size(139, 25);
            this.cboBrand.TabIndex = 1;
            this.cboBrand.Visible = false;
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(891, 35);
            this.txtName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(161, 24);
            this.txtName.TabIndex = 9;
            this.txtName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtName_KeyDown);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label5.Location = new System.Drawing.Point(825, 37);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 18);
            this.label5.TabIndex = 8;
            this.label5.Text = "Name :";
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.Transparent;
            this.btnSearch.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(223)))), ((int)(((byte)(223)))));
            this.btnSearch.FlatAppearance.BorderSize = 0;
            this.btnSearch.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(223)))), ((int)(((byte)(223)))));
            this.btnSearch.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(223)))), ((int)(((byte)(223)))));
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearch.Image = global::POS.Properties.Resources.search_small;
            this.btnSearch.Location = new System.Drawing.Point(1071, 19);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(100, 57);
            this.btnSearch.TabIndex = 10;
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.SystemColors.Control;
            this.btnAdd.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(223)))), ((int)(((byte)(223)))));
            this.btnAdd.FlatAppearance.BorderSize = 0;
            this.btnAdd.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(223)))), ((int)(((byte)(223)))));
            this.btnAdd.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(223)))), ((int)(((byte)(223)))));
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.Image = global::POS.Properties.Resources.addnewproduct;
            this.btnAdd.Location = new System.Drawing.Point(12, 2);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(239, 73);
            this.btnAdd.TabIndex = 0;
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(202)))), ((int)(((byte)(125)))));
            this.btnRefresh.FlatAppearance.BorderSize = 0;
            this.btnRefresh.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnRefresh.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.Image = global::POS.Properties.Resources.refresh_small;
            this.btnRefresh.Location = new System.Drawing.Point(1199, 19);
            this.btnRefresh.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(104, 57);
            this.btnRefresh.TabIndex = 11;
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(573, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 18);
            this.label1.TabIndex = 6;
            this.label1.Text = "Barcode :";
            // 
            // txtBarcode
            // 
            this.txtBarcode.Location = new System.Drawing.Point(660, 32);
            this.txtBarcode.Name = "txtBarcode";
            this.txtBarcode.Size = new System.Drawing.Size(122, 24);
            this.txtBarcode.TabIndex = 7;
            this.txtBarcode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtBarcode_KeyDown);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnRefresh);
            this.groupBox1.Controls.Add(this.txtBarcode);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.cboMainCategory);
            this.groupBox1.Controls.Add(this.cboSubCategory);
            this.groupBox1.Controls.Add(this.btnSearch);
            this.groupBox1.Controls.Add(this.txtName);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(10, 82);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1321, 89);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rdAll);
            this.groupBox2.Controls.Add(this.rdNonConsignment);
            this.groupBox2.Controls.Add(this.rdConsignment);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(286, 19);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(590, 56);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Counter";
            // 
            // rdAll
            // 
            this.rdAll.AutoSize = true;
            this.rdAll.Checked = true;
            this.rdAll.Location = new System.Drawing.Point(9, 24);
            this.rdAll.Name = "rdAll";
            this.rdAll.Size = new System.Drawing.Size(44, 22);
            this.rdAll.TabIndex = 0;
            this.rdAll.TabStop = true;
            this.rdAll.Text = "All";
            this.rdAll.UseVisualStyleBackColor = true;
            this.rdAll.CheckedChanged += new System.EventHandler(this.rdAll_CheckedChanged);
            // 
            // rdNonConsignment
            // 
            this.rdNonConsignment.AutoSize = true;
            this.rdNonConsignment.Location = new System.Drawing.Point(135, 24);
            this.rdNonConsignment.Name = "rdNonConsignment";
            this.rdNonConsignment.Size = new System.Drawing.Size(207, 22);
            this.rdNonConsignment.TabIndex = 2;
            this.rdNonConsignment.Text = "Non-Consignment Counter";
            this.rdNonConsignment.UseVisualStyleBackColor = true;
            this.rdNonConsignment.CheckedChanged += new System.EventHandler(this.rdNonConsignment_CheckedChanged);
            // 
            // rdConsignment
            // 
            this.rdConsignment.AutoSize = true;
            this.rdConsignment.Location = new System.Drawing.Point(388, 24);
            this.rdConsignment.Name = "rdConsignment";
            this.rdConsignment.Size = new System.Drawing.Size(174, 22);
            this.rdConsignment.TabIndex = 1;
            this.rdConsignment.Text = "Consignment Counter";
            this.rdConsignment.UseVisualStyleBackColor = true;
            this.rdConsignment.CheckedChanged += new System.EventHandler(this.rdConsignment_CheckedChanged);
            // 
            // btnPBC
            // 
            this.btnPBC.Location = new System.Drawing.Point(1157, 32);
            this.btnPBC.Name = "btnPBC";
            this.btnPBC.Size = new System.Drawing.Size(121, 33);
            this.btnPBC.TabIndex = 4;
            this.btnPBC.Text = "Print Barcode";
            this.btnPBC.UseVisualStyleBackColor = false;
            this.btnPBC.Visible = false;
            this.btnPBC.Click += new System.EventHandler(this.btnPBC_Click);
            // 
            // ItemList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1343, 685);
            this.Controls.Add(this.btnPBC);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.dgvItemList);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cboBrand);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "ItemList";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Product List";
            this.Load += new System.EventHandler(this.ItemList_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvItemList)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.DataGridView dgvItemList;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cboMainCategory;
        private System.Windows.Forms.ComboBox cboSubCategory;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.ComboBox cboBrand;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtBarcode;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rdAll;
        private System.Windows.Forms.RadioButton rdNonConsignment;
        private System.Windows.Forms.RadioButton rdConsignment;
        private System.Windows.Forms.Button btnPBC;
        private System.Windows.Forms.DataGridViewTextBoxColumn colId;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProductCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBarcode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProductName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUnitPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDisPercent;
        private System.Windows.Forms.DataGridViewLinkColumn colEdit;
        private System.Windows.Forms.DataGridViewLinkColumn ColDelete;
        private System.Windows.Forms.DataGridViewLinkColumn colPriceChange;
    }
}