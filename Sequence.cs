﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace Utils
{
    public static class Sequence
    {
        /// <summary>
        /// Retorna o próximo número da sequência informada no parametro. 
        /// </summary>
        /// <param name="Sequence"></param>
        /// <returns></returns>
        public static long GetNextVal(string Sequence)
        {
            string ConnectionString = "server=localhost;user id=root;Password=pass4admin;database=dbintegracao";
            long retorno = 0;
            MySqlConnection con = new MySqlConnection(ConnectionString);
            try
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT nextval('" + Sequence + "') as nextsequence", con);
                MySqlDataReader dr = cmd.ExecuteReader();
                dr.Read();
                retorno = dr.GetInt64("nextsequence");                
                dr.Close();
                dr.Dispose();
                cmd.Dispose();
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

        public static long GetCurrentVal(string Sequence)
        {
            string ConnectionString = ConfigurationManager.ConnectionStrings["MySQLEntities_local"].ConnectionString;
            long retorno = 0;
            MySqlConnection con = new MySqlConnection(ConnectionString);
            try
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT currentval('" + Sequence + "') as currentsequence", con);
                MySqlDataReader dr = cmd.ExecuteReader();
                dr.Read();
                retorno = dr.GetInt64("currentsequence");
                dr.Close();
                dr.Dispose();
                cmd.Dispose();
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

        public static void SetCurrentVal(string Sequence, long Value)
        {
            string ConnectionString = ConfigurationManager.ConnectionStrings["MySQLEntities_local"].ConnectionString;            
            MySqlConnection con = new MySqlConnection(ConnectionString);
            try
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("UPDATE sequence_data SET sequence_cur_value = "+ Convert.ToString(Value)+ " WHERE sequence_name = " + Sequence, con);
                cmd.ExecuteNonQuery();                
                cmd.Dispose();
                
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
    }
}