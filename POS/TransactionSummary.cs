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
    public partial class TransactionSummary : Form
    {
        #region Variable

        POSEntities entity = new POSEntities();
        List<Transaction> transList = new List<Transaction>();
        //List<Transaction> RtransList = new List<Transaction>();
        //List<Transaction> DtransList = new List<Transaction>();
        //List<Transaction> MPUtransList = new List<Transaction>();
        List<TransactionDetail> FOCtrnsList = new List<TransactionDetail>();
        List<TransactionDetail> OtherFOCtrnsList = new List<TransactionDetail>();
        string DateFormat;
        Boolean isstart = false;
        #endregion

        #region Event
        public TransactionSummary()
        {
            InitializeComponent();
        }

        private void TransactionSummary_Load(object sender, EventArgs e)
        {
            Localization.Localize_FormControls(this);
            Utility.BindShop(cboshoplist,true);
            cboshoplist.Text = SettingController.DefaultShop.ShopName;
            Utility.ShopComBo_EnableOrNot(cboshoplist);
            isstart = true;
           // cboshoplist.SelectedIndex = 0;
            DateFormat = SettingController.GlobalDateFormat;
            SettingController.SetGlobalDateFormat(dtpFrom);
            SettingController.SetGlobalDateFormat(dtpTo);
            // this.reportViewer1.RefreshReport();
            //LoadData();
        }   

        private void btnPrint_Click(object sender, EventArgs e)
        {
            #region [print]
            long totalSale = 0, totalSummary = 0; long totalCashInHand = 0, totalIncomeAmount = 0, totalFOC = 0, totalReceived = 0; long totalDiscount = 0;

            totalSale = transList.Sum(x => x.TotalAmount).Value;

            foreach (Transaction t in transList)
            {
                long itemdiscount = (long)t.TransactionDetails.Sum(x => (x.UnitPrice * (x.DiscountRate / 100)) * x.Qty);
                totalDiscount += (long)t.DiscountAmount - itemdiscount;

              
            
            }
            //totalRefund = RtransList.Sum(x => x.TotalAmount).Value;
            //totalRefundDiscount = RtransList.Sum(x => x.DiscountAmount).Value;
            //totalDebt = DtransList.Sum(x => x.TotalAmount).Value;
            //totalCreditRefund = CRtransList.Sum(x => x.TotalAmount).Value;
            //totalCreditRefundDiscount = CRtransList.Sum(x => x.DiscountAmount).Value;
            //totalGiftCard = GCtransList.Sum(x => x.TotalAmount).Value;
            //totalCredit = CtransList.Sum(x => x.TotalAmount).Value;
            //totalCreditRecieve = CtransList.Sum(x => x.RecieveAmount).Value;
            //totalMPU = MPUtransList.Sum(x => x.TotalAmount).Value;
            totalFOC = FOCtrnsList.Sum(x => x.TotalAmount).Value;
            //totalTester = TesterCtrnsList.Sum(x => x.TotalAmount).Value;

            //totalSummary = (totalSale + totalDebt + totalCreditRefund + totalGiftCard) - totalRefund;
            //totalSummary = ((totalSale + totalCredit + totalGiftCard + totalMPU) - (totalRefund + totalCreditRefund + totalFOC));
            //totalCashInHand = (totalSale + totalDebt + totalCreditRecieve) - totalRefund;
            //totalExpense = (totalRefund + totalCreditRefund + totalFOC);
            //totalIncomeAmount = (totalSale + totalCredit + totalGiftCard + totalMPU);
            //totalReceived = (totalSale + totalDebt + totalCreditRecieve);
            string reportPath = Application.StartupPath + "\\Reports\\TransactionSummary.rdlc";
            reportViewer1.LocalReport.ReportPath = reportPath;
            reportViewer1.LocalReport.DataSources.Clear();

            ReportParameter TotalDiscount = new ReportParameter("TotalDiscount", totalDiscount.ToString());
            reportViewer1.LocalReport.SetParameters(TotalDiscount);

            //ReportParameter TotalMCDiscount = new ReportParameter("TotalMCDiscount", totalDiscount.ToString());
            //reportViewer1.LocalReport.SetParameters(TotalMCDiscount);

            //ReportParameter TotalRefundDiscount = new ReportParameter("TotalRefundDiscount", totalRefundDiscount.ToString());
            //reportViewer1.LocalReport.SetParameters(TotalRefundDiscount);

            //ReportParameter TotalCreditRefundDiscount = new ReportParameter("TotalCreditRefundDiscount", totalCreditRefundDiscount.ToString());
            //reportViewer1.LocalReport.SetParameters(TotalCreditRefundDiscount);

            ReportParameter ActualAmount = new ReportParameter("ActualAmount", totalReceived.ToString());
            reportViewer1.LocalReport.SetParameters(ActualAmount);

            ReportParameter TotalFOC = new ReportParameter("TotalFOC", totalFOC.ToString());
            reportViewer1.LocalReport.SetParameters(TotalFOC);

            //ReportParameter TotalTester = new ReportParameter("TotalTester", totalTester.ToString());
            //reportViewer1.LocalReport.SetParameters(TotalTester);

            //ReportParameter TotalMPU = new ReportParameter("TotalMPU", totalMPU.ToString());
            //reportViewer1.LocalReport.SetParameters(TotalMPU);

            ReportParameter TotalSale = new ReportParameter("TotalSale", totalSale.ToString());
            reportViewer1.LocalReport.SetParameters(TotalSale);

            //ReportParameter CreditRecieve = new ReportParameter("CreditRecieve", totalCreditRecieve.ToString());
            //reportViewer1.LocalReport.SetParameters(CreditRecieve);

            //ReportParameter Expense = new ReportParameter("Expense", totalExpense.ToString());
            //reportViewer1.LocalReport.SetParameters(Expense);

            ReportParameter IncomeAmount = new ReportParameter("IncomeAmount", totalIncomeAmount.ToString());
            reportViewer1.LocalReport.SetParameters(IncomeAmount);

            ReportParameter CashInHand = new ReportParameter("CashInHand", totalCashInHand.ToString());
            reportViewer1.LocalReport.SetParameters(CashInHand);

            //ReportParameter TotalDebt = new ReportParameter("TotalDebt", totalDebt.ToString());
            //reportViewer1.LocalReport.SetParameters(TotalDebt);

            //ReportParameter TotalRefund = new ReportParameter("TotalRefund", totalRefund.ToString());
            //reportViewer1.LocalReport.SetParameters(TotalRefund);

            ReportParameter TotalSummary = new ReportParameter("TotalSummary", totalSummary.ToString());
            reportViewer1.LocalReport.SetParameters(TotalSummary);

            //ReportParameter TotalCreditRefund = new ReportParameter("TotalCreditRefund", totalCreditRefund.ToString());
            //reportViewer1.LocalReport.SetParameters(TotalCreditRefund);

            //ReportParameter TotalGiftCard = new ReportParameter("TotalGiftCard", totalGiftCard.ToString());
            //reportViewer1.LocalReport.SetParameters(TotalGiftCard);

            //ReportParameter TotalCredit = new ReportParameter("TotalCredit", totalCredit.ToString());
            //reportViewer1.LocalReport.SetParameters(TotalCredit);

            ReportParameter HeaderTitle = new ReportParameter("HeaderTitle", "Transaction Summary for " + SettingController.ShopName);
            reportViewer1.LocalReport.SetParameters(HeaderTitle);

            ReportParameter Date = new ReportParameter("Date", " From " + dtpFrom.Value.Date.ToString(DateFormat) + " To " + dtpTo.Value.Date.ToString(DateFormat));
            reportViewer1.LocalReport.SetParameters(Date);

            PrintDoc.PrintReport(reportViewer1);
            #endregion
        }

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
            //LoadData();
        }

        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {
            //LoadData();
        }
        private void cboshoplist_selectedIndexChanged(object sender, EventArgs e)
        {
            //LoadData();
        }
        #endregion

        #region Function
        private void LoadData()
        {
            try
            {
                Utility.WaitCursor();
                if (isstart == true)
                {
                    int shopid = Convert.ToInt32(cboshoplist.SelectedValue);
                    string shopname = "";
                    string currentshortcode = "";
                    if (shopid != 0)
                    {
                       var currentshop = (from p in entity.Shops where p.Id == shopid select p).FirstOrDefault();
                        if(currentshop != null)
                        {
                            shopname=currentshop.ShopName;
                            currentshortcode = currentshop.ShortCode;
                        }
                    }
                    else
                    {
                        currentshortcode = "0";
                        shopname = "ALL";
                    }
                    DateTime fromDate = dtpFrom.Value.Date;
                    DateTime toDate = dtpTo.Value.Date;

                    FOCtrnsList = (from t in entity.Transactions
                                   join td in entity.TransactionDetails on t.Id equals td.TransactionId
                                   where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true
                                       && t.Type == TransactionType.Sale && t.PaymentTypeId == 4 && (t.IsDeleted == null || t.IsDeleted == false) && ((shopid != 0 && t.Id.Substring(2, 2) == currentshortcode) || (shopid == 0 && 1 == 1))
                                   select td).ToList<TransactionDetail>();
                   ShowReportViewer1(shopname, currentshortcode);
                    lblPeriod.Text = fromDate.ToString(DateFormat) + " To " + toDate.ToString(DateFormat);
                    // lblNumberofTransaction.Text = transList.Count.ToString();
                    gbTransactionList.Text = "Transaction Summary Report";
                    //lblTotalAmount.Text = "";
                }
            }
            finally
            {
                Utility.LeaveCursor();
            }
        }

        private void ShowReportViewer1(string shopname,string currentshortcode)
        {
            long totalSale = 0;  long totalFOC = 0,totalDiscount = 0;
           
            DateTime fromDate = dtpFrom.Value.Date;
            DateTime toDate = dtpTo.Value.Date;
            transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.PaymentTypeId == 1 && t.IsComplete == true && t.IsActive == true && t.Type == TransactionType.Sale && (t.IsDeleted == null || t.IsDeleted == false) && ((currentshortcode != "0" && t.Id.Substring(2, 2) == currentshortcode) || (currentshortcode == "0" && 1 == 1)) select t).ToList<Transaction>();
            //foreach (Transaction t in discounttransList)
            //{
            //    long itemdiscount = (long)t.TransactionDetails.Sum(x => (x.UnitPrice * (x.DiscountRate / 100)) * x.Qty);
            //    totalDiscount += (long)t.DiscountAmount - itemdiscount;

            //}
            totalDiscount = transList.Sum(x => x.DiscountAmount).Value;
            totalSale = transList.Sum(x => x.RecieveAmount).Value;

            totalFOC = Convert.ToInt32(FOCtrnsList.Sum((x => x.SellingPrice * x.Qty)));
            string reportPath = Application.StartupPath + "\\Reports\\TransactionSummary.rdlc";
            reportViewer1.LocalReport.ReportPath = reportPath;
            reportViewer1.LocalReport.DataSources.Clear();

            ReportParameter TotalDiscount = new ReportParameter("TotalDiscount", totalDiscount.ToString());
            reportViewer1.LocalReport.SetParameters(TotalDiscount);

            ReportParameter TotalFOC = new ReportParameter("TotalFOC", totalFOC.ToString());
            reportViewer1.LocalReport.SetParameters(TotalFOC);

            ReportParameter TotalSale = new ReportParameter("TotalSale", totalSale.ToString());
            reportViewer1.LocalReport.SetParameters(TotalSale);

            ReportParameter CashInHand = new ReportParameter("CashInHand", totalSale.ToString());
            reportViewer1.LocalReport.SetParameters(CashInHand);

            ReportParameter HeaderTitle = new ReportParameter("HeaderTitle", "Transaction Summary for " + shopname  );
            reportViewer1.LocalReport.SetParameters(HeaderTitle);

            ReportParameter Date = new ReportParameter("Date", " From " + dtpFrom.Value.Date.ToString(DateFormat) + " To " + dtpTo.Value.Date.ToString(DateFormat));
            reportViewer1.LocalReport.SetParameters(Date);

            reportViewer1.RefreshReport();
        }
        
        #endregion

        private void btnSearch_Click(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
