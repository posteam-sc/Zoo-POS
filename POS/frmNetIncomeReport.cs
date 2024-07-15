using POS.APP_Data;
using System;
using System.Linq;
using System.Windows.Forms;

namespace POS
{
    public partial class frmNetIncomeReport : Form
    {
        #region Initialized
        public frmNetIncomeReport()
        {
            InitializeComponent();
        }
        #endregion

        #region variable
        POSEntities entity = new POSEntities();
    
        //int year = 0;
        //int month = 0;
        //string monthName = "";
        //Boolean IsStart = false;
        #endregion

        private void frmNetIncomeReport_Load(object sender, EventArgs e)
        {
            Localization.Localize_FormControls(this);
            var startYear = Convert.ToDateTime(SettingController.Company_StartDate).Year;
            cboYear.DataSource = Enumerable.Range(startYear, 100).ToList();
            cboYear.Text = DateTime.Now.Year.ToString();
            cboMonth.Text = DateTime.Now.ToString("MMMM");

            Utility.BindShop(cboshoplist, true);

            cboshoplist.Text = SettingController.DefaultShop.ShopName;
            Utility.ShopComBo_EnableOrNot(cboshoplist);
            cboshoplist.SelectedIndex = 0;
            //IsStart = true;
            LoadData();
        }


