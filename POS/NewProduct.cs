using POS.APP_Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace POS
{
    public partial class NewProduct : Form
    {
        #region Variables
        //string file;
        private String FilePath;
        TextBox Qtycode = new TextBox();

        public Boolean isEdit { get; set; }
        public Boolean IsReference = false;
        public int ProductId { get; set; }

        private POSEntities entity = new POSEntities();

        private ToolTip tp = new ToolTip();

        //List<wproductlist> productList = new List<wproductlist>();


        Product currentProduct;
        //List<Stock_Transaction> product_List = new List<Stock_Transaction>();
        bool IsStockAutoGenerate = false;
        #endregion

        #region Event

        public NewProduct()
        {
            InitializeComponent();
        }

        private void NewProduct_Load(object sender, EventArgs e)
        {
            Localization.Localize_FormControls(this);
            if (SettingController.UseCustomSKU)
            {
                txtProductCode.Enabled = true;
            }
            if (SettingController.UpperCase_ProductName)
            {
                txtName.CharacterCasing = CharacterCasing.Upper;
            }
            else
            {
                txtName.CharacterCasing = CharacterCasing.Normal;
            }
            bool notbackoffice = Utility.IsNotBackOffice();
            if (notbackoffice)
            {
                btnNewBrand.Enabled = false;
                btnNewCategory.Enabled = false;
                btnNewSubCategofry.Enabled = false;
               
            }
            else
            {
                btnNewBrand.Enabled = true;
                btnNewCategory.Enabled = true;
                btnNewSubCategofry.Enabled = true;
              
            }
            Utility.Check_AddFOCInCag();

            IsStockAutoGenerate = SettingController.UseStockAutoGenerate;
            if (IsStockAutoGenerate == true)
            {
                this.ActiveControl = txtName;
                txtBarcode.Enabled = false;
            }
            else
            {
                txtBarcode.Enabled = true;
                this.ActiveControl = txtBarcode;
            }
            //txtName.Focus();
            #region Setting Hot Kyes For the Controls
            SendKeys.Send("%"); SendKeys.Send("%"); // Clicking "Alt" on page load to show underline of Hot Keys
            this.KeyPreview = true;
            this.KeyDown += new KeyEventHandler(Form_KeyDown);
            #endregion

            // chkSpecialPromotion.Enabled = false;
            //   txtName.Focused = true;
            //txtBarcode.ReadOnly = true;
            //txtProductCode.ReadOnly = true;
            Utility.BindReferenceProduct(cboReferenceProductName);
            // IsReference = true;

            List<APP_Data.Brand> BrandList = new List<APP_Data.Brand>();
            APP_Data.Brand brandObj1 = new APP_Data.Brand();
            brandObj1.Id = 0;
            brandObj1.Name = "Select";
            //APP_Data.Brand brandObj2 = new APP_Data.Brand();
            //brandObj2.Id = 1;
            //brandObj2.Name = "None";
            BrandList.Add(brandObj1);
            //BrandList.Add(brandObj2);
            BrandList.AddRange((from bList in entity.Brands where bList.IsDelete == false select bList).ToList());
            cboBrand.DataSource = BrandList;
            cboBrand.DisplayMember = "Name";
            cboBrand.ValueMember = "Id";
            cboBrand.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboBrand.AutoCompleteSource = AutoCompleteSource.ListItems;

            List<APP_Data.ProductSubCategory> pSubCatList = new List<APP_Data.ProductSubCategory>();
            APP_Data.ProductSubCategory SubCategoryObj1 = new APP_Data.ProductSubCategory();
            SubCategoryObj1.Id = 0;
            SubCategoryObj1.Name = "Select";
            pSubCatList.Add(SubCategoryObj1);


            //APP_Data.ProductSubCategory SubCategoryObj2 = new APP_Data.ProductSubCategory();
            //SubCategoryObj2.Id = 1;
            //SubCategoryObj2.Name = "None";
            //pSubCatList.AddRange((from c in entity.ProductSubCategories where c.ProductCategoryId == Convert.ToInt32(cboMainCategory.SelectedValue) select c).ToList());
            cboSubCategory.DataSource = pSubCatList;
            cboSubCategory.DisplayMember = "Name";
            cboSubCategory.ValueMember = "Id";


            List<APP_Data.ProductCategory> pMainCatList = new List<APP_Data.ProductCategory>();
            APP_Data.ProductCategory MainCategoryObj1 = new APP_Data.ProductCategory();
            MainCategoryObj1.Id = 0;
            MainCategoryObj1.Name = "Select";
            //APP_Data.ProductCategory MainCategoryObj2 = new APP_Data.ProductCategory();
            //MainCategoryObj2.Id = 1;
            //MainCategoryObj2.Name = "None";
            pMainCatList.Add(MainCategoryObj1);
            //pMainCatList.Add(MainCategoryObj2);
            pMainCatList.AddRange((from MainCategory in entity.ProductCategories where MainCategory.IsDelete == false select MainCategory).ToList());
            cboMainCategory.DataSource = pMainCatList;
            cboMainCategory.DisplayMember = "Name";
            cboMainCategory.ValueMember = "Id";
            cboMainCategory.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboMainCategory.AutoCompleteSource = AutoCompleteSource.ListItems;
            //productList.Clear();
            if (isEdit)
            {
                //chkIsConsignment.Enabled = false;
                //Editing here
                currentProduct = (from p in entity.Products where p.Id == ProductId select p).FirstOrDefault();
                txtBarcode.Text = currentProduct.Barcode;
                txtProductCode.Text = currentProduct.ProductCode;
                txtName.Text = currentProduct.Name;
                txtUnitPrice.Text = currentProduct.Price.ToString();
                if (currentProduct.Brand != null)
                {
                    cboBrand.Text = currentProduct.Brand.Name;

                }
                //else
                //{
                //    cboBrand.Text = "None";
                //}
                if (currentProduct.ProductCategory != null)
                {
                    cboMainCategory.Text = currentProduct.ProductCategory.Name;
                    if (currentProduct.ProductSubCategory != null)
                    {
                        cboSubCategory.Text = currentProduct.ProductSubCategory.Name;
                    }
                    //else
                    //{
                    //    cboSubCategory.Text = "None";
                    //}
                    cboSubCategory.Enabled = true;

                }
                else
                {
                    cboMainCategory.Text = "Select";
                    cboSubCategory.Enabled = false;
                }
                txtDiscount.Text = currentProduct.DiscountRate.ToString();

               
                

                //product image
                if (currentProduct.PhotoPath != null && currentProduct.PhotoPath != "")
                {
                    this.txtImagePath.Text = currentProduct.PhotoPath.ToString();
                    string FileNmae = txtImagePath.Text.Substring(9);
                    this.ptImage.ImageLocation = Application.StartupPath + "\\Images\\" + FileNmae;
                    this.ptImage.SizeMode = PictureBoxSizeMode.Zoom;
                }
                btnSubmit.Image = POS.Properties.Resources.update_big;

                // cboMainCategory.Enabled = false;
                // cboSubCategory.Enabled = false;
                // btnNewCategory.Enabled = false;
                // btnNewSubCategofry.Enabled = false;
            }
            else
            {
               txtUnitPrice.Text = "0";
            }
            if (notbackoffice == false)
            {
                var bndi1 = cboBrand.SelectedValue;
                var gi1 = cboMainCategory.SelectedValue;
                var bcgid1 = cboSubCategory.SelectedValue;

            }


            var brandid1 = cboBrand.SelectedValue;
            var cagid1 = cboMainCategory.SelectedValue;
            var subcagid1 = cboSubCategory.SelectedValue;
        }

       

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            Boolean hasError = false;
            //   POSEntities entity = new POSEntities();

            tp.RemoveAll();
            tp.IsBalloon = true;
            tp.ToolTipIcon = ToolTipIcon.Error;
            tp.ToolTipTitle = "Error";
            
            //Validation
            if (txtBarcode.Text.Trim() == string.Empty)
            {
                tp.SetToolTip(txtBarcode, "Error");
                tp.Show("Please fill up barcode!", txtBarcode);
                txtBarcode.Focus();
                hasError = true;
            }
            else if (txtProductCode.Text.Trim() == string.Empty)
            {
                tp.SetToolTip(txtProductCode, "Error");
                tp.Show("Please fill up product code!", txtProductCode);
                txtProductCode.Focus();
                hasError = true;
            }
            else if (txtName.Text.Trim() == string.Empty)
            {
                tp.SetToolTip(txtName, "Error");
                tp.Show("Please fill up product name!", txtName);
                txtName.Focus();
                hasError = true;
            }

            else if (Utility.Brand_Combo_Control(cboBrand))
            {
                cboBrand.Focus();
                return;
            }
            else if (Utility.MainCategory_Combo_Control(cboMainCategory))
            {
                cboMainCategory.Focus();
                return;
            }
            //else if (Utility.SubCategory_Combo_Control(cboSubCategory))
            //{
            //    cboSubCategory.Focus();
            //    return;
            //}
           

            else if (cboBrand.SelectedIndex == 0)
            {
                tp.SetToolTip(cboBrand, "Error");
                tp.Show("Please select brand name!", cboBrand);
                cboBrand.Focus();
                hasError = true;
            }
            
            else if (cboMainCategory.SelectedIndex == 0)
            {
                tp.SetToolTip(cboMainCategory, "Error");
                tp.Show("Please select main category name!", cboMainCategory);
                cboMainCategory.Focus();
                hasError = true;
            }
            ////////else if (cboMainCategory.SelectedIndex > 0 && cboSubCategory.SelectedIndex == 0)
            ////////{
            ////////    tp.SetToolTip(cboSubCategory, "Error");
            ////////    tp.Show("Please select sub category name!", cboSubCategory);
            ////////    cboSubCategory.Focus();
            ////////    hasError = true;
            ////////}
            else if (txtDiscount.Text.Trim() != string.Empty && Convert.ToDouble(txtDiscount.Text) > 100.00)
            {
                tp.SetToolTip(txtDiscount, "Error");
                tp.Show("Discount percent must not over 100!", txtDiscount);
                txtDiscount.Focus();
                hasError = true;
            }

           
            else if (cboMainCategory.Text != "FOC")
            {
                if (txtUnitPrice.Text.Trim() == string.Empty || txtUnitPrice.Text == "0")
                {
                    tp.SetToolTip(txtUnitPrice, "Error");
                    tp.Show("Please fill up product price!", txtUnitPrice);
                    txtUnitPrice.Focus();
                    hasError = true;
                }
            }
            //else if (txtWholeSalePrice.Text.Trim() == string.Empty || txtWholeSalePrice.Text == "0")
            //{
            //    tp.SetToolTip(txtWholeSalePrice, "Error");
            //    tp.Show("Please fill up whole sale price!", txtWholeSalePrice);
            //    txtWholeSalePrice.Focus();
            //    hasError = true;
            //}

            if (!hasError)
            {
                //Edit product
                if (isEdit)
                {
                    DialogResult result = MessageBox.Show("Are you sure you want to update?", "Update", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                    if (result.Equals(DialogResult.OK))
                    {

                        APP_Data.Product editProductObj = (from p in entity.Products where p.Id == ProductId select p).FirstOrDefault();
                        int oldPrice = Convert.ToInt32(editProductObj.Price); int currentPrice = 0; int differentPrice = 0;
                        int ProductCodeCount = 0, BarcodeCount = 0, ProductNameCount = 0;
                        //count = (from p in entity.Products where p.Name == txtName.Text select p).ToList().Count;
                        if (txtProductCode.Text.Trim() != editProductObj.ProductCode.Trim())
                        {
                            ProductCodeCount = (from p in entity.Products where p.ProductCode.Trim() == txtProductCode.Text.Trim() select p).ToList().Count;
                        }
                        if (txtBarcode.Text.Trim() != editProductObj.Barcode.Trim())
                        {
                            BarcodeCount = (from p in entity.Products where p.Barcode.Trim() == txtBarcode.Text.Trim() select p).ToList().Count;
                        }
                        if (txtName.Text.Trim() != editProductObj.Name.Trim())
                        {
                            ProductNameCount = (from p in entity.Products where p.Name.Trim() == txtName.Text.Trim() select p).ToList().Count;
                        }

                        if (ProductNameCount > 0)
                        {
                            tp.SetToolTip(txtName, "Error");
                            tp.Show("Product Name is already exist!", txtName);
                            return;
                        }
                        if (ProductCodeCount == 0 && BarcodeCount == 0)
                        {

                            editProductObj.Barcode = txtBarcode.Text.Trim();
                            editProductObj.ProductCode = txtProductCode.Text.Trim();
                            editProductObj.Name = txtName.Text.Trim();

                            editProductObj.BrandId = Convert.ToInt32(cboBrand.SelectedValue.ToString());

                            txtUnitPrice.Text = txtUnitPrice.Text.Trim().Replace(",", "");

                            editProductObj.UpdatedBy = MemberShip.UserId;
                            editProductObj.UpdatedDate = DateTime.Now;
                            editProductObj.Price = Convert.ToInt32(txtUnitPrice.Text);



                            //get price different
                            currentPrice = Convert.ToInt32(txtUnitPrice.Text);
                            differentPrice = currentPrice - oldPrice;

                            //product image

                            if (!(string.IsNullOrEmpty(this.txtImagePath.Text.Trim())))
                            {
                                try
                                {
                                    File.Copy(txtImagePath.Text, Application.StartupPath + "\\Images\\" + FilePath);
                                    editProductObj.PhotoPath = "~\\Images\\" + FilePath;
                                }
                                catch
                                {
                                    editProductObj.PhotoPath = "~\\Images\\" + FilePath;
                                }
                            }
                            else
                            {

                                editProductObj.PhotoPath = string.Empty;
                            }


                            if (txtDiscount.Text.Trim() == string.Empty)
                            {
                                editProductObj.DiscountRate = 0;
                            }
                            else
                            {
                                editProductObj.DiscountRate = Convert.ToDecimal(txtDiscount.Text);
                            }


                            //if (chkIsConsignment.Checked)
                            //{
                            //    productObj.Qty = Convert.ToInt32(txtToalConQty.Text);
                            //}
                            //if minstock qty is null, add default value

                            editProductObj.ProductCategoryId = Convert.ToInt32(cboMainCategory.SelectedValue);

                            if (cboSubCategory.SelectedIndex > 0)
                            {
                                editProductObj.ProductSubCategoryId = Convert.ToInt32(cboSubCategory.SelectedValue);
                            }

                            //product_List.Clear();
                        }

                        else if (BarcodeCount > 0)
                        {
                            tp.SetToolTip(txtBarcode, "Error");
                            tp.Show("This barcode is already exist!", txtBarcode);

                        }
                        else if (ProductCodeCount < 0)
                        {
                            tp.SetToolTip(txtProductCode, "Error");
                            tp.Show("This product code is already exist!", txtProductCode);
                        }
                    }
                }

            }
            //add new product
            else
            {
                int ProductCodeCount = 0, BarcodeCount = 0, ProductNameCount = 0;
                //count = (from p in entity.Products where p.Name == txtName.Text select p).ToList().Count;
                ProductCodeCount = (from p in entity.Products where p.ProductCode.Trim() == txtProductCode.Text.Trim() select p).ToList().Count;
                BarcodeCount = (from p in entity.Products where p.Barcode.Trim() == txtBarcode.Text.Trim() select p).ToList().Count;
                ProductNameCount = (from p in entity.Products where p.Name.Trim() == txtName.Text.Trim() select p).ToList().Count;

                if (ProductNameCount > 0)
                {
                    tp.SetToolTip(txtName, "Error");
                    tp.Show("Product Name is already exist!", txtName);
                    return;
                }
                if (ProductCodeCount == 0 && BarcodeCount == 0)
                {
                    APP_Data.Product productObj = new APP_Data.Product();

                    productObj.Barcode = txtBarcode.Text.Trim();
                    productObj.ProductCode = txtProductCode.Text.Trim();

                    productObj.Name = txtName.Text.Trim();

                    productObj.BrandId = Convert.ToInt32(cboBrand.SelectedValue.ToString());

                    txtUnitPrice.Text = txtUnitPrice.Text.Trim().Replace(",", "");
                    productObj.CreatedBy = MemberShip.UserId;
                    productObj.CreatedDate = DateTime.Now;
                    productObj.UpdatedBy = MemberShip.UserId;
                    productObj.UpdatedDate = DateTime.Now;
                    productObj.Price = Convert.ToInt32(txtUnitPrice.Text);
                    //if discount is null, add default value
                    if (txtDiscount.Text.Trim() == string.Empty)
                    {
                        productObj.DiscountRate = 0;
                    }
                    else
                    {
                        productObj.DiscountRate = Convert.ToDecimal(txtDiscount.Text);
                    }
                  

                    productObj.ProductCategoryId = Convert.ToInt32(cboMainCategory.SelectedValue);

                    if (cboSubCategory.SelectedIndex > 0)
                    {
                        productObj.ProductSubCategoryId = Convert.ToInt32(cboSubCategory.SelectedValue);
                    }

                    //product photo
                    if (!(string.IsNullOrEmpty(this.txtImagePath.Text.Trim())))
                    {
                        try
                        {
                            File.Copy(txtImagePath.Text, Application.StartupPath + "\\Images\\" + FilePath);

                            productObj.PhotoPath = "~\\Images\\" + FilePath;
                        }
                        catch
                        {
                            productObj.PhotoPath = "~\\Images\\" + FilePath;
                        }
                    }
                    else
                    {
                        productObj.PhotoPath = string.Empty;
                    }


                    //if (productObj.MinStockQty != null)
                    //{
                    //if (productObj.Qty > productObj.MinStockQty || chkMinStock.Checked == false)
                    //{
                    entity.Products.Add(productObj);
                    entity.SaveChanges();


                    #region Update AutoGenerateNo. in brand table
                    int _brandId = Convert.ToInt32(cboBrand.SelectedValue);
                    var _brandData = (from b in entity.Brands where b.Id == _brandId select b).FirstOrDefault();
                    if (_brandData.AutoGenerateNo == null)
                    {
                        _brandData.AutoGenerateNo = 1;
                    }
                    else
                    {
                        _brandData.AutoGenerateNo += 1;
                    }
                    entity.Entry(_brandData).State = EntityState.Modified;
                    //entity.Entry(_brandData).State = System.Data.EntityState.Modified;
                    entity.SaveChanges();
                    #endregion



                    MessageBox.Show("Successfully Saved!", "Save");

                    if (System.Windows.Forms.Application.OpenForms["ItemList"] != null)
                    {
                        ItemList newForm = (ItemList)System.Windows.Forms.Application.OpenForms["ItemList"];
                        newForm.DataBind();
                    }
                    if (System.Windows.Forms.Application.OpenForms["Sales"] != null)
                    {
                        Sales newForm = (Sales)System.Windows.Forms.Application.OpenForms["Sales"];
                        newForm.Clear();
                    }

                    ClearInputs();

                }
                else if (BarcodeCount > 0)
                {
                    tp.SetToolTip(txtBarcode, "Error");
                    tp.Show("This barcode is already exist!", txtBarcode);
                }
                else if (ProductCodeCount > 0)
                {
                    tp.SetToolTip(txtProductCode, "Error");
                    tp.Show("This product code is already exist!", txtProductCode);
                }
            }
            List<Product> productList1 = new List<Product>();

            Product productObj1 = new Product();
            productObj1.Name = "Select";
            productObj1.Id = 0;
            productList1.Add(productObj1);
            productList1.AddRange((from pList in entity.Products select pList).ToList());
            Utility.BindReferenceProduct(cboReferenceProductName);
            chkUseReference.CheckedChanged += new EventHandler(chkUseReference_CheckedChanged);

        }


        private Image bytearraytoimage(byte[] b)
        {
            MemoryStream ms = new MemoryStream(b);
            Image img = Image.FromStream(ms);
            return img;
        }

        private void cboMainCategory_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cboMainCategory.SelectedIndex > 0)
            {
                int productCategoryId = Int32.Parse(cboMainCategory.SelectedValue.ToString());
                List<APP_Data.ProductSubCategory> pSubCatList = new List<APP_Data.ProductSubCategory>();
                APP_Data.ProductSubCategory SubCategoryObj1 = new APP_Data.ProductSubCategory();
                SubCategoryObj1.Id = 0;
                SubCategoryObj1.Name = "Select";
                APP_Data.ProductSubCategory SubCategoryObj2 = new APP_Data.ProductSubCategory();
                //SubCategoryObj2.Id = 1;
                //SubCategoryObj2.Name = "None";
                pSubCatList.Add(SubCategoryObj1);
                //  pSubCatList.Add(SubCategoryObj2);
                pSubCatList.AddRange((from c in entity.ProductSubCategories where c.ProductCategoryId == productCategoryId select c).ToList());
                cboSubCategory.DataSource = pSubCatList;
                cboSubCategory.DisplayMember = "Name";
                cboSubCategory.ValueMember = "Id";
                cboSubCategory.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                cboSubCategory.AutoCompleteSource = AutoCompleteSource.ListItems;
                cboSubCategory.Enabled = true;
            }
            else
            {
                cboSubCategory.SelectedIndex = 0;
                cboSubCategory.Enabled = false;
            }
        }


        private void NewProduct_MouseMove(object sender, MouseEventArgs e)
        {
            tp.Hide(txtName);
            tp.Hide(txtUnitPrice);
            tp.Hide(cboBrand);
            tp.Hide(cboMainCategory);
            tp.Hide(cboSubCategory);
            tp.Hide(txtBarcode);
        }

        private void txtUnitPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }
        }

        private void txtDiscount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            txtBarcode.Text = "";
            txtProductCode.Text = "";
            txtDiscount.Text = "";
            txtName.Text = "";
            txtUnitPrice.Text = "";
            cboBrand.SelectedIndex = 0;
            cboMainCategory.SelectedIndex = 0;
            cboSubCategory.SelectedIndex = 0;
            cboSubCategory.Enabled = false;
            btnSubmit.Image = POS.Properties.Resources.save_big;
            cboReferenceProductName.SelectedIndex = 0;


        }


        private void btnNewBrand_Click(object sender, EventArgs e)
        {
            Brand newForm = new Brand();
            newForm.ShowDialog();
        }

        private void btnNewCategory_Click(object sender, EventArgs e)
        {
            ProductCategory newForm = new ProductCategory();
            newForm.ShowDialog();
        }

        private void btnNewSubCategofry_Click(object sender, EventArgs e)
        {
            ProductSubCategory newFrom = new ProductSubCategory();
            newFrom.ShowDialog();
        }
        #endregion

        #region Function
        private void Clear_Control()
        {
            txtBarcode.Text = "";
            txtProductCode.Text = "";
          
            txtDiscount.Text = "";
            txtName.Text = "";
            txtUnitPrice.Text = "";
            cboBrand.SelectedIndex = 0;
            cboMainCategory.SelectedIndex = 0;
            cboSubCategory.SelectedIndex = 0;
            cboSubCategory.Enabled = false;
           
            txtImagePath.Clear();
            ptImage.Image = null;
        }

        private void ClearInputs()
        {
            txtBarcode.Text = "";
            txtProductCode.Text = "";
            txtDiscount.Text = "";
            txtName.Text = "";
            txtUnitPrice.Text = "";
            cboBrand.SelectedIndex = 0;
            cboMainCategory.SelectedIndex = 0;
            cboSubCategory.SelectedIndex = 0;
            cboSubCategory.Enabled = false;
            txtImagePath.Clear();
            ptImage.Image = null;

            chkUseReference.Checked = false;
            cboReferenceProductName.SelectedIndex = -1;
            UseReference(false);
        }

        public void ReloadBrand()
        {
            entity = new POSEntities();
            List<APP_Data.Brand> BrandList = new List<APP_Data.Brand>();

            APP_Data.Brand brandObj1 = new APP_Data.Brand();
            brandObj1.Id = 0;
            brandObj1.Name = "Select";

            BrandList.Add(brandObj1);

            BrandList.AddRange((from bList in entity.Brands select bList).ToList());
            cboBrand.DataSource = BrandList;
            cboBrand.DisplayMember = "Name";
            cboBrand.ValueMember = "Id";
            cboBrand.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboBrand.AutoCompleteSource = AutoCompleteSource.ListItems;
            if (isEdit)
            {
                if (currentProduct.Brand != null)
                {
                    cboBrand.Text = currentProduct.Brand.Name;
                }
                //else
                //{
                //    cboBrand.Text = "None";
                //}
            }
        }
        public void SetCurrentBrand(Int32 BrandId)
        {
            APP_Data.Brand currentBrand = entity.Brands.Where(x => x.Id == BrandId).FirstOrDefault();
            if (currentBrand != null)
            {
                cboBrand.SelectedValue = currentBrand.Id;
            }
        }

        public void SetCurrentCategory(Int32 CategoryId)
        {
            APP_Data.ProductCategory currentCategory = entity.ProductCategories.Where(x => x.Id == CategoryId).FirstOrDefault();
            if (currentCategory != null)
            {
                cboMainCategory.SelectedValue = currentCategory.Id;
            }
        }

        public void SetCurrentSubCategory(Int32 SubCategoryId)
        {
            APP_Data.ProductSubCategory currentSubCategory = entity.ProductSubCategories.Where(x => x.Id == SubCategoryId).FirstOrDefault();
            if (currentSubCategory != null)
            {
                cboSubCategory.Text = currentSubCategory.Name;
            }
        }


        public void ReloadCategory()
        {
            entity = new POSEntities();
            List<APP_Data.ProductSubCategory> pSubCatList = new List<APP_Data.ProductSubCategory>();

            APP_Data.ProductSubCategory SubCategoryObj1 = new APP_Data.ProductSubCategory();
            SubCategoryObj1.Id = 0;
            SubCategoryObj1.Name = "Select";
            pSubCatList.Add(SubCategoryObj1);

            cboSubCategory.DataSource = pSubCatList;
            cboSubCategory.DisplayMember = "Name";
            cboSubCategory.ValueMember = "Id";

            List<APP_Data.ProductCategory> pMainCatList = new List<APP_Data.ProductCategory>();
            //var pMainCatList = (from maincag in entity.ProductCategories select maincag).ToList();
            APP_Data.ProductCategory MainCategoryObj1 = new APP_Data.ProductCategory();
            MainCategoryObj1.Id = 0;
            MainCategoryObj1.Name = "Select";

            pMainCatList.Add(MainCategoryObj1);

            pMainCatList.AddRange((from MainCategory in entity.ProductCategories select MainCategory).ToList());
            cboMainCategory.DataSource = pMainCatList;
            cboMainCategory.DisplayMember = "Name";
            cboMainCategory.ValueMember = "Id";
            cboMainCategory.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboMainCategory.AutoCompleteSource = AutoCompleteSource.ListItems;


            if (isEdit)
            {
                if (currentProduct.ProductCategory != null)
                {
                    cboMainCategory.Text = currentProduct.ProductCategory.Name;
                    if (currentProduct.ProductSubCategory != null)
                    {
                        cboSubCategory.Text = currentProduct.ProductSubCategory.Name;
                    }
                    //else
                    //{
                    //    cboSubCategory.Text = "None";
                    //}
                    cboSubCategory.Enabled = true;
                }
                else
                {
                    cboMainCategory.Text = "Select";
                    cboSubCategory.Enabled = false;
                }
            }
        }
        public void ReloadSubCategory()
        {
            List<APP_Data.ProductSubCategory> pSubCatList = new List<APP_Data.ProductSubCategory>();
            APP_Data.ProductSubCategory SubCategoryObj1 = new APP_Data.ProductSubCategory();
            SubCategoryObj1.Id = 0;
            SubCategoryObj1.Name = "Select";
            pSubCatList.Add(SubCategoryObj1);

            cboSubCategory.DataSource = pSubCatList;
            cboSubCategory.DisplayMember = "Name";
            cboSubCategory.ValueMember = "Id";

            List<APP_Data.ProductCategory> pMainCatList = new List<APP_Data.ProductCategory>();
            APP_Data.ProductCategory MainCategoryObj1 = new APP_Data.ProductCategory();
            MainCategoryObj1.Id = 0;
            MainCategoryObj1.Name = "Select";

            pMainCatList.Add(MainCategoryObj1);

            pMainCatList.AddRange((from MainCategory in entity.ProductCategories select MainCategory).ToList());
            cboMainCategory.DataSource = pMainCatList;
            cboMainCategory.DisplayMember = "Name";
            cboMainCategory.ValueMember = "Id";
            cboMainCategory.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboMainCategory.AutoCompleteSource = AutoCompleteSource.ListItems;
            if (isEdit)
            {
                if (currentProduct.ProductCategory != null)
                {
                    cboMainCategory.Text = currentProduct.ProductCategory.Name;
                    if (currentProduct.ProductSubCategory != null)
                    {
                        cboSubCategory.Text = currentProduct.ProductSubCategory.Name;
                    }
                    //else
                    //{
                    //    cboSubCategory.Text = "None";
                    //}
                    cboSubCategory.Enabled = true;
                }
                else
                {
                    cboMainCategory.Text = "Select";
                    cboSubCategory.Enabled = false;
                }
            }
        }

        #endregion





        private void cboMainCategory_SelectedIndexChanged(object sender, EventArgs e)
        {


            if (cboMainCategory.Text == "FOC")
            {
                cboSubCategory.Text = "FOC";
                txtUnitPrice.Text = "0";
              
            }
        }

        private void txtBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtProductCode.Text = txtBarcode.Text.ToString();
                txtName.Focus();
            }
        }

        private void txtBarcode_Leave(object sender, EventArgs e)
        {
            txtProductCode.Text = txtBarcode.Text.ToString();
            txtName.Focus();
        }

        private void txtAmt_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

      


        private void txtDiscount_Leave(object sender, EventArgs e)
        {
            //chechk_symbol(txtDiscount);
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

        public bool ThumbnailCallback()
        {
            return false;
        }


        public byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            return ms.ToArray();
        }

        private void chkUseReference_CheckedChanged(object sender, EventArgs e)
        {
            if (chkUseReference.Checked)
            {
                UseReference(true);
            }
            else
            {
                UseReference(false);
            }
        }

        private void UseReference(bool _ref)
        {
            lblReferenceProductName.Enabled = _ref;
            cboReferenceProductName.Enabled = _ref;
            IsReference = _ref;
        }

        private void cboReferenceProductName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (IsReference)
            {
                entity = new POSEntities();
                if (cboReferenceProductName.SelectedIndex > 0)
                {
                    int _productId = Convert.ToInt32(cboReferenceProductName.SelectedValue);
                    var _referenceData = (from pro in entity.Products where pro.Id == _productId select pro).FirstOrDefault();
                    txtName.Text = _referenceData.Name;
                    txtUnitPrice.Text = _referenceData.Price.ToString();
                     cboBrand.SelectedValue = _referenceData.BrandId;
                    if (_referenceData.ProductCategoryId != null)
                    {
                        cboMainCategory.SelectedValue = _referenceData.ProductCategoryId;
                    }

                    if (_referenceData.ProductSubCategoryId != null)
                    {
                        //cboSubCategory.SelectedValue = _referenceData.ProductSubCategoryId;
                        cboSubCategory.Text = _referenceData.ProductSubCategory.Name;
                    }
                  
                }
                else
                {
                    Clear_Control();
                }
            }

        }

        void Form_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)      //  Ctrl + M => Click Save
            {
                btnSubmit.PerformClick();
            }
            else if (e.Control && e.KeyCode == Keys.A) //  Ctrl +A => Click Cancel
            {
                btnCancel.PerformClick();
            }
            else if (e.Control && e.KeyCode == Keys.U) //  Ctrl +U=> Click Cancel
            {
                btnSubmit.PerformClick();
            }

        }

        private void txtBarcode_TextChanged(object sender, EventArgs e)
        {
            txtProductCode.Text = txtBarcode.Text.ToString();
        }

        private void cboReferenceProductName_Leave(object sender, EventArgs e)
        {
            if (IsReference)
            {
                entity = new POSEntities();
                if (cboReferenceProductName.SelectedIndex > 0)
                {
                    int _productId = Convert.ToInt32(cboReferenceProductName.SelectedValue);
                    var _referenceData = (from pro in entity.Products where pro.Id == _productId select pro).FirstOrDefault();
                    txtName.Text = _referenceData.Name;
                    txtUnitPrice.Text = _referenceData.Price.ToString();
                    cboBrand.SelectedValue = _referenceData.BrandId;
                    if (_referenceData.ProductCategoryId != null)
                    {
                        cboMainCategory.SelectedValue = _referenceData.ProductCategoryId;
                    }

                    if (_referenceData.ProductSubCategoryId != null)
                    {
                        cboSubCategory.SelectedValue = _referenceData.ProductSubCategoryId;
                    }
                  
                   
                }
                else
                {
                    Clear_Control();
                }
            }
        }

        private void cboBrand_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboBrand.SelectedIndex > 0)
            {
                if (!isEdit)
                {
                    if (IsStockAutoGenerate == true)
                    {
                        int _brandId = Convert.ToInt32(cboBrand.SelectedValue);
                        string _stockAutoGenerateNo = Utility.Stock_AutoGenerateNo(_brandId);

                        txtBarcode.Text = _stockAutoGenerateNo;
                        txtProductCode.Text = _stockAutoGenerateNo;
                    }
                }
            }
        }
    }
}
