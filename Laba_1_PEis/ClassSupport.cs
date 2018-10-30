using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Laba_1_PEis
{
    static class ClassSupport
    {
        public static string sPath = Path.Combine(Application.StartupPath, "mybd.db");
        public static string ConnectionString = @"Data Source=" + sPath + ";New=False;Version=3";
        private static SQLiteConnection sql_con;
        private static SQLiteCommand sql_cmd;
        public class Product_count
        {
            public int Material_id { set; get; }
            public int Sum { set; get; }
            public int Count { set; get; }
            public string Date { set; get; }
            public int Day { set; get; }
            public int Month { set; get; }
            public int Year { set; get; }
        }

        public static void ExecuteQuery(string txtQuery)
        {
            sql_con = new SQLiteConnection("Data Source=" + sPath +
           ";Version=3;New=False;Compress=True;");
            sql_con.Open();
            sql_cmd = sql_con.CreateCommand();
            sql_cmd.CommandText = txtQuery;
            sql_cmd.ExecuteNonQuery();
            sql_con.Close();
        }

        public static object selectValue( String selectCommand)
        {
            SQLiteConnection connect = new
           SQLiteConnection(ConnectionString);
            connect.Open();
            SQLiteCommand command = new SQLiteCommand(selectCommand, connect);
            SQLiteDataReader reader = command.ExecuteReader();
            object value = "";
            while (reader.Read())
            {
                value = reader[0];
            }
            connect.Close();
            return value;
        }


        public static Dictionary<int, int> selectValueCheck(String selectCommand)
        {
            SQLiteConnection connect = new
           SQLiteConnection(ConnectionString);
            connect.Open();
            SQLiteCommand command = new SQLiteCommand(selectCommand, connect);
            SQLiteDataReader reader = command.ExecuteReader();
            Dictionary<int, int> value = new Dictionary<int, int>();
            while (reader.Read())
            {
                value.Add(Convert.ToInt32(reader[0]), Convert.ToInt32(reader[1]));
                
            }
            connect.Close();
            return value;
        }

        public static List<object> selectValueOthet(String selectCommand)
        {
            SQLiteConnection connect = new
           SQLiteConnection(ConnectionString);
            connect.Open();
            SQLiteCommand command = new SQLiteCommand(selectCommand, connect);
            SQLiteDataReader reader = command.ExecuteReader();
            List<object> value = new List<object>();
            while (reader.Read())
            {
                value.Add(reader[0]);
            }
            connect.Close();
            return value;
        }

        public static List<Product_count> selectValueProduct(String selectCommand)
        {
            SQLiteConnection connect = new
           SQLiteConnection(ConnectionString);
            connect.Open();
            SQLiteCommand command = new SQLiteCommand(selectCommand, connect);
            SQLiteDataReader reader = command.ExecuteReader();
            List<Product_count> value = new List<Product_count>();
            while (reader.Read())
            {
                value.Add(new Product_count {
                    Material_id=Convert.ToInt32(reader[0]),
                    Sum = Convert.ToInt32(reader[1]),
                    Count = Convert.ToInt32(reader[2]),
                    Date = reader[3].ToString(),
                    Day = Convert.ToInt32(reader[4]),
                    Month = Convert.ToInt32(reader[5]),
                    Year = Convert.ToInt32(reader[6]),

                });
            }
            connect.Close();
            return value;
        }

        public static Dictionary<int, int> selectValueList(String selectCommand)
        {
            SQLiteConnection connect = new
           SQLiteConnection(ConnectionString);
            connect.Open();
            SQLiteCommand command = new SQLiteCommand(selectCommand, connect);
            SQLiteDataReader reader = command.ExecuteReader();
            // List<object> value=new List<object>();
            Dictionary<int, int> value = new Dictionary<int, int>();
            while (reader.Read())
            {
                value.Add(Convert.ToInt32(reader[0]), Convert.ToInt32(reader[1]));
            }
            connect.Close();
            return value;
        }

        public static void changeValue( String selectCommand)
        {

            SQLiteConnection connect = new
           SQLiteConnection(ConnectionString);
            connect.Open();
            SQLiteTransaction trans;
            SQLiteCommand cmd = new SQLiteCommand();
            trans = connect.BeginTransaction();
            cmd.Connection = connect;
            cmd.CommandText = selectCommand;
            cmd.ExecuteNonQuery();
            trans.Commit();
            connect.Close();
        }
        public static DataSet Connections( String selectCommand) {

            SQLiteConnection connect = new
              SQLiteConnection(ConnectionString);
            connect.Open();
            SQLiteDataAdapter dataAdapter = new
           SQLiteDataAdapter(selectCommand, connect);
            DataSet ds = new DataSet();
            dataAdapter.Fill(ds);
            connect.Close();
            return ds;
        }


    }
}
