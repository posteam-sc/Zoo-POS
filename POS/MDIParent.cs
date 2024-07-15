using POS.APP_Data;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace POS
{
    public partial class MDIParent : Form
    {
        //string  expireDate = "15-Jan-2019";
        public static long LocalAdultPrice = 0;
        public static long LocalChildPrice = 0;
        public static long ForeignAdultPrice = 0;
        public static long ForeignChildPrice = 0;

        public static long SanghaPrice = 0;
        public static long PrivateTourPrice = 0;
        public static decimal LocalAdultDis = 0;
        public static decimal LocalChildDis = 0;
        public static decimal ForeignAdultDis = 0;
        public static decimal ForeignChildDis = 0;

        public static decimal SanghaDis = 0;
        public static decimal PrivateTourDis = 0;
        #region Events


        public MDIParent()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            
            //this.Size = Screen.PrimaryScreen.WorkingArea.Size;
        }
        ApplicationLog al = new ApplicationLog();
        private void MDIParent_Load(object sender, EventArgs e)
        {
            try
            {
                localizationToolStripMenuItem.Visible = !SettingController.IsSourcecode;
                
                POS.APP_Data.POSEntities entity = new POS.APP_Data.POSEntities();
                this.TopMost = SettingController.TopMost;

                var iProduct = entity.Products.Where(x => x.ProductCode == "11111" || x.ProductCode == "11112" || x.ProductCode == "11113" || x.ProductCode == "11114" || x.ProductCode == "11115" || x.ProductCode == "11117").OrderBy(x => x.ProductCode).ToList();
                if (iProduct != null && iProduct.Count() == 6)
                {
                    LocalAdultPrice = iProduct[0].Price;
                    LocalChildPrice = iProduct[1].Price;
                    ForeignAdultPrice = iProduct[2].Price;
                    ForeignChildPrice = iProduct[3].Price;
                    SanghaPrice = iProduct[4].Price;

                    PrivateTourPrice = iProduct[5].Price;
                    LocalAdultDis = iProduct[0].DiscountRate;
                    LocalChildDis = iProduct[1].DiscountRate;
                    ForeignAdultDis = iProduct[2].DiscountRate;
                    ForeignChildDis = iProduct[3].DiscountRate;
                    SanghaDis = iProduct[4].DiscountRate;

                    PrivateTourDis = iProduct[5].DiscountRate;
                }
                #region SQL ServiceCheck
                if (new Utility.DBService().AllowToStart && !new Utility.DBService().Running)
                {
                    new Utility.DBService().Run();
                }
                #endregion
                Localization.Forms_Controls_Insertion();
                Localization.Localize_FormControls(this);
                Create_RequirementsForDB();
                if (!Utility.IsRegister())
                {
                    Register regform = new Register();
                    regform.WindowState = FormWindowState.Maximized;
                    regform.MdiParent = this;
                    regform.Show();
                    this.menuStrip.Enabled = false;
                    //this.menuStrip.Visible = false;
                }
                else if (!MemberShip.isLogin)
                {
                    Login loginForm = new Login();
                    loginForm.MdiParent = this;
                    loginForm.WindowState = FormWindowState.Maximized;
                    loginForm.Show();
                    this.menuStrip.Enabled = false;
                    // this.menuStrip.Visible = false;
                    toolStripStatusLabel.Text = string.Empty;
                    toolStripStatusLabel1.Text = string.Empty;
                    tSSBOOrPOS.Text = string.Empty;
                    statusToolStripMenuItem.Text = string.Empty;
                }
                else
                {
                    Sales form = new Sales();
                    form.WindowState = FormWindowState.Maximized;
                    form.MdiParent = this;
                    form.Show();
                }
            }catch(Exception ex)
            {
                string sExec = ex.Message;
                if(ex.InnerException!=null)
                {
                    sExec = ex.InnerException.ToString();
                }
                al.WriteErrorLog(ex.ToString(), "MDIParent_Load", sExec);
            }
        }

        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

      
        
        private void transactionSummaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Role Management
            RoleManagementController controller = new RoleManagementController();
            controller.Load(MemberShip.UserRoleId);
            if (controller.TransactionSummaryReport.View || MemberShip.isAdmin)
            {
                TransactionSummary newform = new TransactionSummary();
                newform.ShowDialog();
            }
            else
            {
                MessageBox.Show("You are not allowed to view transaction summary report", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }



        }

       
  
        private void transactionListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TransactionList newForm = new TransactionList();
            newForm.ShowDialog();
        }
        private void logInToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Boolean isAlreadyHave = false;

            foreach (Form child in this.MdiChildren)
            {
                if (child.Text == "Login")
                {
                    child.Activate();
                    isAlreadyHave = true;
                }
                else
                {
                    child.Close();
                }
            }
            if (!isAlreadyHave)
            {
                Login f = new Login();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void logOutToolStripMenuItem_Click(object sender, EventArgs e)
        {

            DialogResult result = MessageBox.Show("Are you sure you want to logout?", "Logout", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (result.Equals(DialogResult.OK))
            {
                Boolean isAlreadyHave = false;

                foreach (Form child in this.MdiChildren)
                {
                    if (child.Text == "LogIn")
                    {
                        child.Activate();
                        isAlreadyHave = true;
                    }
                    else
                    {
                        child.Close();
                    }
                }
                MemberShip.UserId = 0;
                MemberShip.UserName = "";
                MemberShip.UserRole = null;
                //MemberShip.isAdmin = false;
                //MemberShip.isLogin = false;

                if (!isAlreadyHave)
                {
                    Login f = new Login();
                    f.MdiParent = this;
                    f.WindowState = FormWindowState.Maximized;
                    f.Show();

                    //DisableControls();
                }
                menuStrip.Enabled = false;
                toolStripStatusLabel.Text = string.Empty;
                toolStripStatusLabel1.Text = string.Empty;
                tSSBOOrPOS.Text = string.Empty;
                statusToolStripMenuItem.Text = string.Empty;
                logOutToolStripMenuItem.Visible = false;
                logInToolStripMenuItem1.Visible = true;
            }
        }


        private void transactionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Role Management
            RoleManagementController controller = new RoleManagementController();
            controller.Load(MemberShip.UserRoleId);
            if (controller.TransactionReport.View || MemberShip.isAdmin)
            {
                TransactionReport_FOC_MPU newform = new TransactionReport_FOC_MPU();
                newform.ShowDialog();
            }
            else
            {
                MessageBox.Show("You are not allowed to view transaction report", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void itemSummaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Role Management
            RoleManagementController controller = new RoleManagementController();
            controller.Load(MemberShip.UserRoleId);
            if (controller.ItemSummaryReport.View || MemberShip.isAdmin)
            {
                ItemSummary newform = new ItemSummary();
                newform.ShowDialog();
            }
            else
            {
                MessageBox.Show("You are not allowed to view item sale summary report", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }      

        private void addCityToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Role Management
            RoleManagementController controller = new RoleManagementController();
            controller.Load(MemberShip.UserRoleId);
            if (controller.City.Add || MemberShip.isAdmin)
            {
                City newForm = new City();
                newForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("You are not allowed to add city", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void deleteLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteLogForm newform = new DeleteLogForm();
            newform.ShowDialog();
        }

    

     
        private void transactionDetailByItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Role Management
            RoleManagementController controller = new RoleManagementController();
            controller.Load(MemberShip.UserRoleId);
            if (controller.TransactionDetailReport.View || MemberShip.isAdmin)
            {
                TransactionDetailByItem newform = new TransactionDetailByItem();
                newform.ShowDialog();
            }
            else
            {
                MessageBox.Show("You are not allowed to view transaction detail report", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }


        }

     

        private void addNewProductToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Role Management
            RoleManagementController controller = new RoleManagementController();
            controller.Load(MemberShip.UserRoleId);
            if (controller.Product.Add || MemberShip.isAdmin)
            {
                NewProduct newForm = new NewProduct();
                newForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("You are not allowed to add new product", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }


        }

        private void productCategoryToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            RoleManagementController controller = new RoleManagementController();
            controller.Load(MemberShip.UserRoleId);
            if (controller.Category.View || MemberShip.isAdmin)
            {
                ProductCategory form = new ProductCategory();
                form.ShowDialog();
            }
            else
            {
                MessageBox.Show("You are not allowed to add product category", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }

        private void productSubCategoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RoleManagementController controller = new RoleManagementController();
            controller.Load(MemberShip.UserRoleId);
            if (controller.SubCategory.View || MemberShip.isAdmin)
            {
                ProductSubCategory newform = new ProductSubCategory();
                newform.ShowDialog();
            }
            else
            {
                MessageBox.Show("You are not allowed to view sub product category", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }

        private void brandToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            RoleManagementController controller = new RoleManagementController();
            controller.Load(MemberShip.UserRoleId);
            if (controller.Brand.View || MemberShip.isAdmin)
            {
                Brand newForm = new Brand();
                newForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("You are not allowed to view brand", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }

        private void productListToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            RoleManagementController controller = new RoleManagementController();
            controller.Load(MemberShip.UserRoleId);
            if (controller.Product.View || MemberShip.isAdmin)
            {
                ItemList newForm = new ItemList();
                newForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("You are not allowed to view product", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }

        private void addNewUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewUser form = new NewUser();
            form.ShowDialog();
        }

        private void userListToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            UserControl form = new UserControl();
            form.ShowDialog();
        }

        private void roleManagementToolStripMenuItem_Click(object sender, EventArgs e)
        {           
            RoleManagement newform = new RoleManagement();
            newform.ShowDialog();
        }

        private void configurationSettingToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
            Setting newform = new Setting();
            newform.ShowDialog();
        }

        private void counterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RoleManagementController controller = new RoleManagementController();
            controller.Load(MemberShip.UserRoleId);
            if (controller.Counter.Add || MemberShip.isAdmin)
            {
                Counter newForm = new Counter();
                newForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("You are not allowed to add counter", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }

        private void databaseExportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //db backup to client pc
            //Backup(true);
        
            //db backup to server pc
            BackupFileToServerPath(true);
        }

        private void databaseImportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MemberShip.isAdmin)
            {
                string fileName = Backup(false);
                Restore(ref fileName);
            }
            else
            {
                MessageBox.Show("You are not allowed to restore database.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
           
        }

        private void MDIParent_FormClosing(object sender, FormClosingEventArgs e)
        {
            toolStripStatusLabel.Text = "Saving data.. Please wait";

            //Only main server will make backup
            if (DatabaseControlSetting._ServerName.ToUpper().StartsWith(System.Environment.MachineName.ToUpper()))
            {
                Backup(true);
            }

            //db backup to server pc update date:2019-05-08
            BackupFileToServerPath(true);
            }

        private void topToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Role Management
            RoleManagementController controller = new RoleManagementController();
            controller.Load(MemberShip.UserRoleId);
            if (controller.TopBestSellerReport.View || MemberShip.isAdmin)
            {
                TopSaleReport newForm = new TopSaleReport();
                newForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("You are not allowed to view best seller item  report", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }


        }
       
        #endregion


        #region Functions


        private void Create_RequirementsForDB()
        {
            APP_Data.POSEntities entity = new APP_Data.POSEntities();
            // First Time Running... and Database is Null / Create a default Admin User
            var _userRoleList = entity.UserRoles.ToList();

            if (_userRoleList.Count == 0)
            {
                #region UserRole
                UserRole _userRole = new UserRole();
                _userRole.RoleName = "Admin";
                entity.UserRoles.Add(_userRole);
                entity.SaveChanges();

                _userRole.RoleName = "Super Cashier";
                entity.UserRoles.Add(_userRole);
                entity.SaveChanges();

                _userRole.RoleName = "Cashier";
                entity.UserRoles.Add(_userRole);
                entity.SaveChanges();
                #endregion

                #region City
                APP_Data.City _city = new APP_Data.City();
                _city.CityName = "Yangon";
                _city.IsDelete = false;
                entity.Cities.Add(_city);
                entity.SaveChanges();
                #endregion


                #region Shop
                APP_Data.Shop _shop = new APP_Data.Shop();
                _shop.ShopName = "MainOffice";
                _shop.CityId = 1;
                _shop.ShortCode = "MO";
                _shop.IsDefaultShop = true;

                entity.Shops.Add(_shop);
                entity.SaveChanges();
                #endregion

           
               

                #region Setting
                APP_Data.Setting _setting = new APP_Data.Setting();
                _setting.Key = "barcode_printer";
                _setting.Value = "Plz Choose Bar Code Printer";
                entity.Settings.Add(_setting);
                entity.SaveChanges();

                _setting.Key = "a4_printer";
                _setting.Value = "Plz Choose A4 Printer";
                entity.Settings.Add(_setting);
                entity.SaveChanges();

                _setting.Key = "slip_printer_counter1";
                _setting.Value = "Plz Choose Slip Printer";
                entity.Settings.Add(_setting);
                entity.SaveChanges();

                _setting.Key = "shop_name";
                _setting.Value = "Plz Enter Zoo Name";
                entity.Settings.Add(_setting);
                entity.SaveChanges();

                _setting.Key = "branch_name";
                _setting.Value = "Plz Enter Branch Name or Address";
                entity.Settings.Add(_setting);
                entity.SaveChanges();

                _setting.Key = "phone_number";
                _setting.Value = "Plz Enter Phone Number";
                entity.Settings.Add(_setting);
                entity.SaveChanges();

                _setting.Key = "opening_hours";
                _setting.Value = "Plz Enter Opening Hours";
                entity.Settings.Add(_setting);
                entity.SaveChanges();

                _setting.Key = "default_tax_rate";
                _setting.Value = "1";
                entity.Settings.Add(_setting);
                entity.SaveChanges();

                _setting.Key = "default_city_id";
                _setting.Value = "1";
                entity.Settings.Add(_setting);
                entity.SaveChanges();

                _setting.Key = "default_top_sale_row";
                _setting.Value = "1";
                entity.Settings.Add(_setting);
                entity.SaveChanges();

                _setting.Key = "default_currency";
                _setting.Value = "1";
                entity.Settings.Add(_setting);
                entity.SaveChanges();

                _setting.Key = "Company_StartDate";
                _setting.Value = DateTime.Now.ToString();
                entity.Settings.Add(_setting);
                entity.SaveChanges();

                _setting.Key = "default_printer";
                _setting.Value = "Plz choose Default Printer";
                entity.Settings.Add(_setting);
                entity.SaveChanges();

                _setting.Key = "IsBackOffice";
                _setting.Value = "1";
                entity.Settings.Add(_setting);
                entity.SaveChanges();

                _setting.Key = "default_font";
                _setting.Value = "English";
                entity.Settings.Add(_setting);
                entity.SaveChanges();

                _setting.Key = "IsSourcecode";
                _setting.Value = "False";
                entity.Settings.Add(_setting);
                entity.SaveChanges();

                _setting.Key = "pos_id";
                _setting.Value = "0";
                entity.Settings.Add(_setting);
                entity.SaveChanges();

                _setting.Key = "retrieve_pattern";
                _setting.Value = "fefo";
                entity.Settings.Add(_setting);
                entity.SaveChanges();

                _setting.Key = "allow_dynamic";
                _setting.Value = "False";
                entity.Settings.Add(_setting);
                entity.SaveChanges();

                _setting.Key = "app_mode";
                _setting.Value = "Production";
                entity.Settings.Add(_setting);
                entity.SaveChanges();

                _setting.Key = "usetable";
                _setting.Value = "False";
                entity.Settings.Add(_setting);
                entity.SaveChanges();

                _setting.Key = "usequeue";
                _setting.Value = "False";
                entity.Settings.Add(_setting);
                entity.SaveChanges();

                _setting.Key = "service_fee";
                _setting.Value = "0";
                entity.Settings.Add(_setting);
                entity.SaveChanges();

                _setting.Key = "detect_idle";
                _setting.Value = "False";
                entity.Settings.Add(_setting);
                entity.SaveChanges();


                _setting.Key = "idle_Time";
                _setting.Value = "1";
                entity.Settings.Add(_setting);
                entity.SaveChanges();

                _setting.Key = "ticketsale";
                _setting.Value = "False";
                entity.Settings.Add(_setting);
                entity.SaveChanges();

                _setting.Key = "allow_minimize";
                _setting.Value = "True";
                entity.Settings.Add(_setting);
                entity.SaveChanges();

                _setting.Key = "topmost";
                _setting.Value = "False";
                entity.Settings.Add(_setting);
                entity.SaveChanges();

                _setting.Key = "customsku";
                _setting.Value = "False";
                entity.Settings.Add(_setting);
                entity.SaveChanges();


                _setting.Key = "uppercase";
                _setting.Value = "False";
                entity.Settings.Add(_setting);
                entity.SaveChanges();


                _setting.Key = "a4image";
                _setting.Value = "False";
                entity.Settings.Add(_setting);
                entity.SaveChanges();



                #endregion

                #region User
                string UserCodeNo = "";
                User _user = new User();
                _user.ShopId = 1;
                _user.Name = "admin";

                _user.UserRoleId = 1;
                _user.Password = Utility.EncryptString("123456", "SCPos");
                _user.DateTime = System.DateTime.Now;
                _user.MenuPermission = "Both";

                //to get user code No
                UserCodeNo = Utility.Get_UserCodeNo(1);

                _user.UserCodeNo = UserCodeNo;
                entity.Users.Add(_user);
                entity.SaveChanges();


                _user.ShopId = 1;
                _user.Name = "sourcecodeadmin";
                _user.UserRoleId = 1;
                _user.Password = Utility.EncryptString("Sourcec0de", "SCPos");
                _user.DateTime = System.DateTime.Now;
                _user.MenuPermission = "Both";
                //to get user code No
                UserCodeNo = Utility.Get_UserCodeNo(1);

                _user.UserCodeNo = UserCodeNo;

                entity.Users.Add(_user);
                entity.SaveChanges();
                #endregion

                #region Counter
                APP_Data.Counter _counter = new APP_Data.Counter();
                _counter.Name = "One";
                _counter.IsDelete = false;
                entity.Counters.Add(_counter);
                entity.SaveChanges();
                #endregion

                #region PaymentType
                APP_Data.PaymentType _paymentType = new APP_Data.PaymentType();
                _paymentType.Name = "Cash";

                entity.PaymentTypes.Add(_paymentType);
                entity.SaveChanges();

                _paymentType.Name = "Credit";
                entity.PaymentTypes.Add(_paymentType);
                entity.SaveChanges();

                _paymentType.Name = "GiftCard";
                entity.PaymentTypes.Add(_paymentType);
                entity.SaveChanges();

                _paymentType.Name = "FOC";
                entity.PaymentTypes.Add(_paymentType);
                entity.SaveChanges();

                _paymentType.Name = "MPU";
                entity.PaymentTypes.Add(_paymentType);
                entity.SaveChanges();

                _paymentType.Name = "Tester";
                entity.PaymentTypes.Add(_paymentType);
                entity.SaveChanges();
                #endregion



            }
           
            //if (entity.TicketButtonAssigns.Count() == 0)
            //{
            //    for (int i = 0; i < 4; i++)
            //    {
            //        TicketButtonAssign tba = new TicketButtonAssign();
            //        tba.ButtonName = "btnAdd" + (i + 1).ToString();
            //        tba.ButtonText = "+";

            //        entity.TicketButtonAssigns.Add(tba);
            //        entity.SaveChanges();
            //        TicketButtonAssign tba1 = new TicketButtonAssign();
            //        tba1.ButtonName = "btnAdd" + (i + 1).ToString() + "Minus";
            //        tba1.ButtonText = "-";

            //        entity.TicketButtonAssigns.Add(tba1);
            //        entity.SaveChanges();
            //        if (tba.ButtonName == "btnAdd1")
            //        {
            //            TicketButtonAssign lblLocalAdult = new TicketButtonAssign();
            //            lblLocalAdult.ButtonName = "lblLocalAdult";
            //            lblLocalAdult.ButtonText = "0";

            //            entity.TicketButtonAssigns.Add(lblLocalAdult);
            //            entity.SaveChanges();
            //        }
            //        if (tba.ButtonName == "btnAdd2")
            //        {
            //            TicketButtonAssign lblLocalChild = new TicketButtonAssign();
            //            lblLocalChild.ButtonName = "lblLocalChild";
            //            lblLocalChild.ButtonText = "0";
            //            entity.TicketButtonAssigns.Add(lblLocalChild);
            //            entity.SaveChanges();
            //        }
            //        if (tba.ButtonName == "btnAdd3")
            //        {
            //            TicketButtonAssign lblForeignAdult = new TicketButtonAssign();
            //            lblForeignAdult.ButtonName = "lblForeignAdult";
            //            lblForeignAdult.ButtonText = "0";
            //            entity.TicketButtonAssigns.Add(lblForeignAdult);
            //            entity.SaveChanges();
            //        }
            //        if (tba.ButtonName == "btnAdd4")
            //        {
            //            TicketButtonAssign lblForeignChild = new TicketButtonAssign();
            //            lblForeignChild.ButtonName = "lblForeignChild";
            //            lblForeignChild.ButtonText = "0";
            //            entity.TicketButtonAssigns.Add(lblForeignChild);
            //            entity.SaveChanges();
            //        }
            //    }
            //}

        }

        private void Restore(ref string fileName)
        {
            if (MessageBox.Show("Are you sure that you want to restore", "", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                if (ofdDBBackup.ShowDialog(this) == DialogResult.Cancel)
                {
                    return;
                }

                APP_Data.POSEntities entity = new APP_Data.POSEntities();
                entity.ClearDBConnections();

                fileName = ofdDBBackup.FileName;
                string destFileName = string.Empty;
                string[] fnArr1 = fileName.Split('_');
                string[] fnArr2 = fileName.Split('.');
                string[] fnArr3 = fileName.Split('\\');
                string filePath = string.Empty;
                for (int i = 0; i < fnArr3.Length - 1; i++)
                {
                    if (i + 1 != fnArr3.Length - 1)
                    {
                        filePath += fnArr3[i] + "/";
                    }
                    else
                    {
                        filePath += fnArr3[i];
                    }
                }

                /*--Decrypt DB--*/
                for (int i = 0; i < fnArr1.Length - 1; i++)
                {
                    if (i + 1 != fnArr1.Length - 1)
                    {
                        destFileName += fnArr1[i] + "_";
                    }
                    else
                    {
                        destFileName += fnArr1[i];
                    }
                }
                destFileName = destFileName + "." + fnArr2[1];
                if (File.Exists(destFileName)) File.Delete(destFileName);

                Utility.DecryptFile(fileName, destFileName);

                /*--Restore DB--*/
                RestoreHelper restoreHelper = new RestoreHelper();

                string[] tempConString = Properties.Settings.Default.MyConnectionString.Split(';');
                string[] userNameArr = tempConString[tempConString.Length - 2].Split('=');
                string[] passwordArr = tempConString[tempConString.Length - 1].Split('=');

                //restoreHelper.RestoreDatabase(Utility._DBName, destFileName + "." + fnArr2[1], Utility._ServerName, userNameArr[userNameArr.Length - 1], passwordArr[passwordArr.Length - 1]);
                restoreHelper.RestoreDatabase(DatabaseControlSetting._DBName, destFileName, DatabaseControlSetting._ServerName, DatabaseControlSetting._DBUser, DatabaseControlSetting._DBPassword);
                try
                {
                    if (File.Exists(destFileName))
                    {
                        File.Delete(destFileName);
                    }
                    MessageBox.Show("Successfully Restored..");
                }
                catch
                {
                    MessageBox.Show("Can't remove temp files", "Error!!");
                }
            }
        }

        private string Backup(bool IsManual)
        {
            string activeDir = @"d:\";
            bool directorD = true;
            if (!Directory.Exists(activeDir))
            {
                directorD = false;
                activeDir = @"c:\";
            }
            /*-- Create a new subfolder under the current active folder --*/
            string newPath;
            if (IsManual)
            {
                newPath = System.IO.Path.Combine(activeDir, "Manual_Backups");
            }
            else
            {
                newPath = System.IO.Path.Combine(activeDir, "DB_Backups");
            }

            if (!System.IO.Directory.Exists(newPath))
            {
                DirectoryInfo di = Directory.CreateDirectory(newPath);
                //di.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
            }

            /*-- Backup DB --*/
            BackupHelper backHelper = new BackupHelper();
            string fileName;
            string bakName = DatabaseControlSetting._DBName + "[" + DateTime.Now.ToString("dd-MM-yyyy hh-mm tt") + "].bak";
            if (IsManual)
                fileName = directorD == true ? "D:/Manual_Backups/" + bakName : "C:/Manual_Backups/" + bakName;
            else
                fileName = directorD == true ? "D:/DB_Backups/" + bakName : "C:/DB_Backups/" + bakName;

            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            bool isBackup = false;
            backHelper.BackupDatabase(DatabaseControlSetting._DBName, DatabaseControlSetting._DBUser, DatabaseControlSetting._DBPassword, DatabaseControlSetting._ServerName, fileName, ref isBackup);

            /*-- Encrypt DB --*/
            string[] fileNameArr = fileName.Split('\\');
            string[] encryptFileNameArr = fileName.Split('.');
            string tempFileName = encryptFileNameArr[0] + "_encrypted." + encryptFileNameArr[1];

            if (File.Exists(tempFileName))
            {
                File.Delete(tempFileName);
            }
            if (isBackup)
            {
                Utility.EncryptFile(fileName, tempFileName);
            }

            try
            {
                File.Delete(fileName);
                if (isBackup)
                {
                    MessageBox.Show("Successfully Exported to " + newPath);
                }
            }
            catch
            {
                MessageBox.Show("Can't remove temporary files");
            }
            return fileName;
        }
        private string BackupFileToServerPath(bool IsManual) {
            string fileName=string.Empty;
            if (System.Configuration.ConfigurationManager.AppSettings["IsBackupByClient"]=="true") {
                string activeDir = System.Configuration.ConfigurationManager.AppSettings["DB_BackupPath"];//backuping db file to other server path
                bool directorD = true;

                /*-- Create a new subfolder under the current active folder --*/
                string newPath;
                if (IsManual) {
                    newPath = Path.Combine(activeDir, @"Manual_Backups\");
                    }
                else {
                    newPath = Path.Combine(activeDir, @"DB_Backups\");
                    }

                if (!System.IO.Directory.Exists(newPath)) {
                    DirectoryInfo di = Directory.CreateDirectory(newPath);
                    //di.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
                    }

                /*-- Backup DB --*/
                BackupHelper backHelper = new BackupHelper();
                string bakName = DatabaseControlSetting._DBName + "[" + DateTime.Now.ToString("dd-MM-yyyy hh-mm tt") + "].bak";
                if (IsManual)
                    //fileName = directorD == true ? "D:/Manual_Backups/" + bakName : "C:/Manual_Backups/" + bakName;//for same server
                    fileName = directorD == true ? newPath + bakName : "C:/Manual_Backups/" + bakName;
                else
                    // fileName = directorD == true ? "D:/DB_Backups/" + bakName : "C:/DB_Backups/" + bakName; //old style for same server db and mpos
                    fileName = directorD == true ? newPath + bakName : "C:/DB_Backups/" + bakName;

                if (File.Exists(fileName)) {
                    File.Delete(fileName);
                    }
                bool isBackup = false;
                backHelper.BackupDatabase(DatabaseControlSetting._DBName, DatabaseControlSetting._DBUser, DatabaseControlSetting._DBPassword, DatabaseControlSetting._ServerName, fileName, ref isBackup);

                /*-- Encrypt DB --*/
                string[] fileNameArr = fileName.Split('\\');
                string[] encryptFileNameArr = fileName.Split('.');
                string tempFileName = encryptFileNameArr[0] + "_encrypted." + encryptFileNameArr[1];

                if (File.Exists(tempFileName)) {
                    File.Delete(tempFileName);
                    }
                if (isBackup) {
                    Utility.EncryptFile(fileName, tempFileName);
                    }

                try {
                    File.Delete(fileName);
                    if (isBackup) {
                        MessageBox.Show("Successfully Database Backup of mPOS System!!!", "Information :)", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                catch {
                    MessageBox.Show("Can't remove temporary files", "Information :)", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    } 
                }
            return fileName;
            }
        #endregion
        private void productPriceChangeHistoryListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PriceChangeHistoryList newform = new PriceChangeHistoryList();
            newform.ShowDialog();
        }

        private void saleToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if ((Application.OpenForms["Sales"] as Sales) != null)
            {
                //Form is already open
            }
            else
            {
                Sales form = new Sales();
                form.WindowState = FormWindowState.Maximized;
                form.MdiParent = this;
                form.Show();
            }
        }

        private void dailySummaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Role Management
            RoleManagementController controller = new RoleManagementController();
            controller.Load(MemberShip.UserRoleId);
            if (controller.DailySaleSummary.View || MemberShip.isAdmin)
            {
                Loc_ItemSummary newForm = new Loc_ItemSummary();
                newForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("You are not allowed to view daily summary report", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("MPOS.chm");
        }

      
       private void averageMonthlyReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Role Management
            RoleManagementController controller = new RoleManagementController();
            controller.Load(MemberShip.UserRoleId);
            if (controller.AverageMonthlyReport.View || MemberShip.isAdmin)
            {
                AverageMonthlySaleReport_frm avgMonthlyReport = new AverageMonthlySaleReport_frm();
                avgMonthlyReport.ShowDialog();
            }
            else
            {
                MessageBox.Show("You are not allowed to view transaction detail report", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

       
     
     
     

        private void menuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        
        private void addShopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Shop form = new Shop();
            form.ShowDialog();
        }

     
        private void tSSBOOrPOS_Click(object sender, EventArgs e)
        {
            switch (tSSBOOrPOS.Text)
            {
                case "Continue to Back Office ->":

                    ContinueToBOORPOS("BackOffice");
                    break;
                case "Continue to POS ->":
                    ContinueToBOORPOS("POS");
                    break;
            }
        }

        private void ContinueToBOORPOS(string _menu)
        {
            APP_Data.POSEntities entity = new APP_Data.POSEntities();
            string menuPermission = entity.Users.Where(x => x.Id == MemberShip.UserId).Select(x => x.MenuPermission).FirstOrDefault();
            Login form = new Login();
            form.MdiParent = this;
            form.Permission_BO_OR_POS(_menu, menuPermission);

        }

       
        private void localizationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SettingController.IsSourcecode)
            {
                frmlocalize newform = new frmlocalize();
                newform.Show(this);
            }
            else
            {
                MessageBox.Show("You are not allowed to perform this action.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void versiontoolStripMenuItem_Click(object sender, EventArgs e)
        {
            VersionInfo newfrom = new VersionInfo();
            newfrom.Show(this);
        }

        private void MDIParent_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Shift && e.KeyCode == Keys.D)
            {

                if (MemberShip.isAdmin)
                {
                    if (string.IsNullOrEmpty(SettingController.ApplicationMode))
                    {
                        SettingController.ApplicationMode = "Production";
                    }
                    if (SettingController.ApplicationMode == "Developer")
                    {
                        DialogResult diaResult = MessageBox.Show("Swith to Production Mode", "mPOS", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                        if (diaResult == DialogResult.OK)
                        {
                            SettingController.ApplicationMode = "Production";
                        }
                    }
                    else if (SettingController.ApplicationMode == "Production")
                    {
                        DialogResult diaResult = MessageBox.Show("Swith to Developer Mode", "mPOS", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                        if (diaResult == DialogResult.OK)
                        {
                            SettingController.ApplicationMode = "Developer";
                        }
                    }
                }
            }
            else if (e.Control && e.KeyCode == Keys.R)
            {
                chart newchart = new chart();
                newchart.FormFresh();
            }
        }

      
        public string minimizePass { get; set; }
        private void MDIParent_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized && !SettingController.AllowMinimize)
            {
                if (minimizePass != "scadm1n")
                {
                    this.WindowState = FormWindowState.Maximized;
                    GeneralPassword newform = new GeneralPassword();
                    newform.Parent = this;
                    DialogResult dir = newform.ShowDialog();
                }
                if (minimizePass == "scadm1n")
                {
                    this.WindowState = FormWindowState.Minimized;
                    minimizePass = "";
                }
                else
                {
                    this.WindowState = FormWindowState.Maximized;
                    minimizePass = "";
                    MessageBox.Show("You can't minimize mPOS");
                }
            }
        }

        private void assignTicketToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AssignTicketButton form = new AssignTicketButton();
            form.Show();
        }

       

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //Ticket Summary by HMT
            RoleManagementController controller = new RoleManagementController();
            controller.Load(MemberShip.UserRoleId);
            if (controller.TicketSummaryReport.View || MemberShip.isAdmin)
            {
                TicketSummary newForm = new TicketSummary();
                newForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("You are not allowed to view ticket summary report", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            //Ticket Detail by HMT
            RoleManagementController controller = new RoleManagementController();
            controller.Load(MemberShip.UserRoleId);
            if (controller.TicketDetailReport.View || MemberShip.isAdmin)
            {
                TicketDetail newForm = new TicketDetail();
                newForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("You are not allowed to view ticket detail report", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void statusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            switch (statusToolStripMenuItem.Text)
            {
                case "Continue to Back Office":

                    ContinueToBOORPOS("BackOffice");
                    break;
                case "Continue to POS":
                    ContinueToBOORPOS("POS");
                    break;
            }
        }
    }
}

