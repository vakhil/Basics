using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using MySql.Data.MySqlClient;
using System.Net.Mail;
using System.IO;
using System.Web.Security;
using System.Net;
    

public partial class MasterPage : System.Web.UI.MasterPage
{
    MySqlCommand cmd;
    string UserName = "";
    int zoomlevel = 4;
    VehicleDBMgr vdm;
    DataDownloader ddwnldr;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["field1"] == null)
            Response.Redirect("Login.aspx");
        else
        {
            //vdm.UserName = Session["field1"].ToString();
            UserName = Session["field1"].ToString();
            vdm = new VehicleDBMgr();
            vdm.InitializeDB();
            if (!this.IsPostBack)
            {
                if (!Page.IsCallback)
                {
                    try
                    {
                        lblMessage.Text = "Welcome : " + UserName;
                    }
                    catch
                    {
                    }
                }
            }
        }
    }

}
