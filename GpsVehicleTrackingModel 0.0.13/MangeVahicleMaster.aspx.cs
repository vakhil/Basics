using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;

public partial class MangeVahicleMaster : System.Web.UI.Page
{
    MySqlCommand cmd;
    static VehicleDBMgr vdm;
    static string UserName = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["field1"] == null)
            Response.Redirect("Login.aspx");
        if (!Page.IsCallback)
        {
            if (!Page.IsPostBack)
            {
                vdm = new VehicleDBMgr();
                vdm.InitializeDB();
                UserName = Session["field1"].ToString();
                UpdateVehicleManage();
            }
        }
    }

    void UpdateVehicleManage()
    {
        try
        {
            cmd = new MySqlCommand("select * from vehiclemanage where UserName=@UserName");
            cmd.Parameters.Add("@UserName", UserName);
            DataTable dt = vdm.SelectQuery(cmd).Tables[0];
            grdVehicleManage.DataSource = dt;
            grdVehicleManage.DataBind();
            Session["xportdata"] = dt;
            Session["title"] = "Vehicle Data Manage";
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, this);
        }
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            if (btnAdd.Text == "Add")
            {
                cmd = new MySqlCommand("insert into vehiclemanage  (ItemType, ItemName,ItemCode,UserName) values (@ItemType, @ItemName,@ItemCode,@UserName)");
                cmd.Parameters.Add("@ItemType", ddlItemType.SelectedValue);
                cmd.Parameters.Add("@ItemName", txtItemName.Text.Trim());
                cmd.Parameters.Add("@ItemCode", txtItemCode.Text.Trim());
                cmd.Parameters.Add("@UserName", UserName);
                vdm.insert(cmd);
                Refresh();
                UpdateVehicleManage();
                lblSuccess.Text = "Record added Successfully";
            }
            else
            {
                cmd = new MySqlCommand("update vehiclemanage set ItemType=@ItemType,ItemName=@ItemName, ItemCode=@ItemCode where UserName=UserName and SNo=@Sno");
                cmd.Parameters.Add("@ItemType", ddlItemType.SelectedValue);
                cmd.Parameters.Add("@ItemName", txtItemName.Text.Trim());
                cmd.Parameters.Add("@ItemCode", txtItemCode.Text.Trim());
                cmd.Parameters.Add("@UserName", UserName);
                cmd.Parameters.Add("@Sno", sno);
                vdm.Update(cmd);
                Refresh();
                UpdateVehicleManage();
                btnAdd.Text = "Add";
                lblSuccess.Text = "Record Updated Successfully";
            }
        }
        catch
        {
            lblSuccess.Text = "Error,Please try again";
        }
    }
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        Refresh();
    }
    void Refresh()
    {
        lblSuccess.Text = "";
        ddlItemType.ClearSelection();
        txtItemName.Text = "";
        txtItemCode.Text = "";
        btnAdd.Text = "Add";
    }
    static string sno;
    protected void grdVehicleManage_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (grdVehicleManage.SelectedIndex >= -1)
        {
            GridViewRow selectedrw = grdVehicleManage.SelectedRow;
            ddlItemType.SelectedValue = selectedrw.Cells[1].Text;
            txtItemCode.Text = selectedrw.Cells[4].Text;
            txtItemName.Text = selectedrw.Cells[2].Text;
            sno = selectedrw.Cells[5].Text;
            btnDelete.Enabled = true;
            lblSuccess.Text = "";
            btnAdd.Text = "Edit";
        }
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        if (grdVehicleManage.SelectedIndex > -1)
        {
            cmd = new MySqlCommand("delete from vehiclemanage where  UserName=@UserName and SNo=@SNo");
            cmd.Parameters.Add("@UserName", UserName);
            cmd.Parameters.Add("@SNo", sno);
            vdm.Delete(cmd);
            Refresh();
            UpdateVehicleManage();
            lblSuccess.Text = "Record Deleted Successfully";
            grdVehicleManage.SelectedIndex = -1;
        }
    }
}