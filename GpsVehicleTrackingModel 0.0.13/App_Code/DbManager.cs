using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using System.Data;
using System.Configuration;

/// <summary>
/// Summary description for DbManager
/// </summary>
public class DbManager
{
     MySqlConnection con = new MySqlConnection(ConfigurationSettings.AppSettings["ConnectionString"]);
 
    public   int Execute(MySqlCommand cmd)
    {

        try
        {
            int changes = 0;
            lock (cmd)
            {
                cmd.Connection = con;
                if (cmd.Connection.State == ConnectionState.Open)
                {
                    cmd.Connection.Close();
                }
                cmd.Connection.Open();
                changes = cmd.ExecuteNonQuery();
                cmd.Connection.Close();
            }
            return changes;
        }
        catch (Exception ex)
        {
            cmd.Connection.Close();
        throw new ApplicationException(ex.Message);
        }
        //}
    }

    public   DataSet SelectQuery(MySqlCommand cmd)
    {

        lock (cmd)
        {
            try
            {
                DataSet ds = new DataSet();
                cmd.Connection = con;
                if (cmd.Connection.State == ConnectionState.Open)
                {
                    cmd.Connection.Close();
                }
                con.Open();
                //cmd.ExecuteNonQuery();
                MySqlDataAdapter sda = new MySqlDataAdapter();
                sda.SelectCommand = cmd;
                sda.Fill(ds, "Table");
                con.Close();
                return ds;
            }
            catch (Exception ex)
            {
                con.Close();
                throw new ApplicationException(ex.Message);
            }
        }
    }
    public static DateTime GetTime(MySqlConnection conn)
    {

        DataSet ds = new DataSet();
        DateTime dt = DateTime.Now;
        MySqlCommand cmd = new MySqlCommand("SELECT NOW()");
        cmd.Connection = conn;
        if (cmd.Connection.State == ConnectionState.Open)
        {
            cmd.Connection.Close();
        }
        conn.Open();
        //cmd.ExecuteNonQuery();
        MySqlDataAdapter sda = new MySqlDataAdapter();
        sda.SelectCommand = cmd;
        sda.Fill(ds, "Table");
        conn.Close();
        if (ds.Tables[0].Rows.Count > 0)
        {
            dt = (DateTime)ds.Tables[0].Rows[0][0];
        }
        return dt;

    }

}
