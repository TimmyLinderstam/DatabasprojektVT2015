using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using Webshop.App_Start;

namespace Webshop.DBM
{
    public class DBManager
    {
        private bool ConnectionOpen { get; set; }

        private MySqlConnection Connection { get; set; }

        public DBManager()
        {
            GetConnection();
            CreateTablesIfNotExists();
        }

        private void CreateTablesIfNotExists()
        {

            if (File.Exists(HostingEnvironment.MapPath(@"~/Content/tables.sql")))
            {
                var cmd = CreateCmd();
                String tables = File.ReadAllText(HostingEnvironment.MapPath(@"~/Content/tables.sql"));
                cmd.CommandText = tables;
                cmd.ExecuteNonQuery();
            }
        }

        public void ExecuteQuery(MySqlCommand cmd)
        {
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error occurred loading the Column details: "
                    + e.ErrorCode + " - " + e.Message;

            }
        }

        public T Read<T>(string sql)
        {
            var cmd = CreateCmd();
            cmd.CommandText = sql;

            MySqlDataReader reader = cmd.ExecuteReader();

            var list = new List<Dictionary<string, object>>();
            while (reader.Read())
            {
                var row = new Dictionary<string, object>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    row.Add(reader.GetName(i), reader.IsDBNull(i) ? null : reader.GetValue(i));
                }

                list.Add(row);
            }

            reader.Close();

            if (list.Count == 1)
            {
                return list[0].GetObject<T>();
            }

            var def = default(T);
            if (def is ValueType && Nullable.GetUnderlyingType(typeof(T)) == null)
            {
                throw new InvalidOperationException(
                    string.Format("Cannot instantiate with non-nullable type: {0}",
                        typeof(T)));
            }
            return def;
        }

        public List<T> ReadList<T>(MySqlCommand cmd)
        {
             MySqlDataReader reader = null;
            try
            {
                reader = cmd.ExecuteReader();

                var list = new List<Dictionary<string, object>>();
                while (reader.Read())
                {
                    var row = new Dictionary<string, object>();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        row.Add(reader.GetName(i), reader.IsDBNull(i) ? null : reader.GetValue(i));
                    }

                    list.Add(row);
                }



                List<T> results = new List<T>();
                foreach (var item in list)
                {
                    results.Add(item.GetObject<T>());
                }

                return results;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

        }

        protected MySqlCommand CreateCmd()
        {
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = Connection;
            return cmd;
        }

        private void GetConnection()
        {
            ConnectionOpen = false;

            Connection = new MySqlConnection();
            Connection.ConnectionString = ConfigurationManager.ConnectionStrings["ShopConnection"].ConnectionString;

            if (OpenLocalConnection())
            {
                ConnectionOpen = true;
            }
            else
            {
                throw new Exception("Please Open a connection first");
            }

        }

        private bool OpenLocalConnection()
        {
            try
            {
                Connection.Open();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }



        ~DBManager()
        {
            Console.WriteLine("Closed SQL Connection");
            Connection.Close();
        }

    }
}
