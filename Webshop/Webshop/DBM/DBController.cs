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
        private static DBController _instance;
        public static DBController Instance { get { if (_instance == null) _instance = new DBController(); return _instance; } private set { _instance = value; } }

        public void AddProduct(Product p)
        {
            var cmd = CreateCmd();

            cmd.CommandText = "INSERT INTO Product(Name, Units, Price,AcquisitionPrice, Series, Category, Energyclass, Description, Manufactor, Brand, Model)" +
                "VALUES(NULL, @Name, @Units, @Energyclass, @Model, @AcquisitionPrice, @Description, @Price, @Manufactor)";

            cmd.Prepare();

            // Add values
            cmd.Parameters.AddWithValue("@Name", p.Name);
            cmd.Parameters.AddWithValue("@Units", p.Units);
            cmd.Parameters.AddWithValue("@Energyclass", p.Energyclass);
            cmd.Parameters.AddWithValue("@Model", p.Model);
            cmd.Parameters.AddWithValue("@AcquisitionPrice", p.AcquisitionPrice);
            cmd.Parameters.AddWithValue("@Description", p.Description);
            cmd.Parameters.AddWithValue("@Price", p.Price);
            cmd.Parameters.AddWithValue("@Manufactor", p.Manufactor);

            cmd.ExecuteNonQuery();
        }

        public void RemoveProduct(int id)
        {
            var cmd = CreateCmd();
            cmd.CommandText = String.Format("DELETE from Product WHERE Id={0}", id);
            cmd.ExecuteNonQuery();
        }
    }
}
