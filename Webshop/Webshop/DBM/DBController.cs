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

        public void SaveProduct(Product p)
        {
            var cmd = CreateCmd();

            if (p.Id == 0)
            {
                cmd.CommandText = "INSERT INTO Product " +
                    "VALUES(NULL, @Name, @Units, @Energyclass, @Model, @AcquisitionPrice, @Description, @Price, @Manufactor, @Category, @Series)";

            }
            else
            {
                cmd.CommandText = String.Format("UPDATE Product " +
                   "SET Name=@Name, Units=@Units, Energyclass=@Energyclass, Model=@Model, AcquisitionPrice=@AcquisitionPrice, Description=@Description, Price=@Price, Manufactor=@Manufactor, Category=@Category, Series=@Series WHERE Id={0}", p.Id);
            }

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

            // Foreign keys
            cmd.Parameters.AddWithValue("@Category", p.Category);

            if (p.Series == 0)
                cmd.Parameters.AddWithValue("@Series", null);
            else
                cmd.Parameters.AddWithValue("@Series", p.Series);

            cmd.ExecuteNonQuery();
        }

        public void RemoveProduct(int id)
        {
            var cmd = CreateCmd();
            cmd.CommandText = String.Format("DELETE from Product WHERE Id={0}", id);
            cmd.ExecuteNonQuery();
        }

        public Product GetProduct(int id)
        {
            var sql = String.Format("SELECT * FROM Product WHERE Id={0}", id);

            var product = Read<Product>(sql);
            if (product != null)
            {
                product.CategoryObj = Read<Category>(String.Format("SELECT * FROM Category WHERE Id={0}", product.Category));
                product.SeriesObj = Read<Serie>(String.Format("SELECT * FROM Serie WHERE Id={0}", product.Series));
            }
            return product;
        }

        public List<Product> GetProducts()
        {
            var cmd = CreateCmd();
            cmd.CommandText = "SELECT * FROM Product";
            return ReadList<Product>(cmd);
        }

        public List<Product> GetProductSuggestions(Product product)
        {
            var cmd = CreateCmd();
            cmd.CommandText = String.Format("SELECT products.* FROM Product as products inner join Serie on Serie.Id=products.Series WHERE Serie.Id={0} AND products.Id !={1} LIMIT 5", product.Series, product.Id);
            return ReadList<Product>(cmd);
        }

        // CAT

        public List<Category> GetCategories()
        {
            var cmd = CreateCmd();
            cmd.CommandText = "SELECT * FROM Category";
            return ReadList<Category>(cmd);
        }

        public void SaveCategory(Category category)
        {
            var cmd = CreateCmd();

            if (category.Id == 0)
            {
                cmd.CommandText = "INSERT INTO Category " +
                    "VALUES(NULL, @CategoryName)";

            }
            else
            {
                cmd.CommandText = String.Format("UPDATE Category " +
                   "SET CategoryName=@CategoryName WHERE Id={0}", category.Id);
            }

            cmd.Prepare();

            // Add values
            cmd.Parameters.AddWithValue("@CategoryName", category.CategoryName);

            cmd.ExecuteNonQuery();
        }

        public void RemoveCategory(int id)
        {
            var cmd = CreateCmd();
            cmd.CommandText = String.Format("DELETE from Category WHERE Id={0}", id);
            cmd.ExecuteNonQuery();
        }

        // SERIE

        public List<Serie> GetSeries()
        {
            var cmd = CreateCmd();
            cmd.CommandText = "SELECT * FROM Serie";
            return ReadList<Serie>(cmd);
        }

        public void SaveSerie(Serie serie)
        {
            var cmd = CreateCmd();

            if (serie.Id == 0)
            {
                cmd.CommandText = "INSERT INTO Serie " +
                    "VALUES(NULL, @SerieName)";

            }
            else
            {
                cmd.CommandText = String.Format("UPDATE Serie " +
                   "SET SerieName=@SerieName WHERE Id={0}", serie.Id);
            }

            cmd.Prepare();

            // Add values
            cmd.Parameters.AddWithValue("@SerieName", serie.SerieName);

            cmd.ExecuteNonQuery();
        }

        public void RemoveSerie(int id)
        {
            var cmd = CreateCmd();
            cmd.CommandText = String.Format("DELETE from Serie WHERE Id={0}", id);
            cmd.ExecuteNonQuery();
        }
    }
}
