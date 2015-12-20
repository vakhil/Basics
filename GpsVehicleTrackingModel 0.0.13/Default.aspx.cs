using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using MySql.Data.MySqlClient;

public partial class Default2 : System.Web.UI.Page
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
                    //ddwnldr = new DataDownloader();
                    //ddwnldr.UpdateBranchDetails(UserName);
                    UpdateVehicleGroupData();
                    //FillMyLocations();
                }
            }
        }
    }

    //void FillMyLocations()
    //{
    //    vdm = new VehicleDBMgr();
    //    vdm.InitializeDB();
    //    cmd = new MySqlCommand("SELECT branchdata.BranchID, branchdata.Description, branchdata.Latitude, branchdata.Longitude, branchdata.PhoneNumber, branchdata.ImagePath, branchdata.ImageType, branchdata.Radious, branchdata.UserName FROM loginstable INNER JOIN branchdata ON loginstable.main_user = branchdata.UserName WHERE (loginstable.loginid = @UserName)");
    //    cmd.Parameters.Add("@UserName", UserName);
    //    DataTable Branchdata = vdm.SelectQuery(cmd).Tables[0];
    //     vdm = null;
    //        foreach (DataRow dr in Branchdata.Rows)
    //        {
    //            ddlMyLocations.Items.Add(dr["BranchID"].ToString());
    //        }
    //}
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
        chblvelstatus.Items.Add("All Vehicles");
        chblvelstatus.Items.Add("Stopped Vehicles");
        chblvelstatus.Items.Add("Running Vehicles");
        chblvelstatus.Items.Add("Inside POI Vehicles");
        chblvelstatus.Items.Add("Outside POI Vehicles");
        chblvelstatus.Items.Add("MainPower Off Vehicles");
        chblvelstatus.Items.Add("Delay in Update Vehicles");
    }
}