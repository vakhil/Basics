using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;

public partial class Geofence : System.Web.UI.Page
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
                    getGeofenseData();
                }
            }
        }

    }
    static string geofenseName = "";
    void GoogleMapForASPNet1_Geofence_PushpinMoved(string pID)
    {
        ////GooglePoint gp = GoogleMapForASPNet2.GoogleMapObject.Points[pID];
        ////txt_GeoFence_latitude.Text = gp.Latitude.ToString();
        ////txt_Geofence_longitude.Text = gp.Longitude.ToString();
    }

    static DataTable geofensedata;
    void getGeofenseData()
    {
        cmd = new MySqlCommand("select Name,Radious,Latitude,Longitude from GeofenseTable where UserName=@UserName");
        cmd.Parameters.Add("@UserName", UserName);

        geofensedata = vdm.SelectQuery(cmd).Tables[0];
        grdGeofence.DataSource = geofensedata;
        grdGeofence.DataBind();
    }


    private void resetall()
    {
        txt_Geofencename.Text = "";
        txt_GeoFence_latitude.Text = "";
        txt_Geofence_longitude.Text = "";
        txt_Radious.Text = "";
        btnGeofenceDelete.Enabled = false;
        BtnSave.Text = "Save";
    }
    protected void grdGeofence_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (grdGeofence.SelectedIndex >= -1)
        {
            GridViewRow selectedrw = grdGeofence.SelectedRow;
            txt_Geofencename.Text = selectedrw.Cells[1].Text;
            geofenseName = selectedrw.Cells[1].Text;
            txt_GeoFence_latitude.Text = selectedrw.Cells[3].Text;
            txt_Geofence_longitude.Text = selectedrw.Cells[4].Text;
            txt_Radious.Text = selectedrw.Cells[2].Text;
            btnGeofenceDelete.Enabled = true;
            BtnSave.Text = "Edit";
            ////GoogleMapForASPNet2.GoogleMapObject.Points.Remove("Pointer");
            //GooglePoint GP1 = new GooglePoint();
            //GP1.ID = "Pointer";
            //GP1.Latitude = double.Parse(selectedrw.Cells[3].Text);
            //GP1.Longitude = double.Parse(selectedrw.Cells[4].Text);
            //GP1.InfoHTML = "This is Location Point";
            //GP1.Draggable = true;
            ////GoogleMapForASPNet2.GoogleMapObject.Points.Add(GP1);
            ScriptManager.RegisterStartupScript(Page, GetType(), "JsStatus", "PointsAdd();", true);
        }
    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        if (txt_Geofencename.Text != "" && txt_GeoFence_latitude.Text != "" && txt_Geofence_longitude.Text != "" && txt_Radious.Text != "")
        {
            try
            {
                if (BtnSave.Text == "Save")
                {
                    cmd = new MySqlCommand("insert into GeofenseTable (Name,Latitude,Longitude,Radious,UserName) values (@Name,@Latitude,@Longitude,@Radious,@UserName)");
                    cmd.Parameters.Add("@Name", txt_Geofencename.Text.Trim());
                    cmd.Parameters.Add("@Latitude", txt_GeoFence_latitude.Text.Trim());
                    cmd.Parameters.Add("@Longitude", txt_Geofence_longitude.Text.Trim());
                    cmd.Parameters.Add("@Radious", txt_Radious.Text.Trim());
                    cmd.Parameters.Add("@UserName", UserName);
                    vdm.insert(cmd);
                    MessageBox.Show("Successfully Geofence added", this);
                    getGeofenseData();
                    resetall();
                }
                else
                {
                    cmd = new MySqlCommand("update GeofenseTable set  Name=@Name,Latitude=@Latitude,Longitude=@Longitude,Radious=@Radious where UserName=@UserName and Name=@Nme");
                    cmd.Parameters.Add("@Name", txt_Geofencename.Text);
                    cmd.Parameters.Add("@Latitude", txt_GeoFence_latitude.Text);
                    cmd.Parameters.Add("@Longitude", txt_Geofence_longitude.Text);
                    cmd.Parameters.Add("@Radious", txt_Radious.Text);
                    cmd.Parameters.Add("@Nme", geofenseName);
                    cmd.Parameters.Add("@UserName", UserName);
                    vdm.Update(cmd);
                    MessageBox.Show("Successfully Geofence Modified", this);
                    getGeofenseData();

                    resetall();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this);
                //  Response.Write(@"<script language='javascript'>alert('"+ex.Message+"')</script>");
            }
        }
        else
        {
            //   Response.Write(@"<script language='javascript'>alert('Please fill all required filelds')</script>");
            MessageBox.Show("Please fill all required filelds", this);
        }
    }
    protected void btnGeofenceDelete_Click(object sender, EventArgs e)
    {
        try
        {
            if (grdGeofence.SelectedIndex > -1)
            {
                cmd = new MySqlCommand("delete from GeofenseTable where  UserName=@UserName and Name=@Nme");
                cmd.Parameters.Add("@Nme", txt_Geofencename.Text);
                cmd.Parameters.Add("@UserName", UserName);
                vdm.Delete(cmd);
                MessageBox.Show("Deleted successfully", this);
                lblstatus.Text = "Deleted successfully";
                getGeofenseData();
                resetall();
            }
            else
            {
                MessageBox.Show("No row selected to delete the Location", this);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, this);
        }
    }
    protected void btnGeofenceRefresh_Click(object sender, EventArgs e)
    {
        resetall();
    }
}