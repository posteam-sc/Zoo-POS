using POS.APP_Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Objects;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;


namespace POS
{
    public partial class chart : Form
    {
        POSEntities entity = new POSEntities();
        
        List<dailysaleclass> dailychart = new List<dailysaleclass>();

        List<TopProductHolder> itemtopList = new List<TopProductHolder>();

        List<SaleBreakDownController> FinalResultListCat = new List<SaleBreakDownController>();
        
        public chart()
        {
            InitializeComponent();
        }

        private void chart_Load(object sender, EventArgs e)
        {
            Localization.Localize_FormControls(this);
            LoadDataDaily();
            toptenproduct();
            salebreakdowncategory();
            NetIncome();
           
            topproduct.ChartAreas[0].AxisX.LabelStyle.Font = new Font("Zawgyi-One", 9, FontStyle.Regular);
            foreach (dailysaleclass d in dailychart.Where(d => d.Payment_Type != "Refund").ToList())
            {
                if (d.Payment_Type != null)
                {
                    dailysalechart1.Series["DailySale"].Points.AddXY(d.Payment_Type, d.Total);
                }
            }
            decimal dtotal = 0;
            dailychart.Where(d => d.Payment_Type != "FOC").ToList().ForEach(d => dtotal += d.Total);
            ZeroSeperate(lbltotal, (int)dtotal);
            if (itemtopList.Count != 0)
            {
                foreach (TopProductHolder d in itemtopList)
                {

                    topproduct.Series["Daily_top10product"].Points.AddXY(d.Name, d.Qty);

                }
            }
            else
            {
                topproduct.Series["Daily_top10product"].Points.AddXY("Product", 0);
            }


            foreach (SaleBreakDownController s in FinalResultListCat.OrderByDescending(x => x.saleQty).ToList())
            {
                if (s.BreakDown != 0)
                {
                    salebreakchart.Series["Sale_BreakDown_By_Category"].Points.AddXY(s.BreakDown + "%", s.BreakDown);

                }
            }
            foreach (SaleBreakDownController s in FinalResultListCat.OrderByDescending(x => x.saleQty).ToList())
            {
                if (s.BreakDown != 0)
                {
                    salebreakchart.Series["Sale_BreakDown_By_Category"].Sort(PointSortOrder.Descending, "Y");
                    int i = 0;
                    foreach (DataPoint dp in salebreakchart.Series["Sale_BreakDown_By_Category"].Points)
                    {

                        if (dp.YValues[i].ToString() == s.BreakDown.ToString())
                        {
                            dp.LegendText = s.Name;
                        }
                    }
                    i++;
                }

            }




        }
        public void FormFresh()
        {
            LoadDataDaily();
            toptenproduct();
            salebreakdowncategory();
            //Recieveable();
            NetIncome();
            //MinQty();
            //Payable();
        }
        void ZeroSeperate(Label lab, Int32 value)
        {
            CultureInfo ci = new CultureInfo("en-us");
            lab.Text = lab.Name == "lbltotal" ? "Total Sale : " + value.ToString("N01", ci) + " Ks" : value.ToString("N01", ci) + " Ks";
            lab.Refresh();
        }
        
