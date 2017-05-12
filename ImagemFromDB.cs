using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace Utils
{
    public static class ImagemFromDB
    {
        public static Image GetImagem(Int64 Id, string tabela, string campoId)
        {
            string ConnectionString = ConfigurationManager.ConnectionStrings["prjbase.Properties.Settings.dbintegracaoConnectionString"].ConnectionString;
            Byte[] ImageByte;
            Image retorno = null;
            MySqlConnection con = new MySqlConnection(ConnectionString);
            try
            {
                con.Open();
                string sql = string.Empty;
                if (!string.IsNullOrEmpty(tabela) & !string.IsNullOrEmpty(campoId))
                {
                    sql = "SELECT imagem from " + tabela + " where " + campoId + " = ?Id";
                    MySqlCommand cmd = new MySqlCommand(sql, con);
                    cmd.Parameters.AddWithValue("?Id", Id);
                    MySqlDataReader dr = cmd.ExecuteReader();
                    dr.Read();
                    if (dr.HasRows)
                    {
                        ImageByte = (Byte[])dr["imagem"];
                        if (ImageByte != null)
                        {                            
                            retorno = ByteToImage(ImageByte);                            
                        }

                    }
                    dr.Close();
                    dr.Dispose();
                    cmd.Dispose();                    
                }

                return retorno;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }

        private static Bitmap ByteToImage(byte[] blob)
        {
            MemoryStream mStream = new MemoryStream();
            byte[] pData = blob;
            mStream.Write(pData, 0, Convert.ToInt32(pData.Length));
            Bitmap bm = new Bitmap(mStream, false);                        
            mStream.Dispose();
            return bm;

        }

        public static void setImagem(Int64 Id, string tabela, string campoId, Image imagem)
        {
            string ConnectionString = ConfigurationManager.ConnectionStrings["prjbase.Properties.Settings.dbintegracaoConnectionString"].ConnectionString;
            long? Id_Img = null;
            MySqlConnection con = new MySqlConnection(ConnectionString);
            try
            {
                if (!string.IsNullOrEmpty(tabela) & !string.IsNullOrEmpty(campoId))
                {                    
                    byte[] pic_arr = ConverterFotoParaByteArray((Image)imagem.Clone());


                    con.Open();
                    string sql = "SELECT Id from " + tabela + " where " + campoId + " = ?Id";
                    MySqlCommand cmd = new MySqlCommand(sql, con);
                    cmd.Parameters.AddWithValue("?Id", Id);

                    MySqlDataReader dr = cmd.ExecuteReader();
                    dr.Read();
                    if (dr.HasRows)
                    {
                        Id_Img = dr.GetInt64("Id");
                    }
                    dr.Close();
                    dr.Dispose();
                    cmd.Dispose();

                    sql = string.Empty;
                    if (Id_Img != null)
                    {
                        sql = @"update " + tabela + " set imagem = ?imagem where Id = ?Id";
                        MySqlCommand cmdUpdate = new MySqlCommand(sql, con);
                        cmdUpdate.Parameters.AddWithValue("?imagem", pic_arr);
                        cmdUpdate.Parameters.AddWithValue("?Id", Id_Img);
                        cmdUpdate.ExecuteNonQuery();
                        cmdUpdate.Dispose();
                    }
                    else
                    {
                        sql = @"insert into " + tabela + " (" + campoId + ", imagem) values(?campoId,?imagem)";
                        MySqlCommand cmdUpdate = new MySqlCommand(sql, con);
                        cmdUpdate.Parameters.AddWithValue("?imagem", pic_arr);
                        cmdUpdate.Parameters.AddWithValue("?campoId", Id);
                        cmdUpdate.ExecuteNonQuery();
                        cmdUpdate.Dispose();
                    }
                }
            
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }

        private static byte[] ConverterFotoParaByteArray(Image imagem)
        {
            using (var stream = new System.IO.MemoryStream())
            {
                Bitmap b = new Bitmap(imagem);
                try
                {
                    Bitmap c = b.Clone(new Rectangle(0, 0, b.Width, b.Height), PixelFormat.Format32bppRgb);
                    c.Save(stream, ImageFormat.Jpeg);
                    //imagem.Save(stream, ImageFormat.Png);
                    stream.Seek(0, System.IO.SeekOrigin.Begin);
                    byte[] bArray = new byte[stream.Length];
                    stream.Read(bArray, 0, System.Convert.ToInt32(stream.Length));
                    return bArray;
                }
                finally
                {
                    b.Dispose();
                }
                
            }
        }
    }
}
