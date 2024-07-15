﻿using POS.APP_Data;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace POS
{
    public partial class DraftDetail : Form
    {

        #region Variables

        public string DraftId { get; set; }

        private POSEntities entity = new POSEntities();

        #endregion

        public DraftDetail()
        {
            InitializeComponent();
        }

        private void DraftDetail_Load(object sender, EventArgs e)
        {
            Localization.Localize_FormControls(this);
            Transaction currentDraft = (from c in entity.Transactions where c.Id == DraftId select c).FirstOrDefault<Transaction>();

            if (currentDraft != null)
            {
                dgvSalesItem.AutoGenerateColumns = false;
                dgvSalesItem.DataSource = currentDraft.TransactionDetails.ToList();

                lblDate.Text = currentDraft.DateTime.Value.ToString(SettingController.GlobalDateFormat);
                lblTime.Text = currentDraft.DateTime.Value.ToString("hh:mm");
                lblSalesPersonName.Text = currentDraft.User.Name;
            }
            else
            {
                MessageBox.Show("This draft isn't exist any more");
                this.Dispose();
            }
        }

        private void dgvSalesItem_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow row in dgvSalesItem.Rows)
            {
                TransactionDetail currentt = (TransactionDetail)row.DataBoundItem;
                row.Cells[0].Value = currentt.Product.ProductCode;
                row.Cells[2].Value = currentt.Product.Name;                
            }
        }
    }
}
