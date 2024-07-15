using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace POS
{
    public partial class RoleManagement : Form
    {
        #region Events

        public RoleManagement()
        {
            InitializeComponent();
        }

        private void RoleManagement_Load(object sender, EventArgs e)
        {
            Localization.Localize_FormControls(this);
            // tabPage1.Height= (tabControl1.Height - tabPage1.Height) + 10;
            #region Load Data for Super Casher Role
            RoleManagementController controller = new RoleManagementController();
            //Super Casher Id
            controller.Load(2);

            //Setting
            chkSettingSC.Checked = controller.Setting.Add;



            //City
            chkAddCitySC.Checked = controller.City.Add;
            chkEditCitySC.Checked = controller.City.EditOrDelete;

            //Product
            chkViewProductSC.Checked = controller.Product.View;
            chkEditProductSC.Checked = controller.Product.EditOrDelete;
            chkAddProductSC.Checked = controller.Product.Add;

            //Brand
            chkViewBrandSC.Checked = controller.Brand.View;
            chkEditBrandSC.Checked = controller.Brand.EditOrDelete;
            chkAddBrandSC.Checked = controller.Brand.Add;


            //Category
            chkViewCategorySC.Checked = controller.Category.View;
            chkEditCategorySC.Checked = controller.Category.EditOrDelete;
            chkAddCategorySC.Checked = controller.Category.Add;

            //SubCategory
            chkViewSubCategorySC.Checked = controller.SubCategory.View;
            chkEditSubCategorySC.Checked = controller.SubCategory.EditOrDelete;
            chkAddSubCategorySC.Checked = controller.SubCategory.Add;

            //Counter            
            chkEditCounterSC.Checked = controller.Counter.EditOrDelete;
            chkAddCounterSC.Checked = controller.Counter.Add;


            //Transaction 
            chkDeleteTransactionSC.Checked = controller.Transaction.EditOrDelete;
            chkDeleteAndCopyTransactionSC.Checked = controller.Transaction.DeleteAndCopy;

            //Ticket 
            chkReprintTicketSC.Checked = controller.Ticket.Reprint;

            //Transaction Detail
            chkDeleteTransactionDetailSC.Checked = controller.TransactionDetail.EditOrDelete;


            // Reports
            chkDailySaleSummary_SC1.Checked = controller.DailySaleSummary.View;
            chkTransactionSC1.Checked = controller.TransactionReport.View;
            chkTransactionSummarySC1.Checked = controller.TransactionSummaryReport.View;
            chkTransactionDetail_SC1.Checked = controller.TransactionDetailReport.View;
            chkItemSummary_SC1.Checked = controller.ItemSummaryReport.View;
            chkTopBestSellerSC1.Checked = controller.TopBestSellerReport.View;
            //chkReportAdjustmentSC.Checked = controller.AdjustmentReport.View;

            chkReportAMRSC1.Checked = controller.AverageMonthlyReport.View;
            chkNetIncomeSC.Checked = controller.NetIncomeReport.View;

            chkTicketDetailRSC.Checked = controller.TicketDetailReport.View;  /* HMT*/
            chkTicketSumRSC.Checked = controller.TicketSummaryReport.View;      /* HMT*/
          
            #endregion

            #region Load Data for Casher Role

            RoleManagementController controllerCasher = new RoleManagementController();



            //Super Casher Id
            controllerCasher.Load(3);

            //Setting
            chkSettingC.Checked = controllerCasher.Setting.Add;


            //City
            chkAddCityC.Checked = controllerCasher.City.Add;
            chkEditCityC.Checked = controllerCasher.City.EditOrDelete;

            //Product
            chkViewProductC.Checked = controllerCasher.Product.View;
            chkEditProductC.Checked = controllerCasher.Product.EditOrDelete;
            chkAddProductC.Checked = controllerCasher.Product.Add;

            //Brand
            chkViewBrandC.Checked = controllerCasher.Brand.View;
            chkEditBrandC.Checked = controllerCasher.Brand.EditOrDelete;
            chkAddBrandC.Checked = controllerCasher.Brand.Add;

            //Ticket 
            chkReprintTicketC.Checked = controllerCasher.Ticket.Reprint;

            //Category
            chkViewCategoryC.Checked = controllerCasher.Category.View;
            chkEditCategoryC.Checked = controllerCasher.Category.EditOrDelete;
            chkAddCategoryC.Checked = controllerCasher.Category.Add;

            //SubCategory
            chkViewSubCategoryC.Checked = controllerCasher.SubCategory.View;
            chkEditSubCategoryC.Checked = controllerCasher.SubCategory.EditOrDelete;
            chkAddSubCategoryC.Checked = controllerCasher.SubCategory.Add;

            //Counter            
            chkEditCounterC.Checked = controllerCasher.Counter.EditOrDelete;
            chkAddCounterC.Checked = controllerCasher.Counter.Add;


            //Transaction 
            chkDeleteTransactionC.Checked = controllerCasher.Transaction.EditOrDelete;
            chkDeleteAndCopyTransactionC.Checked = controllerCasher.Transaction.DeleteAndCopy;

            //Transaction Detail
            chkDeleteTransactionDetailC.Checked = controllerCasher.TransactionDetail.EditOrDelete;

            //Report
            chkDailySaleSummary_C1.Checked = controllerCasher.DailySaleSummary.View;
            chkTransactionC1.Checked = controllerCasher.TransactionReport.View;
            chkTransactionSummaryC1.Checked = controllerCasher.TransactionSummaryReport.View;
            chkTransactionDetail_C1.Checked = controllerCasher.TransactionDetailReport.View;
            chkItemSummary_C1.Checked = controllerCasher.ItemSummaryReport.View;
            chkTopBestSellerC1.Checked = controllerCasher.TopBestSellerReport.View;
            chkReportAMRC1.Checked = controllerCasher.AverageMonthlyReport.View;
            chkNetIncomeC.Checked = controllerCasher.NetIncomeReport.View;
            chkTicketDetailRC.Checked = controllerCasher.TicketDetailReport.View;     /* HMT*/
            chkTicketSumRC.Checked = controllerCasher.TicketSummaryReport.View;     /* HMT*/

            #endregion

            IsAllCheckedOperation(2);
            IsAllCheckedOperation(3);
            IsAllCheckedReport(2);
            IsAllCheckedReport(3);

        }



        private void btnSubmit_Click(object sender, EventArgs e)
        {
            #region Save for Super Casher Role

            RoleManagementController controller = new RoleManagementController();

            //Setting
            controller.Setting.Add = chkSettingSC.Checked;

            //City
            controller.City.EditOrDelete = chkEditCitySC.Checked;
            controller.City.Add = chkAddCitySC.Checked;

            //Product
            controller.Product.View = chkViewProductSC.Checked;
            controller.Product.EditOrDelete = chkEditProductSC.Checked;
            controller.Product.Add = chkAddProductSC.Checked;

            //Brand
            controller.Brand.View = chkViewBrandSC.Checked;
            controller.Brand.EditOrDelete = chkEditBrandSC.Checked;
            controller.Brand.Add = chkAddBrandSC.Checked;

            //Category
            controller.Category.View = chkViewCategorySC.Checked;
            controller.Category.EditOrDelete = chkEditCategorySC.Checked;
            controller.Category.Add = chkAddCategorySC.Checked;

            //SubCategory
            controller.SubCategory.View = chkViewSubCategorySC.Checked;
            controller.SubCategory.EditOrDelete = chkEditSubCategorySC.Checked;
            controller.SubCategory.Add = chkAddSubCategorySC.Checked;

            //Counter            
            controller.Counter.EditOrDelete = chkEditCounterSC.Checked;
            controller.Counter.Add = chkAddCounterSC.Checked;

            // Transaction
            controller.Transaction.EditOrDelete = chkDeleteTransactionSC.Checked;
            controller.Transaction.DeleteAndCopy = chkDeleteAndCopyTransactionSC.Checked;

            // Transaction Detial
            controller.TransactionDetail.EditOrDelete = chkDeleteTransactionDetailSC.Checked;

            // Reports
            controller.DailySaleSummary.View = chkDailySaleSummary_SC1.Checked;
            controller.TransactionReport.View = chkTransactionSC1.Checked;
            controller.TransactionSummaryReport.View = chkTransactionSummarySC1.Checked;
            controller.TransactionDetailReport.View = chkTransactionDetail_SC1.Checked;
            controller.ItemSummaryReport.View = chkItemSummary_SC1.Checked;
            controller.TopBestSellerReport.View = chkTopBestSellerSC1.Checked;
            controller.AverageMonthlyReport.View = chkReportAMRSC1.Checked;
            controller.NetIncomeReport.View = chkNetIncomeSC.Checked;

            controller.TicketDetailReport.View = chkTicketDetailRSC.Checked;   /* HMT*/
            controller.TicketSummaryReport.View = chkTicketSumRSC.Checked;    /* HMT*/
            controller.Ticket.Reprint = chkReprintTicketSC.Checked;
            //Super Casher Id
            controller.Save(2);

            #endregion

            #region Save for Casher Role

            RoleManagementController controllerCasher = new RoleManagementController();
            //Setting
            controllerCasher.Setting.Add = chkSettingC.Checked;

            //City
            controllerCasher.City.EditOrDelete = chkEditCityC.Checked;
            controllerCasher.City.Add = chkAddCityC.Checked;

            //Product
            controllerCasher.Product.View = chkViewProductC.Checked;
            controllerCasher.Product.EditOrDelete = chkEditProductC.Checked;
            controllerCasher.Product.Add = chkAddProductC.Checked;

            //Brand
            controllerCasher.Brand.View = chkViewBrandC.Checked;
            controllerCasher.Brand.EditOrDelete = chkEditBrandC.Checked;
            controllerCasher.Brand.Add = chkAddBrandC.Checked;

            //Category
            controllerCasher.Category.View = chkViewCategoryC.Checked;
            controllerCasher.Category.EditOrDelete = chkEditCategoryC.Checked;
            controllerCasher.Category.Add = chkAddCategoryC.Checked;

            //SubCategory
            controllerCasher.SubCategory.View = chkViewSubCategoryC.Checked;
            controllerCasher.SubCategory.EditOrDelete = chkEditSubCategoryC.Checked;
            controllerCasher.SubCategory.Add = chkAddSubCategoryC.Checked;

            //Counter            
            controllerCasher.Counter.EditOrDelete = chkEditCounterC.Checked;
            controllerCasher.Counter.Add = chkAddCounterC.Checked;

            // Transaction
            controllerCasher.Transaction.EditOrDelete = chkDeleteTransactionC.Checked;
            controllerCasher.Transaction.DeleteAndCopy = chkDeleteAndCopyTransactionC.Checked;

            // Transaction Detial
            controllerCasher.TransactionDetail.EditOrDelete = chkDeleteTransactionDetailC.Checked;

            //Reports
            controllerCasher.DailySaleSummary.View = chkDailySaleSummary_C1.Checked;
            controllerCasher.TransactionReport.View = chkTransactionC1.Checked;
            controllerCasher.TransactionSummaryReport.View = chkTransactionSummaryC1.Checked;
            controllerCasher.TransactionDetailReport.View = chkTransactionDetail_C1.Checked;
            controllerCasher.ItemSummaryReport.View = chkItemSummary_C1.Checked;
            controllerCasher.TopBestSellerReport.View = chkTopBestSellerC1.Checked;
            controllerCasher.AverageMonthlyReport.View = chkReportAMRC1.Checked;
            controllerCasher.NetIncomeReport.View = chkNetIncomeC.Checked;
            controllerCasher.TicketDetailReport.View = chkTicketDetailRC.Checked;   /* HMT*/
            controllerCasher.TicketSummaryReport.View = chkTicketSumRC.Checked;    /* HMT*/
            controllerCasher.Ticket.Reprint = chkReprintTicketC.Checked;

            controllerCasher.Save(3);

            #endregion

            MessageBox.Show("Saving Complete");
            this.Dispose();
        }

        private void chkSelectAllSC_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
            {
                #region Brand
                chkViewBrandSC.Checked =
                     chkEditBrandSC.Checked =
                     chkAddBrandSC.Checked =
                #endregion

                #region Main Category
                    chkViewCategorySC.Checked =
                     chkEditCategorySC.Checked =
                     chkAddCategorySC.Checked =
                #endregion

                #region Sub Category
                    chkViewSubCategorySC.Checked =
                     chkEditSubCategorySC.Checked =
                     chkAddSubCategorySC.Checked =
                #endregion
                
                #region Counter
                chkAddCounterSC.Checked =
                           chkEditCounterSC.Checked =
                #endregion
                #region Ticket
                chkReprintTicketC.Checked =
                #endregion

                #region City
                chkAddCitySC.Checked =
                               chkEditCitySC.Checked = chkSelectAllBOSC.Checked;

                #endregion

            }
            else if (tabControl1.SelectedIndex == 1)
            {
                #region Setting
                chkSettingSC.Checked =

                #endregion

                #region Product
                chkViewProductSC.Checked =
                chkEditProductSC.Checked =
                chkAddProductSC.Checked =
                #endregion




                #region Transaction
                chkDeleteTransactionSC.Checked =
                chkDeleteAndCopyTransactionSC.Checked =
                #endregion
                #region Ticket
                chkReprintTicketSC.Checked =
                #endregion
                #region Transaction Detail
                chkDeleteTransactionDetailSC.Checked = chkSelectAllBOPOSSC.Checked;
                #endregion

            }

        }
        private void chkSelectAllC_Click(object sender, EventArgs e)
        {

            if (tabControl1.SelectedIndex == 0 || tabControl1.SelectedIndex == 1)
            {

                #region City
                chkAddCityC.Checked =
                               chkEditCityC.Checked =

                #endregion


                #region Brand
                chkViewBrandC.Checked =
                           chkEditBrandC.Checked =
                           chkAddBrandC.Checked =
                #endregion

                #region Main Category
                chkViewCategoryC.Checked =
                           chkEditCategoryC.Checked =
                           chkAddCategoryC.Checked =
                #endregion

                #region Sub Category
                chkViewSubCategoryC.Checked =
                           chkEditSubCategoryC.Checked =
                           chkAddSubCategoryC.Checked =
                #endregion

                #region Counter
                chkAddCounterC.Checked =
                           chkEditCounterC.Checked = chkSelectAllBOC.Checked;
                #endregion



            }
            else if (tabControl1.SelectedIndex == 2 || tabControl1.SelectedIndex == 3)
            {
                #region Setting
                chkSettingC.Checked =

                #endregion

                #region Product
 chkViewProductC.Checked =
                chkEditProductC.Checked =
                chkAddProductC.Checked =
                #endregion

                #region Transaction
 chkDeleteTransactionC.Checked =
                chkDeleteAndCopyTransactionC.Checked =
                #endregion

                #region Transaction Detail
 chkDeleteTransactionDetailC.Checked = chkSelectAllBOPOSC.Checked;
                #endregion


            }






        }

        private void chkSelectAllReportSC_Click(object sender, EventArgs e)
        {
            // Reports
            chkDailySaleSummary_SC1.Checked =
            chkTransactionSC1.Checked =
            chkTransactionSummarySC1.Checked =
            chkTransactionDetail_SC1.Checked =
            chkItemSummary_SC1.Checked =
            chkTopBestSellerSC1.Checked =
            chkReportAMRSC1.Checked =
            chkNetIncomeSC.Checked =
            chkTicketDetailRSC.Checked =
            chkTicketSumRSC.Checked = chkSelectAllReportSC.Checked;

        }

        private void chkSelectAllReportC_Click(object sender, EventArgs e)
        {
            // Reports
            chkDailySaleSummary_C1.Checked =
            chkTransactionC1.Checked =
            chkTransactionSummaryC1.Checked =
            chkTransactionDetail_C1.Checked =
            chkItemSummary_C1.Checked =
            chkTopBestSellerC1.Checked =
            chkReportAMRC1.Checked =
            chkNetIncomeC.Checked =
            chkTicketDetailRSC.Checked =
            chkTicketSumRSC.Checked = chkSelectAllReportC.Checked;
        }
        #endregion

        #region Functions
        #region Operation Pannel
        public void IsAllCheckedOperation(int RoleId)
        {
            RoleManagementController controller = new RoleManagementController();
            var RoleAllowanceList = controller.IsAllAllowedForOperation(RoleId);
            bool IsAllTrue = RoleAllowanceList.All(x => x == true);
            bool IsAtLeastOneCheck = RoleAllowanceList.Any(x => x == true);
            // SC
            if (RoleId == 2)
            {
                chkSelectAllBOSC.Checked = IsAllTrue;
                chkSelectAllBOPOSSC.Checked = IsAllTrue;
                if (!IsAllTrue && IsAtLeastOneCheck)
                {
                    chkSelectAllBOSC.CheckState = CheckState.Indeterminate;
                }
            }
            // C
            else if (RoleId == 3)
            {
                chkSelectAllBOC.Checked = IsAllTrue;
                chkSelectAllBOPOSC.Checked = IsAllTrue;
                if (!IsAllTrue && IsAtLeastOneCheck)
                {
                    chkSelectAllBOC.CheckState = CheckState.Indeterminate;
                }
            }
        }

        //private void checkbox_ClickSC(object sender, EventArgs eventArgs)
        //{
        //    var controlList = this.tbLayoutBOTran.Controls.Cast<Control>().Where(c => c.GetType() == typeof(CheckBox) && c.Name.Contains("SC") && c != chkSelectAllSC).ToList();
        //    controlList.AddRange(this.tbLayOutBOSetUp.Controls.Cast<Control>().Where(c => c.GetType() == typeof(CheckBox) && !c.Name.Contains("SC")).ToList());
        //    controlList.AddRange(this.tbLayOutBOSetUp1.Controls.Cast<Control>().Where(c => c.GetType() == typeof(CheckBox) && !c.Name.Contains("SC")).ToList());
        //    controlList.AddRange(this.tbLayoutBOTran.Controls.Cast<Control>().Where(c => c.GetType() == typeof(CheckBox) && !c.Name.Contains("SC")).ToList());
        //    controlList.AddRange(this.tbLayoutBOTran1.Controls.Cast<Control>().Where(c => c.GetType() == typeof(CheckBox) && !c.Name.Contains("SC")).ToList());
        //    controlList.AddRange(this.tbLayoutBOPOS.Controls.Cast<Control>().Where(c => c.GetType() == typeof(CheckBox) && !c.Name.Contains("SC")).ToList());
        //    controlList.AddRange(this.tbLayoutBOPOS1.Controls.Cast<Control>().Where(c => c.GetType() == typeof(CheckBox) && !c.Name.Contains("SC")).ToList());
        //    controlList.AddRange(this.tbLayoutBOPOSReport.Controls.Cast<Control>().Where(c => c.GetType() == typeof(CheckBox) && !c.Name.Contains("SC")).ToList());
        //    controlList.AddRange(this.tbLayoutBOPOSReport1.Controls.Cast<Control>().Where(c => c.GetType() == typeof(CheckBox) && !c.Name.Contains("SC")).ToList());
        //    var checkboxList = controlList.Select(c => c as CheckBox).ToList();


        //    bool IsAllChecked = checkboxList.All(c => c.Checked);
        //    bool IsAtLeastOneChecked = checkboxList.Any(c => c.Checked);

        //    chkSelectAllSC.Checked = IsAllChecked;
        //    if (!IsAllChecked && IsAtLeastOneChecked)
        //    {
        //        chkSelectAllSC.CheckState = CheckState.Indeterminate;
        //    }
        //    if (IsAllChecked)
        //    {
        //        chkSelectAllSC.CheckState = CheckState.Checked;
        //    }
        //}

        //private void checkbox_ClickC(object sender, EventArgs eventArgs)
        //{
        //    var controlList = this.tbLayoutBOTran.Controls.Cast<Control>().Where(c => c.GetType() == typeof(CheckBox) && !c.Name.Contains("C") && c != chkSelectAllC).ToList();
        //    controlList.AddRange(this.tbLayOutBOSetUp.Controls.Cast<Control>().Where(c => c.GetType() == typeof(CheckBox) && !c.Name.Contains("C")).ToList());
        //    controlList.AddRange(this.tbLayOutBOSetUp1.Controls.Cast<Control>().Where(c => c.GetType() == typeof(CheckBox) && !c.Name.Contains("C")).ToList());
        //    controlList.AddRange(this.tbLayoutBOTran.Controls.Cast<Control>().Where(c => c.GetType() == typeof(CheckBox) && !c.Name.Contains("C")).ToList());
        //    controlList.AddRange(this.tbLayoutBOTran1.Controls.Cast<Control>().Where(c => c.GetType() == typeof(CheckBox) && !c.Name.Contains("C")).ToList());
        //    controlList.AddRange(this.tbLayoutBOPOS.Controls.Cast<Control>().Where(c => c.GetType() == typeof(CheckBox) && !c.Name.Contains("C")).ToList());
        //    controlList.AddRange(this.tbLayoutBOPOS1.Controls.Cast<Control>().Where(c => c.GetType() == typeof(CheckBox) && !c.Name.Contains("C")).ToList());
        //    controlList.AddRange(this.tbLayoutBOPOSReport.Controls.Cast<Control>().Where(c => c.GetType() == typeof(CheckBox) && !c.Name.Contains("C")).ToList());
        //    controlList.AddRange(this.tbLayoutBOPOSReport1.Controls.Cast<Control>().Where(c => c.GetType() == typeof(CheckBox) && !c.Name.Contains("C")).ToList());

        //    var checkboxList = controlList.Select(c => c as CheckBox).ToList();


        //    bool IsAllChecked = checkboxList.All(c => c.Checked);
        //    bool IsAtLeastOneChecked = checkboxList.Any(c => c.Checked);

        //    chkSelectAllC.Checked = IsAllChecked;
        //    if (!IsAllChecked && IsAtLeastOneChecked)
        //    {
        //        chkSelectAllC.CheckState = CheckState.Indeterminate;
        //    }
        //    if (IsAllChecked)
        //    {
        //        chkSelectAllC.CheckState = CheckState.Checked;
        //    }
        //}
        #endregion

        #region Report Pannel
        public void IsAllCheckedReport(int RoleId)
        {
            RoleManagementController controller = new RoleManagementController();
            var RoleAllowanceList = controller.IsAllAllowedForReport(RoleId);
            bool IsAllTrue = RoleAllowanceList.All(x => x == true);
            bool IsAtLeastOneCheck = RoleAllowanceList.Any(x => x == true);
            // SC
            if (RoleId == 2)
            {
                chkSelectAllReportSC.Checked = IsAllTrue;
                if (!IsAllTrue && IsAtLeastOneCheck)
                {
                    chkSelectAllReportSC.CheckState = CheckState.Indeterminate;
                }
            }
            // C
            else if (RoleId == 3)
            {
                chkSelectAllReportC.Checked = IsAllTrue;
                if (!IsAllTrue && IsAtLeastOneCheck)
                {
                    chkSelectAllReportC.CheckState = CheckState.Indeterminate;
                }
            }
        }

        private void checkboxReport_ClickSC(object sender, EventArgs eventArgs)
        {
            var controlList = this.tbLayoutBOPOSReport.Controls.Cast<Control>().Where(c => c.GetType() == typeof(CheckBox) && c.Name.Contains("SC")).ToList();
            // controlList.AddRange(this.tableLayoutPanel5.Controls.Cast<Control>().Where(c => c.GetType() == typeof(CheckBox) && c.Name.Contains("SC")).ToList());
            var checkboxList = controlList.Select(c => c as CheckBox).ToList();


            bool IsAllChecked = checkboxList.All(c => c.Checked);
            bool IsAtLeastOneChecked = checkboxList.Any(c => c.Checked);

            chkSelectAllReportSC.Checked = IsAllChecked;
            if (!IsAllChecked && IsAtLeastOneChecked)
            {
                chkSelectAllReportSC.CheckState = CheckState.Indeterminate;
            }
            if (IsAllChecked)
            {
                chkSelectAllReportSC.CheckState = CheckState.Checked;
            }
        }

        private void checkboxReport_ClickC(object sender, EventArgs eventArgs)
        {
            var controlList = this.tbLayoutBOPOSReport.Controls.Cast<Control>().Where(c => c.GetType() == typeof(CheckBox) && !c.Name.Contains("SC")).ToList();
            // controlList.AddRange(this.tableLayoutPanel5.Controls.Cast<Control>().Where(c => c.GetType() == typeof(CheckBox) && !c.Name.Contains("SC")).ToList());
            var checkboxList = controlList.Select(c => c as CheckBox).ToList();


            bool IsAllChecked = checkboxList.All(c => c.Checked);
            bool IsAtLeastOneChecked = checkboxList.Any(c => c.Checked);

            chkSelectAllReportC.Checked = IsAllChecked;
            if (!IsAllChecked && IsAtLeastOneChecked)
            {
                chkSelectAllReportC.CheckState = CheckState.Indeterminate;
            }
            if (IsAllChecked)
            {
                chkSelectAllReportC.CheckState = CheckState.Checked;
            }
        }






        #endregion

        #endregion

      
    }
}
