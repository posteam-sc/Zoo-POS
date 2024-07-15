using Microsoft.Reporting.WinForms;
using POS.APP_Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
namespace POS
{
    public partial class TicketSummary : Form
    {
        #region Variable

        Boolean IsStart = false;
        POSEntities entity = new POSEntities();

        #endregion
        public TicketSummary()
        {
            InitializeComponent();
        }
        private void TicketSummary_Load(object sender, EventArgs e)
        {
            Localization.Localize_FormControls(this);
            SettingController.SetGlobalDateFormat(dtpFrom);
            SettingController.SetGlobalDateFormat(dtpTo);

            IsStart = true;
            //List<Currency> curList = new List<Currency>();
            //Currency curObj = new Currency();
            //curObj.Id = 0;
            //curObj.CurrencyCode = "ALL";
            //curList.Add(curObj);
            //curList.AddRange((from cur in entity.Currencies orderby cur.Id select cur).ToList());
            //cboCurrency.DataSource = curList;
            //cboCurrency.DisplayMember = "CurrencyCode";
            //cboCurrency.ValueMember = "Id";

            BindCounter();

            //this.reportViewer1.RefreshReport();
            //LoadData();

        }

        private void BindCounter() {
            List<APP_Data.Counter> counterList = new List<APP_Data.Counter>();
            APP_Data.Counter counterObj = new APP_Data.Counter();
            counterObj.Id = 0;
            counterObj.Name = "Select";
            counterList.Add(counterObj);
            counterList.AddRange((from c in entity.Counters orderby c.Id select c).ToList());
            cboCounter.DataSource = counterList;
            cboCounter.DisplayMember = "Name";
            cboCounter.ValueMember = "Id";
            }

        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {
            //LoadData();
        }

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
           // LoadData();
        }


        private void cboCurrency_SelectedValueChanged(object sender, EventArgs e)
        {
          // LoadData();
        }


        private void cboCurrency_SelectedIndexChanged(object sender, EventArgs e)
        {
          //  LoadData();
        }

      

        private void LoadData()
        {
            if (IsStart == true)
            {

                SearchReportFilter();

            }
        }

        private void SearchReportFilter()
        {
            try
            {
                Utility.WaitCursor();

                DateTime fromDate = dtpFrom.Value.Date;
                DateTime toDate = dtpTo.Value.Date;

                int counterId = 0;
                if (chkCounter.Checked && cboCounter.SelectedIndex > 0)
                {
                    counterId = Convert.ToInt32(cboCounter.SelectedValue);
                }
                
             
                //#region for Counter
                if (counterId > 0)
                {
                    List<TicketSummaryReport_Result> resultListMMK = new List<TicketSummaryReport_Result>();
                    resultListMMK = entity.TicketSummaryReport(fromDate, toDate, counterId).ToList();
                    ReportDataSource rds = new ReportDataSource();
                    rds.Name = "DataSet1";
                    rds.Value = resultListMMK;
                    string reportPath = Application.StartupPath + "\\Reports\\TicketSum.rdlc";
                    reportViewer1.LocalReport.ReportPath = reportPath;
                    reportViewer1.LocalReport.DataSources.Clear();
                    reportViewer1.LocalReport.DataSources.Add(rds);

                    //finding the brand name to use as zoo name report
                    string zooName = entity.Brands.Where(x => x.IsDelete == false).SingleOrDefault().Name;

                    ReportParameter ShopNameAsBrandRecord = new ReportParameter("ShopNameAsBrandRecord", zooName);
                    reportViewer1.LocalReport.SetParameters(ShopNameAsBrandRecord);

                    ReportParameter Date = new ReportParameter("Date", " From " + dtpFrom.Value.Date.ToString(SettingController.GlobalDateFormat) + " To " + dtpTo.Value.Date.ToString(SettingController.GlobalDateFormat));
                    reportViewer1.LocalReport.SetParameters(Date);

                }               
                //for not filter contaion...all currency selection(currencycombo selected index is 0)
                else
                {
                    List<TicketSummaryReport_Result> resultListMMK = new List<TicketSummaryReport_Result>();
                    resultListMMK = entity.TicketSummaryReport(fromDate, toDate, 0).ToList();
                    ReportDataSource rdsinmmk = new ReportDataSource();
                    rdsinmmk.Name = "DataSet1";
                    rdsinmmk.Value = resultListMMK;

                   
                    string reportPath = Application.StartupPath + "\\Reports\\TicketSumCur.rdlc";
                    reportViewer1.LocalReport.ReportPath = reportPath;
                    reportViewer1.LocalReport.DataSources.Clear();
                    reportViewer1.LocalReport.DataSources.Add(rdsinmmk);
                     //finding the brand name to use as zoo name report
                    string zooName = entity.Brands.Where(x => x.IsDelete == false).SingleOrDefault().Name;

                    ReportParameter ShopNameAsBrandRecord = new ReportParameter("ShopNameAsBrandRecord", zooName);
                    reportViewer1.LocalReport.SetParameters(ShopNameAsBrandRecord);
                    ReportParameter Date = new ReportParameter("Date", " From " + dtpFrom.Value.Date.ToString(SettingController.GlobalDateFormat) + " To " + dtpTo.Value.Date.ToString(SettingController.GlobalDateFormat));
                    reportViewer1.LocalReport.SetParameters(Date);

                }
                reportViewer1.RefreshReport();
            }
            finally
            {
                Utility.LeaveCursor();
            }
        }

        private void chkCounter_CheckedChanged(object sender, EventArgs e) {
            if (chkCounter.Checked) {
                cboCounter.Enabled = true;
                }
            else {
                cboCounter.Enabled = false;
                cboCounter.SelectedIndex = 1;
                //LoadData();
                }
            }

     
        private void chkCounter_CheckStateChanged(object sender, EventArgs e) {
            cboCounter.SelectedIndex = 0;
            //LoadData();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
