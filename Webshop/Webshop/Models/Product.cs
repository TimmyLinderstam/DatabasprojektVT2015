using System.Web;

namespace Webshop.Models
{
    public class Product
    {

        public int Id { get; set; } // Primary Key
        public int Units { get; set; }
        public int Price { get; set; }
        public int AcquisitionPrice { get; set; }
        public int Series { get; set; } // Foreign Key --> Serie(Id)
        public int Category { get; set; } // Foreign Key --> Category(Id)

        public string Energyclass { get; set; }
        public string Description { get; set; }
        public string Manufactor { get; set; }
        public string Brand { get; set; }

    }
}