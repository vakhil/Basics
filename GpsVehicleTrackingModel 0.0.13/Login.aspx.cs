using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;

public partial class Login1 : System.Web.UI.Page
{
    MySqlCommand cmd;
    VehicleDBMgr vdm;

    protected void Page_Load(object sender, EventArgs e)
    {
        vdm = new VehicleDBMgr();

    }

    protected void btnReset_Click(object sender, ImageClickEventArgs e)
    {
        txtUserName.Text = "";
        txtPassword.Text = "";
    }
    private DateTime GetLowDate(DateTime dt)
    {
        double Hour, Min, Sec;
        DateTime DT = DateTime.Now;
        DT = dt;
        Hour = -dt.Hour;
        Min = -dt.Minute;
        Sec = -dt.Second;
        DT = DT.AddHours(Hour);
        DT = DT.AddMinutes(Min);
        DT = DT.AddSeconds(Sec);
        return DT;

    }

    private DateTime GetHighDate(DateTime dt)
    {
        double Hour, Min, Sec;
        DateTime DT = DateTime.Now;
        Hour = 23 - dt.Hour;
        Min = 59 - dt.Minute;
        Sec = 59 - dt.Second;
        DT = dt;
        DT = DT.AddHours(Hour);
        DT = DT.AddMinutes(Min);
        DT = DT.AddSeconds(Sec);
        return DT;
    }
    protected void btnLogIn_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Session["allvehicles"] = null;
            vdm.InitializeDB();

            String UN = "";
            String UserName = txtUserName.Text, PassWord = txtPassword.Text;
            cmd = new MySqlCommand("SELECT refno, main_user, loginid as UserName, pwd, usertype FROM loginstable WHERE (loginid = @UN) and (pwd=@Pwd)");
            cmd.Parameters.Add("@UN", UserName);
            cmd.Parameters.Add("@Pwd", PassWord);
            DataTable dt = vdm.SelectQuery(cmd).Tables[0];//"ManageData", "UserName", new string[] { "UserName=@UserName" }, new string[] { UserName }, new string[] { "" }).Tables[0];
            if (dt.Rows.Count > 0)
            {
                Session["field1"] = dt.Rows[0]["UserName"].ToString();
                Session["field2"] = true;
                Session["UserType"] = dt.Rows[0]["usertype"].ToString();
                Session["field3"] = dt.Rows[0]["refno"].ToString();

                Response.Redirect("Default.aspx", false);
            }
            else
            {
                //MessageBox.Show("Please enter Correct User ID",this);
                MessageBox.Show("Please enter Correct User ID and Password", this);
            }
        }
        catch (Exception ex)
        {
            lbl_validation.Text = ex.ToString();
        }
    }

}