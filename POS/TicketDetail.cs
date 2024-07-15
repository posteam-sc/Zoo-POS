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
    public partial class TicketDetail : Form
    {
        #region Variable

        POSEntities entity = new POSEntities();
        ObjectResult<TicketDetailReport_Result> resultList;
        
        private ToolTip tp = new ToolTip();
        Boolean Isstart = false;

        #endregion

        public TicketDetail()
        {
            InitializeComponent();
        }

        private void TicketDetail_Load(object sender, EventArgs e)
        {
            Localization.Localize_FormControls(this);
            SettingController.SetGlobalDateFormat(dtpFrom);
            SettingController.SetGlobalDateFormat(dtpTo);

            List<APP_Data.Counter> counterList = new List<APP_Data.Counter>();
            APP_Data.Counter counterObj = new APP_Data.Counter();
            counterObj.Id = 0;
            counterObj.Name = "Select Counter";
            counterList.Add(counterObj);
            counterList.AddRange((from c in entity.Counters orderby c.Id select c).ToList());
            cboCounter.DataSource = counterList;
            cboCounter.DisplayMember = "Name";
            cboCounter.ValueMember = "Id";

            List<APP_Data.User> userList = new List<APP_Data.User>();
            APP_Data.User userObj = new APP_Data.User();
            userObj.Id = 0;
            userObj.Name = "Select Cashier";
            userList.Add(userObj);
            userList.AddRange((from u in entity.Users orderby u.Id select u).ToList());
            cboCashier.DataSource = userList;
            cboCashier.DisplayMember = "Name";
            cboCashier.ValueMember = "Id";

            List<APP_Data.Product> productList = new List<APP_Data.Product>();
            APP_Data.Product productObj = new APP_Data.Product();
            productObj.Id = 0;
            productObj.Name = "All Ticket Types";
            productList.Add(productObj);
            productList.AddRange((from p in entity.Products orderby p.Id select p).ToList());
            cboTiType.DataSource = productList;
            cboTiType.DisplayMember = "Name";
            cboTiType.ValueMember = "Id";

            //List<Currency> curList = new List<APP_Data.Currency>();
            //APP_Data.Currency curObj = new APP_Data.Currency();
            //curObj.Id = 0;
            //curObj.CurrencyCode = "ALL";
            //curList.Add(curObj);
            //curList.AddRange((from cur in entity.Currencies orderby cur.Id select cur).ToList());
            //cboCurrency.DataSource = curList;
            //cboCurrency.DisplayMember = "CurrencyCode";
            //cboCurrency.ValueMember = "Id";

            List<APP_Data.tblTime> timeFList = new List<APP_Data.tblTime>();
            APP_Data.tblTime timeFObj = new APP_Data.tblTime();
            timeFObj.Id = 0;
            timeFObj.time = 0;
            timeFList.AddRange((from t in entity.tblTimes orderby t.Id select t).ToList());
            cboTTP.DataSource = timeFList;
            cboTTP.DisplayMember = "time";
            cboTTP.ValueMember = "Id";


            List<APP_Data.tblTime> timeTList = new List<APP_Data.tblTime>();
            APP_Data.tblTime timeTObj = new APP_Data.tblTime();
            timeTObj.Id = 0;
            timeTObj.time = 0;
            timeTList.AddRange((from tt in entity.tblTimes orderby tt.Id select tt).ToList());
            cboFTP.DataSource = timeTList;
            cboFTP.DisplayMember = "time";
            cboFTP.ValueMember = "Id";


            List<APP_Data.tblPeriod> periodFList = new List<APP_Data.tblPeriod>();
            APP_Data.tblPeriod periodFObj = new APP_Data.tblPeriod();
            periodFObj.ID = 0;
            periodFObj.period = "Select";
            periodFList.AddRange((from pp in entity.tblPeriods orderby pp.ID select pp).ToList());
            cboFP.DataSource = periodFList;
            cboFP.DisplayMember = "period";
            cboFP.ValueMember = "ID";

            List<APP_Data.tblPeriod> periodTList = new List<APP_Data.tblPeriod>();
            APP_Data.tblPeriod periodTObj = new APP_Data.tblPeriod();
            periodTObj.ID = 0;
            periodTObj.period = "Select";
            periodTList.AddRange((from ppt in entity.tblPeriods orderby ppt.ID select ppt).ToList());
            cboTP.DataSource = periodTList;
            cboTP.DisplayMember = "period";
            cboFP.ValueMember = "ID";

            Isstart = true;
            //this.reportViewer1.RefreshReport();
            //LoadData();

        }

        #region Function
        private void LoadData()
        {

            try
            {
                Utility.WaitCursor();
                if (Isstart == true)
                {

                    DateTime fromDate = dtpFrom.Value.Date;
                    DateTime toDate = dtpTo.Value.Date;
                    TimeSpan from = TimeSpan.FromHours(1);
                    TimeSpan to = TimeSpan.FromHours(23);
                    DateTime fromtime = Convert.ToDateTime(from.ToString());
                    DateTime totime = Convert.ToDateTime(to.ToString());

                    bool IsTime = chkTime.Checked;

                    bool IsCounter = chkCounter.Checked;
                    bool IsCashier = chkCashier.Checked;
                    bool IsTicketType = chkTiType.Checked;
                    //bool IsCurrency = chkCurrency.Checked;

                    int ftp = 0, ttp = 0;
                    string fp = "";//, tpp = "";

                    int CashierId = 0;
                    int CounterId = 0;
                    int pId = 0;
                    //int cId = 0;
                    string category = "";
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


                    else if (IsTime)
                    {
                        fromDate = dtpFrom.Value.Date;
                        toDate = dtpTo.Value.Date;

                        if (cboFP.SelectedIndex >= 0)
                        {
                            fp = (cboFP.SelectedValue).ToString();
                            ftp = Convert.ToInt32(cboFTP.SelectedValue);
                            if (fp == "1")
                            {
                                fromtime = Convert.ToDateTime(TimeSpan.FromHours(ftp).ToString());
                            }
                            else if (fp == "2")
                            {
                                if (ftp == 12) fromtime = Convert.ToDateTime(TimeSpan.FromSeconds(82856).ToString());
                                else fromtime = Convert.ToDateTime(TimeSpan.FromHours(ftp + 12).ToString());
                            }
                        }

                        if (cboTP.SelectedIndex >= 0)
                        {
                            ttp = Convert.ToInt32(cboTTP.SelectedValue);
                            if (cboTP.SelectedIndex == 0)
                            {
                                totime = Convert.ToDateTime(TimeSpan.FromHours(ttp).ToString());
                            }
                            else if (cboTP.SelectedIndex == 1)
                            {
                                if (ttp == 12) totime = Convert.ToDateTime(TimeSpan.FromSeconds(82856).ToString());
                                else totime = Convert.ToDateTime(TimeSpan.FromHours(ttp + 12).ToString());

                            }
                        }
                    }//end of if time condition


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
                        if (cboTiType.SelectedIndex > 0)
                        {
                            pId = Convert.ToInt32(cboTiType.SelectedValue);
                            category = (from p in entity.Products where p.Id == pId select p.Name).FirstOrDefault();
                        }
                       

                        resultList = entity.TicketDetailReport(fromDate, toDate, CounterId, CashierId, category, fromtime, totime);

                        #region get transaction with cashier & counter & tickettype & currency
                        if (IsCashier == true && IsCounter == true && IsTicketType == true)
                        {
                            CounterId = Convert.ToInt32(cboCounter.SelectedValue);
                            CashierId = Convert.ToInt32(cboCashier.SelectedValue);
                            pId = Convert.ToInt32(cboTiType.SelectedValue);
                            category = (from p in entity.Products where p.Id == pId select p.Name).FirstOrDefault();
                            ShowReportViewer1();
                            lblPeriod.Text = fromDate.ToString(SettingController.GlobalDateFormat) + " To " + toDate.ToString(SettingController.GlobalDateFormat);
                            gbTransactionList.Text = "Ticket Sale Detail Report";


                        }
                        #endregion
                        #region get ticket with cashier only
                        else if (IsCashier == true && IsCounter == false && IsTicketType == false)
                        {
                            CashierId = Convert.ToInt32(cboCashier.SelectedValue);
                            ShowReportViewer1();
                            lblPeriod.Text = fromDate.ToString(SettingController.GlobalDateFormat) + " To " + toDate.ToString(SettingController.GlobalDateFormat);
                            gbTransactionList.Text = "Sale Transaction Report for ";

                        }
                        #endregion
                        #region get all tickets with counter only
                        else if (IsCashier == false && IsCounter == true && IsTicketType == false)
                        {
                            CounterId = Convert.ToInt32(cboCounter.SelectedValue);
                            ShowReportViewer1();
                            lblPeriod.Text = fromDate.ToString(SettingController.GlobalDateFormat) + " To " + toDate.ToString(SettingController.GlobalDateFormat);

                            gbTransactionList.Text = "Sale Transaction Report for ";

                        }
                        #endregion

                        #region get all tickets with Ticket Type only
                        else if (IsCashier == false && IsCounter == false && IsTicketType == true)
                        {
                            pId = Convert.ToInt32(cboTiType.SelectedValue);
                            category = (from p in entity.Products where p.Id == pId select p.Name).FirstOrDefault();
                            resultList = entity.TicketDetailReport(fromDate, toDate, CounterId, CashierId, category, fromtime, totime);
                            ShowReportViewer1();
                            lblPeriod.Text = fromDate.ToString(SettingController.GlobalDateFormat) + " To " + toDate.ToString(SettingController.GlobalDateFormat);
                            gbTransactionList.Text = "Sale Transaction Report for ";
                        }
                        #endregion

                        #region get all tickets with Ticket Type only
                        else if (IsCashier == false && IsCounter == false && IsTicketType == false)
                        {
                            ShowReportViewer();
                            gbTransactionList.Text = "Sale Transaction Report for ";

                        }
                        #endregion

                        #region get all transactions
                        else
                        {

                            //resultList = entity.TicketDetailReport(fromDate, toDate, CounterId, CashierId, category, fromtime, totime);
                            //resultList2 = entity.TicketDetailReport(fromDate, toDate, CounterId, CashierId, category, fromtime, totime);
                            ShowReportViewer();
                            gbTransactionList.Text = "Sale Transaction Report for ";
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

        private void ShowReportViewer1()
        {
            //String gdt = DateTime.Now.ToString(SettingController.GlobalDateFormat);
            ReportDataSource rds = new ReportDataSource();
            rds.Name = "DataSet1";
            rds.Value = resultList;
            string reportPath = Application.StartupPath + "\\Reports\\TicketDetailReport.rdlc";
            reportViewer1.LocalReport.ReportPath = reportPath;
            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(rds);

            //finding the brand name to use as zoo name report
            string zooName = entity.Brands.Where(x => x.IsDelete == false).SingleOrDefault().Name;

            ReportParameter ShopNameAsBrandRecord = new ReportParameter("ShopNameAsBrandRecord", zooName);
            reportViewer1.LocalReport.SetParameters(ShopNameAsBrandRecord);

            ReportParameter TimePeriod = new ReportParameter("TimePeriod", " From " + dtpFrom.Value.ToString(SettingController.GlobalDateFormat) + " To " + dtpTo.Value.ToString(SettingController.GlobalDateFormat));
            reportViewer1.LocalReport.SetParameters(TimePeriod);

            //ReportParameter GenerateDateTime = new ReportParameter("GenerateDateTime", gdt);
            //reportViewer1.LocalReport.SetParameters(GenerateDateTime);

            reportViewer1.RefreshReport();
        }

        private void ShowReportViewer()
        {
            String gdt = DateTime.Now.ToString(SettingController.GlobalDateFormat+" hh:mm:ss tt");

            ReportDataSource rds = new ReportDataSource();
            ReportDataSource rds2 = new ReportDataSource();
            rds.Name = "DataSet1";
            rds.Value = resultList;
            rds2.Name = "DataSet2";
            //rds2.Value = resultList2;
            string reportPath = Application.StartupPath + "\\Reports\\TicketDetail.rdlc";
            reportViewer1.LocalReport.ReportPath = reportPath;
            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(rds);
            reportViewer1.LocalReport.DataSources.Add(rds2);

            //finding the brand name to use as zoo name report
            string zooName = entity.Brands.Where(x => x.IsDelete == false).SingleOrDefault().Name;

            ReportParameter ShopNameAsBrandRecord = new ReportParameter("ShopNameAsBrandRecord", zooName);
            reportViewer1.LocalReport.SetParameters(ShopNameAsBrandRecord);
            ReportParameter TimePeriod = new ReportParameter("TimePeriod", " From " + dtpFrom.Value.ToString(SettingController.GlobalDateFormat) + " To " + dtpTo.Value.ToString(SettingController.GlobalDateFormat));
            reportViewer1.LocalReport.SetParameters(TimePeriod);

            //ReportParameter GenerateDateTime = new ReportParameter("GenerateDateTime", gdt);
            //reportViewer1.LocalReport.SetParameters(GenerateDateTime);

            reportViewer1.RefreshReport();

        }


        #endregion

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
            //LoadData();
        }

        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {
            //LoadData();
        }

        private void chkCashier_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCashier.Checked)
            {
                cboCashier.Enabled = true;
            }
            else
            {
                cboCashier.Enabled = false;
                cboCashier.SelectedIndex = 0;
                //LoadData();
            }
        }

        private void chkCounter_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCounter.Checked)
            {
                cboCounter.Enabled = true;
            }
            else
            {
                 cboCounter.Enabled = false;
                cboCounter.SelectedIndex = 1;
                //LoadData();
            }
        }

        private void cboCashier_SelectedValueChanged(object sender, EventArgs e)
        {
            //LoadData();
        }

        private void cboCounter_SelectedValueChanged(object sender, EventArgs e)
        {
           // LoadData();
        }

        private void cboCashier_SelectedIndexChanged(object sender, EventArgs e)
        {
            //LoadData();
        }

        private void cboCounter_SelectedIndexChanged(object sender, EventArgs e)
        {
            //LoadData();
        }

        private void cboTiType_SelectedValueChanged(object sender, EventArgs e)
        {
            //LoadData();
        }

        private void chkTiType_CheckedChanged(object sender, EventArgs e)
        {
            if (chkTiType.Checked)
            {
                cboTiType.Enabled = true;
            }
            else
            {
                cboTiType.Enabled = false;
                cboTiType.SelectedIndex = 0;
               // LoadData();
            }
        }

        private void chkTime_CheckedChanged(object sender, EventArgs e)
        {
            if (chkTime.Checked)
            {
                label5.Enabled = true;
                label4.Enabled = true;
                cboFTP.Enabled = true;
                cboFP.Enabled = true;
                cboTTP.Enabled = true;
                cboTP.Enabled = true;
            }
            else
            {
                label5.Enabled = false;
                label4.Enabled = false;
                cboFTP.Enabled = false;
                cboFP.Enabled = false;
                cboTTP.Enabled = false;
                cboTP.Enabled = false;
            }
        }

        private void cboFTP_SelectedIndexChanged(object sender, EventArgs e)
        {
            //LoadData();
        }

        private void cboFP_SelectedIndexChanged(object sender, EventArgs e)
        {
            //LoadData();
        }

        private void cboTTP_SelectedIndexChanged(object sender, EventArgs e)
        {
            //LoadData();
        }

        private void cboTP_SelectedIndexChanged(object sender, EventArgs e)
        {
            //LoadData();
        }

        private void cboCurrency_SelectedIndexChanged(object sender, EventArgs e)
        {
            //LoadData();
        }

       
        private void cboCurrency_SelectedValueChanged(object sender, EventArgs e)
        {
            //LoadData();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            LoadData();
        }

    }
}
