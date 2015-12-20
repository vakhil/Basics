using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;

public partial class Deisel : System.Web.UI.Page
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
                FillVehicleNo();
                FillPartName();
                UpdateDeisel();
            }
        }
    }
    public void FillVehicleNo()
    {
        try
        {
            ddlVehicleNo.Items.Clear();

            cmd = new MySqlCommand("select * from PairedData where UserID=@UserName");
            cmd.Parameters.Add("@UserName", UserName);
            if (ddlVehicleNo.SelectedIndex == -1)
            {
                ddlVehicleNo.Items.Add("Select Vehicle No");
            }
            DataTable dt = vdm.SelectQuery(cmd).Tables[0];
            foreach (DataRow dr in dt.Rows)
            {

                ddlVehicleNo.Items.Add(dr["VehicleNumber"].ToString());
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, this);
        }
    }
    public static DataTable dtPartName = new DataTable();
    public void FillPartName()
    {
        try
        {
            ddlPartName.Items.Clear();
            string itemtype = "Expenses";
            cmd = new MySqlCommand("select * from vehiclemanage where UserName=@UserName and ItemType='" + itemtype + "'");
            cmd.Parameters.Add("@UserName", UserName);
            if (ddlPartName.SelectedIndex == -1)
            {
                ddlPartName.Items.Add("Select Part Name");
                txtPartNo.Text = "";
            }
            dtPartName = vdm.SelectQuery(cmd).Tables[0];
            foreach (DataRow dr in dtPartName.Rows)
            {

                ddlPartName.Items.Add(dr["ItemName"].ToString());
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, this);
        }
    }
    protected void ddlPartName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlPartName.SelectedIndex == 0)
        {
            txtPartNo.Text = "";
        }
        foreach (DataRow dr in dtPartName.Select("ItemName='" + ddlPartName.SelectedValue + "'"))
        {
            txtPartNo.Text = dr["Itemcode"].ToString();
        }
    }
    void UpdateDeisel()
    {
        try
        {
            cmd = new MySqlCommand("select VehicleNo,PartNo,PartName,Qty,UnitPrice,TotalAmount, Datetime,Sno from inventory where UserName=@UserName ");
            cmd.Parameters.Add("@UserName", UserName);
            DataTable dt = vdm.SelectQuery(cmd).Tables[0];
            grdVehicleExpences.DataSource = dt;
            grdVehicleExpences.DataBind();

        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, this);
        }
    }
    void Refresh()
    {
        ddlVehicleNo.ClearSelection();
        txtPartNo.Text = "";
        ddlPartName.ClearSelection();
        txtQty.Text = "";
        txtUnitPrice.Text = "";
        txtTotalAmount.Text = "";
        txtDate.Text = "";
        lblStatus.Text = "";
    }
    static string Sno = "";
    protected void btn_DeiselVal_Add_Click(object sender, EventArgs e)
    {
        try
        {
            DateTime fromdate = DateTime.Now;
            string[] datestrig = txtDate.Text.Split(' ');

            if (datestrig.Length > 1)
            {
                if (datestrig[0].Split('/').Length > 0)
                {
                    string[] dates = datestrig[0].Split('/');
                    string[] times = datestrig[1].Split(':');
                    fromdate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                }
            }
            if (btn_DeiselVal_Add.Text == "Add")
            {
                cmd = new MySqlCommand("insert into inventory  (VehicleNo,PartNo,PartName,Qty,UnitPrice,TotalAmount,Datetime,UserName)values (@VehicleNo,@PartNo,@PartName,@Qty,@UnitPrice,@TotalAmount,@Datetime,@UserName)");
                cmd.Parameters.Add("@VehicleNo", ddlVehicleNo.SelectedValue);
                cmd.Parameters.Add("@PartNo", txtPartNo.Text.Trim());
                cmd.Parameters.Add("@PartName", ddlPartName.SelectedValue);
                cmd.Parameters.Add("@Qty", txtQty.Text.Trim());
                cmd.Parameters.Add("@UnitPrice", txtUnitPrice.Text.Trim());
                cmd.Parameters.Add("@TotalAmount", txtTotalAmount.Text.Trim());
                //   cmd.Parameters.Add("@Datetime", txtDate.Text.Trim());
                cmd.Parameters.Add("@Datetime", fromdate);
                cmd.Parameters.Add("@UserName", UserName);
                vdm.insert(cmd);
                Refresh();
                UpdateDeisel();
                lblStatus.Text = "Record added Successfully";
            }
            else
            {
                cmd = new MySqlCommand("update inventory set  VehicleNo=@VehicleNo,PartNo=@PartNo,PartName=@PartName,Qty=@Qty,UnitPrice=@UnitPrice,TotalAmount=@TotalAmount,Datetime=@Datetime where UserName=UserName and Sno=@Sno");
                cmd.Parameters.Add("@VehicleNo", ddlVehicleNo.SelectedValue);
                cmd.Parameters.Add("@PartNo", txtPartNo.Text.Trim());
                cmd.Parameters.Add("@PartName", ddlPartName.SelectedValue);
                cmd.Parameters.Add("@Qty", txtQty.Text.Trim());
                cmd.Parameters.Add("@UnitPrice", txtUnitPrice.Text.Trim());
                cmd.Parameters.Add("@TotalAmount", txtTotalAmount.Text.Trim());
                cmd.Parameters.Add("@Datetime", fromdate);
                cmd.Parameters.Add("@UserName", UserName);
                cmd.Parameters.Add("@Sno", Sno);
                vdm.Update(cmd);
                Refresh();
                UpdateDeisel();
                btn_DeiselVal_Add.Text = "Add";
                lblStatus.Text = "Record Updated Successfully";
            }
        }
        catch
        {
        }
    }
    protected void btn_DeiselVal_Del_Click(object sender, EventArgs e)
    {
        try
        {
            if (grdVehicleExpences.SelectedIndex > -1)
            {
                cmd = new MySqlCommand("delete from inventory where  UserName=@UserName and Sno=@Sno");
                cmd.Parameters.Add("@UserName", UserName);
                cmd.Parameters.Add("@Sno", Sno);
                vdm.Delete(cmd);
                Refresh();
                UpdateDeisel();
                btn_DeiselVal_Add.Text = "Add";
                lblStatus.Text = "Record Deleted Successfully";
            }
        }
        catch
        {
        }
    }
    protected void btn_DeiselVal_Refresh_Click(object sender, EventArgs e)
    {
        Refresh();
    }
    protected void grdDeiselValue_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (grdVehicleExpences.SelectedIndex >= -1)
            {
                GridViewRow selectedrw = grdVehicleExpences.SelectedRow;
                ddlVehicleNo.SelectedValue = selectedrw.Cells[1].Text;
              txtPartNo.Text = selectedrw.Cells[2].Text;
              ddlPartName.SelectedValue = selectedrw.Cells[3].Text;
              txtQty.Text = selectedrw.Cells[4].Text;
              txtUnitPrice.Text = selectedrw.Cells[5].Text;
              txtTotalAmount.Text = selectedrw.Cells[6].Text;
              txtDate.Text = selectedrw.Cells[7].Text;
                Sno = selectedrw.Cells[8].Text;
                btn_DeiselVal_Add.Text = "Edit";
                btn_DeiselVal_Del.Enabled = true;
            }
        }
        catch
        {
        }
    }
}