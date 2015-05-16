using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Models;

namespace Webshop.DBM
{
    public class DBController : DBManager
    {
        private static DBManager db = Instance;

        public void AddProduct(Product p)
        {
            var cmd = CreateCmd();
        }
    }
}
