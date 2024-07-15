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
    public partial class PaidByCash2 : Form
    {
        #region Variables
        public List<TransactionDetail> DetailList = new List<TransactionDetail>();

        public int Discount { get; set; }

       
        public bool IsFoc { get; set; }
        public int ExtraDiscount { get; set; }

        public string counterName { get; set; }
      
        
        private ToolTip tp = new ToolTip();

        public string Note = "";

        #endregion
        ApplicationLog al = new ApplicationLog();

        public PaidByCash2()
        {
            InitializeComponent();
        }

        #region Hot keys handler
        //void Form_KeyDown(object sender, KeyEventArgs e)
        //{
        //    if (e.Control && e.KeyCode == Keys.U)      //  Ctrl + U => Focus Currency
        //    {
        //        cboCurrency.DroppedDown = true;
        //        if (cboCurrency.Focused != true)
        //        {
        //            cboCurrency.Focus();
        //        }
        //    }
        //    else if (e.Control && e.KeyCode == Keys.R)      // Ctrl + R => Focus Receive Amt
        //    {
        //        cboCurrency.DroppedDown = false;
        //        txtReceiveAmount.Focus();
        //    }
        //    else if (e.Control && e.KeyCode == Keys.S)      // Ctrl + S => Click Save
        //    {
        //        cboCurrency.DroppedDown = false;
        //        btnSubmit.PerformClick();
        //    }
        //    else if (e.Control && e.KeyCode == Keys.C)      // Ctrl + C => Focus DropDown Customer
        //    {
        //        cboCurrency.DroppedDown = false;
        //        btnCancel.PerformClick();
        //    }
        //}
        #endregion

        private void PaidByCash2_Load(object sender, EventArgs e)
        {
            try
            {
                //Localization.Localize_FormControls(this);
                #region Setting Hot Kyes For the Controls
                //SendKeys.Send("%"); SendKeys.Send("%"); // Clicking "Alt" on page load to show underline of Hot Keys
                this.KeyPreview = true;
                //this.KeyDown += new KeyEventHandler(Form_KeyDown);
                #endregion

                #region currency

                //txtExchangeRate.Text = SettingController.DefaultExchangeRate.ToString();
                #endregion
                if (IsFoc)
                {
                    lblTotalCost.Text = "0";
                    txtReceiveAmount.Text = "0";
                    txtReceiveAmount.Enabled = false;
                }
                else
                {
                    lblTotalCost.Text = (DetailList.Sum(x => x.TotalAmount) - ExtraDiscount).ToString();
                    if (SettingController.TicketSale)
                    {
                        txtReceiveAmount.Text = lblTotalCost.Text;
                    }
                }

                // txtReceiveAmount.Focus();
                
                

            }
            catch (Exception ex)
            {
                string sExec = ex.Message;
                if (ex.InnerException != null)
                {
                    sExec = ex.InnerException.ToString();
                }
                al.WriteErrorLog(ex.ToString(), "PaidByCash2_Load", sExec);
            }
        }
        class TD4Ticket
        {
            public long? Tdid { get; set; }
            public int? Qty { get; set; }
            public decimal DiscountRate { get; set; }
            public string Category { get; set; }
        }
        List<TD4Ticket> tdlist = new List<TD4Ticket>();

        private void btnSubmit_Click(object sender, EventArgs e)
        {            
            try
            {
                POSEntities entity = new POSEntities();

                string shopname = SettingController.DefaultShop.ShopName;

                btnSubmit.Enabled = false;
                Application.UseWaitCursor = true;
                Cursor.Current = Cursors.WaitCursor;
                bool hasError = false;
                tp.RemoveAll();
                tp.IsBalloon = true;
                tp.ToolTipIcon = ToolTipIcon.Error;
                tp.ToolTipTitle = "Error";
                long receiveAmount = 0;
                long totalCost = (long)DetailList.Sum(x => x.TotalAmount) - ExtraDiscount;
                
                //total cost wint unit price
                long unitpriceTotalCost = (long)DetailList.Sum(x => x.UnitPrice * x.Qty);
                Int64.TryParse(txtReceiveAmount.Text, out receiveAmount);
                decimal totalCashSaleAmount = Convert.ToDecimal(lblTotalCost.Text);

                //if (cboCurrency.SelectedIndex == -1)
                //{
                //    tp.SetToolTip(cboCurrency, "Error");
                //    tp.Show("Please select currency!", cboCurrency);
                //    return;
                //}
                //string currVal = cboCurrency.Text;
                //int currencyId = (from c in entity.Currencies where c.CurrencyCode == currVal select c.Id).SingleOrDefault();

                ////Validation
                ///
                decimal TotalAmt = Convert.ToDecimal(lblTotalCost.Text);

                if (receiveAmount < TotalAmt)
                {
                    tp.SetToolTip(txtReceiveAmount, "Error");
                    tp.Show("Receive amount must be greater than total cost!", txtReceiveAmount);
                    hasError = true;
                }

                if (!hasError)
                {
                    System.Data.Objects.ObjectResult<String> Id;
                    Transaction insertedTransaction = new Transaction();
                    //decimal change = 0;

                    //change = Convert.ToDecimal(lblChanges.Text);
                    int paymentTypeId = 1;
                    if(IsFoc)
                    {
                        paymentTypeId = 4;
                        totalCost = 0;
                        receiveAmount = 0;
                        ExtraDiscount = 0;
                        Discount = 0;
                    }
                    Id = entity.InsertTransaction(DateTime.Now, MemberShip.UserId, MemberShip.CounterId, TransactionType.Sale, true, true, paymentTypeId, ExtraDiscount + Discount, totalCost, receiveAmount, SettingController.DefaultShop.Id, SettingController.DefaultShop.ShortCode, Note);
                    entity = new POSEntities();
                    string resultId = Id.FirstOrDefault().ToString();
                    insertedTransaction = (from trans in entity.Transactions where trans.Id == resultId select trans).FirstOrDefault<Transaction>();
                    insertedTransaction.IsDeleted = false;
                    
                    tdlist.Clear();
                    foreach (TransactionDetail detail in DetailList)
                    {
                        detail.IsDeleted = false;//Update IsDelete (Null to 0)

                        if (!IsFoc)
                        {
                            var detailID = entity.InsertTransactionDetail(insertedTransaction.Id, Convert.ToInt32(detail.ProductId), Convert.ToInt32(detail.Qty), Convert.ToInt32(detail.UnitPrice), Convert.ToDouble(detail.DiscountRate), Convert.ToInt32(detail.TotalAmount), detail.IsDeleted, detail.IsFOC, Convert.ToInt32(detail.SellingPrice)).SingleOrDefault();
                            TD4Ticket td4tk = new TD4Ticket();
                            td4tk.Tdid = (long?)detailID;
                            td4tk.Qty = detail.Qty;
                            td4tk.DiscountRate = detail.DiscountRate;
                            var product = Sales.productVar.Where(x => x.Id == (long)detail.ProductId).FirstOrDefault();
                            if (product != null)
                            {
                                td4tk.Category = product.Name;
                            }
                            else
                            {
                                td4tk.Category = "Ticket";
                            }
                            tdlist.Add(td4tk);

                            entity.SaveChanges();
                        }
                        else
                        {
                            var detailID = entity.InsertTransactionDetail(insertedTransaction.Id, Convert.ToInt32(detail.ProductId), Convert.ToInt32(detail.Qty), Convert.ToInt32(detail.UnitPrice), 0, 0, detail.IsDeleted, true, 0).SingleOrDefault();
                            TD4Ticket td4tk = new TD4Ticket();
                            td4tk.Tdid = (long?)detailID;
                            td4tk.Qty = detail.Qty;
                            td4tk.DiscountRate = detail.DiscountRate;
                            var product = Sales.productVar.Where(x => x.Id == (long)detail.ProductId).FirstOrDefault();
                            if (product != null)
                            {
                                td4tk.Category = product.Name;
                            }
                            else
                            {
                                td4tk.Category = "Ticket";
                            }
                            tdlist.Add(td4tk);

                            entity.SaveChanges();
                        }
                        
                    }

                    entity.SaveChanges();

                    
                    //generate ticket
                    //if (SettingController.TicketSale)
                    //{
                        long unitPrice = 0; string vOut = "";
                        string ticketTitle = "";
                        decimal discountRate = 0;
                        List<TD4Ticket> copyList = tdlist;
                        List<string> TicketList = new List<string>();
                        
                        foreach (var item in copyList)
                        {

                        //for (int i = 0; i < item.Qty; i++)
                        //{
                        //    string sTicket = entity.InsertTicket(item.Tdid, false, DateTime.Now.Date, item.Category, "CT" + MemberShip.CounterReferenceNo).FirstOrDefault();
                        //    if (sTicket != null)
                        //    {
                        //        TicketList.Add(sTicket);
                        //    }
                        //}
                        string sTicket = entity.InsertTicket(item.Tdid, false, DateTime.Now.Date, item.Category, "CT" + MemberShip.CounterReferenceNo, item.Qty).FirstOrDefault();
                        sTicket = sTicket.TrimEnd();
                        TicketList = sTicket.Split(' ').ToList();

                        foreach (string t in TicketList)
                            {

                                dsReportTemp dsReport = new dsReportTemp();
                                dsReportTemp.TicketReportDataTable dtReport = (dsReportTemp.TicketReportDataTable)dsReport.Tables["TicketReport"];
                                dsReportTemp.TicketReportRow newRow = dtReport.NewTicketReportRow();
                                newRow.DateTime = DateTime.Now.Date.ToString("dd/MMM/yyyy");
                                newRow.EventName = shopname;

                                QRCoder.QRCodeGenerator qrgenerator = new QRCoder.QRCodeGenerator();
                                QRCoder.QRCodeData qrdata = qrgenerator.CreateQrCode(t.Replace("CT", "").Remove(1,4), QRCoder.QRCodeGenerator.ECCLevel.Q);
                                QRCoder.QRCode qrcode = new QRCoder.QRCode(qrdata);
                                Bitmap qrImage = qrcode.GetGraphic(20);
                                ImageConverter converter = new ImageConverter();
                                newRow.QRCode = (byte[])converter.ConvertTo(qrImage, typeof(byte[]));
                                dtReport.AddTicketReportRow(newRow);
                                string reportPath = "";
                                ReportViewer rv = new ReportViewer();
                                ReportDataSource rds = new ReportDataSource("Ticket", dsReport.Tables["TicketReport"]);
                                reportPath = Application.StartupPath + "\\Reports\\Ticket_2023.rdlc";

                                rv.Reset();
                                rv.LocalReport.ReportPath = reportPath;
                                rv.LocalReport.DataSources.Add(rds);

                                rv.LocalReport.EnableExternalImages = true;

                                string mainlogoimagepath = System.Configuration.ConfigurationManager.AppSettings["ygnzoologo"];
                                ReportParameter mainlogoimagepar = new ReportParameter("MainLogoImagePath", mainlogoimagepath);
                                rv.LocalReport.SetParameters(mainlogoimagepar);

                              

                                rv.LocalReport.Refresh();
                                #region Report parameter passing data
                                ticketTitle = item.Category;
                                discountRate = item.DiscountRate;
                                decimal disRate = 0;
                            if (ticketTitle == "Local Adult")
                            {
                                ReportParameter Title = new ReportParameter("Title", "Local Adult");
                                rv.LocalReport.SetParameters(Title);
                                unitPrice = MDIParent.LocalAdultPrice;
                                disRate = discountRate;
                                //disRate = MDIParent.LocalAdultDis;
                            }
                            else if (ticketTitle == "Local Child")
                            {
                                ReportParameter Title = new ReportParameter("Title", "Local Child");
                                rv.LocalReport.SetParameters(Title);
                                unitPrice = MDIParent.LocalChildPrice;
                                //disRate = MDIParent.LocalChildDis;
                                disRate = discountRate;
                            }
                            else if (ticketTitle == "Foreign Adult")
                            {
                                ReportParameter Title = new ReportParameter("Title", "Foreign Adult");
                                rv.LocalReport.SetParameters(Title);
                                unitPrice = MDIParent.ForeignAdultPrice;
                                //disRate = MDIParent.ForeignAdultDis;
                                disRate = discountRate;
                            }
                            else if (ticketTitle == "Foreign Child")
                            {
                                ReportParameter Title = new ReportParameter("Title", "Foreign Child");
                                rv.LocalReport.SetParameters(Title);
                                unitPrice = MDIParent.ForeignChildPrice;
                                //disRate = MDIParent.ForeignChildDis;
                                disRate = discountRate;
                            }

                            else
                            {
                                ReportParameter Title = new ReportParameter("Title", ticketTitle);
                                rv.LocalReport.SetParameters(Title);
                                if (ticketTitle.Contains("Sangha"))
                                {
                                    unitPrice = MDIParent.SanghaPrice;
                                    //disRate = MDIParent.SanghaDis;
                                    disRate = discountRate;
                                }
                                else if(ticketTitle.Contains("Private Tour"))
                                {
                                    unitPrice = MDIParent.PrivateTourPrice;
                                    //disRate = MDIParent.PrivateTourDis;
                                    disRate = discountRate;
                                }
                            }

                                //ReportParameter Time = new ReportParameter("Time", DateTime.Now.ToString("hh:mm:ss tt"));
                                //rv.LocalReport.SetParameters(Time);
                                ReportParameter TranNo = new ReportParameter("TranNo", insertedTransaction.Id);
                                rv.LocalReport.SetParameters(TranNo);

                                //var varTDetail = insertedTransaction.TransactionDetails.Where(x=>x.Id==item.Tdid).FirstOrDefault();
                                //unitPrice = varTDetail.UnitPrice.Value;

                                decimal discountAmount = 0;

                                if (disRate > 0)
                                {//if discunt have on each ticket item
                                    discountAmount = unitPrice * disRate / 100;
                                }
                                //    if (varTDetail.Transaction.ReceivedCurrencyId == 1)
                                //{
                                //  ReportParameter CounterName = new ReportParameter("CounterName", counterName);
                                ReportParameter CounterName = new ReportParameter("CounterName", counterName);
                                rv.LocalReport.SetParameters(CounterName);

                                ReportParameter PrintDateTime = new ReportParameter("PrintDateTime", DateTime.Now.ToString("dd/MMM/yyyy HH:mm:ss"));
                                rv.LocalReport.SetParameters(PrintDateTime);

                                ReportParameter CasherName = new ReportParameter("CashierName", MemberShip.UserName);
                                rv.LocalReport.SetParameters(CasherName);

                                vOut = (unitPrice- discountAmount).ToString();
                                ReportParameter Price = new ReportParameter("Price", vOut);
                                rv.LocalReport.SetParameters(Price);
                                //}
                                //else
                                //{
                                //    int pp = Convert.ToInt32(unitPrice);
                                //    int p = pp /  Convert.ToInt32(cu.LatestExchangeRate);
                                //    ReportParameter Price = new ReportParameter("Price", p.ToString());
                                //    rv.LocalReport.SetParameters(Price);
                                //}


                                //if(varTDetail.Transaction.ReceivedCurrencyId == 2)
                                //{
                                //    ReportParameter Cucy = new ReportParameter("Cucy", "USD");
                                //    rv.LocalReport.SetParameters(Cucy);
                                //}
                                //else
                                //{
                                //ReportParameter Cucy = new ReportParameter("Cucy", "MMK");
                                //rv.LocalReport.SetParameters(Cucy);
                                //}

                                ReportParameter TicketNo = new ReportParameter("TicketNo", t);
                                rv.LocalReport.SetParameters(TicketNo);

                                //ReportParameter DiscountRaterparamer = new ReportParameter("DiscountRate", disRate.ToString());
                                //rv.LocalReport.SetParameters(DiscountRaterparamer);

                                //ReportParameter TotalAmountparamer = new ReportParameter("TotalAmount", (unitPrice - discountAmount).ToString());
                                //rv.LocalReport.SetParameters(TotalAmountparamer);


                                //ReportParameter TicketType = new ReportParameter("TicketType", item.Category);
                                //rv.LocalReport.SetParameters(TicketType);
                                #endregion

                                //finding the brand name to use as zoo name report


                                ReportParameter ZooName = new ReportParameter("ZooName", Sales.zooName);
                                rv.LocalReport.SetParameters(ZooName);

                                ReportParameter fbLink = new ReportParameter("fbLink", System.Configuration.ConfigurationManager.AppSettings["fbLink"]);
                                rv.LocalReport.SetParameters(fbLink);
                                Utility.Get_Print(rv);
                            }//foreach loop for ticket list.
                             //
                            TicketList.Clear();
                        }
                   // }

                    if (MessageShow() == DialogResult.OK)
                    {
                        if (System.Windows.Forms.Application.OpenForms["chart"] != null)
                        {
                            chart newForm = (chart)System.Windows.Forms.Application.OpenForms["chart"];
                            newForm.FormFresh();
                        }
                    }

                    if (System.Windows.Forms.Application.OpenForms["Sales"] != null)
                    {
                        Sales newForm = (Sales)System.Windows.Forms.Application.OpenForms["Sales"];
                        newForm.note = "";
                        newForm.Clear();
                    }
                }

                Note = "";
                this.Dispose();


            }
            catch (Exception ex)
            {
                string sExec = ex.Message;
                if (ex.InnerException != null)
                {
                    sExec = ex.InnerException.ToString();
                }
                al.WriteErrorLog(ex.ToString(), "btnSummit_Click", sExec);
                MessageBox.Show("An error occurred while printing the ticket, Please contact an administrator for assist.");
            }
            finally
            {
                Application.UseWaitCursor = false;
                Cursor.Current = Cursors.Default;
                btnSubmit.Enabled = true;
            }

        }

        static int countDigit(long n)   //add by HMT
        {
            int count = 0;
            while (n != 0)
            {
                n = n / 10;
                ++count;
            }
            return count;
        }

        private DialogResult MessageShow()
        {
            DialogResult result = MessageBox.Show(this, "Payment Completed", "mPOS", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return result;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void PaidByCash_MouseMove(object sender, MouseEventArgs e)
        {
            tp.Hide(txtReceiveAmount);
            //tp.Hide(cboCurrency);
        }

        private void txtReceiveAmount_KeyUp(object sender, KeyEventArgs e)
        {
            int amount = 0;
            Int32.TryParse(txtReceiveAmount.Text, out amount);
            int Cost = 0;
            Int32.TryParse(lblTotalCost.Text, out Cost);
            lblChanges.Text = (amount - (DetailList.Sum(x => x.TotalAmount) - ExtraDiscount)).ToString();


        }

        private void txtReceiveAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }



        private void txtReceiveAmount_KeyUp_1(object sender, KeyEventArgs e)
        {
            decimal amount = 0;
            decimal.TryParse(txtReceiveAmount.Text, out amount);
            decimal Cost = 0;
            decimal.TryParse(lblTotalCost.Text, out Cost);

            if (txtReceiveAmount.Text != string.Empty)
            {
                //lblChanges.Text = (amount - (DetailList.Sum(x => x.TotalAmount) - ExtraDiscount - MCDiscount - BDDiscount + ExtraTax)).ToString();
                lblChanges.Text = (amount - Cost).ToString();
            }
        }

      
    }
}

