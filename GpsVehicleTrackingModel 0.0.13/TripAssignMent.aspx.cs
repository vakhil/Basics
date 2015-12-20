using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;

public partial class TripAssignMent : System.Web.UI.Page
{
    MySqlCommand cmd;
    string UserName = "";
    VehicleDBMgr vdm;
    DataDownloader ddwnldr;
    protected void Page_Load(object sender, EventArgs e)
    {
        /////////////////.................MyLocation....................////////////////
        if (Session["field1"] == null)
            Response.Redirect("Login.aspx");
        else
        {
            UserName = Session["field1"].ToString();
            Session["field2"] = "MyLocationtrue";
            vdm = new VehicleDBMgr();
            vdm.InitializeDB();
            if (!Page.IsPostBack)
            {
                if (!Page.IsCallback)
                {
                    ddwnldr = new DataDownloader();
                    vdm.InitializeDB();
                    fillPlant();
                    fillvehicle();
                    Updatetripconfiguration();
                }
            }
        }
    }
    public void fillvehicle()
    {
        cmd = new MySqlCommand("select VehicleNumber from PairedData where UserID=@UserName");
        cmd.Parameters.Add("@UserName", UserName);
        DataTable dtVehicleno = vdm.SelectQuery(cmd).Tables[0];
        foreach (DataRow dr in dtVehicleno.Rows)
        {
            ddlVehicleno.Items.Add(dr["VehicleNumber"].ToString());
        }
    }
    public void fillPlant()
    {
        ddlPlantName.Items.Clear();
        string Itemtype = "Plant Name";
        cmd = new MySqlCommand("SELECT SNo,UserName, BranchID, Description, Latitude, Longitude, PhoneNumber, ImagePath, ImageType, Radious, PlantName, IsPlant FROM branchdata WHERE (UserName = @UserName) AND (IsPlant = '1')");
        cmd.Parameters.Add("@UserName", UserName);
        DataTable dtPlant = vdm.SelectQuery(cmd).Tables[0];
        ddlPlantName.DataSource = dtPlant;
        ddlPlantName.DataTextField = "BranchID";
        ddlPlantName.DataValueField = "SNo";
        ddlPlantName.DataBind();

        
    }
    public void Updatetripconfiguration()
    {
        cmd = new MySqlCommand("SELECT paireddata.VehicleNumber, tripconfiguration.sno, tripconfiguration.TripName, tripconfiguration.StartTime, tripconfiguration.EndTime, tripconfiguration.Status AS s, '' AS Status, tripconfiguration.CreationDate, tripconfiguration.Isrepeat AS r, '' AS Isrepeat, routetable.RouteName, branchdata.BranchID, tripconfiguration.PlantName, tripconfiguration.Kms, tripconfiguration.extrakms AS Emptykms, tripconfiguration.Chargeperkm FROM tripconfiguration INNER JOIN paireddata ON tripconfiguration.Veh_Sno = paireddata.Sno INNER JOIN routetable ON tripconfiguration.RouteID = routetable.SNo INNER JOIN branchdata ON tripconfiguration.PlantName = branchdata.Sno WHERE (tripconfiguration.UserID = @UserID)");
        //cmd = new MySqlCommand(" SELECT  paireddata.VehicleNumber, tripconfiguration.sno, tripconfiguration.TripName, tripconfiguration.StartTime, tripconfiguration.EndTime, tripconfiguration.Status AS s, '' AS Status, tripconfiguration.CreationDate, tripconfiguration.Isrepeat AS r, '' AS Isrepeat, routetable.RouteName, vehiclemanage.ItemName,tripconfiguration.PlantName,tripconfiguration.Kms,tripconfiguration.extrakms as Emptykms,tripconfiguration.Chargeperkm FROM tripconfiguration INNER JOIN paireddata ON tripconfiguration.Veh_Sno = paireddata.Sno INNER JOIN routetable ON tripconfiguration.RouteID = routetable.SNo INNER JOIN vehiclemanage ON tripconfiguration.PlantName = vehiclemanage.SNo WHERE        (tripconfiguration.UserID = @UserID)"); 
        cmd.Parameters.Add("@UserID", UserName);
        DataTable dt = vdm.SelectQuery(cmd).Tables[0];
        
        foreach (DataRow dr in dt.Rows)
        {
            if (dr["s"].ToString() == "1")
            {
                dr["Status"] = "Active";
            }
            else 
            {
                dr["Status"] = "Inactive";
            }
            if (dr["r"].ToString() == "1")
            {
                dr["Isrepeat"] = "true";
            }
            else
            {
                dr["Isrepeat"] = "false";
            }
        }
        grdTrip.DataSource = dt;
        grdTrip.DataBind();
      //  grdTrip.Columns[0].Visible = false;
    }

