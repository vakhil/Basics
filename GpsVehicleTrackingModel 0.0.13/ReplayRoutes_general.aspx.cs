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
    MySqlCommand cmd;
    VehicleDBMgr vdm;
    DataDownloader ddwnldr;
    string UserName = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["field1"] == null)
            Response.Redirect("Login.aspx");
        else
        {
            //vdm.UserName = Session["field1"].ToString();
            UserName = Session["field1"].ToString();
            vdm = new VehicleDBMgr();
            vdm.InitializeDB();
            if (!this.IsPostBack)
            {
                if (!Page.IsCallback)
                {
                    ddwnldr = new DataDownloader();
                    ddwnldr.UpdateBranchDetails(UserName);
                    UpdateVehicleGroupData();
                  //  UpdateVehicleGroupData();
                }
            }
        }
    }


    public void UpdateVehicleGroupData()
    {
        Session["Authorized"] = "Plants";
        DataTable totaldata = new DataTable();
        if (Session["vendorstable"] != null)
        {
            totaldata = (DataTable)Session["vendorstable"];
        }
        else
        {
            cmd = new MySqlCommand("SELECT cabmanagement.PlantName, cabmanagement.VehicleType, cabmanagement.VehicleID,cabmanagement.RouteName, cabmanagement.RouteCode FROM cabmanagement INNER JOIN loginsconfigtable ON cabmanagement.VehicleID = loginsconfigtable.VehicleID INNER JOIN  loginstable ON cabmanagement.UserID = loginstable.main_user AND loginsconfigtable.Refno = loginstable.refno WHERE (loginstable.loginid = @UserName)");
            //cmd = new MySqlCommand("SELECT cabmanagement.PlantName, cabmanagement.VehicleType, cabmanagement.VehicleID FROM cabmanagement INNER JOIN loginsconfigtable ON cabmanagement.VehicleID = loginsconfigtable.VehicleID INNER JOIN  loginstable ON cabmanagement.UserID = loginstable.main_user AND loginsconfigtable.Refno = loginstable.refno WHERE (loginstable.loginid = @UserName)");
            cmd.Parameters.Add("@UserName", UserName);
            totaldata = vdm.SelectQuery(cmd).Tables[0];
            Session["vendorstable"] = totaldata;
        }

        //cmd = new MySqlCommand("SELECT UserName, BranchID, Description, Latitude, Longitude, PhoneNumber, ImagePath, Radious,Sno, PlantName, IsPlant FROM branchdata WHERE (UserName = @UserName) AND (IsPlant = '1')");
        //cmd.Parameters.Add("@UserName", UserName);
        //DataTable dtPlant = vdm.SelectQuery(cmd).Tables[0];
        DataView view = new DataView(totaldata);
        DataTable dtPlant = view.ToTable(true, "PlantName");

        view = new DataView(totaldata);
        DataTable vehicletypes = view.ToTable(true, "VehicleType");
        chblVehicleTypes.Items.Clear();
        chblZones.Items.Clear();

        if (chblVehicleTypes.SelectedIndex == -1)
        {
            chblVehicleTypes.Items.Add("All Vehicle Types");
        }
        if (chblZones.SelectedIndex == -1)
        {
            chblZones.Items.Add("All Plants");
        }
        foreach (DataRow dr in vehicletypes.Rows)
        {
            if (dr["VehicleType"].ToString() != "")
                chblVehicleTypes.Items.Add(dr["VehicleType"].ToString());
        }
        foreach (DataRow dr in dtPlant.Rows)
        {
            if (dr["PlantName"].ToString() != "")
                chblZones.Items.Add(dr["PlantName"].ToString());
        }
    }
}