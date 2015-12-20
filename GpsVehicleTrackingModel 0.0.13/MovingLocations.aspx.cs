using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;

public partial class MovingLocations : System.Web.UI.Page
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
                    updateML_gridview();
                }
            }
        }

    }
    static string ml_sno = "";
    protected void btn_ML_Add_Click(object sender, EventArgs e)
    {
        if (btn_ML_Add.Text == "Add")
        {
            //if (rb_ML_vehicletype.SelectedItem.Text == "Vehicles")
            //{
            cmd = new MySqlCommand("insert into MovingLoc (VehicleNo,UserName,VehicleType,ConditionType,MaxSpeed,MinSpeed) values (@VehicleNo,@UN,@VT,@CT,@MxS,@MiS)");
            cmd.Parameters.Add("@VehicleNo", ddl_ML_VehicleNo.Text);
            cmd.Parameters.Add("@UN", UserName);
            cmd.Parameters.Add("@VT", rb_ML_vehicletype.SelectedItem.Text);
            cmd.Parameters.Add("@CT", ddl_ml_type.Text);
            cmd.Parameters.Add("@MxS", txt_ml_MinSpeed.Text);
            cmd.Parameters.Add("@MiS", txt_ml_MaxSpeed.Text);
            vdm.insert(cmd);
            updateML_gridview();
            ML_Refresh();
            //}
            //else
            //{

            //}
        }
        else
        {
            cmd = new MySqlCommand("update MovingLoc set VehicleNo=@VehicleNo, VehicleType=@VT, ConditionType=@CT, MaxSpeed=@MxS,MinSpeed=@MiS where sno=@sno ");
            cmd.Parameters.Add("@VehicleNo", ddl_ML_VehicleNo.Text);
            cmd.Parameters.Add("@VT", rb_ML_vehicletype.SelectedItem.Text);
            cmd.Parameters.Add("@CT", ddl_ml_type.Text);
            cmd.Parameters.Add("@MxS", txt_ml_MinSpeed.Text);
            cmd.Parameters.Add("@MiS", txt_ml_MaxSpeed.Text);
            cmd.Parameters.Add("@sno", ml_sno);
            vdm.Update(cmd);
            updateML_gridview();
            ML_Refresh();
        }
     
    }
    protected void btn_ML_Refresh_Click(object sender, EventArgs e)
    {
        ML_Refresh();
    }

    void ML_Refresh()
    {
        rb_ML_vehicletype.Items.FindByValue("Groups").Selected = false;
        rb_ML_vehicletype.Items.FindByValue("Vehicles").Selected = true;
        txt_ml_MaxSpeed.Enabled = false;
        txt_ml_MinSpeed.Enabled = false;
        txt_ml_MinSpeed.Text = "";
        txt_ml_MaxSpeed.Text = "";
        rblUnAuthorisedManagement_SelectedIndexChanged(null, null);
        btn_ML_Add.Text = "Add";
        btn_ML_Delete.Enabled = false;

    }
    protected void gv_ML_Data_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (gv_ML_Data.Rows.Count > 0)
        {
            if (gv_ML_Data.SelectedIndex >= -1)
            {
                GridViewRow dgr = gv_ML_Data.SelectedRow;
               ml_sno = dgr.Cells[1].Text;
               if (dgr.Cells[5].Text == "Groups")
                {
                    rb_ML_vehicletype.Items.FindByValue("Groups").Selected = true;
                    rb_ML_vehicletype.Items.FindByValue("Vehicles").Selected = false;
                }
                else
                {
                    rb_ML_vehicletype.Items.FindByValue("Groups").Selected = false;
                    rb_ML_vehicletype.Items.FindByValue("Vehicles").Selected = true;
                }
                rblUnAuthorisedManagement_SelectedIndexChanged(null, null);
              //  ddl_ML_VehicleNo.Text = dgr.Cells[3].Text;
                ddl_ML_VehicleNo.Text = dgr.Cells[2].Text;
                ddl_ml_type.Text = dgr.Cells[5].Text;//.Items.FindByValue(dgr.Cells[6].Text); //dgr.Cells[6].Text;
                if (dgr.Cells[6].Text == "Moving Loc")
                {
                    txt_ml_MaxSpeed.Enabled = false;
                    txt_ml_MinSpeed.Enabled = false;
                    txt_ml_MinSpeed.Text = "";
                    txt_ml_MaxSpeed.Text = "";

                }
                else
                {
                    txt_ml_MaxSpeed.Enabled = true;
                    txt_ml_MinSpeed.Enabled = true;
                    txt_ml_MinSpeed.Text = dgr.Cells[6].Text;
                    txt_ml_MaxSpeed.Text = dgr.Cells[7].Text;
                }
                btn_ML_Add.Text = "Edit";
                btn_ML_Delete.Enabled = true;
            }
        }
    }

    void updateML_gridview()
    {
        try
        {
            cmd = new MySqlCommand("select sno,VehicleNo,UserName,VehicleType,conditionType,MaxSpeed,MinSpeed from MovingLoc where UserName=@UN");
            cmd.Parameters.Add("@UN", UserName);
            DataTable dt = vdm.SelectQuery(cmd).Tables[0];
            gv_ML_Data.DataSource = dt;
            gv_ML_Data.DataBind();
            int mmm = gv_ML_Data.Columns.Count;
        }
        catch (Exception ex)
        {

        }
    }

    protected void Onclickdelete(object sender, CommandEventArgs e)
    {
        GridViewRow row = gv_ML_Data.Rows[int.Parse(e.CommandArgument.ToString())];
        if (row != null)
        {
            cmd = new MySqlCommand("delete from MovingLoc where VehicleNo=@VN and UserName=@UN");
            cmd.Parameters.Add("@UN", UserName);
            cmd.Parameters.Add("@VN", row.Cells[2].Text);
            vdm.Delete(cmd);
            updateML_gridview();
        }
    }



    protected void gv_ML_Data_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        if (gv_ML_Data.SelectedIndex > -1)
        {
            cmd = new MySqlCommand("delete from MovingLoc where sno=@sno");
            cmd.Parameters.Add("@sno", ml_sno);
            vdm.Delete(cmd);
            updateML_gridview();

        }
    }


    protected void btn_ML_Delete_Click(object sender, EventArgs e)
    {
        if (gv_ML_Data.SelectedIndex > -1)
        {
            GridViewRow gvr = gv_ML_Data.SelectedRow;

            cmd = new MySqlCommand("delete from MovingLoc where VehicleNo=@VN and UserName=@UN and sno=@sno");
            cmd.Parameters.Add("@UN", UserName);
            cmd.Parameters.Add("@VN", ddl_ML_VehicleNo.Text);
            cmd.Parameters.Add("@sno", ml_sno);
            vdm.Delete(cmd);
            updateML_gridview();
            ML_Refresh();
        }
    }

    protected void ddl_ml_type_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddl_ml_type.SelectedValue.ToString() == "Moving Loc")
        {
            txt_ml_MaxSpeed.Enabled = false;
            txt_ml_MinSpeed.Enabled = false;
        }
        else
        {
            txt_ml_MaxSpeed.Enabled = true;
            txt_ml_MinSpeed.Enabled = true;
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
                foreach (DataRow dr in fleetVehiceData.Rows)
                {
                    ddl_ML_VehicleNo.Items.Add(dr["VehicleID"].ToString());
                }
            
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, this);
        }
    }
     static DataTable VehicleGroups;
    public  void UpdateVehicleGroupData()
    {
        cmd = new MySqlCommand("select * from VehicleGroup where UserName=@UserName");
        cmd.Parameters.Add("@UserName", UserName);
        VehicleGroups = vdm.SelectQuery(cmd).Tables[0];
    
    }
    protected void rblUnAuthorisedManagement_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rb_ML_vehicletype.SelectedValue == "Groups")
        {
            ddl_ML_VehicleNo.Items.Clear();
            foreach (DataRow dr in VehicleGroups.DefaultView.ToTable(true, "GroupName").Rows)
            {
                ddl_ML_VehicleNo.Items.Add(dr["GroupName"].ToString());
            }
        }
        else
        {
            ddl_ML_VehicleNo.Items.Clear();
            foreach (DataRow drr in fleetVehiceData.Rows)
            {
                ddl_ML_VehicleNo.Items.Add(drr["VehicleID"].ToString());
            }
        }
    }
}