using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace SPA_angular_test.Controllers
{
    public class Thumbnail_Retrieval
    {

        static public string[][] Retrieve_Thumbnail()
        {
            Collection<string[]> imagearray = new Collection<string[]>();
            MySqlConnection con = new MySqlConnection();
            con.ConnectionString = sqlconnectionbuilder();
            MySqlCommand command = con.CreateCommand();
            command.CommandText = "SELECT imagename,image,size FROM images;";
            con.Open();
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                string[] tempstring = new string[3] { reader.GetString(0), System.Convert.ToBase64String((byte[])reader.GetValue(1)),reader.GetString(2) };
                imagearray.Add(tempstring);
            }
            con.Close();
            return imagearray.ToArray();
        }

        private static string sqlconnectionbuilder()
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "localhost";
            builder.UserID = "Me";
            builder.Password = "";
            builder.InitialCatalog = "testing_1pagewebsite";

            return builder.ConnectionString;
        }
    }
}
