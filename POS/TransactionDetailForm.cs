using Microsoft.Reporting.WinForms;
using POS.APP_Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace POS
{
    public partial class TransactionDetailForm : Form
    {
        #region Variable

        private POSEntities entity = new POSEntities();
        public string transactionId;
        public string transactionDetailId;
        //int ExtraDiscount, ExtraTax;
        //private int CustomerId = 0;
        public int shopid;
        public bool delete = false;
        public bool DeleteLink = true;
        public DateTime date;
        //List<Stock_Transaction> productList = new List<Stock_Transaction>();

        #endregion

        #region Event

        public TransactionDetailForm()
        {
            InitializeComponent();
        }

        private void TransactionDetailForm_Load(object sender, EventArgs e)
        {
            Localization.Localize_FormControls(this);
            //Localization.Localize_FormControls(this);
            LoadData();
        }

        private void dgvTransactionDetail_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow row in dgvTransactionDetail.Rows)
            {
                TransactionDetail transactionDetailObj = (TransactionDetail)row.DataBoundItem;
                row.Cells[0].Value = transactionDetailObj.Product.ProductCode;
                row.Cells[1].Value = transactionDetailObj.Product.Name;
                row.Cells[2].Value = transactionDetailObj.Qty;


                string tranId = transactionDetailObj.TransactionId;


                row.Cells[3].Value = transactionDetailObj.UnitPrice;
                row.Cells[4].Value = transactionDetailObj.DiscountRate + "%";
                row.Cells[5].Value = transactionDetailObj.TotalAmount;

                if (transactionDetailObj.IsFOC == true)
                {
                    row.Cells[7].Value = "FOC";
                }
                else
                {
                    row.Cells[7].Value = "";
                }
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            string _defaultPrinter = Utility.GetDefaultPrinter();
            int _tAmt = 0;
            Transaction transactionObj = (from t in entity.Transactions where t.Id == transactionId select t).FirstOrDefault();

            string tranId = transactionObj.Id;

            List<TransactionDetail> _tdList = (from td in transactionObj.TransactionDetails where td.IsDeleted == false select td).ToList();


            Int64 totalAmountRep = 0;

            if (transactionObj.PaymentTypeId == 1)
            {
                #region [ Print ] for Cash


                dsReportTemp dsReport = new dsReportTemp();
                dsReportTemp.ItemListDataTable dtReport = (dsReportTemp.ItemListDataTable)dsReport.Tables["ItemList"];
                //List<TransactionDetail> _tdList = (from td in transactionObj.TransactionDetails where td.IsDeleted == false select td).ToList();



                foreach (TransactionDetail transaction in _tdList)
                {
                    dsReportTemp.ItemListRow newRow = dtReport.NewItemListRow();
                    newRow.ItemId = transaction.Product.ProductCode;
                    newRow.Name = transaction.Product.Name;
                    newRow.Qty = transaction.Qty.ToString();
                    newRow.PhotoPath = string.IsNullOrEmpty(transaction.Product.PhotoPath) ? "" : Application.StartupPath + transaction.Product.PhotoPath.Remove(0, 1);


                    //newRow.TotalAmount = (int)transaction.TotalAmount; //Edit By ZMH
                    //ZP(TDO)
                    newRow.DiscountPercent = transaction.DiscountRate.ToString();
                    //  newRow.DiscountPercent = Convert.ToInt32(transaction.DiscountRate).ToString();
                    newRow.TotalAmount = (int)transaction.UnitPrice * (int)transaction.Qty; //Edit By ZMH


                    if (transaction.IsFOC == true)
                    {
                        newRow.IsFOC = "FOC";
                    }

                    switch (Utility.GetDefaultPrinter())
                    {
                        case "A4 Printer":
                            newRow.UnitPrice = transaction.UnitPrice.ToString();
                            break;
                        case "Slip Printer":
                            newRow.UnitPrice = "1@" + transaction.UnitPrice.ToString();
                            break;
                    }
                    //   _tAmt += newRow.TotalAmount;

                    // dtReport.AddItemListRow(newRow);
                    // unitpriceTotalCost = (int)transaction.UnitPrice * (int)transaction.Qty;                    
                }


                if (dtReport.Count > 0)
                {

                    string reportPath = "";
                    ReportViewer rv = new ReportViewer();
                    ReportDataSource rds = new ReportDataSource("DataSet1", dsReport.Tables["ItemList"]);

                    reportPath = Application.StartupPath + Utility.GetReportPath("Cash");
                    rv.Reset();
                    rv.LocalReport.ReportPath = reportPath;
                    rv.LocalReport.DataSources.Add(rds);


                    Utility.Slip_Log(rv);
                    //switch (_defaultPrinter)
                    //{

                    //    case "Slip Printer":
                    //        Utility.Slip_A4_Footer(rv);
                    //        break;
                    //}
                    Utility.Slip_A4_Footer(rv);

                    //ReportParameter CustomerName = new ReportParameter("CustomerName", cus.Name);
                    //rv.LocalReport.SetParameters(CustomerName);


                    //string _tAmt1 = string.Format("{0:#,##0.00}", _tAmt);
                    //ReportParameter TAmt = new ReportParameter("TAmt", _tAmt1);
                    //rv.LocalReport.SetParameters(TAmt);
                    ReportParameter TAmt = new ReportParameter("TAmt", _tAmt.ToString());
                    rv.LocalReport.SetParameters(TAmt);
                    if (SettingController.SelectDefaultPrinter != "Slip Printer")
                    {
                        ReportParameter PrintProductImage = new ReportParameter("PrintImage", SettingController.ShowProductImageIn_A4Reports.ToString());
                        rv.LocalReport.SetParameters(PrintProductImage);
                    }

                    if (transactionObj.Note.ToString() == "")
                    {
                        ReportParameter Notes = new ReportParameter("Notes", "");
                        rv.LocalReport.SetParameters(Notes);
                    }
                    else
                    {
                        ReportParameter Notes = new ReportParameter("Notes", transactionObj.Note.ToString());
                        rv.LocalReport.SetParameters(Notes);
                    }
                    ReportParameter ShopName = new ReportParameter("ShopName", SettingController.ShopName);
                    rv.LocalReport.SetParameters(ShopName);

                    ReportParameter BranchName = new ReportParameter("BranchName", SettingController.BranchName);
                    rv.LocalReport.SetParameters(BranchName);

                    ReportParameter Phone = new ReportParameter("Phone", SettingController.PhoneNo);
                    rv.LocalReport.SetParameters(Phone);

                    ReportParameter OpeningHours = new ReportParameter("OpeningHours", SettingController.OpeningHours);
                    rv.LocalReport.SetParameters(OpeningHours);

                    ReportParameter TransactionId = new ReportParameter("TransactionId", transactionId.ToString());
                    rv.LocalReport.SetParameters(TransactionId);

                    APP_Data.Counter c = entity.Counters.FirstOrDefault(x => x.Id == MemberShip.CounterId);

                    ReportParameter CounterName = new ReportParameter("CounterName", c.Name);
                    rv.LocalReport.SetParameters(CounterName);

                    ReportParameter PrintDateTime = new ReportParameter();
                    switch (Utility.GetDefaultPrinter())
                    {
                        case "A4 Printer":
                            PrintDateTime = new ReportParameter("PrintDateTime", Convert.ToDateTime(transactionObj.DateTime).ToString("dd/MMM/yyyy"));
                            rv.LocalReport.SetParameters(PrintDateTime);
                            break;
                        case "Slip Printer":
                            PrintDateTime = new ReportParameter("PrintDateTime", Convert.ToDateTime(transactionObj.DateTime).ToString("dd/MMM/yyyy hh:mm"));
                            rv.LocalReport.SetParameters(PrintDateTime);
                            break;
                    }

                    ReportParameter CasherName = new ReportParameter("CasherName", MemberShip.UserName);
                    rv.LocalReport.SetParameters(CasherName);

                    //ReportParameter TotalAmount = new ReportParameter("TotalAmount", transactionObj.TotalAmount.ToString()); //Edit By ZMH
                    //rv.LocalReport.SetParameters(TotalAmount);
                    //Int64 totalAmountRep = transactionObj.TotalAmount == null ? 0 : Convert.ToInt64(transactionObj.TotalAmount);
                    totalAmountRep = (_tAmt - Convert.ToInt32(transactionObj.DiscountAmount));
                    ReportParameter TotalAmount = new ReportParameter("TotalAmount", (totalAmountRep).ToString());
                    rv.LocalReport.SetParameters(TotalAmount);


                    if (transactionObj.DiscountAmount == 0)
                    {
                        ReportParameter DiscountAmount = new ReportParameter("DiscountAmount", transactionObj.DiscountAmount.ToString());
                        rv.LocalReport.SetParameters(DiscountAmount);
                    }
                    else
                    {
                        ReportParameter DiscountAmount = new ReportParameter("DiscountAmount", "-" + transactionObj.DiscountAmount.ToString());
                        rv.LocalReport.SetParameters(DiscountAmount);
                    }
                    ReportParameter PaidAmount = new ReportParameter("PaidAmount", transactionObj.RecieveAmount.ToString());
                    rv.LocalReport.SetParameters(PaidAmount);

                    //  ReportParameter Change = new ReportParameter("Change",(transactionObj.RecieveAmount - (transactionObj.TotalAmount - ExtraDiscount + ExtraTax)).ToString());//(amount - (DetailList.Sum(x => x.TotalAmount) - ExtraDiscount + ExtraTax))
                    ReportParameter Change = new ReportParameter("Change", (transactionObj.RecieveAmount - totalAmountRep).ToString());//(amount - (DetailList.Sum(x => x.TotalAmount) - ExtraDiscount + ExtraTax))
                    rv.LocalReport.SetParameters(Change);





                    //if (Utility.GetDefaultPrinter() == "A4 Printer")
                    //{
                    //    ReportParameter CusAddress = new ReportParameter("CusAddress", cus.Address);
                    //    rv.LocalReport.SetParameters(CusAddress);
                    //}

                    var b = rv.LocalReport.GetParameters();
                    foreach (var item in b)
                    {

                    }

                    ////       PrintDoc.PrintReport(rv, Utility.GetDefaultPrinter());
                    //PrintDoc.PrintReport(rv, "Slip");
                    Utility.Get_Print(rv);
                }
                else
                {
                    MessageBox.Show("Invoice No." + tranId + "  is already made refund all items.", "mPOS");
                }
                #endregion
            }
            else if (transactionObj.PaymentTypeId == 5)
            {
                #region [ Print ] for MPU


                dsReportTemp dsReport = new dsReportTemp();
                dsReportTemp.ItemListDataTable dtReport = (dsReportTemp.ItemListDataTable)dsReport.Tables["ItemList"];
                //List<TransactionDetail> _tdList = (from td in transactionObj.TransactionDetails where td.IsDeleted == false select td).ToList();

                foreach (TransactionDetail transaction in _tdList)
                {
                    dsReportTemp.ItemListRow newRow = dtReport.NewItemListRow();
                    newRow.Name = transaction.Product.Name;
                    newRow.Qty = transaction.Qty.ToString();
                    newRow.PhotoPath = string.IsNullOrEmpty(transaction.Product.PhotoPath) ? "" : Application.StartupPath + transaction.Product.PhotoPath.Remove(0, 1);
                    //newRow.TotalAmount = (int)transaction.TotalAmount; //Edit By ZMH
                    newRow.DiscountPercent = transaction.DiscountRate.ToString();
                    newRow.TotalAmount = (int)transaction.UnitPrice * (int)transaction.Qty; //Edit By ZMH

                    if (transaction.IsFOC == true)
                    {
                        newRow.IsFOC = "FOC";
                    }

                    switch (Utility.GetDefaultPrinter())
                    {
                        case "A4 Printer":
                            newRow.UnitPrice = transaction.UnitPrice.ToString();
                            break;
                        case "Slip Printer":
                            newRow.UnitPrice = "1@" + transaction.UnitPrice.ToString();
                            break;
                    }


                    _tAmt += newRow.TotalAmount;

                    dtReport.AddItemListRow(newRow);
                    //   unitpriceTotalCost = (int)transaction.UnitPrice * (int)transaction.Qty;
                }

                if (dtReport.Count > 0)
                {
                    string reportPath = "";
                    ReportViewer rv = new ReportViewer();
                    ReportDataSource rds = new ReportDataSource("DataSet1", dsReport.Tables["ItemList"]);
                    reportPath = Application.StartupPath + Utility.GetReportPath("MPU");
                    rv.Reset();
                    rv.LocalReport.ReportPath = reportPath;
                    rv.LocalReport.DataSources.Add(rds);

                    Utility.Slip_Log(rv);
                    ////switch (_defaultPrinter)
                    ////{

                    ////    case "Slip Printer":
                    ////        Utility.Slip_A4_Footer(rv);
                    ////        break;
                    ////}

                    Utility.Slip_A4_Footer(rv);

                    //ReportParameter CustomerName = new ReportParameter("CustomerName", cus.Name);
                    //rv.LocalReport.SetParameters(CustomerName);


                    ReportParameter TAmt = new ReportParameter("TAmt", _tAmt.ToString());
                    rv.LocalReport.SetParameters(TAmt);
                    if (SettingController.SelectDefaultPrinter != "Slip Printer")
                    {
                        ReportParameter PrintProductImage = new ReportParameter("PrintImage", SettingController.ShowProductImageIn_A4Reports.ToString());
                        rv.LocalReport.SetParameters(PrintProductImage);
                    }
                    if (transactionObj.Note.ToString() == "")
                    {
                        ReportParameter Notes = new ReportParameter("Notes", " ");
                        rv.LocalReport.SetParameters(Notes);
                    }
                    else
                    {
                        ReportParameter Notes = new ReportParameter("Notes", transactionObj.Note.ToString());
                        rv.LocalReport.SetParameters(Notes);
                    }
                    ReportParameter ShopName = new ReportParameter("ShopName", SettingController.ShopName);
                    rv.LocalReport.SetParameters(ShopName);

                    ReportParameter BranchName = new ReportParameter("BranchName", SettingController.BranchName);
                    rv.LocalReport.SetParameters(BranchName);

                    ReportParameter Phone = new ReportParameter("Phone", SettingController.PhoneNo);
                    rv.LocalReport.SetParameters(Phone);

                    ReportParameter OpeningHours = new ReportParameter("OpeningHours", SettingController.OpeningHours);
                    rv.LocalReport.SetParameters(OpeningHours);

                    ReportParameter TransactionId = new ReportParameter("TransactionId", transactionId.ToString());
                    rv.LocalReport.SetParameters(TransactionId);

                    APP_Data.Counter c = entity.Counters.FirstOrDefault(x => x.Id == MemberShip.CounterId);

                    ReportParameter CounterName = new ReportParameter("CounterName", c.Name);
                    rv.LocalReport.SetParameters(CounterName);
                    ReportParameter PrintDateTime = new ReportParameter();
                    switch (Utility.GetDefaultPrinter())
                    {
                        case "A4 Printer":
                            PrintDateTime = new ReportParameter("PrintDateTime", Convert.ToDateTime(transactionObj.DateTime).ToString(SettingController.GlobalDateFormat));
                            rv.LocalReport.SetParameters(PrintDateTime);
                            break;
                        case "Slip Printer":
                            PrintDateTime = new ReportParameter("PrintDateTime", Convert.ToDateTime(transactionObj.DateTime).ToString(SettingController.GlobalDateFormat + " hh:mm"));
                            rv.LocalReport.SetParameters(PrintDateTime);
                            break;
                    }


                    ReportParameter CasherName = new ReportParameter("CasherName", MemberShip.UserName);
                    rv.LocalReport.SetParameters(CasherName);


                    totalAmountRep = (_tAmt + Convert.ToInt32(transactionObj.DiscountAmount));
                    ReportParameter TotalAmount = new ReportParameter("TotalAmount", totalAmountRep.ToString());
                    rv.LocalReport.SetParameters(TotalAmount);
                    if (Convert.ToInt32(transactionObj.DiscountAmount) == 0)
                    {
                        ReportParameter DiscountAmount = new ReportParameter("DiscountAmount", transactionObj.DiscountAmount.ToString());
                        rv.LocalReport.SetParameters(DiscountAmount);
                    }
                    else
                    {
                        ReportParameter DiscountAmount = new ReportParameter("DiscountAmount", "-" + transactionObj.DiscountAmount.ToString());
                        rv.LocalReport.SetParameters(DiscountAmount);
                    }

                    ReportParameter PaidAmount = new ReportParameter("PaidAmount", transactionObj.RecieveAmount.ToString());
                    rv.LocalReport.SetParameters(PaidAmount);

                    ReportParameter Change = new ReportParameter("Change", "0");//(amount - (DetailList.Sum(x => x.TotalAmount) - ExtraDiscount + ExtraTax))
                    rv.LocalReport.SetParameters(Change);



                    //if (Utility.GetDefaultPrinter() == "A4 Printer")
                    //{
                    //    ReportParameter CusAddress = new ReportParameter("CusAddress", transactionObj.Customer.Address);
                    //    rv.LocalReport.SetParameters(CusAddress);
                    //}


                    ////  PrintDoc.PrintReport(rv, Utility.GetDefaultPrinter());
                    Utility.Get_Print(rv);
                }
                else
                {
                    MessageBox.Show("Invoice No." + tranId + "  is already made refund all items.", "mPOS");
                }
                #endregion
            }
            else if (transactionObj.PaymentTypeId == 4)
            {
                #region [ Print ] for FOC


                dsReportTemp dsReport = new dsReportTemp();
                dsReportTemp.ItemListDataTable dtReport = (dsReportTemp.ItemListDataTable)dsReport.Tables["ItemList"];
                //List<TransactionDetail> _tdList = (from td in transactionObj.TransactionDetails where td.IsDeleted == false select td).ToList();

                foreach (TransactionDetail transaction in _tdList)
                {
                    dsReportTemp.ItemListRow newRow = dtReport.NewItemListRow();
                    newRow.Name = transaction.Product.Name;
                    newRow.Qty = transaction.Qty.ToString();
                    newRow.PhotoPath = string.IsNullOrEmpty(transaction.Product.PhotoPath) ? "" : Application.StartupPath + transaction.Product.PhotoPath.Remove(0, 1);
                    //newRow.TotalAmount = (int)transaction.TotalAmount; //Edit By ZMH
                    newRow.DiscountPercent = transaction.DiscountRate.ToString();
                    newRow.TotalAmount = (int)transaction.UnitPrice * (int)transaction.Qty; //Edit By ZMH
                    switch (Utility.GetDefaultPrinter())
                    {
                        case "A4 Printer":
                            //newRow.UnitPrice = transaction.UnitPrice.ToString();
                            newRow.UnitPrice = transaction.SellingPrice.ToString();
                            break;
                        case "Slip Printer":
                            //  newRow.UnitPrice = "1@" + transaction.UnitPrice.ToString();
                            newRow.UnitPrice = "1@" + transaction.SellingPrice.ToString();
                            break;
                    }

                    _tAmt += newRow.TotalAmount;

                    dtReport.AddItemListRow(newRow);
                }

                if (dtReport.Count > 0)
                {
                    string reportPath = "";
                    ReportViewer rv = new ReportViewer();
                    ReportDataSource rds = new ReportDataSource("DataSet1", dsReport.Tables["ItemList"]);
                    reportPath = Application.StartupPath + Utility.GetReportPath("FOC");
                    rv.Reset();
                    rv.LocalReport.ReportPath = reportPath;
                    rv.LocalReport.DataSources.Add(rds);

                    Utility.Slip_Log(rv);
                    //switch (_defaultPrinter)
                    //{

                    //    case "Slip Printer":
                    //        Utility.Slip_A4_Footer(rv);
                    //        break;
                    //}
                    Utility.Slip_A4_Footer(rv);

                    //ReportParameter CustomerName = new ReportParameter("CustomerName", cus.Name);
                    //rv.LocalReport.SetParameters(CustomerName);

                    ReportParameter TAmt = new ReportParameter("TAmt", _tAmt.ToString());
                    rv.LocalReport.SetParameters(TAmt);
                    if (SettingController.SelectDefaultPrinter != "Slip Printer")
                    {
                        ReportParameter PrintProductImage = new ReportParameter("PrintImage", SettingController.ShowProductImageIn_A4Reports.ToString());
                        rv.LocalReport.SetParameters(PrintProductImage);
                    }
                    if (transactionObj.Note.ToString() == "")
                    {
                        ReportParameter Notes = new ReportParameter("Notes", " ");
                        rv.LocalReport.SetParameters(Notes);
                    }
                    else
                    {
                        ReportParameter Notes = new ReportParameter("Notes", transactionObj.Note.ToString());
                        rv.LocalReport.SetParameters(Notes);
                    }
                    ReportParameter ShopName = new ReportParameter("ShopName", SettingController.ShopName);
                    rv.LocalReport.SetParameters(ShopName);

                    ReportParameter BranchName = new ReportParameter("BranchName", SettingController.BranchName);
                    rv.LocalReport.SetParameters(BranchName);

                    ReportParameter Phone = new ReportParameter("Phone", SettingController.PhoneNo);
                    rv.LocalReport.SetParameters(Phone);

                    ReportParameter OpeningHours = new ReportParameter("OpeningHours", SettingController.OpeningHours);
                    rv.LocalReport.SetParameters(OpeningHours);

                    ReportParameter TransactionId = new ReportParameter("TransactionId", transactionId.ToString());
                    rv.LocalReport.SetParameters(TransactionId);

                    APP_Data.Counter c = entity.Counters.FirstOrDefault(x => x.Id == MemberShip.CounterId);

                    ReportParameter CounterName = new ReportParameter("CounterName", c.Name);
                    rv.LocalReport.SetParameters(CounterName);

                    ReportParameter PrintDateTime = new ReportParameter();
                    switch (Utility.GetDefaultPrinter())
                    {
                        case "A4 Printer":
                            PrintDateTime = new ReportParameter("PrintDateTime", Convert.ToDateTime(transactionObj.DateTime).ToString(SettingController.GlobalDateFormat));
                            rv.LocalReport.SetParameters(PrintDateTime);
                            break;
                        case "Slip Printer":
                            PrintDateTime = new ReportParameter("PrintDateTime", Convert.ToDateTime(transactionObj.DateTime).ToString(SettingController.GlobalDateFormat + " hh:mm"));
                            rv.LocalReport.SetParameters(PrintDateTime);
                            break;
                    }

                    ReportParameter CasherName = new ReportParameter("CasherName", MemberShip.UserName);
                    rv.LocalReport.SetParameters(CasherName);

                    totalAmountRep = (_tAmt + Convert.ToInt32(transactionObj.DiscountAmount));
                    ReportParameter TotalAmount = new ReportParameter("TotalAmount", totalAmountRep.ToString());
                    rv.LocalReport.SetParameters(TotalAmount);

                    if (Convert.ToInt32(transactionObj.DiscountAmount) == 0)
                    {
                        ReportParameter DiscountAmount = new ReportParameter("DiscountAmount", Convert.ToInt32(transactionObj.DiscountAmount).ToString());
                        rv.LocalReport.SetParameters(DiscountAmount);
                    }
                    else
                    {
                        ReportParameter DiscountAmount = new ReportParameter("DiscountAmount", "-" + Convert.ToInt32(transactionObj.DiscountAmount).ToString());
                        rv.LocalReport.SetParameters(DiscountAmount);
                    }

                    ReportParameter PaidAmount = new ReportParameter("PaidAmount", transactionObj.RecieveAmount.ToString());
                    rv.LocalReport.SetParameters(PaidAmount);

                    //ReportParameter Change = new ReportParameter("Change", (transactionObj.RecieveAmount - (transactionObj.TotalAmount - ExtraDiscount + ExtraTax)).ToString());//(amount - (DetailList.Sum(x => x.TotalAmount) - ExtraDiscount + ExtraTax))
                    ReportParameter Change = new ReportParameter("Change", "0");//(amount - (DetailList.Sum(x => x.TotalAmount) - ExtraDiscount + ExtraTax))
                    rv.LocalReport.SetParameters(Change);

                    //if (Utility.GetDefaultPrinter() == "A4 Printer")
                    //{
                    //    ReportParameter CusAddress = new ReportParameter("CusAddress", transactionObj.Customer.Address);
                    //    rv.LocalReport.SetParameters(CusAddress);
                    //}
                    //// PrintDoc.PrintReport(rv, Utility.GetDefaultPrinter());
                    Utility.Get_Print(rv);
                }
                else
                {
                    MessageBox.Show("Invoice No." + tranId + "  is already made refund all items.", "mPOS");
                }
                #endregion
            }

        }

        private void dgvTransactionDetail_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int currentTransactionId = Convert.ToInt32(dgvTransactionDetail.Rows[e.RowIndex].Cells[6].Value.ToString());
                //Delete the record and add delete log
                if (e.ColumnIndex == 8)
                {

                    if (!DeleteLink)
                    {
                        dgvTransactionDetail.Rows[e.RowIndex].Cells[8].ReadOnly = true;
                        return;
                    }

                    APP_Data.TransactionDetail tdOBj = new TransactionDetail();
                    APP_Data.Transaction tObj = new Transaction();

                    DialogResult result = MessageBox.Show("Are you sure you want to delete?", "Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                    if (result.Equals(DialogResult.OK))
                    {
                        tdOBj = entity.TransactionDetails.Where(x => x.Id == currentTransactionId).FirstOrDefault();
                        if (tdOBj != null)
                        {
                            if (dgvTransactionDetail.Rows.Count <= 1)
                            {
                                DialogResult result2 = MessageBox.Show("You have only one record!.If you delete this,system will automatically delete Transaction of this record", "Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                                if (result2.Equals(DialogResult.OK))
                                {
                                    TransactionDetail ts = entity.TransactionDetails.Where(x => x.Id == currentTransactionId).FirstOrDefault();
                                    Transaction t = entity.Transactions.Where(x => x.Id == ts.TransactionId).FirstOrDefault();


                                    t.IsDeleted = true;
                                    foreach (TransactionDetail td in t.TransactionDetails)
                                    {
                                        //td.IsDeleted = false;
                                        td.IsDeleted = true;
                                    }

                                    Ticket ti = entity.Tickets.Where(x => x.TransactionDetailId == ts.Id).FirstOrDefault();
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



                                    DeleteLog dl = new DeleteLog();
                                    dl.DeletedDate = DateTime.Now;
                                    dl.CounterId = MemberShip.CounterId;
                                    dl.UserId = MemberShip.UserId;
                                    dl.IsParent = true;
                                    dl.TransactionId = t.Id;
                                    //dl.TransactionDetailId = ts.Id;



                                    List<DeleteLog> delist = entity.DeleteLogs.Where(x => x.TransactionId == t.Id && x.TransactionDetailId != null && x.IsParent == false).ToList();

                                    foreach (DeleteLog d in delist)
                                    {
                                        entity.DeleteLogs.Remove(d);
                                    }
                                    entity.DeleteLogs.Add(dl);
                                    entity.SaveChanges();
                                    LoadData();
                                    this.Close();

                                    if (System.Windows.Forms.Application.OpenForms["TransactionList"] != null)
                                    {
                                        TransactionList newForm = (TransactionList)System.Windows.Forms.Application.OpenForms["TransactionList"];
                                        newForm.LoadData();
                                    }
                                }
                            }
                            else
                            {
                                TransactionDetail ts = entity.TransactionDetails.Where(x => x.Id == currentTransactionId).FirstOrDefault();
                                Transaction t = entity.Transactions.Where(x => x.Id == ts.TransactionId).FirstOrDefault();

                                ts.IsDeleted = true;




                                DeleteLog dl = new DeleteLog();
                                dl.DeletedDate = DateTime.Now;
                                dl.CounterId = MemberShip.CounterId;
                                dl.UserId = MemberShip.UserId;
                                dl.IsParent = false;
                                dl.TransactionId = ts.TransactionId;
                                dl.TransactionDetailId = ts.Id;

                                Transaction ParentTransaction = entity.Transactions.Where(x => x.Id == ts.TransactionId).FirstOrDefault();
                                ParentTransaction.TotalAmount = ParentTransaction.TotalAmount - ts.TotalAmount;

                                int _disAmt = Convert.ToInt32((ts.UnitPrice / 100) * ts.DiscountRate);
                                ParentTransaction.DiscountAmount = Convert.ToInt32(ParentTransaction.DiscountAmount - _disAmt);

                                entity.DeleteLogs.Add(dl);
                                entity.SaveChanges();



                                LoadData();
                                if (System.Windows.Forms.Application.OpenForms["TransactionList"] != null)
                                {
                                    TransactionList newForm = (TransactionList)System.Windows.Forms.Application.OpenForms["TransactionList"];
                                    newForm.LoadData();
                                }


                            }
                        }
                    }
                }
            }
        }


        #endregion

        #region Function
        //private void Visible_Prepaid(bool v)
        //{
        //    lblPrevTitle.Visible = v;
        //    label19.Visible = v;
        //    lblOutstandingAmount.Visible = v;
        //}

        private void LoadData()
        {
            bool optionvisible = true;//Utility.TransactionDelRefHide(shopid);
            dgvTransactionDetail.Columns[8].Visible = optionvisible;

            dgvTransactionDetail_CustomCellFormatting();
            dgvTransactionDetail.AutoGenerateColumns = false;
            //tlpCredit.Visible = false;
            //Visible_Prepaid(false);
            Transaction transactionObject = (from t in entity.Transactions where t.Id == transactionId select t).FirstOrDefault();
            lblSalePerson.Text = (transactionObject.User == null) ? "-" : transactionObject.User.Name;
            lblDate.Text = transactionObject.DateTime.Value.ToString(SettingController.GlobalDateFormat);

            date = transactionObject.DateTime.Value;
            lblTime.Text = transactionObject.DateTime.Value.ToString("hh:mm");
            txtNote.Text = transactionObject.Note;

            List<TransactionDetail> _tdList = new List<TransactionDetail>();



            if (transactionObject.Type == TransactionType.Sale)
            {
                //dgvTransactionDetail.DataSource = transactionObject.TransactionDetails.Where(x=>x.IsDeleted != true).ToList();
                dgvTransactionDetail.DataSource = transactionObject.TransactionDetails.Where(x => x.IsDeleted == delete && x.ProductId != null).ToList();
                // lblRecieveAmunt.Text = transactionObject.RecieveAmount.ToString();
                int discount = 0;

                //List<TransactionDetail> _tdList = (from td in transactionObject.TransactionDetails where td.IsDeleted == false select td).ToList();
                _tdList = (from td in transactionObject.TransactionDetails where td.IsDeleted == delete && td.ProductId != null select td).ToList();
                foreach (TransactionDetail td in _tdList)
                {
                    discount += Convert.ToInt32(((td.UnitPrice) * (td.DiscountRate / 100)) * td.Qty);
                }
                lblDiscount.Text = (transactionObject.DiscountAmount).ToString();
                lblTotal.Text = transactionObject.TotalAmount.ToString();
                //ExtraDiscount = Convert.ToInt32(transactionObject.DiscountAmount - discount);



                lblPaymentMethod1.Text = (transactionObject.PaymentType == null) ? "-" : transactionObject.PaymentType.Name; ;

                tlpCash.Visible = true;

            }

            int itemDiscountAmt = 0;
            foreach (TransactionDetail td in transactionObject.TransactionDetails)
            {
                itemDiscountAmt += Convert.ToInt32(((td.UnitPrice) * (td.DiscountRate / 100)) * td.Qty);

            }




            //if (IsCash)
            //{
            //    lblTotal.Text = (transactionObject.TotalAmount - Convert.ToInt32(lblRefundAmt.Text)).ToString();
            //}
            //else
            //{
            //    lblTotal.Text = (transactionObject.RecieveAmount).ToString();
            //}

            lblRecieveAmunt.Text = transactionObject.RecieveAmount.ToString();

        }



        private void dgvTransactionDetail_CustomCellFormatting()
        {
            //Role Management
            RoleManagementController controller = new RoleManagementController();
            controller.Load(MemberShip.UserRoleId);
            // Transaction Delete
            if (!MemberShip.isAdmin && !controller.TransactionDetail.EditOrDelete)
            {
                dgvTransactionDetail.Columns["colDelete"].Visible = false;
            }

        }
        #endregion

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to update?", "Update", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (result.Equals(DialogResult.OK))
            {

                Transaction transactionObject = (from t in entity.Transactions where t.Id == transactionId select t).FirstOrDefault();
                transactionObject.UpdatedDate = DateTime.Now;
                entity.Entry(transactionObject).State = EntityState.Modified;
                entity.SaveChanges();
                MessageBox.Show("Successfully Updated!", "Update");
            }
        }

        private void lbAdvanceSearch_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //CustomerSearch form = new CustomerSearch();
            //form.ShowDialog();
        }


        private void TransactionDetailForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (System.Windows.Forms.Application.OpenForms["TransactionList"] != null)
            {
                TransactionList newForm = (TransactionList)System.Windows.Forms.Application.OpenForms["TransactionList"];
                newForm.LoadData();
            }
        }



    }
}
