using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;

public partial class ReplayRoutes : System.Web.UI.Page
{
    //MySqlCommand cmd;
    //static VehicleDBMgr vdm;
    //static string UserName = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        //if (Session["field1"] == null)
        //    Response.Redirect("Login.aspx");
        //if (!Page.IsCallback)
        //{
        //    if (!Page.IsPostBack)
        //    {
        //vdm = new VehicleDBMgr();
        //vdm.InitializeDB();
        //UserName = Session["field1"].ToString();
        //cmd = new MySqlCommand("SELECT UserName, BranchID, Description, Latitude, Longitude, PhoneNumber, ImagePath, ImageType, Radious, PlantName, IsPlant FROM branchdata WHERE (UserName = @UserName) AND (IsPlant = '1')");
        //cmd.Parameters.Add("@UserName", UserName);
        //DataTable plants = vdm.SelectQuery(cmd).Tables[0];
        //ddl_plant.Items.Clear();
        //if (ddl_plant.SelectedIndex == -1)
        //{
        //    ddl_plant.Items.Add("Select Plant");
        //}
        //foreach (DataRow dr in plants.Rows)
        //{
        //    ddl_plant.Items.Add(dr["BranchID"].ToString());
        //}
        //cmd = new MySqlCommand("SELECT sno, TripName, StartTime, EndTime, RouteID, Status, CreationDate, Isrepeat, UserID, PlantName, Kms, extrakms, Chargeperkm FROM tripconfiguration WHERE (UserID = @UserName)");
        //cmd.Parameters.Add("@UserName", UserName);
        //DataTable routetabledata = vdm.SelectQuery(cmd).Tables[0];
        //ddl_trip.Items.Clear();
        //if (ddl_trip.SelectedIndex == -1)
        //{
        //    ddl_trip.Items.Add("Select Trip");
        //}
        //foreach (DataRow dr in routetabledata.Rows)
        //{
        //    ddl_trip.Items.Add(dr["TripName"].ToString());
        //}
        //startdate.Text = GetLowDate(DateTime.Now).ToString("dd-MM-yyyy HH:mm");
        //enddate.Text = GetHighDate(DateTime.Now).ToString("dd-MM-yyyy HH:mm");
        //    }
        //}
    }
    //private DateTime GetLowDate(DateTime dt)
    //{
    //    double Hour, Min, Sec;
    //    DateTime DT = DateTime.Now;
    //    DT = dt;
    //    Hour = -dt.Hour;
    //    Min = -dt.Minute;
    //    Sec = -dt.Second;
    //    DT = DT.AddHours(Hour);
    //    DT = DT.AddMinutes(Min);
    //    DT = DT.AddSeconds(Sec);
    //    return DT;

    //}

    //private DateTime GetHighDate(DateTime dt)
    //{
    //    double Hour, Min, Sec;
    //    DateTime DT = DateTime.Now;
    //    Hour = 23 - dt.Hour;
    //    Min = 59 - dt.Minute;
    //    Sec = 59 - dt.Second;
    //    DT = dt;
    //    DT = DT.AddHours(Hour);
    //    DT = DT.AddMinutes(Min);
    //    DT = DT.AddSeconds(Sec);
    //    return DT;
    //}
}