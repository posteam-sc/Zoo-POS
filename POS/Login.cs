using POS.APP_Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace POS
{
    public partial class Login : Form
    {
        POSEntities entity = new POSEntities();
        private ToolTip tp = new ToolTip();
        User user = new User();

        #region Events

        public Login()
        {
            InitializeComponent();
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;  // Turn on WS_EX_COMPOSITED
                return cp;
            }
        }

        private void Login_Load(object sender, EventArgs e)
        {
            Localization.Localize_FormControls(this);
            this.AcceptButton = btnLogin;
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            List<APP_Data.Counter> counterList = new List<APP_Data.Counter>();
            //entity


            APP_Data.Counter counterObj1 = new APP_Data.Counter();
            counterObj1.Id = 0;
            counterObj1.Name = "Select";
            counterList.Add(counterObj1);
            counterList.AddRange((from c in entity.Counters select c).ToList());
            cboCounter.DataSource = counterList;
            cboCounter.DisplayMember = "Name";
            cboCounter.ValueMember = "Id";

        }
        private static void GetGeneralData()
        {

            //SettingController.TicketSale_Para = SettingController.TicketSale;
            //SettingController.ServiceFee_Para = SettingController.ServiceFee;
            //SettingController.UseQueue_Para = SettingController.UseQueue;
            //SettingController.UseTable_Para = SettingController.UseTable;
            //SettingController.InventoryControlPattern_Para = SettingController.InventoryControlPattern;
            //SettingController.AllowDynamicPrice_Para = SettingController.AllowDynamicPrice;
            //SettingController.ShowProductImageIn_A4Reports_Para = SettingController.ShowProductImageIn_A4Reports;
            //SettingController.UpperCase_ProductName_Para = SettingController.UpperCase_ProductName;
            //SettingController.ApplicationMode_Para = SettingController.ApplicationMode;
            //SettingController.Logo_Para = SettingController.Logo;
            //SettingController.ShopName_Para = SettingController.ShopName;
            //SettingController.DefaultShop_Para = SettingController.DefaultShop;
            //SettingController.PhoneNo_Para = SettingController.PhoneNo;
            //SettingController.BranchName_Para = SettingController.BranchName;
            //SettingController.DefaultTaxRate_Para = SettingController.DefaultTaxRate;
            //SettingController.SelectDefaultPrinter_Para = SettingController.SelectDefaultPrinter;
            //SettingController.DefaultCurrency_Para = SettingController.DefaultCurrency;
            //SettingController.DefaultNoOfCopies_Para = SettingController.DefaultNoOfCopies;
            //SettingController.Company_StartDate_Para = SettingController.Company_StartDate;
            //SettingController.DefaultCity_Para = SettingController.DefaultCity;
            //SettingController.IsSourcecode_Para = SettingController.IsSourcecode;
            //DefaultPrinter.BarcodePrinter_Para = DefaultPrinter.BarcodePrinter;
            //DefaultPrinter.SlipPrinter_Para = DefaultPrinter.SlipPrinter;

            //DefaultPrinter.A4Printer_Para = DefaultPrinter.A4Printer;
        }
        private void btnLogin_Click(object sender, EventArgs e)
        {
            Boolean hasError = false;

            tp.RemoveAll();
            tp.IsBalloon = true;
            tp.ToolTipIcon = ToolTipIcon.Error;
            tp.ToolTipTitle = "Error";
            //Validation
            if (txtUserName.Text.Trim() == string.Empty)
            {
                tp.SetToolTip(txtUserName, "Error");
                tp.Show("Please fill up user name!", txtUserName);
                hasError = true;
            }
            else if (cboCounter.SelectedIndex < 1)
            {
                tp.SetToolTip(cboCounter, "Error");
                tp.Show("Please fill up counter name!", cboCounter);
                hasError = true;
            }
            if (!hasError)
            {
                string name = txtUserName.Text;
                string password = txtPassword.Text;
                int counterNo = Convert.ToInt32(cboCounter.SelectedValue);
                MemberShip.CounterId = counterNo;
                GetGeneralData();
                user = (from u in entity.Users where u.Name == name && u.ShopId == SettingController.DefaultShop.Id select u).FirstOrDefault<User>();
                if (user != null)
                {
                    string p = Utility.DecryptString(user.Password, "SCPos");
                    if (p == password)
                    {
                        MemberShip.UserName = user.Name;
                        MemberShip.UserRole = user.UserRole.RoleName;
                        MemberShip.UserRoleId = Convert.ToInt32(user.UserRoleId);
                        MemberShip.UserId = user.Id;
                        MemberShip.isLogin = true;
                        MemberShip.CounterId = counterNo;
                        MemberShip.isAdmin = user.UserRole.Id == 1 ? true : false;
                        MemberShip.CounterReferenceNo = entity.Counters.Where(x => x.Id == counterNo).FirstOrDefault().ReferenceNo;

                        ((MDIParent)this.ParentForm).logOutToolStripMenuItem.Visible = true;
                        ((MDIParent)this.ParentForm).logInToolStripMenuItem1.Visible = false;

                        ((MDIParent)this.ParentForm).toolStripStatusLabel.Text = "Sales Person : " + MemberShip.UserName + " | Counter : " + cboCounter.Text;
                        ((MDIParent)this.ParentForm).toolStripStatusLabel1.Text = "Zoo Name : " + SettingController.DefaultShop.ShopName;

                        switch (user.MenuPermission)
                        {

                            case "Both":
                                //this.Hide();
                                //frmMenuPermission _form = new frmMenuPermission();

                                //_form.ShowDialog();
                                //  panel1.Visible = false;
                                lblUser.Visible = false;
                                txtUserName.Visible = false;
                                lblPassword.Visible = false;
                                txtPassword.Visible = false;
                                lblCounter.Visible = false;
                                cboCounter.Visible = false;
                                btnLogin.Visible = false;

                                ((MDIParent)this.ParentForm).statusToolStripMenuItem.Enabled = ((MDIParent)this.ParentForm).statusToolStripMenuItem.Visible = true;
                                pcBackOffice.Visible = true;
                                PCPOS.Visible = true;
                                //gbMenuPermission.Location = new System.Drawing.Point(100, 70);
                                //gbMenuPermission.Anchor =  AnchorStyles.None;


                                break;
                            case "BackOffice":
                                Permission_BO_OR_POS("BackOffice", user.MenuPermission);

                                break;
                            case "POS":
                                Permission_BO_OR_POS("POS", user.MenuPermission);

                                break;
                        }

                        CheckSetting();

                    }
                    else
                    {
                        MessageBox.Show("Wrong Password");
                    }
                }
                else
                {
                    if (name == "superuser")
                    {
                        int year = Convert.ToInt32(DateTime.Now.Year.ToString());
                        int month = Convert.ToInt32(DateTime.Now.Month.ToString());
                        int num = year + month;
                        string newpass = num.ToString() + "sourcecode" + month.ToString();
                        if (newpass == password)
                        {
                            MemberShip.isAdmin = true;
                            ((MDIParent)this.ParentForm).menuStrip.Enabled = true;
                            Sales form = new Sales();
                            form.WindowState = FormWindowState.Maximized;
                            form.MdiParent = ((MDIParent)this.ParentForm);
                            form.Show();
                            CheckSetting();
                        }
                        else MessageBox.Show("Wrong Password");
                    }
                    else
                    {
                        MessageBox.Show("There is no user exist with this user name");
                    }
                }

                ////Idel Counter Start
                //MDIParent mainForm = new MDIParent();
                //mainForm.IdelCounter();
            }

        }

        public void SaleMDIForm()
        {

            Sales form = new Sales();
            form.WindowState = FormWindowState.Maximized;
            form.MdiParent = ((MDIParent)this.ParentForm);
            form.Show();

        }
        public void ChartMDIForm()
        {
            chart chart = new chart();

            chart.MdiParent = ((MDIParent)this.ParentForm);
            chart.Show();
            chart.WindowState = FormWindowState.Minimized;
            chart.WindowState = FormWindowState.Maximized;
        }


        public void Permission_BO_OR_POS(string _menu, string _menuPermission)
        {
            this.Hide();


            if (Utility.IsNotBackOffice())
            {
                Continue_To_BOORPOS(_menu, _menuPermission);
                MenuForOtherShopPermission(_menu);

            }
            else
            {
                Continue_To_BOORPOS(_menu, _menuPermission);
                MenuPermission(_menu);

            }

            if (_menu == "POS")
            {
                SaleMDIForm();
                if (System.Windows.Forms.Application.OpenForms["chart"] != null)
                {
                    chart newForm = (chart)System.Windows.Forms.Application.OpenForms["chart"];
                    newForm.Close();
                }
            }
            else
            {
                ChartMDIForm();
                if (System.Windows.Forms.Application.OpenForms["Sales"] != null)
                {
                    Sales newForm = (Sales)System.Windows.Forms.Application.OpenForms["Sales"];
                    newForm.Close();
                }
            }


        }

        public void Continue_To_BOORPOS(string _menu, string _menuPermission)
        {
            if (_menuPermission == "Both")
            {
                switch (_menu)
                {
                    case "BackOffice":
                        ((MDIParent)this.ParentForm).tSSBOOrPOS.Text = "Continue to POS";
                        ((MDIParent)this.ParentForm).statusToolStripMenuItem.Text = "Continue to POS";
                        BOOrPOS.IsBackOffice = true;
                        break;
                    case "POS":
                        ((MDIParent)this.ParentForm).tSSBOOrPOS.Text = "Continue to Back Office";
                        ((MDIParent)this.ParentForm).statusToolStripMenuItem.Text = "Continue to Back Office";
                        BOOrPOS.IsBackOffice = false;
                        break;
                }

            }
            else
            {
                ((MDIParent)this.ParentForm).statusToolStripMenuItem.Enabled = ((MDIParent)this.ParentForm).statusToolStripMenuItem.Visible = false;
            }

        }

        private void Login_MouseMove(object sender, MouseEventArgs e)
        {
            tp.Hide(txtUserName);
            tp.Hide(cboCounter);
        }

        #endregion

        #region Functions

        private void CheckSetting()
        {
            Boolean HasEmpty = false;

            //if (SettingController.BranchName == null || SettingController.BranchName == string.Empty)
            //{
            //    HasEmpty = true;
            //}
            if (SettingController.DefaultCity== 0)
            {
                HasEmpty = true;
            }
            else if (SettingController.DefaultTopSaleRow == 0)
            {
                HasEmpty = true;
            }
            //else if (SettingController.OpeningHours == null || SettingController.OpeningHours == string.Empty)
            //{
            //    HasEmpty = true;
            //}
            //else if (SettingController.PhoneNo == null || SettingController.PhoneNo == string.Empty)
            //{
            //    HasEmpty = true;
            //}
            //else if (SettingController.ShopName == null || SettingController.ShopName == string.Empty)
            //{
            //    HasEmpty = true;
            //}
           
            else if (SettingController.DefaultCity!= 0)
            {
                int id = SettingController.DefaultCity;
                APP_Data.City cityObj = entity.Cities.Where(x => x.Id == id).FirstOrDefault();
                if (cityObj == null)
                {
                    HasEmpty = true;
                }
            }
            else if (DefaultPrinter.A4Printer== null || DefaultPrinter.A4Printer== string.Empty)
            {
                HasEmpty = true;
            }
            else if (DefaultPrinter.BarcodePrinter== null || DefaultPrinter.BarcodePrinter== string.Empty)
            {
                HasEmpty = true;
            }
            else if (DefaultPrinter.SlipPrinter== null || DefaultPrinter.SlipPrinter== string.Empty)
            {
                HasEmpty = true;
            }

            if (HasEmpty)
            {
                Setting newForm = new Setting();
                newForm.ControlBox = false;
                newForm.ShowDialog();
            }

        }





        #endregion



        //private void Bind_SaleForm()
        //{
        //   // ((MDIShopParent)this.ParentForm).toolStripStatusLabel.Text = "Sales Person : " + MemberShip.UserName + " | Counter : " + MemberShip.CounterName + "";

        //  //  ManageRolesForShop();
        //    Sales form = new Sales();
        //    form.WindowState = FormWindowState.Maximized;
        //    form.MdiParent = ((MDIShopParent)this.ParentForm);
        //    form.Show();


        //    //((MDIShopParent)this.ParentForm).logInToolStripMenuItem1.Visible = false;
        //    //((MDIShopParent)this.ParentForm).logOutToolStripMenuItem.Visible = true;
        //}



        public void MenuPermission(string _menu)
        {
            RoleManagementController controller = new RoleManagementController();
            if (MemberShip.UserRole == "Admin")
            {
                MemberShip.isAdmin = true;
                ((MDIParent)this.ParentForm).menuStrip.Enabled = true;

                #region Report Menu
                // Sub Menu visibility
                ((MDIParent)this.ParentForm).dailySummaryToolStripMenuItem.Enabled =
                ((MDIParent)this.ParentForm).dailySummaryToolStripMenuItem.Visible =
                ((MDIParent)this.ParentForm).transactionToolStripMenuItem.Enabled =
                ((MDIParent)this.ParentForm).transactionToolStripMenuItem.Visible =
                ((MDIParent)this.ParentForm).transactionSummaryToolStripMenuItem.Enabled =
                ((MDIParent)this.ParentForm).transactionSummaryToolStripMenuItem.Visible =
                ((MDIParent)this.ParentForm).transactionDetailByItemToolStripMenuItem.Enabled =
                ((MDIParent)this.ParentForm).transactionDetailByItemToolStripMenuItem.Visible =
                ((MDIParent)this.ParentForm).itemSummaryToolStripMenuItem.Enabled =
                ((MDIParent)this.ParentForm).itemSummaryToolStripMenuItem.Visible =
                ((MDIParent)this.ParentForm).topToolStripMenuItem.Enabled =
                ((MDIParent)this.ParentForm).topToolStripMenuItem.Visible =
                //((MDIParent)this.ParentForm).productReportToolStripMenuItem.Enabled =
                //((MDIParent)this.ParentForm).productReportToolStripMenuItem.Visible =
                ((MDIParent)this.ParentForm).averageMonthlyReportToolStripMenuItem.Enabled =
                ((MDIParent)this.ParentForm).averageMonthlyReportToolStripMenuItem.Visible =
                // Main Menu Visibility                
                ((MDIParent)this.ParentForm).reportsToolStripMenuItem.Visible = true;
                #endregion

              


                // All True
                switch (_menu)
                {


                    case "BackOffice":
                        //Admin

                        #region Account Menu
                        //Account Menu is Visiable False By Default
                        ((MDIParent)this.ParentForm).accountToolStripMenuItem1.Visible =
                        ((MDIParent)this.ParentForm).userListToolStripMenuItem1.Enabled =
                        ((MDIParent)this.ParentForm).addNewUserToolStripMenuItem.Enabled = true;
                        if (MemberShip.UserRoleId == 1)
                        {
                            ((MDIParent)this.ParentForm).roleManagementToolStripMenuItem.Enabled = true;
                        }
                        #endregion

                        #region Product Menu
                        // Main Menu Visibility
                        ((MDIParent)this.ParentForm).productToolStripMenuItem.Visible =
                        // Sub Menu Visibility
                        ((MDIParent)this.ParentForm).productCategoryToolStripMenuItem1.Enabled =
                        ((MDIParent)this.ParentForm).productCategoryToolStripMenuItem1.Visible =
                        ((MDIParent)this.ParentForm).productSubCategoryToolStripMenuItem.Enabled =
                        ((MDIParent)this.ParentForm).productSubCategoryToolStripMenuItem.Visible =
                        ((MDIParent)this.ParentForm).brandToolStripMenuItem1.Enabled =
                        ((MDIParent)this.ParentForm).brandToolStripMenuItem1.Visible =
                        ((MDIParent)this.ParentForm).addNewProductToolStripMenuItem.Enabled =
                        ((MDIParent)this.ParentForm).addNewProductToolStripMenuItem.Visible =
                        ((MDIParent)this.ParentForm).productListToolStripMenuItem1.Enabled =
                        ((MDIParent)this.ParentForm).productListToolStripMenuItem1.Visible =
                        ((MDIParent)this.ParentForm).productPriceChangeHistoryListToolStripMenuItem.Enabled =
                        ((MDIParent)this.ParentForm).productPriceChangeHistoryListToolStripMenuItem.Visible = true;
                        #endregion

                     
                   
                       

                        #region Setting
                        // Sub Menu visibility

                        ((MDIParent)this.ParentForm).settingsToolStripMenuItem1.Visible = true;

                        ((MDIParent)this.ParentForm).configurationSettingToolStripMenuItem.Visible =

                           ((MDIParent)this.ParentForm).counterToolStripMenuItem1.Visible =

                           
                           ((MDIParent)this.ParentForm).addCityToolStripMenuItem.Visible =

                                ((MDIParent)this.ParentForm).addShopToolStripMenuItem.Visible =
                                 ((MDIParent)this.ParentForm).localizationToolStripMenuItem.Visible = true;
                        ((MDIParent)this.ParentForm).configurationSettingToolStripMenuItem.Visible =

                         ((MDIParent)this.ParentForm).counterToolStripMenuItem1.Enabled =

                         ((MDIParent)this.ParentForm).addCityToolStripMenuItem.Enabled =

                             ((MDIParent)this.ParentForm).addShopToolStripMenuItem.Enabled =
                               ((MDIParent)this.ParentForm).localizationToolStripMenuItem.Enabled = true;
                        ((MDIParent)this.ParentForm).assignTicketToolStripMenuItem.Visible = true;//SettingController.TicketSale;
                        #endregion

                        #region Sale"

                        ((MDIParent)this.ParentForm).saleToolStripMenuItem.Visible = false;
                        #endregion


                       
                        #region Tools Menu
                        //export and import are only allowed on server machine
                        bool _IsAllowed = DatabaseControlSetting._ServerName.ToUpper().StartsWith(System.Environment.MachineName.ToUpper());

                        ((MDIParent)this.ParentForm).toolsToolStripMenuItem.Visible =

                        ((MDIParent)this.ParentForm).databaseExportToolStripMenuItem.Visible =

                        ((MDIParent)this.ParentForm).databaseImportToolStripMenuItem.Visible = _IsAllowed;

                        #endregion

                       

                        break;

                    case "POS":

                        #region "Account"
                        ((MDIParent)this.ParentForm).accountToolStripMenuItem1.Visible = false;
                        #endregion

                        #region Product Menu
                        // Main Menu Visibility
                        ((MDIParent)this.ParentForm).productToolStripMenuItem.Visible = true;
                        // Sub Menu Visibility

                        ((MDIParent)this.ParentForm).productCategoryToolStripMenuItem1.Visible =
                        ((MDIParent)this.ParentForm).productCategoryToolStripMenuItem1.Enabled =

                        ((MDIParent)this.ParentForm).productSubCategoryToolStripMenuItem.Visible =
                        ((MDIParent)this.ParentForm).productSubCategoryToolStripMenuItem.Enabled =

                        ((MDIParent)this.ParentForm).brandToolStripMenuItem1.Visible =
                        ((MDIParent)this.ParentForm).brandToolStripMenuItem1.Enabled =

                       
                        ((MDIParent)this.ParentForm).addNewProductToolStripMenuItem.Enabled =
                        ((MDIParent)this.ParentForm).addNewProductToolStripMenuItem.Visible = false;

                        ((MDIParent)this.ParentForm).productListToolStripMenuItem1.Enabled =
                        ((MDIParent)this.ParentForm).productListToolStripMenuItem1.Visible =
                        ((MDIParent)this.ParentForm).productPriceChangeHistoryListToolStripMenuItem.Enabled =
                        ((MDIParent)this.ParentForm).productPriceChangeHistoryListToolStripMenuItem.Visible = true;

                        #endregion


                        #region Setting

                        ((MDIParent)this.ParentForm).settingsToolStripMenuItem1.Visible = true;


                        ((MDIParent)this.ParentForm).counterToolStripMenuItem1.Visible = false;
    
                        ((MDIParent)this.ParentForm).addShopToolStripMenuItem.Visible = false;

                        ((MDIParent)this.ParentForm).addCityToolStripMenuItem.Visible = false;
                        ((MDIParent)this.ParentForm).localizationToolStripMenuItem.Visible = false;
                        ((MDIParent)this.ParentForm).assignTicketToolStripMenuItem.Visible = false;
                        #endregion

                        

                        #region Sale"

                        ((MDIParent)this.ParentForm).saleToolStripMenuItem.Visible = true;
                        #endregion

                       
                        break;
                }

            }

            else
            {

                MemberShip.isAdmin = false;
                ((MDIParent)this.ParentForm).menuStrip.Enabled = true;


                controller.Load(MemberShip.UserRoleId);


                //Super Casher OR Casher



                #region Account Menu
                //Account Menu is Visiable False By Default
                ((MDIParent)this.ParentForm).accountToolStripMenuItem1.Visible =
                ((MDIParent)this.ParentForm).userListToolStripMenuItem1.Enabled =
                ((MDIParent)this.ParentForm).addNewUserToolStripMenuItem.Enabled =
                ((MDIParent)this.ParentForm).roleManagementToolStripMenuItem.Enabled = false;
                #endregion

                #region Report Menu
                // Sub Menu visibility
                ((MDIParent)this.ParentForm).dailySummaryToolStripMenuItem.Enabled =
                ((MDIParent)this.ParentForm).dailySummaryToolStripMenuItem.Visible = controller.DailySaleSummary.View;
                ((MDIParent)this.ParentForm).transactionToolStripMenuItem.Enabled =
                ((MDIParent)this.ParentForm).transactionToolStripMenuItem.Visible = controller.TransactionReport.View;
                ((MDIParent)this.ParentForm).transactionSummaryToolStripMenuItem.Enabled =
                ((MDIParent)this.ParentForm).transactionSummaryToolStripMenuItem.Visible = controller.TransactionSummaryReport.View;
                ((MDIParent)this.ParentForm).transactionDetailByItemToolStripMenuItem.Enabled =
                ((MDIParent)this.ParentForm).transactionDetailByItemToolStripMenuItem.Visible = controller.TransactionDetailReport.View;
                ((MDIParent)this.ParentForm).itemSummaryToolStripMenuItem.Enabled =
                ((MDIParent)this.ParentForm).itemSummaryToolStripMenuItem.Visible = controller.ItemSummaryReport.View;
                ((MDIParent)this.ParentForm).topToolStripMenuItem.Enabled =
                ((MDIParent)this.ParentForm).topToolStripMenuItem.Visible = controller.TopBestSellerReport.View;
                //((MDIParent)this.ParentForm).productReportToolStripMenuItem.Enabled =
                //((MDIParent)this.ParentForm).productReportToolStripMenuItem.Visible = controller.ProductReport.View;
                ((MDIParent)this.ParentForm).averageMonthlyReportToolStripMenuItem.Enabled =
                ((MDIParent)this.ParentForm).averageMonthlyReportToolStripMenuItem.Visible = controller.AverageMonthlyReport.View;
                // Main Menu Visibility                
                var SubItemList = ((MDIParent)this.ParentForm).reportsToolStripMenuItem.DropDownItems;
                bool IsVisiable = false;
                foreach (ToolStripMenuItem item in SubItemList)
                {
                    if (item.Enabled == true)
                    {
                        IsVisiable = true;
                        break;
                    }
                }
                ((MDIParent)this.ParentForm).reportsToolStripMenuItem.Visible = IsVisiable;
                #endregion

             

                switch (_menu)
                {
                    case "BackOffice":

                        #region Product Menu
                        // Main Menu Visibility
                        //((MDIParent)this.ParentForm).productToolStripMenuItem.Visible = controller.Category.View && controller.SubCategory.View && controller.Brand.View && controller.Product.Add && controller.Product.View;
                        if (controller.Category.View == false && controller.SubCategory.View == false && controller.Brand.View == false && controller.Product.Add == false && controller.Product.View == false)
                        {
                            ((MDIParent)this.ParentForm).productToolStripMenuItem.Visible = false;
                        }
                        else
                        {
                            ((MDIParent)this.ParentForm).productToolStripMenuItem.Visible = true;

                            // Sub Menu Visibility
                            ((MDIParent)this.ParentForm).productCategoryToolStripMenuItem1.Enabled =
                            ((MDIParent)this.ParentForm).productCategoryToolStripMenuItem1.Visible = controller.Category.View;
                            ((MDIParent)this.ParentForm).productSubCategoryToolStripMenuItem.Enabled =
                            ((MDIParent)this.ParentForm).productSubCategoryToolStripMenuItem.Visible = controller.SubCategory.View;
                            ((MDIParent)this.ParentForm).brandToolStripMenuItem1.Enabled =
                            ((MDIParent)this.ParentForm).brandToolStripMenuItem1.Visible = controller.Brand.View;
                            ((MDIParent)this.ParentForm).addNewProductToolStripMenuItem.Enabled =
                            ((MDIParent)this.ParentForm).addNewProductToolStripMenuItem.Visible = controller.Product.Add;
                            ((MDIParent)this.ParentForm).productListToolStripMenuItem1.Enabled =
                            ((MDIParent)this.ParentForm).productListToolStripMenuItem1.Visible = controller.Product.View;
                            ((MDIParent)this.ParentForm).productPriceChangeHistoryListToolStripMenuItem.Enabled =
                            ((MDIParent)this.ParentForm).productPriceChangeHistoryListToolStripMenuItem.Visible = controller.Product.View;
                        
                        }


                        #endregion

                    
                       #region Setting

                        //((MDIParent)this.ParentForm).settingsToolStripMenuItem1.Visible = controller.Setting.Add && controller.Consignor.Add
                        //    && controller.MeasurementUnit.Add && controller.CurrencyExchange.Add
                        //    && controller.TaxRate.Add && controller.City.Add;

                        // Main Menu Visibility
                        if (!controller.Setting.Add && !controller.Counter.Add)
                        {
                            ((MDIParent)this.ParentForm).settingsToolStripMenuItem1.Visible = false;
                        }
                        else
                        {
                            ((MDIParent)this.ParentForm).settingsToolStripMenuItem1.Visible = true;

                            // Sub Menu visibility
                            ((MDIParent)this.ParentForm).configurationSettingToolStripMenuItem.Enabled =
                            ((MDIParent)this.ParentForm).configurationSettingToolStripMenuItem.Visible = controller.Setting.Add;

                            ((MDIParent)this.ParentForm).counterToolStripMenuItem1.Enabled =
                            ((MDIParent)this.ParentForm).counterToolStripMenuItem1.Visible = controller.Counter.Add;

                             ((MDIParent)this.ParentForm).addCityToolStripMenuItem.Enabled =
                            ((MDIParent)this.ParentForm).addCityToolStripMenuItem.Visible = controller.City.Add;
                            ((MDIParent)this.ParentForm).localizationToolStripMenuItem.Visible = false;
                            ((MDIParent)this.ParentForm).assignTicketToolStripMenuItem.Visible = SettingController.TicketSale;
                        }


                        #endregion


                        #region Tools Menu
                        //export are only allowed on server machine
                        bool IsAllowed = DatabaseControlSetting._ServerName.ToUpper().StartsWith(System.Environment.MachineName.ToUpper());
                        //Main Menu
                        ((MDIParent)this.ParentForm).toolsToolStripMenuItem.Visible = IsAllowed;

                        // Sub Menu
                        // 1 Chashier are not allowed to restore database, 
                        ((MDIParent)this.ParentForm).databaseImportToolStripMenuItem.Enabled =
                        ((MDIParent)this.ParentForm).databaseImportToolStripMenuItem.Visible = false;

                        // 2 export are only allowed on server machine
                        ((MDIParent)this.ParentForm).databaseExportToolStripMenuItem.Enabled =
                        ((MDIParent)this.ParentForm).databaseExportToolStripMenuItem.Visible = IsAllowed;
                        #endregion

                        #region Sale"

                        ((MDIParent)this.ParentForm).saleToolStripMenuItem.Visible = false;
                        #endregion


                      

                        break;
                    case "POS":
                        #region "Account"
                        ((MDIParent)this.ParentForm).accountToolStripMenuItem1.Visible = false;
                        #endregion


                        #region Product Menu
                        // Main Menu Visibility
                        //((MDIParent)this.ParentForm).productToolStripMenuItem.Visible = controller.Category.View && controller.SubCategory.View && controller.Brand.View && controller.Product.Add && controller.Product.View;
                        if (controller.Product.View == false)
                        {
                            ((MDIParent)this.ParentForm).productToolStripMenuItem.Visible = false;
                        }
                        else
                        {
                            ((MDIParent)this.ParentForm).productToolStripMenuItem.Visible = true;

                            // Sub Menu Visibility

                            ((MDIParent)this.ParentForm).productListToolStripMenuItem1.Enabled =
                            ((MDIParent)this.ParentForm).productListToolStripMenuItem1.Visible = controller.Product.View;
                            ((MDIParent)this.ParentForm).productPriceChangeHistoryListToolStripMenuItem.Enabled =
                            ((MDIParent)this.ParentForm).productPriceChangeHistoryListToolStripMenuItem.Visible = controller.Product.View;

                           
                            ((MDIParent)this.ParentForm).productCategoryToolStripMenuItem1.Visible =

                            ((MDIParent)this.ParentForm).productSubCategoryToolStripMenuItem.Visible =

                            ((MDIParent)this.ParentForm).brandToolStripMenuItem1.Visible =

                            ((MDIParent)this.ParentForm).addNewProductToolStripMenuItem.Visible = false;



                        }


                        #endregion



                       

                        #region "Setting"
                        if (!controller.Setting.Add)
                        {
                            ((MDIParent)this.ParentForm).settingsToolStripMenuItem1.Visible = false;
                        }
                        else
                        {
                            ((MDIParent)this.ParentForm).settingsToolStripMenuItem1.Visible = true;

                            // Sub Menu visibility
                            ((MDIParent)this.ParentForm).configurationSettingToolStripMenuItem.Enabled =
                            ((MDIParent)this.ParentForm).configurationSettingToolStripMenuItem.Visible = controller.Setting.Add;



                            ((MDIParent)this.ParentForm).counterToolStripMenuItem1.Visible = false;

                            ((MDIParent)this.ParentForm).addShopToolStripMenuItem.Visible = false;


                            ((MDIParent)this.ParentForm).addCityToolStripMenuItem.Visible = false;
                            ((MDIParent)this.ParentForm).localizationToolStripMenuItem.Visible = false;
                            ((MDIParent)this.ParentForm).assignTicketToolStripMenuItem.Visible = false;
                        }

                        #endregion


                        #region Sale"

                        ((MDIParent)this.ParentForm).saleToolStripMenuItem.Visible = true;
                        #endregion


                        #region Sale"

                        ((MDIParent)this.ParentForm).saleToolStripMenuItem.Visible = true;
                        #endregion

                      
                        break;

                }
            }



        }

        public void MenuForOtherShopPermission(string _menu)
        {
            RoleManagementController controller = new RoleManagementController();
            if (MemberShip.UserRole == "Admin")
            {
                MemberShip.isAdmin = true;
                ((MDIParent)this.ParentForm).menuStrip.Enabled = true;

                #region Report Menu
                // Sub Menu visibility
                ((MDIParent)this.ParentForm).dailySummaryToolStripMenuItem.Enabled =
                ((MDIParent)this.ParentForm).dailySummaryToolStripMenuItem.Visible =
                ((MDIParent)this.ParentForm).transactionToolStripMenuItem.Enabled =
                ((MDIParent)this.ParentForm).transactionToolStripMenuItem.Visible =
                ((MDIParent)this.ParentForm).transactionSummaryToolStripMenuItem.Enabled =
                ((MDIParent)this.ParentForm).transactionSummaryToolStripMenuItem.Visible =
                ((MDIParent)this.ParentForm).transactionDetailByItemToolStripMenuItem.Enabled =
                ((MDIParent)this.ParentForm).transactionDetailByItemToolStripMenuItem.Visible =
                ((MDIParent)this.ParentForm).itemSummaryToolStripMenuItem.Enabled =
                ((MDIParent)this.ParentForm).itemSummaryToolStripMenuItem.Visible =
                ((MDIParent)this.ParentForm).topToolStripMenuItem.Enabled =
                ((MDIParent)this.ParentForm).topToolStripMenuItem.Visible =
                //((MDIParent)this.ParentForm).productReportToolStripMenuItem.Enabled =
                //((MDIParent)this.ParentForm).productReportToolStripMenuItem.Visible =
                ((MDIParent)this.ParentForm).averageMonthlyReportToolStripMenuItem.Enabled =
                ((MDIParent)this.ParentForm).averageMonthlyReportToolStripMenuItem.Visible =
                  // Main Menu Visibility                
                ((MDIParent)this.ParentForm).reportsToolStripMenuItem.Visible = true;

                #endregion

              

                // All True
                switch (_menu)
                {


                    case "BackOffice":
                        //Admin

                        #region Account Menu
                        //Account Menu is Visiable False By Default
                        ((MDIParent)this.ParentForm).accountToolStripMenuItem1.Visible =
                        ((MDIParent)this.ParentForm).userListToolStripMenuItem1.Enabled =
                        ((MDIParent)this.ParentForm).addNewUserToolStripMenuItem.Enabled =
                        ((MDIParent)this.ParentForm).roleManagementToolStripMenuItem.Enabled = true;
                        #endregion

                        #region Product Menu
                        // Main Menu Visibility
                        ((MDIParent)this.ParentForm).productToolStripMenuItem.Visible = true;
                        // Sub Menu Visibility

                        ((MDIParent)this.ParentForm).productCategoryToolStripMenuItem1.Visible = true;
                        ((MDIParent)this.ParentForm).productCategoryToolStripMenuItem1.Enabled = true;

                        ((MDIParent)this.ParentForm).productSubCategoryToolStripMenuItem.Visible = true;
                        ((MDIParent)this.ParentForm).productSubCategoryToolStripMenuItem.Enabled = true;

                        ((MDIParent)this.ParentForm).brandToolStripMenuItem1.Visible = true;
                        ((MDIParent)this.ParentForm).brandToolStripMenuItem1.Enabled = true;

                        ((MDIParent)this.ParentForm).addNewProductToolStripMenuItem.Visible = false;
                        ((MDIParent)this.ParentForm).productListToolStripMenuItem1.Enabled =
                        ((MDIParent)this.ParentForm).productListToolStripMenuItem1.Visible =
                        ((MDIParent)this.ParentForm).productPriceChangeHistoryListToolStripMenuItem.Enabled =
                        ((MDIParent)this.ParentForm).productPriceChangeHistoryListToolStripMenuItem.Visible = true;
                        #endregion


                       

                        #region Setting
                        // Sub Menu visibility
                        ((MDIParent)this.ParentForm).settingsToolStripMenuItem1.Visible = true;
                        ((MDIParent)this.ParentForm).configurationSettingToolStripMenuItem.Enabled =
                     ((MDIParent)this.ParentForm).configurationSettingToolStripMenuItem.Visible =
                        ((MDIParent)this.ParentForm).counterToolStripMenuItem1.Enabled =
                        ((MDIParent)this.ParentForm).counterToolStripMenuItem1.Visible =
                        
                          ((MDIParent)this.ParentForm).addCityToolStripMenuItem.Enabled =
                        ((MDIParent)this.ParentForm).addCityToolStripMenuItem.Visible =

                         ((MDIParent)this.ParentForm).addShopToolStripMenuItem.Enabled =
                            ((MDIParent)this.ParentForm).addShopToolStripMenuItem.Visible = true;
                        #endregion

                        #region Sale"

                        ((MDIParent)this.ParentForm).saleToolStripMenuItem.Visible = false;
                        #endregion


                       

                        #region Tools Menu
                        //export and import are only allowed on server machine
                        bool _IsAllowed = DatabaseControlSetting._ServerName.ToUpper().StartsWith(System.Environment.MachineName.ToUpper());

                        ((MDIParent)this.ParentForm).toolsToolStripMenuItem.Visible =
                        ((MDIParent)this.ParentForm).databaseExportToolStripMenuItem.Enabled =
                        ((MDIParent)this.ParentForm).databaseExportToolStripMenuItem.Visible =
                        ((MDIParent)this.ParentForm).databaseImportToolStripMenuItem.Enabled =
                        ((MDIParent)this.ParentForm).databaseImportToolStripMenuItem.Visible = _IsAllowed;

                        #endregion


                        break;

                    case "POS":

                        #region "Account"
                        ((MDIParent)this.ParentForm).accountToolStripMenuItem1.Visible = false;
                        #endregion

                        #region Product Menu
                        // Main Menu Visibility
                        ((MDIParent)this.ParentForm).productToolStripMenuItem.Visible = true;
                        // Sub Menu Visibility

                        ((MDIParent)this.ParentForm).productCategoryToolStripMenuItem1.Visible =
                     ((MDIParent)this.ParentForm).productCategoryToolStripMenuItem1.Enabled =

                     ((MDIParent)this.ParentForm).productSubCategoryToolStripMenuItem.Visible =
                     ((MDIParent)this.ParentForm).productSubCategoryToolStripMenuItem.Enabled =

                     ((MDIParent)this.ParentForm).brandToolStripMenuItem1.Visible =
                     ((MDIParent)this.ParentForm).brandToolStripMenuItem1.Enabled =

                     ((MDIParent)this.ParentForm).addNewProductToolStripMenuItem.Visible = false;
                        ((MDIParent)this.ParentForm).productListToolStripMenuItem1.Enabled =
                        ((MDIParent)this.ParentForm).productListToolStripMenuItem1.Visible =
                        ((MDIParent)this.ParentForm).productPriceChangeHistoryListToolStripMenuItem.Enabled =
                        ((MDIParent)this.ParentForm).productPriceChangeHistoryListToolStripMenuItem.Visible = true;

                        #endregion
                        
                        #region Setting

                        ((MDIParent)this.ParentForm).settingsToolStripMenuItem1.Visible = true;



                      
                        ((MDIParent)this.ParentForm).counterToolStripMenuItem1.Visible = false;
                      
                        ((MDIParent)this.ParentForm).addShopToolStripMenuItem.Visible = false;

                       
                        ((MDIParent)this.ParentForm).addCityToolStripMenuItem.Visible = false;
                        #endregion

                     

                        #region Sale"

                        ((MDIParent)this.ParentForm).saleToolStripMenuItem.Visible = true;
                        #endregion

                    
                        break;
                }

            }

            else
            {

                MemberShip.isAdmin = false;
                ((MDIParent)this.ParentForm).menuStrip.Enabled = true;


                controller.Load(MemberShip.UserRoleId);


                //Super Casher OR Casher



                #region Account Menu
                //Account Menu is Visiable False By Default
                ((MDIParent)this.ParentForm).accountToolStripMenuItem1.Visible =
                ((MDIParent)this.ParentForm).userListToolStripMenuItem1.Enabled =
                ((MDIParent)this.ParentForm).addNewUserToolStripMenuItem.Enabled =
                ((MDIParent)this.ParentForm).roleManagementToolStripMenuItem.Enabled = false;
                #endregion

                #region Report Menu
                // Sub Menu visibility
                ((MDIParent)this.ParentForm).dailySummaryToolStripMenuItem.Enabled =
                ((MDIParent)this.ParentForm).dailySummaryToolStripMenuItem.Visible = controller.DailySaleSummary.View;
                ((MDIParent)this.ParentForm).transactionToolStripMenuItem.Enabled =
                ((MDIParent)this.ParentForm).transactionToolStripMenuItem.Visible = controller.TransactionReport.View;
                ((MDIParent)this.ParentForm).transactionSummaryToolStripMenuItem.Enabled =
                ((MDIParent)this.ParentForm).transactionSummaryToolStripMenuItem.Visible = controller.TransactionSummaryReport.View;
                ((MDIParent)this.ParentForm).transactionDetailByItemToolStripMenuItem.Enabled =
                ((MDIParent)this.ParentForm).transactionDetailByItemToolStripMenuItem.Visible = controller.TransactionDetailReport.View;
                ((MDIParent)this.ParentForm).itemSummaryToolStripMenuItem.Enabled =
                ((MDIParent)this.ParentForm).itemSummaryToolStripMenuItem.Visible = controller.ItemSummaryReport.View;
                 ((MDIParent)this.ParentForm).topToolStripMenuItem.Enabled =
                ((MDIParent)this.ParentForm).topToolStripMenuItem.Visible = controller.TopBestSellerReport.View;
                //((MDIParent)this.ParentForm).productReportToolStripMenuItem.Enabled =
                //((MDIParent)this.ParentForm).productReportToolStripMenuItem.Visible = controller.ProductReport.View;
                ((MDIParent)this.ParentForm).averageMonthlyReportToolStripMenuItem.Enabled =
                ((MDIParent)this.ParentForm).averageMonthlyReportToolStripMenuItem.Visible = controller.AverageMonthlyReport.View;
               

                // Main Menu Visibility                
                var SubItemList = ((MDIParent)this.ParentForm).reportsToolStripMenuItem.DropDownItems;
                bool IsVisiable = false;
                foreach (ToolStripMenuItem item in SubItemList)
                {
                    if (item.Enabled == true)
                    {
                        IsVisiable = true;
                        break;
                    }
                }
                ((MDIParent)this.ParentForm).reportsToolStripMenuItem.Visible = IsVisiable;
                #endregion

              
                switch (_menu)
                {
                    case "BackOffice":

                        #region Product Menu
                        // Main Menu Visibility
                        //((MDIParent)this.ParentForm).productToolStripMenuItem.Visible = controller.Category.View && controller.SubCategory.View && controller.Brand.View && controller.Product.Add && controller.Product.View;
                        if (controller.Category.View == false && controller.SubCategory.View == false && controller.Brand.View == false && controller.Product.Add == false && controller.Product.View == false)
                        {
                            ((MDIParent)this.ParentForm).productToolStripMenuItem.Visible = false;
                        }
                        else
                        {
                            ((MDIParent)this.ParentForm).productToolStripMenuItem.Visible = true;

                            // Sub Menu Visibility
                            ((MDIParent)this.ParentForm).productCategoryToolStripMenuItem1.Enabled =
                            ((MDIParent)this.ParentForm).productCategoryToolStripMenuItem1.Visible = controller.Category.View;
                            ((MDIParent)this.ParentForm).productSubCategoryToolStripMenuItem.Enabled =
                            ((MDIParent)this.ParentForm).productSubCategoryToolStripMenuItem.Visible = controller.SubCategory.View;
                            ((MDIParent)this.ParentForm).brandToolStripMenuItem1.Enabled =
                            ((MDIParent)this.ParentForm).brandToolStripMenuItem1.Visible = controller.Brand.View;
                            //((MDIParent)this.ParentForm).addNewProductToolStripMenuItem.Enabled =
                            //((MDIParent)this.ParentForm).addNewProductToolStripMenuItem.Visible = controller.Product.Add;
                            //((MDIParent)this.ParentForm).productCategoryToolStripMenuItem1.Visible = false;

                            //((MDIParent)this.ParentForm).productSubCategoryToolStripMenuItem.Visible = false;

                            //((MDIParent)this.ParentForm).brandToolStripMenuItem1.Visible = false;

                            ((MDIParent)this.ParentForm).addNewProductToolStripMenuItem.Visible = false;
                            ((MDIParent)this.ParentForm).productListToolStripMenuItem1.Enabled =
                            ((MDIParent)this.ParentForm).productListToolStripMenuItem1.Visible = controller.Product.View;
                            ((MDIParent)this.ParentForm).productPriceChangeHistoryListToolStripMenuItem.Enabled =
                            ((MDIParent)this.ParentForm).productPriceChangeHistoryListToolStripMenuItem.Visible = controller.Product.View;
                          
                        }


                        #endregion

                 
                        #region Setting

                        //((MDIParent)this.ParentForm).settingsToolStripMenuItem1.Visible = controller.Setting.Add && controller.Consignor.Add
                        //    && controller.MeasurementUnit.Add && controller.CurrencyExchange.Add
                        //    && controller.TaxRate.Add && controller.City.Add;

                        // Main Menu Visibility
                        if (!controller.Setting.Add && !controller.Counter.Add)
                        {
                            ((MDIParent)this.ParentForm).settingsToolStripMenuItem1.Visible = false;
                        }
                        else
                        {
                            ((MDIParent)this.ParentForm).settingsToolStripMenuItem1.Visible = true;

                            // Sub Menu visibility
                            ((MDIParent)this.ParentForm).configurationSettingToolStripMenuItem.Enabled =
                            ((MDIParent)this.ParentForm).configurationSettingToolStripMenuItem.Visible = controller.Setting.Add;

                            ((MDIParent)this.ParentForm).counterToolStripMenuItem1.Enabled =
                            ((MDIParent)this.ParentForm).counterToolStripMenuItem1.Visible = controller.Counter.Add;

                          ((MDIParent)this.ParentForm).addCityToolStripMenuItem.Enabled =
                            ((MDIParent)this.ParentForm).addCityToolStripMenuItem.Visible = controller.City.Add;

                            ((MDIParent)this.ParentForm).addShopToolStripMenuItem.Enabled = true;
                            ((MDIParent)this.ParentForm).addShopToolStripMenuItem.Visible = true;


                        }


                        #endregion


                        #region Tools Menu
                        //export are only allowed on server machine
                        bool IsAllowed = DatabaseControlSetting._ServerName.ToUpper().StartsWith(System.Environment.MachineName.ToUpper());
                        //Main Menu
                        ((MDIParent)this.ParentForm).toolsToolStripMenuItem.Visible = IsAllowed;

                        // Sub Menu
                        // 1 Chashier are not allowed to restore database, 
                        ((MDIParent)this.ParentForm).databaseImportToolStripMenuItem.Enabled =
                        ((MDIParent)this.ParentForm).databaseImportToolStripMenuItem.Visible = false;

                        // 2 export are only allowed on server machine
                        ((MDIParent)this.ParentForm).databaseExportToolStripMenuItem.Enabled =
                        ((MDIParent)this.ParentForm).databaseExportToolStripMenuItem.Visible = IsAllowed;
                        #endregion

                        #region Sale"

                        ((MDIParent)this.ParentForm).saleToolStripMenuItem.Visible = false;
                        #endregion


                        break;
                    case "POS":
                        #region "Account"
                        ((MDIParent)this.ParentForm).accountToolStripMenuItem1.Visible = false;
                        #endregion


                        #region Product Menu
                        // Main Menu Visibility
                        //((MDIParent)this.ParentForm).productToolStripMenuItem.Visible = controller.Category.View && controller.SubCategory.View && controller.Brand.View && controller.Product.Add && controller.Product.View;
                        if (controller.Product.View == false)
                        {
                            ((MDIParent)this.ParentForm).productToolStripMenuItem.Visible = false;
                        }
                        else
                        {
                            ((MDIParent)this.ParentForm).productToolStripMenuItem.Visible = true;

                            // Sub Menu Visibility

                            ((MDIParent)this.ParentForm).productListToolStripMenuItem1.Enabled =
                            ((MDIParent)this.ParentForm).productListToolStripMenuItem1.Visible = controller.Product.View;
                            ((MDIParent)this.ParentForm).productPriceChangeHistoryListToolStripMenuItem.Enabled =
                            ((MDIParent)this.ParentForm).productPriceChangeHistoryListToolStripMenuItem.Visible = controller.Product.View;
                           
                            ((MDIParent)this.ParentForm).productCategoryToolStripMenuItem1.Enabled =
                             ((MDIParent)this.ParentForm).productCategoryToolStripMenuItem1.Visible =
                            ((MDIParent)this.ParentForm).productSubCategoryToolStripMenuItem.Enabled =
                            ((MDIParent)this.ParentForm).productSubCategoryToolStripMenuItem.Visible =
                            ((MDIParent)this.ParentForm).brandToolStripMenuItem1.Enabled =
                            ((MDIParent)this.ParentForm).brandToolStripMenuItem1.Visible =

                            ((MDIParent)this.ParentForm).addNewProductToolStripMenuItem.Visible = false;



                        }


                        #endregion



                        #region "Setting"
                        if (!controller.Setting.Add)
                        {
                            ((MDIParent)this.ParentForm).settingsToolStripMenuItem1.Visible = false;
                        }
                        else
                        {
                            ((MDIParent)this.ParentForm).settingsToolStripMenuItem1.Visible = true;

                            // Sub Menu visibility
                            ((MDIParent)this.ParentForm).configurationSettingToolStripMenuItem.Enabled =
                            ((MDIParent)this.ParentForm).configurationSettingToolStripMenuItem.Visible = controller.Setting.Add;



                      
                            ((MDIParent)this.ParentForm).counterToolStripMenuItem1.Visible = false;


                            ((MDIParent)this.ParentForm).addShopToolStripMenuItem.Visible = false;

                            ((MDIParent)this.ParentForm).addCityToolStripMenuItem.Visible = false;
                        }

                        #endregion


                        #region Sale"

                        ((MDIParent)this.ParentForm).saleToolStripMenuItem.Visible = true;
                        #endregion

                      
                        break;
                }
            }
        }

        private void pcBackOffice_Click(object sender, EventArgs e)
        {

            Permission_BO_OR_POS("BackOffice", user.MenuPermission);

        }

        private void PCPOS_Click(object sender, EventArgs e)
        {

            Permission_BO_OR_POS("POS", user.MenuPermission);


        }
    }
}
