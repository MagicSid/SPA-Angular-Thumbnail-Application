using MySql.Data.MySqlClient;
using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace SPA_angular_test.Controllers
{
    public class Thumbnail_Saving
    {


        static public void Scanner(string path)
        {

            DirectoryInfo d = new DirectoryInfo(path);
            foreach (var file in d.GetFiles("*.jpg"))
            {
                Image tempimage = Resizeimage(Image.FromFile(file.FullName));
                Savethumbnail(tempimage, file.DirectoryName, file.Name);
            }
            foreach (var file in d.GetFiles("*.jpeg"))
            {
                Image tempimage = Resizeimage(Image.FromFile(file.FullName));
                Savethumbnail(tempimage, file.DirectoryName, file.Name);
            }

        }

        static private Image Resizeimage(Image image)
        {
            Double newwidth;
            Double newheight;
            if (image.Width >= image.Height)
            {
                newwidth = 180;
                newheight = Math.Floor((Double)(image.Height / (image.Width / 180)));
            }
            else
            {
                newheight = 180;
                newwidth = Math.Floor((Double)(image.Width / (image.Height / 180)));
            }
            Console.WriteLine("(" + image.Height + "," + image.Width + ") + new proportions (" + newheight + "," + newwidth + ")");
            return image.GetThumbnailImage((int)Math.Floor(newwidth), (int)Math.Floor(newheight), null, IntPtr.Zero);
        }
        static private void Savethumbnail(Image image, String path, string name)
        {
            String fullpath = path + "\\Thumbnails";
            Directory.CreateDirectory(fullpath);
            image.Save(fullpath + "\\thumb" + name);
            SaveToDB(image, name, path);
        }

        private static void SaveToDB(Image image, string name, String path)
        {
            byte[] bytearray = new byte[0];
            using(MemoryStream stream = new MemoryStream())
            {
                image.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                stream.Close();
                bytearray = stream.ToArray();
            }
            
            SendDataToDB(bytearray, name.Substring(0,name.LastIndexOf(".")), path); 
        }


        private static void SendDataToDB(byte[] blob, string name, String path)
        {
            MySqlConnection con = new MySqlConnection();
            con.ConnectionString = sqlconnectionbuilder();
            MySqlCommand command = con.CreateCommand();
            command.CommandText = "INSERT INTO images (imagename, image, size) VALUES (?imagename, ?image, ?size)";
            MySqlParameter imagenameparameter = new MySqlParameter("?imagename",MySqlDbType.VarChar,20);
            MySqlParameter imageparameter = new MySqlParameter("?image",MySqlDbType.Blob ,blob.Length);
            MySqlParameter sizeparameter = new MySqlParameter("?size", MySqlDbType.Float, 20);
            imagenameparameter.Value = name;
            imageparameter.Value = blob;
            try
            {
                FileInfo inf = new FileInfo(path+"\\"+name+".jpg");
                sizeparameter.Value = (float)inf.Length;
            } catch (FileNotFoundException)
            {
                FileInfo inf = new FileInfo(path + "\\" + name + ".jpeg");
                sizeparameter.Value = (float)inf.Length;
            } catch 
            {
                System.Diagnostics.Debug.WriteLine("Dev Note: Add Appropriate File Extension here");
            }
            command.Parameters.Add(imagenameparameter);
            command.Parameters.Add(imageparameter);
            command.Parameters.Add(sizeparameter);
            con.Open();
            command.ExecuteNonQuery();
            con.Close();

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
