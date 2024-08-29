using Accounting.ViewModels.Customers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.DataLayer {
    public class CustomerRepository : ICustomerRepository {

        private Accounting_DBEntities db;

        public CustomerRepository(Accounting_DBEntities context) {
            db = context;
        }

        public bool DeleteCustomer(Customers customer) {
            try {
                db.Entry(customer).State = EntityState.Deleted;
                return true;
            }
            catch {
                return false;
            }
        }

        public bool DeleteCustomer(int customerId) {
            try {
                var customer = GetCustomerById(customerId);
                DeleteCustomer(customer);
                return true;
            }
            catch {
                return false;
            }
        }

        public List<Customers> GetAllCustomers() {
            return db.Customers.ToList();
        }

        public IEnumerable<Customers> GetCustomerByFilter(string parameter) {
            return db.Customers.Where(c => c.FullName.Contains(parameter) || c.Email.Contains(parameter) || c.Mobile.Contains(parameter)).ToList();
        }

        public Customers GetCustomerById(int customerId) {
            return db.Customers.Find(customerId);
        }

        public int GetCustomerIdByName(string name) {
            return db.Customers.First(c => c.FullName == name).CustomerId;
        }

        public List<ListCustomerViewModel> GetNameCustomers(string filter = "") {
            if( filter == "" ) {
                return db
                    .Customers
                    .Select(c => new ListCustomerViewModel() { FullName = c.FullName })
                    .ToList();
            }

            return db
                .Customers
                .Where(c => c.FullName.Contains(filter))
                .Select(c => new ListCustomerViewModel() { FullName = c.FullName })
                .ToList();
        }

        public bool InsertCustomer(Customers customer) {
            try {
                db.Customers.Add(customer);
                return true;
            }
            catch {
                return false;
            }
        }

        public bool UpdateCustomer(Customers customer) {
            try {
                var local = db.Set<Customers>().Local.FirstOrDefault(f => f.CustomerId == customer.CustomerId);
                if( local != null ) {
                    db.Entry(local).State = EntityState.Detached;
                }
                db.Entry(customer).State = EntityState.Modified;
                return true;
            }
            catch {
                return false;
            }
        }


    }
}
