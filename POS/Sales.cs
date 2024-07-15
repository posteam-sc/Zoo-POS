using POS.APP_Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common.CommandTrees.ExpressionBuilder;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace POS
{
    public partial class Sales : Form
    {
        #region Variables
        private POSEntities entity = new POSEntities();

        List<_dynamicPrice> dynamicPrice = new List<_dynamicPrice>();
        int sangaticketcount { get; set; }
        TextBox prodCode = new TextBox();

        private String DraftId = string.Empty;

        bool isDraft { get; set; }
        bool isload = false;

        string mssg = "";

        public static string zooName;


        int _rowIndex, curQty = 1;
        public string note = "",counterName;

        public static IQueryable<Product> productVar;
        //int Qty = 0;


        // public Product _productInfo=new Product();
        public int FOCQty = 1;
        
        //bool isAdd = false;



        #endregion
        ApplicationLog al = new ApplicationLog();
        #region Events

        public Sales()
        {
            InitializeComponent();
        }

        //protected override CreateParams CreateParams
        //{
        //    get
        //    {
        //        CreateParams cp = base.CreateParams;
        //        cp.ExStyle |= 0x02000000;  // Turn on WS_EX_COMPOSITED
        //        return cp;
        //    }
        //}

        #region Hot keys handler
        void Form_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.M)      //  Ctrl + M => Focus Member Id
            {
                cboProductName.DroppedDown = false;
                cboPaymentMethod.DroppedDown = false;

            }
            else if (e.Control && e.KeyCode == Keys.E)      // Ctrl + E => Focus DropDown Customer
            {
                cboProductName.DroppedDown = false;
                cboPaymentMethod.DroppedDown = false;

            }
            else if (e.Control && e.KeyCode == Keys.N) //  Ctrl + N => Click Create New Customer
            {
                cboProductName.DroppedDown = false;
                cboPaymentMethod.DroppedDown = false;

            }
            else if (e.Control && e.KeyCode == Keys.A) // Ctrl + A => Focus Search Product Code Drop Down 
            {
                cboPaymentMethod.DroppedDown = false;
                cboProductName.DroppedDown = true;
                if (cboProductName.Focused != true)
                {
                    cboProductName.Focus();
                }
            }
            else if (e.Control && e.KeyCode == Keys.H) // Ctrl + H => Click Search 
            {
                cboProductName.DroppedDown = false;
                cboPaymentMethod.DroppedDown = false;
                btnSearch.PerformClick();
            }
            else if (e.Control && e.KeyCode == Keys.D) // Ctrl + D => focus discount
            {
                cboProductName.DroppedDown = false;
                cboPaymentMethod.DroppedDown = false;
                txtAdditionalDiscount.Focus();
            }
            else if (e.Control && e.KeyCode == Keys.Y) // Ctrl + Y => focus Payment Method
            {
                cboProductName.DroppedDown = false;
                cboPaymentMethod.DroppedDown = true;
                if (cboPaymentMethod.Focused != true)
                {
                    cboPaymentMethod.Focus();
                }
            }
            else if (e.Control && e.KeyCode == Keys.T) // Ctrl + T => focus c in table
            {
                cboProductName.DroppedDown = false;
                cboPaymentMethod.DroppedDown = false;
                dgvSalesItem.CurrentCell = dgvSalesItem.Rows[1].Cells[8];
                dgvSalesItem.Focus();
            }
            else if (e.Control && e.KeyCode == Keys.Q) // Ctrl + Q => focus Quantity in table
            {
                cboProductName.DroppedDown = false;
                cboPaymentMethod.DroppedDown = false;
                dgvSalesItem.CurrentCell = dgvSalesItem.Rows[1].Cells[3];
                dgvSalesItem.Focus();
            }
            else if (e.Control && e.KeyCode == Keys.P)     // Ctrl + P => Click Paid
            {
                cboProductName.DroppedDown = false;
                cboPaymentMethod.DroppedDown = false;
                btnPaid.PerformClick();
            }
            else if (e.Control && e.KeyCode == Keys.S)     // Ctrl + S => Click Save As Draft
            {
                cboProductName.DroppedDown = false;
                cboPaymentMethod.DroppedDown = false;
                btnSave.PerformClick();
            }
            else if (e.Control && e.KeyCode == Keys.L)     // Ctrl + L => Click Load As Draft
            {
                cboProductName.DroppedDown = false;
                cboPaymentMethod.DroppedDown = false;
                btnLoadDraft.PerformClick();
            }
            else if (e.Control && e.KeyCode == Keys.F)     // Ctrl + C => Click FOC
            {
                btnFOC.PerformClick();
            }
        }
        #endregion


        public void PriceColumnControl()
        {
            dgvSalesItem.Columns["colUnitPrice"].ReadOnly = SettingController.AllowDynamicPrice ? false : true;
            this.dgvSalesItem.Refresh();
            this.Refresh();
        }

        private void Sales_Load(object sender, EventArgs e)
        {
            try
            {
                //SaleFormSize();
                PriceColumnControl();
                Localization.Localize_FormControls(this);
                // dgvSalesItem.Columns[4].ReadOnly = Convert.ToBoolean(SettingController.DynamicPrice);
                #region Setting Hot Kyes For the Controls
                SendKeys.Send("%"); SendKeys.Send("%"); // Clicking "Alt" on page load to show underline of Hot Keys
                this.KeyPreview = true;
                this.KeyDown += new KeyEventHandler(Form_KeyDown);
                #endregion

                #region Disable Sort Mode of dgvSaleItem Grid
                foreach (DataGridViewColumn col in dgvSalesItem.Columns)
                    col.SortMode = DataGridViewColumnSortMode.NotSortable;
                #endregion

                this.cboPaymentMethod.TextChanged -= new EventHandler(cboPaymentMethod_TextChanged);
                dgvSearchProductList.AutoGenerateColumns = false;
                cboPaymentMethod.DataSource = entity.PaymentTypes.ToList();
                cboPaymentMethod.DisplayMember = "Name";
                cboPaymentMethod.ValueMember = "Id";
                productVar=entity.Products;
                
                Utility.BindProduct(cboProduct);
                List<Product> productList = new List<Product>();
                Product productObj = new Product();
                productObj.Id = 0;
                productObj.Name = "";
                productList.Add(productObj);
                productList.AddRange(productVar.ToList());
                cboProductName.DataSource = productList;
                cboProductName.DisplayMember = "Name";
                cboProductName.ValueMember = "Id";
                cboProductName.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                cboProductName.AutoCompleteSource = AutoCompleteSource.ListItems;
                

                this.cboPaymentMethod.TextChanged += new EventHandler(cboPaymentMethod_TextChanged);
                dgvSalesItem.ColumnHeadersDefaultCellStyle.Font = new Font("Zawgyi-One", 9F);
                dgvSalesItem.Focus();
                if (SettingController.TicketSale)
                {
                    ForTicket();
                }
                counterName = entity.Counters.Where(x => x.Id == MemberShip.CounterId).FirstOrDefault().Name;
                zooName= entity.Brands.Where(x => x.IsDelete == false).SingleOrDefault().Name;
            }
            catch (Exception ex)
            {
                string sExec = ex.Message;
                if (ex.InnerException != null)
                {
                    sExec = ex.InnerException.ToString();
                }
                al.WriteErrorLog(ex.ToString(), "Sales_Load", sExec);
            }
        }


        public void ForTicket()
        {
            gbTicketing.Visible = SettingController.TicketSale;
           
            btnLoadDraft.Visible = btnSave.Visible = btnnote.Visible = !SettingController.TicketSale;

            //btnCancel.Location = new Point(223, 4);

            TableLayoutPanel tlp = this.tableLayoutPanel2;
            if (this.btnCancel.Parent == tlp)
            {
                var cancelCell = tlp.GetCellPosition(btnCancel);
                var savedraftCell = tlp.GetCellPosition(btnSave);
                tlp.SetCellPosition(btnCancel, savedraftCell);
                tlp.SetCellPosition(btnSave, cancelCell);
            }
            var list = entity.TicketButtonAssigns.ToList();

            if (list.Count() > 0)
            {
                btnAdd1.Tag = list.Where(l => l.ButtonName == btnAdd1.Name).First().Assignproductid;
                lblLocalAdult.Tag = list.Where(l => l.ButtonName == lblLocalAdult.Name).First().Assignproductid;
                btnAdd1Minus.Tag = list.Where(l => l.ButtonName == btnAdd1Minus.Name).First().Assignproductid;

                btnAdd2.Tag = list.Where(l => l.ButtonName == btnAdd2.Name).First().Assignproductid;
                lblLocalChild.Tag = list.Where(l => l.ButtonName == lblLocalChild.Name).First().Assignproductid;
                btnAdd2Minus.Tag = list.Where(l => l.ButtonName == btnAdd2Minus.Name).First().Assignproductid;

                btnAdd3.Tag = list.Where(l => l.ButtonName == btnAdd3.Name).First().Assignproductid;
                lblForeignAdult.Tag = list.Where(l => l.ButtonName == lblForeignAdult.Name).First().Assignproductid;
                btnAdd3Minus.Tag = list.Where(l => l.ButtonName == btnAdd3Minus.Name).First().Assignproductid;

                btnAdd4.Tag = list.Where(l => l.ButtonName == btnAdd4.Name).First().Assignproductid;
                lblForeignChild.Tag = list.Where(l => l.ButtonName == lblForeignChild.Name).First().Assignproductid;
                btnAdd4Minus.Tag = list.Where(l => l.ButtonName == btnAdd4Minus.Name).First().Assignproductid;

                btnSangaAdd.Tag = list.Where(l => l.ButtonName == btnSangaAdd.Name).First().Assignproductid;
                lblSangaTicketStatus.Tag = list.Where(l => l.ButtonName == lblSangaTicketStatus.Name).First().Assignproductid;
                btnSangaMinus.Tag = list.Where(l => l.ButtonName == btnSangaMinus.Name).First().Assignproductid;
            }
        }
        //void FillServiceFee()
        //{
        //    lblServiceFee.Text = SettingController.ServiceFee.ToString();
        //}
        private void dgvSalesItem_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgvSalesItem_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            _rowIndex = e.RowIndex;
            if (e.RowIndex >= 0)
            {
                //FillServiceFee();
                //Delete
                if (e.ColumnIndex == DGV.delete)
                {
                    object deleteProductCode = dgvSalesItem[1, e.RowIndex].Value;

                    //If product code is null, this is just new role without product. Do not need to delete the row.
                    if (deleteProductCode != null)
                    {
                        DialogResult result = MessageBox.Show("Are you sure you want to delete?", "Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                        if (result.Equals(DialogResult.OK))
                        {
                            int currentProductId = Convert.ToInt32(dgvSalesItem[9, e.RowIndex].Value);
                            entity = new POSEntities();
                            Product pro = productVar.Where(p => p.Id == currentProductId).FirstOrDefault<Product>();

                            if (dgvSalesItem.Rows.Count != 0)
                            {
                                if (dgvSalesItem[9, e.RowIndex].Value == null || Convert.ToInt32(dgvSalesItem[9, e.RowIndex].Value) == 0)
                                {
                                     if (SettingController.TicketSale)
                                    {
                                        if (Convert.ToInt16(lblLocalAdult.Tag) == pro.Id)
                                        {
                                            lblLocalAdult.Text = "0";
                                        }
                                        else if (Convert.ToInt16(lblLocalChild.Tag) == pro.Id)
                                        {
                                            lblLocalChild.Text = "0";
                                        }
                                        else if (Convert.ToInt16(lblForeignAdult.Tag) == pro.Id)
                                        {
                                            lblForeignAdult.Text = "0";
                                        }
                                        else if (Convert.ToInt16(lblForeignChild.Tag) == pro.Id)
                                        {
                                            lblForeignChild.Text = "0";
                                        }
                                        else if (Convert.ToInt16(lblSangaTicketStatus.Tag) == pro.Id)
                                        {
                                            sangaticketcount = 0;
                                            lblSangaTicketStatus.Text = "0";
                                        }
                                        lblTicketTotal.Text = Convert.ToString(Convert.ToInt16(lblLocalAdult.Text) + Convert.ToInt16(lblLocalChild.Text) + Convert.ToInt16(lblForeignAdult.Text) + Convert.ToInt16(lblForeignChild.Text) + Convert.ToInt16(lblSangaTicketStatus.Text));
                                    }
                                    if (dynamicPrice.Where(a => a.dynamicProductCode == pro.ProductCode).FirstOrDefault() != null)
                                    {
                                        dynamicPrice.Remove(dynamicPrice.Where(a => a.dynamicProductCode == pro.ProductCode).FirstOrDefault());
                                    }


                                }
                            }
                            if (dynamicPrice.Where(a => a.dynamicProductCode == dgvSalesItem[1, e.RowIndex].Value.ToString()) != null)
                            {
                                dynamicPrice.Remove(dynamicPrice.Where(a => a.dynamicProductCode == dgvSalesItem[1, e.RowIndex].Value.ToString()).FirstOrDefault());
                            }


                            if (SettingController.TicketSale)
                            {
                                
                                if (Convert.ToInt16(lblLocalAdult.Tag) == pro.Id)
                                {
                                    lblLocalAdult.Text = "0";
                                }
                                else if (Convert.ToInt16(lblLocalChild.Tag) == pro.Id)
                                {
                                    lblLocalChild.Text = "0";
                                }
                                else if (Convert.ToInt16(lblForeignAdult.Tag) == pro.Id)
                                {
                                    lblForeignAdult.Text = "0";
                                }
                                else if (Convert.ToInt16(lblForeignChild.Tag) == pro.Id)
                                {
                                    lblForeignChild.Text = "0";
                                }
                                else if (Convert.ToInt16(lblSangaTicketStatus.Tag) == pro.Id)
                                {
                                    sangaticketcount = 0;
                                    lblSangaTicketStatus.Text = "0";
                                }
                                lblTicketTotal.Text = Convert.ToString(Convert.ToInt16(lblLocalAdult.Text) + Convert.ToInt16(lblLocalChild.Text) + Convert.ToInt16(lblForeignAdult.Text) + Convert.ToInt16(lblForeignChild.Text) + Convert.ToInt16(lblSangaTicketStatus.Text));
                            }

                            dgvSalesItem.Rows.RemoveAt(e.RowIndex);
                            UpdateTotalCost();
                            dgvSalesItem.CurrentCell = dgvSalesItem[1, e.RowIndex];

                            //Cell_ReadOnly();
                        }
                    }
                }
                else if (e.ColumnIndex == 5)
                {

                }
                //else if (e.ColumnIndex == 3)
                //{
                //    btnAdd1.Enabled = false;
                //    btnAdd1Minus.Enabled = false;
                //    btnAdd2.Enabled = false;
                //    btnAdd2Minus.Enabled = false;
                //    btnAdd3.Enabled = false;
                //    btnAdd3Minus.Enabled = false;
                //    btnAdd4.Enabled = false;
                //    btnAdd4Minus.Enabled = false;

                //}
                else if (e.ColumnIndex == 4)
                {

                }
                else if (e.ColumnIndex == 0 || e.ColumnIndex == 1 || e.ColumnIndex == 2 || e.ColumnIndex == 3 || e.ColumnIndex == 4)
                {
                    dgvSalesItem.CurrentCell = dgvSalesItem.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    dgvSalesItem.BeginEdit(true);
                }

            }
        }

        private void dgvSalesItem_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyData == Keys.Enter)
                {
                    int col = dgvSalesItem.CurrentCell.ColumnIndex;
                    int row = dgvSalesItem.CurrentCell.RowIndex;

                    if (col == 9)
                    {
                        object deleteProductCode = dgvSalesItem[1, row].Value;

                        //If product code is null, this is just new role without product. Do not need to delete the row.
                        if (deleteProductCode != null)
                        {

                            DialogResult result = MessageBox.Show("Are you sure you want to delete?", "Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                            if (result.Equals(DialogResult.OK))
                            {
                                dgvSalesItem.Rows.RemoveAt(row);
                                UpdateTotalCost();
                                dgvSalesItem.CurrentCell = dgvSalesItem[1, row];

                            }
                        }
                    }
                    if (col == 3)
                    {
                        int currentQty = Convert.ToInt32(dgvSalesItem.Rows[row].Cells[3].Value);
                        if (currentQty == 0 || currentQty.ToString() == string.Empty)
                        {
                            //row.Cells[DGV.pname].Value = "1";
                            MessageBox.Show("Please fill Quantity.");

                            dgvSalesItem.Rows[row].Cells[3].Selected = true;
                            return;
                        }
                    }

                    e.Handled = true;
                }
            }
            catch { }
        }
        private void btnPaid_Click(object sender, EventArgs e)
        {
            try
            {
                btnAdd1.Enabled = true;
                btnAdd1Minus.Enabled = true;
                btnAdd2.Enabled = true;
                btnAdd2Minus.Enabled = true;
                btnAdd3.Enabled = true;
                btnAdd3Minus.Enabled = true;
                btnAdd4.Enabled = true;
                btnAdd4Minus.Enabled = true;

                string st = note;

                List<TransactionDetail> DetailList = GetTranscationListFromDataGridView();
                if (DetailList.Count() != 0)
                {
                  

                    var FOCList = DetailList.Where(x => x.IsFOC == true).ToList();

                    if (cboPaymentMethod.Text != "FOC")
                    {
                        if (DetailList.Count == FOCList.Count)
                        {
                            MessageBox.Show("Not allow to save only FOC item. For this operation, please choose FOC payment method.");
                            cboPaymentMethod.Focus();
                            return;
                        }
                    }


                    List<int> index = (from r in dgvSalesItem.Rows.Cast<DataGridViewRow>()
                                       where r.Cells[DGV.qty].Value == null || r.Cells[DGV.qty].Value.ToString() == String.Empty || r.Cells[DGV.qty].Value.ToString() == "0"
                                       select r.Index).ToList();


                    index.RemoveAt(index.Count - 1);

                    if (index.Count > 0)
                    {

                        foreach (var a in index)
                        {
                            dgvSalesItem.Rows[a].DefaultCellStyle.BackColor = Color.Red;

                        }
                        return;
                    }

                    UpdateTotalCost();

                    //Cash
                    if (Convert.ToInt32(cboPaymentMethod.SelectedValue) == 1)
                    {
                        PaidByCash2 form = new PaidByCash2();
                        form.DetailList = DetailList;
                        form.counterName = counterName;
                        int extraDiscount = 0;
                        Int32.TryParse(txtAdditionalDiscount.Text, out extraDiscount);
                        form.Discount = Convert.ToInt32(lblDiscountTotal.Text);
                        form.ExtraDiscount = extraDiscount;
                        form.Text = "Paid By Cash";
                        form.IsFoc = false;
                        form.Note = note;
                        form.ShowDialog();
                    }            
                    else if (Convert.ToInt32(cboPaymentMethod.SelectedValue) == 4)
                    {
                        PaidByCash2 form = new PaidByCash2();
                        form.DetailList = DetailList;
                        form.counterName = counterName;
                        form.Discount =0;
                        form.ExtraDiscount = 0;
                        form.IsFoc = true;
                        form.Text = "Paid By FOC";
                        form.Note = note;
                        form.ShowDialog();
                    }
                    //else if (Convert.ToInt32(cboPaymentMethod.SelectedValue) == 5)
                    //{
                    //    PaidByMPU form = new PaidByMPU();
                    //    form.DetailList = DetailList;
                    //    int extraDiscount = 0;
                    //    Int32.TryParse(txtAdditionalDiscount.Text, out extraDiscount);
                    //    form.IsPrint = true;
                    //    form.isDraft = isDraft;
                    //    form.DraftId = DraftId;
                    //    form.Discount = Convert.ToInt32(lblDiscountTotal.Text);
                    //    form.ExtraDiscount = extraDiscount;
                    //    form.Note = note;

                    //    form.ShowDialog();
                    //}
                    //else if (Convert.ToInt32(cboPaymentMethod.SelectedValue) == 6)
                    //{
                    //    PaidByFOC form = new PaidByFOC();
                    //    form.DetailList = DetailList;
                    //    form.Type = 6;
                    //    int extraDiscount = 0;
                    //    Int32.TryParse(txtAdditionalDiscount.Text, out extraDiscount);
                    //    form.IsPrint = true;
                    //    form.isDraft = isDraft;
                    //    form.DraftId = DraftId;
                    //    form.Discount = Convert.ToInt32(lblDiscountTotal.Text);
                    //    form.ExtraDiscount = extraDiscount;

                    //    form.Note = note;
                    //    form.ShowDialog();
                    //}
                }
                else
                {
                    MessageBox.Show("You haven't select any item to paid");
                }
            }
            catch (Exception ex)
            {
                string sExec = ex.Message;
                if (ex.InnerException != null)
                {
                    sExec = ex.InnerException.ToString();
                }
                al.WriteErrorLog(ex.ToString(), "btnPaid_Click", sExec);
            }
        }




        private void btnLoadDraft_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("This action will erase current sale data. Would you like to continue?", "Load", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (result.Equals(DialogResult.OK))
            {
                DraftList form = new DraftList();
                form.Show();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //will only work if the grid have data row
            //datagrid count header as a row, so we have to check there is more than one row
            if (dgvSalesItem.Rows.Count > 1)
            {
                List<TransactionDetail> DetailList = GetTranscationListFromDataGridView();

                int extraDiscount = 0;
                Int32.TryParse(txtAdditionalDiscount.Text, out extraDiscount);

                System.Data.Objects.ObjectResult<String> Id = entity.InsertDraft(DateTime.Now, MemberShip.UserId, MemberShip.CounterId, TransactionType.Sale, true, true, Convert.ToInt32(cboPaymentMethod.SelectedValue), extraDiscount, DetailList.Sum(x => x.TotalAmount) - extraDiscount, long.Parse(lblTotal.Text));

                entity = new POSEntities();
                string resultId = Id.FirstOrDefault().ToString();
                Transaction insertedTransaction = (from trans in entity.Transactions where trans.Id == resultId select trans).FirstOrDefault<Transaction>();
                insertedTransaction.IsDeleted = false;

                foreach (TransactionDetail detail in DetailList)
                {
                    detail.IsDeleted = false;
                    insertedTransaction.TransactionDetails.Add(detail);
                }

                entity.SaveChanges();


                Clear();
            }
        }

        private void Sales_Activated(object sender, EventArgs e)
        {
            //DailyRecord latestRecord = (from rec in entity.DailyRecords where rec.CounterId == MemberShip.CounterId && rec.IsActive == true select rec).FirstOrDefault();
            //if (latestRecord == null)
            //{
            //    StartDay form = new StartDay();
            //    form.Show();
            //}
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string productName = cboProductName.Text.Trim();
            List<Product> pList = productVar.Where(x => x.Name.Trim().Contains(productName)).ToList();

            if (pList.Count > 0)
            {
                dgvSearchProductList.DataSource = pList;
                dgvSearchProductList.Focus();


            }
            else
            {
                MessageBox.Show("Item not found!", "Cannot find");
                dgvSearchProductList.DataSource = "";
                return;
            }
            this.AcceptButton = null;
        }

        private void CountTicket(long id)
        {
            if ((long)lblLocalAdult.Tag == id)
            {
                lblLocalAdult.Text = Convert.ToString(Convert.ToInt16(lblLocalAdult.Text) + 1);
            }
            if ((long)lblLocalChild.Tag == id)
            {
                lblLocalChild.Text = Convert.ToString(Convert.ToInt16(lblLocalChild.Text) + 1);
            }
            if ((long)lblForeignAdult.Tag == id)
            {
                lblForeignAdult.Text = Convert.ToString(Convert.ToInt16(lblForeignAdult.Text) + 1);
            }
            if ((long)lblForeignChild.Tag == id)
            {
                lblForeignChild.Text = Convert.ToString(Convert.ToInt16(lblForeignChild.Text) + 1);
            }
            if ((long)lblSangaTicketStatus.Tag == id)
            {
                sangaticketcount++;
                lblSangaTicketStatus.Text = Convert.ToString(Convert.ToInt16(lblSangaTicketStatus.Text) + 1);
            }
            lblTicketTotal.Text = Convert.ToString(Convert.ToInt16(lblLocalAdult.Text) + Convert.ToInt16(lblLocalChild.Text) + Convert.ToInt16(lblForeignAdult.Text) + Convert.ToInt16(lblForeignChild.Text) + Convert.ToInt16(lblSangaTicketStatus.Text));
        }

        private void dgvSearchProductList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                //FillServiceFee();
                int currentProductId = Convert.ToInt32(dgvSearchProductList.Rows[e.RowIndex].Cells[0].Value);
                int count = dgvSalesItem.Rows.Count;
                if (e.ColumnIndex == 1)
                {
                    entity = new POSEntities();
                    Product pro = productVar.Where(p => p.Id == currentProductId).FirstOrDefault<Product>();

                    if (SettingController.TicketSale)
                    {
                        CountTicket(pro.Id);
                    }
                    if (pro != null)
                    {
                        curQty = 1;
                        DataGridViewRow row = (DataGridViewRow)dgvSalesItem.Rows[count - 1].Clone();
                        row.Cells[DGV.barcode].Value = pro.Barcode;
                        row.Cells[DGV.pcode].Value = pro.ProductCode;
                        row.Cells[DGV.pname].Value = pro.Name;
                        row.Cells[DGV.qty].Value = 1;
                        row.Cells[DGV.dis].Value = pro.DiscountRate;
                        row.Cells[DGV.price].Value = pro.Price;
                        row.Cells[DGV.cost].Value = getActualCost(pro, false);
                        row.Cells[DGV.remark].Value = "";
                        row.Cells[DGV.pid].Value = currentProductId;
                        dgvSalesItem.Rows.Add(row);
                        _rowIndex = dgvSalesItem.Rows.Count - 2;
                        cboProductName.SelectedIndex = 0;
                        dgvSearchProductList.DataSource = "";
                        dgvSearchProductList.ClearSelection();
                        dgvSalesItem.Focus();
                        // var list = dgvSalesItem.DataSource;
                        Check_ProductCode_Exist(pro.ProductCode,pro.Price,pro.DiscountRate);
                        
                       

                        //dynamicPrice = new List<_dynamicPrice>();
                        Cell_ReadOnly();



                    }
                    else
                    {

                        MessageBox.Show("Item not found!", "Cannot find");
                    }

                    UpdateTotalCost();

                }
            }
        }

        private void dgvSearchProductList_KeyDown(object sender, KeyEventArgs e)
        {

            if ((e.KeyData == Keys.Enter && dgvSearchProductList.CurrentCell != null) || (e.KeyData == Keys.Space && dgvSearchProductList.CurrentCell != null))
            {
                int Row = dgvSearchProductList.CurrentCell.RowIndex;
                int Column = dgvSearchProductList.CurrentCell.ColumnIndex;
                int currentProductId = Convert.ToInt32(dgvSearchProductList.Rows[Row].Cells[0].Value);
                int count = dgvSalesItem.Rows.Count;
                if (Column == 1)
                {
                    entity = new POSEntities();
                    Product pro = productVar.Where(p => p.Id == currentProductId).FirstOrDefault<Product>();

                    if (pro != null)
                    {
                        //fill the new row with the product information
                        //dgvSalesItem.Rows.Add();
                        //int newRowIndex = dgvSalesItem.NewRowIndex;

                        DataGridViewRow row = (DataGridViewRow)dgvSalesItem.Rows[count - 1].Clone();
                        row.Cells[DGV.barcode].Value = pro.Barcode;
                        row.Cells[DGV.pcode].Value = pro.ProductCode;
                        row.Cells[DGV.pname].Value = pro.Name;
                        row.Cells[DGV.price].Value = pro.Price;
                        row.Cells[DGV.dis].Value = pro.DiscountRate;
                        row.Cells[DGV.cost].Value = getActualCost(pro, false);
                        row.Cells[DGV.remark].Value = "";
                        row.Cells[DGV.pid].Value = currentProductId;

                        dgvSalesItem.Rows.Add(row);
                        cboProductName.SelectedIndex = 0;
                        dgvSearchProductList.DataSource = "";
                        dgvSearchProductList.ClearSelection();
                        dgvSalesItem.Focus();
                        Check_ProductCode_Exist(pro.ProductCode,pro.Price, pro.DiscountRate);

                        Cell_ReadOnly();
                    }
                    else
                    {

                        MessageBox.Show("Item not found!", "Cannot find");
                    }

                    UpdateTotalCost();
                }
            }
        }

        private void cboProductName_KeyDown(object sender, KeyEventArgs e)
        {
            this.AcceptButton = btnSearch;
        }

        //private void txtAdditionalDiscount_KeyPress(object sender, KeyPressEventArgs e)
        //{
        //    if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
        //    {
        //        e.Handled = true;
        //    }
        //}

      

        #endregion

        #region Function

        private List<TransactionDetail> GetTranscationListFromDataGridView()
        {
            List<TransactionDetail> DetailList = new List<TransactionDetail>();

            foreach (DataGridViewRow row in dgvSalesItem.Rows)
            {
                if (!row.IsNewRow && row.Cells[DGV.pid].Value != null && row.Cells[DGV.barcode].Value != null && row.Cells[DGV.pcode].Value != null && row.Cells[DGV.pname].Value != null)
                {
                    TransactionDetail transDetail = new TransactionDetail();
                    bool IsFOC = false;
                    if (row.Cells[7].Value.ToString() == "FOC")
                    {
                        IsFOC = true;
                    }
                    int qty = 0, productId = 0;
                    //  bool alreadyinclude = false;
                    decimal discountRate = 0;
                    Int32.TryParse(row.Cells[DGV.pid].Value.ToString(), out productId);
                    Int32.TryParse(row.Cells[DGV.qty].Value.ToString(), out qty);
                    Decimal.TryParse(row.Cells[DGV.dis].Value.ToString(), out discountRate);

                    //Check productId is valid or not.
                    entity = new POSEntities();
                    Product pro = productVar.Where(p => p.Id == productId).FirstOrDefault<Product>();

                    if (pro != null)
                    {
                        transDetail.ProductId = pro.Id;
                        //  transDetail.UnitPrice = pro.Price;
                        transDetail.SellingPrice = pro.Price;

                        if (IsFOC)
                        {
                            transDetail.UnitPrice = 0;
                            transDetail.DiscountRate = 0;
                        }
                        else
                        {
                            transDetail.UnitPrice = pro.Price;
                            transDetail.DiscountRate = discountRate;
                        }
                        transDetail.UnitPrice = pro.Price;
                        transDetail.DiscountRate = discountRate;
                        transDetail.Qty = qty;
                        // transDetail.TotalAmount = Convert.ToInt64(getActualCost(pro, discountRate, IsFOC)) * qty;
                        if (Convert.ToInt32(cboPaymentMethod.SelectedValue) == 4)
                        {
                            transDetail.TotalAmount = 0;
                        }
                        else
                        {
                            transDetail.TotalAmount = Convert.ToInt64(getActualCost(pro, discountRate, IsFOC)) * qty;
                        }
                        if (row.Cells[DGV.remark].Value.ToString() == "FOC")
                        {
                            transDetail.IsFOC = true;
                        }
                        else
                        {
                            transDetail.IsFOC = false;
                        }

                        DetailList.Add(transDetail);
                    }
                    //   }
                }
            }

            return DetailList;
        }

        private void UpdateTotalCost()
        {
            int discount = 0, totalqty = 0;
            int total = 0;
            foreach (DataGridViewRow dgrow in dgvSalesItem.Rows)
            {
                //check if the current one is new empty row
                if (!dgrow.IsNewRow && dgrow.Cells[1].Value != null && dgrow.Cells[DGV.pid].Value != null)
                {
                    string rowProductCode = string.Empty;
                    int qty = 0;
                    //rowProductCode = dgrow.Cells[1].Value.ToString().Trim();
                    rowProductCode = dgrow.Cells[1].Value.ToString();
                    Boolean IsFOC = false;
                    if (dgrow.Cells[DGV.remark].Value == null)
                    {
                        IsFOC = false;
                    }
                    else if (dgrow.Cells[DGV.remark].Value.ToString() == "FOC")
                    {
                        IsFOC = true;
                    }

                    if (rowProductCode != string.Empty && dgrow.Cells[DGV.qty].Value != null)
                    {
                        //Get qty
                        Int32.TryParse(dgrow.Cells[DGV.qty].Value.ToString(), out qty);
                        entity = new POSEntities();
                        Product pro = productVar.Where(p => p.ProductCode == rowProductCode).FirstOrDefault<Product>();

                        decimal productDiscount = 0;
                        if (dgrow.Cells[DGV.dis].Value != null)
                        {
                            Decimal.TryParse(dgrow.Cells[DGV.dis].Value.ToString(), out productDiscount);
                        }
                        else
                        {
                            productDiscount = pro.DiscountRate;
                        }


                         // discount += (int)Math.Ceiling(getDiscountAmount(pro.Price, productDiscount) * qty);
                        if (!IsFOC)
                        {
                            total += (int)Math.Ceiling(getActualCost(pro, productDiscount, IsFOC) * qty);

                            discount += (int)Math.Ceiling(pro.Price * productDiscount / 100 * qty);
                        }
                        totalqty += qty;

                    }

                }

            }

            lblTotal.Text = total.ToString();
            lblDiscountTotal.Text = discount.ToString();
            lblTotalQty.Text = totalqty.ToString();//lblTicketTotal.Text;

        }

        private decimal getActualCost(Product prod, Boolean IsFOC)
        {
            decimal? actualCost = 0;

            //decrease discount ammount            
            if (IsFOC)
            {
                actualCost = 0;

            }
            else
            {
                actualCost = prod.Price - (prod.Price * prod.DiscountRate / 100);
            }
            actualCost = prod.Price - ((prod.Price / 100) * prod.DiscountRate);



            return (decimal)actualCost;
        }

        private decimal getActualCost(Product prod, decimal discountRate, Boolean IsFOC)
        {
            decimal? actualCost = 0;
            //decrease discount ammount            
            // actualCost = prod.Price - ((prod.Price / 100) * discountRate);
            if (IsFOC)
            {
                actualCost = 0;

            }
            else
            {
                actualCost = prod.Price - (prod.Price * discountRate / 100);
            }
            return (decimal)actualCost;
        }



        private decimal getDiscountAmount(decimal productPrice, decimal productDiscount)
        {
            return (((decimal)productPrice / 100) * productDiscount);
        }



        //private void FOC_Price(Product prod, Boolean IsFOC)
        //{
        //    if (IsFOC)
        //    {
        //        prod.Price = 0;
        //        prod.DiscountRate = 0;
        //    }
        //}

        public void LoadDraft(string TransactionId)
        {
            Clear();
            DraftId = TransactionId;

            entity = new POSEntities();
            Transaction draft = (from ts in entity.Transactions where ts.Id == TransactionId && ts.IsComplete == false select ts).FirstOrDefault<Transaction>();
            this.dgvSalesItem.CellValueChanged -= this.dgvSalesItem_CellValueChanged;
            if (draft != null)
            {
                var _tranDetails = (from a in entity.TransactionDetails where a.TransactionId == TransactionId select a).ToList();
                //pre add the rows
                dgvSalesItem.Rows.Insert(0, draft.TransactionDetails.Count());

                int index = 0;
                //foreach (TransactionDetail detail in draft.TransactionDetails)
                foreach (TransactionDetail detail in _tranDetails)
                {
                    //If product still exist
                    if (detail.Product != null)
                    {
                        isload = true;
                        DataGridViewRow row = dgvSalesItem.Rows[index];
                        //fill the current row with the product information
                        row.Cells[DGV.barcode].Value = detail.Product.Barcode;
                        row.Cells[DGV.pcode].Value = detail.Product.ProductCode;
                        row.Cells[DGV.pname].Value = detail.Product.Name;
                        row.Cells[DGV.qty].Value = detail.Qty;
                        // row.Cells[DGV.price].Value = detail.Product.Price;
                        row.Cells[DGV.price].Value = detail.UnitPrice;
                        row.Cells[DGV.dis].Value = detail.DiscountRate;
                        dynamicPrice.Add(new _dynamicPrice { dynamicProductCode = detail.Product.ProductCode, dynamicPrice = Convert.ToInt32(detail.UnitPrice) });
                        row.Cells[DGV.cost].Value = getActualCost(detail.Product, detail.DiscountRate, Convert.ToBoolean(detail.IsFOC)) * detail.Qty;
                        if (Convert.ToBoolean(detail.IsFOC) == true)
                        {
                            row.Cells[DGV.remark].Value = "FOC";
                        }
                        else
                        {
                            row.Cells[DGV.remark].Value = "";
                        }
                        row.Cells[DGV.pid].Value = detail.ProductId;
                        index++;
                    }
                }
                cboPaymentMethod.SelectedValue = draft.PaymentTypeId;


                txtAdditionalDiscount.Text = draft.DiscountAmount.ToString();


                UpdateTotalCost();


                //** delete draft transaction **
                Transaction delete_draft = (from trans in entity.Transactions where trans.Id == DraftId select trans).FirstOrDefault<Transaction>();

                delete_draft.TransactionDetails.Clear();
                var Detail = entity.TransactionDetails.Where(d => d.TransactionId == delete_draft.Id);
                foreach (var d in Detail)
                {
                    entity.TransactionDetails.Remove(d);
                }
                entity.Transactions.Remove(delete_draft);
                entity.SaveChanges();

            }
            else
            {
                //no associate transaction
                MessageBox.Show("The item doesn't exist anymore!");
            }

            isDraft = true;
            this.dgvSalesItem.CellValueChanged += this.dgvSalesItem_CellValueChanged;

        }

        public void Clear()
        {
            btnAdd1.Enabled = true;
            btnAdd1Minus.Enabled = true;
            btnAdd2.Enabled = true;
            btnAdd2Minus.Enabled = true;
            btnAdd3.Enabled = true;
            btnAdd3Minus.Enabled = true;
            btnAdd4.Enabled = true;
            btnAdd4Minus.Enabled = true;
            sangaticketcount = 0;
            lblSangaTicketStatus.Text = "0";
            entity = new POSEntities();
            dgvSalesItem.Rows.Clear();
            dgvSalesItem.Focus();
            txtAdditionalDiscount.Text = "0";
            lblTotal.Text = "0";
            lblDiscountTotal.Text = "0";
            lblTotalQty.Text = "0";
            isDraft = false;
            DraftId = string.Empty;
            dgvSearchProductList.DataSource = "";
            cboProductName.SelectedIndex = 0;
            //List<Product> productList = new List<Product>();
            //Product productObj = new Product();
            //productObj.Id = 0;
            //productObj.Name = "";
            //productList.Add(productObj);
            //productList.AddRange(productVar.ToList());
            //cboProductName.DataSource = productList;
            //cboProductName.DisplayMember = "Name";
            //cboProductName.ValueMember = "Id";
            //cboProductName.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            //cboProductName.AutoCompleteSource = AutoCompleteSource.ListItems;
            cboPaymentMethod.SelectedIndex = 0;
            txtBarcode.Clear();
            //Utility.BindProduct(cboProduct);
            _rowIndex = 0;
            dynamicPrice = new List<_dynamicPrice>();
            if (SettingController.TicketSale)
            {
                lblTicketTotal.Text = lblLocalAdult.Text = lblLocalChild.Text = lblForeignAdult.Text = lblForeignChild.Text = lblSangaTicketStatus.Text= "0";
            }
        }





        private void Cell_ReadOnly()
        {
            //if (_rowIndex != -1)
            //{
                DataGridViewRow row = dgvSalesItem.Rows[_rowIndex];
                if (_rowIndex > 0)
                {
                    if (row.Cells[1].Value != null)
                    {
                        string currentProductCode = row.Cells[DGV.pcode].Value.ToString();
                        List<string> _productList = dgvSalesItem.Rows
                               .OfType<DataGridViewRow>()
                               .Where(r => r.Cells[1].Value != null)
                               .Select(r => r.Cells[1].Value.ToString())
                               .ToList();

                        List<string> _checkProList = new List<string>();

                        _checkProList = (from p in _productList where p.Contains(currentProductCode) select p).ToList();
                        _checkProList.RemoveAt(_checkProList.Count - 1);
                        if (_checkProList.Count == 0)
                        {
                            //dgvSalesItem.Rows[_rowIndex].Cells[0].ReadOnly = true;
                            dgvSalesItem.Rows[_rowIndex].Cells[1].ReadOnly = true;
                            dgvSalesItem.Rows[_rowIndex].Cells[2].ReadOnly = true;
                        }
                    }

                }
                else
                {
                    //dgvSalesItem.Rows[_rowIndex].Cells[0].ReadOnly = true;
                    dgvSalesItem.Rows[_rowIndex].Cells[1].ReadOnly = true;
                    dgvSalesItem.Rows[_rowIndex].Cells[2].ReadOnly = true;
                }
            //}
            //else
            //{
            //    dgvSalesItem.Rows[0].Cells[0].ReadOnly = true;
            //    dgvSalesItem.Rows[0].Cells[1].ReadOnly = true;
            //    dgvSalesItem.Rows[0].Cells[2].ReadOnly = true;
            //}

            dgvSalesItem.CurrentCell = dgvSalesItem[1, dgvSalesItem.Rows.Count - 1];

        }

        private bool Check_ProductCode_Exist(string currentProductCode,decimal Price,decimal DiscountRate)
        {
            this.dgvSalesItem.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSalesItem_CellValueChanged);

            bool check = false;
            //     string currentProductCode = dgvSalesItem.Rows[_rowIndex].Cells[1].Value.ToString();
            List<int> _indexCount = (from r in dgvSalesItem.Rows.Cast<DataGridViewRow>()
                                     where r.Cells[DGV.pcode].Value != null && r.Cells[DGV.pcode].Value.ToString() == currentProductCode
                                       && (r.Cells[DGV.remark].Value.ToString() != null) && (r.Cells[DGV.remark].Value.ToString() != "FOC")
                                     select r.Index).ToList();
            //  }

            if (_indexCount.Count > 1)
            {
                _indexCount.RemoveAt(_indexCount.Count - 1);

                int index = (from r in dgvSalesItem.Rows.Cast<DataGridViewRow>()
                             where r.Cells[DGV.pcode].Value != null && r.Cells[DGV.pcode].Value.ToString() == currentProductCode
                             && (r.Cells[DGV.remark].Value.ToString() != "FOC")
                select r.Index).FirstOrDefault();


                curQty = Convert.ToInt32(dgvSalesItem.Rows[index].Cells[3].Value) + 1;
                dgvSalesItem.Rows[index].Cells[3].Value = curQty;
                dgvSalesItem.Rows[index].Cells[DGV.cost].Value = (Price - ((Price / 100) * DiscountRate)) * curQty;
                // dgvSalesItem.Rows.RemoveAt(dgvSalesItem.Rows.Count-2);
                BeginInvoke(new Action(delegate { dgvSalesItem.Rows.RemoveAt(dgvSalesItem.Rows.Count - 2); }));

                dgvSalesItem.Rows[dgvSalesItem.Rows.Count - 2].Cells[DGV.delete].Value = "delete";
                check = true;

            }
            this.dgvSalesItem.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSalesItem_CellValueChanged);
            return check;

        }

        #endregion

        private void dgvSalesItem_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            _rowIndex = e.RowIndex;
            if (e.RowIndex >= 0)
            {
               //FillServiceFee();
                DataGridViewRow row = dgvSalesItem.Rows[e.RowIndex];
                dgvSalesItem.CommitEdit(new DataGridViewDataErrorContexts());
                if (row.Cells[DGV.barcode].Value != null || row.Cells[DGV.pcode].Value != null || row.Cells[DGV.pname].Value != null)
                {
                    var value = row.Cells[DGV.remark].Value;
                    Boolean IsFOC = false;
                    if (row.Cells[7].Value == null)
                    {
                        IsFOC = false;
                    }
                    else if (row.Cells[7].Value.ToString() == "FOC")
                    {
                        IsFOC = true;
                    }
                    //New item code input
                    if (e.ColumnIndex == 0)
                    {
                        string currentBarcode = row.Cells[DGV.barcode].Value.ToString();

                        entity = new POSEntities();
                        Product pro = productVar.Where(p => p.Barcode == currentBarcode).FirstOrDefault<Product>();

                        if (pro != null)
                        {
                            //fill the current row with the product information

                            isload = true;
                            row.Cells[DGV.barcode].Value = pro.Barcode;
                            row.Cells[DGV.pcode].Value = pro.ProductCode;
                            row.Cells[DGV.pname].Value = pro.Name;
                            row.Cells[DGV.qty].Value = 1;

                            row.Cells[DGV.price].Value = pro.Price;
                            row.Cells[DGV.dis].Value = pro.DiscountRate;
                            row.Cells[DGV.cost].Value = getActualCost(pro, IsFOC);

                            if (IsFOC)
                            {
                                row.Cells[DGV.remark].Value = "FOC";
                            }
                            else
                            {
                                row.Cells[DGV.remark].Value = "";
                            }
                            row.Cells[DGV.pid].Value = pro.Id;

                        }
                        else
                        {
                            //remove current row if input have no associate product
                            MessageBox.Show("Wrong item code");
                            mssg = "Wrong";

                        }
                    }
                    //Product Code Change
                    else if (e.ColumnIndex == DGV.barcode || e.ColumnIndex == DGV.pcode)
                    {
                        string currentProductCode = "";
                        string currentProductName = "";
                        switch (e.ColumnIndex)
                        {
                            case 1:
                                currentProductCode = row.Cells[DGV.pcode].Value.ToString();
                                break;
                            case 2:
                                currentProductName = row.Cells[DGV.pname].Value.ToString();
                                break;
                        }

                        //get current product
                        entity = new POSEntities();
                        Product pro = productVar.Where(p => (currentProductCode != "" && p.ProductCode == currentProductCode) || (currentProductName != "" && p.Name == currentProductName)).FirstOrDefault<Product>(); 
                        if (pro != null)
                        {
                            //fill the current row with the product information

                            isload = true;
                            row.Cells[DGV.barcode].Value = pro.Barcode;
                            row.Cells[DGV.pcode].Value = pro.ProductCode;
                            row.Cells[DGV.pname].Value = pro.Name;
                            row.Cells[DGV.qty].Value = 1;
                            row.Cells[DGV.price].Value = pro.Price;
                            row.Cells[DGV.dis].Value = pro.DiscountRate;
                            row.Cells[DGV.cost].Value = getActualCost(pro, IsFOC);
                            if (IsFOC)
                            {
                                row.Cells[DGV.remark].Value = "FOC";
                            }
                            else
                            {
                                row.Cells[DGV.remark].Value = "";
                            }
                            row.Cells[DGV.pid].Value = pro.Id;

                        }
                        else
                        {
                            //remove current row if input have no associate product
                            MessageBox.Show("Wrong item code");


                        }

                        //check if current row isn't topmost
                        if (e.RowIndex > 0 && mssg == "")
                        {
                            Check_ProductCode_Exist(currentProductCode, pro.Price, pro.DiscountRate);
                        }

                    }
                    //Qty Changes
                    else if (e.ColumnIndex == 3)
                    {
                        int currentQty = 1;

                        if (isload == true)
                        {
                            string currentProductCode = row.Cells[DGV.pcode].Value.ToString();
                            //get current Project by Id
                            entity = new POSEntities();
                            Product pro = productVar.Where(p => p.ProductCode == currentProductCode).FirstOrDefault<Product>();


                            //int currentQty = 1;
                            try
                            {
                                //get updated qty
                                currentQty = Convert.ToInt32(row.Cells[DGV.qty].Value);
                                if (SettingController.TicketSale)
                                {
                                    if (Convert.ToInt16(lblLocalAdult.Tag) == pro.Id)
                                    {
                                        lblLocalAdult.Text = currentQty.ToString();
                                    }
                                    else if (Convert.ToInt16(lblLocalChild.Tag) == pro.Id)
                                    {
                                        lblLocalChild.Text = currentQty.ToString();
                                    }
                                    else if (Convert.ToInt16(lblForeignAdult.Tag) == pro.Id)
                                    {
                                        lblForeignAdult.Text = currentQty.ToString();
                                    }
                                    else if (Convert.ToInt16(lblForeignChild.Tag) == pro.Id)
                                    {
                                        lblForeignChild.Text = currentQty.ToString();
                                    }
                                    else if (Convert.ToInt16(lblSangaTicketStatus.Tag) == pro.Id)
                                    {
                                        sangaticketcount = currentQty;
                                        lblSangaTicketStatus.Text = currentQty.ToString();
                                    }
                                    lblTicketTotal.Text = Convert.ToString(Convert.ToInt16(lblLocalAdult.Text) + Convert.ToInt16(lblLocalChild.Text) + Convert.ToInt16(lblForeignAdult.Text) + Convert.ToInt16(lblForeignChild.Text) + Convert.ToInt16(lblSangaTicketStatus.Text));
                                }
                                if (currentQty.ToString() != null && currentQty != 0)
                                {
                                    row.DefaultCellStyle.BackColor = Color.White;
                                }
                                else
                                {

                                    row.DefaultCellStyle.BackColor = Color.Red;
                                }

                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Input quantity have invalid keywords.");
                                row.Cells[DGV.qty].Value = "1";
                            }


                            //update the total cost
                            row.Cells[DGV.cost].Value = currentQty * getActualCost(pro, IsFOC);
                            isload = false;
                        }
                        else
                        {
                            Decimal currentDiscountRate = 0;

                            int discountRate = 0;


                            currentDiscountRate = Convert.ToDecimal(row.Cells[5].Value);
                            // if (row.Cells[5].Value.ToString() != null &&row.Cells[DGV.dis].Value.ToString() != "0.00")
                            try
                            {
                                if (currentDiscountRate.ToString() != null && currentDiscountRate != 0)
                                {
                                    currentDiscountRate = Convert.ToDecimal(row.Cells[5].Value.ToString());
                                    discountRate = Convert.ToInt32(currentDiscountRate);
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Input Discount rate have invalid keywords.");
                                row.Cells[DGV.dis].Value = "0.00";
                            }

                            string currentProductCode = row.Cells[DGV.pcode].Value.ToString();



                            //get current Project by Id
                            entity = new POSEntities();
                            Product pro = productVar.Where(p => p.ProductCode == currentProductCode).FirstOrDefault<Product>();

                            currentQty = 1;
                            try
                            {
                                //get updated qty
                                currentQty = Convert.ToInt32(row.Cells[DGV.qty].Value);
                                if (SettingController.TicketSale)
                                {
                                    if (Convert.ToInt16(lblLocalAdult.Tag) == pro.Id)
                                    {
                                        lblLocalAdult.Text = currentQty.ToString();
                                    }
                                    else if (Convert.ToInt16(lblLocalChild.Tag) == pro.Id)
                                    {
                                        lblLocalChild.Text = currentQty.ToString();
                                    }
                                    else if (Convert.ToInt16(lblForeignAdult.Tag) == pro.Id)
                                    {
                                        lblForeignAdult.Text = currentQty.ToString();
                                    }
                                    else if (Convert.ToInt16(lblForeignChild.Tag) == pro.Id)
                                    {
                                        lblForeignChild.Text = currentQty.ToString();
                                    }
                                    else if (Convert.ToInt16(lblSangaTicketStatus.Tag) == pro.Id)
                                    {
                                        sangaticketcount = currentQty;
                                        lblSangaTicketStatus.Text = currentQty.ToString();
                                    }
                                    lblTicketTotal.Text = Convert.ToString(Convert.ToInt16(lblLocalAdult.Text) + Convert.ToInt16(lblLocalChild.Text) + Convert.ToInt16(lblForeignAdult.Text) + Convert.ToInt16(lblForeignChild.Text) + Convert.ToInt16(lblSangaTicketStatus.Text));
                                }

                                if (currentQty.ToString() != null && currentQty != 0)
                                {
                                    row.DefaultCellStyle.BackColor = Color.White;
                                }
                                else
                                {
                                    //row.DefaultCellStyle.BackColor = Color.Red;  
                                    if (SettingController.TicketSale)         //HMT
                                    {
                                        int currentProductId = Convert.ToInt32(dgvSalesItem[9, e.RowIndex].Value);
                                        entity = new POSEntities();
                                        Product prod = productVar.Where(p => p.Id == currentProductId).FirstOrDefault<Product>();
                                        if (Convert.ToInt16(lblLocalAdult.Tag) == prod.Id)
                                        {
                                            lblLocalAdult.Text = "0";
                                        }
                                        else if (Convert.ToInt16(lblLocalChild.Tag) == prod.Id)
                                        {
                                            lblLocalChild.Text = "0";
                                        }
                                        else if (Convert.ToInt16(lblForeignAdult.Tag) == prod.Id)
                                        {
                                            lblForeignAdult.Text = "0";
                                        }
                                        else if (Convert.ToInt16(lblForeignChild.Tag) == prod.Id)
                                        {
                                            lblForeignChild.Text = "0";
                                        }
                                        else if (Convert.ToInt16(lblSangaTicketStatus.Tag) == pro.Id)
                                        {
                                            sangaticketcount = 0;
                                            lblSangaTicketStatus.Text = "0";
                                        }
                                        lblTicketTotal.Text = Convert.ToString(Convert.ToInt16(lblLocalAdult.Text) + Convert.ToInt16(lblLocalChild.Text) + Convert.ToInt16(lblForeignAdult.Text) + Convert.ToInt16(lblForeignChild.Text) + Convert.ToInt16(lblSangaTicketStatus.Text));
                                    }

                                    dgvSalesItem.Rows.RemoveAt(e.RowIndex);
                                    UpdateTotalCost();
                                    dgvSalesItem.CurrentCell = dgvSalesItem[1, e.RowIndex];   //HMT
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Input quantity have invalid keywords.");
                                row.Cells[DGV.qty].Value = "1";
                            }

                            //update the total cost
                            //     row.Cells[DGV.cost].Value = currentQty * getActualCost(pro,discountRate);
                            row.Cells[DGV.cost].Value = currentQty * getActualCost(pro, discountRate, IsFOC);
                            return;
                        }

                    }
                    else if (e.ColumnIndex == 4)
                    {
                        Decimal currentDiscountRate = 0;

                        int discountRate = 0;


                        currentDiscountRate = Convert.ToDecimal(row.Cells[5].Value);
                        // if (row.Cells[5].Value.ToString() != null &&row.Cells[DGV.dis].Value.ToString() != "0.00")
                        try
                        {
                            if (currentDiscountRate.ToString() != null && currentDiscountRate != 0)
                            {
                                currentDiscountRate = Convert.ToDecimal(row.Cells[5].Value.ToString());
                                discountRate = Convert.ToInt32(currentDiscountRate);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Input Discount rate have invalid keywords.");
                            row.Cells[DGV.dis].Value = "0.00";
                        }

                        string currentProductCode = row.Cells[DGV.pcode].Value.ToString();



                        //get current Project by Id
                        entity = new POSEntities();
                        Product pro = productVar.Where(p => p.ProductCode == currentProductCode).FirstOrDefault<Product>();


                        var currentQty = 1;
                        try
                        {
                            //get updated qty
                            currentQty = Convert.ToInt32(row.Cells[DGV.qty].Value);
                            if (currentQty.ToString() != null && currentQty != 0)
                            {
                                row.DefaultCellStyle.BackColor = Color.White;
                            }
                            else
                            {
                                row.DefaultCellStyle.BackColor = Color.Red;
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Input quantity have invalid keywords.");
                            row.Cells[DGV.qty].Value = "1";
                        }

                        if (row.Cells[DGV.price].Value.ToString() != null && Convert.ToInt32(row.Cells[DGV.price].Value) != 0)
                        {


                            if (dynamicPrice.Where(p => p.dynamicProductCode == pro.ProductCode).FirstOrDefault() == null)
                            {
                                dynamicPrice.Add(new _dynamicPrice { dynamicProductCode = pro.ProductCode, dynamicPrice = Convert.ToInt32(row.Cells[DGV.price].Value) });
                            }
                            else
                            {
                                dynamicPrice.Where(p => p.dynamicProductCode == pro.ProductCode).First().dynamicPrice = Convert.ToInt32(row.Cells[DGV.price].Value);
                            }

                            row.Cells[DGV.cost].Value = currentQty * getActualCost(pro, discountRate, IsFOC);
                            return;
                        }
                        else
                        {
                            MessageBox.Show("Input price is invalid");
                            row.Cells[DGV.price].Value = pro.Price;
                            return;
                        }
                    }

                    //Discount Rate Change
                    else if (e.ColumnIndex == 5)
                    {
                        string currentProductCode = row.Cells[DGV.pcode].Value.ToString();
                        //get current Project by Id
                        entity = new POSEntities();
                        Product pro = productVar.Where(p => p.ProductCode == currentProductCode).FirstOrDefault<Product>();


                        int currentQty = 1;
                        try
                        {
                            //get updated qty
                            currentQty = Convert.ToInt32(row.Cells[DGV.qty].Value);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Input quantity have invalid keywords.");
                            row.Cells[DGV.qty].Value = "1";
                        }

                        decimal DiscountRate = 0;
                        try
                        {
                            //get updated qty
                            // Decimal.TryParse(row.Cells[5].Value.ToString(), out DiscountRate);
                            DiscountRate = Convert.ToDecimal(row.Cells[5].Value);
                            if (DiscountRate > 100)
                            {
                                row.Cells[DGV.dis].Value = 100;
                                DiscountRate = 100;
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Input Discount rate have invalid keywords.");
                            row.Cells[DGV.dis].Value = "0.00";
                        }


                        //update the total cost
                        row.Cells[DGV.cost].Value = currentQty * getActualCost(pro, DiscountRate, IsFOC);

                    }
                    if (mssg == "")
                    {
                        Cell_ReadOnly();
                    }

                    UpdateTotalCost();
                }
                else
                {
                    //dgvSalesItem.Rows.RemoveAt(e.RowIndex);

                    dgvSalesItem.CurrentCell = dgvSalesItem[1, e.RowIndex];
                    MessageBox.Show("You need to input product code or barcode or product name firstly in order to add product quantity!");
                    mssg = "Wrong";
                }
            }
        }
        internal Boolean DeleteCopy(string TransactionId)
        {
            this.dgvSalesItem.CellValueChanged -= this.dgvSalesItem_CellValueChanged;
            Boolean IsContinue = true; Boolean IsFormClosing = true;

            //Transaction draft = (from ts in entity.Transactions where ts.Id == TransactionId select ts).FirstOrDefault<Transaction>();
            Transaction draft = (from ts in entity.Transactions where ts.Id == TransactionId select ts).FirstOrDefault();

            if (IsContinue)
            {
                Clear();
                entity = new POSEntities();
                draft = (from ts in entity.Transactions where ts.Id == TransactionId select ts).FirstOrDefault();
                draft.IsDeleted = true;
                DraftId = TransactionId;
                decimal disTotal = 0;
                //Delete transaction
                // draft.IsDeleted = true;


                //  List<TransactionDetail> tempTransactionDetaillist = entity.TransactionDetails.Where(x => x.IsDeleted == false).ToList();

                // add gift card amount

                var list = draft.TransactionDetails.ToList();
                foreach (TransactionDetail detail in draft.TransactionDetails.Where(x => x.IsDeleted == false))
                {
                    detail.IsDeleted = true;

                    entity.SaveChanges();
                }


                //DeleteLog dl = new DeleteLog();
                //dl.DeletedDate = DateTime.Now;
                //dl.CounterId = MemberShip.CounterId;
                //dl.UserId = MemberShip.UserId;
                //dl.IsParent = true;
                //dl.TransactionId = draft.Id;

                //entity.DeleteLogs.Add(dl);
                //entity.SaveChanges();

                //copy transaction
                if (draft != null)
                {

                    //pre add the rows
                    // dgvSalesItem.Rows.Insert(0, draft.TransactionDetails.Count());
                    dgvSalesItem.Rows.Insert(0, list.Count());
                    int index1 = 0;
                    // foreach (TransactionDetail detail in draft.TransactionDetails)
                    foreach (TransactionDetail detail in list)
                    {
                        //If product still exist
                        if (detail.Product != null)
                        {
                            DataGridViewRow row = dgvSalesItem.Rows[index1];
                            //fill the current row with the product information
                            row.Cells[DGV.barcode].Value = detail.Product.Barcode;
                            row.Cells[DGV.pcode].Value = detail.Product.ProductCode;
                            row.Cells[DGV.pname].Value = detail.Product.Name;
                            row.Cells[DGV.qty].Value = detail.Qty;

                            row.Cells[DGV.price].Value = detail.UnitPrice;
                            // FOC_Price(detail.Product, detail.IsFOC);
                            // row.Cells[DGV.price].Value = Utility.WholeSalePriceOrSellingPrice(detail.Product,Convert.ToBoolean(draft.IsWholeSale));
                            row.Cells[DGV.dis].Value = detail.DiscountRate;
                            
                            if (Convert.ToBoolean(detail.IsFOC) == true)
                            {
                                row.Cells[DGV.remark].Value = "FOC";
                                row.Cells[DGV.cost].Value = getActualCost(detail.Product, true) * detail.Qty;

                            }
                            else
                            {
                                row.Cells[DGV.remark].Value = "";
                                row.Cells[DGV.cost].Value = (detail.Product.Price * detail.Qty);

                            }
                            disTotal += Convert.ToInt64(getDiscountAmount(Convert.ToInt64(detail.UnitPrice), detail.DiscountRate) * detail.Qty);
                            row.Cells[DGV.pid].Value = detail.ProductId;
                            index1++;
                        }
                    }
                    cboPaymentMethod.SelectedValue = draft.PaymentTypeId;
                    txtAdditionalDiscount.Text = (draft.DiscountAmount - disTotal).ToString();
                    UpdateTotalCost();

                }
            }
            else
            {
                IsFormClosing = false;
            }
            this.dgvSalesItem.CellValueChanged += this.dgvSalesItem_CellValueChanged;
            return IsFormClosing;
        }

        private decimal getActualCost(long productPrice, decimal productDiscount)
        {
            decimal? actualCost = 0;

            //decrease discount ammount            
            actualCost = productPrice - ((productPrice / 100) * productDiscount);
            return (decimal)actualCost;
        }

       


        private void txtAdditionalDiscount_Leave(object sender, EventArgs e)
        {
            //Check_MType();//SD
            UpdateTotalCost();
        }

        private void txtAdditionalDiscount_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.Equals(Keys.Enter))
            {
                // Check_MType();//SD
                UpdateTotalCost();
                SendKeys.Send("{TAB}");

            }
        }

        private void txtNO_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void cboPaymentMethod_TextChanged(object sender, EventArgs e)
        {
            //  Check_MType();  SD
            UpdateTotalCost();
        }

        private void dgvSalesItem_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                //FillServiceFee();
                DataGridViewRow row = dgvSalesItem.Rows[e.RowIndex];
                dgvSalesItem.CommitEdit(new DataGridViewDataErrorContexts());
                if (row.Cells[DGV.barcode].Value == null && row.Cells[DGV.pcode].Value == null && row.Cells[DGV.pname].Value == null && row.Cells[DGV.dis].Value == null && row.Cells[DGV.qty].Value == null && row.Cells[DGV.price].Value == null)
                {
                    if (row.Cells[8].Value != null)
                    {
                        BeginInvoke(new Action(delegate { dgvSalesItem.Rows.RemoveAt(e.RowIndex); }));
                        //dgvSalesItem.CurrentCell = dgvSalesItem[0, e.RowIndex];
                    }
                }
                else if (mssg == "Wrong")
                {
                    if (row.Cells[8].Value != null)
                    {
                        BeginInvoke(new Action(delegate { dgvSalesItem.Rows.RemoveAt(e.RowIndex); }));
                        if (row.Cells[DGV.barcode].Value != null)
                        {
                            dgvSalesItem.CurrentCell = dgvSalesItem[0, e.RowIndex];
                        }
                        else if (row.Cells[1].Value != null)
                        {
                            dgvSalesItem.CurrentCell = dgvSalesItem[1, e.RowIndex];
                        }
                        mssg = "";
                    }
                }
            }
        }

        private void dgvSalesItem_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress -= new KeyPressEventHandler(colQty_KeyPress);
            // prodCode.TextChanged -= new EventHandler(colProductName_TextChanged);
            // if (dgvSalesItem.CurrentCell.ColumnIndex == 2)
            if (dgvSalesItem.CurrentCell.OwningColumn.Name.Equals("colProductName"))
            {
                // TextBox prodCode = new TextBox();
                prodCode = e.Control as TextBox;
                string text = prodCode.Text;
                if (prodCode != null)
                {
                    prodCode.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                    prodCode.AutoCompleteCustomSource = AutoCompleteLoad();
                    prodCode.AutoCompleteSource = AutoCompleteSource.CustomSource;


                }
            }


            else if (dgvSalesItem.CurrentCell.OwningColumn.Name.Equals("colQty")) //Desired Column
            {
                TextBox tb = e.Control as TextBox;
                if (tb != null)
                {
                    prodCode.AutoCompleteCustomSource = null;
                    tb.KeyPress += new KeyPressEventHandler(colQty_KeyPress);
                }
            }
            else if (dgvSalesItem.CurrentCell.OwningColumn.Name.Equals("colBarCode") || dgvSalesItem.CurrentCell.OwningColumn.Name.Equals("colProductCode")) //Desired Column
            {
                prodCode.AutoCompleteCustomSource = null;
            }
        }

        private void colQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        public AutoCompleteStringCollection AutoCompleteLoad()
        {
            AutoCompleteStringCollection str = new AutoCompleteStringCollection();

            var product = productVar.Select(x => x.Name).ToList();

            foreach (var p in product)
            {
                str.Add(p.ToString());
            }
            return str;
        }

        private void btnFOC_Click(object sender, EventArgs e)
        {

            txtBarcode.Focus();
            gbFOC.Visible = true;
            // Utility.BindProduct(cboProduct);
            cboProduct.SelectedIndex = 0;
            txtBarcode.Clear();
            cboProduct.Focus();
        }

        private void Clear_FOC()
        {
            txtBarcode.Clear();
            cboProduct.SelectedIndex = 0;
            txtQty.Text = "0";
        }

        internal bool Add_DataToGrid(int currentProductId)
        {
            Product _productInfo = productVar.Where(p => p.Id == currentProductId).FirstOrDefault<Product>();
            if (_productInfo != null)
            {
                int count = dgvSalesItem.Rows.Count;
                DataGridViewRow row = (DataGridViewRow)dgvSalesItem.Rows[count - 1].Clone();
                row.Cells[DGV.barcode].Value = _productInfo.Barcode;
                row.Cells[DGV.pcode].Value = _productInfo.ProductCode;
                row.Cells[DGV.pname].Value = _productInfo.Name;
                row.Cells[DGV.qty].Value = FOCQty;
                row.Cells[DGV.price].Value = 0;
                row.Cells[DGV.dis].Value = 0;
                row.Cells[DGV.cost].Value = 0;
                row.Cells[DGV.remark].Value = "FOC";
                row.Cells[DGV.pid].Value = _productInfo.Id;

                dgvSalesItem.Rows.Add(row);
                _rowIndex = dgvSalesItem.Rows.Count - 2;
                //cboProductName.SelectedIndex = 0;
                //dgvSearchProductList.DataSource = "";
                //dgvSearchProductList.ClearSelection();
                dgvSalesItem.Focus();
                //var list = dgvSalesItem.DataSource;
                UpdateTotalCost();

                Check_ProductFOCCode_Exist(_productInfo.ProductCode);

                Cell_ReadOnly();
            }


             return true;
        }

        private bool Check_ProductFOCCode_Exist(string currentProductCode)
        {
            bool check = false;
            //     string currentProductCode = dgvSalesItem.Rows[_rowIndex].Cells[1].Value.ToString();
            List<int> _indexCount = (from r in dgvSalesItem.Rows.Cast<DataGridViewRow>()
                                     where r.Cells[1].Value != null && r.Cells[1].Value.ToString() == currentProductCode
                                     && (r.Cells[7].Value.ToString() == "FOC")
                                     select r.Index).ToList();
            //  }

            if (_indexCount.Count > 1)
            {
                _indexCount.RemoveAt(_indexCount.Count - 1);

                int index = (from r in dgvSalesItem.Rows.Cast<DataGridViewRow>()
                             where r.Cells[1].Value != null && r.Cells[1].Value.ToString() == currentProductCode
                              && (r.Cells[7].Value.ToString() == "FOC")
                             select r.Index).FirstOrDefault();




                dgvSalesItem.Rows[index].Cells[3].Value = Convert.ToInt32(dgvSalesItem.Rows[index].Cells[3].Value) + FOCQty;
                // dgvSalesItem.Rows.RemoveAt(dgvSalesItem.Rows.Count-2);
                BeginInvoke(new Action(delegate { dgvSalesItem.Rows.RemoveAt(dgvSalesItem.Rows.Count - 2); }));

                dgvSalesItem.Rows[dgvSalesItem.Rows.Count - 2].Cells[DGV.delete].Value = "delete";
                check = true;

            }
            return check;
        }

        private void txtBarcode_Leave(object sender, EventArgs e)
        {
            Barcode_Input();
        }

        private void Barcode_Input()
        {
            string _barcode = txtBarcode.Text;
            if(string.IsNullOrEmpty(_barcode))
            {
                return;
            }
            long productId = productVar.Where(p => p.Barcode == _barcode).FirstOrDefault<Product>().Id;
            cboProduct.SelectedValue = productId;
            cboProduct.Focus();
        }

        private void cboProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboProduct.SelectedIndex > 0)
            {
                long productId = Convert.ToInt32(cboProduct.SelectedValue);
                string barcode = productVar.Where(p => p.Id == productId).FirstOrDefault<Product>().Barcode;
                txtBarcode.Text = barcode;
                txtQty.Text = "1";
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtBarcode.Text.Trim() != string.Empty && cboProduct.SelectedIndex > 0 && txtQty.Text.Trim() != string.Empty && Convert.ToInt32(txtQty.Text) > 0)
            {
                FOCQty = Convert.ToInt32(txtQty.Text);
                int _proId = Convert.ToInt32(cboProduct.SelectedValue);
                Add_DataToGrid(_proId);
                Clear_FOC();
            }
            // gbFOC.Visible = false;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            gbFOC.Visible = false;
        }

        private void btnnote_Click(object sender, EventArgs e)
        {
            if (note == "")
            {
                AddNote form = new AddNote();
                form.status = "ADD";

                form.ShowDialog();
            }
            else
            {
                AddNote form = new AddNote();
                form.status = "EDIT";
                form.editnote = note;
                form.ShowDialog();
            }

        }

        private void cboProductName_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnAdd1_Click(object sender, EventArgs e)
        {
            if (btnAdd1.Tag == null || Convert.ToInt16(btnAdd1.Tag) == 0
                || btnAdd1Minus.Tag == null || Convert.ToInt16(btnAdd1Minus.Tag) == 0 ||
               lblLocalAdult.Tag == null || Convert.ToInt16(lblLocalAdult.Tag) == 0 ||
               btnAdd2.Tag == null || Convert.ToInt16(btnAdd2.Tag) == 0 ||
                btnAdd2Minus.Tag == null || Convert.ToInt16(btnAdd2Minus.Tag) == 0 ||
               lblLocalChild.Tag == null || Convert.ToInt16(lblLocalChild.Tag) == 0 ||
                btnAdd3.Tag == null || Convert.ToInt16(btnAdd3.Tag) == 0 ||
                btnAdd3Minus.Tag == null || Convert.ToInt16(btnAdd3Minus.Tag) == 0 ||
                lblForeignAdult.Tag == null || Convert.ToInt16(lblForeignAdult.Tag) == 0 ||
                btnAdd4.Tag == null || Convert.ToInt16(btnAdd4.Tag) == 0 ||
                btnAdd4Minus.Tag == null || Convert.ToInt16(btnAdd4Minus.Tag) == 0 ||
                lblForeignChild.Tag == null || Convert.ToInt16(lblForeignChild.Tag) == 0)
            {
                if (MessageBox.Show(this, "Please assign product to Ticket Buttons.", "mPOS Ticket Module", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                {
                    AssignTicketButton newform = new AssignTicketButton();
                    newform.Show();
                }
                else
                {
                    return;
                }



            }
            int currentProductId = 0;

            Button senter = (Button)sender;
            switch (senter.Name)
            {
                case "btnAdd1":
                    currentProductId = Convert.ToInt16(btnAdd1.Tag);
                    lblLocalAdult.Text = (Convert.ToInt16(lblLocalAdult.Text) + 1).ToString();
                    lblTicketTotal.Text = (Convert.ToInt16(lblTicketTotal.Text) + 1).ToString();
                    break;
                case "btnAdd2":
                    currentProductId = Convert.ToInt16(btnAdd2.Tag);
                    lblLocalChild.Text = (Convert.ToInt16(lblLocalChild.Text) + 1).ToString();
                    lblTicketTotal.Text = (Convert.ToInt16(lblTicketTotal.Text) + 1).ToString();
                    break;

                case "btnAdd3":
                    currentProductId = Convert.ToInt16(btnAdd3.Tag);
                    lblForeignAdult.Text = (Convert.ToInt16(lblForeignAdult.Text) + 1).ToString();
                    lblTicketTotal.Text = (Convert.ToInt16(lblTicketTotal.Text) + 1).ToString();
                    break;

                case "btnAdd4":
                    currentProductId = Convert.ToInt16(btnAdd4.Tag);
                    lblForeignChild.Text = (Convert.ToInt16(lblForeignChild.Text) + 1).ToString();
                    lblTicketTotal.Text = (Convert.ToInt16(lblTicketTotal.Text) + 1).ToString();
                    break;

                default: break;
            }
            //FillServiceFee();

            int count = dgvSalesItem.Rows.Count;

            Product pro = productVar.Where(p => p.Id == currentProductId).FirstOrDefault<Product>();
            if (pro != null)
            {

                DataGridViewRow row = (DataGridViewRow)dgvSalesItem.Rows[count - 1].Clone();
                row.Cells[DGV.barcode].Value = pro.Barcode;
                row.Cells[DGV.pcode].Value = pro.ProductCode;
                row.Cells[DGV.pname].Value = pro.Name;
                row.Cells[DGV.qty].Value = 1;
                row.Cells[DGV.price].Value = pro.Price;
                row.Cells[DGV.dis].Value = pro.DiscountRate;
                row.Cells[DGV.cost].Value = getActualCost(pro.Price,pro.DiscountRate);
                row.Cells[DGV.remark].Value = "";
                row.Cells[DGV.pid].Value = currentProductId;

                dgvSalesItem.Rows.Add(row);
                _rowIndex = dgvSalesItem.Rows.Count - 2;
                cboProductName.SelectedIndex = 0;
                dgvSearchProductList.DataSource = "";
                dgvSearchProductList.ClearSelection();
                dgvSalesItem.Focus();
                UpdateTotalCost();

                // var list = dgvSalesItem.DataSource;
                Check_ProductCode_Exist(pro.ProductCode,pro.Price, pro.DiscountRate);
               

                //dynamicPrice = new List<_dynamicPrice>();
                Cell_ReadOnly();

            }
            else
            {

                MessageBox.Show("Item not found!", "Cannot find");
            }

            

        }

        private void btnAdd1Minus_Click(object sender, EventArgs e)
        {
            int currentProductId = 0;
            Button senter = (Button)sender;
            
            switch (senter.Name)
            {
                case "btnAdd1Minus":
                    currentProductId = Convert.ToInt16(btnAdd1Minus.Tag);
                    lblLocalAdult.Text = (Convert.ToInt16(lblLocalAdult.Text) - 1).ToString();
                    //lblTicketTotal.Text = (Convert.ToInt16(lblTicketTotal.Text) - 1).ToString();
                    break;
                case "btnAdd2Minus":
                    currentProductId = Convert.ToInt16(btnAdd2Minus.Tag);
                    lblLocalChild.Text = (Convert.ToInt16(lblLocalChild.Text) - 1).ToString();
                    //lblTicketTotal.Text = (Convert.ToInt16(lblTicketTotal.Text) - 1).ToString();
                    break;
                case "btnAdd3Minus":
                    currentProductId = Convert.ToInt16(btnAdd3Minus.Tag);
                    lblForeignAdult.Text = (Convert.ToInt16(lblForeignAdult.Text) - 1).ToString();
                    //lblTicketTotal.Text = (Convert.ToInt16(lblTicketTotal.Text) - 1).ToString();
                    break;
                case "btnAdd4Minus":
                    currentProductId = Convert.ToInt16(btnAdd4Minus.Tag);
                    lblForeignChild.Text = (Convert.ToInt16(lblForeignChild.Text) - 1).ToString();
                    //lblTicketTotal.Text = (Convert.ToInt16(lblTicketTotal.Text) - 1).ToString();
                    break;

                default: break;
            }
            if (Convert.ToInt16(lblTicketTotal.Text) < 0)
            {
                lblTicketTotal.Text = "0";
            }
            if (Convert.ToInt16(lblLocalAdult.Text) < 0)
            {
                lblLocalAdult.Text = "0";
            }
            if (Convert.ToInt16(lblLocalChild.Text) < 0)
            {
                lblLocalChild.Text = "0";
            }
            if (Convert.ToInt16(lblForeignAdult.Text) < 0)
            {
                lblForeignAdult.Text = "0";
            }
            if (Convert.ToInt16(lblForeignChild.Text) < 0)
            {
                lblForeignChild.Text = "0";
            }

            lblTicketTotal.Text = Convert.ToString(Convert.ToInt16(lblLocalAdult.Text) + Convert.ToInt16(lblLocalChild.Text) + Convert.ToInt16(lblForeignAdult.Text) + Convert.ToInt16(lblForeignChild.Text));

            var currentBarcode = productVar.Where(p => p.Id == currentProductId).FirstOrDefault<Product>().Barcode;
            var rows = dgvSalesItem.Rows;
            if (rows.Count > 0)
            {
                foreach (DataGridViewRow r in rows)
                {
                    if (r.Cells[DGV.barcode].Value != null && r.Cells[DGV.barcode].Value.ToString() == currentBarcode && Convert.ToInt16(r.Cells[DGV.qty].Value) > 0)
                    {
                        r.Cells[DGV.qty].Value = (int)r.Cells[DGV.qty].Value - 1;
                    }
                    else if (r.Cells[DGV.barcode].Value != null && r.Cells[DGV.barcode].Value.ToString() == currentBarcode && Convert.ToInt16(r.Cells[DGV.qty].Value) <= 0)
                    {
                        dgvSalesItem.Rows.Remove(r);
                    }

                }
            }
            else
            {
                MessageBox.Show("No Items in List");
            }
            UpdateTotalCost();

        }


        private void btnSangaAdd_Click(object sender, EventArgs e)
        {
            sangaticketcount++;
            this.lblSangaTicketStatus.Text = sangaticketcount.ToString();
            //adding the recrod to the grid
            addSangaTicketRecrodToGird();
            lblTicketTotal.Text = lblTotalQty.Text;
        }

        private void addSangaTicketRecrodToGird()
        {
            int count = dgvSalesItem.Rows.Count;

            entity = new POSEntities();
            Product pro = productVar.Where(p => p.Id == 9).FirstOrDefault<Product>();
            if (pro != null)
            {

                DataGridViewRow row = (DataGridViewRow)dgvSalesItem.Rows[count - 1].Clone();
                row.Cells[DGV.barcode].Value = pro.Barcode;
                row.Cells[DGV.pcode].Value = pro.ProductCode;
                row.Cells[DGV.pname].Value = pro.Name;
                row.Cells[DGV.qty].Value = 1;
                //row.Cells[DGV.price].Value = pro.Price;
                row.Cells[DGV.price].Value = pro.Price; 
                row.Cells[DGV.dis].Value = pro.DiscountRate;
                row.Cells[DGV.cost].Value = pro.Price;
                row.Cells[DGV.remark].Value = "";
                row.Cells[DGV.pid].Value = pro.Id;

                dgvSalesItem.Rows.Add(row);
                _rowIndex = dgvSalesItem.Rows.Count - 2;
                cboProductName.SelectedIndex = 0;
                dgvSearchProductList.DataSource = "";
                dgvSearchProductList.ClearSelection();
                dgvSalesItem.Focus();
                // var list = dgvSalesItem.DataSource;
                Check_ProductCode_Exist(pro.ProductCode,pro.Price, pro.DiscountRate);
                //dynamicPrice = new List<_dynamicPrice>();
                Cell_ReadOnly();

            }
            else
            {

                MessageBox.Show("Item not found!", "Cannot find");
            }

            UpdateTotalCost();
        }

        private void btnSangaMinus_Click(object sender, EventArgs e)
        {
            if (sangaticketcount > 0)
            {
                sangaticketcount--;
                this.lblSangaTicketStatus.Text = sangaticketcount.ToString();

                //Removing the data to the gird
                removeSangaTicketRecrodFromGrid();
                int totalQty = Convert.ToInt32(lblTotalQty.Text);
                lblTicketTotal.Text = (totalQty).ToString();
            }

        }
        private void removeSangaTicketRecrodFromGrid()
        {
            var currentBarcode = productVar.Where(p => p.Id == 9).FirstOrDefault<Product>().Barcode;
            var rows = dgvSalesItem.Rows;
            if (rows.Count > 0)
            {
                foreach (DataGridViewRow r in rows)
                {
                    if (r.Cells[DGV.barcode].Value != null && r.Cells[DGV.barcode].Value.ToString() == currentBarcode && Convert.ToInt16(r.Cells[DGV.qty].Value) > 0)
                    {
                        r.Cells[DGV.qty].Value = (int)r.Cells[DGV.qty].Value - 1;
                    }
                    else if (r.Cells[DGV.barcode].Value != null && r.Cells[DGV.barcode].Value.ToString() == currentBarcode && Convert.ToInt16(r.Cells[DGV.qty].Value) <= 0)
                    {
                        dgvSalesItem.Rows.Remove(r);
                    }

                }
            }
            else
            {
                MessageBox.Show("No Items in List");
            }
            UpdateTotalCost();
        }
    }
    class DGV
    {
        public static int barcode = 0;
        public static int pcode = 1;
        public static int pname = 2;
        public static int qty = 3;
        public static int price = 4;
        public static int dis = 5;
        public static int cost = 6;
        public static int remark = 7;
        public static int delete = 8;
        public static int pid = 9;

    }
}
