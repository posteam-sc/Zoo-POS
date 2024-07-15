using Microsoft.Reporting.WinForms;
using POS.APP_Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Objects;
using System.Linq;
using System.Windows.Forms;

namespace POS
{
    public partial class TransactionReport_FOC_MPU : Form
    {
        #region Variable

        POSEntities entity = new POSEntities();
        List<Transaction> transList = new List<Transaction>();
        List<Transaction> FOCtrnsList = new List<Transaction>();
        private ToolTip tp = new ToolTip();
        Boolean Isstart = false;

        #endregion

        #region Event
        public TransactionReport_FOC_MPU()
        {
            InitializeComponent();
        }

        private void TransactionReport_FOC_MPU_Load(object sender, EventArgs e)
        {
            Localization.Localize_FormControls(this);

            SettingController.SetGlobalDateFormat(dtpFrom);
            SettingController.SetGlobalDateFormat(dtpTo);
            List<APP_Data.Counter> counterList = new List<APP_Data.Counter>();
            APP_Data.Counter counterObj = new APP_Data.Counter();
            counterObj.Id = 0;
            counterObj.Name = "Select";
            counterList.Add(counterObj);
            counterList.AddRange((from c in entity.Counters orderby c.Id select c).ToList());
            cboCounter.DataSource = counterList;
            cboCounter.DisplayMember = "Name";
            cboCounter.ValueMember = "Id";

            List<APP_Data.User> userList = new List<APP_Data.User>();
            APP_Data.User userObj = new APP_Data.User();
            userObj.Id = 0;
            userObj.Name = "Select";
            userList.Add(userObj);
            userList.AddRange((from u in entity.Users orderby u.Id select u).ToList());
            cboCashier.DataSource = userList;
            cboCashier.DisplayMember = "Name";
            cboCashier.ValueMember = "Id";

            Utility.BindShop(cboshoplist, true);
            cboshoplist.Text = SettingController.DefaultShop.ShopName;
            Utility.ShopComBo_EnableOrNot(cboshoplist);
            Isstart = true;
            cboshoplist.SelectedIndex = 0;
            //this.reportViewer1.RefreshReport();
            //LoadData();
            gbPaymentType.Enabled = false;
        }

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
            //LoadData();

        }

        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {
            //LoadData();
        }

        private void rdbSale_CheckedChanged(object sender, EventArgs e)
        {
            gbPaymentType.Enabled = true;
            // LoadData();
        }

        private void rdbRefund_CheckedChanged(object sender, EventArgs e)
        {
            gbPaymentType.Enabled = false;
            //LoadData();
        }

        private void rdbSummary_CheckedChanged(object sender, EventArgs e)
        {
            gbPaymentType.Enabled = false;
            //LoadData();
        }

