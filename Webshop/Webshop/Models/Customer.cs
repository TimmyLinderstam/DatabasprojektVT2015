using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Webshop.Models
{
    public class Customer
    {
        public int Id { get; set; } // Primary Key
        public int PersonId { get; set; }
        public int TelephoneNumber { get; set; }

        public string EmailAdress { get; set; }
        public string HomeAdress { get; set; }
    }
}