        private void LoadData()
        {
            //if (IsStart == true)
            //{
            //    try
            //    {
            //        int shopid = Convert.ToInt32(cboshoplist.SelectedValue);
            //        string currentshopcode = "";
            //        string currentshopname = "";
            //        if (shopid!=0)
            //        {
            //            currentshopcode = (from p in entity.Shops where p.Id == shopid select p.ShortCode).FirstOrDefault();
            //            currentshopname = (from p in entity.Shops where p.Id == shopid select p.ShopName).FirstOrDefault();
            //        }
            //        else
            //        {
            //            currentshopname = "ALL";
            //        }
            //        int SaleRevenue = 0, OpeningStockAmt = 0, ClosingStockAmt = 0, TotalParchaseAmt = 0, CostofGoodSold = 0;
            //        int NetProfitAndLoss = 0, GrossProfit = 0, TotalExpenseAmt = 0;
            //        year = Convert.ToInt32(cboYear.SelectedValue);
            //        monthName = cboMonth.Text;
            //        month = DateTime.ParseExact(monthName, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;

            //    #region Sale Revenue
            //    var SaleList = (from td in entity.TransactionDetails
            //                    join t in entity.Transactions on td.TransactionId equals t.Id
            //                    join p in entity.Products on td.ProductId equals p.Id
            //                    //join pc in entity.ProductCategories on p.ProductCategoryId equals pc.Id
            //                    where
            //                 (t.DateTime.Value.Year) == year &&
            //                (t.DateTime.Value.Month) == month
            //                 && t.IsDeleted == false && t.PaymentTypeId != 4 && t.PaymentTypeId != 6 && t.IsComplete == true
            //                 && t.IsActive == true && ((shopid!=0 && t.Id.Substring(2,2)==currentshopcode) || (shopid==0 && 1==1))
            //                    select new
            //                    {
            //                        TranId = td.TransactionId,
            //                        ParentId = t.ParentId,
            //                        Type = t.Type,
            //                        TotalAmt = td.TotalAmount

            //                    }
            //                    ).ToList();

            //    if (SaleList.Count > 0)
            //    {
            //        List<string> _type = new List<string> { "Sale","Credit"};
            //        var _tranIdList = SaleList.Where(x => _type.Contains(x.Type) ).Select(x => x.TranId).ToList();
            //        int _RefundTotalAmt = 0;
            //        var _RefundList = SaleList.Where(x => _tranIdList.Contains(x.ParentId) && x.Type == "Refund").ToList();

            //        _RefundTotalAmt = _RefundList.Sum(x => Convert.ToInt32(x.TotalAmt));


            //        var _salerevenueList = SaleList.Where(x => _type.Contains(x.Type)).Sum(x => x.TotalAmt);

            //        SaleRevenue = Convert.ToInt32(_salerevenueList) - Convert.ToInt32(_RefundTotalAmt);

            //    }
            //    #endregion

            //    #region Cost of Good Solds                  
            //    //added by HMT on Jan 2019
            //    var OpeningStock = (from p in entity.Products
            //                            join pd in entity.PurchaseDetails on p.Id equals pd.ProductId
            //                            join st in entity.StockTransactions on pd.ProductId equals st.ProductId
            //                            where (pd.Date.Value.Year) == year && (pd.Date.Value.Month) == (month-1) && pd.IsDeleted == false 
            //                            select new {TotalOpeningAmt = (pd.CurrentQy * pd.UnitPrice) }
            //                                ).ToList();

            //        var ClosingStock = (from p in entity.Products
            //                            join pd in entity.PurchaseDetails on p.Id equals pd.ProductId
            //                            join st in entity.StockTransactions on pd.ProductId equals st.ProductId
            //                            where (pd.Date.Value.Year) == year && (pd.Date.Value.Month) == month && pd.IsDeleted == false
            //                            select new { TotalClosingAmt = (pd.CurrentQy * pd.UnitPrice) }
            //                                ).ToList();

            //        var PurchaseAmt = (from p in entity.Products
            //                           join pd in entity.PurchaseDetails on p.Id equals pd.ProductId
            //                           where (pd.Date.Value.Year) == year && (pd.Date.Value.Month) == month && pd.IsDeleted == false
            //                           select new { TotalPurchaseAmt = (pd.Qty * pd.UnitPrice) }
            //                                ).ToList();

            //        if (OpeningStock.Count > 0 )
            //        {
            //            var _OpeningStockAmt = OpeningStock.Sum(x => x.TotalOpeningAmt);
            //            OpeningStockAmt = Convert.ToInt32(_OpeningStockAmt);
            //        }

            //        if (ClosingStock.Count>0)
            //        {
            //            var _ClosingStockAmt = ClosingStock.Sum(x => x.TotalClosingAmt);
            //            ClosingStockAmt = Convert.ToInt32(_ClosingStockAmt);
            //        }

            //        if (PurchaseAmt.Count > 0)
            //        {
            //            var _PurchaseAmt = PurchaseAmt.Sum(x => x.TotalPurchaseAmt);
            //            TotalParchaseAmt = Convert.ToInt32(_PurchaseAmt);
            //        }

            //        CostofGoodSold = ((OpeningStockAmt + TotalParchaseAmt) - ClosingStockAmt);                   

            //        //old
            //        //var GoodSoldExpList = (from pd in entity.PurchaseDetails
            //        //                       join pdt in entity.PurchaseDetailInTransactions on pd.Id equals pdt.PurchaseDetailId
            //        //                       join td in entity.TransactionDetails on pdt.TransactionDetailId equals td.Id
            //        //                       join t in entity.Transactions on td.TransactionId equals t.Id
            //        //                       join p in entity.Products on pd.ProductId equals p.Id
            //        //                       //join pc in entity.ProductCategories on p.ProductCategoryId equals pc.Id
            //        //                       where (t.DateTime.Value.Year) == year &&
            //        //                      (t.DateTime.Value.Month) == month
            //        //                            && t.IsDeleted == false && t.IsComplete == true
            //        //                               && t.IsActive == true && pd.IsDeleted == false && td.IsDeleted == false
            //        //                               && ((shopid != 0 && t.Id.Substring(2, 2) == currentshopcode) || (shopid == 0 && 1 == 1))
            //        //                       select new
            //        //                       {
            //        //                           TotalAmt = (pdt.Qty * pd.UnitPrice),

            //        //                       }
            //        //              ).ToList();

            //        //if (GoodSoldExpList.Count > 0)
            //        //{
            //        //    var _goodSoldTotalAmt = GoodSoldExpList.Sum(x => x.TotalAmt);
            //        //    CostofGoodSlod = Convert.ToInt32(_goodSoldTotalAmt);
            //        //}
            //    #endregion

            //    #region Gross Profit
            //        //added by HMT on Jan 2019
            //        GrossProfit = SaleRevenue - CostofGoodSold;
            //    #endregion

            //    #region Expense
            //        var ExpenseList = (from e in entity.Expenses
            //                           join ec in entity.ExpenseCategories on e.ExpenseCategoryId equals ec.Id
            //                           where e.IsApproved == true && e.IsDeleted == false
            //                   && (e.ExpenseDate.Value.Year) == year &&
            //                         (e.ExpenseDate.Value.Month) == month && ((shopid != 0 && e.Id.Substring(2, 2) == currentshopcode) || (shopid == 0 && 1 == 1))
            //                           select new
            //                           {
            //                               TotalAmount = e.TotalExpenseAmount,
            //                               ExpCag = ec.Name
            //                           }
            //                    ).ToList().GroupBy(x => x.ExpCag).ToList()
            //                    .Select(x => new
            //                    {
            //                        ExpCag = x.First().ExpCag,
            //                        TotalAmount = x.Sum(xl => xl.TotalAmount)
            //                    }).ToList();

            //        if (ExpenseList.Count > 0)
            //        {
            //            var _ExpenseList = ExpenseList.Sum(x => x.TotalAmount);
            //            TotalExpenseAmt = Convert.ToInt32(_ExpenseList);
            //        }

            //  #endregion

            //    #region Net Profit and Loss
            //        //added by HMT on Jan 2019
            //        NetProfitAndLoss = GrossProfit - TotalExpenseAmt;
            //    #endregion

            //    #region "Salary,Utilities,Rent and General  Expenses "
            //        //var ExpenseList = (from e in entity.Expenses
            //    //                   join ec in entity.ExpenseCategories on e.ExpenseCategoryId equals ec.Id

            //    //                   where e.IsApproved == true && e.IsDeleted == false 
            //    //           && (e.ExpenseDate.Value.Year) == year &&
            //    //                 (e.ExpenseDate.Value.Month) == month && ((shopid != 0 && e.Id.Substring(2, 2) == currentshopcode) || (shopid == 0 && 1 == 1))
            //    //                   select new
            //    //                   {
            //    //                       TotalAmount =  e.TotalExpenseAmount,
            //    //                       ExpCag = ec.Name
            //    //                   }
            //    //                   ).ToList().GroupBy(x=>x.ExpCag).ToList()
            //    //                   .Select(x=>new 
            //    //                   { 
            //    //                       ExpCag=x.First().ExpCag,
            //    //                       TotalAmount= x.Sum(xl=>xl.TotalAmount)
            //    //                    }).ToList();

            //    //if (ExpenseList.Count > 0)
            //    //{
            //    //    //var _applianceFees = ExpenseList.Where(x => x.ExpCag == "Appliance").Sum(x => x.TotalAmount);
            //    //    //ApplianceFees = Convert.ToInt32(_applianceFees);
            //    //    //List<string> cagType = new List<string> { "Salary", "Utilities", "Rent" };
            //    //    //var _salaryExpenseAmt = ExpenseList.Where(x => x.ExpCag == "Salary").Sum(x => x.TotalAmount);
            //    //    //SalaryExpense = Convert.ToInt32(_salaryExpenseAmt);

            //    //    //var _utilitiesExpenseAmt = ExpenseList.Where(x => x.ExpCag == "Utilities").Sum(x => x.TotalAmount);
            //    //    //UtilitiesExpense = Convert.ToInt32(_utilitiesExpenseAmt);

            //    //    //var _rentExpenseAmt = ExpenseList.Where(x => x.ExpCag == "Rent").Sum(x => x.TotalAmount);
            //    //    //RentExpense = Convert.ToInt32(_rentExpenseAmt);

            //    //    //var _generalExpenseAmt = ExpenseList.Where(x => !cagType.Contains(x.ExpCag)).Sum(x => x.TotalAmount);
            //    //    //GeneralExpense = Convert.ToInt32(_generalExpenseAmt);
            //    //}
            //    #endregion

            //    #region Assign Data to For DataSet
            //    dsReportTemp dsReport = new dsReportTemp();  
            //    dsReportTemp.NetIncomeDataTable dtPdReport = (dsReportTemp.NetIncomeDataTable)dsReport.Tables["NetIncome"];
            //    foreach (var item in ExpenseList)
            //    {
            //        dsReportTemp.NetIncomeRow newRow = dtPdReport.NewNetIncomeRow();
            //        newRow.ExpenseCategory = item.ExpCag;
            //        newRow.Cost = (long)item.TotalAmount;
            //        dtPdReport.AddNetIncomeRow(newRow);

            //    }
                         
            //    #endregion

            //    #region Show Report Viewer Part
            //    //added by HMT on Jan 2019
            //    ReportDataSource rds = new ReportDataSource("stest", dsReport.Tables["NetIncome"]);
            //    string reportPath = Application.StartupPath + "\\Reports\\NetIncomeReportUpdate.rdlc";
            //    reportViewer1.LocalReport.ReportPath = reportPath;
            //    reportViewer1.LocalReport.DataSources.Clear();
            //    this.reportViewer1.ZoomMode = ZoomMode.Percent;
            //    reportViewer1.LocalReport.DataSources.Add(rds);

            //    ReportParameter Month = new ReportParameter("MonthName", cboMonth.Text.ToString());
            //    reportViewer1.LocalReport.SetParameters(Month);

            //    ReportParameter ShopName = new ReportParameter("ShopName", currentshopname);
            //    reportViewer1.LocalReport.SetParameters(ShopName);

            //    ReportParameter TSaleRevenue = new ReportParameter("SaleRevenue", SaleRevenue.ToString());
            //    reportViewer1.LocalReport.SetParameters(TSaleRevenue);

            //    ReportParameter GoodSold = new ReportParameter("CostOfGoodSold", CostofGoodSold.ToString());
            //    reportViewer1.LocalReport.SetParameters(GoodSold);
            //    reportViewer1.RefreshReport();

            //    ReportParameter GP = new ReportParameter("GrossProfit", GrossProfit.ToString());
            //    reportViewer1.LocalReport.SetParameters(GP);
            //    reportViewer1.RefreshReport();

            //    ReportParameter NetPL = new ReportParameter("NetProfitAndLoss", NetProfitAndLoss.ToString());
            //    reportViewer1.LocalReport.SetParameters(NetPL);
            //    reportViewer1.RefreshReport();

            //    //old
            //    //ReportDataSource rds = new ReportDataSource("NetIncomeDataSet", dsReport.Tables["NetIncome"]);
            //    //string reportPath = Application.StartupPath + "\\Reports\\NetIncomeReport.rdlc";

            //    //reportViewer1.LocalReport.ReportPath = reportPath;
            //    //reportViewer1.LocalReport.DataSources.Clear();
            //    //this.reportViewer1.ZoomMode = ZoomMode.Percent;
            //    //reportViewer1.LocalReport.DataSources.Add(rds);

            //    //ReportParameter Month = new ReportParameter("MonthName", cboMonth.Text.ToString());
            //    //reportViewer1.LocalReport.SetParameters(Month);

            //    //ReportParameter ShortName = new ReportParameter("ShopName", currentshopname);
            //    //reportViewer1.LocalReport.SetParameters(ShortName);
                
            //    //ReportParameter TSaleRevenue = new ReportParameter("SalesRevenue", SaleRevenue.ToString());
            //    //reportViewer1.LocalReport.SetParameters(TSaleRevenue);

                
            //    //ReportParameter GoodSold = new ReportParameter("CostOfGoodSold", CostofGoodSold.ToString());
            //    //reportViewer1.LocalReport.SetParameters(GoodSold);
            //    //reportViewer1.RefreshReport();
                
            //    //ReportParameter GoodSold = new ReportParameter("CostOfGoodSold", CostofGoodSlod.ToString());
            //    //reportViewer1.LocalReport.SetParameters(GoodSold);
            //    //reportViewer1.RefreshReport();

            //    #endregion

            //    }
            //    catch
            //    {
            //    }
           // }
         
        }

        private void cboYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void cboMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void cboshoplist_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
