using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;

public partial class RouteManagement : System.Web.UI.Page
{
    MySqlCommand cmd;
    static VehicleDBMgr vdm;
    static string UserName = "";
    string Locations;

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
                FillLocations();
                UpdateRoute();
            }
        }
    }
    void FillLocations()
    {
        cmd = new MySqlCommand("SELECT Sno,BranchID,Latitude,Longitude FROM branchdata where UserName=@UserName");
            // cmd = new MySqlCommand("SELECT branchdata.BranchID, branchdata.Description, branchdata.Latitude, branchdata.Longitude, branchdata.PhoneNumber, branchdata.ImagePath, branchdata.ImageType, branchdata.Radious, branchdata.UserName FROM loginstable INNER JOIN branchdata ON loginstable.main_user = branchdata.UserName WHERE (loginstable.loginid = @UserName)");
         cmd.Parameters.Add("@UserName", UserName);
            DataTable Branchdata = vdm.SelectQuery(cmd).Tables[0];
            Session["Branchdata"] = Branchdata;
            vdm = null;
            foreach (DataRow dr in Branchdata.Rows)
            {
                ListItem li = new ListItem(dr["BranchID"].ToString(),dr["Sno"].ToString());
                chbllocations.Items.Add(li);
            }
    }
   
    protected void btnRight_OnClick(object sender, EventArgs e)
    {
        foreach (ListItem LV in chbllocations.Items)
        {
            if (LV.Selected == true)
            {
                if (!chblselected.Items.Contains(LV))
                    chblselected.Items.Add(new ListItem(LV.Text,LV.Value));
            }
        }
        MapAdd();
        if (Btnedit == "true")
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "JsStatus", "ChangeButtonText();", true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "JsStatus", "Addtomap();", true);
        }
    }
    protected void btmLeft_OnClick(object sender, EventArgs e)
    {
        List<ListItem> selectedlist = new List<ListItem>();
        foreach (ListItem LV in chblselected.Items)
        {
            if (LV.Selected == true)
            {
                selectedlist.Add(LV);
            }
        }
        foreach (ListItem str in selectedlist)
        {
            chblselected.Items.Remove(str);
            foreach (ListItem chkitem in chblselected.Items)
            {
                //if (chkitem.Text == str)
                //{
                //    chkitem.Selected = false;
                //}
            }
        }
        MapAdd();
        if (Btnedit == "true")
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "JsStatus", "ChangeButtonText();", true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "JsStatus", "Addtomap();", true);
        }
    }
    protected void btnUp_OnClick(object sender, EventArgs e)
    {
        int Index = chblselected.SelectedIndex;
        if (Index > 0)
        {
            string temttext = chblselected.Items[Index - 1].Text;
            string tempvalue = chblselected.Items[Index - 1].Value;
            chblselected.Items[Index - 1].Text = chblselected.Items[Index].Text;
            chblselected.Items[Index - 1].Value = chblselected.Items[Index].Value;
            chblselected.Items[Index].Text = temttext;
            chblselected.Items[Index].Value = tempvalue;
            chblselected.SelectedIndex = Index - 1;
        }
        MapAdd();
        if (Btnedit == "true")
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "JsStatus", "ChangeButtonText();", true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "JsStatus", "Addtomap();", true);
        }
    }

    protected void btnDown_OnClick(object sender, EventArgs e)
    {
        int Index = chblselected.SelectedIndex;
        if (Index < chblselected.Items.Count-1)
        {
            string temptext = chblselected.Items[Index + 1].Text;
            string tempvalue = chblselected.Items[Index + 1].Value;
            chblselected.Items[Index + 1].Text = chblselected.Items[Index].Text;
            chblselected.Items[Index + 1].Value = chblselected.Items[Index].Value;
            chblselected.Items[Index].Text = temptext;
            chblselected.Items[Index].Value = tempvalue;
            chblselected.SelectedIndex = Index + 1;
        }
        MapAdd();
        //string saghdf = chblselected.Items[Index].Text;
        //string value = chblselected.Items[Index].Value;
        if (Btnedit == "true")
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "JsStatus", "ChangeButtonText();", true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "JsStatus", "Addtomap();", true);
        }

    }
    public void UpdateRoute()
    {
        vdm = new VehicleDBMgr();
        vdm.InitializeDB();
        cmd = new MySqlCommand("SELECT RouteName,SNo FROM routetable where UserID=@UserName");
        cmd.Parameters.Add("@UserName", UserName);
        DataTable routetabledata = vdm.SelectQuery(cmd).Tables[0];
        grdRouteData.DataSource = routetabledata;
        grdRouteData.DataBind();
    }
    public void MapAdd()
    {
        foreach (ListItem chkitem in chblselected.Items)
        {
            Locations += chkitem + ",";
        }
        Session["Locations"] = Locations;
    }
    public static string RefSno = "";
    public static string Btnedit = "false";
    protected void grdRouteData_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (grdRouteData.SelectedIndex >= -1)
        {
            GridViewRow selectedrw = grdRouteData.SelectedRow;
            txtRouteName.Text = selectedrw.Cells[1].Text;
            RefSno = selectedrw.Cells[2].Text;
            Session["Sno"] = RefSno;
            vdm = new VehicleDBMgr();
            vdm.InitializeDB();
            cmd = new MySqlCommand("SELECT routesubtable.SNo, routesubtable.Rank, routesubtable.LocationID, branchdata.BranchID AS BranchName FROM routesubtable INNER JOIN branchdata ON routesubtable.LocationID = branchdata.Sno WHERE (routesubtable.SNo = @SNo)");
            //cmd = new MySqlCommand("SELECT LocationID,Rank,BranchName FROM routesubtable where SNo=@SNo order by Rank ");
            cmd.Parameters.Add("@SNo", RefSno);
            DataTable routesubtabledata = vdm.SelectQuery(cmd).Tables[0];
            chblselected.Items.Clear();
            foreach (ListItem LV in chbllocations.Items)
            {
                LV.Selected = false;
            }
            foreach (DataRow dr in routesubtabledata.Rows)
            {
                foreach (ListItem LV in chbllocations.Items)
                {
                    if (LV.Text == dr["BranchName"].ToString())
                    {
                        LV.Selected = true;
                        break;
                    }
                }
            }
            foreach (DataRow LV in routesubtabledata.Rows)
            {
                //if (LV.Selected == true)
                //{
                    //if (!chblselected.Items.Contains(LV))
                chblselected.Items.Add(new ListItem(LV["BranchName"].ToString(), LV["LocationID"].ToString()));
                //}
            }
            MapAdd();
            Btnedit = "true";
            lblmsg.Text = "";
            btnRouteSave.Text = "Edit";
            ScriptManager.RegisterStartupScript(Page, GetType(), "JsStatus", "ChangeButtonText();", true);
        }
    }
    protected void btnDelete_OnClick(object sender, EventArgs e)
    {
        lblmsg.Text = "";
        cmd = new MySqlCommand("delete from Routetable where RouteName=@RouteName and UserID=@UserID");
        cmd.Parameters.Add("@RouteName", txtRouteName.Text);
        cmd.Parameters.Add("@UserID", UserName);
        vdm.Delete(cmd);
        cmd = new MySqlCommand("delete from Routesubtable where SNo=@SNo");
        cmd.Parameters.Add("@SNo", RefSno);
        vdm.Delete(cmd);
        txtRouteName.Text = "";
        lblmsg.Text = "Route Deleted Successfully";
        refresh();
        UpdateRoute();
        ScriptManager.RegisterStartupScript(Page, GetType(), "JsStatus", "ClearPolylines();", true);
    }
    void refresh()
    {
        foreach (ListItem LV in chbllocations.Items)
        {
            LV.Selected = false;
        }
        txtRouteName.Text = "";
        btnRouteSave.Text = "Save";
        chblselected.Items.Clear();
    }
    protected void btnRouteSave_OnClick(object sender, EventArgs e)
    {
        try
        {

            lblmsg.Text = "";
            vdm = new VehicleDBMgr();
            vdm.InitializeDB();
            string RouteName = txtRouteName.Text;
            //string BrachNames = chblselected.Items;
            //BrachNames = BrachNames.Substring(0, BrachNames.Length - 1);
            //string Ranks = context.Request["Ranks"];
            //string[] strBrachNames = BrachNames.Split('-');
            //Ranks = Ranks.Substring(0, Ranks.Length - 1);
            //string[] strRanks = Ranks.Split('-');
            //string strSno = context.Request["Sno"];
            //string[] SpltSno = strSno.Split('-');
            //int rank = 0;
            //int.TryParse(Ranks.ToString(), out rank);
            //string Username = context.Session["field1"].ToString();
            //string btnSave = context.Request["btnRouteSave"];
            if (btnRouteSave.Text == "Save")
            {
                cmd = new MySqlCommand("Insert into Routetable(RouteName,UserID) values(@RouteName,@UserID)");
                cmd.Parameters.Add("@RouteName", RouteName);
                cmd.Parameters.Add("@UserID", UserName);
                vdm.insert(cmd);
                cmd = new MySqlCommand("Select SNo from Routetable where RouteName=@RouteName and UserID=@UserID ");
                cmd.Parameters.Add("@RouteName", RouteName);
                cmd.Parameters.Add("@UserID", UserName);
                DataTable dt = vdm.SelectQuery(cmd).Tables[0];
                int Sno = (int)dt.Rows[0]["Sno"];
                DataTable dtSno =(DataTable) Session["Branchdata"];
                int i = 0;
                foreach (ListItem str in chblselected.Items)
                {
                    foreach (DataRow dr in dtSno.Rows)
                    {
                        if (str.Text == dr["BranchID"].ToString())
                        {
                            //cmd = new MySqlCommand("Select BranchID from branchdata where UserName=@UserName and Sno=@Sno");
                            //cmd.Parameters.Add("@UserName", UserName);
                            //cmd.Parameters.Add("@Sno", SpltSno[i]);
                            //DataTable dtBranch = vdm.SelectQuery(cmd).Tables[0];
                            //string BranchID = dtBranch.Rows[0]["BranchID"].ToString();
                            cmd = new MySqlCommand("Insert into Routesubtable(SNo,LocationID,BranchName,Rank) values(@SNo,@LocationID,@BranchName,@Rank)");
                            cmd.Parameters.Add("@SNo", Sno);
                            cmd.Parameters.Add("@LocationID", dr["Sno"].ToString());
                            cmd.Parameters.Add("@BranchName", dr["BranchID"].ToString());
                            cmd.Parameters.Add("@Rank", i);
                            vdm.insert(cmd);
                            i++;
                        }
                    }
                }
                lblmsg.Text = "Route Saved Successfully";
                UpdateRoute();
                refresh();
            }
            else if (btnRouteSave.Text == "Edit")
            {
                vdm = new VehicleDBMgr();
                vdm.InitializeDB();
                cmd = new MySqlCommand("Update Routetable set RouteName=@RouteName where UserID=@UserID and SNo=@SNo");
                cmd.Parameters.Add("@RouteName", txtRouteName.Text);
                cmd.Parameters.Add("@UserID", UserName);
                cmd.Parameters.Add("@SNo", RefSno);
                vdm.insert(cmd);
                cmd = new MySqlCommand("Select SNo from Routetable where RouteName=@RouteName and UserID=@UserID ");
                cmd.Parameters.Add("@RouteName", txtRouteName.Text);
                cmd.Parameters.Add("@UserID", UserName);
                DataTable dt = vdm.SelectQuery(cmd).Tables[0];
                int Sno = (int)dt.Rows[0]["Sno"];
                cmd = new MySqlCommand("Delete from  Routesubtable where SNo=@SNo");
                cmd.Parameters.Add("@Sno", Sno);
                vdm.Delete(cmd);
                DataTable dtSno = (DataTable)Session["Branchdata"];
                int i = 0;
                foreach (ListItem str in chblselected.Items)
                {
                    foreach (DataRow dr in dtSno.Rows)
                    {
                        if (str.Text == dr["BranchID"].ToString())
                        {
                            //cmd = new MySqlCommand("Select BranchID from branchdata where UserName=@UserName and Sno=@Sno");
                            //cmd.Parameters.Add("@UserName", UserName);
                            //cmd.Parameters.Add("@Sno", SpltSno[i]);
                            //DataTable dtBranch = vdm.SelectQuery(cmd).Tables[0];
                            //string BranchID = dtBranch.Rows[0]["BranchID"].ToString();
                            cmd = new MySqlCommand("Insert into Routesubtable(SNo,LocationID,BranchName,Rank) values(@SNo,@LocationID,@BranchName,@Rank)");
                            cmd.Parameters.Add("@SNo", Sno);
                            cmd.Parameters.Add("@LocationID", dr["Sno"].ToString());
                            cmd.Parameters.Add("@BranchName", dr["BranchID"].ToString());
                            cmd.Parameters.Add("@Rank", i);
                            vdm.insert(cmd);
                            i++;
                        }
                    }
                }
                lblmsg.Text = "Route Modified Successfully";
                UpdateRoute();
                refresh();

            }
            ScriptManager.RegisterStartupScript(Page, GetType(), "JsStatus", "ClearPolylines();", true);
        }
        catch
        {
        }
    }
}