        public void NetIncome()
        {
            int shopid = SettingController.DefaultShop.Id;
                string currentshopcode = (from p in entity.Shops where p.Id == shopid select p.ShortCode).FirstOrDefault();
                 int SaleRevenue = 0;
                int year = DateTime.Now.Year;
                int month = DateTime.Now.Date.Month;
            #region Sale Revenue
                var SaleList = (from t in entity.Transactions 
                                where
                             (t.DateTime.Value.Year) == year &&
                            (t.DateTime.Value.Month) == month
                             && t.IsDeleted == false && t.PaymentTypeId != 4 && t.IsComplete == true
                             && t.IsActive == true && t.Id.Substring(2, 2) == currentshopcode && t.Type=="Sale"
                                select new
                                {
                                    TotalAmt = t.RecieveAmount
                                }
                                );

                if (SaleList!=null && SaleList.Count() > 0)
                {
                    var _salerevenueList = SaleList.Sum(x => x.TotalAmt);
                    SaleRevenue = Convert.ToInt32(_salerevenueList);

                }
                #endregion

                long netIncome = SaleRevenue;
                ZeroSeperate(lblNetgain, (Int32)netIncome);
            
        }

        
        private void salebreakdowncategory()
        {
            List<SaleBreakDownController> ResultList = new List<SaleBreakDownController>();

            DateTime toDate = DateTime.Today;

            DateTime fromDate = DateTime.Today.AddMonths(-1);
            decimal total = 0;
            System.Data.Objects.ObjectResult<SaleBreakDownBySegmentWithUnitValue_Result> rList = entity.SaleBreakDownBySegmentWithUnitValue(fromDate, toDate, false, SettingController.DefaultShop.ShortCode);
            foreach (SaleBreakDownBySegmentWithUnitValue_Result r in rList)
            {
                SaleBreakDownController saleObj = new SaleBreakDownController();
                saleObj.bId = Convert.ToInt32(r.Id);
                saleObj.Name = r.Name;
                saleObj.Sales = (r.TotalSale == null) ? 0 : Convert.ToDecimal(r.TotalSale);
                saleObj.saleQty = (r.SaleQty == null) ? 0 : Convert.ToInt32(r.SaleQty);
                total += Convert.ToInt32(r.TotalSale);
                ResultList.Add(saleObj);
            }

            FinalResultListCat.Clear();
            foreach (SaleBreakDownController s in ResultList)
            {
                SaleBreakDownController sObj = new SaleBreakDownController();
                s.BreakDown = (s.Sales == 0) ? 0 : Math.Round((s.Sales / total) * 100);
                FinalResultListCat.Add(s);
            }
        }
        private void toptenproduct()
        {
            DateTime toDate = DateTime.Today;

            DateTime fromDate = DateTime.Today.AddMonths(-1);

            int totalRow = 10;

            itemtopList.Clear();
            System.Data.Objects.ObjectResult<Top100SaleItemList_Result> resultList;
            resultList = entity.Top100SaleItemList(fromDate, toDate, false, totalRow, SettingController.DefaultShop.ShortCode);
            foreach (Top100SaleItemList_Result r in resultList)
            {
                TopProductHolder p = new TopProductHolder();
                if (itemtopList.Where(x => x.ProductId == r.ProductCode).FirstOrDefault() != null)
                {
                    p = itemtopList.Where(x => x.ProductId == r.ProductCode).FirstOrDefault();

                    p.Qty += Convert.ToInt32(r.Qty);

                }
                else
                {
                    p.ProductId = r.ProductCode;
                    p.Name = r.ProductName;
                    p.Discount = 0;
                    p.UnitPrice = Convert.ToInt64(r.UnitPrice);
                    p.Qty = Convert.ToInt32(r.Qty);
                    p.totalAmount = Convert.ToInt64(r.Amount);
                    itemtopList.Add(p);
                }

            }
        }
        private void LoadDataDaily()
        {
            List<ReportItemSummary> itemList = new List<ReportItemSummary>();
            List<Transaction> AllTranslist = new List<Transaction>();
            List<Transaction> transList = new List<Transaction>();
            List<ReportItemSummary> FinalResultList = new List<ReportItemSummary>();

            //List<Transaction> DtransList = new List<Transaction>();

            DateTime toDate = DateTime.Today;

            DateTime fromDate = DateTime.Today.AddMonths(-1);
            string currentshortcode = (from p in entity.Shops where p.Id == SettingController.DefaultShop.Id select p.ShortCode).FirstOrDefault();

            transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && t.Type == TransactionType.Sale && (t.IsDeleted == null || t.IsDeleted == false) && t.Id.Substring(2, 2) == currentshortcode select t).ToList<Transaction>();

            //DtransList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && (t.Type == TransactionType.Settlement || t.Type == TransactionType.Prepaid) && (t.IsDeleted == null || t.IsDeleted == false) && t.Id.Substring(2, 2) == currentshortcode select t).ToList<Transaction>();

            bool IsCash = true, IsFOC = true;
            long CashTotal = 0, FOCAmount = 0;

            System.Data.Objects.ObjectResult<SelectItemListByDate_Result> resultList;
            resultList = entity.SelectItemListByDate(fromDate, toDate, 0);
            foreach (SelectItemListByDate_Result r in resultList)
            {
                ReportItemSummary p = new ReportItemSummary();
                p.Id = r.ItemId;
                p.Name = r.ItemName;
                p.Qty = (int)r.ItemQty;
                p.UnitPrice = Convert.ToInt32(r.UnitPrice);
                p.SellingPrice = Convert.ToInt32(r.SellingPrice);
                p.discountrate = (int)r.DiscountRate;
                p.totalAmount = Convert.ToInt64(r.ItemTotalAmount);
                p.PaymentId = (int)r.PaymentTypeId;
                //p.Size = r.Size;
                p.IsFOC = Convert.ToBoolean(r.IsFOC);

                //if (p.IsFOC == true)
                //{
                //    p.Remark = "FOC";
                //}
                //else
                //{
                //    p.Remark = "";
                //}
                p.Remark = r.Type;
                FinalResultList.Add(p);
            }
            AllTranslist.Clear();
            //if (IsSale == true)
            //{

            if (IsCash)
            {
                itemList.AddRange(FinalResultList.Where(x => x.PaymentId == 1 ).ToList());
                CashTotal += FinalResultList.Where(x => x.PaymentId == 1).Sum(x => x.totalAmount);
                AllTranslist.AddRange(transList.Where(x => x.PaymentTypeId == 1).ToList());
                dailysaleclass sale = new dailysaleclass();
                sale.Payment_Type = "Cash";
                sale.Total = CashTotal;
                dailychart.Add(sale);
            }
           
            if (IsFOC)
            {
                itemList.AddRange(FinalResultList.Where(x => x.PaymentId == 4).ToList());
                //  FOCAmount += FinalResultList.Where(x => x.PaymentId == 4).Sum(x => x.totalAmount);
                FOCAmount += FinalResultList.Where(x => x.IsFOC == true).Sum(x => x.SellingPrice * x.Qty);
                AllTranslist.AddRange(transList.Where(x => x.PaymentTypeId == 4).ToList());
                dailysaleclass sale = new dailysaleclass();
                sale.Payment_Type = "FOC";
                sale.Total = FOCAmount;
                dailychart.Add(sale);
            }
            

        }
    }
}
