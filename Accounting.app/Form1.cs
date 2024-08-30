using Accounting.Business;
using Accounting.Utility.Convertor;
using Accounting.ViewModels.Accounting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace Accounting.app {
    public partial class Form1 : Form {
        public Form1() {
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

        private void btnReportPay_Click(object sender, EventArgs e) {
            frmReport frmReport = new frmReport();
            frmReport.TypeId = 2;
            frmReport.ShowDialog();
        }

        private void btnReportRecieve_Click(object sender, EventArgs e) {
            frmReport frmReport = new frmReport();
            frmReport.TypeId = 1;
            frmReport.ShowDialog();
        }

        private void Form1_Load(object sender, EventArgs e) {
            this.Hide();
            frmLogin frmLogin = new frmLogin();
            if( frmLogin.ShowDialog() == DialogResult.OK ) {
                this.Show();
                lblDate.Text = DateTime.Now.ToShamsi();
                lblTime.Text = DateTime.Now.ToString("HH:mm:ss");
                Report();
            }
            else {
                Application.Exit();
            }
        }

        void Report() {
            ReportViewModel report = Account.ReportFormMain();
            lblPay.Text = report.Pay.ToString("#,0");
            lblRecieve.Text = report.Recieve.ToString("#,0");
            lblBalance.Text = report.AccountBalance.ToString("#,0");
        }

        private void timer1_Tick(object sender, EventArgs e) {
            lblTime.Text = DateTime.Now.ToString("hh:mm:ss");
        }

        private void btnEditLogin_Click(object sender, EventArgs e) {
            frmLogin frmLogin = new frmLogin();
            frmLogin.isEdit = true;
            frmLogin.ShowDialog();
        }
    }
}
