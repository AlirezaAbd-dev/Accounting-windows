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
    public partial class frmAddOrEditCustomer : Form {
        UnitOfWork db = new UnitOfWork();
        Customers customer;
        public frmAddOrEditCustomer() {
            InitializeComponent();
        }

        public frmAddOrEditCustomer(Customers customer) {
            InitializeComponent();
            this.customer = customer;

            this.Text = "ویرایش شخص";
            btnSave.Text = "ویرایش";
            txtAddress.Text = customer.Address;
            txtEmail.Text = customer.Email;
            txtMobile.Text = customer.Mobile;
            txtName.Text = customer.FullName;
            pcCustomer.ImageLocation =Application.StartupPath + "/Images/"+ customer.CustomerImage;
        }

        private void frmAddOrEditCustomer_Load(object sender, EventArgs e) {

        }

        private void btnSelectImage_Click(object sender, EventArgs e) {
            OpenFileDialog openFile = new OpenFileDialog();

            if( openFile.ShowDialog() == DialogResult.OK ) {
                pcCustomer.ImageLocation = openFile.FileName;
            }
        }

        private void btnSave_Click(object sender, EventArgs e) {
            if( BaseValidator.IsFormValid(this.components) ) {
                string imageName = Guid.NewGuid().ToString() + Path.GetExtension(pcCustomer.ImageLocation);
                string path = Application.StartupPath + "/Images/";
                if( !Directory.Exists(path) ) {
                    Directory.CreateDirectory(path);
                }
                pcCustomer.Image.Save(path + imageName);

                Customers customer = new Customers() { 
                    Address = txtAddress.Text,
                    Email = txtEmail.Text,
                    FullName = txtName.Text,
                    Mobile = txtMobile.Text,
                    CustomerImage = imageName
                };

                if( this.customer == null ) {
                    db.CustomerRepository.InsertCustomer(customer);
                }
                else {
                    customer.CustomerId = this.customer.CustomerId;
                    db.CustomerRepository.UpdateCustomer(customer);
                }
                db.Save();

                DialogResult = DialogResult.OK;
            }
        }
    }
}
