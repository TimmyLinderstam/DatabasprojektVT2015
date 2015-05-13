using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Webshop.Models
{
    public class Promotion
    {
        public int Id { get; set; } // Primary Key

        public int Value { get; set; } 
        public int Category { get; set; } // Foreign Key --> Category(Id)

        public string Description { get; set; }
    }
}