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
    public partial class ViewTicket : Form
    {
        #region Variable
        private POSEntities entity = new POSEntities();
        public string transactionId;
        #endregion
        public ViewTicket()
        {
            InitializeComponent();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            RoleManagementController controller = new RoleManagementController();
            controller.Load(MemberShip.UserRoleId);
            if (controller.Ticket.Reprint || MemberShip.isAdmin)
            {
                
            }
            else
            {
                MessageBox.Show("You are not allowed to reprint the tickets!", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;

            }
            foreach (DataGridViewRow row in dgvViewTicket.Rows) {
                if (Convert.ToBoolean(row.Cells[4].Value)){                 
                    long tId = 0;
                    tId = Convert.ToInt64(row.Cells[5].Value.ToString());  // chk in dgv                   
                    var curTicket = entity.Tickets.Where(t => t.Id == tId).FirstOrDefault();
                    //int receivedCurrencyId = 0;
                    //var receivedCurrencydata = (from t in entity.Tickets
                    //                          join td in entity.TransactionDetails on t.TransactionDetailId equals td.Id
                    //                          join tt in entity.Transactions on td.TransactionId equals tt.Id
                    //                          where t.Id==tId
                    //                          select new { receivedCurrenctyId=tt.ReceivedCurrencyId });
                    //foreach(var item in receivedCurrencydata) {
                    //    receivedCurrencyId =(int) item.receivedCurrenctyId;
                    //    }
                
                    //Currency currencyEntity = new Currency();
                    //currencyEntity = entity.Currencies.Where(x => x.Id == receivedCurrencyId).SingleOrDefault();
                    if (curTicket.Status == false || curTicket.Status == true) 
                    {
                        // && DateTime.Now.Date == curTicket.CreatedDate.Date
                        string shopname = SettingController.DefaultShop.ShopName;
                       
                        dsReportTemp dsReport = new dsReportTemp();
                        dsReportTemp.TicketReportDataTable dtReport = (dsReportTemp.TicketReportDataTable)dsReport.Tables["TicketReport"];
                        dsReportTemp.TicketReportRow newRow = dtReport.NewTicketReportRow();
                        newRow.DateTime = curTicket.CreatedDate.Date.ToString(SettingController.GlobalDateFormat);
                        newRow.EventName = shopname;

                        QRCoder.QRCodeGenerator qrgenerator = new QRCoder.QRCodeGenerator();
                        QRCoder.QRCodeData qrdata = qrgenerator.CreateQrCode(curTicket.TicketNo.Replace("CT", "").Remove(1, 4), QRCoder.QRCodeGenerator.ECCLevel.Q);
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
                        string ticketTitle = "";
                        long unitPrice;
                        ticketTitle = entity.Tickets.Where(x=> x.TicketNo == curTicket.TicketNo).FirstOrDefault().Category.ToString();
                        if (ticketTitle == "Foreign Adult") {
                            ReportParameter Title = new ReportParameter("Title", "Foreign Adult (R-P)");
                            rv.LocalReport.SetParameters(Title);
                            }
                        else if (ticketTitle == "Foreign Child") {
                            ReportParameter Title = new ReportParameter("Title", "Foreign Child (R-P)");
                            rv.LocalReport.SetParameters(Title);
                            }
                        else if (ticketTitle == "Local Adult") {
                            ReportParameter Title = new ReportParameter("Title", "Local Adult (R-P)");
                            rv.LocalReport.SetParameters(Title);
                            }
                        else if (ticketTitle == "Local Child") {
                            ReportParameter Title = new ReportParameter("Title", "Local Child (R-P)");
                            rv.LocalReport.SetParameters(Title);
                            }

                        else {
                            ReportParameter Title = new ReportParameter("Title", ticketTitle[0].ToString() + ticketTitle[ticketTitle.Length - 1].ToString().ToUpper());
                            rv.LocalReport.SetParameters(Title);
                            }

                        //ReportParameter Time = new ReportParameter("Time", DateTime.Now.ToString("hh:mm:ss tt"));
                        //rv.LocalReport.SetParameters(Time);

                        ReportParameter TranNo = new ReportParameter("TranNo", entity.Tickets.Where(a => a.TicketNo == curTicket.TicketNo).FirstOrDefault().TransactionDetail.Transaction.Id);
                        rv.LocalReport.SetParameters(TranNo);

                        var curTran = curTicket.TransactionDetail.Transaction;
                        unitPrice = curTicket.TransactionDetail.UnitPrice.Value;
                        var discountrate = curTicket.TransactionDetail.DiscountRate;
                        decimal discountAmount = 0;
                        if (discountrate > 0) {//if discunt have on each ticket item
                            discountAmount = unitPrice * discountrate / 100;
                            }
                        ReportParameter Price = new ReportParameter("Price", unitPrice.ToString());
                        rv.LocalReport.SetParameters(Price);



                        //ReportParameter Cucy = new ReportParameter("Cucy", "MMK");
                        //rv.LocalReport.SetParameters(Cucy);

                        ReportParameter CounterName = new ReportParameter("CounterName", currentTransaction.Counter.Name);
                        rv.LocalReport.SetParameters(CounterName);

                        ReportParameter PrintDateTime = new ReportParameter("PrintDateTime", curTicket.CreatedDate.ToString(SettingController.GlobalDateFormat+" HH:mm:ss"));
                        rv.LocalReport.SetParameters(PrintDateTime);

                        ReportParameter CasherName = new ReportParameter("CashierName", currentTransaction.User.Name);
                        rv.LocalReport.SetParameters(CasherName);

                        ReportParameter TicketNo = new ReportParameter("TicketNo", entity.Tickets.Where(a => a.TicketNo == curTicket.TicketNo).FirstOrDefault().TicketNo.ToString());
                        rv.LocalReport.SetParameters(TicketNo);

                        //ReportParameter DiscountRaterparamer = new ReportParameter("DiscountRate", discountrate.ToString());
                        //rv.LocalReport.SetParameters(DiscountRaterparamer);

                        //ReportParameter TotalAmountparamer = new ReportParameter("TotalAmount", (unitPrice - discountAmount).ToString());
                        //rv.LocalReport.SetParameters(TotalAmountparamer);


                        //ReportParameter TicketType = new ReportParameter("TicketType", entity.Tickets.Where(a => a.TicketNo == curTicket.TicketNo).FirstOrDefault().Category.ToString());
                        //rv.LocalReport.SetParameters(TicketType);
                        #endregion

                        //finding the brand name to use as zoo name report
                        string zooName = entity.Brands.Where(x => x.IsDelete == false).SingleOrDefault().Name;

                        ReportParameter ZooName = new ReportParameter("ZooName", zooName);
                        rv.LocalReport.SetParameters(ZooName);

                        ReportParameter fbLink = new ReportParameter("fbLink", System.Configuration.ConfigurationManager.AppSettings["fbLink"]);
                        rv.LocalReport.SetParameters(fbLink);

                        Utility.Get_Print(rv);

                        APP_Data.Ticket Edt = entity.Tickets.Where(x => x.Id == tId).FirstOrDefault();
                        Edt.RePrint = Edt.RePrint == null ? 1 : ++Edt.RePrint;

                        entity.Entry(Edt).State = EntityState.Modified;
                        entity.SaveChanges();
                    }
                    else
                    {
                        MessageBox.Show("You are not allowed to reprint because it is expired!", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        
                    }                                 
                }
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


        private void ViewTicket_Load(object sender, EventArgs e)
        {
            Localization.Localize_FormControls(this);
           LoadData();
        }
        Transaction currentTransaction;
        private void LoadData()
        {
            dgvViewTicket.AutoGenerateColumns = false;
            currentTransaction = entity.Transactions.Find(transactionId);
            var list = currentTransaction.TransactionDetails.SelectMany(a => a.Tickets).Where(b => b.isDelete == false || b.isDelete==null).ToList();
            dgvViewTicket.DataSource = list;

        }

        private void dgvViewTicket_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow row in dgvViewTicket.Rows)
            {
                Ticket curti = (Ticket)row.DataBoundItem;
                row.Cells[0].Value = curti.TicketNo;
                row.Cells[1].Value = curti.Status;
                row.Cells[2].Value = curti.CreatedDate;
                row.Cells[3].Value = curti.Category;
                row.Cells[5].Value = curti.Id;
            }
        }
    }
}
