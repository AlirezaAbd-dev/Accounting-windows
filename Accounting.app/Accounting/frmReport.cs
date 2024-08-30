using Accounting.DataLayer;
using Accounting.DataLayer.Context;
using Accounting.Utility.Convertor;
using Accounting.ViewModels.Customers;
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
    public partial class frmReport : Form {
        public int TypeId = 0;

        public frmReport() {
            InitializeComponent();
            dgvReport.AutoGenerateColumns = false;
        }

        private void frmReport_Load(object sender, EventArgs e) {
            using( UnitOfWork db = new UnitOfWork() ) {
                List<ListCustomerViewModel> list = new List<ListCustomerViewModel>();
                list.Add(
                    new ListCustomerViewModel() {
                        CustomerId = 0,
                        FullName = "انتخاب کنید"
                    }
                );
                list.AddRange(db.CustomerRepository.GetNameCustomers());
                cbCustomer.DataSource = list;
                cbCustomer.DisplayMember = "FullName";
                cbCustomer.ValueMember = "CustomerId";
            }
            if( TypeId == 1 ) {
                this.Text = "گزارش دریافتی ها";
            }
            else {
                this.Text = "گزارش پرداختی ها";
            }
        }

        private void btnFilter_Click(object sender, EventArgs e) {
            Filter();
        }

        void Filter() {
            using( UnitOfWork db = new UnitOfWork() ) {
                dgvReport.Rows.Clear();
                List<DataLayer.Accounting> result = new List<DataLayer.Accounting>();

                DateTime? startDate;
                DateTime? endDate;

                if( (int)cbCustomer.SelectedValue != 0 ) {
                    int customerId = int.Parse(cbCustomer.SelectedValue.ToString());
                    result.AddRange(db.AccountingRepository.Get(a => a.TypeID == TypeId && a.CustomerId == customerId));
                }
                else {
                    result.AddRange(db.AccountingRepository.Get(a => a.TypeID == TypeId));
                }

                if( txtFromDate.Text != "    /  /" ) {
                    startDate = Convert.ToDateTime(txtFromDate.Text);
                    startDate = DateConvertor.ToMiladi(startDate.Value);
                    result = result.Where(r => r.DateTime >= startDate.Value).ToList();
                }

                if( txtToDate.Text != "    /  /" ) {
                    endDate = Convert.ToDateTime(txtToDate.Text);
                    endDate = DateConvertor.ToMiladi(endDate.Value);
                    result = result.Where(r => r.DateTime <= endDate.Value).ToList();

                }

                foreach( var accounting in result ) {
                    string customerName = db.CustomerRepository.GetCustomerNameById(accounting.CustomerId);
                    dgvReport.Rows.Add(accounting.ID, customerName, accounting.Amount, accounting.DateTime.ToShamsi(), accounting.Description);
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e) {
            if( dgvReport.CurrentRow != null ) {
                int id = int.Parse(dgvReport.CurrentRow.Cells[0].Value.ToString());
                if( RtlMessageBox.Show("آیا از حذف مطمئن هستید؟", "هشدار", MessageBoxButtons.YesNo) == DialogResult.Yes ) {
                    using( UnitOfWork db = new UnitOfWork() ) {
                        db.AccountingRepository.Delete(id);
                        db.Save();
                        Filter();
                    }
                }
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e) {
            Filter();
        }

        private void btnEdit_Click(object sender, EventArgs e) {
            if( dgvReport.CurrentRow != null ) {
                int id = int.Parse(dgvReport.CurrentRow.Cells[0].Value.ToString());
                frmNewAccounting frmNew = new frmNewAccounting();
                frmNew.AccountId = id;
                if( frmNew.ShowDialog() == DialogResult.OK ) {
                    Filter();
                }
            }
        }

        private void btnPrint_Click(object sender, EventArgs e) {
            DataTable dtPrint = new DataTable();
            dtPrint.Columns.Add("Customer");
            dtPrint.Columns.Add("Amount");
            dtPrint.Columns.Add("Date");
            dtPrint.Columns.Add("Description");
            foreach( DataGridViewRow item in dgvReport.Rows ) {
                dtPrint.Rows.Add(
                     item.Cells[0].Value.ToString(),
                     item.Cells[1].Value.ToString(),
                     item.Cells[2].Value.ToString(),
                     item.Cells[3].Value.ToString()
                );
            }
            stiPrint.Load(Application.StartupPath + "/Report.mrt");
            stiPrint.RegData("DT",dtPrint);
            stiPrint.Show();
        }
    }
}
