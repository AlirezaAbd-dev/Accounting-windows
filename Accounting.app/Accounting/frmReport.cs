using Accounting.DataLayer;
using Accounting.DataLayer.Context;
using Accounting.Utility.Convertor;
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
                var result = db.AccountingRepository.Get(a => a.TypeID == TypeId);

                foreach( var accounting in result ) {
                    string customerName = db.CustomerRepository.GetCustomerNameById(accounting.CustomerId);
                    dgvReport.Rows.Add(accounting.ID, customerName, accounting.Amount, accounting.DateTime.ToShamsi(),accounting.Description);
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
    }
}
