using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;

public partial class Expensesmanage : System.Web.UI.Page
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
            }
        }
    }


    protected void btnAdd_Click(object sender, EventArgs e)
    {
        string ItemType = "Expenses";
        cmd = new MySqlCommand("insert into vehiclemanage  (ItemType, ItemName,ItemCode,UserName) values (@ItemType, @ItemName,@ItemCode,@UserName)");
        cmd.Parameters.Add("@ItemType", ItemType);
        cmd.Parameters.Add("@ItemCode", txtPartNo.Text.Trim());
        cmd.Parameters.Add("@ItemName", txtPartName.Text.Trim());
        cmd.Parameters.Add("@UserName", UserName);
        vdm.insert(cmd);
        Refresh();
        lblSuccess.Text = "Record added Successfully";

    }
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        Refresh();
    }
    void Refresh()
    {
        lblSuccess.Text = "";
        txtPartName.Text = "";
        txtPartNo.Text = "";
    }
}