using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

using MySql.Data.MySqlClient;
using System.Data;
public partial class ChangePassWord : System.Web.UI.Page
{
    MySqlCommand cmd;
   static string UserName = "";
   static VehicleDBMgr vdm;
   protected void Page_Load(object sender, EventArgs e)
   {
   }
    protected void btnSubmitt_Click(object sender, EventArgs e)
    {
        try
        {
            if (Session["field1"] != null)
            {
                lblError.Text = "";
                UserName = Session["field1"].ToString();
                vdm = new VehicleDBMgr();
                vdm.InitializeDB();
                cmd = new MySqlCommand("SELECT pwd FROM loginstable WHERE loginid = @UN");
                cmd.Parameters.Add("@UN", UserName);
                DataTable dt = vdm.SelectQuery(cmd).Tables[0];//"ManageData", "UserName", new string[] { "UserName=@UserName" }, new string[] { UserName }, new string[] { "" }).Tables[0];
                vdm = null;
                if (dt.Rows.Count > 0)
                {
                    string password = dt.Rows[0]["pwd"].ToString();
                    if (password == txtOldPassWord.Text)
                    {
                        if (txtNewPassWord.Text == txtConformPassWord.Text)
                        {
                            txtNewPassWord.Text = txtConformPassWord.Text;
                            vdm = new VehicleDBMgr();
                            vdm.InitializeDB();
                            cmd = new MySqlCommand("Update loginstable set pwd=@pwd where loginid=@loginid ");
                            cmd.Parameters.Add("@loginid", UserName);
                            cmd.Parameters.Add("@pwd", txtNewPassWord.Text.Trim());
                            vdm.Update(cmd);
                            vdm = null;

                            lblMessage.Text = "Your Password has been Changed successfully";
                        }
                        else
                        {
                            lblError.Text = "Conform password not match";
                        }
                    }
                    else
                    {
                        lblError.Text = "Entered password is incorrect";
                    }
                }
                else
                {
                    lblError.Text = "Entered username is incorrect";
                }
            }
            else
            {
                Response.Redirect("Login.aspx");
            }
        }
        catch (Exception ex)
        {
            lblError.Text = "Password Changed Failed";
        }
    }
}