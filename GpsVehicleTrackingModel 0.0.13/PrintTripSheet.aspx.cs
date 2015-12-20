using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using MySql.Data.MySqlClient;

public partial class PrintTripSheet : System.Web.UI.Page
{
    MySqlCommand cmd;
    DataTable dtAddress = new DataTable();
    VehicleDBMgr vdm;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            if (!Page.IsCallback)
            {
                if (Session["TripSheetNo"] == null)
                {
                }
                else
                {
                    txtTripSheetNo.Text = Session["TripSheetNo"].ToString();
                }
            }
        }
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
    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        vdm = new VehicleDBMgr();
        vdm.InitializeDB();
        cmd = new MySqlCommand("SELECT cabmanagement.VehicleMake, cabmanagement.VehicleModel, cabmanagement.VehicleType, fleettripsheet.TripSheetNo, fleettripsheet.Tripdate, fleettripsheet.Vehicleno, employeestable.EmployeeName, employeestable.phoneNumber, fleettripsheet.Sno, fleettripsheet.LoadType, employeestable.LicenseNo, fleettripsheet.RouteID, routetable.RouteName FROM fleettripsheet INNER JOIN cabmanagement ON fleettripsheet.Vehicleno = cabmanagement.VehicleID INNER JOIN routetable ON fleettripsheet.RouteID = routetable.SNo INNER JOIN employeestable ON fleettripsheet.DriverID = employeestable.Sno WHERE (fleettripsheet.Sno = @TripID)");
        cmd.Parameters.Add("@TripID", txtTripSheetNo.Text);
        DataTable dtTripSheet = vdm.SelectQuery(cmd).Tables[0];
        if (dtTripSheet.Rows.Count > 0)
        {
            lblTripNo.Text = dtTripSheet.Rows[0]["TripSheetNo"].ToString();
            string TripTime = dtTripSheet.Rows[0]["Tripdate"].ToString();
            DateTime dtPlantime = Convert.ToDateTime(TripTime);
            string time = dtPlantime.ToString("dd/MMM/yyyy");
            string strPlantime = dtPlantime.ToString();
            string[] DateTime = strPlantime.Split(' ');
            string[] PlanDateTime = strPlantime.Split(' ');
            lblDate.Text = time;
            lblTime.Text = PlanDateTime[1];
            lblVehicleNo.Text = dtTripSheet.Rows[0]["Vehicleno"].ToString();
            lblMake.Text = dtTripSheet.Rows[0]["VehicleMake"].ToString();
            lblModel.Text = dtTripSheet.Rows[0]["VehicleModel"].ToString();
            lblVehicleType.Text = dtTripSheet.Rows[0]["VehicleType"].ToString();

            lblPhoneNo.Text = dtTripSheet.Rows[0]["phoneNumber"].ToString();
            lblDriverName.Text = dtTripSheet.Rows[0]["EmployeeName"].ToString();
            lblLicenceNo.Text = dtTripSheet.Rows[0]["LicenseNo"].ToString();
            lblAssignRoute.Text = dtTripSheet.Rows[0]["RouteName"].ToString();
            lblTypeOfLoad.Text = dtTripSheet.Rows[0]["LoadType"].ToString();
            
            
            BindEmpty();
        }
    }
    void BindEmpty()
    {
        DataTable EmptyReport = new DataTable();
        EmptyReport.Columns.Add("Sno");
        EmptyReport.Columns.Add("Date");
        EmptyReport.Columns.Add("Time");
        EmptyReport.Columns.Add("Km's");
        EmptyReport.Columns.Add("Place");
        EmptyReport.Columns.Add("Details");
        EmptyReport.Columns.Add("Amount");
        EmptyReport.Columns.Add("Qty");
        EmptyReport.Columns.Add("Diesel Filled");
        int i = 10;
        for (i = 1; i < 11; i++)
        {
            DataRow newr = EmptyReport.NewRow();
            newr["Sno"] = i.ToString();
            EmptyReport.Rows.Add(newr);
        }
        grdReports.DataSource = EmptyReport;
        grdReports.DataBind();
    }
}