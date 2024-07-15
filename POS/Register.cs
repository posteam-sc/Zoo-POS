using POS.APP_Data;
using System;
using System.Windows.Forms;

namespace POS
{
    public partial class Register : Form
    {
        private POSEntities entity = new POSEntities();

        public Register()
        {
            InitializeComponent();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            String Key = txtLicenseKey.Text.Trim();
            Authorize currentKey = new Authorize();
            foreach (Authorize aut in entity.Authorizes)
            {
                if (Utility.EncryptString(aut.licenseKey, "ABCD") == Key)
                {
                    currentKey = aut;
                    break;
                }
            }

            if (currentKey.Id != 0)
            {
                if (currentKey.macAddress == null)
                {
                    try
                    {
                        currentKey.macAddress = Utility.GetSystemMotherBoardId();// Utility.EncryptString(Utility.GetSystemMotherBoardId(), "ABCD");
                        currentKey.CreatedDate = DateTime.Now;
                        entity.SaveChanges();
                        MessageBox.Show("Registration complete", "Complete");


                        Login newform = new Login();
                        newform.WindowState = FormWindowState.Maximized;
                        newform.MdiParent = ((MDIParent)this.ParentForm);
                        newform.Show();
                        this.Dispose();
                    }
                    catch (Exception exe)
                    {
                        MessageBox.Show(exe.Message, "Error");
                    }
                }
                else
                {
                    MessageBox.Show("The Key is already in use");
                }
            }
            else
            {
                MessageBox.Show("Wrong License Key", "Error");
            }
        }

        private void txtLicenseKey_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.AcceptButton = btnRegister;
        }

        private void Register_Load(object sender, EventArgs e)
        {
            Localization.Localize_FormControls(this);
        }
    }
}
