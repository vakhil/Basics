using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;

public partial class DistanceMaster : System.Web.UI.Page
{
    VehicleDBMgr vdm;
    DataDownloader ddwnldr;
    MySqlCommand cmd;
    string UserName;
    Dictionary<string, int> branchsnos = new Dictionary<string, int>();
    Dictionary<int, string> branchnames = new Dictionary<int, string>();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["field1"] == null)
            Response.Redirect("Login.aspx");
        else
        {
            UserName = Session["field1"].ToString();
            string userrefno = Session["field3"].ToString();
            vdm = new VehicleDBMgr();
            vdm.InitializeDB();
            if (!this.IsPostBack)
            {
                if (!Page.IsCallback)
                {
                    ddwnldr = new DataDownloader();
                    ddwnldr.UpdateBranchDetails(UserName);
                    DataTable finaltable = new DataTable();
                    finaltable.Columns.Add("frombranch");
                    finaltable.Columns.Add("tobranch");
                    ViewState["branches"] = ddwnldr.BranchDetails;
                  
                    ddlfromlocation.Items.Add("ALL");
                    foreach (DataRow dr in ddwnldr.BranchDetails.Rows)
                    {
                        ddlfromlocation.Items.Add(dr["BranchID"].ToString().TrimEnd());
                        branchsnos.Add(dr["BranchID"].ToString(), int.Parse(dr["Sno"].ToString()));
                        branchnames.Add(int.Parse(dr["Sno"].ToString()), dr["BranchID"].ToString());
                    }
                    ViewState["branchsnos"] = branchsnos;
                    ViewState["branchnames"] = branchnames;

                    MySqlCommand cmd = new MySqlCommand("SELECT * FROM location_groups WHERE (user_sno = @userid)");
                    cmd.Parameters.Add("@userid", userrefno);
                    DataTable dtPlant = vdm.SelectQuery(cmd).Tables[0];
                    ddl_group.Items.Add("ALL");
                    foreach (DataRow dr in dtPlant.Rows)
                    {
                        ListItem li = new ListItem();
                        li.Text = dr["location_group_name"].ToString().TrimEnd();
                        li.Value = dr["sno"].ToString().TrimEnd();
                        ddl_group.Items.Add(li);
                    }
                }
            }
        }
    }
    protected void ddlfromlocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        lbl_Status.Text = "";
        branchsnos = (Dictionary<string, int>)ViewState["branchsnos"];
        branchnames = (Dictionary<int, string>)ViewState["branchnames"];
        DataTable branches = new DataTable();
        branches = (DataTable)ViewState["branches"];
        if (branches.Rows.Count <= 0)
        {
            ddwnldr.UpdateBranchDetails(UserName);
            branches = ddwnldr.BranchDetails;
            ViewState["branches"] = ddwnldr.BranchDetails;
        }
        DataTable finaltable = new DataTable();
        finaltable.Columns.Add("frombranch");
        finaltable.Columns.Add("tobranch");
        finaltable.Columns.Add("kms");
        finaltable.Columns.Add("time");
        cmd = new MySqlCommand("SELECT sno, frombranchsno, tobranchsno, kms, expectedtime FROM locatondistances where frombranchsno=@frombranchsno and username=@username");
        cmd.Parameters.Add("@frombranchsno", branchsnos[ddlfromlocation.SelectedValue]);
        cmd.Parameters.Add("@username", UserName);
        DataTable branchdata = vdm.SelectQuery(cmd).Tables[0];
        if (branchdata.Rows.Count > 0)
        {
            foreach (DataRow to in branches.Rows)
            {
                DataRow newrow = finaltable.NewRow();
                DataRow[] distancerow = branchdata.Select("frombranchsno=" + branchsnos[ddlfromlocation.SelectedValue] + " and tobranchsno=" + branchsnos[to["BranchID"].ToString()] + "");
                if (distancerow.Length > 0)
                {
                    int inputime = 0;
                    int.TryParse(distancerow[0]["expectedtime"].ToString(), out inputime);
                    int hours = (int)(inputime / 60);
                    int minutes = (int)(inputime % 60);
                    string totalmins = hours + ":" + minutes;

                    newrow["frombranch"] = ddlfromlocation.SelectedValue;
                    newrow["tobranch"] = to["BranchID"].ToString();
                    newrow["kms"] = distancerow[0]["kms"].ToString();
                    newrow["time"] = totalmins;
                    finaltable.Rows.Add(newrow);
                }
                else
                {
                    if (ddlfromlocation.SelectedValue != to["BranchID"].ToString())
                    {
                        newrow["frombranch"] = ddlfromlocation.SelectedValue;
                        newrow["tobranch"] = to["BranchID"].ToString();
                        newrow["time"] = "00:00";
                        finaltable.Rows.Add(newrow);
                    }
                }
            }
        }
        else
        {
            foreach (DataRow to in branches.Rows)
            {
                if (ddlfromlocation.SelectedValue != to["BranchID"].ToString())
                {
                    DataRow newrow = finaltable.NewRow();
                    newrow["frombranch"] = ddlfromlocation.SelectedValue;
                    newrow["tobranch"] = to["BranchID"].ToString();
                    newrow["time"] = "00:00";
                    finaltable.Rows.Add(newrow);
                }
            }
        }
        GridView1.DataSource = finaltable;
        GridView1.DataBind();
        btnsave.Visible = true;
    }
    protected void ddlgroup_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddl_group.SelectedValue != "ALL")
        {
            cmd = new MySqlCommand("SELECT location_groups_mapping.sno, location_groups_mapping.location_group_sno, location_groups_mapping.branch_sno, branchdata.BranchID FROM location_groups_mapping INNER JOIN branchdata ON location_groups_mapping.branch_sno = branchdata.Sno WHERE (location_groups_mapping.location_group_sno = @lgs)");
            cmd.Parameters.Add("@lgs", ddl_group.SelectedValue);
            DataTable locinfo = vdm.SelectQuery(cmd).Tables[0];
            ViewState["branches"] = locinfo;
            ddlfromlocation.Items.Clear();
            ddlfromlocation.Items.Add("ALL");
            foreach (DataRow dr in locinfo.Rows)
            {
                ddlfromlocation.Items.Add(dr["BranchID"].ToString().TrimEnd());
                branchsnos.Add(dr["BranchID"].ToString(), int.Parse(dr["branch_sno"].ToString()));
                branchnames.Add(int.Parse(dr["branch_sno"].ToString()), dr["BranchID"].ToString());
            }
        }
        else
        {
            ddlfromlocation.Items.Clear();
            ddwnldr = new DataDownloader();
            ddwnldr.UpdateBranchDetails(UserName);
            DataTable finaltable = new DataTable();
            finaltable.Columns.Add("frombranch");
            finaltable.Columns.Add("tobranch");
            ViewState["branches"] = ddwnldr.BranchDetails;
            ddlfromlocation.Items.Add("ALL");
            foreach (DataRow dr in ddwnldr.BranchDetails.Rows)
            {
                ddlfromlocation.Items.Add(dr["BranchID"].ToString().TrimEnd());
                branchsnos.Add(dr["BranchID"].ToString(), int.Parse(dr["Sno"].ToString()));
                branchnames.Add(int.Parse(dr["Sno"].ToString()), dr["BranchID"].ToString());
            }
        }
    }
    protected void btnsave_click(object sender, EventArgs e)
    {
        try
        {
            lbl_Status.Text = "";
            branchsnos = (Dictionary<string, int>)ViewState["branchsnos"];
            branchnames = (Dictionary<int, string>)ViewState["branchnames"];
            cmd = new MySqlCommand("SELECT sno, frombranchsno, tobranchsno, kms, expectedtime FROM locatondistances where frombranchsno=@frombranchsno and username=@username");
            cmd.Parameters.Add("@frombranchsno", branchsnos[ddlfromlocation.SelectedValue]);
            cmd.Parameters.Add("@username", UserName);
            DataTable branchdata = vdm.SelectQuery(cmd).Tables[0];
            if (branchdata.Rows.Count > 0)
            {
                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    string frombranchsno = ((TextBox)GridView1.Rows[i].FindControl("txtFrombranch")).Text;
                    string tobranchsno = ((TextBox)GridView1.Rows[i].FindControl("txtTobranch")).Text;
                    string kms = ((TextBox)GridView1.Rows[i].FindControl("txtkms")).Text;
                    float distance = 0;
                    float.TryParse(kms, out distance);

                    string time = ((TextBox)GridView1.Rows[i].FindControl("txttime")).Text;
                    int hours = 0;
                    int mins = 0;
                    double totalmins = 0;
                    if (time != "")
                    {
                        string[] splittedtime = time.Split(':');
                        int.TryParse(splittedtime[0], out hours);
                        int.TryParse(splittedtime[1], out mins);
                        TimeSpan timespanhours = new TimeSpan(hours, mins, 0);
                        totalmins = timespanhours.TotalMinutes;
                    }
                    DataRow[] distancerow = branchdata.Select("frombranchsno=" + branchsnos[frombranchsno] + " and tobranchsno=" + branchsnos[tobranchsno] + "");
                    if (distancerow.Length > 0)
                    {
                        cmd = new MySqlCommand("update locatondistances set kms=@kms,expectedtime=@expectedtime where frombranchsno=@frombranchsno and tobranchsno=@tobranchsno and username=@username");
                        cmd.Parameters.Add("@frombranchsno", branchsnos[frombranchsno]);
                        cmd.Parameters.Add("@tobranchsno", branchsnos[tobranchsno]);
                        cmd.Parameters.Add("@kms", distance);
                        cmd.Parameters.Add("@expectedtime", totalmins);
                        cmd.Parameters.Add("@username", UserName);
                        vdm.Update(cmd);
                    }
                    else
                    {
                        cmd = new MySqlCommand("insert into locatondistances (frombranchsno, tobranchsno,kms,expectedtime,username) values (@frombranchsno, @tobranchsno,@kms,@expectedtime,@username)");
                        cmd.Parameters.Add("@frombranchsno", branchsnos[frombranchsno]);
                        cmd.Parameters.Add("@tobranchsno", branchsnos[tobranchsno]);
                        cmd.Parameters.Add("@kms", distance);
                        cmd.Parameters.Add("@expectedtime", totalmins);
                        cmd.Parameters.Add("@username", UserName);
                        vdm.insert(cmd);
                    }
                }
            }
            else
            {
                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    string frombranchsno = ((TextBox)GridView1.Rows[i].FindControl("txtFrombranch")).Text;
                    string tobranchsno = ((TextBox)GridView1.Rows[i].FindControl("txtTobranch")).Text;
                    string kms = ((TextBox)GridView1.Rows[i].FindControl("txtkms")).Text;
                    if (kms != "")
                    {
                        float distance = 0;
                        float.TryParse(kms, out distance);
                        string time = ((TextBox)GridView1.Rows[i].FindControl("txttime")).Text;
                        int hours = 0;
                        int mins = 0;
                        double totalmins = 0;
                        if (time != "")
                        {
                            string[] splittedtime = time.Split(':');
                            int.TryParse(splittedtime[0], out hours);
                            int.TryParse(splittedtime[1], out mins);
                            TimeSpan timespanhours = new TimeSpan(hours, mins, 0);
                            totalmins = timespanhours.TotalMinutes;
                        }

                        cmd = new MySqlCommand("insert into locatondistances (frombranchsno, tobranchsno,kms,expectedtime,username) values (@frombranchsno, @tobranchsno,@kms,@expectedtime,@username)");
                        cmd.Parameters.Add("@frombranchsno", branchsnos[frombranchsno]);
                        cmd.Parameters.Add("@tobranchsno", branchsnos[tobranchsno]);
                        cmd.Parameters.Add("@kms", distance);
                        cmd.Parameters.Add("@expectedtime", totalmins);
                        cmd.Parameters.Add("@username", UserName);
                        vdm.insert(cmd);
                    }
                }
            }
            lbl_Status.Text = "Data successfully saved";
            //string script = string.Format("alert('{0}');", "Data successfully saved");
            //ScriptManager.RegisterStartupScript(this, GetType(), "alert", script, true);
        }
        catch (Exception ex)
        {
            lbl_Status.Text = "Error.please try again";
        }
    }
}