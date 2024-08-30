using Accounting.DataLayer;
using Accounting.DataLayer.Context;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ValidationComponents;

namespace Accounting.app {
    public partial class frmNewAccounting : Form {
        UnitOfWork db;
        public int AccountId = 0;

        public frmNewAccounting() {
            InitializeComponent();
        }

        private void frmNewAccounting_Load(object sender, EventArgs e) {
            db = new UnitOfWork();
            dgvCustomers.AutoGenerateColumns = false;
            dgvCustomers.DataSource = db.CustomerRepository.GetNameCustomers();
            if( AccountId != 0 ) {
                var account = db.AccountingRepository.GetById(AccountId);
                txtAmount.Value = account.Amount;
                txtDescription.Text = account.Description;
                txtName.Text = db.CustomerRepository.GetCustomerNameById(account.CustomerId);
                if( account.TypeID == 1 ) {
                    rbRecieve.Checked = true;
                }
                else {
                    rbPay.Checked = true;
                }
                this.Text = "ویرایش";
                btnSave.Text = "ویرایش";
            }
            db.Dispose();
        }

        private void txtFilter_TextChanged(object sender, EventArgs e) {
            dgvCustomers.DataSource = db.CustomerRepository.GetNameCustomers(txtFilter.Text);
        }

        private void dgvCustomers_CellClick(object sender, DataGridViewCellEventArgs e) {
            txtName.Text = dgvCustomers.CurrentRow.Cells["FullName"].Value.ToString();
        }

        private void btnSave_Click(object sender, EventArgs e) {
            if( BaseValidator.IsFormValid(this.components) ) {
                if( rbPay.Checked || rbRecieve.Checked ) {
                    db = new UnitOfWork();
                    DataLayer.Accounting accounting = new DataLayer.Accounting() {
                        Amount = int.Parse(txtAmount.Value.ToString()),
                        CustomerId = db.CustomerRepository.GetCustomerIdByName(txtName.Text),
                        TypeID = ( rbRecieve.Checked ? 1 : 2 ),
                        DateTime = DateTime.Now,
                        Description = txtDescription.Text,
                    };

                    if( AccountId == 0 ) {
                        db.AccountingRepository.Insert(accounting);
                    }
                    else {
                        accounting.ID = AccountId;
                        db.AccountingRepository.Update(accounting);
                    }

                    db.Save();
                    db.Dispose();
                    DialogResult = DialogResult.OK;
                }
                else {
                    RtlMessageBox.Show("لطفا نوع تراکنش را انتخاب کنید");
                }
            }
        }
    }
}
