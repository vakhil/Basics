using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;

public partial class Tripend : System.Web.UI.Page
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
                    
                }
            }
        }
    }
    void updateTripend()
    {
        DateTime fromdate = DateTime.Now;
        DateTime todate = DateTime.Now;
        string[] datestrig = txtFromDate.Text.Split(' ');

        if (datestrig.Length > 1)
        {
            if (datestrig[0].Split('-').Length > 0)
            {
                string[] dates = datestrig[0].Split('-');
                string[] times = datestrig[1].Split(':');
                fromdate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
            }
        }
       
        datestrig = txtEnddate.Text.Split(' ');
        if (datestrig.Length > 1)
        {
            if (datestrig[0].Split('-').Length > 0)
            {
                string[] dates = datestrig[0].Split('-');
                string[] times = datestrig[1].Split(':');
                todate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
            }
        }
        cmd = new MySqlCommand("Select Tripid,assigndate,RouteName,Status,Refno,completdate from tripdata where UserID=@UserID and assigndate>@d1 and assigndate<@d2");
        cmd.Parameters.Add("@UserID", UserName);
        cmd.Parameters.Add("@d1", fromdate);
        cmd.Parameters.Add("@d2", todate);
        DataTable dt = vdm.SelectQuery(cmd).Tables[0];
        foreach (DataRow dr in dt.Rows)
        {
            if (dr["Status"].ToString() == "A")
            {
                dr["Status"] = "Assigned";
            }
            if (dr["Status"].ToString() == "B")
            {
                dr["Status"] = "Cancelled";
            }
            if (dr["Status"].ToString() == "C")
            {
                dr["Status"] = "Completed";
            }
        }
        grdTripend.DataSource = dt;
        grdTripend.DataBind();
    }

    protected void btnGenerate_OnClick(object sender, EventArgs e)
    {
        updateTripend();
    }
    protected void btnTripend_OnClick(object sender, EventArgs e)
    {
        try
        {
            DateTime CompleteDate = DateTime.Now;
            DateTime Assigndate = DateTime.Now;
            string[] datestrig = txtCompleteDate.Text.Split(' ');

            if (datestrig.Length > 1)
            {
                if (datestrig[0].Split('-').Length > 0)
                {
                    string[] dates = datestrig[0].Split('-');
                    string[] times = datestrig[1].Split(':');
                    CompleteDate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                }
            }

            datestrig = txtAssigndate.Text.Split(' ');
            if (datestrig.Length > 1)
            {
                if (datestrig[0].Split('-').Length > 0)
                {
                    string[] dates = datestrig[0].Split('-');
                    string[] times = datestrig[1].Split(':');
                    Assigndate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                }
            }
            cmd = new MySqlCommand("update tripdata set completdate=@completdate,Status=@Status,assigndate=@assigndate where Tripid=@Tripid and UserID=@UserID and Refno=@Refno");
            cmd.Parameters.Add("@Tripid", txtTripName.Text);
            cmd.Parameters.Add("@Status", ddlStatus.SelectedValue);
            cmd.Parameters.Add("@completdate", CompleteDate);
            cmd.Parameters.Add("@assigndate", Assigndate);
            cmd.Parameters.Add("@UserID", UserName);
            cmd.Parameters.Add("@Refno", Refno);
            vdm.Update(cmd);
            updateTripend();
            refresh();
            lblmsg.Text = "Saved Successfully";
        }
        catch
        {
        }
    }
    void refresh()
    {
        txtTripName.Text = "";
        txtAssigndate.Text = "";
        txtCompleteDate.Text = "";
        ddlStatus.ClearSelection();
    }
    public static string Refno = "";
    protected void grdTripend_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (grdTripend.SelectedIndex > -1)
        {

            try
            {
                GridViewRow gvr = grdTripend.SelectedRow;
                txtTripName.Text = gvr.Cells[1].Text;
                DateTime FormatAssigndate = Convert.ToDateTime(gvr.Cells[2].Text);
                string sAssigndate = FormatAssigndate.ToString("dd-M-yyyy HH:mm");
                txtAssigndate.Text = sAssigndate;
                Refno = gvr.Cells[5].Text;
            }
            catch
            {
            }
        }
    }
    
}