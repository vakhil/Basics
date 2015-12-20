using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;

public partial class RouteScheduler : System.Web.UI.Page
{
    VehicleDBMgr vdm;
    DataDownloader ddwnldr;
    MySqlCommand cmd;
    string UserName;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["field1"] == null)
            Response.Redirect("Login.aspx");
        else
        {
            UserName = Session["field1"].ToString();
            vdm = new VehicleDBMgr();
            vdm.InitializeDB();
            if (!this.IsPostBack)
            {
                if (!Page.IsCallback)
                {
                    ddwnldr = new DataDownloader();
                    ddwnldr.UpdateBranchDetails(UserName);
                    FillRoute();
                }
            }
        }
    }
    public void FillRoute()
    {
        cmd = new MySqlCommand("Select RouteName from Routetable where UserID=@UserID");
        cmd.Parameters.Add("@UserID", UserName);
        DataTable dtRoute = vdm.SelectQuery(cmd).Tables[0];
        //if (ddlRouteName.SelectedIndex == -1)
        //{
        //    ddlRouteName.Items.Add("Select Route Name");
        //}
        foreach (DataRow dr in dtRoute.Rows)
        {
            ddlRouteName.Items.Add(dr["RouteName"].ToString());
        }
    }
    protected void ddlRouteName_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string RouteName = ddlRouteName.SelectedValue;
            cmd = new MySqlCommand("SELECT tripdata.Refno,tripsubdata.locid FROM tripdata INNER JOIN  tripsubdata ON tripdata.Refno = tripsubdata.Refno WHERE (tripdata.RouteName = @RouteName)");
            cmd.Parameters.Add("@RouteName", RouteName);
            DataTable dtsubRoute = vdm.SelectQuery(cmd).Tables[0];
            DataTable finaltable = new DataTable();
            finaltable.Columns.Add("FromBranchName");
            finaltable.Columns.Add("FromBranchID");
            finaltable.Columns.Add("ToBranchName");
            finaltable.Columns.Add("ToBranchID");
            finaltable.Columns.Add("Time");
            List<Branchclass> SubTriplist = new List<Branchclass>();
            foreach (DataRow drSubTrip in dtsubRoute.Rows)
            {
                Branchclass getBranches = new Branchclass();
                cmd = new MySqlCommand("Select BranchID from branchdata where Sno='" + drSubTrip["locid"].ToString() + "'");
                DataTable dtBranchName = vdm.SelectQuery(cmd).Tables[0];
                if (dtBranchName.Rows.Count > 0)
                {
                    string BranchName = dtBranchName.Rows[0]["BranchID"].ToString();
                    getBranches.BranchName = BranchName;
                    getBranches.SNo = drSubTrip["locid"].ToString();
                    SubTriplist.Add(getBranches);
                }
            }
            cmd = new MySqlCommand("Select * from locatondistances where username=@username");
            cmd.Parameters.Add("@username", UserName);
            DataTable dtlocationdistance = vdm.SelectQuery(cmd).Tables[0];
            foreach (Branchclass strA in SubTriplist)
            {
                foreach (Branchclass strB in SubTriplist)
                {
                    DataRow newrow = finaltable.NewRow();
                    newrow["FromBranchName"] = strA.BranchName;
                    newrow["FromBranchID"] = strA.SNo;
                    newrow["ToBranchName"] = strB.BranchName;
                    newrow["ToBranchID"] = strB.SNo;
                    DataRow[] dr = dtlocationdistance.Select("frombranchsno in (" + strA.SNo + ") and tobranchsno in (" + strB.SNo + ")");
                    if (dr.Length > 0)
                    {
                        newrow["Time"] = dr[0]["expectedtime"].ToString();
                    }
                    else
                    {
                        newrow["Time"] =0;
                    }
                    if (strA.BranchName != strB.BranchName)
                    {
                        finaltable.Rows.Add(newrow);
                    }
                }

            }
            GridView1.DataSource = finaltable;
            GridView1.DataBind();
        }
        catch
        {
        }
    }
    public class Branchclass
    {
        public string BranchName { get; set; }
        public string SNo { get; set; }
    }
    protected void btnSave_OnClick(object sender, EventArgs e)
    {
        try
        {
            cmd = new MySqlCommand("SELECT sno, frombranchsno, tobranchsno, kms, expectedtime FROM locatondistances where  username=@username");
            cmd.Parameters.Add("@username", UserName);
            DataTable branchdata = vdm.SelectQuery(cmd).Tables[0];
            if (branchdata.Rows.Count > 0)
            {
                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    string frombranchsno = ((TextBox)GridView1.Rows[i].FindControl("hbnFrombranch")).Text.Trim();
                    string tobranchsno = ((TextBox)GridView1.Rows[i].FindControl("hbnTobranch")).Text.Trim();
                    string txttime = ((TextBox)GridView1.Rows[i].FindControl("txttime")).Text.Trim();
                    DataRow[] distancerow = branchdata.Select("frombranchsno=" + frombranchsno + " and tobranchsno=" + tobranchsno + "");
                    if (distancerow.Length > 0)
                    {
                        cmd = new MySqlCommand("update locatondistances set expectedtime=@expectedtime where frombranchsno=@frombranchsno and tobranchsno=@tobranchsno and username=@username");
                        cmd.Parameters.Add("@frombranchsno", frombranchsno);
                        cmd.Parameters.Add("@tobranchsno", tobranchsno);
                        cmd.Parameters.Add("@expectedtime", txttime);
                        cmd.Parameters.Add("@username", UserName);
                        vdm.Update(cmd);
                    }
                    else
                    {
                        cmd = new MySqlCommand("insert into locatondistances (frombranchsno, tobranchsno,expectedtime,username) values (@frombranchsno, @tobranchsno,@expectedtime,@username)");
                        cmd.Parameters.Add("@frombranchsno", frombranchsno);
                        cmd.Parameters.Add("@tobranchsno", tobranchsno);
                        cmd.Parameters.Add("@expectedtime", txttime);
                        cmd.Parameters.Add("@username", UserName);
                        vdm.insert(cmd);
                    }
                }
                lblmsg.Text = "Data Updated Successfully";
            }
            else
            {
                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    string frombranchsno = ((TextBox)GridView1.Rows[i].FindControl("hbnFrombranch")).Text.Trim();
                    string tobranchsno = ((TextBox)GridView1.Rows[i].FindControl("hbnTobranch")).Text.Trim();
                    string txttime = ((TextBox)GridView1.Rows[i].FindControl("txttime")).Text.Trim();
                    cmd = new MySqlCommand("insert into locatondistances (frombranchsno, tobranchsno,expectedtime,username) values (@frombranchsno, @tobranchsno,@expectedtime,@username)");
                    cmd.Parameters.Add("@frombranchsno", frombranchsno);
                    cmd.Parameters.Add("@tobranchsno", tobranchsno);
                    cmd.Parameters.Add("@expectedtime", txttime);
                    cmd.Parameters.Add("@username", UserName);
                    vdm.insert(cmd);
                }
                lblmsg.Text = "Data Saved Successfully";
            }
        }
        catch
        {
        }
    }
}