using Microsoft.Reporting.WinForms;
using POS.APP_Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace POS
{
    public partial class Loc_ItemSummary : Form
    {
        #region Variable
        List<Transaction> DtransList = new List<Transaction>();
        POSEntities entity = new POSEntities();
        //List<Product> itemList = new List<Product>();
        List<ReportItemSummary> itemList = new List<ReportItemSummary>();
        List<ReportItemSummary> IList = new List<ReportItemSummary>();
        long CashTotal = 0, CreditTotal = 0, CreditRefundAmt=0, FOCAmount = 0, MPUAmount = 0, TesterAmount = 0, GiftCardAmount = 0, Total = 0, CreditReceive = 0, RefundAmt = 0; long UseGiftAmount = 0; long CashAmtFromGiftCard = 0;
        long totalSettlement = 0;
        List<Transaction> AllTranslist = new List<Transaction>();
         List<ReportItemSummary> FinalResultListInMMK = new List<ReportItemSummary>();
        Boolean Isstart = false;
        #endregion        
        
        #region Event
        public Loc_ItemSummary()
        {
            InitializeComponent();
        }

        private void Loc_ItemSummary_Load(object sender, EventArgs e)
        {
            Localization.Localize_FormControls(this);
            SettingController.SetGlobalDateFormat(dtTo);
            SettingController.SetGlobalDateFormat(dtFrom);

            Isstart = true;
            bindConterList();
            //LoadData();
           // this.reportViewer1.RefreshReport();
        }

        private void bindConterList() {
            List<APP_Data.Counter> counterList = new List<APP_Data.Counter>();
            APP_Data.Counter counterObj = new APP_Data.Counter();
            counterObj.Id = 0;
            counterObj.Name = "Select Counter";
            counterList.Add(counterObj);
            counterList.AddRange((from c in entity.Counters orderby c.Id select c).ToList());
            cboCounter.DataSource = counterList;
            cboCounter.DisplayMember = "Name";
            cboCounter.ValueMember = "Id";
            }

        private void dtFrom_ValueChanged(object sender, EventArgs e)
        {
          //  LoadData();
        }

        private void dtTo_ValueChanged(object sender, EventArgs e)
        {
          //  LoadData();
        }

        private void rdbSale_CheckedChanged(object sender, EventArgs e)
        {
            //LoadData();
        }

        private void cboCounter_SelectedIndexChanged(object sender, EventArgs e) {
           // LoadData();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
           LoadData();
        }

        private void chkCash_CheckedChanged(object sender, EventArgs e)
        {
           // LoadData();
        }

        private void chkGiftCard_CheckedChanged(object sender, EventArgs e)
        {
          //  LoadData();
        }

        private void chkMPU_CheckedChanged(object sender, EventArgs e)
        {
           // LoadData();
        }

        private void chkCredit_CheckedChanged(object sender, EventArgs e)
        {
            //LoadData();
        }

        private void chkFOC_CheckedChanged(object sender, EventArgs e)
        {
           // LoadData();
        }

        private void chkTester_CheckedChanged(object sender, EventArgs e)
        {
           // LoadData();
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
                if (Isstart == true)
                {
                    int CounterId = 0;
                    if (cboCounter.SelectedIndex > 0)
                    {
                        CounterId = Convert.ToInt32(cboCounter.SelectedValue);
                    }
                    DateTime fromDate = dtFrom.Value.Date;
                    DateTime toDate = dtTo.Value.Date;
                    CashTotal = 0; CreditTotal = 0; FOCAmount = 0; MPUAmount = 0; TesterAmount = 0; GiftCardAmount = 0; RefundAmt = 0; CreditRefundAmt = 0; Total = 0; CashAmtFromGiftCard = 0;
                    IList.Clear();
                    itemList.Clear();
                    System.Data.Objects.ObjectResult<SelectItemListByDate_Result> resultListInMMK;
                    List<Transaction> transList = new List<Transaction>();
                    FinalResultListInMMK = new List<ReportItemSummary>();

                    // transList = (from t in entity.Transactions where  EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && (t.Type == TransactionType.Sale || t.Type == TransactionType.Credit || t.Type == TransactionType.Refund || t.Type == TransactionType.CreditRefund) && (t.IsDeleted == null || t.IsDeleted == false) && ((currentshortcode != "0" && t.Id.Substring(2, 2) == currentshortcode) || (currentshortcode == "0" && 1 == 1)) select t).ToList<Transaction>();
                    //DtransList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && (t.Type == TransactionType.Settlement || t.Type == TransactionType.Prepaid) && (t.IsDeleted == null || t.IsDeleted == false) && ((currentshortcode != "0" && t.Id.Substring(2, 2) == currentshortcode) || (currentshortcode == "0" && 1 == 1)) select t).ToList<Transaction>();
                    resultListInMMK = entity.SelectItemListByDate(fromDate, toDate, CounterId);
                    foreach (SelectItemListByDate_Result resultiteminMMK in resultListInMMK)
                    {
                        ReportItemSummary p = new ReportItemSummary();
                        p.Id = resultiteminMMK.ItemId;
                        p.Name = resultiteminMMK.ItemName;
                        p.Qty = (int)resultiteminMMK.ItemQty;
                        p.UnitPrice = Convert.ToInt32(resultiteminMMK.UnitPrice);
                        p.SellingPrice = Convert.ToInt32(resultiteminMMK.SellingPrice);
                        p.discountrate = (int)resultiteminMMK.DiscountRate;
                        p.totalAmount = Convert.ToInt64(resultiteminMMK.ItemTotalAmount);
                        p.PaymentId = (int)resultiteminMMK.PaymentTypeId;
                        p.discountAmount = Convert.ToDecimal(resultiteminMMK.DiscountAmt);
                        p.IsFOC = Convert.ToBoolean(resultiteminMMK.IsFOC);
                        p.Remark = resultiteminMMK.Type;
                        FinalResultListInMMK.Add(p);
                    }
                
                    AllTranslist.Clear();
                    CreditReceive = 0;
                    UseGiftAmount = 0;
                    #region no need to show this record 
                    //  int totalDiscountRFTransAmount = 0;
                    //int totalDiscountCrdRFTransAmount = 0;
                    //  string[] type = { "Refund", "CreditRefund" };
                    //if (IsCash)
                    //{
                    //    itemList.AddRange(FinalResultList.Where(x => x.PaymentId == 1 && !type.Contains(x.Remark)).ToList());
                    //    CashTotal += FinalResultList.Where(x => x.PaymentId == 1 && !type.Contains(x.Remark)).Sum(x => x.totalAmount);
                    //    AllTranslist.AddRange(transList.Where(x => x.PaymentTypeId == 1 && !type.Contains(x.Type)).ToList());
                    //}
                    //if (IsCredit)
                    //{
                    //    itemList.AddRange(FinalResultList.Where(x => x.PaymentId == 2 && !type.Contains(x.Remark)).ToList());
                    //    CreditTotal += FinalResultList.Where(x => x.PaymentId == 2 && !type.Contains(x.Remark)).Sum(x => x.totalAmount);
                    //    AllTranslist.AddRange(transList.Where(x => x.PaymentTypeId == 2 && !type.Contains(x.Type)).ToList());
                    //    CreditReceive += Convert.ToInt64(transList.Where(x => x.PaymentTypeId == 2 && !type.Contains(x.Type)).Sum(x => x.RecieveAmount));
                    //}
                    //if (IsGiftCard)
                    //{
                    //    itemList.AddRange(FinalResultList.Where(x => x.PaymentId == 3 && !type.Contains(x.Remark)).ToList());
                    //    GiftCardAmount += FinalResultList.Where(x => x.PaymentId == 3 && !type.Contains(x.Remark)).Sum(x => x.totalAmount);
                    //    AllTranslist.AddRange(transList.Where(x => x.PaymentTypeId == 3 && !type.Contains(x.Type)).ToList());
                    //    UseGiftAmount += Convert.ToInt64(transList.Where(x => x.PaymentTypeId == 3 && !type.Contains(x.Type)).Sum(x => x.GiftCardAmount));               
                    //    CashAmtFromGiftCard += Convert.ToInt64(transList.Where(x => x.PaymentTypeId == 3 && !type.Contains(x.Type)).Sum(x => x.TotalAmount-x.GiftCardAmount));
                    //}
                    //if (IsRefund)
                    //{    
                    //    totalDiscountRFTransAmount = Convert.ToInt32(transList.Where(x => x.Id.Substring(0, 2) == "RF" && x.Type != "CreditRefund").Sum(x => x.DiscountAmount));
                    //    totalDiscountCrdRFTransAmount = Convert.ToInt32(transList.Where(x => x.Id.Substring(0, 2) == "RF" && x.Type == "CreditRefund").Sum(x => x.DiscountAmount));
                    //    RefundAmt += FinalResultList.Where(x => x.Remark == "Refund").Sum(x => x.totalAmount);
                    //    CreditRefundAmt += FinalResultList.Where(x => x.Remark == "CreditRefund").Sum(x => x.totalAmount);
                    //    if (totalDiscountRFTransAmount != 0)
                    //    {
                    //        RefundAmt -= totalDiscountRFTransAmount;
                    //    }
                    //    if (totalDiscountRFTransAmount != 0)
                    //    {
                    //        CreditRefundAmt -= totalDiscountCrdRFTransAmount;
                    //    }
                    //    AllTranslist.AddRange(transList.Where(x => type.Contains(x.Type)).ToList());
                    //}
                    //if (IsFOC)
                    //{
                    //    itemList.AddRange(FinalResultList.Where(x => x.PaymentId == 4).ToList());
                    //    FOCAmount += FinalResultList.Where(x => x.IsFOC == true).Sum(x => x.SellingPrice * x.Qty);
                    //    AllTranslist.AddRange(transList.Where(x => x.PaymentTypeId == 4).ToList());
                    //}
                    //if (IsMPU)
                    //{
                    //    itemList.AddRange(FinalResultList.Where(x => x.PaymentId == 5).ToList());
                    //    MPUAmount += FinalResultList.Where(x => x.PaymentId == 5).Sum(x => x.totalAmount);
                    //    AllTranslist.AddRange(transList.Where(x => x.PaymentTypeId == 5).ToList());
                    //}
                    //if (IsTester)
                    //{
                    //    itemList.AddRange(FinalResultList.Where(x => x.PaymentId == 6).ToList());
                    //    TesterAmount += FinalResultList.Where(x => x.PaymentId == 6).Sum(x => x.totalAmount);
                    //    AllTranslist.AddRange(transList.Where(x => x.PaymentTypeId == 6).ToList());
                    //}           
                    #endregion
                    ShowReportViewer();
                }

            }
            finally
            {
                Utility.LeaveCursor();
            }
        }

        private void ShowReportViewer()
        {
            dsReportTemp dsReport = new dsReportTemp();
            dsReportTemp._LO_c_ItemSummaryDataTable dtItemsummaryReportInMMK = (dsReportTemp._LO_c_ItemSummaryDataTable)dsReport.Tables["LO'c_ItemSummary"];           
            FinalResultListInMMK = FinalResultListInMMK.OrderBy(x => x.Name).ToList();
            //for mmk 
            foreach (ReportItemSummary p in FinalResultListInMMK) {
                dsReportTemp._LO_c_ItemSummaryRow newitemssummaryRowInMMK = dtItemsummaryReportInMMK.New_LO_c_ItemSummaryRow();
                newitemssummaryRowInMMK.ItemCode = p.Id;
                newitemssummaryRowInMMK.Name = p.Name;
                newitemssummaryRowInMMK.Size = p.Size;
                newitemssummaryRowInMMK.DiscountRate = p.discountrate;
                newitemssummaryRowInMMK.DiscountAmt =Convert.ToString( p.discountAmount);
                newitemssummaryRowInMMK.Qty = p.Qty.ToString();
                newitemssummaryRowInMMK.UnitPrice = p.UnitPrice.ToString();                
                newitemssummaryRowInMMK.TotalAmount = Convert.ToInt64(p.totalAmount);
                newitemssummaryRowInMMK.SellingPrice = Convert.ToInt64(p.SellingPrice);
                newitemssummaryRowInMMK.Remark = p.Remark.ToString();
                //adding the item report rows to the item summary main rdlc.
                dtItemsummaryReportInMMK.Add_LO_c_ItemSummaryRow(newitemssummaryRowInMMK);
            }
            
            Total = CashTotal + CreditTotal + FOCAmount + MPUAmount + GiftCardAmount + TesterAmount;
            totalSettlement = DtransList.Sum(x => x.TotalAmount).Value;           
            decimal OverAllDis = 0;
            decimal OverAllMCDis = 0;
            decimal OverAllDisG = 0;
            decimal OverAllMCDisG = 0;
            decimal OverAllDisCrd = 0;
            decimal OverAllMCDisCrd = 0;
            decimal OverAllDisMpu = 0;
            decimal OverAllMCDisMpu = 0;
            int cashmcdiscount = 0;
            int totalDiscountRFTransAmount = 0;
            int totalDiscountCrdRFTransAmount = 0;
            totalDiscountRFTransAmount = Convert.ToInt32(AllTranslist.Where(x => x.Id.Substring(0, 2) == "RF" && x.Type != "CreditRefund").Sum(x => x.DiscountAmount));
            totalDiscountCrdRFTransAmount = Convert.ToInt32(AllTranslist.Where(x => x.Id.Substring(0, 2) == "RF" && x.Type == "CreditRefund").Sum(x => x.DiscountAmount));
            List<Transaction> creAllTranslist = AllTranslist.Where(x => x.Type == "Credit").ToList();
            List<Transaction> otherallTranslist = AllTranslist.Where(x => x.Type != "Credit").ToList();   
            foreach (Transaction t in otherallTranslist)  {               
                List<TransactionDetail> tdList = new List<TransactionDetail>();
                tdList = t.TransactionDetails.Where(x => x.TransactionId.Substring(0, 2) != "RF").ToList();
                int itemDis = 0;
                foreach (TransactionDetail td in tdList)
                {
                    itemDis += Convert.ToInt32((td.UnitPrice * (td.DiscountRate / 100))*td.Qty);
                }
             
                if(t.PaymentTypeId==5)
                {
                    MPUAmount -= Convert.ToInt64(t.DiscountAmount - itemDis);
                    OverAllDisMpu += Convert.ToDecimal(t.DiscountAmount - itemDis);
                    //if ((int)(t.MCDiscountAmt == null ? 0 : t.MCDiscountAmt) != 0)
                    //{
                    //    MPUAmount -= Convert.ToInt64(t.MCDiscountAmt);
                    //    OverAllMCDisMpu += Convert.ToDecimal(t.MCDiscountAmt);
                    //}
                    //else if ((int)(t.BDDiscountAmt == null ? 0 : t.BDDiscountAmt) != 0)
                    //{
                    //    MPUAmount -= Convert.ToInt64(t.BDDiscountAmt);
                    //    OverAllMCDisMpu += Convert.ToDecimal(t.BDDiscountAmt);
                    //}
                }
                     else if (t.PaymentTypeId==3)
                {
                    OverAllDisG += Convert.ToDecimal(t.DiscountAmount - itemDis);
                  
                }
                else
                {
                    OverAllDis += Convert.ToDecimal(t.DiscountAmount - itemDis);
                   
                }     
            }
            foreach (Transaction t in creAllTranslist)
            {
                List<TransactionDetail> tdList = new List<TransactionDetail>();
                tdList = t.TransactionDetails.Where(x => x.TransactionId.Substring(0, 2) != "RF").ToList();
                int itemDis = 0;
                foreach (TransactionDetail td in tdList)
                {
                    itemDis += Convert.ToInt32((td.UnitPrice * (td.DiscountRate / 100)) * td.Qty);
                }
                OverAllDisCrd += Convert.ToDecimal(t.DiscountAmount - itemDis);


               
            }
            cashmcdiscount = Convert.ToInt32((OverAllMCDis));

            if(totalDiscountRFTransAmount!=0)
            {
                OverAllDis -= totalDiscountRFTransAmount;
            }
            if(totalDiscountCrdRFTransAmount!=0)
            {
                OverAllDisCrd -= totalDiscountCrdRFTransAmount;
            }        
            decimal actualAmount = (Convert.ToDecimal(CashTotal + CreditReceive + CashAmtFromGiftCard) + (totalSettlement ) - (OverAllDis) - (OverAllMCDis));
            ReportDataSource rdsInMMK = new ReportDataSource("ItemSummary", dsReport.Tables["LO'c_ItemSummary"]);
             string reportPath = Application.StartupPath + "\\Reports\\DailySaleSummary.rdlc";
            reportViewer1.LocalReport.ReportPath = reportPath;
            reportViewer1.LocalReport.DataSources.Clear();

            reportViewer1.LocalReport.DataSources.Add(rdsInMMK);
          
            ReportParameter ItemReportTitle = new ReportParameter("ItemReportTitle", gbList.Text + " for " + "MainShop");
            reportViewer1.LocalReport.SetParameters(ItemReportTitle);

            ReportParameter Date = new ReportParameter("Date", " From " + dtFrom.Value.Date.ToString(SettingController.GlobalDateFormat) + " To " + dtTo.Value.Date.ToString(SettingController.GlobalDateFormat));
            reportViewer1.LocalReport.SetParameters(Date);

            ReportParameter TotalAmount = new ReportParameter("TotalAmount", (Total - FOCAmount).ToString());
            reportViewer1.LocalReport.SetParameters(TotalAmount);

            ReportParameter CreditAmount = new ReportParameter("CreditAmount", Convert.ToInt64((CreditTotal - CreditReceive - Convert.ToInt32(OverAllMCDisCrd) - OverAllDisCrd)).ToString());
            reportViewer1.LocalReport.SetParameters(CreditAmount);

            ReportParameter CashAmount = new ReportParameter("CashAmount", (CashTotal + CreditReceive + CashAmtFromGiftCard - cashmcdiscount - OverAllDis).ToString());
            reportViewer1.LocalReport.SetParameters(CashAmount);

            ReportParameter DisAmount = new ReportParameter("DisAmount", OverAllDis.ToString());
            reportViewer1.LocalReport.SetParameters(DisAmount);
            ReportParameter MemberCardDiscount = new ReportParameter("MemberCardDiscount", Convert.ToInt32(OverAllMCDis).ToString());
            reportViewer1.LocalReport.SetParameters(MemberCardDiscount);
            ReportParameter CreditDiscountAmt = new ReportParameter("CreditDiscountAmt", OverAllDisCrd.ToString());
            reportViewer1.LocalReport.SetParameters(CreditDiscountAmt);

            ReportParameter CreditMemberDis = new ReportParameter("CreditMemberDis", Convert.ToInt32(OverAllMCDisCrd).ToString());
            reportViewer1.LocalReport.SetParameters(CreditMemberDis);
          
            ReportParameter MPUDiscountAmt = new ReportParameter("MPUDiscountAmt", OverAllDisMpu.ToString());
            reportViewer1.LocalReport.SetParameters(MPUDiscountAmt);


            ReportParameter MPUMemberDis = new ReportParameter("MPUMemberDis", Convert.ToInt32(OverAllMCDisMpu).ToString());
            reportViewer1.LocalReport.SetParameters(MPUMemberDis);
           
            ReportParameter UsedGiftAmount = new ReportParameter("UsedGiftAmount", UseGiftAmount.ToString());
            reportViewer1.LocalReport.SetParameters(UsedGiftAmount);
            ReportParameter GCDiscountAmt = new ReportParameter("GCDiscountAmt", OverAllDisG.ToString());
            reportViewer1.LocalReport.SetParameters(GCDiscountAmt);


            ReportParameter GCMemberAmt = new ReportParameter("GCMemberAmt", Convert.ToInt32(OverAllMCDisG).ToString());
            reportViewer1.LocalReport.SetParameters(GCMemberAmt);

            ReportParameter FOC = new ReportParameter("FOC", FOCAmount.ToString());
            reportViewer1.LocalReport.SetParameters(FOC);

            ReportParameter MPU = new ReportParameter("MPU", MPUAmount.ToString());
            reportViewer1.LocalReport.SetParameters(MPU);

            ReportParameter Tester = new ReportParameter("Tester", TesterAmount.ToString());
            reportViewer1.LocalReport.SetParameters(Tester);

            ReportParameter TotalSettlement = new ReportParameter("TotalSettlement", totalSettlement.ToString());
            reportViewer1.LocalReport.SetParameters(TotalSettlement);
    
            ReportParameter ActualAmount = new ReportParameter("ActualAmount", Convert.ToInt32(actualAmount).ToString());
            reportViewer1.LocalReport.SetParameters(ActualAmount);

            ReportParameter TotalRefund = new ReportParameter("TotalRefund", RefundAmt.ToString());
            reportViewer1.LocalReport.SetParameters(TotalRefund);

            ReportParameter CreditRefund = new ReportParameter("CreditRefund", CreditRefundAmt.ToString());
            reportViewer1.LocalReport.SetParameters(CreditRefund);
            ReportParameter CashInHand = new ReportParameter("CashInHand", (actualAmount - RefundAmt).ToString());
            reportViewer1.LocalReport.SetParameters(CashInHand);
            //finding the brand name to use as zoo name report
            string zooName = entity.Brands.Where(x => x.IsDelete == false).SingleOrDefault().Name;
            ReportParameter ShopNameAsBrandRecord = new ReportParameter("ShopNameAsBrandRecord", zooName);
            reportViewer1.LocalReport.SetParameters(ShopNameAsBrandRecord);
            reportViewer1.RefreshReport();
        }
        #endregion    
    }
}
