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
using ValidationComponents;

namespace Accounting.app {
    public partial class frmLogin : Form {
        public bool isEdit = false;

        public frmLogin() {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e) {
            if( BaseValidator.IsFormValid(this.components) ) {
                using( UnitOfWork db = new UnitOfWork() ) {
                    if( isEdit ) {
                        var login = db.LoginRepository.Get().First();
                        login.UserName = txtUsername.Text;
                        login.Password = txtPassword.Text;
                        db.LoginRepository.Update( login );
                        db.Save();
                        Application.Restart();
                    }
                    else {
                        if( db.LoginRepository.Get(l => l.UserName == txtUsername.Text && l.Password == txtPassword.Text).Any() ) {
                            DialogResult = DialogResult.OK;
                        }
                        else {
                            RtlMessageBox.Show("اطلاعات وارد شده صحیح نمیباشد");
                        }
                    }
                }
            }
        }

        private void frmLogin_Load(object sender, EventArgs e) {
            if( isEdit ) {
                this.Text = "تنظیمات ورود به برنامه";
                btnLogin.Text = "ذخیره تغییرات";
                using( UnitOfWork db = new UnitOfWork() ) {
                    var login = db.LoginRepository.Get().First();
                    txtUsername.Text = login.UserName;
                    txtPassword.Text = login.Password;
                }
            }
        }
    }
}