    protected void btnRefresh_OnClick(object sender, EventArgs e)
    {
        Refresh();
    }
    protected void btnTripSave_OnClick(object sender, EventArgs e)
    {
        try
        {
            if (btnTripSave.Text == "Save")
            {
                string RouteName = txtRouteName.Value;
                string Vehicleno = ddlVehicleno.SelectedValue;
                cmd = new MySqlCommand("Select Sno from paireddata where VehicleNumber=@VehicleNumber and UserID=@UserID ");
                cmd.Parameters.Add("@VehicleNumber", Vehicleno);
                cmd.Parameters.Add("@UserID", UserName);
                DataTable dtvehicleNo = vdm.SelectQuery(cmd).Tables[0];
                int vehicleRefNo = (int)dtvehicleNo.Rows[0]["Sno"];
                cmd = new MySqlCommand("Select SNo from routetable where RouteName=@RouteName and UserID=@UserID ");
                cmd.Parameters.Add("@RouteName", RouteName);
                cmd.Parameters.Add("@UserID", UserName);
                DataTable dtRouteName = vdm.SelectQuery(cmd).Tables[0];
                int RouteRefNo = (int)dtRouteName.Rows[0]["SNo"];

                cmd = new MySqlCommand("Insert into tripconfiguration(TripName,veh_sno,StartTime,EndTime,Kms,extrakms,Chargeperkm,RouteID,Status,Isrepeat,UserID,PlantName)values(@TripName,@veh_sno,@StartTime,@EndTime,@Kms,@extrakms,@Chargeperkm,@RouteID,@Status,@Isrepeat,@UserID,@PlantName)");
                cmd.Parameters.Add("@TripName", txtTripName.Value);
                cmd.Parameters.Add("@veh_sno", vehicleRefNo);
                cmd.Parameters.Add("@StartTime", txtstartTime.Text);
                cmd.Parameters.Add("@EndTime", txtendtime.Text);
                cmd.Parameters.Add("@Kms", txtKms.Text);
                cmd.Parameters.Add("@extrakms", txt_extrakms.Text);
                cmd.Parameters.Add("@Chargeperkm", txt_charge.Text);
                cmd.Parameters.Add("@RouteID", RouteRefNo);
                cmd.Parameters.Add("@PlantName", ddlPlantName.SelectedValue);
                if (ddlStatus.Value == "Active")
                {
                    cmd.Parameters.Add("@Status", true);
                }
                else if (ddlStatus.Value == "Inactive")
                {
                    cmd.Parameters.Add("@Status", false);
                }
                if (ckbRepeat.Checked == true)
                {
                    cmd.Parameters.Add("@Isrepeat", true);
                }
                else if (ckbRepeat.Checked == false)
                {
                    cmd.Parameters.Add("@Isrepeat", false);
                }
                cmd.Parameters.Add("@UserID", UserName);
                vdm.insert(cmd);
                Refresh();
                lblmsg.Text = "Saved Successfully";
                Updatetripconfiguration();
            }
            else if (btnTripSave.Text == "Edit")
            {
                string RouteName = "";
                int vehicleRefNo;
                if (Session["RouteName"] != null)
                {
                    RouteName = Session["RouteName"].ToString();
                }
                else
                {
                    RouteName = txtRouteName.Value;
                }
                cmd = new MySqlCommand("Select Sno from paireddata where VehicleNumber=@VehicleNumber and UserID=@UserID ");
                cmd.Parameters.Add("@VehicleNumber", ddlVehicleno.SelectedValue);
                cmd.Parameters.Add("@UserID", UserName);
                DataTable dtvehicleNo = vdm.SelectQuery(cmd).Tables[0];
                vehicleRefNo = (int)dtvehicleNo.Rows[0]["Sno"];
                cmd = new MySqlCommand("Select SNo from routetable where RouteName=@RouteName and UserID=@UserID ");
                cmd.Parameters.Add("@RouteName", RouteName);
                cmd.Parameters.Add("@UserID", UserName);
                DataTable dtRouteName = vdm.SelectQuery(cmd).Tables[0];
                int RouteRefNo = (int)dtRouteName.Rows[0]["SNo"];
                cmd = new MySqlCommand("update  tripconfiguration set TripName=@TripName,veh_sno=@veh_sno,StartTime=@StartTime,EndTime=@EndTime,Kms=@Kms,extrakms=@extrakms,Chargeperkm=@Chargeperkm,RouteID=@RouteID,Status=@Status,Isrepeat=@Isrepeat,PlantName=@PlantName where UserID=@UserID and sno=@sno");
                cmd.Parameters.Add("@TripName", txtTripName.Value);
                cmd.Parameters.Add("@veh_sno", vehicleRefNo);
                cmd.Parameters.Add("@StartTime", txtstartTime.Text);
                cmd.Parameters.Add("@EndTime", txtendtime.Text);
                cmd.Parameters.Add("@Kms", txtKms.Text);
                cmd.Parameters.Add("@extrakms", txt_extrakms.Text);
                cmd.Parameters.Add("@Chargeperkm", txt_charge.Text);
                cmd.Parameters.Add("@RouteID", RouteRefNo);
                cmd.Parameters.Add("@PlantName", ddlPlantName.SelectedValue);
                cmd.Parameters.Add("@sno", ViewState["sno"]);
                if (ddlStatus.Value == "Active")
                {
                    cmd.Parameters.Add("@Status", true);
                }
                else if (ddlStatus.Value == "Inactive")
                {
                    cmd.Parameters.Add("@Status", false);
                }
                if (ckbRepeat.Checked == true)
                {
                    cmd.Parameters.Add("@Isrepeat", true);
                }
                else if (ckbRepeat.Checked == false)
                {
                    cmd.Parameters.Add("@Isrepeat", false);
                }
                cmd.Parameters.Add("@UserID", UserName);
                vdm.Update(cmd);
                Refresh();
                lblmsg.Text = "Updated Successfully";
                Updatetripconfiguration();
                btnTripSave.Text = "Save";
            }
            ScriptManager.RegisterStartupScript(Page, GetType(), "JsStatus", "SelectionTrip();", true);
        }
        catch(Exception ex)
        {
            lblmsg.Text = ex.ToString();
        }

    }
    void Refresh()
    {
        lblmsg.Text = "";
        txtTripName.Value ="";
        txtstartTime.Text = "";
        txtendtime.Text = "";
        txtRouteName.Value = "";
        txt_extrakms.Text = "";
        txt_charge.Text = "";
        txtKms.Text = "";
        ddlPlantName.ClearSelection();
    }
    protected void grdTrip_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (grdTrip.SelectedIndex > -1)
        {
            try
            {
                GridViewRow gvr = grdTrip.SelectedRow;
                txtTripName.Value = gvr.Cells[3].Text;
                ddlVehicleno.SelectedValue = gvr.Cells[1].Text;
                Session["status"] = gvr.Cells[7].Text;
                string starttime=gvr.Cells[4].Text;
                txtstartTime.Text = starttime.Substring(0, starttime.LastIndexOf(':')) ;
                string Endtime = gvr.Cells[5].Text;
                txtendtime.Text = Endtime.Substring(0, Endtime.LastIndexOf(':'));
                txtRouteName.Value = gvr.Cells[11].Text;
                if (gvr.Cells[12].Text != "&nbsp;" || gvr.Cells[12].Text == "")
                {
                    ddlPlantName.SelectedValue= gvr.Cells[13].Text;
                }
                if (gvr.Cells[14].Text == "" || gvr.Cells[14].Text == "&nbsp;")
                {
                    txtKms.Text = "0";
                }
                else
                {
                    txtKms.Text = gvr.Cells[14].Text;
                }
                if (gvr.Cells[15].Text == "" || gvr.Cells[15].Text == "&nbsp;")
                {
                    txt_extrakms.Text = "0";
                }
                else
                {
                    txt_extrakms.Text = gvr.Cells[15].Text;
                }
                if (gvr.Cells[16].Text == "" || gvr.Cells[16].Text == "&nbsp;")
                {
                    txt_charge.Text = "0";
                }
                else
                {
                    txt_charge.Text = gvr.Cells[16].Text;
                }
               ViewState["sno"] = gvr.Cells[2].Text;
                lblmsg.Text = "";
                btnTripSave.Text = "Edit";
                if (gvr.Cells[9].Text == "1")
                {
                    ckbRepeat.Checked = true;
                }
                ScriptManager.RegisterStartupScript(Page, GetType(), "JsStatus", "SelectionTrip();", true);
            }
            catch
            {
            }
        }
    }

    protected void grdTrip_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[6].Visible = false;
        e.Row.Cells[2].Visible = false;
        e.Row.Cells[9].Visible = false;
        e.Row.Cells[13].Visible = false;
    }
}