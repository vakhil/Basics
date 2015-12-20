using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;

public partial class StatusReport : System.Web.UI.Page
{
    //MySqlCommand cmd;
    //string UserName = "";
    //VehicleDBMgr vdm;
    //DataDownloader ddwnldr;
    protected void Page_Load(object sender, EventArgs e)
    {
        //if (Session["field1"] == null)
        //    Response.Redirect("Login.aspx");
        //else
        //{

        //    UserName = Session["field1"].ToString();
        //    vdm = new VehicleDBMgr();
        //    vdm.InitializeDB();
        //    if (!Page.IsPostBack)
        //    {
        //        if (!Page.IsCallback)
        //        {

        //        }
        //    }
        //}
    }
    //protected void btn_generate_Click(object sender, EventArgs e)
    //{
    //    DateTime fromdate = DateTime.Now;
    //    DateTime todate = DateTime.Now;
    //    string[] datestrig = startdate.Text.Split(' ');
    //    if (datestrig.Length > 1)
    //    {
    //        if (datestrig[0].Split('-').Length > 0)
    //        {
    //            string[] dates = datestrig[0].Split('-');
    //            string[] times = datestrig[1].Split(':');
    //            fromdate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
    //        }
    //    }
    //    datestrig = enddate.Text.Split(' ');
    //    if (datestrig.Length > 1)
    //    {
    //        if (datestrig[0].Split('-').Length > 0)
    //        {
    //            string[] dates = datestrig[0].Split('-');
    //            string[] times = datestrig[1].Split(':');
    //            todate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
    //        }
    //    }
    //}
}