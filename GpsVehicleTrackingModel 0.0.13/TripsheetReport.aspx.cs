using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;

public partial class TripsheetReport : System.Web.UI.Page
{
    MySqlCommand cmd;
    string UserName = "";
    VehicleDBMgr vdm;
    DataDownloader ddwnldr;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["field1"] == null)
            Response.Redirect("Login.aspx");
        else
        {
            UserName = Session["field1"].ToString();
            vdm = new VehicleDBMgr();
            vdm.InitializeDB();
            if (!Page.IsPostBack)
            {
                if (!Page.IsCallback)
                {

                }
            }
        }
    }

    protected void btn_Generate_Click(object sender, EventArgs e)
    {
        DateTime fromdate = DateTime.Now;
        DateTime todate = DateTime.Now;
        string[] datestrig = dtp_FromDate.Text.Split(' ');
        if (datestrig.Length > 1)
        {
            if (datestrig[0].Split('-').Length > 0)
            {
                string[] dates = datestrig[0].Split('-');
                string[] times = datestrig[1].Split(':');
                fromdate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
            }
        }
        datestrig = dtp_Todate.Text.Split(' ');
        if (datestrig.Length > 1)
        {
            if (datestrig[0].Split('-').Length > 0)
            {
                string[] dates = datestrig[0].Split('-');
                string[] times = datestrig[1].Split(':');
                todate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
            }
        }
        cmd = new MySqlCommand("SELECT fleettripsheet.TripSheetNo, fleettripsheet.Tripdate AS TripStartDate, fleettripsheet.EndDate AS TripEndDate, fleettripsheet.Vehicleno, fleettripsheet.EndOdometerReading - fleettripsheet.StrartReading AS TripKMS, fleettripsheet.GpsKms, fleettripsheet.EndOdometerReading - fleettripsheet.StrartReading - fleettripsheet.GpsKms AS DifferenceKMS, fleettripsheet.StarthrMeter - fleettripsheet.EndHrMeter AS TripHours, fleettripsheet.FuelTank - fleettripsheet.EndFuelValue AS TripFuel, fleettripsheet.LoadType, fleettripsheet.Qty, routetable.RouteName, employeestable.EmployeeName AS Driver, employeestable_1.EmployeeName AS Helper FROM fleettripsheet LEFT OUTER JOIN routetable ON fleettripsheet.RouteID = routetable.SNo LEFT OUTER JOIN employeestable ON fleettripsheet.DriverID = employeestable.Sno LEFT OUTER JOIN employeestable employeestable_1 ON fleettripsheet.HelperID = employeestable_1.Sno WHERE (fleettripsheet.Tripdate >= @d1) AND (fleettripsheet.Tripdate <= @d2) AND (fleettripsheet.UserID = @UserID)");
        cmd.Parameters.Add("@d1",fromdate);
        cmd.Parameters.Add("@d2",todate);
        cmd.Parameters.Add("@UserID",UserName);
        DataTable trips = vdm.SelectQuery(cmd).Tables[0];
        string title = "Tripsheet Report From: " + fromdate.ToString() + "  To: " + todate.ToString();// +" and  TotalDistance Travelled:" + (int)TotalDistance + "\n" +
        Session["title"] = title;
        Session["xportdata"] = trips;
        dataGridView1.DataSource = trips;
        dataGridView1.DataBind();
    }
    protected void btn_Print_Click(object sender, EventArgs e)
    {

    }
}