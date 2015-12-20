using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;

public partial class TripView : System.Web.UI.Page
{
    VehicleDBMgr vdm;
    DataDownloader ddwnldr;
    MySqlCommand cmd;
    string UserName;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["field1"] == null)
            Response.Redirect("Login.aspx");
        else
        {
            UserName = Session["field1"].ToString();
            vdm = new VehicleDBMgr();
            vdm.InitializeDB();
            if (!this.IsPostBack)
            {
                if (!Page.IsCallback)
                {
                    Update();
                }
            }
        }
    }
    void Update()
    {
        cmd = new MySqlCommand("Select * from tripdata where UserID=@UserID and Status='A'");
        cmd.Parameters.Add("@UserID", UserName);
        DataTable dtTrip = vdm.SelectQuery(cmd).Tables[0];
        foreach (DataRow drTrip in dtTrip.Rows)
        {
            int TripRefNo = (int)drTrip["Refno"];
            cmd = new MySqlCommand("Select * from tripsubdata where Refno=@Refno order by Rank");
            cmd.Parameters.Add("@Refno", TripRefNo);
            DataTable dtSubtrip = vdm.SelectQuery(cmd).Tables[0];
            if (dtSubtrip.Rows.Count != 0)
            {
                GridView0.DataSource = dtSubtrip;
                GridView0.DataBind();
            }
            cmd = new MySqlCommand("Select VehicleNumber from paireddata where Sno='" + drTrip["Vehiclemaster_sno"].ToString() + "'");
            DataTable dtVehicleNo = vdm.SelectQuery(cmd).Tables[0];
            string VehicleNo = dtVehicleNo.Rows[0]["VehicleNumber"].ToString();
            //foreach (DataRow drSubTrip in dtSubtrip.Rows)
            //{
            //Tripclass GetTrip = new Tripclass();
            //GetTrip.TripName = drTrip["Tripid"].ToString();
            //GetTrip.VehicleNo = VehicleNo;
            //GetTrip.RouteName = drTrip["RouteName"].ToString();
            //GetTrip.Assigndate = drTrip["assigndate"].ToString();
            //GetTrip.Sno = drSubTrip["rank"].ToString();
            //cmd = new MySqlCommand("Select BranchID from branchdata where Sno='" + drSubTrip["locid"].ToString() + "'");
            //DataTable dtBranchName = vdm.SelectQuery(cmd).Tables[0];
            //string BranchName = dtBranchName.Rows[0]["BranchID"].ToString();
            //GetTrip.LocationName = BranchName;
            //GetTrip.EnterTime = drSubTrip["intime"].ToString();
            //Getriplist.Add(GetTrip);
            //}
        }
    }
}