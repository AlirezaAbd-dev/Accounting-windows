using Accounting.DataLayer;
using Accounting.DataLayer.Context;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Accounting.app {
    public partial class frmCustomers : Form {
        public frmCustomers() {
            InitializeComponent();
        }

        private void frmCustomers_Load(object sender, EventArgs e) {
            BindGrid();
        }

        void BindGrid() {
            using( UnitOfWork db = new UnitOfWork() ) {
                dgvCustomers.AutoGenerateColumns = false;
                var customers = db.CustomerRepository.GetAllCustomers();
                dgvCustomers.DataSource = customers;
            }
        }

        private void btnRefreshCustomer_Click(object sender, EventArgs e) {
            txtFilter.Text = null;
            BindGrid();
        }

        private void txtFilter_TextChanged(object sender, EventArgs e) {
            using( UnitOfWork db = new UnitOfWork() ) {
                var customers = db.CustomerRepository.GetCustomerByFilter(txtFilter.Text);
                dgvCustomers.DataSource = customers;
            }
        }

        private void btnDeleteCustomer_Click(object sender, EventArgs e) {
            int selectedId = int.Parse(dgvCustomers.CurrentRow.Cells["CustomerId"].Value.ToString());
            string selectedName = dgvCustomers.CurrentRow.Cells["FullName"].Value.ToString();

            if(
                RtlMessageBox.Show(
                $"آیا از حذف '{selectedName}' اطمینان دارید؟",
                "توجه", MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning) == DialogResult.Yes
                ) {
                using( UnitOfWork db = new UnitOfWork() ) {
                    db.CustomerRepository.DeleteCustomer(selectedId);
                    db.Save();
                    BindGrid();
                }
            }
        }

        private void btnAddNewCustomer_Click(object sender, EventArgs e) {
            frmAddOrEditCustomer frmAdd = new frmAddOrEditCustomer();

            if(frmAdd.ShowDialog() == DialogResult.OK) {
                BindGrid();
            }
        }

        private void btnEditCustomer_Click(object sender, EventArgs e) {
            DataGridViewCellCollection data = dgvCustomers.CurrentRow.Cells;
            Customers customer = new Customers() {
                Address = data["Address"].Value.ToString(),
                CustomerId = int.Parse(data["CustomerId"].Value.ToString()),
                Email = data["Email"].Value.ToString(),
                CustomerImage = data["CustomerImage"].Value.ToString(),
                FullName= data["FullName"].Value.ToString(),
                Mobile= data["Mobile"].Value.ToString()
            };
            frmAddOrEditCustomer frmEdit = new frmAddOrEditCustomer(customer);

            if( frmEdit.ShowDialog() == DialogResult.OK ) {
                BindGrid();
            }
        }
    }
}
