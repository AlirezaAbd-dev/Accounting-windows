using Accounting.ViewModels.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.DataLayer {
    public interface ICustomerRepository{
        IEnumerable<Customers> GetCustomerByFilter(string parameter);
        List<ListCustomerViewModel> GetNameCustomers(string filter= "");
        List<Customers> GetAllCustomers();
        Customers GetCustomerById(int id);
        bool InsertCustomer(Customers customer);
        bool UpdateCustomer(Customers customer);
        bool DeleteCustomer(Customers customer);
        bool DeleteCustomer(int id);
        int GetCustomerIdByName(string name);
        string GetCustomerNameById(int id);
    }
}