        private void chkCashier_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCashier.Checked)
            {
                lblCashierName.Enabled = true;
                cboCashier.Enabled = true;
            }
            else
            {
                lblCashierName.Enabled = false;
                cboCashier.Enabled = false;
                cboCashier.SelectedIndex = 0;
                //LoadData();
            }

        }

        private void chkCounter_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCounter.Checked)
            {
                lblCounterName.Enabled = true;
                cboCounter.Enabled = true;
            }
            else
            {
                lblCounterName.Enabled = false;
                cboCounter.Enabled = false;
                cboCounter.SelectedIndex = 0;
                //LoadData();
            }

        }

        private void cboCashier_SelectedIndexChanged(object sender, EventArgs e)
        {
            //LoadData();
        }

        private void cboCounter_SelectedIndexChanged(object sender, EventArgs e)
        {
            //LoadData();
        }

        private void chkCash_CheckedChanged(object sender, EventArgs e)
        {
            //LoadData();
        }

        private void chkFOC_CheckedChanged(object sender, EventArgs e)
        {
            //LoadData();
        }

      

        private void cboshoplist_selectedIndexChanged(object sender, EventArgs e)
        {
            //LoadData();
        }
        #endregion

        
        private void LoadData()
        {

            try
            {
                Utility.WaitCursor();
                Application.DoEvents();
                if (Isstart == true)
                {

                    int shopid = Convert.ToInt32(cboshoplist.SelectedValue);
                    string currentshortcode = "";
                    if (shopid != 0)
                    {
                        currentshortcode = (from p in entity.Shops where p.Id == shopid select p.ShortCode).FirstOrDefault();
                    }
                    else
                    {
                        currentshortcode = "0";
                    }


                    DateTime fromDate = dtpFrom.Value.Date;
                    DateTime toDate = dtpTo.Value.Date;
                    bool IsSale = rdbSale.Checked;
                    //bool IsRefund = rdbRefund.Checked;
                    //bool IsDebt = rdbDebt.Checked;
                    bool IsCounter = chkCounter.Checked;
                    bool IsCashier = chkCashier.Checked;
                    //bool IsCredit = chkCredit.Checked;
                    bool IsCash = chkCash.Checked;
                    //bool IsGiftCard = chkGiftCard.Checked;
                    bool IsFOC = chkFOC.Checked;
                    //bool IsMPU = chkMPU.Checked;
                    //bool IsTester = chkTester.Checked;
                    bool IsSummary = rdbSummary.Checked;
                    
                    int CashierId = 0;
                    int CounterId = 0;

                    Boolean hasError = false;

                    tp.RemoveAll();
                    tp.IsBalloon = true;
                    tp.ToolTipIcon = ToolTipIcon.Error;
                    tp.ToolTipTitle = "Error";
                    //Validation
                    if (IsCounter)
                    {
                        if (cboCounter.SelectedIndex == 0)
                        {
                            tp.SetToolTip(cboCounter, "Error");
                            tp.Show("Please select counter name!", cboCounter);
                            hasError = true;
                        }
                    }
                    else if (IsCashier)
                    {
                        if (cboCashier.SelectedIndex == 0)
                        {
                            tp.SetToolTip(cboCashier, "Error");
                            tp.Show("Please select counter name!", cboCashier);
                            hasError = true;
                        }
                    }
                    if (!hasError)
                    {
                        if (cboCounter.SelectedIndex > 0)
                        {
                            CounterId = Convert.ToInt32(cboCounter.SelectedValue);
                        }
                        if (cboCashier.SelectedIndex > 0)
                        {
                            CashierId = Convert.ToInt32(cboCashier.SelectedValue);
                        }
                        #region get transaction with cashier & counter
                        if (IsCashier == true && IsCounter == true)
                        {
                            if (IsSale)
                            {
                                //*Update SD*
                                transList = (from t in entity.Transactions
                                             where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true
                                                 && t.Type == TransactionType.Sale  && t.CounterId == CounterId && t.UserId == CashierId
                                                 && IsCash && t.PaymentTypeId == 1 ||
                                                 (t.IsDeleted == null || t.IsDeleted == false)
                                                 && ((shopid != 0 && t.Id.Substring(2, 2) == currentshortcode) || (shopid == 0 && 1 == 1))
                                             orderby t.PaymentTypeId
                                             select t).ToList<Transaction>();
                                //FOCAmt= transList
                                #region Old Code
                                //if (IsCredit == true && IsCash == true && IsGiftCard == true && IsMPU == true && IsFOC == true)
                                //{
                                //    transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.CounterId == CounterId && t.UserId == CashierId && (t.IsDeleted == null || t.IsDeleted == false) select t).ToList<Transaction>();
                                //}
                                ////one payment type false
                                //else if (IsCredit == true && IsCash == true && IsGiftCard == false && IsMPU == true && IsFOC == true)
                                //{
                                //    transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.CounterId == CounterId && t.UserId == CashierId && t.PaymentTypeId != 3 && (t.IsDeleted == null || t.IsDeleted == false) select t).ToList<Transaction>();
                                //}
                                //else if (IsCredit == false && IsCash == true && IsGiftCard == true && IsMPU == true && IsFOC == true)
                                //{
                                //    transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.CounterId == CounterId && t.UserId == CashierId && t.PaymentTypeId != 2 && (t.IsDeleted == null || t.IsDeleted == false) select t).ToList<Transaction>();
                                //}
                                //else if (IsCredit == true && IsCash == false && IsGiftCard == true && IsMPU == true && IsFOC == true)
                                //{
                                //    transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.CounterId == CounterId && t.UserId == CashierId && t.PaymentTypeId != 1 && (t.IsDeleted == null || t.IsDeleted == false) select t).ToList<Transaction>();
                                //}
                                //else if (IsCredit == true && IsCash == true && IsGiftCard == true && IsMPU == false && IsFOC == true)
                                //{
                                //    transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.CounterId == CounterId && t.UserId == CashierId && t.PaymentTypeId != 5 && (t.IsDeleted == null || t.IsDeleted == false) select t).ToList<Transaction>();
                                //}
                                //else if (IsCredit == true && IsCash == true && IsGiftCard == true && IsMPU == true && IsFOC == false)
                                //{
                                //    transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.CounterId == CounterId && t.UserId == CashierId && t.PaymentTypeId != 4 && (t.IsDeleted == null || t.IsDeleted == false) select t).ToList<Transaction>();
                                //}
                                //// two type false
                                //else if (IsCredit == true && IsCash == false && IsGiftCard == false && IsMPU == true && IsFOC == true)
                                //{
                                //    transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.CounterId == CounterId && t.UserId == CashierId && (t.PaymentTypeId != 1 && t.PaymentTypeId != 3) && (t.IsDeleted == null || t.IsDeleted == false) select t).ToList<Transaction>();
                                //}
                                //else if (IsCredit == false && IsCash == true && IsGiftCard == false && IsMPU == true && IsFOC == true)
                                //{
                                //    transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.CounterId == CounterId && t.UserId == CashierId && (t.PaymentTypeId != 2 && t.PaymentTypeId != 3) && (t.IsDeleted == null || t.IsDeleted == false) select t).ToList<Transaction>();
                                //}
                                //else if (IsCredit == false && IsCash == false && IsGiftCard == true && IsMPU == true && IsFOC == true)
                                //{
                                //    transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.CounterId == CounterId && t.UserId == CashierId && (t.PaymentTypeId != 1 && t.PaymentTypeId != 2) && (t.IsDeleted == null || t.IsDeleted == false) select t).ToList<Transaction>();
                                //}
                                //else if (IsCredit == false && IsCash == true && IsGiftCard == true && IsMPU == false && IsFOC == true)
                                //{
                                //    transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.CounterId == CounterId && t.UserId == CashierId && (t.PaymentTypeId != 2 && t.PaymentTypeId != 5) && (t.IsDeleted == null || t.IsDeleted == false) select t).ToList<Transaction>();
                                //}
                                //else if (IsCredit == false && IsCash == true && IsGiftCard == true && IsMPU == true && IsFOC == false)
                                //{
                                //    transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.CounterId == CounterId && t.UserId == CashierId && (t.PaymentTypeId != 2 && t.PaymentTypeId != 4) && (t.IsDeleted == null || t.IsDeleted == false) select t).ToList<Transaction>();
                                //}
                                //else if (IsCredit == true && IsCash == false && IsGiftCard == true && IsMPU == false && IsFOC == true)
                                //{
                                //    transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.CounterId == CounterId && t.UserId == CashierId && (t.PaymentTypeId != 1 && t.PaymentTypeId != 5) && (t.IsDeleted == null || t.IsDeleted == false) select t).ToList<Transaction>();
                                //}
                                //else if (IsCredit == true && IsCash == false && IsGiftCard == true && IsMPU == true && IsFOC == false)
                                //{
                                //    transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.CounterId == CounterId && t.UserId == CashierId && (t.PaymentTypeId != 1 && t.PaymentTypeId != 4) && (t.IsDeleted == null || t.IsDeleted == false) select t).ToList<Transaction>();
                                //}
                                //else if (IsCredit == true && IsCash == true && IsGiftCard == false && IsMPU == false && IsFOC == true)
                                //{
                                //    transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.CounterId == CounterId && t.UserId == CashierId && (t.PaymentTypeId != 5 && t.PaymentTypeId != 3) && (t.IsDeleted == null || t.IsDeleted == false) select t).ToList<Transaction>();
                                //}
                                //else if (IsCredit == true && IsCash == true && IsGiftCard == false && IsMPU == true && IsFOC == false)
                                //{
                                //    transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.CounterId == CounterId && t.UserId == CashierId && (t.PaymentTypeId != 4 && t.PaymentTypeId != 3) && (t.IsDeleted == null || t.IsDeleted == false) select t).ToList<Transaction>();
                                //}
                                //else if (IsCredit == true && IsCash == true && IsGiftCard == true && IsMPU == false && IsFOC == false)
                                //{
                                //    transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.CounterId == CounterId && t.UserId == CashierId && (t.PaymentTypeId != 4 && t.PaymentTypeId != 5) && (t.IsDeleted == null || t.IsDeleted == false) select t).ToList<Transaction>();
                                //}
                                ////three type false
                                //else if (IsCredit == false && IsCash == false && IsGiftCard == false && IsMPU == true && IsFOC == true)
                                //{
                                //    transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.CounterId == CounterId && t.UserId == CashierId && (t.PaymentTypeId == 4 || t.PaymentTypeId == 5) && (t.IsDeleted == null || t.IsDeleted == false) select t).ToList<Transaction>();
                                //}
                                //else if (IsCredit == false && IsCash == false && IsGiftCard == true && IsMPU == false && IsFOC == true)
                                //{
                                //    transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.CounterId == CounterId && t.UserId == CashierId && (t.PaymentTypeId == 4 || t.PaymentTypeId == 3) && (t.IsDeleted == null || t.IsDeleted == false) select t).ToList<Transaction>();
                                //}
                                //else if (IsCredit == false && IsCash == false && IsGiftCard == true && IsMPU == true && IsFOC == false)
                                //{
                                //    transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.CounterId == CounterId && t.UserId == CashierId && (t.PaymentTypeId == 3 || t.PaymentTypeId == 5) && (t.IsDeleted == null || t.IsDeleted == false) select t).ToList<Transaction>();
                                //}
                                //else if (IsCredit == true && IsCash == false && IsGiftCard == false && IsMPU == false && IsFOC == true)
                                //{
                                //    transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.CounterId == CounterId && t.UserId == CashierId && (t.PaymentTypeId == 4 || t.PaymentTypeId == 2) && (t.IsDeleted == null || t.IsDeleted == false) select t).ToList<Transaction>();
                                //}
                                //else if (IsCredit == true && IsCash == false && IsGiftCard == false && IsMPU == true && IsFOC == false)
                                //{
                                //    transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.CounterId == CounterId && t.UserId == CashierId && (t.PaymentTypeId == 2 || t.PaymentTypeId == 5) && (t.IsDeleted == null || t.IsDeleted == false) select t).ToList<Transaction>();
                                //}
                                //else if (IsCredit == true && IsCash == true && IsGiftCard == false && IsMPU == false && IsFOC == false)
                                //{
                                //    transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.CounterId == CounterId && t.UserId == CashierId && (t.PaymentTypeId == 2 || t.PaymentTypeId == 1) && (t.IsDeleted == null || t.IsDeleted == false) select t).ToList<Transaction>();
                                //}
                                //else if (IsCredit == false && IsCash == true && IsGiftCard == false && IsMPU == false && IsFOC == true)
                                //{
                                //    transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.CounterId == CounterId && t.UserId == CashierId && (t.PaymentTypeId == 4 || t.PaymentTypeId == 1) && (t.IsDeleted == null || t.IsDeleted == false) select t).ToList<Transaction>();
                                //}
                                //else if (IsCredit == true && IsCash == false && IsGiftCard == true && IsMPU == false && IsFOC == false)
                                //{
                                //    transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.CounterId == CounterId && t.UserId == CashierId && (t.PaymentTypeId == 2 || t.PaymentTypeId == 3) && (t.IsDeleted == null || t.IsDeleted == false) select t).ToList<Transaction>();
                                //}
                                //else if (IsCredit == false && IsCash == true && IsGiftCard == true && IsMPU == false && IsFOC == false)
                                //{
                                //    transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.CounterId == CounterId && t.UserId == CashierId && (t.PaymentTypeId == 1 || t.PaymentTypeId == 3) && (t.IsDeleted == null || t.IsDeleted == false) select t).ToList<Transaction>();
                                //}
                                //else if (IsCredit == false && IsCash == true && IsGiftCard == false && IsMPU == true && IsFOC == false)
                                //{
                                //    transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.CounterId == CounterId && t.UserId == CashierId && (t.PaymentTypeId == 1 || t.PaymentTypeId == 5) && (t.IsDeleted == null || t.IsDeleted == false) select t).ToList<Transaction>();
                                //}
                                ////four type is false
                                //else if (IsCredit == false && IsCash == true && IsGiftCard == false && IsMPU == false && IsFOC == false)
                                //{
                                //    transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.CounterId == CounterId && t.UserId == CashierId && (t.PaymentTypeId == 1) && (t.IsDeleted == null || t.IsDeleted == false) select t).ToList<Transaction>();
                                //}
                                //else if (IsCredit == true && IsCash == false && IsGiftCard == false && IsMPU == false && IsFOC == false)
                                //{
                                //    transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.CounterId == CounterId && t.UserId == CashierId && (t.PaymentTypeId == 2) && (t.IsDeleted == null || t.IsDeleted == false) select t).ToList<Transaction>();
                                //}
                                //else if (IsCredit == false && IsCash == false && IsGiftCard == true && IsMPU == false && IsFOC == false)
                                //{
                                //    transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.CounterId == CounterId && t.UserId == CashierId && (t.PaymentTypeId == 3) && (t.IsDeleted == null || t.IsDeleted == false) select t).ToList<Transaction>();
                                //}
                                //else if (IsCredit == false && IsCash == false && IsGiftCard == false && IsMPU == false && IsFOC == true)
                                //{
                                //    transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.CounterId == CounterId && t.UserId == CashierId && (t.PaymentTypeId == 4) && (t.IsDeleted == null || t.IsDeleted == false) select t).ToList<Transaction>();
                                //}
                                //else if (IsCredit == false && IsCash == false && IsGiftCard == false && IsMPU == true && IsFOC == false)
                                //{
                                //    transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.CounterId == CounterId && t.UserId == CashierId && (t.PaymentTypeId == 5) && (t.IsDeleted == null || t.IsDeleted == false) select t).ToList<Transaction>();
                                //}
                                ////all false
                                //else if (IsCredit == false && IsCash == false && IsGiftCard == false && IsMPU == false && IsFOC == false)
                                //{
                                //    transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.CounterId == CounterId && t.UserId == CashierId && (t.PaymentTypeId != 1 && t.PaymentTypeId != 2 && t.PaymentTypeId != 3 && t.PaymentTypeId != 4 && t.PaymentTypeId != 5) && (t.IsDeleted == null || t.IsDeleted == false) select t).ToList<Transaction>();
                                //}
                                #endregion

                                ShowReportViewer();
                                lblPeriod.Text = fromDate.ToString() + " to " + toDate.ToString();
                                // lblNumberofTransaction.Text = transList.Count.ToString();
                                gbTransactionList.Text = "Sale Transaction Report for ";
                                // lblTotalAmount.Text = "";
                            }                         
                            else if (IsSummary)
                            {
                                transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && t.Type == TransactionType.Sale && t.PaymentTypeId == 1 && t.CounterId == CounterId && t.UserId == CashierId && (t.IsDeleted == null || t.IsDeleted == false) && ((shopid != 0 && t.Id.Substring(2, 2) == currentshortcode) || (shopid == 0 && 1 == 1)) select t).ToList<Transaction>();
                                FOCtrnsList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && t.Type == TransactionType.Sale && t.PaymentTypeId == 4 && t.CounterId == CounterId && t.UserId == CashierId && (t.IsDeleted == null || t.IsDeleted == false) && ((shopid != 0 && t.Id.Substring(2, 2) == currentshortcode) || (shopid == 0 && 1 == 1)) select t).ToList<Transaction>();
                                ShowReportViewer1();
                                lblPeriod.Text = fromDate.ToString(SettingController.GlobalDateFormat) + " To " + toDate.ToString(SettingController.GlobalDateFormat);
                                // lblNumberofTransaction.Text = transList.Count.ToString();
                                gbTransactionList.Text = "Transaction Report";
                                //lblTotalAmount.Text = "";
                            }
                        }
                        #endregion
                        #region get transaction with cashier only
                        else if (IsCashier == true && IsCounter == false)
                        {
                            if (IsSale)
                            {
                                //*Update SD*
                                transList = (from t in entity.Transactions
                                             where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true
                                                 && t.Type == TransactionType.Sale && t.UserId == CashierId
                                                 && IsCash && t.PaymentTypeId == 1 ||
                                                 (t.IsDeleted == null || t.IsDeleted == false) && ((shopid != 0 && t.Id.Substring(2, 2) == currentshortcode) || (shopid == 0 && 1 == 1))
                                             orderby t.PaymentTypeId
                                             select t).ToList<Transaction>();

                                #region Old Code
                                //if (IsCredit == true && IsCash == true && IsGiftCard == true && IsMPU == true && IsFOC == true)
                                //{
                                //    transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true
                                //                     && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.UserId == CashierId && (t.IsDeleted == null || t.IsDeleted == false) select t).ToList<Transaction>();
                                //}
                                //else if (IsCredit == true && IsCash == true && IsGiftCard == false && IsMPU == true && IsFOC == true)
                                //{
                                //    transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.UserId == CashierId && t.PaymentTypeId != 3 && (t.IsDeleted == null || t.IsDeleted == false) select t).ToList<Transaction>();
                                //}
                                //else if (IsCredit == false && IsCash == true && IsGiftCard == true && IsMPU == true && IsFOC == true)
                                //{
                                //    transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.UserId == CashierId && t.PaymentTypeId != 2 && (t.IsDeleted == null || t.IsDeleted == false) select t).ToList<Transaction>();
                                //}
                                //else if (IsCredit == true && IsCash == false && IsGiftCard == true && IsMPU == true && IsFOC == true)
                                //{
                                //    transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.UserId == CashierId && t.PaymentTypeId != 1 && (t.IsDeleted == null || t.IsDeleted == false) select t).ToList<Transaction>();
                                //}
                                //else if (IsCredit == true && IsCash == true && IsGiftCard == true && IsMPU == false && IsFOC == true)
                                //{
                                //    transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.UserId == CashierId && t.PaymentTypeId != 5 && (t.IsDeleted == null || t.IsDeleted == false) select t).ToList<Transaction>();
                                //}
                                //else if (IsCredit == true && IsCash == true && IsGiftCard == true && IsMPU == true && IsFOC == false)
                                //{
                                //    transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.UserId == CashierId && t.PaymentTypeId != 4 && (t.IsDeleted == null || t.IsDeleted == false) select t).ToList<Transaction>();
                                //}
                                //// two type false
                                //else if (IsCredit == true && IsCash == false && IsGiftCard == false && IsMPU == true && IsFOC == true)
                                //{
                                //    transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.UserId == CashierId && (t.PaymentTypeId != 1 && t.PaymentTypeId != 3) && (t.IsDeleted == null || t.IsDeleted == false) select t).ToList<Transaction>();
                                //}
                                //else if (IsCredit == false && IsCash == true && IsGiftCard == false && IsMPU == true && IsFOC == true)
                                //{
                                //    transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.UserId == CashierId && (t.PaymentTypeId != 2 && t.PaymentTypeId != 3) && (t.IsDeleted == null || t.IsDeleted == false) select t).ToList<Transaction>();
                                //}
                                //else if (IsCredit == false && IsCash == false && IsGiftCard == true && IsMPU == true && IsFOC == true)
                                //{
                                //    transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.UserId == CashierId && (t.PaymentTypeId != 1 && t.PaymentTypeId != 2) && (t.IsDeleted == null || t.IsDeleted == false) select t).ToList<Transaction>();
                                //}
                                //else if (IsCredit == false && IsCash == true && IsGiftCard == true && IsMPU == false && IsFOC == true)
                                //{
                                //    transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.UserId == CashierId && (t.PaymentTypeId != 2 && t.PaymentTypeId != 5) && (t.IsDeleted == null || t.IsDeleted == false) select t).ToList<Transaction>();
                                //}
                                //else if (IsCredit == false && IsCash == true && IsGiftCard == true && IsMPU == true && IsFOC == false)
                                //{
                                //    transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.UserId == CashierId && (t.PaymentTypeId != 2 && t.PaymentTypeId != 4) && (t.IsDeleted == null || t.IsDeleted == false) select t).ToList<Transaction>();
                                //}
                                //else if (IsCredit == true && IsCash == false && IsGiftCard == true && IsMPU == false && IsFOC == true)
                                //{
                                //    transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.UserId == CashierId && (t.PaymentTypeId != 1 && t.PaymentTypeId != 5) && (t.IsDeleted == null || t.IsDeleted == false) select t).ToList<Transaction>();
                                //}
                                //else if (IsCredit == true && IsCash == false && IsGiftCard == true && IsMPU == true && IsFOC == false)
                                //{
                                //    transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.UserId == CashierId && (t.PaymentTypeId != 1 && t.PaymentTypeId != 4) && (t.IsDeleted == null || t.IsDeleted == false) select t).ToList<Transaction>();
                                //}
                                //else if (IsCredit == true && IsCash == true && IsGiftCard == false && IsMPU == false && IsFOC == true)
                                //{
                                //    transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.UserId == CashierId && (t.PaymentTypeId != 5 && t.PaymentTypeId != 3) && (t.IsDeleted == null || t.IsDeleted == false) select t).ToList<Transaction>();
                                //}
                                //else if (IsCredit == true && IsCash == true && IsGiftCard == false && IsMPU == true && IsFOC == false)
                                //{
                                //    transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.UserId == CashierId && (t.PaymentTypeId != 3 && t.PaymentTypeId != 4) && (t.IsDeleted == null || t.IsDeleted == false) select t).ToList<Transaction>();
                                //}
                                //else if (IsCredit == true && IsCash == true && IsGiftCard == true && IsMPU == false && IsFOC == false)
                                //{
                                //    transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.UserId == CashierId && (t.PaymentTypeId != 4 && t.PaymentTypeId != 5) && (t.IsDeleted == null || t.IsDeleted == false) select t).ToList<Transaction>();
                                //}
                                ////three type false
                                //else if (IsCredit == false && IsCash == false && IsGiftCard == false && IsMPU == true && IsFOC == true)
                                //{
                                //    transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.UserId == CashierId && (t.PaymentTypeId == 4 || t.PaymentTypeId == 5) && (t.IsDeleted == null || t.IsDeleted == false) select t).ToList<Transaction>();
                                //}
                                //else if (IsCredit == false && IsCash == false && IsGiftCard == true && IsMPU == false && IsFOC == true)
                                //{
                                //    transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.UserId == CashierId && (t.PaymentTypeId == 4 || t.PaymentTypeId == 3) && (t.IsDeleted == null || t.IsDeleted == false) select t).ToList<Transaction>();
                                //}
                                //else if (IsCredit == false && IsCash == false && IsGiftCard == true && IsMPU == true && IsFOC == false)
                                //{
                                //    transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.UserId == CashierId && (t.PaymentTypeId == 3 || t.PaymentTypeId == 5) && (t.IsDeleted == null || t.IsDeleted == false) select t).ToList<Transaction>();
                                //}
                                //else if (IsCredit == true && IsCash == false && IsGiftCard == false && IsMPU == false && IsFOC == true)
                                //{
                                //    transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.UserId == CashierId && (t.PaymentTypeId == 4 || t.PaymentTypeId == 2) && (t.IsDeleted == null || t.IsDeleted == false) select t).ToList<Transaction>();
                                //}
                                //else if (IsCredit == true && IsCash == false && IsGiftCard == false && IsMPU == true && IsFOC == false)
                                //{
                                //    transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.UserId == CashierId && (t.PaymentTypeId == 2 || t.PaymentTypeId == 5) && (t.IsDeleted == null || t.IsDeleted == false) select t).ToList<Transaction>();
                                //}
                                //else if (IsCredit == true && IsCash == true && IsGiftCard == false && IsMPU == false && IsFOC == false)
                                //{
                                //    transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.UserId == CashierId && (t.PaymentTypeId == 2 || t.PaymentTypeId == 1) && (t.IsDeleted == null || t.IsDeleted == false) select t).ToList<Transaction>();
                                //}
                                //else if (IsCredit == false && IsCash == true && IsGiftCard == false && IsMPU == false && IsFOC == true)
                                //{
                                //    transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.UserId == CashierId && (t.PaymentTypeId == 4 || t.PaymentTypeId == 1) && (t.IsDeleted == null || t.IsDeleted == false) select t).ToList<Transaction>();
                                //}
                                //else if (IsCredit == true && IsCash == false && IsGiftCard == true && IsMPU == false && IsFOC == false)
                                //{
                                //    transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.UserId == CashierId && (t.PaymentTypeId == 2 || t.PaymentTypeId == 3) && (t.IsDeleted == null || t.IsDeleted == false) select t).ToList<Transaction>();
                                //}
                                //else if (IsCredit == false && IsCash == true && IsGiftCard == true && IsMPU == false && IsFOC == false)
                                //{
                                //    transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.UserId == CashierId && (t.PaymentTypeId == 1 || t.PaymentTypeId == 3) && (t.IsDeleted == null || t.IsDeleted == false) select t).ToList<Transaction>();
                                //}
                                //else if (IsCredit == false && IsCash == true && IsGiftCard == false && IsMPU == true && IsFOC == false)
                                //{
                                //    transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.UserId == CashierId && (t.PaymentTypeId == 1 || t.PaymentTypeId == 5) && (t.IsDeleted == null || t.IsDeleted == false) select t).ToList<Transaction>();
                                //}
                                ////four type is false
                                //else if (IsCredit == false && IsCash == true && IsGiftCard == false && IsMPU == false && IsFOC == false)
                                //{
                                //    transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.UserId == CashierId && (t.PaymentTypeId == 1) && (t.IsDeleted == null || t.IsDeleted == false) select t).ToList<Transaction>();
                                //}
                                //else if (IsCredit == true && IsCash == false && IsGiftCard == false && IsMPU == false && IsFOC == false)
                                //{
                                //    transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.UserId == CashierId && (t.PaymentTypeId == 2) && (t.IsDeleted == null || t.IsDeleted == false) select t).ToList<Transaction>();
                                //}
                                //else if (IsCredit == false && IsCash == false && IsGiftCard == true && IsMPU == false && IsFOC == false)
                                //{
                                //    transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.UserId == CashierId && (t.PaymentTypeId == 3) && (t.IsDeleted == null || t.IsDeleted == false) select t).ToList<Transaction>();
                                //}
                                //else if (IsCredit == false && IsCash == false && IsGiftCard == false && IsMPU == false && IsFOC == true)
                                //{
                                //    transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.UserId == CashierId && (t.PaymentTypeId == 4) && (t.IsDeleted == null || t.IsDeleted == false) select t).ToList<Transaction>();
                                //}
                                //else if (IsCredit == false && IsCash == false && IsGiftCard == false && IsMPU == true && IsFOC == false)
                                //{
                                //    transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.UserId == CashierId && (t.PaymentTypeId == 5) && (t.IsDeleted == null || t.IsDeleted == false) select t).ToList<Transaction>();
                                //}
                                ////all false

                                //else if (IsCredit == false && IsCash == false && IsGiftCard == false && IsMPU == false && IsFOC == false)
                                //{
                                //    transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.UserId == CashierId && (t.PaymentTypeId != 2 && t.PaymentTypeId != 1 && t.PaymentTypeId != 3 && t.PaymentTypeId != 4 && t.PaymentTypeId != 5) && (t.IsDeleted == null || t.IsDeleted == false) select t).ToList<Transaction>();
                                //}
                                #endregion

                                ShowReportViewer();
                                lblPeriod.Text = fromDate.ToString() + " to " + toDate.ToString();
                                // lblNumberofTransaction.Text = transList.Count.ToString();
                                gbTransactionList.Text = "Sale Transaction Report for ";
                                // lblTotalAmount.Text = "";
                            }
                            //else if (IsRefund)
                            //{
                            //    transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Refund || t.Type == TransactionType.CreditRefund) && t.UserId == CashierId && (t.IsDeleted == null || t.IsDeleted == false) && t.Id.Substring(2, 2) == currentshortcode select t).ToList<Transaction>();
                            //    ShowReportViewer();
                            //    lblPeriod.Text = fromDate.ToString() + " to " + toDate.ToString();
                            //    // lblNumberofTransaction.Text = transList.Count.ToString();
                            //    gbTransactionList.Text = "Refund Transaction Report for ";
                            //    //lblTotalAmount.Text = "";
                            //}
                            //else if (IsDebt)
                            //{
                            //    transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && (t.Type == TransactionType.Prepaid || t.Type == TransactionType.Settlement) && t.UserId == CashierId && (t.IsDeleted == null || t.IsDeleted == false) && t.Id.Substring(2, 2) == currentshortcode select t).ToList<Transaction>();
                            //    ShowReportViewer();
                            //    lblPeriod.Text = fromDate.ToString() + " to " + toDate.ToString();
                            //    // lblNumberofTransaction.Text = transList.Count.ToString();
                            //    gbTransactionList.Text = "Debt Transaction Report for ";
                            //    //lblTotalAmount.Text = "";
                            //}
                            else if (IsSummary)
                            {
                                transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && t.Type == TransactionType.Sale && t.PaymentTypeId == 1 && t.UserId == CashierId && (t.IsDeleted == null || t.IsDeleted == false) && ((shopid != 0 && t.Id.Substring(2, 2) == currentshortcode) || (shopid == 0 && 1 == 1)) select t).ToList<Transaction>();
                                //RtransList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && t.Type == TransactionType.Refund && t.UserId == CashierId && (t.IsDeleted == null || t.IsDeleted == false) && ((shopid != 0 && t.Id.Substring(2, 2) == currentshortcode) || (shopid == 0 && 1 == 1)) select t).ToList<Transaction>();
                                //DtransList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && (t.Type == TransactionType.Settlement || t.Type == TransactionType.Prepaid) && t.UserId == CashierId && (t.IsDeleted == null || t.IsDeleted == false) && ((shopid != 0 && t.Id.Substring(2, 2) == currentshortcode) || (shopid == 0 && 1 == 1)) select t).ToList<Transaction>();
                                //CRtransList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && t.Type == TransactionType.CreditRefund && t.UserId == CashierId && (t.IsDeleted == null || t.IsDeleted == false) && ((shopid != 0 && t.Id.Substring(2, 2) == currentshortcode) || (shopid == 0 && 1 == 1)) select t).ToList<Transaction>();
                                //GCtransList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && t.Type == TransactionType.Sale && t.PaymentTypeId == 3 && t.UserId == CashierId && (t.IsDeleted == null || t.IsDeleted == false) && ((shopid != 0 && t.Id.Substring(2, 2) == currentshortcode) || (shopid == 0 && 1 == 1)) select t).ToList<Transaction>();
                                //CtransList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && t.Type == TransactionType.Credit && t.UserId == CashierId && (t.IsDeleted == null || t.IsDeleted == false) && ((shopid != 0 && t.Id.Substring(2, 2) == currentshortcode) || (shopid == 0 && 1 == 1)) select t).ToList<Transaction>();
                                //MPUtransList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && t.Type == TransactionType.Sale && t.PaymentTypeId == 5 && t.UserId == CashierId && (t.IsDeleted == null || t.IsDeleted == false) && ((shopid != 0 && t.Id.Substring(2, 2) == currentshortcode) || (shopid == 0 && 1 == 1)) select t).ToList<Transaction>();
                                FOCtrnsList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && t.Type == TransactionType.Sale && t.PaymentTypeId == 4 && t.UserId == CashierId && (t.IsDeleted == null || t.IsDeleted == false) && ((shopid != 0 && t.Id.Substring(2, 2) == currentshortcode) || (shopid == 0 && 1 == 1)) select t).ToList<Transaction>();
                               // TesterList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && t.Type == TransactionType.Sale && t.PaymentTypeId == 6 && t.UserId == CashierId && (t.IsDeleted == null || t.IsDeleted == false) && ((shopid != 0 && t.Id.Substring(2, 2) == currentshortcode) || (shopid == 0 && 1 == 1)) select t).ToList<Transaction>();
                                ShowReportViewer1();
                                lblPeriod.Text = fromDate.ToString() + " to " + toDate.ToString();
                                // lblNumberofTransaction.Text = transList.Count.ToString();
                                gbTransactionList.Text = "Transaction Report";
                                //lblTotalAmount.Text = "";
                            }
                        }
                        #endregion
                        #region get all transactions with counter only
                        else if (IsCashier == false && IsCounter == true)
                        {
                            if (IsSale)
                            {
                                #region Payment

                                //*Update SD*
                                transList = (from t in entity.Transactions
                                             where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true
                                                 && t.Type == TransactionType.Sale && t.CounterId == CounterId
                                                 && (IsCash && t.PaymentTypeId == 1) ||
                                                 (t.IsDeleted == null || t.IsDeleted == false) && ((shopid != 0 && t.Id.Substring(2, 2) == currentshortcode) || (shopid == 0 && 1 == 1))
                                             orderby t.PaymentTypeId
                                             select t).ToList<Transaction>();


                             

                                #endregion
                                ShowReportViewer();
                                lblPeriod.Text = fromDate.ToString(SettingController.GlobalDateFormat) + " To " + toDate.ToString(SettingController.GlobalDateFormat);
                                // lblNumberofTransaction.Text = transList.Count.ToString();
                                gbTransactionList.Text = "Sale Transaction Report for ";
                                // lblTotalAmount.Text = "";
                            }
                           
                            else if (IsSummary)
                            {
                                transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && t.Type == TransactionType.Sale && t.PaymentTypeId == 1 && t.CounterId == CounterId && (t.IsDeleted == null || t.IsDeleted == false) && ((shopid != 0 && t.Id.Substring(2, 2) == currentshortcode) || (shopid == 0 && 1 == 1)) select t).ToList<Transaction>();
                                FOCtrnsList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && t.Type == TransactionType.Sale && t.PaymentTypeId == 4 && t.CounterId == CounterId && (t.IsDeleted == null || t.IsDeleted == false) && ((shopid != 0 && t.Id.Substring(2, 2) == currentshortcode) || (shopid == 0 && 1 == 1)) select t).ToList<Transaction>();
                                ShowReportViewer1();
                                lblPeriod.Text = fromDate.ToString(SettingController.GlobalDateFormat) + " To " + toDate.ToString(SettingController.GlobalDateFormat);
                                // lblNumberofTransaction.Text = transList.Count.ToString();
                                gbTransactionList.Text = "Transaction Report";
                                //lblTotalAmount.Text = "";
                            }
                        }
                        #endregion
                        #region get all transactions
                        else
                        {
                            if (IsSale)
                            {
                                //*Update SD*
                                transList = (from t in entity.Transactions
                                             where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true
                                                 && t.Type == TransactionType.Sale
                                                 && (IsCash && t.PaymentTypeId == 1) || (IsFOC && t.PaymentTypeId == 4) &&
                                                 (t.IsDeleted == null || t.IsDeleted == false) && ((shopid != 0 && t.Id.Substring(2, 2) == currentshortcode) || (shopid == 0 && 1 == 1))
                                             orderby t.PaymentTypeId
                                             select t).ToList<Transaction>();

                                #region Old Code
                                //if (IsCredit == true && IsCash == true && IsGiftCard == true && IsMPU == true && IsFOC == true)
                                //{
                                //    transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && (t.IsDeleted == null || t.IsDeleted == false) select t).ToList<Transaction>();
                                //}
                                ////one payment type false
                                //else if (IsCredit == true && IsCash == true && IsGiftCard == false && IsMPU == true && IsFOC == true)
                                //{
                                //    transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.PaymentTypeId != 3 && (t.IsDeleted == null || t.IsDeleted == false) select t).ToList<Transaction>();
                                //}
                                //else if (IsCredit == false && IsCash == true && IsGiftCard == true && IsMPU == true && IsFOC == true)
                                //{
                                //    transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.PaymentTypeId != 2 && (t.IsDeleted == null || t.IsDeleted == false) select t).ToList<Transaction>();
                                //}
                                //else if (IsCredit == true && IsCash == false && IsGiftCard == true && IsMPU == true && IsFOC == true)
                                //{
                                //    transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.PaymentTypeId != 1 && (t.IsDeleted == null || t.IsDeleted == false) select t).ToList<Transaction>();
                                //}
                                //else if (IsCredit == true && IsCash == true && IsGiftCard == true && IsMPU == false && IsFOC == true)
                                //{
                                //    transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.PaymentTypeId != 5 && (t.IsDeleted == null || t.IsDeleted == false) select t).ToList<Transaction>();
                                //}
                                //else if (IsCredit == true && IsCash == true && IsGiftCard == true && IsMPU == true && IsFOC == false)
                                //{
                                //    transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && t.PaymentTypeId != 4 && (t.IsDeleted == null || t.IsDeleted == false) select t).ToList<Transaction>();
                                //}
                                //// two type false
                                //else if (IsCredit == true && IsCash == false && IsGiftCard == false && IsMPU == true && IsFOC == true)
                                //{
                                //    transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && (t.PaymentTypeId != 3 && t.PaymentTypeId != 1) && (t.IsDeleted == null || t.IsDeleted == false) select t).ToList<Transaction>();
                                //}
                                //else if (IsCredit == false && IsCash == true && IsGiftCard == false && IsMPU == true && IsFOC == true)
                                //{
                                //    transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && (t.PaymentTypeId != 3 && t.PaymentTypeId != 2) && (t.IsDeleted == null || t.IsDeleted == false) select t).ToList<Transaction>();
                                //}
                                //else if (IsCredit == false && IsCash == false && IsGiftCard == true && IsMPU == true && IsFOC == true)
                                //{
                                //    transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && (t.PaymentTypeId != 2 && t.PaymentTypeId != 1) && (t.IsDeleted == null || t.IsDeleted == false) select t).ToList<Transaction>();
                                //}
                                //else if (IsCredit == false && IsCash == true && IsGiftCard == true && IsMPU == false && IsFOC == true)
                                //{
                                //    transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && (t.PaymentTypeId != 2 && t.PaymentTypeId != 5) && (t.IsDeleted == null || t.IsDeleted == false) select t).ToList<Transaction>();
                                //}
                                //else if (IsCredit == false && IsCash == true && IsGiftCard == true && IsMPU == true && IsFOC == false)
                                //{
                                //    transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && (t.PaymentTypeId != 2 && t.PaymentTypeId != 4) && (t.IsDeleted == null || t.IsDeleted == false) select t).ToList<Transaction>();
                                //}
                                //else if (IsCredit == true && IsCash == false && IsGiftCard == true && IsMPU == false && IsFOC == true)
                                //{
                                //    transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && (t.PaymentTypeId != 5 && t.PaymentTypeId != 1) && (t.IsDeleted == null || t.IsDeleted == false) select t).ToList<Transaction>();
                                //}
                                //else if (IsCredit == true && IsCash == false && IsGiftCard == true && IsMPU == true && IsFOC == false)
                                //{
                                //    transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && (t.PaymentTypeId != 4 && t.PaymentTypeId != 1) && (t.IsDeleted == null || t.IsDeleted == false) select t).ToList<Transaction>();
                                //}
                                //else if (IsCredit == true && IsCash == true && IsGiftCard == false && IsMPU == false && IsFOC == true)
                                //{
                                //    transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && (t.PaymentTypeId != 3 && t.PaymentTypeId != 5) && (t.IsDeleted == null || t.IsDeleted == false) select t).ToList<Transaction>();
                                //}
                                //else if (IsCredit == true && IsCash == true && IsGiftCard == false && IsMPU == true && IsFOC == false)
                                //{
                                //    transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && (t.PaymentTypeId != 3 && t.PaymentTypeId != 4) && (t.IsDeleted == null || t.IsDeleted == false) select t).ToList<Transaction>();
                                //}
                                //else if (IsCredit == true && IsCash == true && IsGiftCard == true && IsMPU == false && IsFOC == false)
                                //{
                                //    transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && (t.PaymentTypeId != 4 && t.PaymentTypeId != 5) && (t.IsDeleted == null || t.IsDeleted == false) select t).ToList<Transaction>();
                                //}
                                ////three type false
                                //else if (IsCredit == false && IsCash == false && IsGiftCard == false && IsMPU == true && IsFOC == true)
                                //{
                                //    transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && (t.PaymentTypeId == 4 || t.PaymentTypeId == 5) && (t.IsDeleted == null || t.IsDeleted == false) select t).ToList<Transaction>();
                                //}
                                //else if (IsCredit == false && IsCash == false && IsGiftCard == true && IsMPU == false && IsFOC == true)
                                //{
                                //    transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && (t.PaymentTypeId == 4 || t.PaymentTypeId == 3) && (t.IsDeleted == null || t.IsDeleted == false) select t).ToList<Transaction>();
                                //}
                                //else if (IsCredit == false && IsCash == false && IsGiftCard == true && IsMPU == true && IsFOC == false)
                                //{
                                //    transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && (t.PaymentTypeId == 3 || t.PaymentTypeId == 5) && (t.IsDeleted == null || t.IsDeleted == false) select t).ToList<Transaction>();
                                //}
                                //else if (IsCredit == true && IsCash == false && IsGiftCard == false && IsMPU == false && IsFOC == true)
                                //{
                                //    transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && (t.PaymentTypeId == 4 || t.PaymentTypeId == 2) && (t.IsDeleted == null || t.IsDeleted == false) select t).ToList<Transaction>();
                                //}
                                //else if (IsCredit == true && IsCash == false && IsGiftCard == false && IsMPU == true && IsFOC == false)
                                //{
                                //    transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && (t.PaymentTypeId == 2 || t.PaymentTypeId == 5) && (t.IsDeleted == null || t.IsDeleted == false) select t).ToList<Transaction>();
                                //}
                                //else if (IsCredit == true && IsCash == true && IsGiftCard == false && IsMPU == false && IsFOC == false)
                                //{
                                //    transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && (t.PaymentTypeId == 2 || t.PaymentTypeId == 1) && (t.IsDeleted == null || t.IsDeleted == false) select t).ToList<Transaction>();
                                //}
                                //else if (IsCredit == false && IsCash == true && IsGiftCard == false && IsMPU == false && IsFOC == true)
                                //{
                                //    transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && (t.PaymentTypeId == 4 || t.PaymentTypeId == 1) && (t.IsDeleted == null || t.IsDeleted == false) select t).ToList<Transaction>();
                                //}
                                //else if (IsCredit == true && IsCash == false && IsGiftCard == true && IsMPU == false && IsFOC == false)
                                //{
                                //    transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && (t.PaymentTypeId == 2 || t.PaymentTypeId == 3) && (t.IsDeleted == null || t.IsDeleted == false) select t).ToList<Transaction>();
                                //}
                                //else if (IsCredit == false && IsCash == true && IsGiftCard == true && IsMPU == false && IsFOC == false)
                                //{
                                //    transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && (t.PaymentTypeId == 1 || t.PaymentTypeId == 3) && (t.IsDeleted == null || t.IsDeleted == false) select t).ToList<Transaction>();
                                //}
                                //else if (IsCredit == false && IsCash == true && IsGiftCard == false && IsMPU == true && IsFOC == false)
                                //{
                                //    transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && (t.PaymentTypeId == 1 || t.PaymentTypeId == 5) && (t.IsDeleted == null || t.IsDeleted == false) select t).ToList<Transaction>();
                                //}
                                ////four type is false
                                //else if (IsCredit == false && IsCash == true && IsGiftCard == false && IsMPU == false && IsFOC == false)
                                //{
                                //    transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && (t.PaymentTypeId == 1) && (t.IsDeleted == null || t.IsDeleted == false) select t).ToList<Transaction>();
                                //}
                                //else if (IsCredit == true && IsCash == false && IsGiftCard == false && IsMPU == false && IsFOC == false)
                                //{
                                //    transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && (t.PaymentTypeId == 2) && (t.IsDeleted == null || t.IsDeleted == false) select t).ToList<Transaction>();
                                //}
                                //else if (IsCredit == false && IsCash == false && IsGiftCard == true && IsMPU == false && IsFOC == false)
                                //{
                                //    transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && (t.PaymentTypeId == 3) && (t.IsDeleted == null || t.IsDeleted == false) select t).ToList<Transaction>();
                                //}
                                //else if (IsCredit == false && IsCash == false && IsGiftCard == false && IsMPU == false && IsFOC == true)
                                //{
                                //    transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && (t.PaymentTypeId == 4) && (t.IsDeleted == null || t.IsDeleted == false) select t).ToList<Transaction>();
                                //}
                                //else if (IsCredit == false && IsCash == false && IsGiftCard == false && IsMPU == true && IsFOC == false)
                                //{
                                //    transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && (t.PaymentTypeId == 5) && (t.IsDeleted == null || t.IsDeleted == false) select t).ToList<Transaction>();
                                //}
                                ////all false

                                //else if (IsCredit == false && IsCash == false && IsGiftCard == false && IsFOC == false && IsMPU == false)
                                //{
                                //    transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit) && (t.PaymentTypeId != 1 && t.PaymentTypeId != 2 && t.PaymentTypeId != 3 && t.PaymentTypeId != 4 && t.PaymentTypeId != 5) && (t.IsDeleted == null || t.IsDeleted == false) select t).ToList<Transaction>();
                                //}
                                #endregion


                                ShowReportViewer();
                                lblPeriod.Text = fromDate.ToString(SettingController.GlobalDateFormat) + " To " + toDate.ToString(SettingController.GlobalDateFormat);
                                // lblNumberofTransaction.Text = transList.Count.ToString();
                                gbTransactionList.Text = "Sale Transaction Report for ";
                                // lblTotalAmount.Text = "";
                            }
                            //else if (IsRefund)
                            //{
                            //    transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Refund || t.Type == TransactionType.CreditRefund) && (t.IsDeleted == null || t.IsDeleted == false) && ((shopid != 0 && t.Id.Substring(2, 2) == currentshortcode) || (shopid == 0 && 1 == 1)) select t).ToList<Transaction>();
                            //    ShowReportViewer();
                            //    lblPeriod.Text = fromDate.ToString() + " to " + toDate.ToString();
                            //    // lblNumberofTransaction.Text = transList.Count.ToString();
                            //    gbTransactionList.Text = "Refund Transaction Report for ";
                            //    //lblTotalAmount.Text = "";
                            //}
                            //else if (IsDebt)
                            //{
                            //    transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && (t.Type == TransactionType.Prepaid || t.Type == TransactionType.Settlement) && (t.IsDeleted == null || t.IsDeleted == false) && ((shopid != 0 && t.Id.Substring(2, 2) == currentshortcode) || (shopid == 0 && 1 == 1)) select t).ToList<Transaction>();
                            //    ShowReportViewer();
                            //    lblPeriod.Text = fromDate.ToString() + " to " + toDate.ToString();
                            //    // lblNumberofTransaction.Text = transList.Count.ToString();
                            //    gbTransactionList.Text = "Debt Transaction Report for ";
                            //    //lblTotalAmount.Text = "";
                            //}
                            else if (IsSummary)
                            {
                                transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && t.Type == TransactionType.Sale && t.PaymentTypeId == 1 && (t.IsDeleted == null || t.IsDeleted == false) && ((shopid != 0 && t.Id.Substring(2, 2) == currentshortcode) || (shopid == 0 && 1 == 1)) select t).ToList<Transaction>();
                                //RtransList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && t.Type == TransactionType.Refund && (t.IsDeleted == null || t.IsDeleted == false) && ((shopid != 0 && t.Id.Substring(2, 2) == currentshortcode) || (shopid == 0 && 1 == 1)) select t).ToList<Transaction>();
                                //DtransList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && (t.Type == TransactionType.Settlement || t.Type == TransactionType.Prepaid) && (t.IsDeleted == null || t.IsDeleted == false) && ((shopid != 0 && t.Id.Substring(2, 2) == currentshortcode) || (shopid == 0 && 1 == 1)) select t).ToList<Transaction>();
                                //CRtransList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && t.Type == TransactionType.CreditRefund && (t.IsDeleted == null || t.IsDeleted == false) && ((shopid != 0 && t.Id.Substring(2, 2) == currentshortcode) || (shopid == 0 && 1 == 1)) select t).ToList<Transaction>();
                                //GCtransList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && t.Type == TransactionType.Sale && t.PaymentTypeId == 3 && (t.IsDeleted == null || t.IsDeleted == false) && ((shopid != 0 && t.Id.Substring(2, 2) == currentshortcode) || (shopid == 0 && 1 == 1)) select t).ToList<Transaction>();
                                //CtransList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && t.Type == TransactionType.Credit && (t.IsDeleted == null || t.IsDeleted == false) && ((shopid != 0 && t.Id.Substring(2, 2) == currentshortcode) || (shopid == 0 && 1 == 1)) select t).ToList<Transaction>();
                                //MPUtransList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && t.Type == TransactionType.Sale && t.PaymentTypeId == 5 && (t.IsDeleted == null || t.IsDeleted == false) && ((shopid != 0 && t.Id.Substring(2, 2) == currentshortcode) || (shopid == 0 && 1 == 1)) select t).ToList<Transaction>();
                                FOCtrnsList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && t.Type == TransactionType.Sale && t.PaymentTypeId == 4 && (t.IsDeleted == null || t.IsDeleted == false) && ((shopid != 0 && t.Id.Substring(2, 2) == currentshortcode) || (shopid == 0 && 1 == 1)) select t).ToList<Transaction>();
                                //   TesterList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && t.Type == TransactionType.Sale && t.PaymentTypeId == 6 && t.CounterId == CounterId && (t.IsDeleted == null || t.IsDeleted == false) select t).ToList<Transaction>();
                                //TesterList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && t.Type == TransactionType.Sale && t.PaymentTypeId == 6 && (t.IsDeleted == null || t.IsDeleted == false) && ((shopid != 0 && t.Id.Substring(2, 2) == currentshortcode) || (shopid == 0 && 1 == 1)) select t).ToList<Transaction>();
                                ShowReportViewer1();
                                lblPeriod.Text = fromDate.ToString(SettingController.GlobalDateFormat) + " To " + toDate.ToString(SettingController.GlobalDateFormat);
                                // lblNumberofTransaction.Text = transList.Count.ToString();
                                gbTransactionList.Text = "Transaction Report";
                                //lblTotalAmount.Text = "";
                            }
                        }
                        #endregion
                    }
                }
            }
            finally
            {
                Utility.LeaveCursor();
            }

        }

        private void ShowReportViewer()
        {
            int shopid = Convert.ToInt32(cboshoplist.SelectedValue);
            string shopname = (from p in entity.Shops where p.Id == shopid select p.ShopName).FirstOrDefault();

            dsReportTemp dsReport = new dsReportTemp();
            int totalAmount = 0, totalQty = 0;
            dsReportTemp.TransactionListDataTable dtTransactionReport = (dsReportTemp.TransactionListDataTable)dsReport.Tables["TransactionList"];
            foreach (Transaction transaction in transList)
            {
                dsReportTemp.TransactionListRow newRow = dtTransactionReport.NewTransactionListRow();
                newRow.TransactionId = transaction.Id;
                newRow.Date = Convert.ToDateTime(transaction.DateTime);
                newRow.SalePerson = transaction.User.Name;
                newRow.PaymentMethod = transaction.PaymentType.Name;
                newRow.Amount = Convert.ToInt32(transaction.TotalAmount);
                totalAmount += newRow.Amount;
                newRow.DiscountAmount = transaction.DiscountAmount.ToString();
                newRow.Type = transaction.Type;
                //newRow.Qty = Convert.ToInt32(transaction.TransactionDetails.Sum(x =>  x.Qty));
                newRow.Qty = Convert.ToInt32(transaction.TransactionDetails.Where(x => x.IsDeleted == false).Sum(x => x.Qty));
                totalQty += newRow.Qty;

                dtTransactionReport.AddTransactionListRow(newRow);
            }

            ReportDataSource rds = new ReportDataSource("DataSet1", dsReport.Tables["TransactionList"]);
            string reportPath = Application.StartupPath + "\\Reports\\TransactionReport.rdlc";
            reportViewer1.LocalReport.ReportPath = reportPath;
            //reportViewer1.Refresh();
            //reportViewer1.RefreshReport();
            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(rds);
            //ReportParameter amountTotal = new ReportParameter("Amount", totalAmount.ToString());
            //reportViewer1.LocalReport.SetParameters(amountTotal);
            //ReportParameter QtyTotal = new ReportParameter("Qty", totalQty.ToString());
            //reportViewer1.LocalReport.SetParameters(QtyTotal);

            ReportParameter TransactionTitle = new ReportParameter("TransactionTitle", gbTransactionList.Text + " for " + shopname);
            reportViewer1.LocalReport.SetParameters(TransactionTitle);

            ReportParameter Date = new ReportParameter("Date", " From " + dtpFrom.Value.Date.ToString(SettingController.GlobalDateFormat) + " To " + dtpTo.Value.Date.ToString(SettingController.GlobalDateFormat));
            reportViewer1.LocalReport.SetParameters(Date);

            reportViewer1.RefreshReport();
        }

        private void ShowReportViewer1()
        {
            int shopid = Convert.ToInt32(cboshoplist.SelectedValue);
            APP_Data.Shop currentshop;
            if (shopid == 0)
            {
                currentshop = SettingController.DefaultShop;
            }
            else
            {
                currentshop = entity.Shops.Find(shopid);
            }
            int totalSale = 0, totalSummary = 0, totalFOC = 0; 
            long totalDiscount = 0;
            int totalOtherFOC = 0;
            #region Transaction
            dsReportTemp dsReport = new dsReportTemp();
            dsReportTemp.TransactionListDataTable dtTransactionReport = (dsReportTemp.TransactionListDataTable)dsReport.Tables["TransactionList"];

            foreach (Transaction transaction in transList)
            {
                dsReportTemp.TransactionListRow newRow = dtTransactionReport.NewTransactionListRow();
                newRow.TransactionId = transaction.Id;
                newRow.Date = Convert.ToDateTime(transaction.DateTime);
                newRow.SalePerson = transaction.User.Name;
                newRow.PaymentMethod = transaction.PaymentType.Name;
                newRow.Amount = Convert.ToInt32(transaction.TotalAmount);
                newRow.DiscountAmount = transaction.DiscountAmount.ToString();
                newRow.Type = transaction.Type;
                //newRow.Qty = Convert.ToInt32(transaction.TransactionDetails.Sum(x => x.Qty));
                newRow.Qty = Convert.ToInt32(transaction.TransactionDetails.Where(x => x.IsDeleted == false).Sum(x => x.Qty));
                totalSale += Convert.ToInt32(transaction.TotalAmount);

                totalOtherFOC    += Convert.ToInt32(transaction.TransactionDetails.Where(x=>x.IsFOC==true).Sum((x=>x.SellingPrice * x.Qty)));
                dtTransactionReport.AddTransactionListRow(newRow);
            }
            ReportDataSource rds1 = new ReportDataSource("SaleDataSet", dsReport.Tables["TransactionList"]);
            #endregion
            #region Get Discount Value

            DateTime fromDate = dtpFrom.Value.Date;
            DateTime toDate = dtpTo.Value.Date;
            int CashierId = 0;
            int CounterId = 0;

            List<Transaction> discounttransList = new List<Transaction>();

            //If user use filter for both Counter and Casher
            if (cboCounter.SelectedIndex > 0 && cboCashier.SelectedIndex > 0)
            {
                CounterId = Convert.ToInt32(cboCounter.SelectedValue);
                CashierId = Convert.ToInt32(cboCashier.SelectedValue);

                discounttransList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.CounterId == CounterId && t.UserId == CashierId && t.IsComplete == true && t.IsActive == true && t.Type == TransactionType.Sale && (t.IsDeleted == null || t.IsDeleted == false) && t.Id.Substring(2,2)==currentshop.ShortCode select t).ToList<Transaction>();
            }
            // User just use Counter filter
            else if (cboCounter.SelectedIndex > 0)
            {
                CounterId = Convert.ToInt32(cboCounter.SelectedValue);
                discounttransList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.CounterId == CounterId && t.IsComplete == true && t.IsActive == true && t.Type == TransactionType.Sale && (t.IsDeleted == null || t.IsDeleted == false) && t.Id.Substring(2, 2) == currentshop.ShortCode select t).ToList<Transaction>();
            }
            // User just use Casher filter
            else if (cboCashier.SelectedIndex > 0)
            {
                CashierId = Convert.ToInt32(cboCashier.SelectedValue);
                discounttransList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.UserId == CashierId && t.IsComplete == true && t.IsActive == true && t.Type == TransactionType.Sale && (t.IsDeleted == null || t.IsDeleted == false) && t.Id.Substring(2, 2) == currentshop.ShortCode select t).ToList<Transaction>();
            }
            // User ignore both filter
            else
            {
                discounttransList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && t.Type == TransactionType.Sale && (t.IsDeleted == null || t.IsDeleted == false) && t.Id.Substring(2, 2) == currentshop.ShortCode select t).ToList<Transaction>();
            }

            foreach (Transaction t in discounttransList)
            {
                long itemdiscount = (long)t.TransactionDetails.Sum(x => (x.UnitPrice * (x.DiscountRate / 100)) * x.Qty);
                totalDiscount += (long)t.DiscountAmount - itemdiscount;

               

            }

     
            #region FOC
            dsReportTemp dsReportFOCTransaction = new dsReportTemp();
            dsReportTemp.TransactionListDataTable dtTransactionReportFOC = (dsReportTemp.TransactionListDataTable)dsReportFOCTransaction.Tables["TransactionList"];
            foreach (Transaction transaction in FOCtrnsList)
            {
                dsReportTemp.TransactionListRow newRow = dtTransactionReportFOC.NewTransactionListRow();
                newRow.TransactionId = transaction.Id;
                newRow.Date = Convert.ToDateTime(transaction.DateTime);
                newRow.SalePerson = transaction.User.Name;
                newRow.PaymentMethod = transaction.PaymentType.Name;
                //newRow.Amount = Convert.ToInt32(transaction.TotalAmount);
                newRow.Amount = Convert.ToInt32(transaction.TransactionDetails.Sum((x => x.SellingPrice * x.Qty)));
                newRow.DiscountAmount = transaction.DiscountAmount.ToString();
                newRow.Type = transaction.Type;
              //  newRow.Qty = Convert.ToInt32(transaction.TransactionDetails.Sum(x => x.Qty));
                newRow.Qty = Convert.ToInt32(transaction.TransactionDetails.Where(x => x.IsDeleted == false).Sum(x => x.Qty));
                //totalFOC += Convert.ToInt32(transaction.TotalAmount);
                totalFOC += Convert.ToInt32(transaction.TransactionDetails.Sum((x=>x.SellingPrice * x.Qty)));
                dtTransactionReportFOC.AddTransactionListRow(newRow);
            }
            ReportDataSource rds8 = new ReportDataSource("FOCDataSet", dsReportFOCTransaction.Tables["TransactionList"]);
            #endregion

          totalSummary = ((totalSale ) - (totalFOC));
           string reportPath = Application.StartupPath + "\\Reports\\TransactionsDetailReport.rdlc";
            reportViewer1.LocalReport.ReportPath = reportPath;
            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(rds1);
            reportViewer1.LocalReport.DataSources.Add(rds8);
            //reportViewer1.LocalReport.DataSources.Add(rds9);

            ReportParameter TotalSales = new ReportParameter("TotalSale", totalSummary.ToString());
            reportViewer1.LocalReport.SetParameters(TotalSales);

            ReportParameter TotalDiscount = new ReportParameter("TotalDiscount", totalDiscount.ToString());
            reportViewer1.LocalReport.SetParameters(TotalDiscount);

            //ReportParameter TotalMCDiscount = new ReportParameter("TotalMCDiscount", totalMCDiscount.ToString());
            //reportViewer1.LocalReport.SetParameters(TotalMCDiscount);


            //ReportParameter TotalRefundDiscount = new ReportParameter("TotalRefundDiscount", totalRefundDiscount.ToString());
            //reportViewer1.LocalReport.SetParameters(TotalRefundDiscount);

            //ReportParameter TotalCreditRefundDiscount = new ReportParameter("TotalCreditRefundDiscount", totalCreditRefundDiscount.ToString());
            //reportViewer1.LocalReport.SetParameters(TotalCreditRefundDiscount);

            ReportParameter ActualAmount = new ReportParameter("ActualAmount", totalSale.ToString());
            reportViewer1.LocalReport.SetParameters(ActualAmount);

            ReportParameter TotalFOC = new ReportParameter("TotalFOC", totalFOC.ToString());
            reportViewer1.LocalReport.SetParameters(TotalFOC);

            ReportParameter TotalOtherFOC = new ReportParameter("TotalOtherFOC", totalOtherFOC.ToString());
            reportViewer1.LocalReport.SetParameters(TotalOtherFOC);

            //ReportParameter TotalMPU = new ReportParameter("TotalMPU", totalMPU.ToString());
            //reportViewer1.LocalReport.SetParameters(TotalMPU);

            
            //ReportParameter CreditRecieve = new ReportParameter("CreditRecieve", totalCreditRecieve.ToString());
            //reportViewer1.LocalReport.SetParameters(CreditRecieve);

            //ReportParameter Expense = new ReportParameter("Expense", totalExpense.ToString());
            //reportViewer1.LocalReport.SetParameters(Expense);

            ReportParameter IncomeAmount = new ReportParameter("IncomeAmount", totalSale.ToString());
            reportViewer1.LocalReport.SetParameters(IncomeAmount);

            ReportParameter CashInHand = new ReportParameter("CashInHand", totalSale.ToString());
            reportViewer1.LocalReport.SetParameters(CashInHand);

            //ReportParameter TotalDebt = new ReportParameter("TotalDebt", totalDebt.ToString());
            //reportViewer1.LocalReport.SetParameters(TotalDebt);

            //ReportParameter TotalRefund = new ReportParameter("TotalRefund", totalRefund.ToString());
            //reportViewer1.LocalReport.SetParameters(TotalRefund);

            ReportParameter TotalSummary = new ReportParameter("TotalSummary", totalSummary.ToString());
            reportViewer1.LocalReport.SetParameters(TotalSummary);

            //ReportParameter TotalCreditRefund = new ReportParameter("TotalCreditRefund", totalCreditRefund.ToString());
            //reportViewer1.LocalReport.SetParameters(TotalCreditRefund);

            //ReportParameter TotalGiftCard = new ReportParameter("TotalGiftCard", totalGiftCard .ToString());
            //reportViewer1.LocalReport.SetParameters(TotalGiftCard);

            //ReportParameter TotalCredit = new ReportParameter("TotalCredit", totalCredit.ToString());
            //reportViewer1.LocalReport.SetParameters(TotalCredit);

            ReportParameter HeaderTitle = new ReportParameter("HeaderTitle", "Transaction Summary for " + currentshop.ShopName);
            reportViewer1.LocalReport.SetParameters(HeaderTitle);

            ReportParameter Date = new ReportParameter("Date", " From " + dtpFrom.Value.Date.ToString(SettingController.GlobalDateFormat) + " To " + dtpTo.Value.Date.ToString(SettingController.GlobalDateFormat));
            reportViewer1.LocalReport.SetParameters(Date);

            //ReportParameter TesterTotal = new ReportParameter("TesterTotal", totalTester.ToString());
            //reportViewer1.LocalReport.SetParameters(TesterTotal);


            reportViewer1.RefreshReport();
        }




        #endregion

        private void btnSearch_Click(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
