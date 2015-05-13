using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Webshop.Models
{
    public class Purchase
    {
        public int Id { get; set; } // Primary Key
        public int Customer { get; set; } // Foreign Key --> Customer(Id)
        public int Product { get; set; } // Foreign Key --> Product(Id)

        

        

    }
}