using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;

public partial class AssignGeofense : System.Web.UI.Page
{
    MySqlCommand cmd;
    static string SlNo = "";
    string UserName = "";
    GooglePoint GP1;
    double Lvalue1 = 17.497535;
    double Lonvalue2 = 78.408622;
    static decimal MyLocationNameSno = 0;
    DataTable dtAddress = new DataTable();
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
                    FillSelectedAssignGeofenceVehicle();
                    FillSelectedGeofence();
                    UpdateAssignGeofence();
                }
            }
        }

    }
    void FillSelectedAssignGeofenceVehicle()
    {
        try
        {
            ddlSelectVehicle.Items.Clear();

            cmd = new MySqlCommand("select * from ManageData where UserName=@UserName");
            cmd.Parameters.Add("@UserName", UserName);
            DataTable dt = vdm.SelectQuery(cmd).Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                ddlSelectVehicle.Items.Add(dr["VehicleID"].ToString());
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, this);
        }
    }
    static DataTable GeofenceDataTable;
    void FillSelectedGeofence()
    {
        try
        {
            ddlSelectGeofence.Items.Clear();

            cmd = new MySqlCommand("select * from GeofenseTable where UserName=@UserName");
            cmd.Parameters.Add("@UserName", UserName);
            GeofenceDataTable = vdm.SelectQuery(cmd).Tables[0];
            foreach (DataRow dr in GeofenceDataTable.Rows)
            {
                ddlSelectGeofence.Items.Add(dr["Name"].ToString());
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, this);
        }
    }
    void UpdateAssignGeofence()
    {
        try
        {
            cmd = new MySqlCommand("select VehicleID,Geofencename ,GeofenceType,convert(varchar(8),DateOfAssign,3) as DateOfAssign,convert(varchar(8),DateofUnAssign,3) as DateofUnAssign, SlNo from AssignGeofence where   UserName=@UN");
            cmd.Parameters.Add("@UN", UserName);
            DataTable dt = vdm.SelectQuery(cmd).Tables[0];
            GrdAssignGeofence.DataSource = dt;
            GrdAssignGeofence.DataBind();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, this);
        }
    }

    protected void BtnAddAssignGeofence_Click(object sender, EventArgs e)
    {
        try
        {

            if (BtnAddAssignGeofence.Text == "Save")
            {

                if (txtDateofUnAssign.Text != "")
                {
                    cmd = new MySqlCommand("insert into AssignGeofence  (VehicleID, Geofencename, GeofenceType,DateOfAssign,DateofUnAssign,Latitude,Longitude,Radius,UserName ) values (@VehicleID, @Geofencename, @GeofenceType,@DateOfAssign,@DateofUnAssign,@Latitude,@Longitude,@Radius, @UserName)");
                }
                else
                {
                    cmd = new MySqlCommand("insert into AssignGeofence  (VehicleID, Geofencename, GeofenceType,DateOfAssign,Latitude,Longitude,Radius,UserName ) values (@VehicleID, @Geofencename, @GeofenceType,@DateOfAssign,@Latitude,@Longitude,@Radius, @UserName)");
                }
                DateTime fromdate = DateTime.Now;//System.Convert.ToDateTime(startdate.Text);//startdate_CalendarExtender.SelectedDate ?? DateTime.Now;// DateTime.Now.AddMonths(-3);//DateTime.Parse(startdate.Text); ;
                DateTime todate = DateTime.Now;//System.Convert.ToDateTime(enddate.Text);//enddate_CalendarExtender.SelectedDate ?? DateTime.Now; //DateTime.Parse(enddate.Text);
                // d/M/yyyy HH:mm
                string[] datestrig = txtDateofAssign.Text.Split(' ');

                if (datestrig.Length > 1)
                {
                    if (datestrig[0].Split('/').Length > 0)
                    {
                        string[] dates = datestrig[0].Split('/');
                        string[] times = datestrig[1].Split(':');
                        fromdate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                    }
                }
                else
                {
                    // MessageBox.Show("Date Time Format Wrong");
                    lbl_nofifier.Text = "From Date Time Format Wrong";
                    return;
                }
                if (txtDateofUnAssign.Text != "")
                {
                    datestrig = txtDateofUnAssign.Text.Split(' ');
                    if (datestrig.Length > 1)
                    {
                        if (datestrig[0].Split('/').Length > 0)
                        {
                            string[] dates = datestrig[0].Split('/');
                            string[] times = datestrig[1].Split(':');
                            todate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                        }
                    }
                    else
                    {
                        // MessageBox.Show("Date Time Format Wrong");
                        lbl_nofifier.Text = "To Date Time Format Wrong";
                        return;
                    }
                }
                cmd.Parameters.Add("@VehicleID", ddlSelectVehicle.SelectedValue.Trim());
                cmd.Parameters.Add("@Geofencename", ddlSelectGeofence.SelectedValue.Trim());
                cmd.Parameters.Add("@GeofenceType", ddlGeofenceType.SelectedValue.Trim());
                cmd.Parameters.Add("@DateOfAssign", fromdate);
                if (txtDateofUnAssign.Text != "")
                {
                    cmd.Parameters.Add("@DateofUnAssign", todate);
                }
                DataRow[] drAssignGeofence = GeofenceDataTable.Select("Name='" + ddlSelectGeofence.SelectedValue + "'");
                cmd.Parameters.Add("@Latitude", drAssignGeofence[0]["Latitude"].ToString());
                cmd.Parameters.Add("@Longitude", drAssignGeofence[0]["Longitude"].ToString());
                cmd.Parameters.Add("@Radius", drAssignGeofence[0]["Radious"].ToString());
                cmd.Parameters.Add("@UserName", UserName);
                vdm.insert(cmd);
                MessageBox.Show("Data Added Successfully..", this);
                UpdateAssignGeofence();
                RefreshAssignGeofence();
            }
            else
            {
                if (txtDateofUnAssign.Text != "")
                {
                    cmd = new MySqlCommand("Update AssignGeofence set VehicleID=@VehicleID,Geofencename=@Geofencename,GeofenceType=@GeofenceType,DateOfAssign=@DateOfAssign,DateofUnAssign=@DateofUnAssign,Latitude=@Latitude,Longitude=@Longitude,Radius=@Radius where SlNo=@SlNo and UserName=@UserName");
                }
                else
                {
                    cmd = new MySqlCommand("Update AssignGeofence set VehicleID=@VehicleID,Geofencename=@Geofencename,GeofenceType=@GeofenceType,DateOfAssign=@DateOfAssign,Latitude=@Latitude,Longitude=@Longitude,Radius=@Radius where SlNo=@SlNo and UserName=@UserName");

                }
                DateTime fromdate = DateTime.Now;//System.Convert.ToDateTime(startdate.Text);//startdate_CalendarExtender.SelectedDate ?? DateTime.Now;// DateTime.Now.AddMonths(-3);//DateTime.Parse(startdate.Text); ;
                DateTime todate = DateTime.Now;//System.Convert.ToDateTime(enddate.Text);//enddate_CalendarExtender.SelectedDate ?? DateTime.Now; //DateTime.Parse(enddate.Text);
                // d/M/yyyy HH:mm
                string[] datestrig = txtDateofAssign.Text.Split(' ');

                if (datestrig.Length > 1)
                {
                    if (datestrig[0].Split('/').Length > 0)
                    {
                        string[] dates = datestrig[0].Split('/');
                        string[] times = datestrig[1].Split(':');
                        fromdate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                    }
                }
                else
                {
                    // MessageBox.Show("Date Time Format Wrong");
                    lbl_nofifier.Text = "From Date Time Format Wrong";
                    return;
                }

                datestrig = txtDateofUnAssign.Text.Split(' ');
                if (datestrig.Length > 1)
                {
                    if (datestrig[0].Split('/').Length > 0)
                    {
                        string[] dates = datestrig[0].Split('/');
                        string[] times = datestrig[1].Split(':');
                        todate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                    }
                }
                else
                {
                    // MessageBox.Show("Date Time Format Wrong");
                    lbl_nofifier.Text = "To Date Time Format Wrong";
                    return;
                }

                cmd.Parameters.Add("@VehicleID", ddlSelectVehicle.Text.Trim());
                cmd.Parameters.Add("@Geofencename", ddlSelectGeofence.Text.Trim());
                cmd.Parameters.Add("@GeofenceType", ddlGeofenceType.Text.Trim());
                cmd.Parameters.Add("@DateOfAssign", fromdate);
                cmd.Parameters.Add("@DateofUnAssign", todate);

                DataRow[] drAssignGeofence = GeofenceDataTable.Select("Name='" + ddlSelectGeofence.SelectedValue + "'");
                cmd.Parameters.Add("@Latitude", drAssignGeofence[0]["Latitude"].ToString());
                cmd.Parameters.Add("@Longitude", drAssignGeofence[0]["Longitude"].ToString());
                cmd.Parameters.Add("@Radius", drAssignGeofence[0]["Radious"].ToString());
                cmd.Parameters.Add("@UserName", UserName);
                cmd.Parameters.Add("@SlNo", SlNo);
                vdm.Update(cmd);
                BtnAddAssignGeofence.Text = "Save";
                MessageBox.Show("Successfully Data Modified", this);
                UpdateAssignGeofence();
                RefreshAssignGeofence();
            }
        }
        catch (Exception ex)
        {
            if (ex.Message == "-2146232060")
                MessageBox.Show(" is already Existed", this);
            else
                MessageBox.Show(ex.Message, this);
        }
    }
    protected void btnDeleteAssignGeofence_Click(object sender, EventArgs e)
    {
        if (GrdAssignGeofence.SelectedIndex > -1)
        {
            try
            {
                cmd = new MySqlCommand("delete AssignGeofence where SlNo=@SlNo and UserName=@UserName");
                cmd.Parameters.Add("@SlNo", SlNo);
                cmd.Parameters.Add("@UserName", UserName);
                vdm.Delete(cmd);
                UpdateAssignGeofence();
                RefreshAssignGeofence();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this);
            }
        }
        else
        {
            MessageBox.Show("No row selected to delete Driver Data", this);
        }
    }
    protected void btnRefreshAssignGeofence_Click(object sender, EventArgs e)
    {
        RefreshAssignGeofence();
    }
    void RefreshAssignGeofence()
    {
        ddlSelectVehicle.ClearSelection();
        ddlSelectGeofence.ClearSelection();
        ddlGeofenceType.ClearSelection();
        txtDateofAssign.Text = "";
        txtDateofUnAssign.Text = "";
    }
    protected void GrdAssignGeofence_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GrdAssignGeofence.PageIndex = e.NewPageIndex;
        GrdAssignGeofence.DataBind();
    }
    protected void GrdAssignGeofence_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (GrdAssignGeofence.SelectedIndex > -1)
        {
            try
            {
                GridViewRow gvr = GrdAssignGeofence.SelectedRow;

                BtnAddAssignGeofence.Text = "Edit";
                ddlSelectVehicle.SelectedValue = gvr.Cells[1].Text;
                ddlSelectGeofence.SelectedValue = gvr.Cells[2].Text;
                ddlGeofenceType.SelectedValue = gvr.Cells[3].Text;
                txtDateofAssign.Text = gvr.Cells[4].Text;
                txtDateofUnAssign.Text = gvr.Cells[5].Text;
                SlNo = gvr.Cells[6].Text;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this);
            }
        }
    }
}