using POS.APP_Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Objects;
using System.Linq;
using System.Windows.Forms;

namespace POS
{
    public partial class TransactionList : Form
    {
        #region Variables

        private POSEntities entity = new POSEntities();
       
       // List<Stock_Transaction> productList = new List<Stock_Transaction>();

        public int index = 0;
        private Boolean IsStart = false;
        public string note;
        #endregion

        #region Event

        public TransactionList()
        {
            InitializeComponent();
        }

        private void TransactionList_Load(object sender, EventArgs e)
        {
            dgvTransactionList.AutoGenerateColumns = false;
            Localization.Localize_FormControls(this);

            SettingController.SetGlobalDateFormat(dtpFrom);
            SettingController.SetGlobalDateFormat(dtpTo);
            Utility.BindShop(cboshoplist);
            cboshoplist.Text = SettingController.DefaultShop.ShopName;
            Utility.ShopComBo_EnableOrNot(cboshoplist);
            IsStart = true;
            //if (SettingController.TicketSale)
            //{
                dgvTransactionList.Columns[11].Visible = true;
            //}
            
           LoadData();
        }

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
            //LoadData();
        }

        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {
           // LoadData();
        }

        private void rdbDate_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbDate.Checked)
            {
                gbDate.Enabled = true;
                gbId.Enabled = false;
                txtId.Clear();
            }
            else
            {
                gbDate.Enabled = false;
                gbId.Enabled = true;
            }
            //LoadData();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            LoadData();

        }

        private void dgvTransactionList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            index = e.RowIndex;
            if (e.RowIndex >= 0)
            {
                string currentTransactionId = dgvTransactionList.Rows[e.RowIndex].Cells[0].Value.ToString();

                List<int> type = new List<int> { 3, 4, 5, 6 }; 
                //View Detail
                if (e.ColumnIndex == 7)
                {
                   TransactionDetailForm newForm = new TransactionDetailForm();
                        newForm.transactionId = currentTransactionId;
                        newForm.shopid = Convert.ToInt32(cboshoplist.SelectedValue);
                        newForm.ShowDialog();
                }
                //Delete the record and add delete log
                else if (e.ColumnIndex == 8)
                {
                    //Role Management
                    RoleManagementController controller = new RoleManagementController();
                    controller.Load(MemberShip.UserRoleId);
                    if (controller.Transaction.EditOrDelete || MemberShip.isAdmin)
                    {
                        #region Delete
                        Transaction ts = entity.Transactions.Where(x => x.Id == currentTransactionId).FirstOrDefault();
                        if (ts != null)
                        {
                            DialogResult result = MessageBox.Show("Are you sure you want to delete?", "Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                            if (result.Equals(DialogResult.OK))
                            {

                                ts.IsDeleted = true;
                                ts.UpdatedDate = DateTime.Now;
                                // add gift card amount

                                foreach (TransactionDetail detail in ts.TransactionDetails)
                                {
                                    //detail.IsDeleted = false;
                                    detail.IsDeleted = true;


                                    //For Ticket
                                    Ticket ti = entity.Tickets.Where(x => x.TransactionDetailId == detail.Id).FirstOrDefault();
                                    if (ti != null)
                                    {
                                        ti.isDelete = true;
                                        ti.DeletedDate = DateTime.Now;
                                        int uid = MemberShip.UserId;
                                        var cUser = entity.Users.Find(uid);
                                        ti.UserName = cUser.Name;
                                        entity.Entry(ti).State = EntityState.Modified;
                                        entity.SaveChanges();
                                    }
                                }
                                string date = dgvTransactionList.Rows[e.RowIndex].Cells[3].Value.ToString();
                                DateTime _Trandate = Utility.Convert_Date(date);
                                DeleteLog dl = new DeleteLog();
                                dl.DeletedDate = DateTime.Now;
                                dl.CounterId = MemberShip.CounterId;
                                dl.UserId = MemberShip.UserId;
                                dl.IsParent = true;
                                dl.TransactionId = ts.Id;

                                entity.DeleteLogs.Add(dl);

                                entity.SaveChanges();

                                LoadData();

                                if (System.Windows.Forms.Application.OpenForms["chart"] != null)
                                {
                                    chart newForm = (chart)System.Windows.Forms.Application.OpenForms["chart"];
                                    newForm.FormFresh();
                                }
                            }

                        }
                        #endregion
                    }
                    else
                    {
                        MessageBox.Show("You are not allowed to delete transaction information", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }

                //modify delete and copy (zp)
                else if (e.ColumnIndex == 9)
                {
                    //Role Management
                    RoleManagementController controller = new RoleManagementController();
                    controller.Load(MemberShip.UserRoleId);
                    if (controller.Transaction.DeleteAndCopy || MemberShip.isAdmin)
                    {
                        #region Delete And Copy
                        Transaction ts = entity.Transactions.Where(x => x.Id == currentTransactionId).FirstOrDefault();
                        if(ts!=null)
                        { 
                            DialogResult result = MessageBox.Show("Are you sure you want to delete?", "Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                                if (result.Equals(DialogResult.OK))
                                {

                                    ts.IsDeleted = true;
                                    ts.UpdatedDate = DateTime.Now;
                                    // add gift card amount
                                    foreach (TransactionDetail detail in ts.TransactionDetails)
                                    {
                                        //detail.IsDeleted = false;
                                        detail.IsDeleted = true;
                                      
                                    }

                                    string date = dgvTransactionList.Rows[e.RowIndex].Cells[3].Value.ToString();
                                    DateTime _Trandate = Utility.Convert_Date(date);
                                    //productList.Clear();
                                    DeleteLog dl = new DeleteLog();
                                    dl.DeletedDate = DateTime.Now;
                                    dl.CounterId = MemberShip.CounterId;
                                    dl.UserId = MemberShip.UserId;
                                    dl.IsParent = true;
                                    dl.TransactionId = ts.Id;

                                    entity.DeleteLogs.Add(dl);

                                    entity.SaveChanges();
                                    if (System.Windows.Forms.Application.OpenForms["Sales"] != null)
                                    {
                                        Sales openedForm = (Sales)System.Windows.Forms.Application.OpenForms["Sales"];
                                        openedForm.DeleteCopy(currentTransactionId);
                                        this.Dispose();
                                    }
                                }
                            }
                        
                        #endregion
                    }
                    else
                    {
                        MessageBox.Show("You are not allowed to delete transaction information", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                else if (e.ColumnIndex == 10)
                {
                    string Note = (from p in entity.Transactions where p.Id == currentTransactionId select p.Note).FirstOrDefault();
                    AddNote note = new AddNote();
                    note.editnote = Note;
                    note.status = "EDIT";
                    note.tranid = currentTransactionId;
                    note.ShowDialog();
                }

                else if (e.ColumnIndex == 11)
                {
                    var tranType = (from t in entity.Transactions where t.Id == currentTransactionId select t.Type).FirstOrDefault();
                    ViewTicket newForm = new ViewTicket();
                    newForm.transactionId = currentTransactionId;
                    newForm.ShowDialog();
                   
                }
            }
        }

        private void dgvTransactionList_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow row in dgvTransactionList.Rows)
            {
                Transaction currentt = (Transaction)row.DataBoundItem;
                row.Cells[0].Value = currentt.Id;
                row.Cells[1].Value = currentt.Type;
                row.Cells[2].Value = currentt.PaymentType.Name;
                row.Cells[3].Value = currentt.DateTime.Value.ToString(SettingController.GlobalDateFormat);
                row.Cells[4].Value = currentt.DateTime.Value.ToString("hh:mm");
                row.Cells[5].Value = currentt.User.Name;



                if (currentt.PaymentType.Name != "FOC")
                {
                    row.Cells[6].Value = currentt.TotalAmount;
                }
                else
                {
                    row.Cells[6].Value = 0;
                }

               if (BOOrPOS.IsBackOffice == true)
                {
                    if (dgvTransactionList.Columns[9].Visible != false)
                    {
                        dgvTransactionList.Columns[9].Visible = false;
                    }
                }
                else
                {
                    if (dgvTransactionList.Columns[9].Visible != false)
                    {
                        dgvTransactionList.Columns[9].Visible = true;
                    }
                }

            }
        }

        private void dgvTransactionList_CustomCellFormatting()
        {
            //Role Management
            RoleManagementController controller = new RoleManagementController();
            controller.Load(MemberShip.UserRoleId);
            // Transaction Delete
            if (!MemberShip.isAdmin && !controller.Transaction.EditOrDelete)
            {
                dgvTransactionList.Columns["colDelete"].Visible = false;
            }
            // Transaction Delete And Copy
            if (!MemberShip.isAdmin && !controller.Transaction.DeleteAndCopy)
            {
                dgvTransactionList.Columns["colDeleteAndCopy"].Visible = false;
            }
        }

        private void cboshoplist_selectedvaluechanged(object sender, EventArgs e)
        {

            //LoadData();
        }
        #endregion

    
        public void LoadData()
        {

            try
            {
                Utility.WaitCursor();
                if (IsStart == true)
                {


                    entity = new POSEntities();
                    dgvTransactionList_CustomCellFormatting();

                    int shopid = Convert.ToInt32(cboshoplist.SelectedValue);
                    string shortcode = (from p in entity.Shops where p.Id == shopid select p.ShortCode).FirstOrDefault();

                    bool optionvisible = true;//Utility.TransactionDelRefHide(shopid);
                    dgvTransactionList.Columns[8].Visible = optionvisible;
                    dgvTransactionList.Columns[9].Visible = optionvisible;

                    List<string> type = new List<string>();
                    type.Add(TransactionType.Sale);

                    if (rdbDate.Checked)
                    {
                        DateTime fromDate = dtpFrom.Value.Date;
                        DateTime toDate = dtpTo.Value.Date;

                        // type.Add(TransactionType.Settlement);
                        // type.Add(TransactionType.Prepaid);


                        //List<Transaction> transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && t.Type == TransactionType.Sale && t.IsDeleted==false select t).ToList<Transaction>();
                        List<Transaction> transList = (from t in entity.Transactions
                                                       where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true
                                                           && t.IsActive == true && type.Contains(t.Type) && (t.IsDeleted == false || t.IsDeleted == null)
                                                           && t.Id.Substring(2, 2) == shortcode
                                                       select t).ToList<Transaction>();
                        if (transList.Count > 0)
                        {
                            dgvTransactionList.DataSource = transList;
                        }
                        else
                        {
                            dgvTransactionList.DataSource = "";
                            //MessageBox.Show("Item not found!", "Cannot find");
                        }

                    }
                    else
                    {
                        string Id = txtId.Text;
                        if (Id.Trim() != string.Empty)
                        {
                            List<Transaction> transList = (from t in entity.Transactions where t.Id == Id && type.Contains(t.Type) && t.Id.Substring(2, 2) == shortcode select t).ToList().Where(x => x.IsDeleted != true).ToList();
                            if (transList.Count > 0)
                            {
                                dgvTransactionList.DataSource = transList;
                            }
                            else
                            {
                                dgvTransactionList.DataSource = "";
                                MessageBox.Show("Item not found!", "Cannot find");
                            }
                        }
                        else
                        {
                            dgvTransactionList.DataSource = "";
                        }
                    }
                }
            }
            finally
            {
                Utility.LeaveCursor();
            }
        }

       

    }
}

