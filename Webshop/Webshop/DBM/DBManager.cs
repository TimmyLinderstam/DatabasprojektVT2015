using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webshop.DBM
{
    public class DBManager
    {
        private static DBManager _instance;
        public static DBManager Instance { get { if (_instance == null) _instance = new DBManager(); return _instance; } private set { _instance = value; } }

        private bool ConnectionOpen { get; set; }

        private MySqlConnection Connection { get; set; }

        public DBManager()
        {
            GetConnection();
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

        public T ReadQuery<T>(MySqlCommand cmd)
        {
            MySqlDataReader reader = cmd.ExecuteReader();

            List<Object> list = new List<object>();
            while (reader.Read())
            {
                dynamic row = new ExpandoObject();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    row[reader.GetName(i)] = reader.GetValue(i);
                }

                list.Add(row);
            }


            if (list.Count == 1)
            {
                return (T)list[0];
            }
            // else todo

            return default(T);
        }


        public void Test()
        {
            try
            {
                /* MySqlCommand cmd = new MySqlCommand();
                 cmd.Connection = Connection;
                 cmd.CommandText = "INSERT INTO Authors(Name) VALUES(@Name)";
                 cmd.Prepare();

                 cmd.Parameters.AddWithValue("@Name", "Trygve Gulbranssen");
                 cmd.ExecuteNonQuery();
                 */

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = Connection;
                cmd.CommandText =
            string.Format("select * from Anstalld where AnstID = '{0}'", 1);

                //cmd.

                MySqlDataReader reader = cmd.ExecuteReader();



                try
                {
                    List<Object> b = new List<object>();
                    while (reader.Read())
                    {
                        dynamic row = new ExpandoObject();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            row[reader.GetName(i)] = reader.GetValue(i);
                        }

                        b.Add(row);

                        // Console.WriteLine(reader.GetString(0).PadRight(18) +
                        //  reader.GetString(1));
                    }

                    reader.Read();
                    String Name;
                    if (reader.IsDBNull(1) == false)
                        Name = reader.GetString(1);
                    else
                        Name = null;


                    reader.Close();

                }
                catch (MySqlException e)
                {
                    string MessageString = "Read error occurred  / entry not found loading the Column details: "
                        + e.ErrorCode + " - " + e.Message + "; \n\nPlease Continue";

                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error occurred loading the Column details: "
                    + e.ErrorCode + " - " + e.Message;

            }


            // Connection.Close();

        }

        private MySqlCommand Create()
        {
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = Connection;
            return cmd;
        }

        private void GetConnection()
        {
            ConnectionOpen = false;

            Connection = new MySqlConnection();
            Connection.ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

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
            Connection.Close();
        }

    }
}
