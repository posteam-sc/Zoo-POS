using POS.APP_Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace POS
{
    public partial class Setting : Form
    {
       
      
        #region Variable
        POSEntities entity = new POSEntities();
        private ToolTip tp = new ToolTip();
        private String FilePath;
        #endregion
        public Setting()
        {
            InitializeComponent();
        }
         
   
        private void Setting_Load(object sender, EventArgs e)
        {
            if (!MemberShip.isAdmin)
            {
                foreach (Control ctl in ConfigurationPage.Controls) ctl.Enabled = MemberShip.isAdmin;
            }
            Localization.Localize_FormControls(this);
            SettingController.SetGlobalDateFormat(dpCompanyStartDate);
            //Check Admin Or Not
           
            #region BindShop
            Utility.BindShop(cboShopList);

            cboShopList.Text = SettingController.DefaultShop.ShopName;
            #endregion

            Utility.ShopComBo_EnableOrNot(cboShopList,true);
            #region Printer

            foreach (string printerName in PrinterSettings.InstalledPrinters)
            {
                cboBarcodePrinter.Items.Add(printerName);
                cboA4Printer.Items.Add(printerName);
                cboSlipPrinter.Items.Add(printerName);
            }

            if (DefaultPrinter.BarcodePrinter != null)
            {
                cboBarcodePrinter.Text = DefaultPrinter.BarcodePrinter;
            }
            if (DefaultPrinter.A4Printer != null)
            {
                cboA4Printer.Text = DefaultPrinter.A4Printer;
            }
            if (DefaultPrinter.SlipPrinter != null)
            {
                cboSlipPrinter.Text = DefaultPrinter.SlipPrinter;
            }

            if (SettingController.SelectDefaultPrinter != "")
            {
                switch (SettingController.SelectDefaultPrinter)
                {
                    case "A4 Printer":
                        rdoA4Printer.Checked=true;
                        break;
                    case "Slip Printer":
                        rdoSlipPrinter.Checked=true;
                        break;
                }
            }
            #endregion

            #region city
            List<APP_Data.City> cityList = entity.Cities.ToList();
            cboCity.DataSource = cityList;
            cboCity.DisplayMember = "CityName";
            cboCity.ValueMember = "Id";
            if (SettingController.DefaultCity != 0)
            {
                int id = Convert.ToInt32(SettingController.DefaultCity);
                APP_Data.City city = entity.Cities.Where(x => x.Id == id).FirstOrDefault();
                cboCity.Text = city.CityName;
            }
            #endregion

           
            #region font
            if (string.IsNullOrEmpty(SettingController.DefaultFont))
            {
                SettingController.DefaultFont = cboLanguage.Text;
            }
            else
            {
                cboLanguage.Text = SettingController.DefaultFont;
            }
            #endregion
            #region Mode
            if (string.IsNullOrEmpty(SettingController.ApplicationMode))
            {
                SettingController.ApplicationMode = "Production";
               
            }
            #endregion
            #region DynamicPrice
            //chkDynamic.Checked = SettingController.AllowDynamicPrice;
            #endregion
            #region Pattern
            //if (SettingController.InventoryControlPattern=="fifo")
            //{
            //    rdoFiFo.Checked = true;
            //}
            //else if(SettingController.InventoryControlPattern == "lifo")
            //{
            //    rdoLiFo.Checked = true;
            //}
            //else if (SettingController.InventoryControlPattern == "fefo")
            //{
            //    rdoFeFo.Checked = true;
            //}
            #endregion
            #region ServiceFee
            //gbServiceCharge.Visible = SettingController.UseTable;
            //txtServiceFee.Text = SettingController.ServiceFee.ToString();
            //chkUseTable.Checked = SettingController.UseTable;
            //chkusequeue.Checked = SettingController.UseQueue;
            #endregion
            #region IdleDetection
            chkIdleDetect.Checked = SettingController.DetectIdle;
            txtIdleTime.Text = SettingController.IdleTime.ToString();
            txtIdleTime.ReadOnly = !SettingController.DetectIdle;

            #endregion
            #region TicketSale
            chkTicketSale.Checked = true;//SettingController.TicketSale;
            #endregion
            #region AllowMinimize
            chkAllowMinimize.Checked = SettingController.AllowMinimize;
            #endregion
            #region TopMost
            chkTopMost.Checked = SettingController.TopMost;
            #endregion

            txtNoOfCopy.Text = SettingController.DefaultNoOfCopies.ToString();

            string sysUIFormat = CultureInfo.CurrentUICulture.DateTimeFormat.ShortDatePattern;
            //if (sysUIFormat!= "dd-MM-yyyy")
            //{
            //    //MessageBox.Show(this, "Please change PC's Datetime format to dd/MM/yyyy","mPOS" ,MessageBoxButtons.OK,MessageBoxIcon.Information);
            //    return;
            //}
            dpCompanyStartDate.Format = DateTimePickerFormat.Custom;
            dpCompanyStartDate.CustomFormat = SettingController.GlobalDateFormat;

            dpCompanyStartDate.Value = Convert.ToDateTime(SettingController.Company_StartDate);
            txtDefaultSalesRow.Text = SettingController.DefaultTopSaleRow.ToString();

            chkUseStockAutoGenerate.Checked = SettingController.UseStockAutoGenerate;
            chkCustomSKU.Checked = SettingController.UseCustomSKU;
            chkUpper.Checked = SettingController.UpperCase_ProductName;
            chkProductImage.Checked = SettingController.ShowProductImageIn_A4Reports;
            chkBO.Checked = SettingController.IsBackOffice;
            txtFooterPage.Text = SettingController.FooterPage;

            
            if (SettingController.Logo != null && SettingController.Logo != "")
            {
                this.txtImagePath.Text = SettingController.Logo.ToString();
                string FileNmae = txtImagePath.Text.Substring(7);
                this.ptImage.ImageLocation = Application.StartupPath + "\\logo\\" + FileNmae;
                this.ptImage.SizeMode = PictureBoxSizeMode.Zoom;
            }
            if (SettingController.ApplicationMode=="Developer")
            {
                btnadvanced.Visible = true;
            }
            else
            {
                btnadvanced.Visible = false;
            }
           


        }

        private void txtNO_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            RoleManagementController controller = new RoleManagementController();
            controller.Load(MemberShip.UserRoleId);
            if (!controller.Setting.Add && !MemberShip.isAdmin)
            {
                MessageBox.Show("You are not allowed to define setting", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            
            Boolean hasError = false;

            tp.RemoveAll();
            tp.IsBalloon = true;
            tp.ToolTipIcon = ToolTipIcon.Error;
            tp.ToolTipTitle = "Error";
            //Validation
            //if (txtShopName.Text.Trim() == string.Empty)
            //{
            //    tp.SetToolTip(txtShopName, "Error");
            //    tp.Show("Please fill up shop name!", txtShopName);
            //    hasError = true;
            //}
            //else if (txtBranchName.Text.Trim() == string.Empty)
            //{
            //    tp.SetToolTip(txtBranchName, "Error");
            //    tp.Show("Please fill up branch name or address!", txtBranchName);
            //    hasError = true;
            //}
            //else if (txtPhoneNo.Text.Trim() == string.Empty)
            //{
            //    tp.SetToolTip(txtPhoneNo, "Error");
            //    tp.Show("Please fill up pnone number!", txtPhoneNo);
            //    hasError = true;
            //}
            //else if (txtOpeningHours.Text.Trim() == string.Empty)
            //{
            //    tp.SetToolTip(txtOpeningHours, "Error");
            //    tp.Show("Please fill up opening hour!", txtOpeningHours);
            //    hasError = true;
            //}

            //cboShopList.SelectedIndex = cboShopList.Items.IndexOf(SettingController.DefaultShop.ShopName);
            if (cboShopList.SelectedIndex < 0)
            {
               
                tabControl1.SelectedTab = ConfigurationPage;
                tabControl1.Refresh();
                tp.SetToolTip(cboShopList, "Error");
                tp.Show("Please choose default shop!", cboShopList);
                hasError = true;
            }
             if (dpCompanyStartDate.Value == null)
            {
                tabControl1.SelectedTab = ConfigurationPage;
                tabControl1.Refresh();
                tp.SetToolTip(dpCompanyStartDate, "Error");
                tp.Show("Please fill up Company Start Date!", dpCompanyStartDate);
                hasError = true;
            }

            else if (txtImagePath.Text.Trim() == string.Empty)
            {
                tabControl1.SelectedTab = PrinterSelection;
                tabControl1.Refresh();
                tp.SetToolTip(txtImagePath, "Error");
                tp.Show("Please browser Logo! (optional) ", txtImagePath);

                //hasError = true;
            }
            else if (ptImage == null)
            {
                tabControl1.SelectedTab = PrinterSelection;
                tabControl1.Refresh();
                tp.SetToolTip(ptImage, "Error");
                tp.Show("Please browser Logo!(optional)", ptImage);
                //hasError = true;
            }
            else if (rdoA4Printer.Checked)
            {
                if (cboA4Printer.Text.Trim() == string.Empty)
                {
                    tabControl1.SelectedTab = PrinterSelection;
                    tabControl1.Refresh();
                    tp.SetToolTip(cboA4Printer, "Error");
                    tp.Show("Please select A4 Printer!", cboA4Printer);
                    hasError = true;
                }
            }
             else if (txtNoOfCopy.Text.Trim() == string.Empty || Convert.ToInt32(txtNoOfCopy.Text) == 0)
             {
                 tabControl1.SelectedTab = PrinterSelection;
                 tabControl1.Refresh();
                 
                 tp.SetToolTip(txtNoOfCopy, "Error");
                 tp.Show("Please number of Copy!", txtNoOfCopy);
                 hasError = true;
             }
            else if (rdoSlipPrinter.Checked)
            {
                if (cboSlipPrinter.Text.Trim() == string.Empty)
                {
                    tabControl1.SelectedTab = PrinterSelection;
                    tabControl1.Refresh();
                    tp.SetToolTip(cboSlipPrinter, "Error");
                    tp.Show("Please select Slip Printer!", cboSlipPrinter);
                    hasError = true;
                }
            }
            
     
            if (!hasError)
            {

                DefaultPrinter.BarcodePrinter = cboBarcodePrinter.Text;
                DefaultPrinter.A4Printer = cboA4Printer.Text;
                DefaultPrinter.SlipPrinter = cboSlipPrinter.Text;

                SettingController.ShopName = txtShopName.Text;
                //SettingController.ShopName = txtShopName.Text;
                SettingController.BranchName = txtBranchName.Text;
                //SettingController.BranchName_Para = txtBranchName.Text;
                SettingController.PhoneNo = txtPhoneNo.Text;
                //SettingController.PhoneNo_Para = txtPhoneNo.Text;
                SettingController.OpeningHours = txtOpeningHours.Text;
                SettingController.Company_StartDate = dpCompanyStartDate.Value.ToString();
                //SettingController.DefaultTaxRate_Para = cboTaxList.SelectedValue.ToString();
                SettingController.UseStockAutoGenerate = chkUseStockAutoGenerate.Checked;
                SettingController.FooterPage = txtFooterPage.Text;
                SettingController.DefaultNoOfCopies = Convert.ToInt32(txtNoOfCopy.Text);

                //SettingController.AllowDynamicPrice = chkDynamic.Checked;
                //SettingController.AllowDynamicPrice_Para = chkDynamic.Checked;
                //SettingController.UseTable = chkUseTable.Checked;
                ////SettingController.UseTable_Para = chkUseTable.Checked;
                //SettingController.UseQueue = chkusequeue.Checked;
                //SettingController.UseQueue_Para = chkusequeue.Checked;
                //SettingController.ServiceFee = string.IsNullOrEmpty(txtServiceFee.Text)?0:Convert.ToInt32(txtServiceFee.Text);
                //SettingController.ServiceFee_Para = string.IsNullOrEmpty(txtServiceFee.Text) ? 0 : Convert.ToInt32(txtServiceFee.Text);
                SettingController.UseCustomSKU = chkCustomSKU.Checked;
                SettingController.TicketSale = chkTicketSale.Checked;
                //SettingController.TicketSale_Para = chkTicketSale.Checked;
                SettingController.UpperCase_ProductName = chkUpper.Checked;
                //SettingController.UpperCase_ProductName_Para = chkUpper.Checked;
                SettingController.ShowProductImageIn_A4Reports = chkProductImage.Checked;

                //SettingController.ShowProductImageIn_A4Reports = chkProductImage.Checked;
                SettingController.IsBackOffice = chkBO.Checked;

                if ((Application.OpenForms["Sales"] as Sales) != null)
                {
                    Sales form = (Application.OpenForms["Sales"] as Sales);
                    form.ForTicket();
                }
                    
                SettingController.AllowMinimize = chkAllowMinimize.Checked;
                SettingController.TopMost = chkTopMost.Checked;
                //SettingController.TopMost_Para = chkTopMost.Checked;
                //SettingController.InventoryControlPattern = rdoFiFo.Checked ? "fifo" : rdoLiFo.Checked ? "lifo" :rdoFeFo.Checked?"fefo":"fefo";
               // SettingController.InventoryControlPattern_Para = rdoFiFo.Checked ? "fifo" : rdoLiFo.Checked ? "lifo" : rdoFeFo.Checked ? "fefo" : "fefo";
                //SettingController.ApplicationMode = "Production";
                if (cboLanguage.Text != SettingController.DefaultFont || chkIdleDetect.Checked!=SettingController.DetectIdle || txtIdleTime.Text!=SettingController.IdleTime.ToString() )
                {

                    if (MessageBox.Show("Some setting changes you just have made require Application Restart", "mPOS", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                    {
                        SettingController.DetectIdle = chkIdleDetect.Checked;
                        SettingController.IdleTime = string.IsNullOrEmpty(txtIdleTime.Text) ? 1 : Convert.ToInt32(txtIdleTime.Text);
                        SettingController.DefaultFont = cboLanguage.Text;
                        Application.Restart();
                        //System.Diagnostics.Process.Start(Application.ExecutablePath);
                        //Application.Exit();
                    }
                }
                
                try
                {
                    File.Copy(txtImagePath.Text, Application.StartupPath + "\\logo\\" + FilePath);
                    SettingController.Logo = "~\\logo\\" + FilePath;
                   // SettingController.Logo_Para = "~\\logo\\" + FilePath;
                }
                catch
                {
                    if (FilePath != null)
                    {
                        SettingController.Logo = "~\\logo\\" + FilePath;
                        //SettingController.Logo_Para = "~\\logo\\" + FilePath;
                    }
                }


                if (rdoA4Printer.Checked)
                {
                    SettingController.SelectDefaultPrinter = rdoA4Printer.Text.ToString();
                    //SettingController.SelectDefaultPrinter_Para = rdoA4Printer.Text.ToString();
                }
                else
                {
                    SettingController.SelectDefaultPrinter = rdoSlipPrinter.Text.ToString();
                    //SettingController.SelectDefaultPrinter_Para = rdoSlipPrinter.Text.ToString();
                }


                //int Id = Convert.ToInt32(cboCurrency.SelectedValue);
                //SettingController.DefaultCurrency = Id;
                //SettingController.DefaultCurrency_Para = Id;
                int topcity = 0;
                Int32.TryParse(cboCity.SelectedValue.ToString(), out topcity);
                SettingController.DefaultCity = topcity;
                int topSalesRow = 0;
                Int32.TryParse(txtDefaultSalesRow.Text, out topSalesRow);
                SettingController.DefaultTopSaleRow = topSalesRow;

                List<APP_Data.Shop> shopList = entity.Shops.Where(x => x.IsDefaultShop == true).ToList();
                (from x in entity.Shops where x.IsDefaultShop == true select x).ToList().ForEach(x=> x.IsDefaultShop = false);

                entity.SaveChanges();

                int defaultShopId = Convert.ToInt32(cboShopList.SelectedValue);
               APP_Data.Shop defaultShop = entity.Shops.Where(x => x.Id == defaultShopId).FirstOrDefault();
                defaultShop.IsDefaultShop = true;
                //SettingController.DefaultShop  = defaultShop;
                entity.SaveChanges();
                Utility.generalEntity_Para = SettingController.generalEntity;

                MessageBox.Show("Successfully Saved!");

                if (Application.OpenForms["Sales"] != null)
                {
                    Sales form = (Sales)Application.OpenForms["Sales"];
                    form.PriceColumnControl();
                   // form.ForRestaurant();
                    
                }
                if (entity.ProductCategories.Where(a => a.Name == "Ticket").Count() == 0)//SettingController.TicketSale &&
                {
                    APP_Data.ProductCategory pc = new APP_Data.ProductCategory();
                    pc.Name = "Ticket";
                    pc.IsDelete = false;
                    entity.ProductCategories.Add(pc);
                    entity.SaveChanges();

                    for (int i = 0; i < 4; i++)
                    {
                        APP_Data.ProductSubCategory psc = new APP_Data.ProductSubCategory();
                        switch (i)
                        {
                            case 1: psc.Name = "LocalAdult"; break;
                            case 2: psc.Name = "LocalChild"; break;
                            case 3: psc.Name = "ForeignAdult"; break;
                            case 4: psc.Name = "ForeignChild"; break;
                        }

                        psc.IsDelete = false;
                        psc.ProductCategoryId = pc.Id;
                        psc.Prefix = null;
                    }


                }
                this.Dispose();



            }
        }

        
        private void btnNewCity_Click(object sender, EventArgs e)
        {
            City newForm = new City();
            newForm.ShowDialog();
        }

        #region Function
        public void ReLoadCity()
        {
            #region city
            List<APP_Data.City> cityList = entity.Cities.ToList();
            cboCity.DataSource = cityList;
            cboCity.DisplayMember = "CityName";
            cboCity.ValueMember = "Id";
            if (SettingController.DefaultCity != 0)
            {
                int id = Convert.ToInt32(SettingController.DefaultCity);
                APP_Data.City city = entity.Cities.Where(x => x.Id == id).FirstOrDefault();
                cboCity.Text = city.CityName;
            }
            #endregion
        }

        public void SetCurrentCity(Int32 CityId)
        {
            APP_Data.City currentCity = entity.Cities.Where(x => x.Id == CityId).FirstOrDefault();
            if (currentCity != null)
            {
                cboCity.SelectedValue = currentCity.Id;
            }
        }

     

     
        #endregion

        private void Setting_MouseMove(object sender, MouseEventArgs e)
        {
            tp.Hide(txtShopName);
            tp.Hide(txtBranchName);
            tp.Hide(txtPhoneNo);
            tp.Hide(txtOpeningHours);
            tp.Hide(dpCompanyStartDate);
            tp.Hide(cboA4Printer);
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title = "Open Picture";
            dlg.Filter = "JPEGs (*.jpg;*.jpeg;*.jpe) |*.jpg;*.jpeg;*.jpe |GIFs (*.gif)|*.gif|PNGs (*.png)|*.png";

            try
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    txtImagePath.Text = dlg.FileName;
                    ptImage.SizeMode = PictureBoxSizeMode.StretchImage;
                    ptImage.ImageLocation = txtImagePath.Text;
                    FilePath = System.IO.Path.GetFileName(dlg.FileName);

                }

            }
            catch (Exception ex)
            {
                //MessageBox.ShowMessage(Globalizer.MessageType.Warning, "You have to select Picture.\n Try again!!!");
                MessageBox.Show("You have to select Picture.\n Try again!!!");
                throw ex;
            }
        }

        private void cboShopList_SelectedIndexChanged(object sender, EventArgs e)
        {
            APP_Data.Shop currentShop = (APP_Data.Shop)cboShopList.SelectedItem;
            txtShopName.Text = currentShop.ShopName;
            txtBranchName.Text = currentShop.Address;
            txtOpeningHours.Text = currentShop.OpeningHours;
            txtPhoneNo.Text = currentShop.PhoneNumber;
            lblCity.Text = currentShop.City.CityName;
        }

        private void btnadvanced_Click(object sender, EventArgs e)
        {
            if (pnlAdvanced.Visible)
                pnlAdvanced.Visible = false;
            else
                pnlAdvanced.Visible = true;

            if (pnlAdvanced.Visible)
            {
                chkSourceCode.Checked = Convert.ToBoolean(SettingController.IsSourcecode);
                txtPOSID.Text = SettingController.POSID.ToString();

                //chkUseTable.Checked = SettingController.UseTable;
                //chkusequeue.Checked = SettingController.UseQueue;
               // txtServiceFee.Text = SettingController.ServiceFee.ToString();
            }
        }

        private void chkSourceCode_CheckedChanged(object sender, EventArgs e)
        {
            SettingController.IsSourcecode = chkSourceCode.Checked;
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            pnlAdvanced.Visible = false;
        }

        private void chkDynamic_CheckedChanged(object sender, EventArgs e)
        {
            if (!MemberShip.isAdmin)
            {
                MessageBox.Show("This setting is only allowed to Admin.", "mPOS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        //private void chkUseTable_CheckedChanged(object sender, EventArgs e)
        //{
        //    //gbServiceCharge.Visible = chkUseTable.Checked;
     
        //    chkusequeue.Checked = false;
        //}

        //private void chkusequeue_CheckedChanged(object sender, EventArgs e)
        //{
        //    //gbServiceCharge.Visible = chkusequeue.Checked;
         
        //    chkUseTable.Checked = false;
        //}

        private void chkIdleDetect_CheckedChanged(object sender, EventArgs e)
        {
            txtIdleTime.ReadOnly = !chkIdleDetect.Checked;
        }
    }
    
}
