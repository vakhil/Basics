using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;

public partial class VehicleGroups : System.Web.UI.Page
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
                    FillSelectVehicle();
                    UpdateVehicleGroupData();
                }
            }
        }

    }
    static string vehicleGroup = "";
    private bool ValidateVehicleGroup()
    {
        if (txtGroupName.Text == "")
        {
            MessageBox.Show("Enter Group Name", this);
           // txt_Radious.Focus();
            return false;
        }
        return true;
    }
    protected void btn_VehicleGroup_Add_Click(object sender, EventArgs e)
    {
        try
        {
            if (!ValidateVehicleGroup())
                return;
            if (cblSelectVehicle.Items.Count > 0)
            {
                if (btn_VehicleGroup_Add.Text == "Add")
                {
                    foreach (ListItem obj in cblSelectVehicle.Items)
                    {
                        if (obj.Selected)
                        {

                            cmd = new MySqlCommand("insert into VehicleGroup (GroupName,VehicleID,UserName) values (@GroupName,@VehicleID,@UserName) ");
                            cmd.Parameters.Add("@GroupName", txtGroupName.Text.Trim());
                            cmd.Parameters.Add("@VehicleID", obj.Text.Trim());
                            cmd.Parameters.Add("@UserName", UserName);
                            vdm.insert(cmd);
                        }
                    }

                    UpdateVehicleGroupData();
                    ResetVehicleGroup();
                    lblGroupStatus.Text = "Record Added Successfully";
                }


                else
                {
                    cmd = new MySqlCommand("Delete from VehicleGroup where GroupName=@GroupName and UserName=@UN");
                    cmd.Parameters.Add("@GroupName", vehicleGroup);
                    cmd.Parameters.Add("@UN", UserName);
                    vdm.Delete(cmd);

                    foreach (ListItem obj in cblSelectVehicle.Items)
                    {
                        if (obj.Selected)
                        {
                            cmd = new MySqlCommand("insert into VehicleGroup (GroupName,VehicleID,UserName) values (@GroupName,@VehicleID,@UserName) ");
                            cmd.Parameters.Add("@GroupName", txtGroupName.Text);
                            cmd.Parameters.Add("@VehicleID", obj.Text);
                            cmd.Parameters.Add("@UserName", UserName);
                        }
                    }
                    vdm.insert(cmd);
                    btn_VehicleGroup_Add.Text = "Add";
                    MessageBox.Show("Successfully Groups Modified", this);
                    UpdateVehicleGroupData();
                    ResetVehicleGroup();
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    static DataTable fleetVehiceData;
    public void FillSelectVehicle()
    {
        try
        {

            cmd = new MySqlCommand("select * from ManageData where UserName=@UserName");
            cmd.Parameters.Add("@UserName", UserName);
            fleetVehiceData = vdm.SelectQuery(cmd).Tables[0];
            cblSelectVehicle.Items.Clear();
            foreach (DataRow dr in fleetVehiceData.Rows)
            {
                cblSelectVehicle.Items.Add(dr["VehicleID"].ToString());
                //ddl_ML_VehicleNo.Items.Add(dr["VehicleID"].ToString());
            }

        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, this);
        }
    }
   static DataTable VehicleGroup;
    public  void UpdateVehicleGroupData()
    {
        cmd = new MySqlCommand("select * from VehicleGroup where UserName=@UserName");
        cmd.Parameters.Add("@UserName", UserName);
        VehicleGroup = vdm.SelectQuery(cmd).Tables[0];
        grdVehicleGroup.DataSource = VehicleGroup;
        grdVehicleGroup.DataBind();
    }
    protected void btn_VehicleGroup_Del_Click(object sender, EventArgs e)
    {
        cmd = new MySqlCommand("Delete from VehicleGroup where GroupName=@GroupName");
        cmd.Parameters.Add("@GroupName", txtGroupName.Text);
        vdm.Delete(cmd);
        MessageBox.Show("Deleted successfully", this);
        UpdateVehicleGroupData();
        ResetVehicleGroup();
    }
    protected void btn_VehicleGroup_Refresh_Click(object sender, EventArgs e)
    {
        ResetVehicleGroup();
    }
    public void ResetVehicleGroup()
    {
        txtGroupName.Text = "";
        cblSelectVehicle.ClearSelection();
        btn_VehicleGroup_Add.Text = "Add";
        btn_VehicleGroup_Del.Enabled = false;
        lblGroupStatus.Text = "";
    }

    protected void grdVehicleGroup_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (grdVehicleGroup.SelectedIndex >= -1)
        {
            GridViewRow selectedrw = grdVehicleGroup.SelectedRow;
            txtGroupName.Text = selectedrw.Cells[1].Text;
            vehicleGroup = selectedrw.Cells[1].Text;
            cblSelectVehicle.Text = selectedrw.Cells[2].Text;
            btn_VehicleGroup_Add.Text = "Edit";
            btn_VehicleGroup_Del.Enabled = true;
        }
    }
}