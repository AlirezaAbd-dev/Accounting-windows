using Accounting.DataLayer;
using Accounting.DataLayer.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1 {
    internal class Program {
        static void Main(string[] args) {
            UnitOfWork db = new UnitOfWork();

            var list = db.CustomerRepository.GetAllCustomers();

            db.Dispose();
        }
    }
}
