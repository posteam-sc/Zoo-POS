using Microsoft.Reporting.WinForms;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using POS.APP_Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Security.Cryptography;
using System.ServiceProcess;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace POS
{
    class Utility
    {
        /// <summary>
        /// Decrypting incomming file.
        /// </summary>
        /// <param name="inputFile"></param>
        /// <param name="outputFile"></param>
        public static void DecryptFile(string inputFile, string outputFile)
        {
            try
            {
                string password = @"myKey123";

                UnicodeEncoding UE = new UnicodeEncoding();
                byte[] key = UE.GetBytes(password);

                FileStream fsCrypt = new FileStream(inputFile, FileMode.Open);

                RijndaelManaged RMCrypto = new RijndaelManaged();

                CryptoStream cs = new CryptoStream(fsCrypt, RMCrypto.CreateDecryptor(key, key), CryptoStreamMode.Read);

                FileStream fsOut = new FileStream(outputFile, FileMode.Create);

                int data;
                while ((data = cs.ReadByte()) != -1)
                    fsOut.WriteByte((byte)data);

                fsOut.Close();
                cs.Close();
                fsCrypt.Close();
            }
            catch
            {
                MessageBox.Show("Decryption failed!", "Error");
            }
        }
        public static void WaitCursor()
        {
            Application.UseWaitCursor = true;
            System.Windows.Forms.Cursor.Current = Cursors.WaitCursor;
        }
        public static void LeaveCursor()
        {
            Application.UseWaitCursor = false;
            System.Windows.Forms.Cursor.Current = Cursors.Default;
        }

        public static IQueryable<POS.APP_Data.Setting> generalEntity_Para;

        /// <summary>
        /// Encrypting incomming file.
        /// </summary>
        /// <param name="inputFile"></param>
        /// <param name="outputFile"></param>
        public static void EncryptFile(string inputFile, string outputFile)
        {
            try
            {
                string password = @"myKey123";
                UnicodeEncoding UE = new UnicodeEncoding();
                byte[] key = UE.GetBytes(password);

                string cryptFile = outputFile;

                FileStream fsCrypt = new FileStream(cryptFile, FileMode.Create);

                RijndaelManaged RMCrypto = new RijndaelManaged();

                CryptoStream cs = new CryptoStream(fsCrypt, RMCrypto.CreateEncryptor(key, key), CryptoStreamMode.Write);

                FileStream fsIn = new FileStream(inputFile, FileMode.Open);

                int data;
                while ((data = fsIn.ReadByte()) != -1)
                    cs.WriteByte((byte)data);


                fsIn.Close();
                cs.Close();
                fsCrypt.Close();
            }
            catch
            {
                MessageBox.Show("Encryption failed!", "Information :)", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
        }

        /// <summary>
        /// Decrypt the input string ( Eg: EncryptString("ABC", string.Empty); )  
        /// </summary>
        public static string EncryptString(string Message, string Passphrase)
        {
            byte[] Results;
            System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();

            // Step 1. We hash the passphrase using MD5
            // We use the MD5 hash generator as the result is a 128 bit byte array
            // which is a valid length for the TripleDES encoder we use below

            MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
            byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(Passphrase));

            // Step 2. Create a new TripleDESCryptoServiceProvider object
            TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();

            // Step 3. Setup the encoder
            TDESAlgorithm.Key = TDESKey;
            TDESAlgorithm.Mode = CipherMode.ECB;
            TDESAlgorithm.Padding = PaddingMode.PKCS7;

            // Step 4. Convert the input string to a byte[]
            byte[] DataToEncrypt = UTF8.GetBytes(Message);

            // Step 5. Attempt to encrypt the string
            try
            {
                ICryptoTransform Encryptor = TDESAlgorithm.CreateEncryptor();
                Results = Encryptor.TransformFinalBlock(DataToEncrypt, 0, DataToEncrypt.Length);
            }
            finally
            {
                // Clear the TripleDes and Hashprovider services of any sensitive information
                TDESAlgorithm.Clear();
                HashProvider.Clear();
            }

            // Step 6. Return the encrypted string as a base64 encoded string
            return Convert.ToBase64String(Results);
        }
       
       public class DBService
        {
            private  ServiceController sqlService;
            public  bool Running = true;
            public bool AllowToStart = true;
            /// <summary>
            /// Check and Start the SQL service for sure Running.
            /// </summary>
            public DBService()
            {
                ServiceController[] svc = ServiceController.GetServices();
                AllowToStart = DatabaseControlSetting._ServerName.ToUpper().StartsWith(System.Environment.MachineName.ToUpper());
                for (int i = 0; i < svc.Count(); i++)
                {
                    if (svc[i].ServiceName.Contains("MSSQL$") || svc[i].ServiceName.Contains("MSSQLSERVER"))
                    {
                        svc[i].Refresh();
                        if ((svc[i].ServiceName.Contains("$") && svc[i].ServiceName.Substring(0, svc[i].ServiceName.IndexOf('$')) == "MSSQL" )|| svc[i].ServiceName== "MSSQLSERVER" )
                        {
                            if (svc[i].Status != ServiceControllerStatus.Running)
                            {
                                Running = false;
                                sqlService = svc[i];
                                break;
                            }
                            else
                            {
                                Running = true;
                            }
                           
                        }
                    }
                }
            }
          
            public void Run()
            {
                if (sqlService.Status==ServiceControllerStatus.Stopped)
                {
                    sqlService.Start();
                }
                else if (sqlService.Status==ServiceControllerStatus.Paused)
                {
                    sqlService.Continue();
                }
                sqlService.WaitForStatus(ServiceControllerStatus.Running);
            }
        }
        /// <summary>
        /// Decrypt the input string ( Eg: DecryptString("LoBCnf0JCg8=", string.Empty); )  
        /// </summary>
        public static string DecryptString(string Message, string Passphrase)
        {
            byte[] Results;
            System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();

            // Step 1. We hash the passphrase using MD5
            // We use the MD5 hash generator as the result is a 128 bit byte array
            // which is a valid length for the TripleDES encoder we use below

            MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
            byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(Passphrase));

            // Step 2. Create a new TripleDESCryptoServiceProvider object
            TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();

            // Step 3. Setup the decoder
            TDESAlgorithm.Key = TDESKey;
            TDESAlgorithm.Mode = CipherMode.ECB;
            TDESAlgorithm.Padding = PaddingMode.PKCS7;

            // Step 4. Convert the input string to a byte[]
            byte[] DataToDecrypt = Convert.FromBase64String(Message);

            // Step 5. Attempt to decrypt the string
            try
            {
                ICryptoTransform Decryptor = TDESAlgorithm.CreateDecryptor();
                Results = Decryptor.TransformFinalBlock(DataToDecrypt, 0, DataToDecrypt.Length);
            }
            finally
            {
                // Clear the TripleDes and Hashprovider services of any sensitive information
                TDESAlgorithm.Clear();
                HashProvider.Clear();
            }

            // Step 6. Return the decrypted string in UTF8 format
            return UTF8.GetString(Results);
        }
       
        public static string GetSystemMACID()
        {
            string systemName = System.Windows.Forms.SystemInformation.ComputerName;
            try
            {
                NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
                String sMacAddress = string.Empty;
                foreach (NetworkInterface adapter in nics)
                {
                    if (sMacAddress == String.Empty)// only return MAC Address from first card
                    {
                        IPInterfaceProperties properties = adapter.GetIPProperties();
                        sMacAddress = adapter.GetPhysicalAddress().ToString();
                    }
                }
                return Regex.Replace(sMacAddress, ".{2}", "$0-").Substring(0, 17);
            }
            catch (ManagementException e)
            {
            }
            catch (System.UnauthorizedAccessException e)
            {

            }
            return string.Empty;
        }

        public static string GetSystemMotherBoardId()
        {
            try
            {
                ManagementObjectSearcher _mbs = new ManagementObjectSearcher("Select SerialNumber From Win32_BaseBoard");
                ManagementObjectCollection _mbsList = _mbs.Get();
                string _id = string.Empty;
                foreach (ManagementObject _mo in _mbsList)
                {
                    _id = _mo["SerialNumber"].ToString();
                    break;
                }

                return _id;
            }
            catch
            {
                return string.Empty;
            }

        }
        public static Boolean IsRegister()
        {
            if (SettingController.IsSourcecode == true)
            {

                POSEntities entity = new POSEntities();
                string MacId = Utility.GetSystemMotherBoardId();
                foreach (Authorize item in entity.Authorizes.ToList())
                {
                    //string currentKey = Utility.DecryptString(item.macAddress, "ABCD");
                    if (item.macAddress != "" && item.macAddress == MacId)
                    {
                        return true;
                    }
                }
                return false;
            }
            else
            {
                POSEntities entity = new POSEntities();
                string MacId = Utility.GetSystemMotherBoardId();
                foreach (Authorize item in entity.Authorizes.ToList())
                {
                    //string currentKey = Utility.DecryptString(item.macAddress, "ABCD");
                    if (item.macAddress != "" && item.macAddress == MacId)
                    {
                        return true;
                    }
                }
                return false;
            }


        }

        
      
        public static void UpdateProductCode(string s, long p)
        {
            SqlParameter[] paras = new SqlParameter[2];
            paras[0] = new SqlParameter("@ProductCode", s);
            paras[1] = new SqlParameter("@interestAmount", p);
        }


        #region Control Event
        //brand
        public static Boolean Brand_Combo_Control(ComboBox cboBrand)
        {
            bool _condition = false;
            POSEntities entity = new POSEntities();
            string _brandName = cboBrand.Text;

            if (_brandName != "Select" && _brandName != string.Empty)
            {
                var brand = (from c in entity.Brands where c.Name == _brandName select c).ToList();

                if (brand.Count <= 0)
                {
                    MessageBox.Show("Brand Name '" + cboBrand.Text + "' haven't registered yet!", "mPOS");
                    cboBrand.Focus();
                    _condition = true;
                }
            }
            return _condition;
        }

        //main category
        public static Boolean MainCategory_Combo_Control(ComboBox cboCategory)
        {
            bool _condition = false;
            POSEntities entity = new POSEntities();
            string _cagName = cboCategory.Text;

            if (_cagName != "Select" && _cagName != string.Empty)
            {
                var cag = (from c in entity.ProductCategories where c.Name == _cagName select c).ToList();

                if (cag.Count <= 0)
                {
                    MessageBox.Show("Category '" + cboCategory.Text + "' haven't registered yet!", "mPOS");
                    cboCategory.Focus();
                    _condition = true;
                }
            }
            return _condition;
        }


        //sub category
        public static Boolean SubCategory_Combo_Control(ComboBox cboSubCategory)
        {
            bool _condition = false;
            POSEntities entity = new POSEntities();
            string _subCagName = cboSubCategory.Text;

            if (_subCagName != "None" && _subCagName != "Select" && _subCagName != string.Empty)
            {
                var subCag = (from c in entity.ProductSubCategories where c.Name == _subCagName select c).ToList();

                if (subCag.Count <= 0)
                {
                    MessageBox.Show("Sub Category '" + cboSubCategory.Text + "' haven't registered yet!", "mPOS");
                    cboSubCategory.Focus();
                    _condition = true;
                }
            }
            return _condition;
        }
   //product
        public static Boolean Product_Combo_Control(ComboBox cboProduct)
        {
            bool _condition = false;
            POSEntities entity = new POSEntities();
            string _productName = cboProduct.Text;

            if (_productName != "Select" && _productName != string.Empty)
            {
                var pro = (from c in entity.Products where c.Name == _productName select c).ToList();

                if (pro.Count <= 0)
                {
                    MessageBox.Show("Product Name '" + cboProduct.Text + "' haven't registered yet!", "mPOS");
                    cboProduct.Focus();
                    _condition = true;
                }
            }
            return _condition;
        }

#endregion

        #region Combo Bind Event
        
        //Product
        public static void BindProduct(ComboBox cboProduct)
        {
            POSEntities entity = new POSEntities();
            List<Product> productList = new List<Product>();
            Product productObj = new Product();
            productObj.Id = 0;
            productObj.Name = "Select All";
            productList.Add(productObj);
            productList.AddRange(entity.Products.ToList());
            cboProduct.DataSource = productList;
            cboProduct.DisplayMember = "Name";
            cboProduct.ValueMember = "Id";
            cboProduct.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboProduct.AutoCompleteSource = AutoCompleteSource.ListItems;
        }

        //Conversion Product
        //public static void BindConversionProduct(ComboBox cboProduct)
        //{
        //    POSEntities entity = new POSEntities();
        //    List<Product> productList = new List<Product>();
        //    Product productObj = new Product();
        //    productObj.Id = 0;
        //    productObj.Name = "ALL";
        //    productList.Add(productObj);
        //    productList.AddRange(entity.Products.ToList());
        //    cboProduct.DataSource = productList;
        //    cboProduct.DisplayMember = "Name";
        //    cboProduct.ValueMember = "Id";
        //    cboProduct.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
        //    cboProduct.AutoCompleteSource = AutoCompleteSource.ListItems;
        //}

        //Bind Reference Product
        public static void BindReferenceProduct(ComboBox cboProduct)
        {
            POSEntities entity = new POSEntities();
            List<Product> productList = new List<Product>();
            Product productObj = new Product();
            productObj.Id = 0;
            productObj.Name = "Select";
            productList.Add(productObj);
            productList.AddRange(entity.Products.ToList());
            cboProduct.DataSource = productList;
            cboProduct.DisplayMember = "Name";
            cboProduct.ValueMember = "Id";
            cboProduct.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboProduct.AutoCompleteSource = AutoCompleteSource.ListItems;
        }

         //Brand
        public static void BindBrand(ComboBox cboBrand)
        {
            POSEntities entity = new POSEntities();
            List<APP_Data.Brand> brandList = new List<APP_Data.Brand>();

            APP_Data.Brand brandObj = new APP_Data.Brand();
            brandObj.Id = 0;
            brandObj.Name = "Select";
            brandList.Add(brandObj);

            brandList.AddRange((from _brand in entity.Brands select _brand).ToList());
            cboBrand.DataSource = brandList;
            cboBrand.DisplayMember = "Name";
            cboBrand.ValueMember = "Id";
            cboBrand.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboBrand.AutoCompleteSource = AutoCompleteSource.ListItems;
        }

        //ExpenseCategory
       
        public static void BindCity(ComboBox cboCity)
        {
            POSEntities entity = new POSEntities();
             List<APP_Data.City> cityList = entity.Cities.ToList();
            cboCity.DataSource = cityList;
            cboCity.DisplayMember = "CityName";
            cboCity.ValueMember = "Id";

            cboCity.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboCity.AutoCompleteSource = AutoCompleteSource.ListItems;
        }


        public static void BindShop(ComboBox cboShop, bool includeALL = false)
        {
            if (includeALL)
            {
                POSEntities entity = new POSEntities();
                List<APP_Data.Shop> shopList = new List<APP_Data.Shop>();

                APP_Data.Shop shop = new APP_Data.Shop();
                shop.ShopName = "ALL Zoo";
                shop.Id = 0;

                var shops = entity.Shops.ToList();
                shopList.Add(shop);
                shopList.AddRange(shops);
                cboShop.DataSource = shopList;
                cboShop.DisplayMember = "ShopName";
                cboShop.ValueMember = "Id";

                cboShop.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                cboShop.AutoCompleteSource = AutoCompleteSource.ListItems;
            }
            else
            {
                POSEntities entity = new POSEntities();
                List<APP_Data.Shop> shopList = entity.Shops.ToList();
                cboShop.DataSource = shopList;
                cboShop.DisplayMember = "ShopName";
                cboShop.ValueMember = "Id";

                cboShop.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                cboShop.AutoCompleteSource = AutoCompleteSource.ListItems;
            }
           
        }
        

        public static void BindShopALL(ComboBox cboShop)
        {
            POSEntities entity = new POSEntities();
            List<APP_Data.Shop> shopList = new List<APP_Data.Shop>();

            APP_Data.Shop shopObj = new APP_Data.Shop();
            shopObj.Id = 0;
            shopObj.ShopName = "ALL";
            shopList.Add(shopObj);
            shopList.AddRange(entity.Shops.ToList());
            cboShop.DataSource = shopList;
            cboShop.DisplayMember = "ShopName";
            cboShop.ValueMember = "Id";

            cboShop.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboShop.AutoCompleteSource = AutoCompleteSource.ListItems;
        }
        #endregion

       

        #region StockTransaction


      
     
        public static string Month_Name(int _month, int _year)
        {
            string _tranDate = "";
            switch (_month)
            {
                case 1:
                    _tranDate = "Janauary" + "-" + _year.ToString();
                    break;
                case 2:
                    _tranDate = "February" + "-" + _year.ToString();
                    break;
                case 3:
                    _tranDate = "March" + "-" + _year.ToString();
                    break;
                case 4:
                    _tranDate = "April" + "-" + _year.ToString();
                    break;
                case 5:
                    _tranDate = "May" + "-" + _year.ToString();
                    break;
                case 6:
                    _tranDate = "June" + "-" + _year.ToString();
                    break;
                case 7:
                    _tranDate = "July" + "-" + _year.ToString();
                    break;
                case 8:
                    _tranDate = "August" + "-" + _year.ToString();
                    break;
                case 9:
                    _tranDate = "September" + "-" + _year.ToString();
                    break;
                case 10:
                    _tranDate = "October" + "-" + _year.ToString();
                    break;
                case 11:
                    _tranDate = "November" + "-" + _year.ToString();
                    break;
                case 12:
                    _tranDate = "December" + "-" + _year.ToString();
                    break;
            }
            return _tranDate;
        }

      
        //Sale (Get ProductId and Sale Qty List)
        //public static List<Stock_Transaction> Sale_Balance(int _year, int _month, List<long> productIdList)
        //{
        //    POSEntities entity = new POSEntities();
        //    List<string> _type = new List<string> { "Sale", "Credit" };
        //    List<Stock_Transaction> _saleResult = new List<Stock_Transaction>();
        //    var _saleList = (from tds in entity.TransactionDetails
        //                     join pd in entity.Products on tds.ProductId equals pd.Id
        //                     join t in entity.Transactions on tds.TransactionId equals t.Id
        //                     where tds.IsDeleted == false
        //                     && t.IsComplete == true
        //                       && productIdList.Contains(pd.Id)
        //                     && _type.Contains(t.Type)
        //                        && t.DateTime.Value.Month == _month && t.DateTime.Value.Year == _year
        //                        && t.DateTime.Value.Month == _month && t.DateTime.Value.Year == _year
        //                     select new Stock_Transaction
        //                     {
        //                         ProductId = tds.ProductId,
        //                         Sale = tds.Qty
        //                     }
        //                           ).ToList();

        //    if (_saleList.Count > 0)
        //    {
        //        _saleResult = _saleList.GroupBy(x => new { x.ProductId })
        //                        .Select(y => new Stock_Transaction()
        //                        {


        //                            ProductId = y.Key.ProductId,
        //                            Sale = y.Sum(x => x.Sale)

        //                        }).ToList();

        //    }
        //    return _saleResult;
        //}


        
        #endregion

        #region Convert String To Date Time
        public static DateTime Convert_Date(string date)
        {
            // string date = dgvTransactionList.Rows[e.RowIndex].Cells[2].Value.ToString();
            DateTime _Trandate = DateTime.ParseExact(date, SettingController.GlobalDateFormat, null);
            return _Trandate;
        }

        #endregion

        #region  Remove Comma

        public static string[] Remove_Comma(string _dataList)
        {
            string[] _removeCommaDataList = _dataList.Split(',');
            return _removeCommaDataList;
        }
        #endregion

        #region Convert string[] To Long[]
        public static List<long> Convert_String_To_Long(string[] _dataList)
        {
            List<long> _longDataList = _dataList.Select(long.Parse).ToList();
            return _longDataList;
        }
        #endregion

        
        #region Get Report Path
        public static string GetReportPath(string paymentType)
        {
            string reportPath = "";
            string printer = SettingController.SelectDefaultPrinter;
            switch (printer)
            {
                case "A4 Printer":
                    bool _isSourcecode = SettingController.IsSourcecode;
                    switch (paymentType)
                    {
                        case "Cash":

                            if (!_isSourcecode)
                            {
                                if (SettingController.ShowProductImageIn_A4Reports)
                                {
                                    reportPath = "\\Reports\\InvoiceCashA4PImage.rdlc";
                                }
                                else
                                {
                                    reportPath = "\\Reports\\InvoiceCashA4.rdlc";
                                }
                               
                            }
                            else if (_isSourcecode)
                            {
                                reportPath = "\\Reports\\SourcecodeInvoiceCashA4.rdlc";
                            }
                            break;
                        case "Credit":
                            if (!_isSourcecode)
                            {
                                if (SettingController.ShowProductImageIn_A4Reports)
                                {
                                    reportPath = "\\Reports\\InvoiceCreditA4PImage.rdlc";
                                }
                                else
                                {
                                    reportPath = "\\Reports\\InvoiceCreditA4.rdlc";
                                }
                               
                            }
                            else if (_isSourcecode)
                            {
                                reportPath = "\\Reports\\SourcecodeInvoiceCreditA4.rdlc";
                            }
                            break;
                        case "Settlement":
                            if (!_isSourcecode)
                            {
                                reportPath = "\\Reports\\InvoiceSettlementA4.rdlc";
                            }
                            else if (_isSourcecode)
                            {
                                reportPath = "\\Reports\\SourcecodeInvoiceSettlementA4.rdlc";
                            }
                            break;
                        case "MPU":
                            if (SettingController.ShowProductImageIn_A4Reports)
                            {
                                reportPath = "\\Reports\\InvoiceMPUA4PImage.rdlc";
                            }
                            else
                            {
                                reportPath = "\\Reports\\InvoiceMPUA4.rdlc";
                            }
                          
                            break;
                        case "GiftCard":
                            if (SettingController.ShowProductImageIn_A4Reports)
                            {
                                reportPath = "\\Reports\\InvoiceGiftcardA4PImage.rdlc";
                            }
                            else
                            {
                                reportPath = "\\Reports\\InvoiceGiftcardA4.rdlc";
                            }
                            
                            break;
                        case "FOC":
                            if (SettingController.ShowProductImageIn_A4Reports)
                            {
                                reportPath = "\\Reports\\InvoiceFOCA4PImage.rdlc";
                            }
                            else
                            {
                                reportPath = "\\Reports\\InvoiceFOCA4.rdlc";
                            }
                           
                            break;
                        case "Tester":
                            if (SettingController.ShowProductImageIn_A4Reports)
                            {
                                reportPath = "\\Reports\\InvoiceFOCA4PImage.rdlc";
                            }
                            else
                            {
                                reportPath = "\\Reports\\InvoiceFOCA4.rdlc";
                            }
                            break;
                       
                    }
                    break;
                case "Slip Printer":
                    switch (paymentType)
                    {
                        case "Cash":
                            reportPath = "\\Reports\\InvoiceCash.rdlc";
                            break;
                        case "Credit":
                            reportPath = "\\Reports\\InvoiceCredit.rdlc";
                            break;
                        case "MPU":
                            reportPath = "\\Reports\\InvoiceMPU.rdlc";
                            break;
                        case "GiftCard":
                            reportPath = "\\Reports\\InvoiceGiftcard.rdlc";
                            break;
                        case "FOC":
                            reportPath = "\\Reports\\InvoiceFOC.rdlc";
                            break;
                        case "Tester":
                            reportPath = "\\Reports\\InvoiceFOC.rdlc";
                            break;
                        case "Settlement":
                            reportPath = "\\Reports\\InvoiceSettlement.rdlc";
                            break;
                    }
                    break;
            }
            return reportPath;
        }
        #endregion

        #region  Get A4 Print or Slip Print
        public static string GetDefaultPrinter()
        {
            string printer = SettingController.SelectDefaultPrinter;
            return printer;
        }
        #endregion

        #region Slip Logo
        public static void Slip_Log(ReportViewer rv)
        {

            ReportParameter ImagePath = new ReportParameter("ImagePath", "file:\\" + Application.StartupPath + "\\img\\logo_frenzo.png");
            if (Get_Image(rv))
            {
                //MessageBox.Show("Please define Logo in Configuration Setting!", "mPOS");
                //Setting form = new Setting();
                //form.ShowDialog();
                Get_Image(rv);
            }
            else
            {
                Image_Parameter(rv, "");
            }

        }

        private static bool Get_Image(ReportViewer rv)
        {
            if (SettingController.Logo != "")
            {
                string imagePath = SettingController.Logo.Substring(7);
                Image_Parameter(rv, imagePath);
                return true;
            }
            else
                Image_Parameter(rv, "");
            return false;
        }

        private static void Image_Parameter(ReportViewer rv, string imagePath)
        {
            if (imagePath=="")
            {
                rv.LocalReport.EnableExternalImages = true;
                ReportParameter ImagePath = new ReportParameter("ImagePath", imagePath);
                rv.LocalReport.SetParameters(ImagePath);
            }else
            {
                rv.LocalReport.EnableExternalImages = true;
                ReportParameter ImagePath = new ReportParameter("ImagePath", "file:\\" + Application.StartupPath + "\\logo\\" + imagePath);
                rv.LocalReport.SetParameters(ImagePath);
            }
        }
        #endregion

        #region Slip Footer
        public static void Slip_A4_Footer(ReportViewer rv)
        {
            rv.LocalReport.EnableExternalImages = true;
            if (string.IsNullOrEmpty(SettingController.FooterPage))
            {
                SettingController.FooterPage= SettingController.FooterPage;
            }
            ReportParameter Footer = new ReportParameter("Footer", SettingController.FooterPage);
            rv.LocalReport.SetParameters(Footer);
        }
        #endregion

        #region Product AutoGenerateNo
        public static string Stock_AutoGenerateNo(int brandId)
        {
            string _registerNo = "020";
            string _autoGenerateNo = Get_BrandAutoNo_IncrementNo(brandId);

            string _stockAutoGenerateNo = _registerNo + _autoGenerateNo;
            return _stockAutoGenerateNo;

        }
        public static string Stock_AutoGenerateNo(int brandId, List<APP_Data.Brand> brand)
        {
            string _registerNo = "020";
            string _autoGenerateNo = Get_BrandAutoNo_IncrementNo(brandId,brand);

            string _stockAutoGenerateNo = _registerNo + _autoGenerateNo;
            return _stockAutoGenerateNo;

        }

        public static string Get_BrandAutoNo_IncrementNo(int brandId)
        {
            string _stockGenerateNo = "";
            POSEntities entity = new POSEntities();

            var _brandData = (from b in entity.Brands where b.Id == brandId select b).FirstOrDefault();
            string _brandId = _brandData.Id.ToString();
            int _incrementNo = 0;
            if ((_brandData.AutoGenerateNo.ToString()).Length >= 5)
            {
                _incrementNo = Convert.ToInt32(_brandData.AutoGenerateNo);
            }
            else
            {
                _incrementNo = Convert.ToInt32(_brandData.AutoGenerateNo) + 1;
            }



            string incrementNo = _incrementNo.ToString();
            string _brandAutoNo = "";
            switch (_brandId.Length)
            {
                case 1:
                    _brandAutoNo = _brandId + "0000";
                    break;
                case 2:
                    _brandAutoNo = _brandId + "000";
                    break;
                case 3:
                    _brandAutoNo = _brandId + "00";
                    break;
                case 4:
                    _brandAutoNo = _brandId + "0";
                    break;
                case 5:
                    _brandAutoNo = _brandId;
                    break;
            }


            string _autoGenerateNo = "";
            switch (incrementNo.Length)
            {
                case 1:
                    _autoGenerateNo = "0000" + incrementNo;
                    break;
                case 2:
                    _autoGenerateNo = "000" + incrementNo;
                    break;
                case 3:
                    _autoGenerateNo = "00" + incrementNo;
                    break;
                case 4:
                    _autoGenerateNo = "0" + incrementNo;
                    break;
                case 5:
                    _autoGenerateNo = incrementNo;
                    break;
            }

            _stockGenerateNo = _brandAutoNo + _autoGenerateNo;

            return _stockGenerateNo;
        }
        public static string Get_BrandAutoNo_IncrementNo(int brandId,List<APP_Data.Brand> brand)
        {
            string _stockGenerateNo = "";
            POSEntities entity = new POSEntities();

            var _brandData = (from b in brand where b.Id == brandId select b).FirstOrDefault();
            string _brandId = _brandData.Id.ToString();
            int _incrementNo = 0;
            if ((_brandData.AutoGenerateNo.ToString()).Length >= 5)
            {
                _incrementNo = Convert.ToInt32(_brandData.AutoGenerateNo);
            }
            else
            {
                _incrementNo = Convert.ToInt32(_brandData.AutoGenerateNo) + 1;
            }



            string incrementNo = _incrementNo.ToString();
            string _brandAutoNo = "";
            switch (_brandId.Length)
            {
                case 1:
                    _brandAutoNo = _brandId + "0000";
                    break;
                case 2:
                    _brandAutoNo = _brandId + "000";
                    break;
                case 3:
                    _brandAutoNo = _brandId + "00";
                    break;
                case 4:
                    _brandAutoNo = _brandId + "0";
                    break;
                case 5:
                    _brandAutoNo = _brandId;
                    break;
            }


            string _autoGenerateNo = "";
            switch (incrementNo.Length)
            {
                case 1:
                    _autoGenerateNo = "0000" + incrementNo;
                    break;
                case 2:
                    _autoGenerateNo = "000" + incrementNo;
                    break;
                case 3:
                    _autoGenerateNo = "00" + incrementNo;
                    break;
                case 4:
                    _autoGenerateNo = "0" + incrementNo;
                    break;
                case 5:
                    _autoGenerateNo = incrementNo;
                    break;
            }

            _stockGenerateNo = _brandAutoNo + _autoGenerateNo;

            return _stockGenerateNo;
        }
        #endregion

        #region Printing
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rv"></param>
        public static void Get_Print(ReportViewer rv)
        {
            int copy = Convert.ToInt32(SettingController.DefaultNoOfCopies);
            for (int i = 0; i < copy; i++)
            {
                PrintDoc.PrintReport(rv, GetDefaultPrinter());
            }
        }
        #endregion

        #region Check and Add FOC in Category
        public static void Check_AddFOCInCag()
        {
            POSEntities entity = new POSEntities();
            var val = (from c in entity.ProductCategories where c.Name == "FOC" select c).FirstOrDefault();

            if (val == null)
            {
                APP_Data.ProductCategory cag = new APP_Data.ProductCategory();

                cag.Name = "FOC";
                cag.IsDelete = false;
                entity.ProductCategories.Add(cag);
                entity.SaveChanges();

                APP_Data.ProductSubCategory subCag = new APP_Data.ProductSubCategory();
                subCag.Name = "FOC";
                subCag.ProductCategoryId = cag.Id;
                subCag.IsDelete = false;
                entity.ProductSubCategories.Add(subCag);
                entity.SaveChanges();

            }
        }
        #endregion

        #region Related Shop 
        public static void ShopComBo_EnableOrNot(ComboBox cboShop, Boolean IsSetting = false)
        {
            POSEntities entity = new POSEntities();
         
            switch (IsSetting)
            {
                case true:
                    Disabled_Shop(cboShop);
                    break;
                case false:
            var _isBackOffice = entity.Settings.Where(x => x.Key == "IsBackOffice" && x.Value == "1").FirstOrDefault();

            if (_isBackOffice == null)
            {
                Disabled_Shop(cboShop);
            }
                    break;
            }
        }

        public static bool IsNotBackOffice()
        {
            POSEntities entity = new POSEntities();
            var isNotBackOffice = entity.Settings.Where(x => x.Key == "IsBackOffice" && (x.Value == "0" || x.Value == null)).FirstOrDefault();
            if (isNotBackOffice != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool TransactionDelRefHide(int shopid)
        {
            if (SettingController.DefaultShop.Id != shopid)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static void Gpvisible(GroupBox g, Boolean edit)
        {
            if (edit == true)
            {
                g.Enabled = true;
            }
            else
            {
                g.Enabled = false;
            }
        }
        private static void Disabled_Shop(ComboBox cboShop)
        {
            int _shopId = 0;
            _shopId = SettingController.DefaultShop.Id;
            if (_shopId > 0)
            {
                cboShop.SelectedValue = _shopId;
                cboShop.Enabled = false;
            }
        }
        #endregion

        #region "Generate Code No"
        public static string Get_CodeNo(string pre , string shortCode)
        {
            
            string _codeNo = "";
            string month = "";
            if (DateTime.Now.Month < 10)
            {
                month = "0" + DateTime.Now.Month.ToString();
            }
            else
            {
                month = DateTime.Now.Month.ToString();
            }

            _codeNo = pre + shortCode + DateTime.Now.Year + month + DateTime.Now.Day;
                return _codeNo;
        }
        #endregion

        #region "Generate User Code No"
        public static string Get_UserCodeNo(int _shopId)
        {
            POSEntities entity = new POSEntities();
            var _userId = 0;
            _userId = (from con in entity.Users orderby con.Id descending select con.Id).FirstOrDefault();
            var _shortCode = entity.Shops.Where(x => x.Id == _shopId).Select(x => x.ShortCode).FirstOrDefault();
            //string UserCodeNo = "US" + _shortCode + DateTime.Now.Year.ToString() + month + DateTime.Now.Day.ToString() + (_userId + 1).ToString();
            string UserCodeNo = Get_CodeNo("US", _shortCode) + (_userId + 1).ToString();

            return UserCodeNo;
        }
        #endregion
    }
    public class _dynamicPrice
    {
        public string dynamicProductCode { get; set; }

        public int dynamicPrice { get; set; }

    }
    public class Convert_Controller
    {
        public int PurchaseDetailId;
        public int ConvertQty;
    }

    public class Damage_Controller
    {
        public int Id;
        public string ProductName;
        public string ProductCode;
        public DateTime? AdjustmentDateTime;
        public int? DamageQty;
        public long Price;
        public long? TotalCost;
        public string Reason;
        public string ResponsibleName;

    }

    //public class Adjustment_Controller
    //{
    //    // public int Id;
    //    public string ProductName;
    //    public string ProductCode;
    //    public DateTime? AdjustmentDateTime;
    //    public int? StockIn;
    //    public int? StockOut;
    //    public long Price;
    //    public long? TotalCost;
    //    public string Reason;
    //    public string ResponsibleName;

    //}

    //public class ExpenseController
    //{
    //    public int ExpenseDetailId;
    //    public string ExpenseNo;
    //    public string Description;
    //    public decimal Qty;
    //    public decimal Price;
    //    public decimal Amount;
    //    public int CategoryId;
    //    public string CategoryName;
    //}
    //public class ExpenseRp
    //{
   
    //    public string ExpenseNo;
    //    public string Description;
    //    public int Qty;
    //    public int Price;
    //    public int Amount;
    //    public string CreatedUser;
    //    public string CategoryName;
    //    public DateTime ExpenseDate;
    //}
    //public class strockInheader
    //{
    //    public long? StockInHeaderId;
    //    public long StockInDetailId;
    //    public long? productId;
    //    public string barcode;
    //    public string productname;
    //    public int? Qty;

    //}
    //public class stockheader
    //{
    //    public Int64 StockId;
    //    public string StockCode;
    //    public string fromshop;
    //    public DateTime? date;
    //    public string status;

    //}

    //public class Stock_Transaction
    //{

    //    public long? ProductId;
    //    public string TranDate;
    //    public int? Opening;
    //    public int? Purchase;
    //    public int? Refund;
    //    public int? Sale;
    //    // public int? Adjustment;
    //    public long? Consignment;

    //    public string ProductName;
    //    public string ProductCode;
    //    public int? AdjustmentStockIn;
    //    public int? AdjustmentStockOut;
    //    public int? ConversionStockIn;
    //    public int? ConversionStockOut;

    //    public int? AdjustmentQty;
    //    public int? StockIn;
    //    public int? StockOut;
    //}

    public static class MemberShip
    {
        public static string UserName;
        public static string UserRole;
        public static int UserRoleId;
        public static int UserId;
        public static bool isLogin;
        public static bool isAdmin;
        public static int CounterId;
        public static string CounterName;
        public static string CounterReferenceNo;
    }

    public static class TransactionType
    {
        public static string Sale
        {
            get { return "Sale"; }
        }
        //public static string Refund
        //{
        //    get { return "Refund"; }
        //}
        //public static string Settlement
        //{
        //    get { return "Settlement"; }
        //}
        //public static string Credit
        //{
        //    get { return "Credit"; }
        //}
        //public static string CreditRefund
        //{
        //    get { return "CreditRefund"; }
        //}
        //public static string Prepaid
        //{
        //    get { return "Prepaid"; }
        //}
    }

    public class TransactionDetailByItemHolder
    {
        //   public int ItemId;
        //public string ProductSku;
        public string ItemNo;
        public string Name;
        public string TransactionId;
        public string TransactionType;
        public int Qty;
        public int TotalAmount;
        //public DateTime date;
        public DateTime TransactionDate;
        public string Counter_Name;
    }

    public class TopProductHolder
    {
        public string ProductId;
        public string Name;
        public decimal Discount;
        public int Qty;
        public long UnitPrice;
        public long totalAmount;

    }
    //public class CustomerInfoHolder
    //{
    //    public int PayableAmount;
    //    public int Id;
    //    public string Name;
    //    public string PhNo;
    //    public string Address;
    //    public long OutstandingAmount;
    //    public long RefundAmount;
    //}


    //public class ConsignmentController
    //{
    //    public int ProductId;
    //    public string Name;
    //    public int Qty;
    //    public int Price;
    //    public long Total;
    //    public int ConsignmentPrice;
    //    public long TotalConsignmentPrice;
    //    public string counter;
    //    public int Profit;
    //    public long TotalProfit;
    //    public int TransactionDetailId;

    //}
    public class ReportItemSummary
    {
        public string Id;
        public string Name;
        public int Qty;
        public int UnitPrice;
        public long totalAmount;
        public int PaymentId;
        public string Size;
        public Boolean IsFOC;
        public int SellingPrice;
        public string Remark;
        public int discountrate;
        public int ReceivedCurrencyId;
        public decimal discountAmount;
    }
    
    public class ProductReportController
    {
        public int Id;
        public int Qty;
        public string SKUCode;
        public string ProductName;
        public string BrandName;
        public int TotalQty;
        public string Segment;
        public string SubSegment;
        public string Line;
        public bool IsDiscontinous;
        public bool isConsignment;
        public int UnitPrice;
        public int TotalPrice;
        public int SubTotalPrice;
        public int? PurchasePrice;
        public string PhotoPath;


    }

    //public class CustomerSaleController
    //{
    //    public string CustomerName;
    //    public string ProductName;
    //    public int Price;
    //    public int Qty;
    //    public int TotalAmount;
    //    public int MemberDiscount;
    //    public decimal? MCDiscount;
    //    public int SubTotal;
    //    public string TransactionId;
    //    public string MemberType;
    //    public DateTime SaleDate;
    //    public string Remark;
    //}
    public class SaleBreakDownController
    {
        public int bId;
        public string Name;
        public decimal Sales;
        public decimal BreakDown;
        public int saleQty;
        public decimal Refund;
        public int refundQty;
    }
    //public class SpecialPromotionController
    //{
    //    public int bId;
    //    public string Name;
    //    public decimal Sales;
    //    public decimal BreakDown;
    //    public int saleQty;
    //    public decimal Refund;
    //    public int refundQty;
    //}

    //public class PurchaseReportController
    //{
    //    public DateTime Date;
    //    public string ProductName;
    //    public string SupplierName;
    //    public int UnitPrice;
    //    public int Qty;
    //    public Int64 TotalAmount;
    //    public string VourcherNo;

    //}

    public class TotalDailyReport
    {
        public DateTime Date;
        public int TotalTransaction;
        public int TotalQty;
        public Int64 TotalCashAmount;
        public Int64 TotalCreditAmount;
        public Int64 TotalMPUAmount;
        public Int64 TotalGiftCardAmount;
        public Int64 TotalFOCAmount;
        public Int64 TotalTesterAmount;
        public Int64 TotalRefundAmount;
        public Int64 TotalRefundQty;
        public Int64 RepaidAmount;
    }
    //public class wproductlist
    //{
    //    public long Id;
    //    public string ProductCode;
    //    public string Barcode;
    //    public string Name;
    //    public long Price;
    //    public int? ChildQty;
    //}
    public class ProfitAndLoss
    {
        public DateTime SaleDate;
        public int TotalSaleQty;
        public Int64 TotalPurchaseAmount;
        public Int64 TotalSaleAmount;
        public Int64 TotalDiscountAmount;
        public Int64 TotalTaxAmount;
        public Int64 ProfitAndLossAmount;
    }

    //public class PurchaseProductController
    //{

    //    public int PurchaseDetailId;
    //    public string Barcode;
    //    public string ProductName;
    //    public int Qty;
    //    public Int64 PurchasePrice;
    //    public Int64 Total;
    //    public Int64 ProductId;
    //    public DateTime? ExpireDate;
    //    public int Id;
    //}

    //public class GiftCardController
    //{

    //    public int GiftCardId;
    //    public int Amount;
    //    public string CardNumber;

    //}

    //public class PurchaseDiscountController
    //{
    //    public DateTime PurchaseDate;
    //    public string VoucherNo;
    //    public string SupplierName;
    //    public Int64 TotalAmount;
    //    public int DiscountAmount;
    //}
    public static class BOOrPOS
    {
        public static bool IsBackOffice;

    }

    //public class SaleProductController
    //{

    //    public int PurchaseDetailId;
    //    public string Barcode;
    //    public string ProductName;
    //    public int Qty;
    //    public Int64 UnitPrice;
    //    public Int64 ConsignmentPrice;
    //    public Int64 ProductId;
    //    public int DiscountPercent;
    //    public int Tax;
    //    public bool IsFOC;
    //}

    //public class TransactionNote
    //{
    //    public string note;
    //    public string status;
    //}
    public class AverageMonthlySaleController
    {
        public string ProductCode;
        public string ProductName;
        public string Unit;
        public int JanQty;
        public int FebQty;
        public int MarQty;
        public int AprQty;
        public int MayQty;
        public int JunQty;
        public int JulQty;
        public int AugQty;
        public int SepQty;
        public int OctQty;
        public int NovQty;
        public int DecQty;
        public int TotalQty;
        public decimal AvgQty;
        public int SellingPrice;
        public Int64 TotalAmount;
        public string Remark;
    }

 


    public class RoleManagementController
    {
        public RoleManagementModel Setting { get; set; }
        public RoleManagementModel CSV { get; set; }
        //public RoleManagementModel Consignor { get; set; }
        //public RoleManagementModel MeasurementUnit { get; set; }
        //public RoleManagementModel CurrencyExchange { get; set; }
        //public RoleManagementModel TaxRate { get; set; }
        public RoleManagementModel City { get; set; }
        public RoleManagementModel Product { get; set; }
        public RoleManagementModel Brand { get; set; }
        //public RoleManagementModel GiftCard { get; set; }
        //public RoleManagementModel Customer { get; set; }
        //public RoleManagementModel OutstandingCustomer { get; set; }
        //public RoleManagementModel Supplier { get; set; }
        //public RoleManagementModel OutstandingSupplier { get; set; }
        //public RoleManagementModel ConsigmentSettlement { get; set; }
        public RoleManagementModel Category { get; set; }
        public RoleManagementModel SubCategory { get; set; }
        public RoleManagementModel Counter { get; set; }
        //public RoleManagementModel PurchaseRole { get; set; }
        //public RoleManagementModel MemberRule { get; set; }
        //public RoleManagementModel Adjustment { get; set; }
        public RoleManagementModel Transaction { get; set; }
        //public RoleManagementModel CreditTransaction { get; set; }
        public RoleManagementModel TransactionDetail { get; set; }
        //public RoleManagementModel Refund { get; set; }
        //   public RoleManagementModel AdjustmentType { get; set; }
        //public RoleManagementModel UnitConversion { get; set; }
        //public RoleManagementModel Expense { get; set; }
        //public RoleManagementModel ExpenseCategory { get; set; }

        //Reports
        public RoleManagementModel TransactionReport { get; set; }
        public RoleManagementModel ItemSummaryReport { get; set; }
        //public RoleManagementModel TaxSummaryReport { get; set; }
        //public RoleManagementModel ReorderPointReport { get; set; }
        public RoleManagementModel TransactionDetailReport { get; set; }
        //public RoleManagementModel OutstandingCustomerReport { get; set; }
        public RoleManagementModel TopBestSellerReport { get; set; }
        public RoleManagementModel TransactionSummaryReport { get; set; }
        public RoleManagementModel Ticket { get; set; }

        public RoleManagementModel DailySaleSummary { get; set; }
        public RoleManagementModel DailyTotalTransactions { get; set; }
        //public RoleManagementModel PurchaseReport { get; set; }
        //public RoleManagementModel PurchaseDiscount { get; set; }
        public RoleManagementModel SaleBreakdown { get; set; }
        //public RoleManagementModel CustomerSales { get; set; }
        public RoleManagementModel ProductReport { get; set; }
        public RoleManagementModel CustomerInformation { get; set; }
        //public RoleManagementModel Consigment { get; set; }
        public RoleManagementModel ProfitAndLoss { get; set; }
        //public RoleManagementModel AdjustmentReport { get; set; }
        //public RoleManagementModel StockTransactionReport { get; set; }
        public RoleManagementModel AverageMonthlyReport { get; set; }
        public RoleManagementModel NetIncomeReport { get; set; }
       
        public RoleManagementModel TicketDetailReport { get; set; } /* HMT*/
        public RoleManagementModel TicketSummaryReport { get; set; }   /* HMT*/
        //public RoleManagementModel StockManagement { get; set; }
        private int UserRoleId { get; set; }

        public RoleManagementController()
        {
            Setting = new RoleManagementModel();
            City = new RoleManagementModel();
            Product = new RoleManagementModel();
            Brand = new RoleManagementModel();
            Category = new RoleManagementModel();
            SubCategory = new RoleManagementModel();
            Counter = new RoleManagementModel();
            Transaction = new RoleManagementModel();
            TransactionDetail = new RoleManagementModel();
            CSV = new RoleManagementModel();
            DailySaleSummary = new RoleManagementModel();
            TransactionReport = new RoleManagementModel();
            TransactionSummaryReport = new RoleManagementModel();
            TransactionDetailReport = new RoleManagementModel();
            DailyTotalTransactions = new RoleManagementModel();
            ItemSummaryReport = new RoleManagementModel();
            SaleBreakdown = new RoleManagementModel();
            TopBestSellerReport = new RoleManagementModel();
            CustomerInformation = new RoleManagementModel();
            ProductReport = new RoleManagementModel();
            ProfitAndLoss = new RoleManagementModel();
            AverageMonthlyReport = new RoleManagementModel();
            NetIncomeReport = new RoleManagementModel();
            TicketDetailReport = new RoleManagementModel();   /*HMT*/
            TicketSummaryReport = new RoleManagementModel();    /*HMT*/
            Ticket = new RoleManagementModel();
        }




        public void Load(int roleId)
        {
            UserRoleId = roleId;


            POSEntities entity = new POSEntities();
            //Setting
            Setting.Add = LoadRules(entity, "setting_define");

            //City
            City.EditOrDelete = LoadRules(entity, "city_edit");
            City.Add = LoadRules(entity, "city_add");

            //Product
            Product.View = LoadRules(entity, "product_view");
            Product.EditOrDelete = LoadRules(entity, "product_edit");
            Product.Add = LoadRules(entity, "product_add");
            //Brand
            Brand.View = LoadRules(entity, "brand_view");
            Brand.EditOrDelete = LoadRules(entity, "brand_edit");
            Brand.Add = LoadRules(entity, "brand_add");
            //Category
            Category.View = LoadRules(entity, "category_view");
            Category.EditOrDelete = LoadRules(entity, "category_edit");
            Category.Add = LoadRules(entity, "category_add");
            //Sub Category
            SubCategory.View = LoadRules(entity, "subcategory_view");
            SubCategory.EditOrDelete = LoadRules(entity, "subcategory_edit");
            SubCategory.Add = LoadRules(entity, "subcategory_add");
            //Counter
            Counter.EditOrDelete = LoadRules(entity, "counter_edit");
            Counter.Add = LoadRules(entity, "counter_add");
            //Transaction
            Transaction.EditOrDelete = LoadRules(entity, "transaction_delete");
            Transaction.DeleteAndCopy = LoadRules(entity, "transaction_deleteandcopy");

            // Transaction Detail
            TransactionDetail.EditOrDelete = LoadRules(entity, "transactionDetail_delete");

            //Reports
            DailySaleSummary.View = LoadRules(entity, "dailySaleSummary_view");
            TransactionReport.View = LoadRules(entity, "transactionReport_view");
            TransactionSummaryReport.View = LoadRules(entity, "transactionSummary_view");
            TransactionDetailReport.View = LoadRules(entity, "transactionDetailReport_view");
            DailyTotalTransactions.View = LoadRules(entity, "dailyTotalTransactions_view");
            ItemSummaryReport.View = LoadRules(entity, "itemSummaryReport_view");
            SaleBreakdown.View = LoadRules(entity, "saleBreakdown_view");
            TopBestSellerReport.View = LoadRules(entity, "topBestSellerReport_view");
            CustomerInformation.View = LoadRules(entity, "customerInformation_view");
            ProductReport.View = LoadRules(entity, "productReport_view");
            ProfitAndLoss.View = LoadRules(entity, "ProfitAndLoss_view");
            AverageMonthlyReport.View = LoadRules(entity, "AverageMonthlyReport_view");
            NetIncomeReport.View = LoadRules(entity, "NetIncomeReport_view");
            TicketDetailReport.View = LoadRules(entity, "TicketDetailReport_view");  /* HMT*/
            TicketSummaryReport.View = LoadRules(entity, "TicketSummaryReport_view");  /* HMT*/

            Ticket.Reprint = LoadRules(entity, "Ticket_reprint"); 
        }

        public void Save(int roleId)
        {
            UserRoleId = roleId;
            POSEntities entity = new POSEntities();

            //Delete old entry for this userroldId firstly
            List<APP_Data.RoleManagement> RulesListById = entity.RoleManagements.Where(x => x.UserRoleId == UserRoleId).ToList();
            foreach (APP_Data.RoleManagement rule in RulesListById)
            {
                entity.RoleManagements.Remove(rule);
            }
            
            //Setting
            CreateRules(entity, Setting.Add, "setting_define");

            //City
            CreateRules(entity, City.EditOrDelete, "city_edit");
            CreateRules(entity, City.Add, "city_add");

            //Product
            CreateRules(entity, Product.View, "product_view");
            CreateRules(entity, Product.EditOrDelete, "product_edit");
            CreateRules(entity, Product.Add, "product_add");
            //Brand
            CreateRules(entity, Brand.View, "brand_view");
            CreateRules(entity, Brand.EditOrDelete, "brand_edit");
            CreateRules(entity, Brand.Add, "brand_add");
            //Category
            CreateRules(entity, Category.View, "category_view");
            CreateRules(entity, Category.EditOrDelete, "category_edit");
            CreateRules(entity, Category.Add, "category_add");
            //Sub Category
            CreateRules(entity, SubCategory.View, "subcategory_view");
            CreateRules(entity, SubCategory.EditOrDelete, "subcategory_edit");
            CreateRules(entity, SubCategory.Add, "subcategory_add");
            //Counter
            CreateRules(entity, Counter.EditOrDelete, "counter_edit");
            CreateRules(entity, Counter.Add, "counter_add");
            // Transaction
            CreateRules(entity, Transaction.EditOrDelete, "transaction_delete");
            CreateRules(entity, Transaction.DeleteAndCopy, "transaction_deleteandcopy");

            // Transaction Detail
            CreateRules(entity, TransactionDetail.EditOrDelete, "transactionDetail_delete");

           //Reports
            CreateRules(entity, DailySaleSummary.View, "dailySaleSummary_view");
            CreateRules(entity, TransactionReport.View, "transactionReport_view");
            CreateRules(entity, TransactionSummaryReport.View, "transactionSummary_view");
            CreateRules(entity, TransactionDetailReport.View, "transactionDetailReport_view");
            CreateRules(entity, DailyTotalTransactions.View, "dailyTotalTransactions_view");
            CreateRules(entity, ItemSummaryReport.View, "itemSummaryReport_view");
            CreateRules(entity, TopBestSellerReport.View, "topBestSellerReport_view");
            CreateRules(entity, ProductReport.View, "productReport_view");
            CreateRules(entity, ProfitAndLoss.View, "ProfitAndLoss_view");
            CreateRules(entity, AverageMonthlyReport.View, "AverageMonthlyReport_view");
            CreateRules(entity, NetIncomeReport.View, "NetIncomeReport_view");
            CreateRules(entity, TicketDetailReport.View, "TicketDetailReport_view");    /*HMT*/
            CreateRules(entity, TicketSummaryReport.View, "TicketSummaryReport_view");    /*HMT*/

            CreateRules(entity, Ticket.Reprint, "Ticket_reprint");    /*HMT*/
        }

        private void CreateRules(POSEntities entity, Boolean IsAllowed, String Rule)
        {
            APP_Data.RoleManagement obj = new APP_Data.RoleManagement();
            obj.UserRoleId = UserRoleId;
            obj.IsAllowed = IsAllowed;
            obj.RuleFeature = Rule;
            entity.RoleManagements.Add(obj);
            entity.SaveChanges();
        }

        private Boolean LoadRules(POSEntities entity, String Rule)
        {
            APP_Data.RoleManagement obj = entity.RoleManagements.Where(x => x.RuleFeature == Rule && x.UserRoleId == UserRoleId).FirstOrDefault();
            Boolean result = false;
            if (obj != null) result = obj.IsAllowed;

            return result;
        }

        public List<bool> IsAllAllowedForOperation(int roleId)
        {
            // First, Make a List of Rule Name which we will show in Role Management Form in Operation Pannel
            #region Rule Name List
            List<string> RuleNameList = new List<string>(new string[]{
            "product_view",
            "product_view",
            "product_edit",

            "brand_view",
            "brand_edit",
            "brand_add",

            "category_view",
            "category_edit",
            "category_add",

            "subcategory_view",
            "subcategory_edit",
            "subcategory_add",

            "counter_edit",
            "counter_add",

            "supplier_view",
            "supplier_edit",
            "supplier_add",
            "supplier_viewDetail",

            "transaction_delete",
            "transaction_deleteandcopy",

            "transactionDetail_delete",

            });
            #endregion

            POSEntities entity = new POSEntities();
            var obj = (from rm in entity.RoleManagements.AsEnumerable()
                       join r in RuleNameList on rm.RuleFeature.Trim() equals r
                       where rm.UserRoleId == roleId
                       select rm.IsAllowed).ToList();

            return obj;
        }

        public List<bool> IsAllAllowedForReport(int roleId)
        {
            // First, Make a List of Rule Name which we will show in Role Management Form in Report Pannel
            #region Rule Name List
            List<string> RuleNameList = new List<string>(new string[]{
            "dailySaleSummary_view",
            "transactionReport_view",
            "transactionSummary_view",
            "transactionDetailReport_view",
            "dailyTotalTransactions_view",
            "itemSummaryReport_view",
            "topBestSellerReport_view",
            "productReport_view",
            "ProfitAndLoss_view",
            "AverageMonthlyReport_view",
            "NetIncomeReport_view",
            "TicketDetailReport_view",
            "TicketSummaryReport_view",
            "Ticket_reprint"
            });
            #endregion
         
            POSEntities entity = new POSEntities();
            var obj = (from rm in entity.RoleManagements.AsEnumerable()
                       join r in RuleNameList on rm.RuleFeature.Trim() equals r
                       where rm.UserRoleId == roleId
                       select rm.IsAllowed).ToList();

            return obj;
        }
    }

    public class RoleManagementModel
    {
        public Boolean View { get; set; }
        public Boolean EditOrDelete { get; set; }
        public Boolean Add { get; set; }
        public Boolean ViewDetail { get; set; }
        public Boolean DeleteAndCopy { get; set; }
        public Boolean DeleteLog { get; set; }
        public Boolean Approved { get; set; }

        public Boolean Reprint { get; set; }
    }
    /// <summary>
    /// Perform translation of Controls' Text to desire language.
    /// </summary>
    public class Localization
    {
        static string ZawGyi = "Zawgyi-one";
        static string Myanmar3 = "Myanmar3";
        static string Other1 = "Zawgyi-one";
        static string Other2 = "Zawgyi-one";
        static bool AllowType(Type type)
        {
            if (type == typeof(Label) || type == typeof(GroupBox) || type == typeof(DataGridView) || type == typeof(RadioButton) || type == typeof(CheckBox) || type == typeof(Chart))
            {
                return true;
            }
            return false;
        }
        private static void ControlInsert(Control ct, pjForm dbform)
        {
            POSEntities db = new POSEntities();
            if (ct.GetType() == typeof(MenuStrip))
            {
                MenuStrip menu = (MenuStrip)ct;
                foreach (var tsmi in menu.Items)
                {
                    ToolStripMenuItem tsm = tsmi as ToolStripMenuItem;
                    pjForms_Localization cont = new pjForms_Localization();
                    cont.Id = db.pjForms_Localization.Count() > 0 ? db.pjForms_Localization.Max(a => a.Id) + 1 : 1;
                    cont.FormId = dbform.Id;
                    cont.ControlName = tsm.Name;
                    cont.Type = "MenuItem";
                    cont.Eng = tsm.Text;
                    cont.ZawGyi = "";
                    cont.MM3 = "";
                    cont.Other1 = "";
                    cont.Other2 = "";
                    cont.AllowToLoad = true;
                    db.pjForms_Localization.Add(cont);
                    db.SaveChanges();

                    if (tsm.HasDropDownItems)
                    {
                        if (tsm.Name != "fileMenu")
                        {
                            foreach (var ddi in tsm.DropDownItems)
                            {

                                ToolStripMenuItem ddtsm = ddi as ToolStripMenuItem;
                                if (ddtsm != null)
                                {
                                    pjForms_Localization conte = new pjForms_Localization();
                                    conte.Id = db.pjForms_Localization.Count() > 0 ? db.pjForms_Localization.Max(a => a.Id) + 1 : 1;
                                    conte.FormId = dbform.Id;
                                    conte.ControlName = ddtsm.Name;
                                    conte.Type = "MenuItem";
                                    conte.Eng = ddtsm.Text;
                                    conte.ZawGyi = "";
                                    conte.MM3 = "";
                                    conte.Other1 = "";
                                    conte.Other2 = "";
                                    conte.AllowToLoad = true;
                                    db.pjForms_Localization.Add(conte);
                                    db.SaveChanges();
                                }
                            }
                        }

                    }
                }
            }
            else if (ct.GetType() == typeof(Label))
            {
                Label lab = (Label)ct;
                pjForms_Localization cont = new pjForms_Localization();
                cont.Id = db.pjForms_Localization.Count() > 0 ? db.pjForms_Localization.Max(a => a.Id) + 1 : 1;
                cont.FormId = dbform.Id;
                cont.ControlName = lab.Name;
                cont.Type = "Label";
                cont.Eng = lab.Text;
                cont.ZawGyi = "";
                cont.MM3 = "";
                cont.Other1 = "";
                cont.Other2 = "";
                cont.AllowToLoad = lab.Name.Length >= 4 && lab.Name.Substring(0, 4) == "label" || string.IsNullOrEmpty(lab.Text) ? false : true;
                db.pjForms_Localization.Add(cont);
                db.SaveChanges();

            }
            else if (ct.GetType() == typeof(DataGridView))
            {
                DataGridView dgv = (DataGridView)ct;
                pjForms_Localization cont = new pjForms_Localization();
                cont.Id = db.pjForms_Localization.Count() > 0 ? db.pjForms_Localization.Max(a => a.Id) + 1 : 1; 
                cont.FormId = dbform.Id;
                cont.ControlName = dgv.Name;
                cont.Type = "GridView";
                cont.Eng = dgv.Text;
                cont.ZawGyi = "";
                cont.MM3 = "";
                cont.Other1 = "";
                cont.Other2 = "";
                cont.AllowToLoad = true;
                db.pjForms_Localization.Add(cont);
                db.SaveChanges();

            }
            else if (ct.GetType() == typeof(GroupBox))
            {
                GroupBox dgv = (GroupBox)ct;
                pjForms_Localization cont = new pjForms_Localization();
                cont.Id = db.pjForms_Localization.Count() > 0 ? db.pjForms_Localization.Max(a => a.Id) + 1 : 1; ;
                cont.FormId = dbform.Id;
                cont.ControlName = dgv.Name;
                cont.Type = "GroupBox";
                cont.Eng = dgv.Text;
                cont.ZawGyi = "";
                cont.MM3 = "";
                cont.Other1 = "";
                cont.Other2 = "";
                cont.AllowToLoad = dgv.Name.Length >= 7 && dgv.Name.Substring(0, 7) == "groupbox" || string.IsNullOrEmpty(dgv.Text) ? false : true;
                db.pjForms_Localization.Add(cont);
                db.SaveChanges();
            }
            else if (ct.GetType() == typeof(RadioButton))
            {
                RadioButton rdo = (RadioButton)ct;
                pjForms_Localization cont = new pjForms_Localization();
                cont.Id = db.pjForms_Localization.Count() > 0 ? db.pjForms_Localization.Max(a => a.Id) + 1 : 1; ;
                cont.FormId = dbform.Id;
                cont.ControlName = rdo.Name;
                cont.Type = "Radio";
                cont.Eng = rdo.Text;
                cont.ZawGyi = "";
                cont.MM3 = "";
                cont.Other1 = "";
                cont.Other2 = "";
                cont.AllowToLoad = rdo.Name.Length >= 11 && rdo.Name.Substring(0, 11) == "radiobutton" || string.IsNullOrEmpty(rdo.Text) ? false : true;
                db.pjForms_Localization.Add(cont);
                db.SaveChanges();
            }
            else if (ct.GetType() == typeof(CheckBox))
            {
                CheckBox cbx = (CheckBox)ct;
                pjForms_Localization cont = new pjForms_Localization();
                cont.Id = db.pjForms_Localization.Count() > 0 ? db.pjForms_Localization.Max(a => a.Id) + 1 : 1; ;
                cont.FormId = dbform.Id;
                cont.ControlName = cbx.Name;
                cont.Type = "CheckBox";
                cont.Eng = cbx.Text;
                cont.ZawGyi = "";
                cont.MM3 = "";
                cont.Other1 = "";
                cont.Other2 = "";
                cont.AllowToLoad = cbx.Name.Length >= 8 && cbx.Name.Substring(0, 8) == "checkbox" || string.IsNullOrEmpty(cbx.Text) ? false : true;
                db.pjForms_Localization.Add(cont);
                db.SaveChanges();
            }
            if (ct.GetType() == typeof(System.Windows.Forms.DataVisualization.Charting.Chart))
            {
                System.Windows.Forms.DataVisualization.Charting.Chart chart = (System.Windows.Forms.DataVisualization.Charting.Chart)ct;
                pjForms_Localization cont = new pjForms_Localization();
                cont.Id = db.pjForms_Localization.Count() > 0 ? db.pjForms_Localization.Max(a => a.Id) + 1 : 1; ;
                cont.FormId = dbform.Id;
                cont.ControlName = chart.Titles.Select(a => a.Name).FirstOrDefault().ToString();
                cont.Type = "Chart";
                cont.Eng = chart.Titles.Select(a => a.Text).FirstOrDefault().ToString();
                cont.ZawGyi = "";
                cont.MM3 = "";
                cont.Other1 = "";
                cont.Other2 = "";
                cont.AllowToLoad = true;
                db.pjForms_Localization.Add(cont);
                db.SaveChanges();
            }
        }
        /// <summary>
        /// Insert Foms and Contols of Assembly to database.
        /// </summary>
        public static void Forms_Controls_Insertion()
        {
           
            #region Variables
            POSEntities db = new POSEntities();
            List<Form> allFormList = new List<Form>();
            Assembly pos = Assembly.Load("POS");
            var existingForms = db.pjForms.Select(p => new { Name = p.Name }).ToList();
            List<pjForms_Localization> existingControls = db.pjForms_Localization.ToList();
            List<pjForms_Localization> newlyAddedControls = new List<pjForms_Localization>();
            bool isFormsFirstTime = existingForms.Count > 0 ? false : true;
            bool isControlsFirstTime = existingControls.Count > 0 ? false : true;
            #endregion
            #region FormInsert
            if (db.pjForms.Count() == 0)
            {
                foreach (var t in pos.GetTypes())
                {
                    if (t.BaseType == typeof(Form))
                    {
                        var emptyCtor = t.GetConstructor(Type.EmptyTypes);
                        if (emptyCtor != null)
                        {
                            var f = (Form)emptyCtor.Invoke(new object[] { });
                            allFormList.Add(f);
                        }
                    }
                }
                if (isFormsFirstTime)
                {
                    List<pjForm> toSaveForms = new List<pjForm>();
                    foreach (Form f in allFormList)
                    {
                        pjForm toSaveForm = new pjForm();
                        toSaveForm.Name = f.Name;
                        toSaveForm.TextEng = f.Text;
                        toSaveForm.AllowToLoad = f.Controls.OfType<Label>().Count() > 0 ||
                                                  f.Controls.OfType<DataGridView>().Count() > 0 ||
                                                  f.Controls.OfType<GroupBox>().Count() > 0 ||
                                                  f.Controls.OfType<MenuStrip>().Count() > 0 ||
                                                  f.Controls.OfType<Panel>().Count() > 0 ||
                                                  f.Controls.OfType<FlowLayoutPanel>().Count() > 0 ||
                                                  f.Controls.OfType<TableLayoutPanel>().Count() > 0 ||
                                                   f.Controls.OfType<TabControl>().Count() > 0 ? true : false;
                        toSaveForm.TextMyanmar = "";
                        db.pjForms.Add(toSaveForm);
                        db.SaveChanges();
                    }
                }
                else
                {
                    var projectsForms = allFormList.Select(f => new { Name = f.Name }).ToList();
                    var newlyAddedForms = projectsForms.Except(existingForms).ToList();
                    if (newlyAddedForms.Count > 0)
                    {
                        foreach (var item in newlyAddedForms)
                        {
                            pjForm newly = new pjForm();
                            Form f = allFormList.Where(a => a.Name == item.Name).FirstOrDefault();
                            newly.Name = f.Name;
                            newly.TextEng = f.Text;
                            newly.TextMyanmar = "";
                            newly.AllowToLoad = f.Controls.OfType<Label>().Count() > 0 ||
                                                  f.Controls.OfType<DataGridView>().Count() > 0 ||
                                                  f.Controls.OfType<GroupBox>().Count() > 0 ||
                                                  f.Controls.OfType<MenuStrip>().Count() > 0 ||
                                                  f.Controls.OfType<Panel>().Count() > 0 ||
                                                  f.Controls.OfType<FlowLayoutPanel>().Count() > 0 ||
                                                  f.Controls.OfType<TableLayoutPanel>().Count() > 0 ||
                                                   f.Controls.OfType<TabControl>().Count() > 0 ? true : false;
                            db.pjForms.Add(newly);
                            db.SaveChanges();
                        }
                    }
                }

            #endregion
                #region ControlInsert
                if (db.pjForms.Count() > 0)
                {
                    foreach (var dbform in db.pjForms.Where(a => a.AllowToLoad == true).ToList())
                    {
                        var formType = Assembly.GetExecutingAssembly().GetTypes()
                                  .Where(a => a.BaseType == typeof(Form) && a.Name == dbform.Name)
                                  .FirstOrDefault();

                        Form form = (Form)Activator.CreateInstance(formType);
                        foreach (var ct in GetAll(form, null).ToList())
                        {

                            if (!isControlsFirstTime && db.pjForms_Localization.Where(a => a.ControlName == ct.Name && a.pjForm.Name == dbform.Name).FirstOrDefault() == null && ct.GetType() != typeof(MenuStrip) && AllowType(ct.GetType()))
                            {
                                ControlInsert(ct, dbform);
                            }
                            else if (isControlsFirstTime)
                            {
                                ControlInsert(ct, dbform);
                            }
                        }
                    }
                }
            }
                #endregion
        }
        private static IEnumerable<Control> GetAll(Control control, Type type = null)
        {
            var controls = control.Controls.Cast<Control>();

            if (type == null)
                return controls.SelectMany(ctrl => GetAll(ctrl, type)).Concat(controls);
            else
                return controls.SelectMany(ctrl => GetAll(ctrl, type)).Concat(controls).Where(c => c.GetType() == type);
        }
        static Font DefaultFont(string defaultFont, string formname = null)
        {
            switch (defaultFont)
            {
                case "ZawGyi":
                    return formname == "chart" || formname == "dashboard" ? new Font(ZawGyi, 10, FontStyle.Bold) : new Font(ZawGyi, 9, FontStyle.Regular);
                case "MM3":
                    return formname == "chart" || formname == "dashboard" ? new Font(Myanmar3, 10, FontStyle.Bold) : new Font(Myanmar3, 9, FontStyle.Regular);
                case "Other1":
                    return formname == "chart" || formname == "dashboard" ? new Font(Other1, 10, FontStyle.Bold) : new Font(Other1, 9, FontStyle.Regular); ;
                case "Other2":
                    return formname == "chart" || formname == "dashboard" ? new Font(Other2, 10, FontStyle.Bold) : new Font(Other2, 9, FontStyle.Regular); ;
                default:
                    return new Font(ZawGyi, 9, FontStyle.Regular);
            }
        }
        /// <summary>
        /// Translate all controls of desired type of given form.
        /// </summary>
        /// <param name="form"></param>
        public static void Localize_FormControls(Form form)
        {
            //if(!SettingController.TopMost_Para)
            //{
            //    SettingController.TopMost_Para = SettingController.TopMost;
            //}
            if (SettingController.TopMost)
            {
                form.TopMost = true;
            }
            POSEntities db = new POSEntities();
            pjForm dbForm =
                db.pjForms.AsEnumerable()
                .Where(f => f.Name == form.Name && f.AllowToLoad == true).FirstOrDefault();

            if (dbForm != null)
            {
                string defaultFont = SettingController.DefaultFont;
                if (defaultFont != "English")
                {
                    string FORMNAME = form.Name.ToLower();
                    foreach (var c in GetAll(form, null).ToList())
                    {
                        if (c.GetType() == typeof(MenuStrip))
                        {
                            MenuStrip menu = (MenuStrip)c;
                            foreach (var tsm in menu.Items)
                            {

                                ToolStripMenuItem tsmi = tsm as ToolStripMenuItem;
                                tsmi.Font = DefaultFont(defaultFont, FORMNAME);
                                var dbC = db.pjForms_Localization.Where(p => p.ControlName == tsmi.Name).FirstOrDefault();
                                if (defaultFont == "ZawGyi" && dbC != null && !string.IsNullOrEmpty(dbC.ZawGyi) && !string.IsNullOrWhiteSpace(dbC.ZawGyi))
                                {
                                    tsmi.Text = dbC.ZawGyi;
                                }
                                else if (defaultFont == "Myanmar3" && dbC != null && !string.IsNullOrEmpty(dbC.MM3) && !string.IsNullOrWhiteSpace(dbC.MM3))
                                {
                                    tsmi.Text = dbC.MM3;
                                }
                                else if (defaultFont == "Other1" && dbC != null && !string.IsNullOrEmpty(dbC.Other1) && !string.IsNullOrWhiteSpace(dbC.Other1))
                                {
                                    tsmi.Text = dbC.Other1;
                                }
                                else if (defaultFont == "Other2" && dbC != null && !string.IsNullOrEmpty(dbC.Other2) && !string.IsNullOrWhiteSpace(dbC.Other2))
                                {
                                    tsmi.Text = dbC.Other2;
                                }
                                else if (dbC != null)
                                {
                                    tsmi.Text = dbC.Eng;
                                }
                                if (tsmi.HasDropDownItems)
                                {
                                    if (tsmi.Name != "fileMenu")
                                    {
                                        foreach (var child in tsmi.DropDownItems)
                                        {
                                            ToolStripMenuItem childtsmi = child as ToolStripMenuItem;
                                            childtsmi.Font = DefaultFont(defaultFont, FORMNAME);
                                            var dbChild = db.pjForms_Localization.Where(p => p.ControlName == childtsmi.Name && p.AllowToLoad == true).FirstOrDefault();
                                            if (defaultFont == "ZawGyi" && dbChild != null && !string.IsNullOrWhiteSpace(dbChild.ZawGyi))
                                            {
                                                childtsmi.Text = dbChild.ZawGyi;
                                            }
                                            else if (defaultFont == "Myanmar3" && dbChild != null && !string.IsNullOrWhiteSpace(dbChild.MM3))
                                            {
                                                childtsmi.Text = dbChild.MM3;
                                            }
                                            else if (defaultFont == "Other1" && dbChild != null && !string.IsNullOrWhiteSpace(dbChild.Other1))
                                            {
                                                childtsmi.Text = dbChild.Other1;
                                            }
                                            else if (defaultFont == "Other2" && dbChild != null && !string.IsNullOrWhiteSpace(dbChild.Other2))
                                            {
                                                childtsmi.Text = dbChild.Other2;
                                            }
                                            else if (dbChild != null)
                                            {
                                                childtsmi.Text = dbChild.Eng;
                                            }
                                        }
                                    }

                                }
                            }

                        }
                        else if (c.GetType() == typeof(Label))
                        {
                            Label label = c as Label;
                            label.Font = DefaultFont(defaultFont, FORMNAME);
                            var dbLabel = db.pjForms_Localization.Where(p => p.ControlName == label.Name && p.pjForm.Name == form.Name && p.AllowToLoad == true).FirstOrDefault();
                            if (defaultFont == "ZawGyi" && dbLabel != null && !string.IsNullOrWhiteSpace(dbLabel.ZawGyi))
                            {
                                label.Text = dbLabel.ZawGyi;
                            }
                            else if (defaultFont == "Myanmar3" && dbLabel != null && !string.IsNullOrWhiteSpace(dbLabel.MM3))
                            {
                                label.Text = dbLabel.MM3;
                            }
                            else if (defaultFont == "Other1" && dbLabel != null && !string.IsNullOrWhiteSpace(dbLabel.Other1))
                            {
                                label.Text = dbLabel.Other1;
                            }
                            else if (defaultFont == "Other2" && dbLabel != null && !string.IsNullOrWhiteSpace(dbLabel.Other2))
                            {
                                label.Text = dbLabel.Other2;
                            }
                            else if (dbLabel != null)
                            {
                                label.Text = dbLabel.Eng;
                            }
                        }
                        else if (c.GetType() == typeof(GroupBox))
                        {
                            GroupBox groupbox = c as GroupBox;
                            groupbox.Font = DefaultFont(defaultFont, FORMNAME);
                            var dbLabel = db.pjForms_Localization.Where(p => p.ControlName == groupbox.Name && p.pjForm.Name == form.Name && p.AllowToLoad == true).FirstOrDefault();
                            if (defaultFont == "ZawGyi" && dbLabel != null && !string.IsNullOrWhiteSpace(dbLabel.ZawGyi))
                            {
                                groupbox.Text = dbLabel.ZawGyi;
                            }
                            else if (defaultFont == "Myanmar3" && dbLabel != null && !string.IsNullOrWhiteSpace(dbLabel.MM3))
                            {
                                groupbox.Text = dbLabel.MM3;
                            }
                            else if (defaultFont == "Other1" && dbLabel != null && !string.IsNullOrWhiteSpace(dbLabel.Other1))
                            {
                                groupbox.Text = dbLabel.Other1;
                            }
                            else if (defaultFont == "Other2" && dbLabel != null && !string.IsNullOrWhiteSpace(dbLabel.Other2))
                            {
                                groupbox.Text = dbLabel.Other2;
                            }
                            else if (dbLabel != null)
                            {
                                groupbox.Text = dbLabel.Eng;
                            }
                        }
                        else if (c.GetType() == typeof(RadioButton))
                        {
                            RadioButton groupbox = c as RadioButton;
                            groupbox.Font = DefaultFont(defaultFont, FORMNAME);
                            var dbLabel = db.pjForms_Localization.Where(p => p.ControlName == groupbox.Name && p.pjForm.Name == form.Name && p.AllowToLoad == true).FirstOrDefault();
                            if (defaultFont == "ZawGyi" && dbLabel != null && !string.IsNullOrWhiteSpace(dbLabel.ZawGyi))
                            {
                                groupbox.Text = dbLabel.ZawGyi;
                            }
                            else if (defaultFont == "Myanmar3" && dbLabel != null && !string.IsNullOrWhiteSpace(dbLabel.MM3))
                            {
                                groupbox.Text = dbLabel.MM3;
                            }
                            else if (defaultFont == "Other1" && dbLabel != null && !string.IsNullOrWhiteSpace(dbLabel.Other1))
                            {
                                groupbox.Text = dbLabel.Other1;
                            }
                            else if (defaultFont == "Other2" && dbLabel != null && !string.IsNullOrWhiteSpace(dbLabel.Other2))
                            {
                                groupbox.Text = dbLabel.Other2;
                            }
                            else if (dbLabel != null)
                            {
                                groupbox.Text = dbLabel.Eng;
                            }
                        }
                        else if (c.GetType() == typeof(CheckBox))
                        {
                            CheckBox groupbox = c as CheckBox;
                            groupbox.Font = DefaultFont(defaultFont, FORMNAME);
                            var dbLabel = db.pjForms_Localization.Where(p => p.ControlName == groupbox.Name && p.pjForm.Name == form.Name && p.AllowToLoad == true).FirstOrDefault();
                            if (defaultFont == "ZawGyi" && dbLabel != null && !string.IsNullOrWhiteSpace(dbLabel.ZawGyi))
                            {
                                groupbox.Text = dbLabel.ZawGyi;
                            }
                            else if (defaultFont == "Myanmar3" && dbLabel != null && !string.IsNullOrWhiteSpace(dbLabel.MM3))
                            {
                                groupbox.Text = dbLabel.MM3;
                            }
                            else if (defaultFont == "Other1" && dbLabel != null && !string.IsNullOrWhiteSpace(dbLabel.Other1))
                            {
                                groupbox.Text = dbLabel.Other1;
                            }
                            else if (defaultFont == "Other2" && dbLabel != null && !string.IsNullOrWhiteSpace(dbLabel.Other2))
                            {
                                groupbox.Text = dbLabel.Other2;
                            }
                            else if (dbLabel != null)
                            {
                                groupbox.Text = dbLabel.Eng;
                            }
                        }
                        else if (c.GetType() == typeof(System.Windows.Forms.DataVisualization.Charting.Chart))
                        {
                            System.Windows.Forms.DataVisualization.Charting.Chart chart = (System.Windows.Forms.DataVisualization.Charting.Chart)c;
                            string titleName = ""; string titleText = "";
                            titleName = chart.Titles.Select(a => a.Name).FirstOrDefault().ToString();
                            titleText = chart.Titles.Select(a => a.Text).FirstOrDefault().ToString();
                            chart.Titles.FirstOrDefault().Font = DefaultFont(defaultFont, FORMNAME);
                            //chart.Font = DefaultFont(defaultFont, FORMNAME);

                            var dbLabel = db.pjForms_Localization.Where(p => p.ControlName == titleName && p.pjForm.Name == form.Name && p.AllowToLoad == true).FirstOrDefault();
                            if (defaultFont == "ZawGyi" && dbLabel != null && !string.IsNullOrWhiteSpace(dbLabel.ZawGyi))
                            {
                                chart.Titles.FirstOrDefault().Text = dbLabel.ZawGyi;
                            }
                            else if (defaultFont == "Myanmar3" && dbLabel != null && !string.IsNullOrWhiteSpace(dbLabel.MM3))
                            {
                                chart.Titles.FirstOrDefault().Text = dbLabel.MM3;
                            }
                            else if (defaultFont == "Other1" && dbLabel != null && !string.IsNullOrWhiteSpace(dbLabel.Other1))
                            {
                                chart.Titles.FirstOrDefault().Text = dbLabel.Other1;
                            }
                            else if (defaultFont == "Other2" && dbLabel != null && !string.IsNullOrWhiteSpace(dbLabel.Other2))
                            {
                                chart.Titles.FirstOrDefault().Text = dbLabel.Other2;
                            }
                            else if (dbLabel != null)
                            {
                                chart.Titles.FirstOrDefault().Text = dbLabel.Eng;
                            }
                        }
                        else if (c.GetType()==typeof(ReportViewer))
                        {
                            ReportViewer rvr = c as ReportViewer;
                            rvr.Font = DefaultFont(defaultFont, FORMNAME);
                        }

                    }
                }
            }
        }
    }

   
    public static class SettingController
    {
        public static void SetGlobalDateFormat(DateTimePicker dtpGlobal)
        {
            dtpGlobal.Format = DateTimePickerFormat.Custom;
            dtpGlobal.CustomFormat = "dd/MMM/yyyy";
            dtpGlobal.Size= new System.Drawing.Size(115, 24);
        }
        public static string GlobalDateFormat
        {
            get { return "dd/MMM/yyyy"; }
        }
        public static IQueryable<POS.APP_Data.Setting> generalEntity
        {
            get
            {
                POSEntities entity = new POSEntities();
                return entity.Settings;
            }
            
        }
        //public static Boolean TopMost_Para;
        public static Boolean TopMost
        {
            get
            {
                APP_Data.Setting currentSet = Utility.generalEntity_Para.FirstOrDefault(x => x.Key == "topmost");
                if (currentSet != null)
                {
                    return Convert.ToBoolean(currentSet.Value);
                }

                return false;
            }
            set
            {
                POSEntities entity = new POSEntities();
                APP_Data.Setting currentSet = entity.Settings.FirstOrDefault(x => x.Key == "topmost");
                if (currentSet == null)
                {
                    currentSet = new APP_Data.Setting();
                    currentSet.Key = "topmost";
                    currentSet.Value = value.ToString();
                    entity.Settings.Add(currentSet);
                }
                else
                {
                    currentSet.Value = value.ToString();
                }
                entity.SaveChanges();
            }
        }
        public static Boolean AllowMinimize
        {
            get
            {
                APP_Data.Setting currentSet = Utility.generalEntity_Para.FirstOrDefault(x => x.Key == "allow_minimize");
                if (currentSet != null)
                {
                    return Convert.ToBoolean(currentSet.Value);
                }

                return false;
            }
            set
            {
                POSEntities entity = new POSEntities();
                APP_Data.Setting currentSet = entity.Settings.FirstOrDefault(x => x.Key == "allow_minimize");
                if (currentSet == null)
                {
                    currentSet = new APP_Data.Setting();
                    currentSet.Key = "allow_minimize";
                    currentSet.Value = value.ToString();
                    entity.Settings.Add(currentSet);
                }
                else
                {
                    currentSet.Value = value.ToString();
                }
                entity.SaveChanges();
            }
        }
        //public static Boolean TicketSale_Para;
        public static Boolean TicketSale
        {
            get
            {
                return true;
                //APP_Data.Setting currentSet = Utility.generalEntity_Para.FirstOrDefault(x => x.Key == "ticketsale");
                //if (currentSet != null)
                //{
                //    return Convert.ToBoolean(currentSet.Value);
                //}

                //return false;

            }
            set
            {
                POSEntities entity = new POSEntities();
                APP_Data.Setting currentSet = entity.Settings.FirstOrDefault(x => x.Key == "ticketsale");
                if (currentSet == null)
                {
                    currentSet = new APP_Data.Setting();
                    currentSet.Key = "ticketsale";
                    currentSet.Value = value.ToString();
                    entity.Settings.Add(currentSet);
                }
                else
                {
                    currentSet.Value = value.ToString();
                }
                entity.SaveChanges();
            }
        }
        public static Boolean DetectIdle
        {
            get
            {
                APP_Data.Setting currentSet =  Utility.generalEntity_Para.FirstOrDefault(x => x.Key == "detect_idle");
                if (currentSet != null)
                {
                    return Convert.ToBoolean(currentSet.Value);
                }

                return false;
            }
            set
            {
                POSEntities entity = new POSEntities();
                APP_Data.Setting currentSet = entity.Settings.FirstOrDefault(x => x.Key == "detect_idle");
                if (currentSet == null)
                {
                    currentSet = new APP_Data.Setting();
                    currentSet.Key = "detect_idle";
                    currentSet.Value = value.ToString();
                    entity.Settings.Add(currentSet);
                }
                else
                {
                    currentSet.Value = value.ToString();
                }
                entity.SaveChanges();
            }
        }
        public static int IdleTime
        {
            get
            {
                try {
                    APP_Data.Setting currentSet =  Utility.generalEntity_Para.FirstOrDefault(x => x.Key == "idle_Time");
                    if (currentSet != null) {
                        return Convert.ToInt32(currentSet.Value);
                        }
                    }
                catch (Exception ex) {
                    throw;
                    }

                return 0;
            }
            set
            {
                POSEntities entity = new POSEntities();
                APP_Data.Setting currentSet = entity.Settings.FirstOrDefault(x => x.Key == "idle_Time");
                if (currentSet == null)
                {
                    currentSet = new APP_Data.Setting();
                    currentSet.Key = "idle_Time";
                    currentSet.Value = value.ToString();
                    entity.Settings.Add(currentSet);
                }
                else
                {
                    currentSet.Value = value.ToString();
                }
                entity.SaveChanges();
            }
        }
        //public static int ServiceFee_Para;
        //public static int ServiceFee
        //{
        //    get
        //    {
        //        APP_Data.Setting currentSet =  Utility.generalEntity_Para.FirstOrDefault(x => x.Key == "service_fee");
        //        if (currentSet != null)
        //        {
        //            return Convert.ToInt32(currentSet.Value);
        //        }

        //        return 0;


        //    }
        //    set
        //    {
        //        POSEntities entity = new POSEntities();
        //        APP_Data.Setting currentSet = entity.Settings.FirstOrDefault(x => x.Key == "service_fee");
        //        if (currentSet == null)
        //        {
        //            currentSet = new APP_Data.Setting();
        //            currentSet.Key = "service_fee";
        //            currentSet.Value = value.ToString();
        //            entity.Settings.Add(currentSet);
        //        }
        //        else
        //        {
        //            currentSet.Value = value.ToString();
        //        }
        //        entity.SaveChanges();
        //    }
        //}
        ////public static Boolean UseQueue_Para;
        public static Boolean UseQueue
        {
            get
            {
                APP_Data.Setting currentSet =  Utility.generalEntity_Para.FirstOrDefault(x => x.Key == "usequeue");
                if (currentSet != null)
                {
                    return Convert.ToBoolean(currentSet.Value);
                }

                return false;

            }
            set
            {
                POSEntities entity = new POSEntities();
                APP_Data.Setting currentSet = entity.Settings.FirstOrDefault(x => x.Key == "usequeue");
                if (currentSet == null)
                {
                    currentSet = new APP_Data.Setting();
                    currentSet.Key = "usequeue";
                    currentSet.Value = value.ToString();
                    entity.Settings.Add(currentSet);
                }
                else
                {
                    currentSet.Value = value.ToString();
                }
                entity.SaveChanges();
            }
        }
        //public static Boolean UseTable_Para;
        //public static Boolean UseTable
        //{
        //    get
        //    {
        //        APP_Data.Setting currentSet =  Utility.generalEntity_Para.FirstOrDefault(x => x.Key == "usetable");
        //        if (currentSet != null)
        //        {
        //            return Convert.ToBoolean(currentSet.Value);
        //        }

        //        return false;
        //    }
        //    set
        //    {
        //        POSEntities entity = new POSEntities();
        //        APP_Data.Setting currentSet = entity.Settings.FirstOrDefault(x => x.Key == "usetable");
        //        if (currentSet == null)
        //        {
        //            currentSet = new APP_Data.Setting();
        //            currentSet.Key = "usetable";
        //            currentSet.Value = value.ToString();
        //            entity.Settings.Add(currentSet);
        //        }
        //        else
        //        {
        //            currentSet.Value = value.ToString();
        //        }
        //        entity.SaveChanges();
        //    }
        //}
        ////public static string InventoryControlPattern_Para;
        //public static string InventoryControlPattern
        //{
        //    get
        //    {
        //        APP_Data.Setting currentSet =  Utility.generalEntity_Para.FirstOrDefault(x => x.Key == "retrieve_pattern");
        //        if (currentSet != null)
        //        {
        //            return Convert.ToString(currentSet.Value);
        //        }

              
        //        return string.Empty;
        //    }
        //    set
        //    {
        //        POSEntities entity = new POSEntities();
        //        APP_Data.Setting currentSet = entity.Settings.FirstOrDefault(x => x.Key == "retrieve_pattern");
        //        if (currentSet == null)
        //        {
        //            currentSet = new APP_Data.Setting();
        //            currentSet.Key = "retrieve_pattern";
        //            currentSet.Value = value.ToString();
        //            entity.Settings.Add(currentSet);
        //        }
        //        else
        //        {
        //            currentSet.Value = value.ToString();
        //        }
        //        entity.SaveChanges();
        //    }
        //}
        ////public static Boolean AllowDynamicPrice_Para;
        public static Boolean AllowDynamicPrice
        {
            get
            {
                APP_Data.Setting currentSet =  Utility.generalEntity_Para.FirstOrDefault(x => x.Key == "allow_dynamic");
                if (currentSet != null)
                {
                    return Convert.ToBoolean(currentSet.Value);
                }

                return false;
            }
            set {
                POSEntities entity = new POSEntities();
                APP_Data.Setting currentSet = entity.Settings.FirstOrDefault(x => x.Key == "allow_dynamic");
                if (currentSet == null)
                {
                    currentSet = new APP_Data.Setting();
                    currentSet.Key = "allow_dynamic";
                    currentSet.Value = value.ToString();
                    entity.Settings.Add(currentSet);
                }
                else
                {
                    currentSet.Value = value.ToString();
                }
                entity.SaveChanges();
            }
        }
        public static Boolean IsBackOffice
        {
            get
            {
               
                APP_Data.Setting currentSet =  Utility.generalEntity_Para.FirstOrDefault(x => x.Key == "IsBackOffice");
                if (currentSet != null)
                {
                    return Convert.ToBoolean(Convert.ToInt16( currentSet.Value));
                }

                return false;
            }
            set
            {
                POSEntities entity = new POSEntities();
                APP_Data.Setting currentSet = entity.Settings.FirstOrDefault(x => x.Key == "IsBackOffice");
                if (currentSet == null)
                {
                    currentSet = new APP_Data.Setting();
                    currentSet.Key = "IsBackOffice";
                    currentSet.Value = value.ToString();
                    entity.Settings.Add(currentSet);
                }
                else
                {
                    currentSet.Value = Convert.ToInt16(value).ToString();
                }
                entity.SaveChanges();
            }
        }

        //public static Boolean ShowProductImageIn_A4Reports_Para;
        public static Boolean ShowProductImageIn_A4Reports
        {
            get
            {
                APP_Data.Setting currentSet =  Utility.generalEntity_Para.FirstOrDefault(x => x.Key == "a4image");
                if (currentSet != null)
                {
                    return Convert.ToBoolean(currentSet.Value);
                }

                return false;
            }
            set
            {
                POSEntities entity = new POSEntities();
                APP_Data.Setting currentSet = entity.Settings.FirstOrDefault(x => x.Key == "a4image");
                if (currentSet == null)
                {
                    currentSet = new APP_Data.Setting();
                    currentSet.Key = "a4image";
                    currentSet.Value = value.ToString();
                    entity.Settings.Add(currentSet);
                }
                else
                {
                    currentSet.Value = value.ToString();
                }
                entity.SaveChanges();
            }
        }

        //public static Boolean UpperCase_ProductName_Para;
        public static Boolean UpperCase_ProductName
        {
            get
            {
                APP_Data.Setting currentSet =  Utility.generalEntity_Para.FirstOrDefault(x => x.Key == "uppercase");
                if (currentSet != null)
                {
                    return Convert.ToBoolean(currentSet.Value);
                }

                return false;
            }
            set
            {
                POSEntities entity = new POSEntities();
                APP_Data.Setting currentSet = entity.Settings.FirstOrDefault(x => x.Key == "uppercase");
                if (currentSet == null)
                {
                    currentSet = new APP_Data.Setting();
                    currentSet.Key = "uppercase";
                    currentSet.Value = value.ToString();
                    entity.Settings.Add(currentSet);
                }
                else
                {
                    currentSet.Value = value.ToString();
                }
                entity.SaveChanges();
            }
        }
        public static Boolean UseCustomSKU
        {
            get
            {
                APP_Data.Setting currentSet =  Utility.generalEntity_Para.FirstOrDefault(x => x.Key == "customsku");
                if (currentSet != null)
                {
                    return Convert.ToBoolean(currentSet.Value);
                }

                return false;
            }
            set
            {
                POSEntities entity = new POSEntities();
                APP_Data.Setting currentSet = entity.Settings.FirstOrDefault(x => x.Key == "customsku");
                if (currentSet == null)
                {
                    currentSet = new APP_Data.Setting();
                    currentSet.Key = "customsku";
                    currentSet.Value = value.ToString();
                    entity.Settings.Add(currentSet);
                }
                else
                {
                    currentSet.Value = value.ToString();
                }
                entity.SaveChanges();
            }
        }
        public static Boolean UseStockAutoGenerate
        {
            get
            {
                APP_Data.Setting currentSet =  Utility.generalEntity_Para.FirstOrDefault(x => x.Key == "UseStockAutoGenerate");
                if (currentSet != null)
                {
                    return Convert.ToBoolean(currentSet.Value);
                }

                return false;
            }
            set
            {
                POSEntities entity = new POSEntities();
                APP_Data.Setting currentSet = entity.Settings.FirstOrDefault(x => x.Key == "UseStockAutoGenerate");
                if (currentSet == null)
                {
                    currentSet = new APP_Data.Setting();
                    currentSet.Key = "UseStockAutoGenerate";
                    currentSet.Value = value.ToString();
                    entity.Settings.Add(currentSet);
                }
                else
                {
                    currentSet.Value = value.ToString();
                }
                entity.SaveChanges();
            }
        }

        //public static string ApplicationMode_Para;
        public static string ApplicationMode
        {
            get
            {
                var currentMode = Utility.generalEntity_Para.Where(s => s.Key == "app_mode").FirstOrDefault();
                if (currentMode!=null)
                {
                    return currentMode.Value;
                }
                return string.Empty;
            }
            set
            {
                POSEntities db = new POSEntities();
                var currentMode = db.Settings.Where(a => a.Key == "app_mode").FirstOrDefault();
                if (currentMode==null)
                {
                    currentMode = new APP_Data.Setting();
                    currentMode.Key = "app_mode";
                    currentMode.Value = value.ToString();
                    db.Settings.Add(currentMode);
                }
                else
                {
                    currentMode.Value = value.ToString();
                }
                db.SaveChanges();
            }
        }
        public static string DefaultFont
        {
            get
            {
               
                var fontSetting = Utility.generalEntity_Para.Where(s => s.Key == "default_font").FirstOrDefault();
                if (fontSetting != null)
                {
                    return fontSetting.Value;
                }
                return string.Empty;
            }
            set
            {
                POSEntities db = new POSEntities();
                var fontSetting = db.Settings.Where(s => s.Key == "default_font").FirstOrDefault();
                if (fontSetting == null)
                {
                    fontSetting = new APP_Data.Setting();
                    fontSetting.Key = "default_font";
                    fontSetting.Value = value.ToString();
                    db.Settings.Add(fontSetting);

                }
                else
                {
                    fontSetting.Value = value.ToString();
                }
                db.SaveChanges();
            }
        }

        //public static string FooterPage_Para;
        public static string FooterPage
        {
            get
            {
                
                APP_Data.Setting currentSet =  Utility.generalEntity_Para.FirstOrDefault(x => x.Key == "FooterPage");
                if (currentSet != null)
                {
                    return Convert.ToString(currentSet.Value);
                }

                return string.Empty;
            }
            set
            {
                POSEntities entity = new POSEntities();
                APP_Data.Setting currentSet = entity.Settings.FirstOrDefault(x => x.Key == "FooterPage");
                if (currentSet == null)
                {
                    currentSet = new APP_Data.Setting();
                    currentSet.Key = "FooterPage";
                    currentSet.Value = value.ToString();
                    entity.Settings.Add(currentSet);
                }
                else
                {
                    currentSet.Value = value.ToString();
                }
                entity.SaveChanges();
            }
        }
        //public static string Logo_Para;
        public static string Logo
        {
            get
            {
                
                APP_Data.Setting currentSet =  Utility.generalEntity_Para.FirstOrDefault(x => x.Key == "Logo");
                if (currentSet != null)
                {
                    return Convert.ToString(currentSet.Value);
                }

                return string.Empty;
            }
            set
            {
                POSEntities entity = new POSEntities();
                APP_Data.Setting currentSet = entity.Settings.FirstOrDefault(x => x.Key == "Logo");
                if (currentSet == null)
                {
                    currentSet = new APP_Data.Setting();
                    currentSet.Key = "Logo";
                    currentSet.Value = value.ToString();
                    entity.Settings.Add(currentSet);
                }
                else
                {
                    currentSet.Value = value.ToString();
                }
                entity.SaveChanges();
            }
        }
        //public static string ShopName_Para;
        public static string ShopName
        {
            get
            {
                APP_Data.Setting currentSet =  Utility.generalEntity_Para.FirstOrDefault(x => x.Key == "shop_name");
                if (currentSet != null)
                {
                    return Convert.ToString(currentSet.Value);
                }

                return string.Empty;
            }
            set
            {
                POSEntities entity = new POSEntities();
                APP_Data.Setting currentSet = entity.Settings.FirstOrDefault(x => x.Key == "shop_name");
                if (currentSet == null)
                {
                    currentSet = new APP_Data.Setting();
                    currentSet.Key = "shop_name";
                    currentSet.Value = value.ToString();
                    entity.Settings.Add(currentSet);
                }
                else
                {
                    currentSet.Value = value.ToString();
                }
                entity.SaveChanges();
            }
        }
        //public static APP_Data.Shop DefaultShop_Para;
        public static APP_Data.Shop DefaultShop
        {
            get
            {
                POSEntities entity = new POSEntities();
                APP_Data.Shop defaultShop = entity.Shops.Where(x => x.IsDefaultShop == true).FirstOrDefault();
                return defaultShop;
            }
        }
        //public static string BranchName_Para;
        public static string BranchName
        {
            get
            {
                APP_Data.Setting currentSet =  Utility.generalEntity_Para.FirstOrDefault(x => x.Key == "branch_name");
                if (currentSet != null)
                {
                    return Convert.ToString(currentSet.Value);
                }

                return string.Empty;
            }
            set
            {
                POSEntities entity = new POSEntities();
                APP_Data.Setting currentSet = entity.Settings.FirstOrDefault(x => x.Key == "branch_name");
                if (currentSet == null)
                {
                    currentSet = new APP_Data.Setting();
                    currentSet.Key = "branch_name";
                    currentSet.Value = value.ToString();
                    entity.Settings.Add(currentSet);
                }
                else
                {
                    currentSet.Value = value.ToString();
                }
                entity.SaveChanges();
            }
        }
        //public static string PhoneNo_Para;
        public static string PhoneNo
        {
            get
            {
                APP_Data.Setting currentSet =  Utility.generalEntity_Para.FirstOrDefault(x => x.Key == "phone_number");
                if (currentSet != null)
                {
                    return Convert.ToString(currentSet.Value);
                }

                return string.Empty;
            }
            set
            {
                POSEntities entity = new POSEntities();
                APP_Data.Setting currentSet = entity.Settings.FirstOrDefault(x => x.Key == "phone_number");
                if (currentSet == null)
                {
                    currentSet = new APP_Data.Setting();
                    currentSet.Key = "phone_number";
                    currentSet.Value = value.ToString();
                    entity.Settings.Add(currentSet);
                }
                else
                {
                    currentSet.Value = value.ToString();
                }
                entity.SaveChanges();
            }
        }
        //public static string OpeningHours_Para;
        public static string OpeningHours
        {
            get
            {
                APP_Data.Setting currentSet =  Utility.generalEntity_Para.FirstOrDefault(x => x.Key == "opening_hours");
                if (currentSet != null)
                {
                    return Convert.ToString(currentSet.Value);
                }

                return string.Empty;
            }
            set
            {
                POSEntities entity = new POSEntities();
                APP_Data.Setting currentSet = entity.Settings.FirstOrDefault(x => x.Key == "opening_hours");
                if (currentSet == null)
                {
                    currentSet = new APP_Data.Setting();
                    currentSet.Key = "opening_hours";
                    currentSet.Value = value.ToString();
                    entity.Settings.Add(currentSet);
                }
                else
                {
                    currentSet.Value = value.ToString();
                }
                entity.SaveChanges();
            }
        }

        //public static string DefaultTaxRate_Para;
   
        //public static string SelectDefaultPrinter_Para;
        public static string SelectDefaultPrinter
        {
            get
            {
                APP_Data.Setting currentSet =  Utility.generalEntity_Para.FirstOrDefault(x => x.Key == "default_printer");
                if (currentSet != null)
                {
                    return Convert.ToString(currentSet.Value);
                }

                return string.Empty;
            }
            set
            {
                POSEntities entity = new POSEntities();
                APP_Data.Setting currentSet = entity.Settings.FirstOrDefault(x => x.Key == "default_printer");
                if (currentSet == null)
                {
                    currentSet = new APP_Data.Setting();
                    currentSet.Key = "default_printer";
                    currentSet.Value = value.ToString();
                    entity.Settings.Add(currentSet);
                }
                else
                {
                    currentSet.Value = value.ToString();
                }
                entity.SaveChanges();
            }
        }

        public static int DefaultTopSaleRow
        {
            get
            {
                APP_Data.Setting currentSet =  Utility.generalEntity_Para.FirstOrDefault(x => x.Key == "default_top_sale_row");
                if (currentSet != null)
                {
                    return Convert.ToInt32(currentSet.Value);
                }

                return 0;
            }
            set
            {
                POSEntities entity = new POSEntities();
                APP_Data.Setting currentSet = entity.Settings.FirstOrDefault(x => x.Key == "default_top_sale_row");
                if (currentSet == null)
                {
                    currentSet = new APP_Data.Setting();
                    currentSet.Key = "default_top_sale_row";
                    currentSet.Value = value.ToString();
                    entity.Settings.Add(currentSet);
                }
                else
                {
                    currentSet.Value = value.ToString();
                }
                entity.SaveChanges();
            }
        }
        //public static int DefaultCurrency_Para;
        //public static int DefaultCurrency
        //{
        //    get
        //    {
                
        //        APP_Data.Setting currentSet =  Utility.generalEntity_Para.FirstOrDefault(x => x.Key == "default_currency");
        //        if (currentSet != null)
        //        {
        //            return Convert.ToInt32(currentSet.Value);
        //        }
        //        return 0;
        //    }

        //    set
        //    {
        //        POSEntities entity = new POSEntities();
        //        APP_Data.Setting currentset = entity.Settings.FirstOrDefault(x => x.Key == "default_currency");
        //        if (currentset == null)
        //        {
        //            currentset = new APP_Data.Setting();
        //            currentset.Key = "default_currency";
        //            currentset.Value = value.ToString();
        //            entity.Settings.Add(currentset);
        //        }
        //        else
        //        {
        //            currentset.Value = value.ToString();
        //        }
        //        entity.SaveChanges();
        //    }
        //}
        ////public static int DefaultNoOfCopies_Para;
        public static int DefaultNoOfCopies
        {
            get
            {
                
                APP_Data.Setting currentSet =  Utility.generalEntity_Para.FirstOrDefault(x => x.Key == "default_noOfCopies");
                if (currentSet != null)
                {
                    return Convert.ToInt32(currentSet.Value);
                }

                return 1;
            }
            set
            {
                POSEntities entity = new POSEntities();
                APP_Data.Setting currentSet = entity.Settings.FirstOrDefault(x => x.Key == "default_noOfCopies");
                if (currentSet == null)
                {
                    currentSet = new APP_Data.Setting();
                    currentSet.Key = "default_noOfCopies";
                    currentSet.Value = value.ToString();
                    entity.Settings.Add(currentSet);
                }
                else
                {
                    currentSet.Value = value.ToString();
                }
                entity.SaveChanges();
            }
        }
        //public static string Company_StartDate_Para;
        public static string Company_StartDate
        {
            get
            {
                APP_Data.Setting currentSet =  Utility.generalEntity_Para.FirstOrDefault(x => x.Key == "Company_StartDate");
                if (currentSet != null)
                {
                    return Convert.ToString(currentSet.Value);
                }
                return string.Empty;
            }

            set
            {
                POSEntities entity = new POSEntities();
                APP_Data.Setting currentset = entity.Settings.FirstOrDefault(x => x.Key == "Company_StartDate");
                if (currentset == null)
                {
                    currentset = new APP_Data.Setting();
                    currentset.Key = "Company_StartDate";
                    currentset.Value = value.ToString();
                    entity.Settings.Add(currentset);
                }
                else
                {
                    currentset.Value = value.ToString();
                }
                entity.SaveChanges();
            }
        }

        //public static int DefaultCity_Para;
        public static int DefaultCity
        {
            get
            {
                
                APP_Data.Setting currentSet =  Utility.generalEntity_Para.FirstOrDefault(x => x.Key == "default_city_id");
                if (currentSet != null)
                {
                    return Convert.ToInt32(currentSet.Value);
                }
                return 0;
            }
            set
            {
                POSEntities entity = new POSEntities();
                APP_Data.Setting currentSet = entity.Settings.FirstOrDefault(x => x.Key == "default_city_id");
                if (currentSet == null)
                {
                    currentSet = new APP_Data.Setting();
                    currentSet.Key = "default_city_id";
                    currentSet.Value = value.ToString();
                    entity.Settings.Add(currentSet);
                }
                else
                {
                    currentSet.Value = value.ToString();
                }
                entity.SaveChanges();
            }
        }
        public static int POSID
        {
            get
            {
                
                APP_Data.Setting currentSet =  Utility.generalEntity_Para.FirstOrDefault(x => x.Key == "pos_id");
                if (currentSet!=null)
                {
                    return Convert.ToInt16(currentSet.Value);
                }
                return 0;
            }
            set
            {
                POSEntities entity = new POSEntities();
                APP_Data.Setting currentSet =  entity.Settings.FirstOrDefault(x => x.Key == "pos_id");
                if (currentSet == null)
                {
                    currentSet = new APP_Data.Setting();
                    currentSet.Key = "pos_id";
                    currentSet.Value = value.ToString();
                    entity.Settings.Add(currentSet);
                }
                else
                {
                    currentSet.Value = value.ToString();
                }
                entity.SaveChanges();
            }
        }

        //public static bool IsSourcecode_Para;
        public static bool IsSourcecode
        {
            get
            {
                
                APP_Data.Setting currentSet =  Utility.generalEntity_Para.FirstOrDefault(x => x.Key == "IsSourcecode");
                if (currentSet != null)
                {
                    return Convert.ToBoolean(currentSet.Value);
                }
                return false;
            }
            set
            {
                POSEntities entity = new POSEntities();
                APP_Data.Setting currentSet =  entity.Settings.FirstOrDefault(x => x.Key == "IsSourcecode");
                if (currentSet==null)
                {
                    currentSet = new APP_Data.Setting();
                    currentSet.Key = "IsSourcecode";
                    currentSet.Value = value.ToString();
                    entity.Settings.Add(currentSet);
                }
                else
                {
                    currentSet.Value = value.ToString();
                }
                entity.SaveChanges();
            }

        }


        public static string Use
        {
            get
            {
                
                APP_Data.Setting currentSet =  Utility.generalEntity_Para.FirstOrDefault(x => x.Key == "default_tax_rate");
                if (currentSet != null)
                {
                    return Convert.ToString(currentSet.Value);
                }

                return string.Empty;
            }
            set
            {
                POSEntities entity = new POSEntities();
                APP_Data.Setting currentSet =  entity.Settings.FirstOrDefault(x => x.Key == "default_tax_rate");
                if (currentSet == null)
                {
                    currentSet = new APP_Data.Setting();
                    currentSet.Key = "default_tax_rate";
                    currentSet.Value = value.ToString();
                    entity.Settings.Add(currentSet);
                }
                else
                {
                    currentSet.Value = value.ToString();
                }
                entity.SaveChanges();
            }
        }


        internal static object CompanyStartDate()
        {
            throw new NotImplementedException();
        }
    }

    public static class DatabaseControlSetting
    {
        public static string _ServerName
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["_ServerName"];
            }
        }
        /// <summary>
        /// Get or Set the Database's Name
        /// </summary>
        public static string _DBName
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["_DBName"];
            }
        }
        /// <summary>
        /// Get or Set the Database's Login User
        /// </summary>
        public static string _DBUser
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["_DBUser"];
            }
        }
        /// <summary>
        /// Get or Set the Database's Login Password
        /// </summary>
        public static string _DBPassword
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["_DBPassword"];
            }
        }

    }

    public class dailysaleclass
    {
        public string Payment_Type;
        public decimal Total;
    }

    public class RestoreHelper
    {
        public RestoreHelper()
        {

        }

        public void RestoreDatabase(String databaseName, String backUpFile, String serverName, String userName, String password)
        {

            ServerConnection connection = new ServerConnection(serverName, userName, password);
            Server sqlServer = new Server(connection);
            string dbaddr = string.Empty;
            if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString))
            {
                dbaddr = System.Configuration.ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;
            }
            SqlConnection conn = new SqlConnection(dbaddr);
            string s = conn.State.ToString();
            conn.Close();
            SqlConnection.ClearPool(conn);
            sqlServer.KillAllProcesses(databaseName);
            sqlServer.KillDatabase(databaseName);
            Database db = new Database(sqlServer, databaseName);
            db.Create();
            Restore rstDatabase = new Restore();
            rstDatabase.Action = RestoreActionType.Database;
            rstDatabase.Database = databaseName;
            BackupDeviceItem bkpDevice = new BackupDeviceItem(backUpFile, DeviceType.File);
            rstDatabase.Devices.Add(bkpDevice);
            rstDatabase.ReplaceDatabase = true;
            rstDatabase.Complete += new ServerMessageEventHandler(sqlRestore_Complete);
            rstDatabase.PercentCompleteNotification = 10;
            rstDatabase.PercentComplete += new PercentCompleteEventHandler(sqlRestore_PercentComplete);
            rstDatabase.SqlRestore(sqlServer);
            sqlServer.Refresh();
        }

        public event EventHandler<PercentCompleteEventArgs> PercentComplete;

        void sqlRestore_PercentComplete(object sender, PercentCompleteEventArgs e)
        {
            if (PercentComplete != null)
                PercentComplete(sender, e);
        }

        public event EventHandler<ServerMessageEventArgs> Complete;

        void sqlRestore_Complete(object sender, ServerMessageEventArgs e)
        {
            if (Complete != null)
                Complete(sender, e);
        }
    }

    public class BackupHelper
    {
        public BackupHelper()
        {

        }
        
        public void BackupDatabase(String databaseName, String userName, String password, String serverName, String destinationPath, ref bool isBackUp)
        {
            Backup sqlBackup = new Backup();

            sqlBackup.Action = BackupActionType.Database;
            sqlBackup.BackupSetDescription = "ArchiveDataBase:" + DateTime.Now.ToShortDateString();
            sqlBackup.BackupSetName = "Archive";

            sqlBackup.Database = databaseName;

            BackupDeviceItem deviceItem = new BackupDeviceItem(destinationPath, DeviceType.File);
            ServerConnection connection = new ServerConnection(serverName, userName, password);
            Server sqlServer = new Server(connection);
          
            //Database db = sqlServer.Databases[databaseName];

            sqlBackup.Initialize = true;
            sqlBackup.Checksum = true;
            sqlBackup.ContinueAfterError = true;

            sqlBackup.Devices.Add(deviceItem);
            sqlBackup.Incremental = false;

            sqlBackup.ExpirationDate = DateTime.Now.AddDays(30);
            sqlBackup.LogTruncation = BackupTruncateLogType.Truncate;

            sqlBackup.FormatMedia = false;
            try
            {
                sqlBackup.SqlBackup(sqlServer);
                isBackUp = true;
            }
            catch
            {
                MessageBox.Show("Please check the database if it's properly installed.");
            }
        }
    }

    #region PrintFunctions

    public static class PrintDoc
    {
        private static Boolean isStickerSize = false;
        private static Boolean isSlipSize = false;
        //private static Boolean isA4Size = false;
        private static IList<Stream> m_streams;
        private static int m_currentPageIndex;

        #region Printing Functions

        private static void Print()
        {
            try
            {
                if (m_streams == null || m_streams.Count == 0)
                    return;

                PrintDocument printDoc = new PrintDocument();

                if (isStickerSize)
                    printDoc.PrinterSettings.PrinterName = DefaultPrinter.BarcodePrinter;
                else if (isSlipSize)
                    printDoc.PrinterSettings.PrinterName = DefaultPrinter.SlipPrinter;
                else
                    printDoc.PrinterSettings.PrinterName = DefaultPrinter.A4Printer;

                if (!printDoc.PrinterSettings.IsValid)
                {
                    string msg = String.Format("Printing can't be processed!.Can't find printer \"{0}\".", DefaultPrinter.A4Printer);
                    System.Diagnostics.Debug.WriteLine(msg);
                    MessageBox.Show(msg, "Change Printer Setting", MessageBoxButtons.OK);
                    return;
                }
                printDoc.PrintPage += new PrintPageEventHandler(PrintPage);
                printDoc.Print();

                printDoc.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public static void PrintReport(ReportViewer rv)
        {
            isStickerSize = false;
            m_currentPageIndex = 0;
            m_streams = null;
            Export(rv.LocalReport);
            Print();
            //  Dispose();
            rv.LocalReport.ReleaseSandboxAppDomain();
        }

        /// <summary>
        /// 
        /// </summary>        
        /// <param name="Type">BarcodeStricker||Slip </param>
        public static void PrintReport(ReportViewer rv, String Type)
        {
            m_currentPageIndex = 0;
            m_streams = null;
            isStickerSize = false;  isSlipSize = false;
            if (Type == "BarcodeSTicker")
            {
                isStickerSize = true;
            }
            else if (Type == "A4 Printer")
            {
                //isA4Size = true;
            }
            else
            {
                isSlipSize = true;
            }

            Export(rv.LocalReport);

            Print();

            //  Dispose();
            rv.LocalReport.ReleaseSandboxAppDomain();
        }

        // Export the given report as an EMF (Enhanced Metafile) file.
        private static void Export(LocalReport report)
        {
            string deviceInfo = string.Empty;
            if (isStickerSize)
            {
                deviceInfo =
                  @"<DeviceInfo>
                <OutputFormat>EMF</OutputFormat>
                <PageWidth>3in</PageWidth>                
                <MarginTop>0in</MarginTop>
                <MarginLeft>0in</MarginLeft>
                <MarginRight>0in</MarginRight>
                <MarginBottom>0in</MarginBottom>
                </DeviceInfo>";
            }
            else if (isSlipSize)
            {
                deviceInfo =
                  @"<DeviceInfo>
                <OutputFormat>EMF</OutputFormat>
                <PageWidth>3.1in</PageWidth>                
                <MarginTop>0in</MarginTop>
                <MarginLeft>0in</MarginLeft>
                <MarginRight>0in</MarginRight>
                <MarginBottom>0in</MarginBottom>
            </DeviceInfo>";
            }
            else
            {
                deviceInfo =
                  @"<DeviceInfo>
                <OutputFormat>EMF</OutputFormat>
                <PageWidth>8in</PageWidth>
                <PageHeight>10.5in</PageHeight>
                <MarginTop>0in</MarginTop>
                <MarginLeft>0in</MarginLeft>
                <MarginRight>0in</MarginRight>
                <MarginBottom>0in</MarginBottom>
            </DeviceInfo>";
            }
            Warning[] warnings;
            m_streams = new List<Stream>();

            report.Refresh();
            var ps = report.GetParameters();

            report.Render("Image", deviceInfo, CreateStream, out warnings);

            foreach (Stream stream in m_streams)
                stream.Position = 0;
        }

        private static void PrintPage(object sender, PrintPageEventArgs ev)
        {
            Metafile pageImage = new Metafile(m_streams[m_currentPageIndex]);
            //ev.Graphics.DrawImage(pageImage, 0, 0);
            ev.Graphics.DrawImage(pageImage, ev.PageBounds);
            m_currentPageIndex++;
            ev.HasMorePages = (m_currentPageIndex < m_streams.Count);
        }

        private static Stream CreateStream(string name, string fileNameExtension, Encoding encoding, string mimeType, bool willSeek)
        {
            Stream stream = new MemoryStream();
            m_streams.Add(stream);
            return stream;
        }

        #endregion
    }

    public static class ExportReport
    {
        public static void Excel(ReportViewer rv, String FileName)
        {
            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string extension;

            byte[] bytes = rv.LocalReport.Render(
               "Excel", null, out mimeType, out encoding,
                out extension,
               out streamids, out warnings);
            try
            {
                FileStream fs = new FileStream(@"D:\Reports\" + FileName + DateTime.Now.ToString("ddMMyyyy") + ".xls", FileMode.Create);
                fs.Write(bytes, 0, bytes.Length);
                fs.Close();
                MessageBox.Show(@"Report file is saved in D\Reports\" + FileName + DateTime.Now.ToString("ddMMyyyy") + ".xls", "Saving Complete");
            }
            catch (DirectoryNotFoundException message)
            {
                MessageBox.Show(@"The file patch (D:\Reports) isn't exist. Please check and create Reports folder in the Drive D", "Error");
            }
        }
    }

    public static class DefaultPrinter
    {
        //public static string BarcodePrinter_Para;
        public static string BarcodePrinter
        {
            get
            {
               APP_Data.Setting currentSet =  Utility.generalEntity_Para.FirstOrDefault(x => x.Key == "barcode_printer");
                if (currentSet != null)
                {
                    return Convert.ToString(currentSet.Value);
                }

                return string.Empty;
            }
            set
            {
                POSEntities entity = new POSEntities();
                APP_Data.Setting currentSet = entity.Settings.FirstOrDefault(x => x.Key == "barcode_printer");
                if (currentSet == null)
                {
                    currentSet = new APP_Data.Setting();
                    currentSet.Key = "barcode_printer";
                    currentSet.Value = value.ToString();
                    entity.Settings.Add(currentSet);
                }
                else
                {
                    currentSet.Value = value.ToString();
                }
                entity.SaveChanges();
            }
        }
        //public static string A4Printer_Para;
        public static string A4Printer
        {
            get
            {
                APP_Data.Setting currentSet =  Utility.generalEntity_Para.FirstOrDefault(x => x.Key == "a4_printer");
                if (currentSet != null)
                {
                    return Convert.ToString(currentSet.Value);
                }

                return string.Empty;
            }
            set
            {
                POSEntities entity = new POSEntities();
                APP_Data.Setting currentSet = entity.Settings.FirstOrDefault(x => x.Key == "a4_printer");
                if (currentSet == null)
                {
                    currentSet = new APP_Data.Setting();
                    currentSet.Key = "a4_printer";
                    currentSet.Value = value.ToString();
                    entity.Settings.Add(currentSet);
                }
                else
                {
                    currentSet.Value = value.ToString();
                }
                entity.SaveChanges();
            }
        }
        //public static string SlipPrinter_Para;
        public static string SlipPrinter
        {
            get
            {
                string key = "slip_printer_counter" + MemberShip.CounterId.ToString();
                APP_Data.Setting currentSet =  Utility.generalEntity_Para.FirstOrDefault(x => x.Key == key);
                if (currentSet != null)
                {
                    return Convert.ToString(currentSet.Value);
                }

                return string.Empty;
            }
            set
            {
                POSEntities entity = new POSEntities();
                string key = "slip_printer_counter" + MemberShip.CounterId.ToString();
                APP_Data.Setting currentSet = entity.Settings.FirstOrDefault(x => x.Key == key);
                if (currentSet == null)
                {
                    currentSet = new APP_Data.Setting();
                    currentSet.Key = key;
                    currentSet.Value = value.ToString();
                    entity.Settings.Add(currentSet);
                }
                else
                {
                    currentSet.Value = value.ToString();
                }
                entity.SaveChanges();
            }
        }
    }

    #endregion


}
