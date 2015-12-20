using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;
using System.IO;

public partial class Mylocation : System.Web.UI.Page
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
        }
    }
 
    //protected void btn_MyLocation_Del_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (grdMylocation.SelectedIndex > -1)
    //        {
    //            cmd = new MySqlCommand("delete from BranchData where Sno=@Sno and UserName= @UserName ");
    //            cmd.Parameters.Add("@UserName", UserName);
    //            cmd.Parameters.Add("@Sno", MyLocationNameSno);
    //            vdm.Delete(cmd);
    //            UpdateMyLocationUI();
    //            ResetMyLocationUI();
    //            lblMylocmsg.Text = "Deleted successfully";

    //        }
    //        else
    //        {
    //            MessageBox.Show("No row selected to delete the Location", this);
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        MessageBox.Show(ex.Message, this);
    //    }
    //}
    
}