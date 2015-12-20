using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;

public partial class EmailUser : System.Web.UI.Page
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
                 UpdateEmails();
             }
         }

    }
    void UpdateEmails()
    {
        cmd = new MySqlCommand("select PName, EmailAddress,Level as l, '' as Level,sno from emailmgmt where UserID=@UserID");
        cmd.Parameters.Add("@UserID", UserName);
        DataTable dt = vdm.SelectQuery(cmd).Tables[0];
        Session["filiemails"] = dt;
        foreach (DataRow dr in dt.Rows)
        {
            if (dr["l"].ToString() == "1")
            {
                dr["Level"] = "Level1";
            }
            if (dr["l"].ToString() == "2")
            {
                dr["Level"] = "Level2";
            }
            if (dr["l"].ToString() == "3")
            {
                dr["l"] = "Level3";
            }
            if (dr["l"].ToString() == "4")
            {
                dr["Level"] = "Level4";
            }
            if (dr["l"].ToString() == "5")
            {
                dr["Level"] = "Level5";
            }
        }
        grdEmail.DataSource = dt;
        grdEmail.DataBind();
    }
    void Refresh()
    {
        txtName.Text = "";
        txtEmail.Text = "";
        btnSave.Text = "Save";
        ddlLevel.ClearSelection();
    }
    protected void btnSave_OnClick(object sender, EventArgs e)
    {
        try
        {
            if (btnSave.Text == "Save")
            {
                cmd = new MySqlCommand("insert into emailmgmt  (PName , EmailAddress,Level,UserID) values (@PName, @EmailAddress,@Level,@UserID)");
                cmd.Parameters.Add("@PName", txtName.Text);
                cmd.Parameters.Add("@EmailAddress", txtEmail.Text);
                cmd.Parameters.Add("@Level", ddlLevel.Text.Trim());
                cmd.Parameters.Add("@UserID", UserName);
                vdm.insert(cmd);
                Refresh();
                UpdateEmails();
                lblmsg.Text = "Saved Successfully";
            }
            else
            {
                cmd = new MySqlCommand("update emailmgmt set PName=@PName,EmailAddress=@EmailAddress,Level=@Level where sno=@sno and UserID=@UserID");
                cmd.Parameters.Add("@PName", txtName.Text);
                cmd.Parameters.Add("@EmailAddress", txtEmail.Text);
                cmd.Parameters.Add("@Level", ddlLevel.Text.Trim());
                cmd.Parameters.Add("@UserID", UserName);
                cmd.Parameters.Add("@sno", Sno);
                vdm.insert(cmd);
                Refresh();
                UpdateEmails();
                lblmsg.Text = "Updated Successfully";
            }
        }
        catch(Exception ex)
        {
            lblmsg.Text = ex.ToString();
        }
    }

    protected void btnClear_OnClick(object sender, EventArgs e)
    {
        Refresh();
    }
    protected void btnDelete_OnClick(object sender, EventArgs e)
    {
        cmd = new MySqlCommand("delete from emailmgmt  where sno=@sno and UserID=@UserID");
        cmd.Parameters.Add("@UserID", UserName);
        cmd.Parameters.Add("@sno", Sno);
        vdm.Delete(cmd);
        Refresh();
        UpdateEmails();
        lblmsg.Text = "Deleted Successfully";
    }
    public static string Sno = "";
    protected void grdEmail_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (grdEmail.SelectedIndex > -1)
        {

            try
            {
                GridViewRow gvr = grdEmail.SelectedRow;
                txtName.Text = gvr.Cells[1].Text;
                txtEmail.Text = gvr.Cells[2].Text;
                ddlLevel.SelectedValue = gvr.Cells[4].Text;
                Sno = gvr.Cells[5].Text;
                btnSave.Text = "Edit";
                lblmsg.Text = "";
            }
            catch
            {
            }
        }
    }
    protected void grdEmail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[3].Visible = false;
    }
}