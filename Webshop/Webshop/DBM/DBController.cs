using MySql.Data.MySqlClient;
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

            SafeClose();
        }

        public void RemoveProduct(int id)
        {
            var cmd = CreateCmd();
            cmd.CommandText = String.Format("DELETE from Product WHERE Id={0}", id);
            ExecuteQueryAndClose(cmd);
        }

        public Product GetProduct(int id)
        {
            var cmd = CreateCmd();
            cmd.CommandText = String.Format("SELECT * FROM Product WHERE Id={0}", id);

            var product = Read<Product>(cmd);
            if (product != null)
            {
                cmd.CommandText = String.Format("SELECT * FROM Category WHERE Id={0}", product.Category);
                product.CategoryObj = Read<Category>(cmd);

                cmd.CommandText = String.Format("SELECT * FROM Serie WHERE Id={0}", product.Series);
                product.SeriesObj = Read<Serie>(cmd);
            }

            SafeClose();
            return product;
        }

        public List<Product> GetProducts()
        {
            var cmd = CreateCmd();
            cmd.CommandText = "SELECT * FROM Product";
            var list = ReadList<Product>(cmd);
            SafeClose();
            return list ;
        }

        public List<Product> GetProductSuggestions(Product product)
        {
            var cmd = CreateCmd();
            cmd.CommandText = String.Format("SELECT products.* FROM Product as products inner join Serie on Serie.Id=products.Series WHERE Serie.Id={0} AND products.Id !={1} LIMIT 5", product.Series, product.Id);
            var list = ReadList<Product>(cmd);
            SafeClose();
            return list;
        }

        // CAT

        public List<Category> GetCategories()
        {
            var cmd = CreateCmd();
            cmd.CommandText = "SELECT * FROM Category";

            var list = ReadList<Category>(cmd);
            SafeClose();
            return list;
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

            ExecuteQueryAndClose(cmd);
        }

        public void RemoveCategory(int id)
        {
            var cmd = CreateCmd();
            cmd.CommandText = String.Format("DELETE from Category WHERE Id={0}", id);
            ExecuteQueryAndClose(cmd);
        }

        // SERIE

        public List<Serie> GetSeries()
        {
            var cmd = CreateCmd();
            cmd.CommandText = "SELECT * FROM Serie";
            var list = ReadList<Serie>(cmd);
            SafeClose();
            return list;
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

            ExecuteQueryAndClose(cmd);
        }

        public void RemoveSerie(int id)
        {
            var cmd = CreateCmd();
            cmd.CommandText = String.Format("DELETE from Serie WHERE Id={0}", id);
            ExecuteQueryAndClose(cmd);
        }

        public List<Basket> GetBasketByPersonId(string personId)
        {
            var cmd = CreateCmd();
            cmd.CommandText = String.Format("SELECT baskets.* FROM Customer inner join Basket as baskets on baskets.Customer=Customer.Id WHERE Customer.PersonId={0}", personId);
            var baskets = ReadList<Basket>(cmd);

            foreach (var item in baskets)
            {
                cmd.CommandText = String.Format("SELECT * FROM Customer WHERE Id={0}", item.Customer);
                item.CustomerObj = Read<Customer>(cmd);

                cmd.CommandText = String.Format("SELECT * FROM Product WHERE Id={0}", item.Product);
                item.ProductObj = Read<Product>(cmd);
            }

            SafeClose();
            return baskets;
        }

        public void SaveCustomer(Customer customer)
        {
            var cmd = CreateCmd();

            if (customer.Id == 0)
            {
                cmd.CommandText = "INSERT INTO Customer " +
                    "VALUES(NULL, @PersonId, @EmailAdress, @HomeAdress, @TelephoneNumber)";

            }
            else
            {
                cmd.CommandText = String.Format("UPDATE Customer " +
                   "SET PersonId=@PersonId, EmailAdress=@EmailAdress, HomeAdress=@HomeAdress, TelephoneNumber=@TelephoneNumber WHERE Id={0}", customer.Id);
            }

            cmd.Prepare();

            // Add values
            cmd.Parameters.AddWithValue("@PersonId", customer.PersonId);
            cmd.Parameters.AddWithValue("@EmailAdress", customer.EmailAdress);
            cmd.Parameters.AddWithValue("@HomeAdress", customer.HomeAdress);
            cmd.Parameters.AddWithValue("@TelephoneNumber", customer.TelephoneNumber);

            ExecuteQueryAndClose(cmd);
        }
        
        public Customer GetCustomer(string id)
        {
            var cmd = CreateCmd();
            cmd.CommandText = String.Format("SELECT * FROM Customer WHERE Id={0}", id);

            var customer = Read<Customer>(cmd);
            SafeClose();
            return customer;
        }

        public Customer GetCustomerByPersonId(string id)
        {
            var cmd = CreateCmd();
            cmd.CommandText = String.Format("SELECT * FROM Customer WHERE PersonId={0}", id);

            var customer = Read<Customer>(cmd);
            SafeClose();
            return customer;
        }

        public bool Buy(Customer c)
        {
            var baskets = GetBasketByPersonId(c.PersonId);

            var cmd = CreateCmdTransaction();
            try
            {
                foreach (var basket in baskets)
                {
                    if (basket.ProductObj.Units - basket.Quantity < 0)
                        throw new Exception("To many!");

                    // lower units count
                    cmd.CommandText = String.Format("UPDATE Product SET Units=Units-{0} WHERE Id={1}", basket.Quantity, basket.Product);
                    cmd.ExecuteNonQuery();

                    // add to purchase
                    cmd.CommandText = "INSERT INTO Purchase " +
                        "VALUES(NULL, @Customer, @Product, @Quantity)";

                    cmd.Prepare();

                    // Add values
                    cmd.Parameters.AddWithValue("@Customer", basket.Customer);
                    cmd.Parameters.AddWithValue("@Product", basket.Product);
                    cmd.Parameters.AddWithValue("@Quantity", basket.Quantity);

                    cmd.ExecuteNonQuery();


                    // Remove from basket
                    cmd.CommandText = String.Format("DELETE FROM Basket WHERE Id={0}", basket.Id);
                    cmd.ExecuteNonQuery();
                }

                cmd.Transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                try
                {
                    cmd.Transaction.Rollback();
                }
                catch (MySqlException mse)
                {
                }
            }
            finally
            {
                SafeClose();
            }
            return false;
        }

        public List<Purchase> GetPurchases()
        {
            var cmd = CreateCmd();
            cmd.CommandText = "SELECT * FROM Purchase";

            var list = ReadList<Purchase>(cmd);
            foreach (var item in list)
            {
                cmd.CommandText = String.Format("SELECT * FROM Customer WHERE Id={0}", item.Customer);
                item.CustomerObj = Read<Customer>(cmd);

                cmd.CommandText = String.Format("SELECT * FROM Product WHERE Id={0}", item.Product);
                item.ProductObj = Read<Product>(cmd);
            }

            SafeClose();

            return list;
        }

        public bool AddToBasket(int id, int customer, int quantity)
        {
            if (GetProduct(id).Units - quantity < 0)
                return false;

            var cmd = CreateCmd();

            cmd.CommandText = "INSERT INTO Basket " +
                    "VALUES(NULL, @Customer, @Product, @Quantity)";

            cmd.Prepare();

            // Add values
            cmd.Parameters.AddWithValue("@Customer", customer);
            cmd.Parameters.AddWithValue("@Product", id);
            cmd.Parameters.AddWithValue("@Quantity", quantity);

            ExecuteQueryAndClose(cmd);
            return true;
        }
    }
}
