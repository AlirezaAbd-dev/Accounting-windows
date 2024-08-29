using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Accounting.app
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnCustomers_Click(object sender, EventArgs e) {
            frmCustomers frmCustomers = new frmCustomers();

            frmCustomers.ShowDialog();
        }

        private void btnNewAccounting_Click(object sender, EventArgs e) {
            frmNewAccounting frmNewAccounting = new frmNewAccounting(); 
            frmNewAccounting.ShowDialog();
        }
    }
}
