using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Webshop.Models
{
    public class Customer
    {
        public int Id { get; set; } // Primary Key

        [MaxLength(10)]
        public string PersonId { get; set; }

        [Phone]
        public string TelephoneNumber { get; set; }

        [EmailAddress]
        public string EmailAdress { get; set; }

        public string HomeAdress { get; set; }
    }
}