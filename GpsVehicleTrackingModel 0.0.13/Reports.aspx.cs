using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Threading;
using MySql.Data.MySqlClient;
using System.IO;
using System.Globalization;
using System.Web.Services;
using System.Web.Script.Services;
using System.Runtime.Remoting.Contexts;
using GPSApplication;
public partial class Reports : System.Web.UI.Page
{
    MySqlCommand cmd;
    double Maxspeed = 0;
    float AvgSpeed = 0;
    float IdleTime = 0;
    float Stationarytime = 0;
    float TotalDistance = 0;
    double totalSpeed = 0;
    double Lvalue1 = 17.497535;
    double Lonvalue1 = 78.408622;
    double Lvalue2 = 17.482964;
    double Lonvalue2 = 78.413509;
    string UserName = "";
    int zoomlevel = 14;
    Queue<GooglePoint> listofpoints = new Queue<GooglePoint>();

    GooglePolyline PL1 = null;
    VehicleDBMgr vdm;
    DataDownloader ddwnldr;
    string reportname = "";
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
                    ddwnldr = new DataDownloader();
                    GetAssignedGeofenceData();
                    ddwnldr.UpdateBranchDetails(UserName);
                    PL1 = new GooglePolyline();
                    PL1.ID = "PL1";
                    //Give Hex code for line color
                    PL1.ColorCode = "#0000FF";
                    //Specify width for line
                    PL1.Width = 5;
                    //lblWaterMark.Text = "reports";
                    FillSelectVehicle();
                    DDL_locations.Items.Clear();
                    ddlfromlocation.Items.Clear();
                    ddltolocation.Items.Clear();
                    DDL_locations.Items.Add("ALL");
                    ddlfromlocation.Items.Add("ALL");
                    ddltolocation.Items.Add("ALL");
                    string userrefno = Session["field3"].ToString();
                    foreach (DataRow dr in ddwnldr.BranchDetails.Rows)
                    {
                        ddlfromlocation.Items.Add(dr["BranchID"].ToString().TrimEnd());
                        ddltolocation.Items.Add(dr["BranchID"].ToString().TrimEnd());
                        DDL_locations.Items.Add(dr["BranchID"].ToString().TrimEnd());
                    }
                    ViewState["branches"] = ddwnldr.BranchDetails;

                    MySqlCommand cmd = new MySqlCommand("SELECT * FROM location_groups WHERE (user_sno = @userid)");
                    cmd.Parameters.Add("@userid", userrefno);
                    DataTable dtPlant = vdm.SelectQuery(cmd).Tables[0];
                    ddlgroup.Items.Add("ALL");
                    foreach (DataRow dr in dtPlant.Rows)
                    {
                        ListItem li = new ListItem();
                        li.Text = dr["location_group_name"].ToString().TrimEnd();
                        li.Value = dr["sno"].ToString().TrimEnd();
                        ddlgroup.Items.Add(li);
                    }
                    UpdateVehicleGroupData();
                    FillVehicleMaster();
                    startdate.Text = GetLowDate(DateTime.Now).ToString("dd-MM-yyyy HH:mm");
                    enddate.Text = GetHighDate(DateTime.Now).ToString("dd-MM-yyyy HH:mm");
                    reportname = Request.QueryString["Report"];
                    lblreportname.Text = reportname;

                    switch (reportname)
                    {
                        case "OverSpeed Report":
                            lbl_show.Text = "SpeedLimit";
                            lbl_show.Visible = true;
                            txt_Reports_TimeGap.Visible = true;
                            DDL_locations.Visible = false;
                            lblfromlocation.Visible = false;
                            lbltolocation.Visible = false;
                            ddlfromlocation.Visible = false;
                            ddltolocation.Visible = false;
                            lblgroup.Visible = false;
                            ddlgroup.Visible = false;
                            break;
                        case "Location HaltingHours Report":
                            lbl_show.Text = "Location";
                            lbl_show.Visible = true;
                            txt_Reports_TimeGap.Visible = false;
                            DDL_locations.Visible = true;
                            ddlfromlocation.Visible = false;
                            ddltolocation.Visible = false;
                            lblfromlocation.Visible = false;
                            lbltolocation.Visible = false;
                            lblgroup.Visible = false;
                            ddlgroup.Visible = false;
                            break;
                        case "Location To Location Report":
                            lbl_show.Text = "Location";
                            lbl_show.Visible = false;
                            txt_Reports_TimeGap.Visible = false;
                            DDL_locations.Visible = false;
                            ddlfromlocation.Visible = true;
                            ddltolocation.Visible = true;
                            lblfromlocation.Visible = true;
                            lbltolocation.Visible = true;
                            lblgroup.Visible = true;
                            ddlgroup.Visible = true;
                            lblCost.Visible = true;
                            txtCost.Visible = true;
                            txtCost.Text = "0";
                            break;
                        case "Tanker and Puffs Trip Report":
                            lbl_show.Text = "Location";
                            lbl_show.Visible = false;
                            txt_Reports_TimeGap.Visible = false;
                            DDL_locations.Visible = false;
                            ddlfromlocation.Visible = false;
                            ddltolocation.Visible = false;
                            lblfromlocation.Visible = false;
                            lbltolocation.Visible = false;
                            lblCost.Visible = true;
                            txtCost.Visible = true;
                            txtCost.Text = "0";
                            break;
                        case "Sales Trip Report":
                             lbl_show.Text = "Location";
                            lbl_show.Visible = false;
                            txt_Reports_TimeGap.Visible = false;
                            DDL_locations.Visible = false;
                            ddlfromlocation.Visible = false;
                            ddltolocation.Visible = false;
                            lblfromlocation.Visible = false;
                            lbltolocation.Visible = false;
                            lblCost.Visible = true;
                            txtCost.Visible = true;
                            txtCost.Text = "0";
                            break;

                        default:
                            lbl_show.Visible = false;
                            txt_Reports_TimeGap.Visible = false;
                            DDL_locations.Visible = false;
                            ddlfromlocation.Visible = false;
                            ddltolocation.Visible = false;
                            lblfromlocation.Visible = false;
                            lbltolocation.Visible = false;
                            lblgroup.Visible = false;
                            ddlgroup.Visible = false;
                            break;
                    }
                }
            }
        }
    }
    protected void ddlgroup_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlgroup.SelectedValue != "ALL")
        {
            cmd = new MySqlCommand("SELECT location_groups_mapping.sno, location_groups_mapping.location_group_sno, location_groups_mapping.branch_sno, branchdata.BranchID,branchdata.Latitude, branchdata.Longitude, branchdata.Radious FROM location_groups_mapping INNER JOIN branchdata ON location_groups_mapping.branch_sno = branchdata.Sno WHERE (location_groups_mapping.location_group_sno = @lgs)");
            cmd.Parameters.Add("@lgs", ddlgroup.SelectedValue);
            DataTable locinfo = vdm.SelectQuery(cmd).Tables[0];
            ViewState["branches"] = locinfo;
            ddlfromlocation.Items.Clear();
            ddlfromlocation.Items.Add("ALL");
            ddltolocation.Items.Clear();
            ddltolocation.Items.Add("ALL");
            foreach (DataRow dr in locinfo.Rows)
            {
                ddlfromlocation.Items.Add(dr["BranchID"].ToString().TrimEnd());
                ddltolocation.Items.Add(dr["BranchID"].ToString().TrimEnd());
            }
        }
        else
        {
            ddltolocation.Items.Clear();
            ddltolocation.Items.Add("ALL");
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
                ddltolocation.Items.Add(dr["BranchID"].ToString().TrimEnd());
            }
        }
    }
    void FillVehicleMaster()
    {
        string reportname = Request.QueryString["Report"];
        if (reportname == "Vehicle Master Report")
        {
            vdm = new VehicleDBMgr();
            vdm.InitializeDB();
            //cmd = new MySqlCommand("SELECT paireddata.VehicleNumber, paireddata.VehicleType, vehiclemaster.VehicleTypeName,vehiclemaster.Capacity,vehiclemaster.VendorNo, vehiclemaster.VendorName,vehiclemaster.MaintenancePlantCode, vehiclemaster.MaintenancePlantName FROM loginsconfigtable INNER JOIN paireddata ON loginsconfigtable.VehicleID = paireddata.VehicleNumber INNER JOIN loginstable ON paireddata.UserID = loginstable.main_user AND loginsconfigtable.Refno = loginstable.refno INNER JOIN vehiclemaster ON loginsconfigtable.VehicleID = vehiclemaster.VehicleID AND paireddata.UserID = vehiclemaster.UserName WHERE (loginstable.loginid = @UserName)");
            cmd = new MySqlCommand("select VehicleID, VehicleType,DriverName,PlantName,RouteName,Routecode,MobileNo,Address,SNo from Cabmanagement where UserID=@UserID ");
            cmd.Parameters.Add("@UserID", UserName);
            DataTable dt = vdm.SelectQuery(cmd).Tables[0];
            grdReports.DataSource = dt;
            grdReports.DataBind();
            for (int i = 0; i < grdReports.Rows.Count; i++)
            {
                grdReports.Rows[i].Cells[0].Controls.Remove(grdReports.Rows[i].Cells[0].Controls[1]);
            }
            Session["xportdata"] = dt;
            Session["title"] = "Vehicle Master Report";
            ScriptManager.RegisterStartupScript(Page, GetType(), "JsStatus", "PopupClose();", true);
        }
        else if (reportname == "Vehicle Manage Report")
        {
            vdm = new VehicleDBMgr();
            vdm.InitializeDB();
            cmd = new MySqlCommand("select ItemType,ItemName,ItemCode from vehiclemanage where UserName=@UserName");
            cmd.Parameters.Add("@UserName", UserName);
            DataTable dt = vdm.SelectQuery(cmd).Tables[0];
            grdReports.DataSource = dt;
            grdReports.DataBind();
            for (int i = 0; i < grdReports.Rows.Count; i++)
            {
                grdReports.Rows[i].Cells[0].Controls.Remove(grdReports.Rows[i].Cells[0].Controls[1]);
            }
            Session["xportdata"] = dt;
            Session["title"] = "Vehicle Manage Report";
            ScriptManager.RegisterStartupScript(Page, GetType(), "JsStatus", "PopupClose();", true);
        }

    }
    public void UpdateVehicleGroupData()
    {
        Session["Authorized"] = "Plants";
        DataTable totaldata = new DataTable();
        if (Session["vendorstable"] != null)
        {
            totaldata = (DataTable)Session["vendorstable"];
        }
        else
        {
            cmd = new MySqlCommand("SELECT cabmanagement.PlantName, cabmanagement.VehicleType, cabmanagement.VehicleID,cabmanagement.RouteName, cabmanagement.RouteCode FROM cabmanagement INNER JOIN loginsconfigtable ON cabmanagement.VehicleID = loginsconfigtable.VehicleID INNER JOIN  loginstable ON cabmanagement.UserID = loginstable.main_user AND loginsconfigtable.Refno = loginstable.refno WHERE (loginstable.loginid = @UserName)");
            //cmd = new MySqlCommand("SELECT cabmanagement.PlantName, cabmanagement.VehicleType, cabmanagement.VehicleID FROM cabmanagement INNER JOIN loginsconfigtable ON cabmanagement.VehicleID = loginsconfigtable.VehicleID INNER JOIN  loginstable ON cabmanagement.UserID = loginstable.main_user AND loginsconfigtable.Refno = loginstable.refno WHERE (loginstable.loginid = @UserName)");
            cmd.Parameters.Add("@UserName", UserName);
            totaldata = vdm.SelectQuery(cmd).Tables[0];
            Session["vendorstable"] = totaldata;
        }

        //cmd = new MySqlCommand("SELECT UserName, BranchID, Description, Latitude, Longitude, PhoneNumber, ImagePath, Radious,Sno, PlantName, IsPlant FROM branchdata WHERE (UserName = @UserName) AND (IsPlant = '1')");
        //cmd.Parameters.Add("@UserName", UserName);
        //DataTable dtPlant = vdm.SelectQuery(cmd).Tables[0];
        DataView view = new DataView(totaldata);
        DataTable dtPlant = view.ToTable(true, "PlantName");

        view = new DataView(totaldata);
        DataTable vehicletypes = view.ToTable(true, "VehicleType");
        chblVehicleTypes.Items.Clear();
        chblZones.Items.Clear();

        if (chblVehicleTypes.SelectedIndex == -1)
        {
            chblVehicleTypes.Items.Add("All Vehicle Types");
        }
        if (chblZones.SelectedIndex == -1)
        {
            chblZones.Items.Add("All Plants");
        }
        foreach (DataRow dr in vehicletypes.Rows)
        {
            if (dr["VehicleType"].ToString() != "")
                chblVehicleTypes.Items.Add(dr["VehicleType"].ToString());
        }
        foreach (DataRow dr in dtPlant.Rows)
        {
            if (dr["PlantName"].ToString() != "")
                chblZones.Items.Add(dr["PlantName"].ToString());
        }
    }
    DataTable AGDataTable;
    void GetAssignedGeofenceData()
    {
        cmd = new MySqlCommand("select VehicleID,Geofencename,GeofenceType from AssignGeofence where UserName=@UserName");
        cmd.Parameters.Add("@UserName", UserName);
        AGDataTable = vdm.SelectQuery(cmd).Tables[0];
    }
    DataRow[] HasGeofence = null;


    DataTable table;
    Dictionary<string, DataTable> reportData = new Dictionary<string, DataTable>();
    //public class GeoCodeCalc
    //{
    //    public const double EarthRadiusInMiles = 3956.0;
    //    public const double EarthRadiusInKilometers = 6367.0;
    //    public static double ToRadian(double val) { return val * (Math.PI / 180); }
    //    public static double DiffRadian(double val1, double val2) { return ToRadian(val2) - ToRadian(val1); }
    //    /// <summary> 
    //    /// Calculate the distance between two geocodes. Defaults to using Miles. 
    //    /// </summary> 
    //    public static double CalcDistance(double lat1, double lng1, double lat2, double lng2)
    //    {
    //        return CalcDistance(lat1, lng1, lat2, lng2, GeoCodeCalcMeasurement.Kilometers);
    //    }
    //    /// <summary> 
    //    /// Calculate the distance between two geocodes. 
    //    /// </summary> 
    //    public static double CalcDistance(double lat1, double lng1, double lat2, double lng2, GeoCodeCalcMeasurement m)
    //    {
    //        double radius = GeoCodeCalc.EarthRadiusInMiles;
    //        if (m == GeoCodeCalcMeasurement.Kilometers) { radius = GeoCodeCalc.EarthRadiusInKilometers; }
    //        return radius * 2 * Math.Asin(Math.Min(1, Math.Sqrt((Math.Pow(Math.Sin((DiffRadian(lat1, lat2)) / 2.0), 2.0) + Math.Cos(ToRadian(lat1)) * Math.Cos(ToRadian(lat2)) * Math.Pow(Math.Sin((DiffRadian(lng1, lng2)) / 2.0), 2.0)))));
    //    }
    //}

    public enum GeoCodeCalcMeasurement : int
    {
        Miles = 0,
        Kilometers = 1
    }
    public class obj
    {
        public int lid;
        public string BranchID;
        public double distance;
        public int radius;
        public double longitude;
        public double latitude;

    }
    public class DistanceAlgorithm
    {
        const double PIx = 3.141592653589793;
        const double RADIUS = 6378.16;

        /// <summary>
        /// This class cannot be instantiated.
        /// </summary>
        private DistanceAlgorithm() { }

        /// <summary>
        /// Convert degrees to Radians
        /// </summary>
        /// <param name="x">Degrees</param>
        /// <returns>The equivalent in radians</returns>
        public static double Radians(double x)
        {
            return x * PIx / 180;
        }

        /// <summary>
        /// Calculate the distance between two places.
        /// </summary>
        /// <param name="lon1"></param>
        /// <param name="lat1"></param>
        /// <param name="lon2"></param>
        /// <param name="lat2"></param>
        /// <returns></returns>
        public static double DistanceBetweenPlaces(
            double lon1,
            double lat1,
            double lon2,
            double lat2)
        {
            double dlon = Radians(lon2 - lon1);
            double dlat = Radians(lat2 - lat1);

            double a = (Math.Sin(dlat / 2) * Math.Sin(dlat / 2)) + Math.Cos(Radians(lat1)) * Math.Cos(Radians(lat2)) * (Math.Sin(dlon / 2) * Math.Sin(dlon / 2));
            double angle = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return angle * RADIUS;
        }
    }
    string mainuser = "";
    protected void btn_generate_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable sampletable = new DataTable();
            grdReports.DataSource = sampletable;
            grdReports.DataBind();
            string ckdvcls = hdnResultValue.Value;
            Array checkedvhcles = ckdvcls.Split(',');
            pointcount = 0;
            int count = 0;
            lbl_nofifier.Text = "";
            if (Session["main_user"] == null)
            {
                cmd = new MySqlCommand("SELECT main_user FROM loginstable WHERE (loginid = @loginid)");
                cmd.Parameters.Add("@loginid", UserName);
                DataTable mainusertbl = vdm.SelectQuery(cmd).Tables[0];
                if (mainusertbl.Rows.Count > 0)
                {
                    mainuser = mainusertbl.Rows[0]["main_user"].ToString();
                    Session["main_user"] = mainuser;
                }
                else
                {
                    mainuser = UserName;
                }
            }
            else
            {
                mainuser = Session["main_user"].ToString();
            }
            reportData = new Dictionary<string, DataTable>();
            // count = checkedvhcles.Length;
            foreach (string vehiclestr in checkedvhcles)
            {
                //AttributeCollection atbcol = lvi.Attributes;
                if (vehiclestr != "0" && vehiclestr != "")
                {
                    count++;
                }
            }
            #region code
            if (count > 0)
            {
                if (startdate.Text != "" && enddate.Text != "")
                {
                    DateTime fromdate = DateTime.Now;//System.Convert.ToDateTime(startdate.Text);//startdate_CalendarExtender.SelectedDate ?? DateTime.Now;// DateTime.Now.AddMonths(-3);//DateTime.Parse(startdate.Text); ;
                    DateTime todate = DateTime.Now;//System.Convert.ToDateTime(enddate.Text);//enddate_CalendarExtender.SelectedDate ?? DateTime.Now; //DateTime.Parse(enddate.Text);
                    DateTime AMfromdate = DateTime.Now;
                    DateTime AMtodate = DateTime.Now;
                    DateTime PMfromdate = DateTime.Now;
                    DateTime PMtodate = DateTime.Now;
                    // d/M/yyyy HH:mm
                    string[] datestrig = startdate.Text.Split(' ');

                    if (datestrig.Length > 1)
                    {
                        if (datestrig[0].Split('-').Length > 0)
                        {
                            string[] dates = datestrig[0].Split('-');
                            string[] times = datestrig[1].Split(':');
                            fromdate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                            AMfromdate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                            PMfromdate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                        }
                    }
                    else
                    {
                        // MessageBox.Show("Date Time Format Wrong");
                        lbl_nofifier.Text = "From Date Time Format Wrong";
                        return;
                    }

                    datestrig = enddate.Text.Split(' ');
                    if (datestrig.Length > 1)
                    {
                        if (datestrig[0].Split('-').Length > 0)
                        {
                            string[] dates = datestrig[0].Split('-');
                            string[] times = datestrig[1].Split(':');
                            todate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                            AMtodate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                            PMtodate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                        }
                    }
                    else
                    {
                        // MessageBox.Show("Date Time Format Wrong");
                        lbl_nofifier.Text = "To Date Time Format Wrong";
                        return;
                    }

                    reportname = Request.QueryString["Report"];
                    List<string> logstbls = new List<string>();
                    logstbls.Add("GpsTrackVehicleLogs");
                    logstbls.Add("GpsTrackVehicleLogs1");
                    logstbls.Add("GpsTrackVehicleLogs2");
                    logstbls.Add("GpsTrackVehicleLogs3");
                    if (reportname == "General Report")
                    {
                        #region GeneralReports
                        DataTable rpttable = new DataTable();
                        
                        DataColumn col = new DataColumn("VehicleID");
                        rpttable.Columns.Add(col);
                        DataColumn colRoute = new DataColumn("Route Name");
                        rpttable.Columns.Add(colRoute);
                        //"Report From: " + fromdate.ToString() + "  To: " + todate.ToString() + " and  TotalDistance Travelled:" + (int)TotalDistance + "\n" +
                        //    "Motion Hours:" + (int)RunningTime / 3600 + "H " + (int)RunningTime % (60) + "Min  Stationary Hours:" + (int)StopTime / 3600 + "H " + (int)StopTime % 60 + "Min  Max Speed:" + (int)Maxspeed + "KMPH  AvgSpeed: " + String.Format("{0:0.00}", avgspeed) + "KMPH  Idle Time: " + (int)IdleTime / 3600 + "H " + (int)IdleTime % 60 + " Min";
                        col = new DataColumn("TotalDistanceTravelled(Kms)");
                        rpttable.Columns.Add(col);
                        col = new DataColumn("WorkingHours");
                        rpttable.Columns.Add(col);
                        col = new DataColumn("MotionHours");
                        rpttable.Columns.Add(col);
                        col = new DataColumn("StationaryHours");
                        rpttable.Columns.Add(col);
                        col = new DataColumn("IdleTime");
                        rpttable.Columns.Add(col);
                        col = new DataColumn("MaxSpeed");
                        rpttable.Columns.Add(col);
                        col = new DataColumn("AvgSpeed");
                        rpttable.Columns.Add(col);
                        col = new DataColumn("A/C ON Time");
                        rpttable.Columns.Add(col);
                        col = new DataColumn("No Of Stops");
                        rpttable.Columns.Add(col);
                        col = new DataColumn("Remarks");
                        rpttable.Columns.Add(col);
                        foreach (string vehiclestr in checkedvhcles)
                        {
                            Maxspeed = 0;
                            #region codefor selected vehicles
                            double SpeedLimit = 0.0;
                            double MaxIdleLimit = 0.0;
                            double MaxStopLimit = 0.0;
                            DataTable logs = new DataTable();
                            DataTable tottable = new DataTable();
                            foreach (string tbname in logstbls)
                            {
                                cmd = new MySqlCommand("SELECT " + tbname + ".VehicleID, " + tbname + ".Speed, " + tbname + ".DateTime, " + tbname + ".Diesel, " + tbname + ".Latitiude, " + tbname + ".Longitude, " + tbname + ".TimeInterval, " + tbname + ".inp4, " + tbname + ".Status, " + tbname + ".Odometer, " + tbname + ".Direction, " + tbname + ".Direction AS Expr1, vehiclemaster.VendorNo, vehiclemaster.VendorName, vehiclemaster.VehicleTypeName, vehiclemaster.MaintenancePlantName FROM " + tbname + " LEFT OUTER JOIN vehiclemaster ON " + tbname + ".VehicleID = vehiclemaster.VehicleID AND " + tbname + ".UserID = vehiclemaster.UserName WHERE (" + tbname + ".DateTime >= @starttime) AND (" + tbname + ".DateTime <= @endtime) AND (" + tbname + ".VehicleID = '" + vehiclestr + "') and (" + tbname + ".UserID='" + mainuser + "') ORDER BY " + tbname + ".DateTime");
                                //cmd = new MySqlCommand("select * from " + tbname + " where DateTime>= @starttime and DateTime<=@endtime and VehicleID='" + vehiclestr + "' and UserID='" + UserName + "' order by DateTime");
                                cmd.Parameters.Add(new MySqlParameter("@starttime", fromdate));
                                cmd.Parameters.Add(new MySqlParameter("@endtime", todate));
                                logs = vdm.SelectQuery(cmd).Tables[0];
                                if (tottable.Rows.Count == 0)
                                {
                                    tottable = logs.Clone();
                                }
                                foreach (DataRow dr in logs.Rows)
                                {
                                    tottable.ImportRow(dr);
                                }
                            }
                            DataView dv = tottable.DefaultView;
                            dv.Sort = "DateTime ASC";
                            table = dv.ToTable();
                            reportData.Add(vehiclestr, table);


                            double lat = 0.0;
                            double longi = 0.0;
                            double prvlat = 0.0;
                            double prevLongi = 0.0;
                            double TotalDistance = 0.0;
                            double IdleTime = 0.0;
                            double TotalTimeSpent = 0.0;
                            double StopTime = 0.0;
                            double totalStops = 0.0;
                            double RunningTime = 0.0;
                            double TotalACTime = 0.0;

                            bool onceMet = false;
                            bool IdleStarted = false;
                            bool runningStarted = false;
                            bool StopStarted = false;
                            bool SpentStarttime = false;
                            bool IsDisplayed = false;
                            bool ACStatred = false;

                            DateTime PrvIdletime = DateTime.Now;
                            DateTime presIdletime = DateTime.Now;
                            DateTime PrvRunningtime = DateTime.Now;
                            DateTime PresRunningtime = DateTime.Now;
                            DateTime PrvStoptime = DateTime.Now;
                            DateTime PresStoptime = DateTime.Now;
                            DateTime PresSpenttime = DateTime.Now;
                            DateTime PrvSpenttime = DateTime.Now;
                            DateTime PresACOnTime = DateTime.Now;
                            DateTime PrvACOnTime = DateTime.Now;

                            DateTime PrevGenTime = DateTime.Now;
                            string vehicleEnteredDate = "";
                            string vehicleLeftDate = "";
                            string Remarks = "No";
                            string PrvBranch = "";
                            string PresBranchName = "";


                            DataRow firstrow = null;
                            DataRow lastrow = null;
                            if (table.Rows.Count > 1)
                            {
                                firstrow = table.Rows[0];
                                lastrow = table.Rows[table.Rows.Count - 1];



                                foreach (DataRow dr1 in table.Rows)
                                {
                                    int AC = 0;
                                    int.TryParse(dr1["inp4"].ToString(), out AC);

                                    if (lat == 0.0 && longi == 0.0)
                                    {
                                        lat = (double)dr1["Latitiude"];
                                        longi = (double)dr1["Longitude"];
                                        prvlat = lat;
                                        prevLongi = longi;
                                        TotalDistance = 0.0;
                                        PrevGenTime = (DateTime)dr1["DateTime"];

                                        if (AC == 1)
                                        {
                                            PrvACOnTime = (DateTime)dr1["DateTime"];
                                            PresACOnTime = (DateTime)dr1["DateTime"];
                                            ACStatred = true;
                                        }
                                        else
                                        {
                                            ACStatred = false;
                                        }
                                    }
                                    else
                                    {

                                        #region Calculations
                                        lat = (double)dr1["Latitiude"];
                                        longi = (double)dr1["Longitude"];
                                        TotalDistance += GeoCodeCalc.CalcDistance(lat, longi, prvlat, prevLongi);
                                        prvlat = lat;
                                        prevLongi = longi;

                                        double deisel = (double)dr1["Diesel"];
                                        double speed = (double)dr1["Speed"];
                                        if (deisel > 50 && speed == 0)
                                        {
                                            runningStarted = false;
                                            StopStarted = false;
                                            if (IdleStarted)
                                            {
                                                presIdletime = (DateTime)dr1["DateTime"];
                                                TimeSpan t = new TimeSpan(presIdletime.Ticks);//presIdletime.Hour, presIdletime.Minute, presIdletime.Second);
                                                TimeSpan t1 = new TimeSpan(PrvIdletime.Ticks);//PrvIdletime.Hour, PrvIdletime.Minute, PrvIdletime.Second);
                                                IdleTime += Math.Abs(t.Subtract(t1).TotalSeconds);
                                                PrvIdletime = presIdletime;
                                                PrevGenTime = presIdletime;
                                                if (IdleTime > MaxIdleLimit)
                                                    Remarks = "YES";
                                            }
                                            else
                                            {
                                                IdleStarted = true;
                                                PrvIdletime = (DateTime)dr1["DateTime"];
                                                TimeSpan t = new TimeSpan(PrevGenTime.Ticks);//presIdletime.Hour, presIdletime.Minute, presIdletime.Second);
                                                TimeSpan t1 = new TimeSpan(PrvIdletime.Ticks);//PrvIdletime.Hour, PrvIdletime.Minute, PrvIdletime.Second);
                                                IdleTime += Math.Abs(t.Subtract(t1).TotalSeconds);
                                                PrevGenTime = PrvIdletime;
                                            }
                                        }
                                        //else if (deisel > 50 && speed > 0)
                                        else if (speed > 0)
                                        {
                                            IdleStarted = false;
                                            StopStarted = false;
                                            if (runningStarted)
                                            {
                                                PresRunningtime = (DateTime)dr1["DateTime"];
                                                TimeSpan t = new TimeSpan(PresRunningtime.Ticks);//PresRunningtime.Hour, PresRunningtime.Minute, PresRunningtime.Second);
                                                TimeSpan t1 = new TimeSpan(PrvRunningtime.Ticks);//PrvRunningtime.Hour, PrvRunningtime.Minute, PrvRunningtime.Second);
                                                RunningTime += Math.Abs(t.Subtract(t1).TotalSeconds);
                                                if (speed > Maxspeed)
                                                    Maxspeed = speed;

                                                totalSpeed += speed;
                                                PrvRunningtime = PresRunningtime;
                                                PrevGenTime = PresRunningtime;
                                            }
                                            else
                                            {
                                                runningStarted = true;
                                                PrvRunningtime = (DateTime)dr1["DateTime"];
                                                TimeSpan t = new TimeSpan(PrevGenTime.Ticks);//presIdletime.Hour, presIdletime.Minute, presIdletime.Second);
                                                TimeSpan t1 = new TimeSpan(PrvRunningtime.Ticks);//PrvIdletime.Hour, PrvIdletime.Minute, PrvIdletime.Second);
                                                RunningTime += Math.Abs(t.Subtract(t1).TotalSeconds);
                                                totalSpeed += speed;
                                                if (speed > Maxspeed)
                                                    Maxspeed = speed;
                                                PrevGenTime = PrvRunningtime;
                                            }
                                        }
                                        else if (deisel < 50 && speed == 0)
                                        {
                                            IdleStarted = false;
                                            runningStarted = false;
                                            if (StopStarted)
                                            {
                                                PresStoptime = (DateTime)dr1["DateTime"];
                                                TimeSpan t = new TimeSpan(PresStoptime.Ticks);//PresStoptime.Hour, PresStoptime.Minute, PresStoptime.Second);
                                                TimeSpan t1 = new TimeSpan(PrvStoptime.Ticks);//PrvStoptime.Hour, PrvStoptime.Minute, PrvStoptime.Second);
                                                StopTime += Math.Abs(t.Subtract(t1).TotalSeconds);
                                                PrvStoptime = PresStoptime;
                                                //if (speed > MaxStopLimit)
                                                //    Remarks = "YES-stopped";
                                                PrevGenTime = PresStoptime;
                                            }
                                            else
                                            {
                                                StopStarted = true;
                                                PrvStoptime = (DateTime)dr1["DateTime"];
                                                TimeSpan t = new TimeSpan(PrevGenTime.Ticks);//presIdletime.Hour, presIdletime.Minute, presIdletime.Second);
                                                TimeSpan t1 = new TimeSpan(PrvStoptime.Ticks);//PrvIdletime.Hour, PrvIdletime.Minute, PrvIdletime.Second);
                                                StopTime += Math.Abs(t.Subtract(t1).TotalSeconds);
                                                PrevGenTime = PrvStoptime;
                                                totalStops += 1;
                                            }
                                        }
                                        #endregion


                                        if (AC == 1)
                                        {
                                            if (ACStatred == false)

                                                PrvACOnTime = (DateTime)dr1["DateTime"];
                                            PresACOnTime = (DateTime)dr1["DateTime"];

                                            ACStatred = true;
                                        }
                                        else
                                        {
                                            ACStatred = false;
                                        }

                                        if (ACStatred)
                                        {
                                            PresACOnTime = (DateTime)dr1["DateTime"];
                                            TimeSpan t = new TimeSpan(PresACOnTime.Ticks);//presIdletime.Hour, presIdletime.Minute, presIdletime.Second);
                                            TimeSpan t1 = new TimeSpan(PrvACOnTime.Ticks);
                                            TotalACTime += Math.Abs(t.Subtract(t1).TotalSeconds);
                                            PrvACOnTime = PresACOnTime;
                                        }
                                    }
                                }


                                if (firstrow != null && lastrow != null)
                                {
                                    double firstval = 0;
                                    double.TryParse(firstrow["Odometer"].ToString(), out firstval);
                                    double lastval = 0;
                                    double.TryParse(lastrow["Odometer"].ToString(), out lastval);
                                    if (lastval > 0 && firstval > 0)
                                        TotalDistance = lastval - firstval;
                                }

                                double avgspeeddiv = (RunningTime / 3600);
                                double avgspeed = 0;
                                if (avgspeeddiv > 0)
                                    avgspeed = TotalDistance / avgspeeddiv;
                                DataRow tablerow = rpttable.NewRow();
                                cmd = new MySqlCommand("SELECT SNo, VehicleID, RouteName, RouteCode FROM cabmanagement WHERE (VehicleID = @VehicleID)");
                                                                    cmd.Parameters.Add(new MySqlParameter("@VehicleID", vehiclestr));
                                DataTable dtRoute = vdm.SelectQuery(cmd).Tables[0];
                                string RouteName = "";
                                if (dtRoute.Rows.Count > 0)
                                {
                                    RouteName = dtRoute.Rows[0]["RouteCode"].ToString();
                                }
                                tablerow["Route Name"] = RouteName;
                                tablerow["VehicleID"] = vehiclestr;
                                tablerow["TotalDistanceTravelled(Kms)"] = (Math.Abs(Math.Round(TotalDistance, 3))).ToString();
                                tablerow["WorkingHours"] = (int)(RunningTime + IdleTime) / 3600 + "H " + (int)((RunningTime + IdleTime) % (60)) + "Min";
                                tablerow["MotionHours"] = (int)RunningTime / 3600 + "H " + (int)RunningTime % (60) + "Min";
                                tablerow["StationaryHours"] = (int)StopTime / 3600 + "H " + (int)StopTime % 60 + "Min";
                                //tablerow["Stopped Time"]=
                                tablerow["MaxSpeed"] = (int)Maxspeed + "KMPH";
                                tablerow["AvgSpeed"] = String.Format("{0:0.00}", avgspeed) + "KMPH";
                                tablerow["IdleTime"] = (int)IdleTime / 3600 + "H " + (int)IdleTime % 60 + " Min";
                                tablerow["No Of Stops"] = totalStops.ToString();
                                tablerow["A/C ON Time"] = (int)TotalACTime / 3600 + "H " + (int)TotalACTime % 60 + " Min";//.ToString();
                                rpttable.Rows.Add(tablerow);
                            #endregion
                            }
                        }
                        Session["reportdata"] = reportData;
                        lbl_ReportStatus.Text = "General Report From: " + fromdate.ToString() + "  To: " + todate.ToString();// +" and  TotalDistance Travelled:" + (int)TotalDistance + "\n" +
                        Session["title"] = lbl_ReportStatus.Text;
                        grdReports.DataSource = rpttable;
                        Session["xportdata"] = rpttable;
                        grdReports.DataBind();
                        divPieChart.Visible = false;
                        #endregion
                    }
                    else if (reportname == "Stopage Report")
                    {
                        #region Stoppage Report
                        string vehicls = "";
                        DataTable Stoppagereport = new DataTable();
                        DataTable dtble = new DataTable();
                        foreach (string vehiclestr in checkedvhcles)
                        {
                            DataTable logs = new DataTable();
                            DataTable tottable = new DataTable();
                            foreach (string tbname in logstbls)
                            {
                                cmd = new MySqlCommand("select * from " + tbname + " where DateTime>= @starttime and DateTime<=@endtime and VehicleID='" + vehiclestr + "' and UserID='" + mainuser + "' order by DateTime");
                                //cmd = new MySqlCommand("select * from " + tbname + " where DateTime>= @starttime and DateTime<=@endtime and VehicleID='" + vehiclestr + "' and UserID='" + UserName + "' order by DateTime");
                                cmd.Parameters.Add(new MySqlParameter("@starttime", fromdate));
                                cmd.Parameters.Add(new MySqlParameter("@endtime", todate));
                                logs = vdm.SelectQuery(cmd).Tables[0];
                                if (tottable.Rows.Count == 0)
                                {
                                    tottable = logs.Clone();
                                }
                                foreach (DataRow dr in logs.Rows)
                                {
                                    tottable.ImportRow(dr);
                                }
                            }
                            DataView dv = tottable.DefaultView;
                            dv.Sort = "DateTime ASC";
                            dtble = dv.ToTable();
                            reportData.Add(vehiclestr, table);

                            Stoppagereport = new DataTable();
                            Stoppagereport.Columns.Add("SNo");
                            Stoppagereport.Columns.Add("DateTime");
                            Stoppagereport.Columns.Add("Place");
                            Stoppagereport.Columns.Add("Stopped Hours");
                            Stoppagereport.Columns.Add("VehicleID");

                            lbl_ReportStatus.Text = "VEHICLE STOPPAGE REPORT FROM: " + fromdate.ToString() + "  To: " + todate.ToString() + " FOR MORE THAN " + txt_Reports_TimeGap.Text + " Min";
                            Session["title"] = lbl_ReportStatus.Text;
                            DateTime pdt = new DateTime();
                            bool first = true;
                            int minutes = 0;
                            int.TryParse(txt_Reports_TimeGap.Text, out minutes);
                            int a = 1;
                            for (int i = 0; i < dtble.Rows.Count; i++)
                            {
                                DataRow row = dtble.Rows[i];
                                DateTime latdat = new DateTime();

                                latdat = (DateTime)row["DateTime"]; //new DateTime(int.Parse(datevalues[2]), int.Parse(datevalues[1]), int.Parse(datevalues[0]), int.Parse(timevalues[0]), int.Parse(timevalues[1]), int.Parse(timevalues[2]));
                                TimeSpan ts1 = new TimeSpan(latdat.Ticks);
                                if (first)
                                {
                                    pdt = latdat;
                                    first = false;
                                }
                                TimeSpan ts2 = new TimeSpan(pdt.Ticks);
                                TimeSpan ts3 = ts1.Subtract(ts2);

                                if (row["Speed"].ToString() == "0")
                                {
                                    if (ts3.TotalMinutes >= minutes)
                                    {

                                        DataRow newrow = Stoppagereport.NewRow();
                                        newrow["SNo"] = a.ToString();
                                        newrow["DateTime"] = latdat;

                                        GP1 = new GooglePoint();
                                        GP1.Latitude = (double)row["Latitiude"];
                                        GP1.Longitude = (double)row["Longitude"];

                                        GooglePoint g1 = new GooglePoint();
                                        g1.Latitude = (double)row["Latitiude"];
                                        g1.Longitude = (double)row["Longitude"];

                                        g1.Address = g1.Latitude + "," + g1.Longitude;
                                        //bool val1 = g1.GeocodeAddress("AIzaSyCpF601309Z6DtpdtiHUce95wmkjVTmoGg");

                                        newrow["Place"] = g1.Address;
                                        //newrow["Latitiude"] = (double)row["Latitiude"];
                                        //newrow["Longitude"] = (double)row["Longitude"];
                                        //newrow["Direction"] = "0";
                                        //newrow["Speed"] = "0";
                                        newrow["VehicleID"] = row["VehicleID"];

                                        newrow["Stopped Hours"] = (int)(ts3.TotalHours % 24) + "Hours " + (int)(ts3.TotalMinutes % 60) + "Min ";

                                        Stoppagereport.Rows.Add(newrow);
                                        a++;
                                    }
                                }
                                pdt = latdat;
                            }
                        }
                        grdReports.DataSource = Stoppagereport;
                        Session["xportdata"] = Stoppagereport;

                        grdReports.DataBind();
                        divPieChart.Visible = false;
                        for (int i = 0; i < grdReports.Rows.Count; i++)
                        {
                            //grdReports.Rows[i].Cells[1].Text = (i+1).ToString();
                            grdReports.Rows[i].Cells[0].Controls.Remove(grdReports.Rows[i].Cells[0].Controls[1]);
                        }
                        #endregion
                    }
                    else if (reportname == "OverSpeed Report")
                    {
                        #region Overspeed Report

                        float spd = 0;
                        int cot = 1;
                        float.TryParse(txt_Reports_TimeGap.Text, out spd);
                        if (spd < 1)
                        {
                            MessageBox.Show("Speed Limit must be Specified", this);
                        }
                        else
                        {
                            var checkedstring = "";
                            foreach (string vehiclestr in checkedvhcles)
                            {
                                checkedstring += "'" + vehiclestr + "',";
                            }
                            checkedstring = checkedstring.Remove(checkedstring.Length - 1);
                            lbl_ReportStatus.Text = "";
                            Session["title"] = lbl_ReportStatus.Text;
                            DataTable SpeedReport = new DataTable();
                            DataTable logs = new DataTable();
                            DataTable tottable = new DataTable();
                            foreach (string tbname in logstbls)
                            {
                                cmd = new MySqlCommand("SELECT '' AS SNo, " + tbname + ".VehicleID, " + tbname + ".DateTime, " + tbname + ".Speed, vehiclemaster.MaintenancePlantName AS MPLName, vehiclemaster.VendorName, vehiclemaster.VendorNo, vehiclemaster.VehicleTypeName AS VehicleType FROM " + tbname + " LEFT OUTER JOIN vehiclemaster ON " + tbname + ".VehicleID = vehiclemaster.VehicleID WHERE (" + tbname + ".DateTime >= @starttime) AND (" + tbname + ".DateTime <= @endtime) AND (" + tbname + ".VehicleID IN (" + checkedstring + ")) AND (" + tbname + ".Speed > @Speed) and (" + tbname + ".UserID='" + mainuser + "')  ORDER BY " + tbname + ".DateTime");
                                cmd.Parameters.Add(new MySqlParameter("@starttime", fromdate));
                                cmd.Parameters.Add(new MySqlParameter("@endtime", todate));
                                cmd.Parameters.Add(new MySqlParameter("@Speed", spd));
                                logs = vdm.SelectQuery(cmd).Tables[0];
                                if (tottable.Rows.Count == 0)
                                {
                                    tottable = logs.Clone();
                                }
                                foreach (DataRow dr in logs.Rows)
                                {
                                    tottable.ImportRow(dr);
                                }
                            }
                            DataView dv = tottable.DefaultView;
                            dv.Sort = "DateTime ASC";
                            SpeedReport = dv.ToTable();

                            for (int i = 0; i < SpeedReport.Rows.Count; i++)
                            {
                                SpeedReport.Rows[i]["SNo"] = (cot);
                                cot++;
                            }
                            grdReports.DataSource = SpeedReport;
                            Session["xportdata"] = SpeedReport;

                            grdReports.DataBind();
                            divPieChart.Visible = false;
                            for (int i = 0; i < grdReports.Rows.Count; i++)
                            {
                                grdReports.Rows[i].Cells[0].Controls.Remove(grdReports.Rows[i].Cells[0].Controls[1]);
                            }
                            lbl_ReportStatus.Text = "Report From: " + fromdate.ToString() + "  To: " + todate.ToString() + " Maximum Speed: " + SpeedReport.Compute("Max(Speed)", "Speed>0") + "KMPH";// +" and  TotalDistance Travelled:" + (int)TotalDistance + "\n" +
                        }

                        #endregion
                    }
                    else if (reportname == "Daily Report")
                    {
                        #region "Daily Report"
                        string Duration = "";
                        string StDuration = "";
                        DateTime Stopdt = DateTime.Now;
                        DateTime Startdt = DateTime.Now;
                        reportData = new Dictionary<string, DataTable>();
                        DataTable dailydatatable = new DataTable();
                        dailydatatable.Columns.Add("StartDate");
                        dailydatatable.Columns.Add("StartTime");
                        dailydatatable.Columns.Add("StopDate");
                        dailydatatable.Columns.Add("StopTime");
                        dailydatatable.Columns.Add("TotalDistanceTravelled(Kms)");
                        dailydatatable.Columns.Add("MotionHours");
                        dailydatatable.Columns.Add("StationaryHours");
                        dailydatatable.Columns.Add("MaxSpeed");
                        dailydatatable.Columns.Add("AvgSpeed");
                        dailydatatable.Columns.Add("IdleTime");
                        dailydatatable.Columns.Add("ACONTime");
                        lbl_ReportStatus.Text = "DAILY REPORT FROM: " + fromdate.ToString("M/dd/yyyy") + "  To: " + todate.ToString("M/dd/yyyy");
                        int cont = 1;
                        if (cont > 0)
                        {
                            #region multyvehicles
                            foreach (string vehiclestr in checkedvhcles)
                            {
                                DataTable logs = new DataTable();
                                DataTable tottable = new DataTable();
                                foreach (string tbname in logstbls)
                                {
                                    cmd = new MySqlCommand("SELECT " + tbname + ".VehicleID, " + tbname + ".Speed, " + tbname + ".DateTime, " + tbname + ".Diesel, " + tbname + ".Latitiude, " + tbname + ".Longitude, " + tbname + ".TimeInterval, " + tbname + ".inp4, " + tbname + ".Status, " + tbname + ".Odometer, " + tbname + ".Direction, " + tbname + ".Direction AS Expr1, vehiclemaster.VendorNo, vehiclemaster.VendorName, vehiclemaster.VehicleTypeName, vehiclemaster.MaintenancePlantName FROM " + tbname + " LEFT OUTER JOIN vehiclemaster ON " + tbname + ".VehicleID = vehiclemaster.VehicleID AND " + tbname + ".UserID = vehiclemaster.UserName WHERE (" + tbname + ".DateTime >= @starttime) AND (" + tbname + ".DateTime <= @endtime) AND (" + tbname + ".VehicleID = '" + vehiclestr + "') and (" + tbname + ".UserID='" + mainuser + "') ORDER BY " + tbname + ".DateTime");
                                    //cmd = new MySqlCommand("select * from " + tbname + " where DateTime>= @starttime and DateTime<=@endtime and VehicleID='" + vehiclestr + "' and UserID='" + UserName + "' order by DateTime");
                                    cmd.Parameters.Add(new MySqlParameter("@starttime", fromdate));
                                    cmd.Parameters.Add(new MySqlParameter("@endtime", todate));
                                    logs = vdm.SelectQuery(cmd).Tables[0];
                                    if (tottable.Rows.Count == 0)
                                    {
                                        tottable = logs.Clone();
                                    }
                                    foreach (DataRow dr in logs.Rows)
                                    {
                                        tottable.ImportRow(dr);
                                    }
                                }
                                DataView dv = tottable.DefaultView;
                                dv.Sort = "DateTime ASC";
                                table = dv.ToTable();
                                if (table.Rows.Count > 0)
                                {
                                    DataRow newrow = dailydatatable.NewRow();
                                    newrow["MotionHours"] = vehiclestr;
                                    dailydatatable.Rows.Add(newrow);


                                    for (DateTime date = fromdate; GetHighDate(todate).CompareTo(GetLowDate(date)) > 0; date = date.AddDays(1.0))
                                    {
                                        double SpeedLimit = 0.0;
                                        double MaxIdleLimit = 0.0;
                                        double MaxStopLimit = 0.0;
                                        Maxspeed = 0;
                                        double lat = 0.0;
                                        double longi = 0.0;
                                        double prvlat = 0.0;
                                        double prevLongi = 0.0;
                                        double TotalDistance = 0.0;
                                        double IdleTime = 0.0;
                                        double TotalTimeSpent = 0.0;
                                        double StopTime = 0.0;
                                        double RunningTime = 0.0;
                                        bool onceMet = false;
                                        bool IdleStarted = false;
                                        bool runningStarted = false;
                                        bool StopStarted = false;
                                        bool SpentStarttime = false;
                                        bool IsDisplayed = false;
                                        bool ACStatred = false;
                                        double TotalACTime = 0.0;

                                        DateTime PrvIdletime = DateTime.Now;
                                        DateTime presIdletime = DateTime.Now;
                                        DateTime PrvRunningtime = DateTime.Now;
                                        DateTime PresRunningtime = DateTime.Now;
                                        DateTime PrvStoptime = DateTime.Now;
                                        DateTime PresStoptime = DateTime.Now;
                                        DateTime PresSpenttime = DateTime.Now;
                                        DateTime PrvSpenttime = DateTime.Now;
                                        DateTime PresACOnTime = DateTime.Now;
                                        DateTime PrvACOnTime = DateTime.Now;

                                        DateTime PrevGenTime = DateTime.Now;
                                        string vehicleEnteredDate = "";
                                        string vehicleLeftDate = "";
                                        string Remarks = "No";
                                        string PrvBranch = "";
                                        string PresBranchName = "";
                                        DataTable daydatatable = table.Clone();
                                        DataRow[] dailyreport = table.Select("DateTime>='" + GetLowDate(date) + "' and DateTime<='" + GetHighDate(date) + "'");
                                        foreach (DataRow row in dailyreport)
                                        {
                                            daydatatable.ImportRow(row);
                                        }
                                        reportData.Add(dailydatatable.Rows.Count.ToString(), daydatatable);


                                        DataRow firstrow = null;
                                        DataRow lastrow = null;
                                        if (dailyreport.Length > 1)
                                        {
                                            firstrow = dailyreport[0];
                                            lastrow = dailyreport[dailyreport.Length - 1];

                                            DateTime starttime = GetLowDate(date);
                                            DateTime stoptime = GetHighDate(date);
                                            bool startflag = false;
                                            bool stopflag = false;
                                            foreach (DataRow dr1 in dailyreport)
                                            {
                                                if (double.Parse(dr1["Speed"].ToString()) > 10)
                                                {
                                                    if (!startflag)
                                                    {
                                                        starttime = (DateTime)dr1["DateTime"];
                                                        startflag = true;
                                                    }
                                                }

                                                if (double.Parse(dr1["Speed"].ToString()) == 0)
                                                {
                                                    stoptime = (DateTime)dr1["DateTime"];
                                                    stopflag = true;
                                                }
                                                else
                                                {
                                                    stopflag = false;
                                                }

                                                int AC = 0;
                                                int.TryParse(dr1["inp4"].ToString(), out AC);
                                                if (lat == 0.0 && longi == 0.0)
                                                {
                                                    lat = (double)dr1["Latitiude"];
                                                    longi = (double)dr1["Longitude"];
                                                    prvlat = lat;
                                                    prevLongi = longi;
                                                    TotalDistance = 0.0;
                                                    PrevGenTime = (DateTime)dr1["DateTime"];



                                                    if (AC == 1)
                                                    {
                                                        PrvACOnTime = (DateTime)dr1["DateTime"];
                                                        PresACOnTime = (DateTime)dr1["DateTime"];
                                                        ACStatred = true;
                                                    }
                                                    else
                                                    {
                                                        ACStatred = false;
                                                    }
                                                }
                                                else
                                                {
                                                    #region calculations
                                                    lat = (double)dr1["Latitiude"];
                                                    longi = (double)dr1["Longitude"];
                                                    TotalDistance += GeoCodeCalc.CalcDistance(lat, longi, prvlat, prevLongi);
                                                    prvlat = lat;
                                                    prevLongi = longi;

                                                    double deisel = (double)dr1["Diesel"];
                                                    double speed = (double)dr1["Speed"];
                                                    if (deisel > 50 && speed == 0)
                                                    {
                                                        runningStarted = false;
                                                        StopStarted = false;
                                                        if (IdleStarted)
                                                        {
                                                            presIdletime = (DateTime)dr1["DateTime"];
                                                            TimeSpan t = new TimeSpan(presIdletime.Ticks);//presIdletime.Hour, presIdletime.Minute, presIdletime.Second);
                                                            TimeSpan t1 = new TimeSpan(PrvIdletime.Ticks);//PrvIdletime.Hour, PrvIdletime.Minute, PrvIdletime.Second);
                                                            IdleTime += Math.Abs(t.Subtract(t1).TotalSeconds);
                                                            PrvIdletime = presIdletime;
                                                            PrevGenTime = presIdletime;
                                                            if (IdleTime > MaxIdleLimit)
                                                                Remarks = "YES";
                                                        }
                                                        else
                                                        {
                                                            IdleStarted = true;
                                                            PrvIdletime = (DateTime)dr1["DateTime"];
                                                            TimeSpan t = new TimeSpan(PrevGenTime.Ticks);//presIdletime.Hour, presIdletime.Minute, presIdletime.Second);
                                                            TimeSpan t1 = new TimeSpan(PrvIdletime.Ticks);//PrvIdletime.Hour, PrvIdletime.Minute, PrvIdletime.Second);
                                                            IdleTime += Math.Abs(t.Subtract(t1).TotalSeconds);
                                                            PrevGenTime = PrvIdletime;
                                                        }
                                                    }
                                                    //else if (deisel > 50 && speed > 0)
                                                    else if (speed > 0)
                                                    {
                                                        IdleStarted = false;
                                                        StopStarted = false;
                                                        if (runningStarted)
                                                        {
                                                            PresRunningtime = (DateTime)dr1["DateTime"];
                                                            TimeSpan t = new TimeSpan(PresRunningtime.Ticks);//PresRunningtime.Hour, PresRunningtime.Minute, PresRunningtime.Second);
                                                            TimeSpan t1 = new TimeSpan(PrvRunningtime.Ticks);//PrvRunningtime.Hour, PrvRunningtime.Minute, PrvRunningtime.Second);
                                                            RunningTime += Math.Abs(t.Subtract(t1).TotalSeconds);
                                                            if (speed > Maxspeed)
                                                                Maxspeed = speed;

                                                            totalSpeed += speed;
                                                            //if (speed > MaxStopLimit)
                                                            //    Remarks = "YES";
                                                            PrvRunningtime = PresRunningtime;
                                                            PrevGenTime = PresRunningtime;
                                                        }
                                                        else
                                                        {
                                                            runningStarted = true;
                                                            PrvRunningtime = (DateTime)dr1["DateTime"];
                                                            TimeSpan t = new TimeSpan(PrevGenTime.Ticks);//presIdletime.Hour, presIdletime.Minute, presIdletime.Second);
                                                            TimeSpan t1 = new TimeSpan(PrvRunningtime.Ticks);//PrvIdletime.Hour, PrvIdletime.Minute, PrvIdletime.Second);
                                                            RunningTime += Math.Abs(t.Subtract(t1).TotalSeconds);
                                                            totalSpeed += speed;
                                                            if (speed > Maxspeed)
                                                                Maxspeed = speed;
                                                            PrevGenTime = PrvRunningtime;
                                                        }
                                                    }
                                                    else if (deisel < 50 && speed == 0)
                                                    {
                                                        IdleStarted = false;
                                                        runningStarted = false;
                                                        if (StopStarted)
                                                        {
                                                            PresStoptime = (DateTime)dr1["DateTime"];
                                                            TimeSpan t = new TimeSpan(PresStoptime.Ticks);//PresStoptime.Hour, PresStoptime.Minute, PresStoptime.Second);
                                                            TimeSpan t1 = new TimeSpan(PrvStoptime.Ticks);//PrvStoptime.Hour, PrvStoptime.Minute, PrvStoptime.Second);
                                                            StopTime += Math.Abs(t.Subtract(t1).TotalSeconds);
                                                            PrvStoptime = PresStoptime;
                                                            //if (speed > MaxStopLimit)
                                                            //    Remarks = "YES-stopped";
                                                            PrevGenTime = PresStoptime;
                                                        }
                                                        else
                                                        {
                                                            StopStarted = true;
                                                            PrvStoptime = (DateTime)dr1["DateTime"];
                                                            TimeSpan t = new TimeSpan(PrevGenTime.Ticks);//presIdletime.Hour, presIdletime.Minute, presIdletime.Second);
                                                            TimeSpan t1 = new TimeSpan(PrvStoptime.Ticks);//PrvIdletime.Hour, PrvIdletime.Minute, PrvIdletime.Second);
                                                            StopTime += Math.Abs(t.Subtract(t1).TotalSeconds);
                                                            PrevGenTime = PrvStoptime;
                                                        }
                                                    }
                                                    #endregion

                                                    if (AC == 1)
                                                    {
                                                        if (ACStatred == false)

                                                            PrvACOnTime = (DateTime)dr1["DateTime"];
                                                        PresACOnTime = (DateTime)dr1["DateTime"];

                                                        ACStatred = true;
                                                    }
                                                    else
                                                    {
                                                        ACStatred = false;
                                                    }

                                                    if (ACStatred)
                                                    {
                                                        PresACOnTime = (DateTime)dr1["DateTime"];
                                                        TimeSpan t = new TimeSpan(PresACOnTime.Ticks);//presIdletime.Hour, presIdletime.Minute, presIdletime.Second);
                                                        TimeSpan t1 = new TimeSpan(PrvACOnTime.Ticks);
                                                        TotalACTime += Math.Abs(t.Subtract(t1).TotalSeconds);
                                                        PrvACOnTime = PresACOnTime;
                                                    }
                                                }
                                            }
                                            //datareport = new DataReport();

                                            if (firstrow != null && lastrow != null)
                                            {
                                                double firstval = 0;
                                                double.TryParse(firstrow["Odometer"].ToString(), out firstval);
                                                double lastval = 0;
                                                double.TryParse(lastrow["Odometer"].ToString(), out lastval);
                                                if (lastval > 0 && firstval > 0)
                                                    TotalDistance = lastval - firstval;
                                            }


                                            double avgspeeddiv = (RunningTime / 3600);
                                            double avgspeed = 0;
                                            if (avgspeeddiv > 0)
                                                avgspeed = TotalDistance / avgspeeddiv;

                                            TimeSpan ts1 = new TimeSpan(starttime.Ticks);
                                            TimeSpan ts2 = new TimeSpan(GetLowDate(date).Ticks);
                                            TimeSpan ts3 = new TimeSpan(GetHighDate(date).Ticks);
                                            TimeSpan ts4 = new TimeSpan(DateTime.Parse(lastrow["DateTime"].ToString()).Ticks);
                                            double ts5 = ts1.Subtract(ts2).TotalSeconds;
                                            double ts6 = ts3.Subtract(ts4).TotalSeconds;
                                            double stationaryhours = ts5 + ts6;

                                            double totalstationaryhours = stationaryhours + StopTime;
                                            DataRow insntrow = dailydatatable.NewRow();
                                            //insntrow["SNo"] = dailydatatable.Rows.Count + 1;
                                            //insntrow["VehicleNo"] = VehicleID;
                                            Startdt = starttime;
                                            string Startdate = Startdt.ToString("M/dd/yyyy");
                                            string StartTime = Startdt.ToString("hh:mm:ss tt");
                                            insntrow["StartDate"] = Startdate;

                                            string[] Reachsplt = StartTime.ToString().Split(' ');
                                            if (Reachsplt.Length > 1)
                                            {
                                                int departuretimemin = 0;
                                                int dephours = 0;
                                                int depmin = 0;
                                                int.TryParse(Reachsplt[0].Split(':')[0], out dephours);
                                                int.TryParse(Reachsplt[0].Split(':')[1], out depmin);
                                                //departuretimemin = 720 - ((dephours * 60) + depmin);

                                                if (Reachsplt[1] == "PM")
                                                {
                                                    if (Reachsplt[0].Split(':')[0] == "12")
                                                        departuretimemin = ((dephours * 60) + depmin);
                                                    else
                                                        departuretimemin = 720 + ((dephours * 60) + depmin);
                                                }
                                                else
                                                {
                                                    if (Reachsplt[0].Split(':')[0] == "12")
                                                        departuretimemin = ((dephours * 60) + depmin) - 720;
                                                    else
                                                        departuretimemin = ((dephours * 60) + depmin);
                                                }

                                                //ddlTravels.Items.Add(dr["traveler_agent"].ToString());

                                                int time = departuretimemin;
                                                if ((time % 60) < 10 && (time / 60) < 10)
                                                {
                                                    Duration = "0" + time / 60 + " : " + "0" + time % 60;
                                                }
                                                else if ((time % 60) >= 10 && (time / 60) < 10)
                                                {
                                                    Duration = "0" + time / 60 + " : " + time % 60;
                                                }
                                                else if ((time % 60) < 10 && (time / 60) >= 10)
                                                {
                                                    Duration = time / 60 + " : " + "0" + time % 60;
                                                }
                                                else if ((time % 60) >= 10 && (time / 60) >= 10)
                                                {
                                                    Duration = time / 60 + " : " + time % 60;
                                                }

                                            }


                                            insntrow["StartTime"] = Duration;
                                            //if(!stopflag)
                                            //    insntrow["StopTime"] = DateConverter.GetHighDate(date);
                                            //else
                                            //    insntrow["StopTime"] = stoptime;
                                            Stopdt = (DateTime)lastrow["DateTime"];
                                            string Stopdate = Stopdt.ToString("M/dd/yyyy");
                                            string StoopTime = Stopdt.ToString("hh:mm:ss tt");
                                            insntrow["StopDate"] = Stopdate;
                                            string[] Stopsplt = StoopTime.ToString().Split(' ');
                                            if (Stopsplt.Length > 1)
                                            {
                                                int departuretimemin = 0;
                                                int dephours = 0;
                                                int depmin = 0;
                                                int.TryParse(Stopsplt[0].Split(':')[0], out dephours);
                                                int.TryParse(Stopsplt[0].Split(':')[1], out depmin);
                                                //departuretimemin = 720 - ((dephours * 60) + depmin);

                                                if (Stopsplt[1] == "PM")
                                                {
                                                    if (Stopsplt[0].Split(':')[0] == "12")
                                                        departuretimemin = ((dephours * 60) + depmin);
                                                    else
                                                        departuretimemin = 720 + ((dephours * 60) + depmin);
                                                }
                                                else
                                                {
                                                    if (Stopsplt[0].Split(':')[0] == "12")
                                                        departuretimemin = ((dephours * 60) + depmin) - 720;
                                                    else
                                                        departuretimemin = ((dephours * 60) + depmin);
                                                }


                                                //ddlTravels.Items.Add(dr["traveler_agent"].ToString());

                                                int time = departuretimemin;
                                                if ((time % 60) < 10 && (time / 60) < 10)
                                                {
                                                    StDuration = "0" + time / 60 + " : " + "0" + time % 60;
                                                }
                                                else if ((time % 60) >= 10 && (time / 60) < 10)
                                                {
                                                    StDuration = "0" + time / 60 + " : " + time % 60;
                                                }
                                                else if ((time % 60) < 10 && (time / 60) >= 10)
                                                {
                                                    StDuration = time / 60 + " : " + "0" + time % 60;
                                                }
                                                else if ((time % 60) >= 10 && (time / 60) >= 10)
                                                {
                                                    StDuration = time / 60 + " : " + time % 60;
                                                }

                                            }


                                            insntrow["StopTime"] = StDuration;
                                            insntrow["TotalDistanceTravelled(Kms)"] = Math.Round(TotalDistance, 3);
                                            insntrow["MotionHours"] = (int)RunningTime / 3600 + "H " + (int)RunningTime % (60) + " Min";
                                            insntrow["StationaryHours"] = (int)totalstationaryhours / 3600 + " H " + (int)totalstationaryhours % 60 + " Min";
                                            insntrow["MaxSpeed"] = (int)Maxspeed;
                                            insntrow["AvgSpeed"] = String.Format("{0:0.00}", avgspeed);
                                            insntrow["IdleTime"] = (int)IdleTime / 3600 + "H " + (int)IdleTime % 60 + " Min";
                                            insntrow["ACONTime"] = (int)TotalACTime / 3600 + "H " + (int)TotalACTime % 60 + " Min";
                                            dailydatatable.Rows.Add(insntrow);
                                        }
                                        else
                                        {
                                            DataRow insntrow = dailydatatable.NewRow();
                                            //insntrow["SNo"] = dailydatatable.Rows.Count + 1;
                                            //insntrow["VehicleNo"] = VehicleID;
                                            insntrow["StartDate"] = "MOMENT NOT FOUND";
                                            insntrow["StartTime"] = "0 H";
                                            //if(!stopflag)
                                            //    insntrow["StopTime"] = DateConverter.GetHighDate(date);
                                            //else
                                            //    insntrow["StopTime"] = stoptime;
                                            insntrow["StopDate"] = "MOMENT NOT FOUND";
                                            insntrow["StopTime"] = "0 H";
                                            insntrow["TotalDistanceTravelled(Kms)"] = 0;
                                            insntrow["MotionHours"] = "0 H 0 Min";
                                            insntrow["StationaryHours"] = "0 H 0 Min";
                                            insntrow["MaxSpeed"] = 0;
                                            insntrow["AvgSpeed"] = 0;
                                            insntrow["IdleTime"] = "0 H 0 Min";
                                            insntrow["ACONTime"] = "0 H 0 Min";
                                            dailydatatable.Rows.Add(insntrow);

                                        }
                                    }
                                    Session["reportdata"] = reportData;
                                }
                            }
                        }
                        Session["title"] = lbl_ReportStatus.Text;
                        grdReports.DataSource = dailydatatable;
                        Session["xportdata"] = dailydatatable;
                        grdReports.DataBind();
                        divPieChart.Visible = false;

                            #endregion

                        for (int i = 0; i < grdReports.Rows.Count - 1; i++)
                        {
                            if (grdReports.Rows[i].Cells[1].Text == "" || grdReports.Rows[i].Cells[1].Text == "&nbsp;")
                            {
                                grdReports.Rows[i].BackColor = System.Drawing.Color.Khaki;
                                grdReports.Rows[i].Cells[0].Controls.Remove(grdReports.Rows[i].Cells[0].Controls[1]);
                            }
                        }

                        #endregion
                    }
                    else if (reportname == "UnAuthorized Report")
                    {
                        #region UnAuthorised  Report

                        reportData = new Dictionary<string, DataTable>();
                        DataTable dailydatatable = new DataTable();
                        dailydatatable.Columns.Add("StartTime");
                        dailydatatable.Columns.Add("StopTime");
                        dailydatatable.Columns.Add("TotalDistanceTravelled(Kms)");
                        dailydatatable.Columns.Add("MotionHours");
                        dailydatatable.Columns.Add("StationaryHours");
                        dailydatatable.Columns.Add("MaxSpeed");
                        dailydatatable.Columns.Add("AvgSpeed");
                        dailydatatable.Columns.Add("IdleTime");
                        dailydatatable.Columns.Add("ACONTime");

                        foreach (string vehiclestr in checkedvhcles)
                        {
                            TimeSpan DiffFromTime = fromdate - GetLowDate(fromdate);
                            TimeSpan DifftoTime = GetHighDate(todate) - todate;
                            DataTable logs = new DataTable();
                            DataTable tottable = new DataTable();
                            foreach (string tbname in logstbls)
                            {
                                cmd = new MySqlCommand("select * from " + tbname + " WHERE (" + tbname + ".DateTime >= @starttime) AND (" + tbname + ".DateTime <= @endtime) AND (" + tbname + ".VehicleID = '" + vehiclestr + "') and (" + tbname + ".UserID='" + mainuser + "')  ORDER BY " + tbname + ".DateTime");
                                //cmd = new MySqlCommand("select * from " + tbname + " where DateTime>= @starttime and DateTime<=@endtime and VehicleID='" + vehiclestr + "' and UserID='" + UserName + "' order by DateTime");
                                cmd.Parameters.Add(new MySqlParameter("@starttime", fromdate));
                                cmd.Parameters.Add(new MySqlParameter("@endtime", todate));
                                logs = vdm.SelectQuery(cmd).Tables[0];
                                if (tottable.Rows.Count == 0)
                                {
                                    tottable = logs.Clone();
                                }
                                foreach (DataRow dr in logs.Rows)
                                {
                                    tottable.ImportRow(dr);
                                }
                            }
                            DataView dv = tottable.DefaultView;
                            dv.Sort = "DateTime ASC";
                            table = dv.ToTable();

                            DataRow newrow = dailydatatable.NewRow();
                            newrow["MotionHours"] = vehiclestr;
                            dailydatatable.Rows.Add(newrow);

                            if (table.Rows.Count > 0)
                            {
                                for (DateTime date = fromdate; GetHighDate(todate).CompareTo(GetLowDate(date)) > 0; date = date.AddDays(1.0))
                                {
                                    double SpeedLimit = 0.0;
                                    double MaxIdleLimit = 0.0;
                                    double MaxStopLimit = 0.0;
                                    Maxspeed = 0;
                                    double lat = 0.0;
                                    double longi = 0.0;
                                    double prvlat = 0.0;
                                    double prevLongi = 0.0;
                                    double TotalDistance = 0.0;
                                    double IdleTime = 0.0;
                                    double TotalTimeSpent = 0.0;
                                    double StopTime = 0.0;
                                    double RunningTime = 0.0;
                                    bool onceMet = false;
                                    bool IdleStarted = false;
                                    bool runningStarted = false;
                                    bool StopStarted = false;
                                    bool SpentStarttime = false;
                                    bool IsDisplayed = false;
                                    bool ACStatred = false;
                                    double TotalACTime = 0.0;

                                    DateTime PrvIdletime = DateTime.Now;
                                    DateTime presIdletime = DateTime.Now;
                                    DateTime PrvRunningtime = DateTime.Now;
                                    DateTime PresRunningtime = DateTime.Now;
                                    DateTime PrvStoptime = DateTime.Now;
                                    DateTime PresStoptime = DateTime.Now;
                                    DateTime PresSpenttime = DateTime.Now;
                                    DateTime PrvSpenttime = DateTime.Now;
                                    DateTime PresACOnTime = DateTime.Now;
                                    DateTime PrvACOnTime = DateTime.Now;

                                    DateTime PrevGenTime = DateTime.Now;
                                    string vehicleEnteredDate = "";
                                    string vehicleLeftDate = "";
                                    string Remarks = "No";
                                    string PrvBranch = "";
                                    string PresBranchName = "";
                                    DataTable daydatatable = table.Clone();
                                    DataRow[] dailyreport = table.Select("DateTime>='" + GetLowDate(date).AddMinutes(DiffFromTime.TotalMinutes) + "' and DateTime<='" + GetHighDate(date).AddMinutes(-(DifftoTime.TotalMinutes)) + "'");
                                    foreach (DataRow row in dailyreport)
                                    {
                                        daydatatable.ImportRow(row);
                                    }
                                    reportData.Add(dailydatatable.Rows.Count.ToString(), daydatatable);


                                    DataRow firstrow = null;
                                    DataRow lastrow = null;
                                    if (dailyreport.Length > 1)
                                    {
                                        firstrow = dailyreport[0];
                                        lastrow = dailyreport[dailyreport.Length - 1];

                                        DateTime starttime = GetLowDate(date).AddMinutes(DiffFromTime.TotalMinutes);
                                        DateTime stoptime = GetHighDate(date).AddMinutes(-(DifftoTime.TotalMinutes));
                                        bool startflag = false;
                                        bool stopflag = false;
                                        foreach (DataRow dr1 in dailyreport)
                                        {
                                            if (double.Parse(dr1["Speed"].ToString()) > 10)
                                            {
                                                if (!startflag)
                                                {
                                                    starttime = (DateTime)dr1["DateTime"];
                                                    startflag = true;
                                                }
                                            }

                                            if (double.Parse(dr1["Speed"].ToString()) == 0)
                                            {
                                                stoptime = (DateTime)dr1["DateTime"];
                                                stopflag = true;
                                            }
                                            else
                                            {
                                                stopflag = false;
                                            }

                                            int AC = 0;
                                            int.TryParse(dr1["inp4"].ToString(), out AC);
                                            if (lat == 0.0 && longi == 0.0)
                                            {
                                                lat = (double)dr1["Latitiude"];
                                                longi = (double)dr1["Longitude"];
                                                prvlat = lat;
                                                prevLongi = longi;
                                                TotalDistance = 0.0;
                                                PrevGenTime = (DateTime)dr1["DateTime"];



                                                if (AC == 1)
                                                {
                                                    PrvACOnTime = (DateTime)dr1["DateTime"];
                                                    PresACOnTime = (DateTime)dr1["DateTime"];
                                                    ACStatred = true;
                                                }
                                                else
                                                {
                                                    ACStatred = false;
                                                }
                                            }
                                            else
                                            {
                                                #region calculations
                                                lat = (double)dr1["Latitiude"];
                                                longi = (double)dr1["Longitude"];
                                                TotalDistance += GeoCodeCalc.CalcDistance(lat, longi, prvlat, prevLongi);
                                                prvlat = lat;
                                                prevLongi = longi;

                                                double deisel = (double)dr1["Diesel"];
                                                double speed = (double)dr1["Speed"];
                                                if (deisel > 50 && speed == 0)
                                                {
                                                    runningStarted = false;
                                                    StopStarted = false;
                                                    if (IdleStarted)
                                                    {
                                                        presIdletime = (DateTime)dr1["DateTime"];
                                                        TimeSpan t = new TimeSpan(presIdletime.Ticks);//presIdletime.Hour, presIdletime.Minute, presIdletime.Second);
                                                        TimeSpan t1 = new TimeSpan(PrvIdletime.Ticks);//PrvIdletime.Hour, PrvIdletime.Minute, PrvIdletime.Second);
                                                        IdleTime += Math.Abs(t.Subtract(t1).TotalSeconds);
                                                        PrvIdletime = presIdletime;
                                                        PrevGenTime = presIdletime;
                                                        if (IdleTime > MaxIdleLimit)
                                                            Remarks = "YES";
                                                    }
                                                    else
                                                    {
                                                        IdleStarted = true;
                                                        PrvIdletime = (DateTime)dr1["DateTime"];
                                                        TimeSpan t = new TimeSpan(PrevGenTime.Ticks);//presIdletime.Hour, presIdletime.Minute, presIdletime.Second);
                                                        TimeSpan t1 = new TimeSpan(PrvIdletime.Ticks);//PrvIdletime.Hour, PrvIdletime.Minute, PrvIdletime.Second);
                                                        IdleTime += Math.Abs(t.Subtract(t1).TotalSeconds);
                                                        PrevGenTime = PrvIdletime;
                                                    }
                                                }
                                                //else if (deisel > 50 && speed > 0)
                                                else if (speed > 0)
                                                {
                                                    IdleStarted = false;
                                                    StopStarted = false;
                                                    if (runningStarted)
                                                    {
                                                        PresRunningtime = (DateTime)dr1["DateTime"];
                                                        TimeSpan t = new TimeSpan(PresRunningtime.Ticks);//PresRunningtime.Hour, PresRunningtime.Minute, PresRunningtime.Second);
                                                        TimeSpan t1 = new TimeSpan(PrvRunningtime.Ticks);//PrvRunningtime.Hour, PrvRunningtime.Minute, PrvRunningtime.Second);
                                                        RunningTime += Math.Abs(t.Subtract(t1).TotalSeconds);
                                                        if (speed > Maxspeed)
                                                            Maxspeed = speed;

                                                        totalSpeed += speed;
                                                        //if (speed > MaxStopLimit)
                                                        //    Remarks = "YES";
                                                        PrvRunningtime = PresRunningtime;
                                                        PrevGenTime = PresRunningtime;
                                                    }
                                                    else
                                                    {
                                                        runningStarted = true;
                                                        PrvRunningtime = (DateTime)dr1["DateTime"];
                                                        TimeSpan t = new TimeSpan(PrevGenTime.Ticks);//presIdletime.Hour, presIdletime.Minute, presIdletime.Second);
                                                        TimeSpan t1 = new TimeSpan(PrvRunningtime.Ticks);//PrvIdletime.Hour, PrvIdletime.Minute, PrvIdletime.Second);
                                                        RunningTime += Math.Abs(t.Subtract(t1).TotalSeconds);
                                                        totalSpeed += speed;
                                                        if (speed > Maxspeed)
                                                            Maxspeed = speed;
                                                        PrevGenTime = PrvRunningtime;
                                                    }
                                                }
                                                else if (deisel < 50 && speed == 0)
                                                {
                                                    IdleStarted = false;
                                                    runningStarted = false;
                                                    if (StopStarted)
                                                    {
                                                        PresStoptime = (DateTime)dr1["DateTime"];
                                                        TimeSpan t = new TimeSpan(PresStoptime.Ticks);//PresStoptime.Hour, PresStoptime.Minute, PresStoptime.Second);
                                                        TimeSpan t1 = new TimeSpan(PrvStoptime.Ticks);//PrvStoptime.Hour, PrvStoptime.Minute, PrvStoptime.Second);
                                                        StopTime += Math.Abs(t.Subtract(t1).TotalSeconds);
                                                        PrvStoptime = PresStoptime;
                                                        //if (speed > MaxStopLimit)
                                                        //    Remarks = "YES-stopped";
                                                        PrevGenTime = PresStoptime;
                                                    }
                                                    else
                                                    {
                                                        StopStarted = true;
                                                        PrvStoptime = (DateTime)dr1["DateTime"];
                                                        TimeSpan t = new TimeSpan(PrevGenTime.Ticks);//presIdletime.Hour, presIdletime.Minute, presIdletime.Second);
                                                        TimeSpan t1 = new TimeSpan(PrvStoptime.Ticks);//PrvIdletime.Hour, PrvIdletime.Minute, PrvIdletime.Second);
                                                        StopTime += Math.Abs(t.Subtract(t1).TotalSeconds);
                                                        PrevGenTime = PrvStoptime;
                                                    }
                                                }
                                                #endregion

                                                if (AC == 1)
                                                {
                                                    if (ACStatred == false)

                                                        PrvACOnTime = (DateTime)dr1["DateTime"];
                                                    PresACOnTime = (DateTime)dr1["DateTime"];

                                                    ACStatred = true;
                                                }
                                                else
                                                {
                                                    ACStatred = false;
                                                }

                                                if (ACStatred)
                                                {
                                                    PresACOnTime = (DateTime)dr1["DateTime"];
                                                    TimeSpan t = new TimeSpan(PresACOnTime.Ticks);//presIdletime.Hour, presIdletime.Minute, presIdletime.Second);
                                                    TimeSpan t1 = new TimeSpan(PrvACOnTime.Ticks);
                                                    TotalACTime += Math.Abs(t.Subtract(t1).TotalSeconds);
                                                    PrvACOnTime = PresACOnTime;
                                                }
                                            }
                                        }
                                        //datareport = new DataReport();

                                        if (firstrow != null && lastrow != null)
                                        {
                                            double firstval = 0;
                                            double.TryParse(firstrow["Odometer"].ToString(), out firstval);
                                            double lastval = 0;
                                            double.TryParse(lastrow["Odometer"].ToString(), out lastval);
                                            if (lastval > 0 && firstval > 0)
                                                TotalDistance = lastval - firstval;
                                        }


                                        double avgspeeddiv = (RunningTime / 3600);
                                        double avgspeed = 0;
                                        if (avgspeeddiv > 0)
                                            avgspeed = TotalDistance / avgspeeddiv;

                                        TimeSpan ts1 = new TimeSpan(starttime.Ticks);
                                        TimeSpan ts2 = new TimeSpan((GetLowDate(date).AddMinutes(DiffFromTime.TotalMinutes)).Ticks);
                                        TimeSpan ts3 = new TimeSpan((GetHighDate(date).AddMinutes(-(DifftoTime.TotalMinutes))).Ticks);
                                        TimeSpan ts4 = new TimeSpan(DateTime.Parse(lastrow["DateTime"].ToString()).Ticks);
                                        double ts5 = ts1.Subtract(ts2).TotalSeconds;
                                        double ts6 = ts3.Subtract(ts4).TotalSeconds;
                                        double stationaryhours = ts5 + ts6;

                                        double totalstationaryhours = stationaryhours + StopTime;
                                        DataRow insntrow = dailydatatable.NewRow();
                                        //insntrow["SNo"] = dailydatatable.Rows.Count + 1;
                                        //insntrow["VehicleNo"] = VehicleID;
                                        insntrow["StartTime"] = starttime;
                                        //if(!stopflag)
                                        //    insntrow["StopTime"] = DateConverter.GetHighDate(date);
                                        //else
                                        //    insntrow["StopTime"] = stoptime;
                                        insntrow["StopTime"] = lastrow["DateTime"].ToString();
                                        insntrow["TotalDistanceTravelled(Kms)"] = Math.Round(TotalDistance, 3);
                                        insntrow["MotionHours"] = (int)RunningTime / 3600 + "H " + (int)RunningTime % (60) + " Min";
                                        insntrow["StationaryHours"] = (int)totalstationaryhours / 3600 + " H " + (int)totalstationaryhours % 60 + " Min";
                                        insntrow["MaxSpeed"] = (int)Maxspeed;
                                        insntrow["AvgSpeed"] = String.Format("{0:0.00}", avgspeed);
                                        insntrow["IdleTime"] = (int)IdleTime / 3600 + "H " + (int)IdleTime % 60 + " Min";
                                        insntrow["ACONTime"] = (int)TotalACTime / 3600 + "H " + (int)TotalACTime % 60 + " Min";
                                        dailydatatable.Rows.Add(insntrow);
                                    }
                                    else
                                    {
                                        DataRow insntrow = dailydatatable.NewRow();
                                        //insntrow["SNo"] = dailydatatable.Rows.Count + 1;
                                        //insntrow["VehicleNo"] = VehicleID;
                                        insntrow["StartTime"] = "MOMENT NOT FOUND";
                                        //if(!stopflag)
                                        //    insntrow["StopTime"] = DateConverter.GetHighDate(date);
                                        //else
                                        //    insntrow["StopTime"] = stoptime;
                                        insntrow["StopTime"] = "MOMENT NOT FOUND";
                                        insntrow["TotalDistanceTravelled(Kms)"] = 0;
                                        insntrow["MotionHours"] = "0 H 0 Min";
                                        insntrow["StationaryHours"] = "0 H 0 Min";
                                        insntrow["MaxSpeed"] = 0;
                                        insntrow["AvgSpeed"] = 0;
                                        insntrow["IdleTime"] = "0 H 0 Min";
                                        insntrow["ACONTime"] = "0 H 0 Min";
                                        dailydatatable.Rows.Add(insntrow);
                                    }
                                }
                            }
                            //Session["reportdata"] = reportData;
                            lbl_ReportStatus.Text = "";
                            Session["title"] = lbl_ReportStatus.Text;
                            grdReports.DataSource = dailydatatable;
                            Session["xportdata"] = dailydatatable;
                            grdReports.DataBind();
                            divPieChart.Visible = false;

                            for (int i = 0; i < grdReports.Rows.Count - 1; i++)
                            {
                                if (grdReports.Rows[i].Cells[1].Text == "" || grdReports.Rows[i].Cells[1].Text == "&nbsp;")
                                {
                                    grdReports.Rows[i].BackColor = System.Drawing.Color.Khaki;
                                    grdReports.Rows[i].Cells[0].Controls.Remove(grdReports.Rows[i].Cells[0].Controls[1]);
                                }
                            }
                            //}
                            Session["reportdata"] = reportData;
                        }
                        #endregion
                    }
                    else if (reportname == "Location HaltingHours Report")
                    {
                        #region Branch wise Reports

                        ddwnldr = new DataDownloader();
                        ddwnldr.UpdateBranchDetails(UserName);
                        int sno = 1;
                        string Duration = "";
                        string StDuration = "";
                        DateTime Enteringdt = DateTime.Now;
                        DateTime Leftingingdt = DateTime.Now;
                        DataTable summeryTable = new DataTable();
                        DataColumn summeryColumn = new DataColumn("SNo");
                        summeryTable.Columns.Add(summeryColumn);
                        summeryColumn = new DataColumn("VehicleNo");
                        summeryTable.Columns.Add(summeryColumn);
                        summeryColumn = new DataColumn("Location Name");
                        summeryTable.Columns.Add(summeryColumn);
                        summeryColumn = new DataColumn("VehicleEnteredDate");
                        //summeryColumn.DataType = System.Type.GetType("System.DateTime");
                        summeryTable.Columns.Add(summeryColumn);
                        summeryColumn = new DataColumn("VehicleEnteredTime");
                        summeryTable.Columns.Add(summeryColumn);
                        summeryColumn = new DataColumn("VehicleLeftDate");
                        //summeryColumn.DataType = System.Type.GetType("System.DateTime");
                        summeryTable.Columns.Add(summeryColumn);
                        summeryColumn = new DataColumn("VehicleLeftTime");
                        summeryTable.Columns.Add(summeryColumn);
                        summeryColumn = new DataColumn("Stopped Hours");
                        summeryTable.Columns.Add(summeryColumn);
                        summeryColumn = new DataColumn("Remarks");
                        summeryTable.Columns.Add(summeryColumn);
                        DataRow summeryRow = null;
                        lbl_ReportStatus.Text = "VEHICLE STOPPAGE REPORT FROM: " + fromdate.ToString() + "  To: " + todate.ToString();
                        Session["title"] = lbl_ReportStatus.Text;

                        foreach (string vehiclestr in checkedvhcles)
                        {
                            DataTable TripData = new DataTable();
                            DataTable logs = new DataTable();
                            DataTable tottable = new DataTable();
                            foreach (string tbname in logstbls)
                            {
                                cmd = new MySqlCommand("SELECT '' AS SNo, " + tbname + ".VehicleID, " + tbname + ".DateTime, " + tbname + ".Speed, " + tbname + ".Latitiude, " + tbname + ".Longitude, " + tbname + ".Direction, " + tbname + ".Diesel, " + tbname + ".Odometer, " + tbname + ".Direction AS Expr1, " + tbname + ".Direction AS Expr2, vehiclemaster.MaintenancePlantName, vehiclemaster.VendorName, vehiclemaster.VendorNo, vehiclemaster.VehicleTypeName FROM " + tbname + " LEFT OUTER JOIN vehiclemaster ON " + tbname + ".VehicleID = vehiclemaster.VehicleID WHERE (" + tbname + ".DateTime >= @starttime) AND (" + tbname + ".DateTime <= @endtime) AND (" + tbname + ".VehicleID = '" + vehiclestr + "') and (" + tbname + ".UserID='" + mainuser + "')  ORDER BY " + tbname + ".DateTime");
                                //cmd = new MySqlCommand("select * from " + tbname + " where DateTime>= @starttime and DateTime<=@endtime and VehicleID='" + vehiclestr + "' and UserID='" + UserName + "' order by DateTime");
                                cmd.Parameters.Add(new MySqlParameter("@starttime", fromdate));
                                cmd.Parameters.Add(new MySqlParameter("@endtime", todate));
                                logs = vdm.SelectQuery(cmd).Tables[0];
                                if (tottable.Rows.Count == 0)
                                {
                                    tottable = logs.Clone();
                                }
                                foreach (DataRow dr in logs.Rows)
                                {
                                    tottable.ImportRow(dr);
                                }
                            }
                            DataView dv = tottable.DefaultView;
                            dv.Sort = "DateTime ASC";
                            TripData = dv.ToTable();

                            DataRow Prevrow = null;
                            summeryRow = null;
                            Dictionary<string, string> statusobserver = new Dictionary<string, string>();
                            foreach (DataRow dr in ddwnldr.BranchDetails.Rows)
                            {
                                if (!statusobserver.ContainsKey(dr["BranchID"].ToString()))
                                {
                                    statusobserver.Add(dr["BranchID"].ToString(), "");
                                }
                            }
                            List<string> alllocations = new List<string>();
                            if (DDL_locations.Text != "ALL")
                            {
                                alllocations.Add(DDL_locations.Text);
                            }
                            else
                            {
                                foreach (ListItem lstLocation in DDL_locations.Items)
                                {
                                    alllocations.Add(lstLocation.Text);
                                }
                            }
                            foreach (DataRow tripdatarow in TripData.Rows)
                            {
                                foreach (string lstLocation in alllocations)
                                {
                                    DataRow[] branch = ddwnldr.BranchDetails.Select("BranchID='" + lstLocation + "'");

                                    double presLat = (double)tripdatarow["Latitiude"];
                                    double PresLng = (double)tripdatarow["Longitude"];

                                    foreach (DataRow Brncs in branch)
                                    {
                                        double ag_Lat = 0;
                                        double.TryParse(Brncs["Latitude"].ToString(), out ag_Lat);
                                        double ag_lng = 0;
                                        double.TryParse(Brncs["Longitude"].ToString(), out ag_lng);
                                        double ag_radious = 100;
                                        double.TryParse(Brncs["Radious"].ToString(), out ag_radious);
                                        string statusvalue = ddwnldr.getGeofenceStatus(presLat, PresLng, ag_Lat, ag_lng, ag_radious);
                                        if (statusobserver[Brncs["BranchID"].ToString()] != statusvalue)
                                        {
                                            statusobserver[Brncs["BranchID"].ToString()] = statusvalue;
                                            if (statusobserver[Brncs["BranchID"].ToString()] == "In Side")
                                            {
                                                summeryRow = summeryTable.NewRow();
                                                summeryRow["SNo"] = sno;
                                                summeryRow["VehicleNo"] = tripdatarow["VehicleID"];
                                                summeryRow["Location Name"] = Brncs["BranchID"];
                                                Enteringdt = (DateTime)tripdatarow["DateTime"];
                                                string Enterdate = Enteringdt.ToString("M/dd/yyyy");
                                                string EnterTime = Enteringdt.ToString("hh:mm:ss tt");
                                                summeryRow["VehicleEnteredDate"] = Enterdate;
                                                string[] Reachsplt = EnterTime.ToString().Split(' ');
                                                if (Reachsplt.Length > 1)
                                                {
                                                    int departuretimemin = 0;
                                                    int dephours = 0;
                                                    int depmin = 0;
                                                    int.TryParse(Reachsplt[0].Split(':')[0], out dephours);
                                                    int.TryParse(Reachsplt[0].Split(':')[1], out depmin);
                                                    //departuretimemin = 720 - ((dephours * 60) + depmin);

                                                    if (Reachsplt[1] == "PM")
                                                    {
                                                        if (Reachsplt[0].Split(':')[0] == "12")
                                                            departuretimemin = ((dephours * 60) + depmin);
                                                        else
                                                            departuretimemin = 720 + ((dephours * 60) + depmin);
                                                    }
                                                    else
                                                    {
                                                        if (Reachsplt[0].Split(':')[0] == "12")
                                                            departuretimemin = ((dephours * 60) + depmin) - 720;
                                                        else
                                                            departuretimemin = ((dephours * 60) + depmin);
                                                    }

                                                    //ddlTravels.Items.Add(dr["traveler_agent"].ToString());

                                                    int time = departuretimemin;
                                                    if ((time % 60) < 10 && (time / 60) < 10)
                                                    {
                                                        Duration = "0" + time / 60 + " : " + "0" + time % 60;
                                                    }
                                                    else if ((time % 60) >= 10 && (time / 60) < 10)
                                                    {
                                                        Duration = "0" + time / 60 + " : " + time % 60;
                                                    }
                                                    else if ((time % 60) < 10 && (time / 60) >= 10)
                                                    {
                                                        Duration = time / 60 + " : " + "0" + time % 60;
                                                    }
                                                    else if ((time % 60) >= 10 && (time / 60) >= 10)
                                                    {
                                                        Duration = time / 60 + " : " + time % 60;
                                                    }

                                                }
                                                summeryRow["VehicleEnteredTime"] = Duration;
                                                sno++;
                                                summeryTable.Rows.Add(summeryRow);
                                            }
                                            if (statusobserver[Brncs["BranchID"].ToString()] == "Out Side")
                                            {
                                                if (summeryRow != null && Prevrow != null)
                                                {
                                                    Leftingingdt = (DateTime)tripdatarow["DateTime"];
                                                    string Leftdate = Leftingingdt.ToString("M/dd/yyyy");
                                                    string LeftTime = Leftingingdt.ToString("hh:mm:ss tt");
                                                    summeryRow["VehicleLeftDate"] = Leftdate;

                                                    string[] Reachsplt = LeftTime.ToString().Split(' ');
                                                    if (Reachsplt.Length > 1)
                                                    {
                                                        int departuretimemin = 0;
                                                        int dephours = 0;
                                                        int depmin = 0;
                                                        int.TryParse(Reachsplt[0].Split(':')[0], out dephours);
                                                        int.TryParse(Reachsplt[0].Split(':')[1], out depmin);
                                                        //departuretimemin = 720 - ((dephours * 60) + depmin);

                                                        if (Reachsplt[1] == "PM")
                                                        {
                                                            if (Reachsplt[0].Split(':')[0] == "12")
                                                                departuretimemin = ((dephours * 60) + depmin);
                                                            else
                                                                departuretimemin = 720 + ((dephours * 60) + depmin);
                                                        }
                                                        else
                                                        {
                                                            if (Reachsplt[0].Split(':')[0] == "12")
                                                                departuretimemin = ((dephours * 60) + depmin) - 720;
                                                            else
                                                                departuretimemin = ((dephours * 60) + depmin);
                                                        }

                                                        //ddlTravels.Items.Add(dr["traveler_agent"].ToString());

                                                        int time = departuretimemin;
                                                        if ((time % 60) < 10 && (time / 60) < 10)
                                                        {
                                                            StDuration = "0" + time / 60 + " : " + "0" + time % 60;
                                                        }
                                                        else if ((time % 60) >= 10 && (time / 60) < 10)
                                                        {
                                                            StDuration = "0" + time / 60 + " : " + time % 60;
                                                        }
                                                        else if ((time % 60) < 10 && (time / 60) >= 10)
                                                        {
                                                            StDuration = time / 60 + " : " + "0" + time % 60;
                                                        }
                                                        else if ((time % 60) >= 10 && (time / 60) >= 10)
                                                        {
                                                            StDuration = time / 60 + " : " + time % 60;
                                                        }

                                                    }
                                                    summeryRow["VehicleLeftTime"] = StDuration;



                                                    DateTime sd = Enteringdt;
                                                    DateTime fd = Leftingingdt;

                                                    TimeSpan ts1 = new TimeSpan(fd.Ticks);
                                                    TimeSpan ts2 = new TimeSpan(sd.Ticks);
                                                    TimeSpan ts3 = ts1.Subtract(ts2);

                                                    if (fd.Ticks != sd.Ticks)
                                                    {
                                                        //summeryRow["Location1EnteredTime"] = Prevrow["DateTime"];
                                                    }
                                                    else
                                                    {
                                                        summeryTable.Rows.Remove(summeryRow);
                                                        sno--;
                                                        summeryRow = null;
                                                        break;
                                                    }
                                                    summeryRow["Stopped Hours"] = (int)(ts3.TotalHours % 24) + "Hours " + (int)(ts3.TotalMinutes % 60) + "Min ";
                                                }
                                            }
                                        }
                                        Prevrow = tripdatarow;
                                    }
                                }
                            }
                            if (summeryRow != null && summeryRow["VehicleLeftDate"].ToString() == "")
                            {
                                Leftingingdt = DateTime.Now;
                                string Leftdate = Leftingingdt.ToString("dd.MM.yyyy");
                                string LeftTime = Leftingingdt.ToString("hh:mm:ss tt");
                                StDuration = dateconverter(LeftTime);

                                DateTime sd = Enteringdt;
                                DateTime fd = Leftingingdt;

                                TimeSpan ts1 = new TimeSpan(fd.Ticks);
                                TimeSpan ts2 = new TimeSpan(sd.Ticks);
                                TimeSpan ts3 = ts1.Subtract(ts2);

                                if (fd.Ticks != sd.Ticks)
                                {
                                    //summeryRow["Location1EnteredTime"] = Prevrow["DateTime"];
                                }
                                else
                                {
                                    summeryTable.Rows.Remove(summeryRow);
                                    sno--;
                                    summeryRow = null;
                                    break;
                                }
                                if ((int)(ts3.TotalDays) > 0)
                                {
                                    summeryRow["Stopped Hours"] = (int)(ts3.TotalDays) + "Days " + (int)(ts3.TotalHours % 24) + "Hours " + (int)(ts3.TotalMinutes % 60) + "Min ";
                                }
                                else
                                {
                                    summeryRow["Stopped Hours"] = (int)(ts3.TotalHours % 24) + "Hours " + (int)(ts3.TotalMinutes % 60) + "Min ";
                                }
                            }
                        }

                        grdReports.DataSource = summeryTable;
                        grdReports.DataBind();
                        Session["xportdata"] = summeryTable;
                        divPieChart.Visible = false;

                        for (int i = 0; i < grdReports.Rows.Count; i++)
                        {
                            grdReports.Rows[i].Cells[0].Controls.Remove(grdReports.Rows[i].Cells[0].Controls[1]);
                            if (grdReports.Rows[i].Cells[6].Text == "" || grdReports.Rows[i].Cells[6].Text == "&nbsp;")
                            {
                                grdReports.Rows[i].BackColor = System.Drawing.Color.Khaki;
                            }
                        }

                        #endregion
                    }
                    else if (reportname == "Location To Location Report")
                    {
                        #region location wise Reports
                        if (txtCost.Text == "")
                        {
                            lbl_nofifier.Text = "Enter Cost per km";
                        }
                        else
                        {
                            DateTime Startingdt = DateTime.Now;
                            string Duration = "";
                            string StDuration = "";
                            ddwnldr = new DataDownloader();
                            ddwnldr.UpdateBranchDetails(UserName);
                            string vehicls = "";
                            ////string Status = "";
                            int sno = 1;
                            DataTable summeryTable = new DataTable();
                            DataColumn summeryColumn = new DataColumn("SNo");
                            summeryTable.Columns.Add(summeryColumn);
                            summeryColumn = new DataColumn("VehicleNo");
                            summeryTable.Columns.Add(summeryColumn);
                            summeryColumn = new DataColumn("From Location");
                            summeryTable.Columns.Add(summeryColumn);
                            //summeryColumn = new DataColumn("Location1EnteredTime");
                            //summeryColumn.DataType = System.Type.GetType("System.DateTime");
                            //summeryTable.Columns.Add(summeryColumn);
                            summeryColumn = new DataColumn("Starting Date");
                            //summeryColumn.DataType = System.Type.GetType("System.DateTime");
                            summeryTable.Columns.Add(summeryColumn);

                            summeryColumn = new DataColumn("Starting Time");
                            summeryTable.Columns.Add(summeryColumn);

                            //summeryColumn = new DataColumn("TimeSpentinL1");
                            //summeryTable.Columns.Add(summeryColumn);
                            summeryColumn = new DataColumn("To Location");
                            summeryTable.Columns.Add(summeryColumn);
                            summeryColumn = new DataColumn("Reaching Date");
                            //summeryColumn.DataType = System.Type.GetType("System.DateTime");
                            summeryTable.Columns.Add(summeryColumn);

                            summeryColumn = new DataColumn("Reaching Time");
                            summeryTable.Columns.Add(summeryColumn);


                            //summeryColumn = new DataColumn("Location2LeftTime");
                            //summeryColumn.DataType = System.Type.GetType("System.DateTime");
                            //summeryTable.Columns.Add(summeryColumn);
                            //summeryColumn = new DataColumn("TimeSpentinL2");
                            //summeryTable.Columns.Add(summeryColumn);
                            //summeryColumn = new DataColumn("Distance(Kms)");
                            summeryTable.Columns.Add("Distance(Kms)").DataType = typeof(Double);
                            //summeryTable.Columns.Add(summeryColumn).DataType = typeof(Double);
                            //summeryColumn = new DataColumn("Cost per kms");
                            //summeryTable.Columns.Add(summeryColumn);
                            summeryTable.Columns.Add("Total Cost").DataType = typeof(Double);
                            summeryColumn = new DataColumn("Journey Hours");
                            summeryTable.Columns.Add(summeryColumn);

                            summeryColumn = new DataColumn("Remarks");
                            summeryTable.Columns.Add(summeryColumn);
                            DataRow summeryRow = null;
                            double prevodometer = 0;
                            double presodometer = 0;

                            //foreach (ListItem obj in chbl_vehicles.Items)
                            //{
                            //    if (obj.Selected)
                            //    {
                            //        vehicls += "'" + obj.Text + "',";
                            //    }
                            //}
                            //vehicls = vehicls.Substring(0, vehicls.LastIndexOf(','));

                            lbl_ReportStatus.Text = "LOCATION TO LOCATION REPORT FROM: " + fromdate.ToString("M/dd/yyyy") + "  To: " + todate.ToString("M/dd/yyyy");
                            Session["title"] = lbl_ReportStatus.Text;
                            foreach (string vehiclestr in checkedvhcles)
                            {
                                Maxspeed = 0;
                                bool isfirlstlog = true;
                                bool islocation1 = true;

                                DataTable TripData = new DataTable();
                                DataTable logs = new DataTable();
                                DataTable tottable = new DataTable();
                                foreach (string tbname in logstbls)
                                {
                                    cmd = new MySqlCommand("SELECT " + tbname + ".sno as SNo, " + tbname + ".VehicleID, " + tbname + ".DateTime, " + tbname + ".Speed, " + tbname + ".Latitiude, " + tbname + ".Longitude, " + tbname + ".Direction, " + tbname + ".Diesel, " + tbname + ".Odometer, " + tbname + ".Direction AS Expr1, " + tbname + ".Direction AS Expr2, vehiclemaster.MaintenancePlantName, vehiclemaster.VendorName, vehiclemaster.VendorNo, vehiclemaster.VehicleTypeName FROM " + tbname + " LEFT OUTER JOIN vehiclemaster ON " + tbname + ".VehicleID = vehiclemaster.VehicleID WHERE (" + tbname + ".DateTime >= @starttime) AND (" + tbname + ".DateTime <= @endtime) AND (" + tbname + ".VehicleID = '" + vehiclestr + "') and (" + tbname + ".UserID='" + mainuser + "')  ORDER BY " + tbname + ".DateTime");
                                    //cmd = new MySqlCommand("select * from " + tbname + " where DateTime>= @starttime and DateTime<=@endtime and VehicleID='" + vehiclestr + "' and UserID='" + UserName + "' order by DateTime");
                                    cmd.Parameters.Add(new MySqlParameter("@starttime", fromdate));
                                    cmd.Parameters.Add(new MySqlParameter("@endtime", todate));
                                    logs = vdm.SelectQuery(cmd).Tables[0];
                                    if (tottable.Rows.Count == 0)
                                    {
                                        tottable = logs.Clone();
                                    }
                                    foreach (DataRow dr in logs.Rows)
                                    {
                                        tottable.ImportRow(dr);
                                    }
                                }
                                DataView dv = tottable.DefaultView;
                                dv.Sort = "DateTime ASC";
                                TripData = dv.ToTable();

                                #region for specified locations
                                if (TripData.Rows.Count > 0)
                                {
                                    if (ddlfromlocation.SelectedValue != "ALL" && ddltolocation.SelectedValue != "ALL")
                                    {
                                        DataRow Prevrow = null;
                                        summeryRow = null;
                                        Dictionary<string, string> statusobserver = new Dictionary<string, string>();
                                        foreach (DataRow dr in ddwnldr.BranchDetails.Rows)
                                        {
                                            if (ddlfromlocation.SelectedValue == dr["BranchID"].ToString().Trim() || ddltolocation.SelectedValue == dr["BranchID"].ToString().Trim())
                                            {
                                                if (!statusobserver.ContainsKey(dr["BranchID"].ToString()))
                                                {
                                                    statusobserver.Add(dr["BranchID"].ToString(), "");
                                                }
                                            }
                                        }
                                        List<string> selectedbranches = new List<string>();
                                        selectedbranches.Add(ddlfromlocation.SelectedValue);
                                        selectedbranches.Add(ddltolocation.SelectedValue);
                                        foreach (DataRow tripdatarow in TripData.Rows)
                                        {
                                            foreach (string lstLocation in selectedbranches)
                                            {
                                                DataRow[] branch = ddwnldr.BranchDetails.Select("BranchID='" + lstLocation + "'");

                                                double presLat = (double)tripdatarow["Latitiude"];
                                                double PresLng = (double)tripdatarow["Longitude"];

                                                foreach (DataRow Brncs in branch)
                                                {
                                                    if (ddlfromlocation.SelectedValue == Brncs["BranchID"].ToString() || ddltolocation.SelectedValue == Brncs["BranchID"].ToString())
                                                    {
                                                        double ag_Lat = 0;
                                                        double.TryParse(Brncs["Latitude"].ToString(), out ag_Lat);
                                                        double ag_lng = 0;
                                                        double.TryParse(Brncs["Longitude"].ToString(), out ag_lng);
                                                        double ag_radious = 100;
                                                        double.TryParse(Brncs["Radious"].ToString(), out ag_radious);
                                                        string statusvalue = ddwnldr.getGeofenceStatus(presLat, PresLng, ag_Lat, ag_lng, ag_radious);

                                                        if (statusobserver[Brncs["BranchID"].ToString()] != statusvalue)
                                                        {
                                                            statusobserver[Brncs["BranchID"].ToString()] = statusvalue;
                                                            if (statusobserver[Brncs["BranchID"].ToString()] == "In Side")
                                                            {
                                                                if (!isfirlstlog)
                                                                {
                                                                    if (ddltolocation.SelectedValue == Brncs["BranchID"].ToString())
                                                                    {
                                                                        if (summeryRow["Reaching Date"].ToString() != "")
                                                                        {
                                                                            break;
                                                                        }
                                                                        summeryRow["To Location"] = Brncs["BranchID"];
                                                                        DateTime Reachingdt = (DateTime)tripdatarow["DateTime"];
                                                                        string Reachdate = Reachingdt.ToString("MM/dd/yyyy");
                                                                        string ReachTime = Reachingdt.ToString("hh:mm:ss tt");
                                                                        Duration = dateconverter(ReachTime);
                                                                        summeryRow["Reaching Date"] = Reachdate;
                                                                        summeryRow["Reaching Time"] = Duration;
                                                                        presodometer = double.Parse(tripdatarow["Odometer"].ToString());
                                                                        if (presodometer < prevodometer)
                                                                        {
                                                                            summeryTable.Rows.Remove(summeryRow);
                                                                            sno--;
                                                                            summeryRow = null;
                                                                            isfirlstlog = true;
                                                                            break;
                                                                        }
                                                                        double totaldistance = presodometer - prevodometer;
                                                                        totaldistance = Math.Abs(totaldistance);
                                                                        summeryRow["Distance(Kms)"] = totaldistance.ToString("00.00");
                                                                        double Cost = 0;
                                                                        double.TryParse(txtCost.Text, out Cost);
                                                                        double totcost = 0;
                                                                        totcost = totaldistance * Cost;
                                                                        summeryRow["Total Cost"] = totcost.ToString("00.00");
                                                                        DateTime l1et = Reachingdt;
                                                                        DateTime l1lt = Startingdt;

                                                                        TimeSpan l1ets = new TimeSpan(l1et.Ticks);
                                                                        TimeSpan l1lts = new TimeSpan(l1lt.Ticks);
                                                                        TimeSpan difftime = l1ets.Subtract(l1lts);

                                                                        if (l1et.Ticks != l1lt.Ticks)
                                                                        {
                                                                            //summeryRow["Location1LeftTime"] = Prevrow["DateTime"];
                                                                        }
                                                                        else
                                                                        {
                                                                            summeryTable.Rows.Remove(summeryRow);
                                                                            sno--;
                                                                            summeryRow = null;
                                                                            isfirlstlog = true;
                                                                            break;
                                                                        }
                                                                        if ((int)(difftime.TotalDays) > 0)
                                                                        {
                                                                            summeryRow["Journey Hours"] = (int)(difftime.TotalDays) + "Days " + (int)(difftime.TotalHours % 24) + "Hours " + (int)(difftime.TotalMinutes % 60) + "Min ";
                                                                        }
                                                                        else
                                                                        {
                                                                            summeryRow["Journey Hours"] = (int)(difftime.TotalHours % 24) + "Hours " + (int)(difftime.TotalMinutes % 60) + "Min ";
                                                                        }
                                                                        islocation1 = false;
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (ddlfromlocation.SelectedValue == Brncs["BranchID"].ToString())
                                                                    {
                                                                        summeryRow = summeryTable.NewRow();
                                                                        summeryRow["SNo"] = sno;
                                                                        summeryRow["VehicleNo"] = tripdatarow["VehicleID"];
                                                                        summeryRow["From Location"] = Brncs["BranchID"];
                                                                        //summeryRow["Location1EnteredTime"] = tripdatarow["DateTime"];
                                                                        sno++;
                                                                        summeryTable.Rows.Add(summeryRow);
                                                                        isfirlstlog = false;
                                                                        islocation1 = true;
                                                                    }
                                                                }
                                                            }
                                                            if (statusobserver[Brncs["BranchID"].ToString()] == "Out Side")
                                                            {
                                                                if (ddlfromlocation.SelectedValue == Brncs["BranchID"].ToString())
                                                                {
                                                                    if (summeryRow != null && Prevrow != null)
                                                                    {

                                                                        Startingdt = (DateTime)tripdatarow["DateTime"];
                                                                        string Startdate = Startingdt.ToString("MM/dd/yyyy");
                                                                        string startTime = Startingdt.ToString("hh:mm:ss tt");
                                                                        string[] Reachsplt = startTime.ToString().Split(' ');
                                                                        StDuration = dateconverter(startTime);
                                                                        if (islocation1)
                                                                        {

                                                                            summeryRow["Starting Date"] = Startdate;
                                                                            summeryRow["Starting Time"] = StDuration;
                                                                            prevodometer = double.Parse(tripdatarow["Odometer"].ToString());
                                                                        }
                                                                        else
                                                                        {
                                                                            if (summeryRow["Reaching Date"].ToString() == "")
                                                                            {
                                                                                summeryTable.Rows.Remove(summeryRow);
                                                                                sno--;
                                                                            }

                                                                            summeryRow = null;
                                                                            //isfirlstlog = true;

                                                                            summeryRow = summeryTable.NewRow();
                                                                            summeryRow["SNo"] = sno;
                                                                            summeryRow["VehicleNo"] = tripdatarow["VehicleID"];
                                                                            summeryRow["From Location"] = Brncs["BranchID"];
                                                                            //summeryRow["Location1EnteredTime"] = prevtime;
                                                                            summeryRow["Starting Date"] = Startdate;
                                                                            summeryRow["Starting Time"] = StDuration;

                                                                            sno++;
                                                                            summeryTable.Rows.Add(summeryRow);
                                                                            prevodometer = double.Parse(tripdatarow["Odometer"].ToString());

                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        Prevrow = tripdatarow;
                                                    }
                                                }
                                            }
                                        }

                                    }
                                    #endregion
                                    else
                                    {
                                        #region for all locations
                                        DataRow Prevrow = null;
                                        summeryRow = null;
                                        ////  cmd = new MySqlCommand("SELECT BranchID, Latitude, Longitude, PhoneNumber, PlantName, UserName, Radious, ImageType, ImagePath, IsPlant FROM branchdata WHERE (PlantName IN (SELECT branchdata_1.Sno FROM cabmanagement INNER JOIN branchdata branchdata_1 ON cabmanagement.PlantName = branchdata_1.BranchID WHERE (cabmanagement.UserID = '" + UserName + "') AND (cabmanagement.VehicleID = '" + vehiclestr + "'))) AND (UserName = '" + UserName + "')");
                                        // cmd = new MySqlCommand("SELECT        BranchID, Latitude, Longitude, PhoneNumber, PlantName, UserName, Radious, ImageType, ImagePath, IsPlant FROM            branchdata WHERE        (UserName = '" + UserName + "')");


                                        // DataTable vehbarnches = vdm.SelectQuery(cmd).Tables[0];
                                        DataTable vehbarnches = (DataTable)ViewState["branches"];
                                        Dictionary<string, string> statusobserver = new Dictionary<string, string>();
                                        Dictionary<int, List<obj>> dictionary = new Dictionary<int, List<obj>>();
                                        foreach (DataRow dr in vehbarnches.Rows)
                                        {
                                            if (!statusobserver.ContainsKey(dr["BranchID"].ToString()))
                                            {
                                                statusobserver.Add(dr["BranchID"].ToString(), "");
                                            }
                                            List<obj> objlist = new List<obj>();
                                            foreach (DataRow dr1 in vehbarnches.Rows)
                                            {
                                                obj objct = new obj();
                                                objct.latitude = double.Parse(dr1["Latitude"].ToString());
                                                objct.longitude = double.Parse(dr1["Longitude"].ToString());
                                                objct.lid= int.Parse(dr1["Sno"].ToString());
                                                objct.BranchID = dr1["BranchID"].ToString();
                                                objct.radius= int.Parse(dr1["Radious"].ToString());
                                                double lon1 = Double.Parse(dr["Longitude"].ToString());
                                                double lat1 = Double.Parse(dr["Latitude"].ToString());
                                                double lon22 = Double.Parse(dr1["Longitude"].ToString());
                                                double lat22 = Double.Parse(dr1["Latitude"].ToString());
                                                objct.distance = DistanceAlgorithm.DistanceBetweenPlaces(lon1, lat1, lon22, lat22) * 1000;
                                                objlist.Add(objct);
                                            }
                                            dictionary.Add(int.Parse(dr["Sno"].ToString()), objlist);
                                        }

                                        #region oldcode
                                        //foreach (DataRow tripdatarow in TripData.Rows)
                                        //{
                                        //    foreach (DataRow Brncs in vehbarnches.Rows)
                                        //    {
                                        //        //DataRow[] branch = vehbarnches.Select("BranchID='" + lstLocation + "'");

                                        //        double presLat = (double)tripdatarow["Latitiude"];
                                        //        double PresLng = (double)tripdatarow["Longitude"];

                                        //        foreach (DataRow Brncs in branch)
                                        //        {
                                        //            double ag_Lat = 0;
                                        //            double.TryParse(Brncs["Latitude"].ToString(), out ag_Lat);
                                        //            double ag_lng = 0;
                                        //            double.TryParse(Brncs["Longitude"].ToString(), out ag_lng);
                                        //            double ag_radious = 100;
                                        //            double.TryParse(Brncs["Radious"].ToString(), out ag_radious);
                                        //            string statusvalue = ddwnldr.getGeofenceStatus(presLat, PresLng, ag_Lat, ag_lng, ag_radious);

                                        //            if (statusobserver[Brncs["BranchID"].ToString()] != statusvalue)
                                        //            {
                                        //                statusobserver[Brncs["BranchID"].ToString()] = statusvalue;
                                        //                if (statusobserver[Brncs["BranchID"].ToString()] == "In Side")
                                        //                {
                                        //                    if (!isfirlstlog)
                                        //                    {

                                        //                        summeryRow["To Location"] = Brncs["BranchID"];
                                        //                        DateTime Reachingdt = (DateTime)tripdatarow["DateTime"];
                                        //                        string Reachdate = Reachingdt.ToString("M/dd/yyyy");
                                        //                        string ReachTime = Reachingdt.ToString("hh:mm:ss tt");
                                        //                        summeryRow["Reaching Date"] = Reachdate;


                                        //                        string[] Reachsplt = ReachTime.ToString().Split(' ');
                                        //                        if (Reachsplt.Length > 1)
                                        //                        {
                                        //                            int departuretimemin = 0;
                                        //                            int dephours = 0;
                                        //                            int depmin = 0;
                                        //                            int.TryParse(Reachsplt[0].Split(':')[0], out dephours);
                                        //                            int.TryParse(Reachsplt[0].Split(':')[1], out depmin);
                                        //                            departuretimemin = 720 - ((dephours * 60) + depmin);

                                        //                            if (Reachsplt[1] == "PM")
                                        //                            {
                                        //                                if (Reachsplt[0].Split(':')[0] == "12")
                                        //                                    departuretimemin = ((dephours * 60) + depmin);
                                        //                                else
                                        //                                    departuretimemin = 720 + ((dephours * 60) + depmin);
                                        //                            }
                                        //                            else
                                        //                            {
                                        //                                if (Reachsplt[0].Split(':')[0] == "12")
                                        //                                    departuretimemin = ((dephours * 60) + depmin) - 720;
                                        //                                else
                                        //                                    departuretimemin = ((dephours * 60) + depmin);
                                        //                            }

                                        //                            ddlTravels.Items.Add(dr["traveler_agent"].ToString());

                                        //                            int time = departuretimemin;
                                        //                            int aaa = time % 60;
                                        //                            int sss = time / 60;
                                        //                            if ((time % 60) < 10 && (time / 60) < 10)
                                        //                            {
                                        //                                Duration = "0" + time / 60 + ":" + "0" + time % 60;
                                        //                            }
                                        //                            else if ((time % 60) >= 10 && (time / 60) < 10)
                                        //                            {
                                        //                                Duration = "0" + time / 60 + ":" + time % 60;
                                        //                            }
                                        //                            else if ((time % 60) < 10 && (time / 60) >= 10)
                                        //                            {
                                        //                                Duration = time / 60 + ":" + "0" + time % 60;
                                        //                            }
                                        //                            else if ((time % 60) >= 10 && (time / 60) >= 10)
                                        //                            {
                                        //                                Duration = time / 60 + ":" + time % 60;
                                        //                            }

                                        //                        }
                                        //                        summeryRow["Reaching Time"] = Duration;
                                        //                        presodometer = double.Parse(tripdatarow["Odometer"].ToString());
                                        //                        if (presodometer < prevodometer)
                                        //                        {
                                        //                            summeryTable.Rows.Remove(summeryRow);
                                        //                            sno--;
                                        //                            summeryRow = null;
                                        //                            isfirlstlog = true;
                                        //                            break;
                                        //                        }
                                        //                        double totaldistance = presodometer - prevodometer;
                                        //                        totaldistance = Math.Abs(totaldistance);
                                        //                        summeryRow["Distance(Kms)"] = totaldistance.ToString("00.00");
                                        //                        double Cost = 0;
                                        //                        double.TryParse(txtCost.Text, out Cost);
                                        //                        double totcost = 0;
                                        //                        totcost = totaldistance * Cost;
                                        //                        summeryRow["Total Cost"] = totcost.ToString("00.00");

                                        //                        DateTime l1et = Reachingdt;
                                        //                        DateTime l1lt = Startingdt;

                                        //                        TimeSpan l1ets = new TimeSpan(l1et.Ticks);
                                        //                        TimeSpan l1lts = new TimeSpan(l1lt.Ticks);
                                        //                        TimeSpan difftime = l1ets.Subtract(l1lts);

                                        //                        if (l1et.Ticks != l1lt.Ticks)
                                        //                        {
                                        //                            summeryRow["Location1LeftTime"] = Prevrow["DateTime"];
                                        //                        }
                                        //                        else
                                        //                        {
                                        //                            summeryTable.Rows.Remove(summeryRow);
                                        //                            sno--;
                                        //                            summeryRow = null;
                                        //                            isfirlstlog = true;
                                        //                            break;
                                        //                        }
                                        //                        if ((int)(difftime.TotalDays) > 0)
                                        //                        {
                                        //                            summeryRow["Journey Hours"] = (int)(difftime.TotalDays) + "Days " + (int)(difftime.TotalHours % 24) + "Hours " + (int)(difftime.TotalMinutes % 60) + "Min ";
                                        //                        }
                                        //                        else
                                        //                        {
                                        //                            summeryRow["Journey Hours"] = (int)(difftime.TotalHours % 24) + "Hours " + (int)(difftime.TotalMinutes % 60) + "Min ";
                                        //                        }

                                        //                        summeryRow["Journey Hours"] = (int)(difftime.TotalHours % 24) + "Hours " + (int)(difftime.TotalMinutes % 60) + "Min ";

                                        //                        islocation1 = false;
                                        //                    }
                                        //                    else
                                        //                    {
                                        //                        summeryRow = summeryTable.NewRow();
                                        //                        summeryRow["SNo"] = sno;
                                        //                        summeryRow["VehicleNo"] = tripdatarow["VehicleID"];
                                        //                        summeryRow["From Location"] = Brncs["BranchID"];
                                        //                        summeryRow["Location1EnteredTime"] = tripdatarow["DateTime"];
                                        //                        sno++;
                                        //                        summeryTable.Rows.Add(summeryRow);
                                        //                        isfirlstlog = false;
                                        //                        islocation1 = true;
                                        //                    }
                                        //                }
                                        //                if (statusobserver[Brncs["BranchID"].ToString()] == "Out Side")
                                        //                {
                                        //                    if (summeryRow != null && Prevrow != null)
                                        //                    {
                                        //                        Startingdt = (DateTime)tripdatarow["DateTime"];
                                        //                        string Startdate = Startingdt.ToString("M/dd/yyyy");
                                        //                        string startTime = Startingdt.ToString("hh:mm:ss tt");
                                        //                        string[] Reachsplt = startTime.ToString().Split(' ');
                                        //                        if (Reachsplt.Length > 1)
                                        //                        {
                                        //                            int departuretimemin = 0;
                                        //                            int dephours = 0;
                                        //                            int depmin = 0;
                                        //                            int.TryParse(Reachsplt[0].Split(':')[0], out dephours);
                                        //                            int.TryParse(Reachsplt[0].Split(':')[1], out depmin);
                                        //                            departuretimemin = 720 - ((dephours * 60) + depmin);

                                        //                            if (Reachsplt[1] == "PM")
                                        //                            {
                                        //                                if (Reachsplt[0].Split(':')[0] == "12")
                                        //                                    departuretimemin = ((dephours * 60) + depmin);
                                        //                                else
                                        //                                    departuretimemin = 720 + ((dephours * 60) + depmin);
                                        //                            }
                                        //                            else
                                        //                            {
                                        //                                if (Reachsplt[0].Split(':')[0] == "12")
                                        //                                    departuretimemin = ((dephours * 60) + depmin) - 720;
                                        //                                else
                                        //                                    departuretimemin = ((dephours * 60) + depmin);
                                        //                            }

                                        //                            ddlTravels.Items.Add(dr["traveler_agent"].ToString());

                                        //                            int time = departuretimemin;
                                        //                            if ((time % 60) < 10 && (time / 60) < 10)
                                        //                            {
                                        //                                StDuration = "0" + time / 60 + ":" + "0" + time % 60;
                                        //                            }
                                        //                            else if ((time % 60) >= 10 && (time / 60) < 10)
                                        //                            {
                                        //                                StDuration = "0" + time / 60 + ":" + time % 60;
                                        //                            }
                                        //                            else if ((time % 60) < 10 && (time / 60) >= 10)
                                        //                            {
                                        //                                StDuration = time / 60 + ":" + "0" + time % 60;
                                        //                            }
                                        //                            else if ((time % 60) >= 10 && (time / 60) >= 10)
                                        //                            {
                                        //                                StDuration = time / 60 + ":" + time % 60;
                                        //                            }

                                        //                        }
                                        //                        if (islocation1)
                                        //                        {

                                        //                            summeryRow["Starting Date"] = Startdate;
                                        //                            summeryRow["Starting Time"] = StDuration;


                                        //                            prevodometer = double.Parse(tripdatarow["Odometer"].ToString());
                                        //                        }
                                        //                        else
                                        //                        {
                                        //                            summeryRow = null;
                                        //                            isfirlstlog = true;

                                        //                            summeryRow = summeryTable.NewRow();
                                        //                            summeryRow["SNo"] = sno;
                                        //                            summeryRow["VehicleNo"] = tripdatarow["VehicleID"];
                                        //                            summeryRow["From Location"] = Brncs["BranchID"];
                                        //                            summeryRow["Location1EnteredTime"] = prevtime;
                                        //                            summeryRow["Starting Date"] = Startdate;
                                        //                            summeryRow["Starting Time"] = StDuration;

                                        //                            sno++;
                                        //                            summeryTable.Rows.Add(summeryRow);
                                        //                            prevodometer = double.Parse(tripdatarow["Odometer"].ToString());

                                        //                        }

                                        //                    }
                                        //                }
                                        //            }
                                        //            Prevrow = tripdatarow;
                                        //        }
                                        //    }
                                        //}
                                        #endregion
                                        #region newcode
                                        double lon, lat, lon2, lat2, distance, mindist;
                                        int i, j, k, flag, index, flag2;
                                        string date;
                                        string bid;
                                        int dog = 0;
                                        int pig = 0;
                                        index = 0;
                                        flag = 0;
                                        flag2 = 0;
                                        obj refpoint = new obj();
                                        mindist = double.PositiveInfinity;
                                        DataTable foundrowbutton2 = TripData;
                                        DataTable foundrow = vehbarnches;
                                        for (j = 0; j < foundrowbutton2.Rows.Count; j++)
                                        {
                                            lon = Double.Parse(foundrowbutton2.Rows[j]["Longitude"].ToString());
                                            lat = Double.Parse(foundrowbutton2.Rows[j]["Latitiude"].ToString());
                                            date = foundrowbutton2.Rows[j]["DateTime"].ToString();
                                            for (i = 0; i < foundrow.Rows.Count; i++)
                                            {
                                                pig++;
                                                lon2 = Double.Parse(foundrow.Rows[i]["Longitude"].ToString());
                                                lat2 = Double.Parse(foundrow.Rows[i]["Latitude"].ToString());
                                                bid = foundrow.Rows[i]["BranchID"].ToString();
                                                distance = DistanceAlgorithm.DistanceBetweenPlaces(lon, lat, lon2, lat2) * 1000;
                                                if (distance <= int.Parse(foundrow.Rows[i]["Radious"].ToString()))
                                                {

                                                    statusobserver[bid] = "In Side";
                                                    refpoint.lid = int.Parse(foundrow.Rows[i]["Sno"].ToString());
                                                    refpoint.longitude = lon2;
                                                    refpoint.latitude = lat2;
                                                    refpoint.BranchID = bid;
                                                    flag = 1;
                                                 
                                                    summeryRow = summeryTable.NewRow();
                                                    summeryRow["SNo"] = sno;
                                                    summeryRow["VehicleNo"] = foundrowbutton2.Rows[j]["VehicleID"];
                                                    summeryRow["From Location"] = bid;
                                                    sno++;
                                                    summeryTable.Rows.Add(summeryRow);
                                                    isfirlstlog = false;
                                                    islocation1 = true;

                                                    break;
                                                }
                                                else
                                                {
                                                    statusobserver[bid] = "Out Side";
                                                }
                                            }
                                            if (flag == 1)
                                            {
                                                index = j;
                                                break;
                                            }
                                        }

                                        //Console.WriteLine("hello");
                                        if (flag == 1 && index < foundrowbutton2.Rows.Count)
                                        {
                                            int sno1 = refpoint.lid;
                                            lon = refpoint.longitude;
                                            lat = refpoint.latitude;
                                            bid = refpoint.BranchID;
                                            List<obj> temp = new List<obj>();
                                            temp = dictionary[sno1];
                                            for (k = index + 1; k < foundrowbutton2.Rows.Count; k++)
                                            {
                                                lon2 = Double.Parse(foundrowbutton2.Rows[k]["Longitude"].ToString());
                                                lat2 = Double.Parse(foundrowbutton2.Rows[k]["Latitiude"].ToString());
                                                date = foundrowbutton2.Rows[k]["DateTime"].ToString();
                                                distance = DistanceAlgorithm.DistanceBetweenPlaces(lon, lat, lon2, lat2) * 1000;
                                                foreach (obj d in temp)
                                                {
                                                    pig++;
                                                    string statusvalue = "";
                                                    if (Math.Abs(d.distance - distance) <= d.radius)
                                                    {
                                                        double gn = DistanceAlgorithm.DistanceBetweenPlaces(d.longitude, d.latitude, lon2, lat2) * 1000;
                                                        dog++;
                                                        if (gn <= d.radius)
                                                        {
                                                            statusvalue = "In Side";
                                                            if (statusobserver[d.BranchID] != statusvalue)
                                                            {
                                                                statusobserver[d.BranchID] = statusvalue;

                                                                if (!isfirlstlog)
                                                                {
                                                                    summeryRow["To Location"] = d.BranchID;
                                                                    DateTime Reachingdt = (DateTime)foundrowbutton2.Rows[k]["DateTime"];
                                                                    string Reachdate = Reachingdt.ToString("M/dd/yyyy");
                                                                    string ReachTime = Reachingdt.ToString("hh:mm:ss tt");
                                                                    summeryRow["Reaching Date"] = Reachdate;


                                                                    string[] Reachsplt = ReachTime.ToString().Split(' ');
                                                                    if (Reachsplt.Length > 1)
                                                                    {
                                                                        int departuretimemin = 0;
                                                                        int dephours = 0;
                                                                        int depmin = 0;
                                                                        int.TryParse(Reachsplt[0].Split(':')[0], out dephours);
                                                                        int.TryParse(Reachsplt[0].Split(':')[1], out depmin);
                                                                        departuretimemin = 720 - ((dephours * 60) + depmin);

                                                                        if (Reachsplt[1] == "PM")
                                                                        {
                                                                            if (Reachsplt[0].Split(':')[0] == "12")
                                                                                departuretimemin = ((dephours * 60) + depmin);
                                                                            else
                                                                                departuretimemin = 720 + ((dephours * 60) + depmin);
                                                                        }
                                                                        else
                                                                        {
                                                                            if (Reachsplt[0].Split(':')[0] == "12")
                                                                                departuretimemin = ((dephours * 60) + depmin) - 720;
                                                                            else
                                                                                departuretimemin = ((dephours * 60) + depmin);
                                                                        }

                                                                        //ddlTravels.Items.Add(dr["traveler_agent"].ToString());

                                                                        int time = departuretimemin;
                                                                        int aaa = time % 60;
                                                                        int sss = time / 60;
                                                                        if ((time % 60) < 10 && (time / 60) < 10)
                                                                        {
                                                                            Duration = "0" + time / 60 + ":" + "0" + time % 60;
                                                                        }
                                                                        else if ((time % 60) >= 10 && (time / 60) < 10)
                                                                        {
                                                                            Duration = "0" + time / 60 + ":" + time % 60;
                                                                        }
                                                                        else if ((time % 60) < 10 && (time / 60) >= 10)
                                                                        {
                                                                            Duration = time / 60 + ":" + "0" + time % 60;
                                                                        }
                                                                        else if ((time % 60) >= 10 && (time / 60) >= 10)
                                                                        {
                                                                            Duration = time / 60 + ":" + time % 60;
                                                                        }

                                                                    }
                                                                    summeryRow["Reaching Time"] = Duration;
                                                                    presodometer = double.Parse(foundrowbutton2.Rows[k]["Odometer"].ToString());
                                                                    if (presodometer < prevodometer)
                                                                    {
                                                                        summeryTable.Rows.Remove(summeryRow);
                                                                        sno--;
                                                                        summeryRow = null;
                                                                        isfirlstlog = true;
                                                                        break;
                                                                    }
                                                                    double totaldistance = presodometer - prevodometer;
                                                                    totaldistance = Math.Abs(totaldistance);
                                                                    summeryRow["Distance(Kms)"] = totaldistance.ToString("00.00");
                                                                    double Cost = 0;
                                                                    double.TryParse(txtCost.Text, out Cost);
                                                                    double totcost = 0;
                                                                    totcost = totaldistance * Cost;
                                                                    summeryRow["Total Cost"] = totcost.ToString("00.00");

                                                                    DateTime l1et = Reachingdt;
                                                                    DateTime l1lt = Startingdt;

                                                                    TimeSpan l1ets = new TimeSpan(l1et.Ticks);
                                                                    TimeSpan l1lts = new TimeSpan(l1lt.Ticks);
                                                                    TimeSpan difftime = l1ets.Subtract(l1lts);

                                                                    if (l1et.Ticks != l1lt.Ticks)
                                                                    {
                                                                    }
                                                                    else
                                                                    {
                                                                        summeryTable.Rows.Remove(summeryRow);
                                                                        sno--;
                                                                        summeryRow = null;
                                                                        isfirlstlog = true;
                                                                        break;
                                                                    }
                                                                    if ((int)(difftime.TotalDays) > 0)
                                                                    {
                                                                        summeryRow["Journey Hours"] = (int)(difftime.TotalDays) + "Days " + (int)(difftime.TotalHours % 24) + "Hours " + (int)(difftime.TotalMinutes % 60) + "Min ";
                                                                    }
                                                                    else
                                                                    {
                                                                        summeryRow["Journey Hours"] = (int)(difftime.TotalHours % 24) + "Hours " + (int)(difftime.TotalMinutes % 60) + "Min ";
                                                                    }

                                                                    summeryRow["Journey Hours"] = (int)(difftime.TotalHours % 24) + "Hours " + (int)(difftime.TotalMinutes % 60) + "Min ";

                                                                    islocation1 = false;
                                                                }
                                                                else
                                                                {
                                                                    summeryRow = summeryTable.NewRow();
                                                                    summeryRow["SNo"] = sno;
                                                                    summeryRow["VehicleNo"] = foundrowbutton2.Rows[k]["VehicleID"];
                                                                    summeryRow["From Location"] = d.BranchID;
                                                                    sno++;
                                                                    summeryTable.Rows.Add(summeryRow);
                                                                    isfirlstlog = false;
                                                                    islocation1 = true;
                                                                }


                                                                refpoint.lid = d.lid;
                                                                refpoint.longitude = d.longitude;
                                                                refpoint.latitude = d.latitude;
                                                                refpoint.BranchID = d.BranchID;
                                                                sno1 = refpoint.lid;
                                                                lon = refpoint.longitude;
                                                                lat = refpoint.latitude;
                                                                temp = dictionary[sno1];
                                                                bid = refpoint.BranchID;
                                                        
                                                                break;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            statusvalue = "Out Side";
                                                            if (statusobserver[d.BranchID] != statusvalue)
                                                            {
                                                                statusobserver[d.BranchID] = statusvalue;
                                                                if (summeryRow != null && Prevrow != null)
                                                                {
                                                                    Startingdt = (DateTime)foundrowbutton2.Rows[k]["DateTime"];
                                                                    string Startdate = Startingdt.ToString("M/dd/yyyy");
                                                                    string startTime = Startingdt.ToString("hh:mm:ss tt");
                                                                    string[] Reachsplt = startTime.ToString().Split(' ');
                                                                    if (Reachsplt.Length > 1)
                                                                    {
                                                                        int departuretimemin = 0;
                                                                        int dephours = 0;
                                                                        int depmin = 0;
                                                                        int.TryParse(Reachsplt[0].Split(':')[0], out dephours);
                                                                        int.TryParse(Reachsplt[0].Split(':')[1], out depmin);
                                                                        departuretimemin = 720 - ((dephours * 60) + depmin);

                                                                        if (Reachsplt[1] == "PM")
                                                                        {
                                                                            if (Reachsplt[0].Split(':')[0] == "12")
                                                                                departuretimemin = ((dephours * 60) + depmin);
                                                                            else
                                                                                departuretimemin = 720 + ((dephours * 60) + depmin);
                                                                        }
                                                                        else
                                                                        {
                                                                            if (Reachsplt[0].Split(':')[0] == "12")
                                                                                departuretimemin = ((dephours * 60) + depmin) - 720;
                                                                            else
                                                                                departuretimemin = ((dephours * 60) + depmin);
                                                                        }


                                                                        int time = departuretimemin;
                                                                        if ((time % 60) < 10 && (time / 60) < 10)
                                                                        {
                                                                            StDuration = "0" + time / 60 + ":" + "0" + time % 60;
                                                                        }
                                                                        else if ((time % 60) >= 10 && (time / 60) < 10)
                                                                        {
                                                                            StDuration = "0" + time / 60 + ":" + time % 60;
                                                                        }
                                                                        else if ((time % 60) < 10 && (time / 60) >= 10)
                                                                        {
                                                                            StDuration = time / 60 + ":" + "0" + time % 60;
                                                                        }
                                                                        else if ((time % 60) >= 10 && (time / 60) >= 10)
                                                                        {
                                                                            StDuration = time / 60 + ":" + time % 60;
                                                                        }

                                                                    }
                                                                    if (islocation1)
                                                                    {

                                                                        summeryRow["Starting Date"] = Startdate;
                                                                        summeryRow["Starting Time"] = StDuration;


                                                                        prevodometer = double.Parse(foundrowbutton2.Rows[k]["Odometer"].ToString());
                                                                    }
                                                                    else
                                                                    {
                                                                        summeryRow = null;
                                                                        isfirlstlog = false;

                                                                        summeryRow = summeryTable.NewRow();
                                                                        summeryRow["SNo"] = sno;
                                                                        summeryRow["VehicleNo"] = foundrowbutton2.Rows[k]["VehicleID"];
                                                                        summeryRow["From Location"] = d.BranchID;
                                                                        summeryRow["Starting Date"] = Startdate;
                                                                        summeryRow["Starting Time"] = StDuration;

                                                                        sno++;
                                                                        summeryTable.Rows.Add(summeryRow);
                                                                        prevodometer = double.Parse(foundrowbutton2.Rows[k]["Odometer"].ToString());

                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        statusvalue = "Out Side";
                                                        if (statusobserver[d.BranchID] != statusvalue)
                                                        {
                                                            statusobserver[d.BranchID] = statusvalue;
                                                            if (summeryRow != null && Prevrow != null)
                                                            {
                                                                Startingdt = (DateTime)foundrowbutton2.Rows[k]["DateTime"];
                                                                string Startdate = Startingdt.ToString("M/dd/yyyy");
                                                                string startTime = Startingdt.ToString("hh:mm:ss tt");
                                                                string[] Reachsplt = startTime.ToString().Split(' ');
                                                                if (Reachsplt.Length > 1)
                                                                {
                                                                    int departuretimemin = 0;
                                                                    int dephours = 0;
                                                                    int depmin = 0;
                                                                    int.TryParse(Reachsplt[0].Split(':')[0], out dephours);
                                                                    int.TryParse(Reachsplt[0].Split(':')[1], out depmin);
                                                                    departuretimemin = 720 - ((dephours * 60) + depmin);

                                                                    if (Reachsplt[1] == "PM")
                                                                    {
                                                                        if (Reachsplt[0].Split(':')[0] == "12")
                                                                            departuretimemin = ((dephours * 60) + depmin);
                                                                        else
                                                                            departuretimemin = 720 + ((dephours * 60) + depmin);
                                                                    }
                                                                    else
                                                                    {
                                                                        if (Reachsplt[0].Split(':')[0] == "12")
                                                                            departuretimemin = ((dephours * 60) + depmin) - 720;
                                                                        else
                                                                            departuretimemin = ((dephours * 60) + depmin);
                                                                    }


                                                                    int time = departuretimemin;
                                                                    if ((time % 60) < 10 && (time / 60) < 10)
                                                                    {
                                                                        StDuration = "0" + time / 60 + ":" + "0" + time % 60;
                                                                    }
                                                                    else if ((time % 60) >= 10 && (time / 60) < 10)
                                                                    {
                                                                        StDuration = "0" + time / 60 + ":" + time % 60;
                                                                    }
                                                                    else if ((time % 60) < 10 && (time / 60) >= 10)
                                                                    {
                                                                        StDuration = time / 60 + ":" + "0" + time % 60;
                                                                    }
                                                                    else if ((time % 60) >= 10 && (time / 60) >= 10)
                                                                    {
                                                                        StDuration = time / 60 + ":" + time % 60;
                                                                    }

                                                                }
                                                                if (islocation1)
                                                                {

                                                                    summeryRow["Starting Date"] = Startdate;
                                                                    summeryRow["Starting Time"] = StDuration;


                                                                    prevodometer = double.Parse(foundrowbutton2.Rows[k]["Odometer"].ToString());
                                                                }
                                                                else
                                                                {
                                                                    summeryRow = null;
                                                                    isfirlstlog = false;

                                                                    summeryRow = summeryTable.NewRow();
                                                                    summeryRow["SNo"] = sno;
                                                                    summeryRow["VehicleNo"] = foundrowbutton2.Rows[k]["VehicleID"];
                                                                    summeryRow["From Location"] = d.BranchID;
                                                                    summeryRow["Starting Date"] = Startdate;
                                                                    summeryRow["Starting Time"] = StDuration;

                                                                    sno++;
                                                                    summeryTable.Rows.Add(summeryRow);
                                                                    prevodometer = double.Parse(foundrowbutton2.Rows[k]["Odometer"].ToString());

                                                                }
                                                            }
                                                        }
                                                    }
                                                    Prevrow = foundrowbutton2.Rows[k];
                                                }
                                            }
                                        }
                                        #endregion
                                        #endregion
                                    }
                                }
                            }
                            if (summeryTable.Rows.Count > 0)
                            {
                                int snocnt = 1;
                                for (int cnt = 0; cnt < summeryTable.Rows.Count; cnt++)
                                {
                                    if (summeryTable.Rows[cnt]["To Location"].ToString() == "" || summeryTable.Rows[cnt]["Starting Date"].ToString() == "")
                                    {
                                        summeryTable.Rows.RemoveAt(cnt);
                                        cnt--;
                                    }
                                    else
                                    {
                                        summeryTable.Rows[cnt]["SNo"] = snocnt;
                                        snocnt++;
                                    }
                                }
                            }
                            DataRow newvartical = summeryTable.NewRow();
                            newvartical["From Location"] = "Total";
                            double val = 0.0;
                            foreach (DataColumn dc in summeryTable.Columns)
                            {
                                if (dc.DataType == typeof(Double))
                                {
                                    val = 0.0;
                                    double.TryParse(summeryTable.Compute("sum([" + dc.ToString() + "])", "[" + dc.ToString() + "]<>'0'").ToString(), out val);
                                    newvartical[dc.ToString()] = val;
                                }
                            }
                            summeryTable.Rows.Add(newvartical);
                            grdReports.DataSource = summeryTable;
                            grdReports.DataBind();
                            Session["xportdata"] = summeryTable;
                            divPieChart.Visible = false;
                            for (int i = 0; i < grdReports.Rows.Count; i++)
                            {
                                if (grdReports.Rows[i].Cells[3].Text == "Total")
                                {
                                    grdReports.Rows[i].Cells[0].Controls.Remove(grdReports.Rows[i].Cells[0].Controls[1]);
                                }
                            }
                        }

                        #endregion
                    }

                    else if (reportname == "Tanker and Puffs Trip Report")
                    {
                        #region Tanker and Puffs Trip Report
                        if (txtCost.Text == "")
                        {
                            lbl_nofifier.Text = "Enter Cost per km";
                        }
                        else
                        {
                            DateTime Startingdt = DateTime.Now;
                            string Duration = "";
                            string StDuration = "";
                            ddwnldr = new DataDownloader();
                            ddwnldr.UpdateBranchDetails(UserName);
                            string vehicls = "";
                            ////string Status = "";
                            int sno = 1;
                            DataTable summeryTable = new DataTable();
                            DataColumn summeryColumn = new DataColumn("SNo");
                            summeryTable.Columns.Add(summeryColumn);
                            summeryColumn = new DataColumn("VehicleNo");
                            summeryTable.Columns.Add(summeryColumn);
                            summeryColumn = new DataColumn("From Location");
                            summeryTable.Columns.Add(summeryColumn);
                            //summeryColumn = new DataColumn("Location1EnteredTime");
                            //summeryColumn.DataType = System.Type.GetType("System.DateTime");
                            //summeryTable.Columns.Add(summeryColumn);
                            summeryColumn = new DataColumn("Starting Date");
                            //summeryColumn.DataType = System.Type.GetType("System.DateTime");
                            summeryTable.Columns.Add(summeryColumn);

                            summeryColumn = new DataColumn("Starting Time");
                            summeryTable.Columns.Add(summeryColumn);

                            //summeryColumn = new DataColumn("TimeSpentinL1");
                            //summeryTable.Columns.Add(summeryColumn);
                            summeryColumn = new DataColumn("To Location");
                            summeryTable.Columns.Add(summeryColumn);
                            summeryColumn = new DataColumn("Reaching Date");
                            //summeryColumn.DataType = System.Type.GetType("System.DateTime");
                            summeryTable.Columns.Add(summeryColumn);

                            summeryColumn = new DataColumn("Reaching Time");
                            summeryTable.Columns.Add(summeryColumn);


                            //summeryColumn = new DataColumn("Location2LeftTime");
                            //summeryColumn.DataType = System.Type.GetType("System.DateTime");
                            //summeryTable.Columns.Add(summeryColumn);
                            //summeryColumn = new DataColumn("TimeSpentinL2");
                            //summeryTable.Columns.Add(summeryColumn);
                            //summeryColumn = new DataColumn("Distance(Kms)");
                            summeryTable.Columns.Add("Distance(Kms)").DataType = typeof(Double);
                            //summeryTable.Columns.Add(summeryColumn).DataType = typeof(Double);
                            //summeryColumn = new DataColumn("Cost per kms");
                            //summeryTable.Columns.Add(summeryColumn);
                            summeryTable.Columns.Add("Total Cost").DataType = typeof(Double);
                            summeryColumn = new DataColumn("Journey Hours");
                            summeryTable.Columns.Add(summeryColumn);

                            summeryColumn = new DataColumn("Reached Locations");
                            summeryTable.Columns.Add(summeryColumn);
                            DataRow summeryRow = null;
                            double prevodometer = 0;
                            double presodometer = 0;

                            //foreach (ListItem obj in chbl_vehicles.Items)
                            //{
                            //    if (obj.Selected)
                            //    {
                            //        vehicls += "'" + obj.Text + "',";
                            //    }
                            //}
                            //vehicls = vehicls.Substring(0, vehicls.LastIndexOf(','));

                            lbl_ReportStatus.Text = "Tanker and Puffs Trip REPORT FROM: " + fromdate.ToString("M/dd/yyyy") + "  To: " + todate.ToString("M/dd/yyyy");
                            Session["title"] = lbl_ReportStatus.Text;

                            cmd = new MySqlCommand("SELECT        Sno, UserName, BranchID, Description, Latitude, Longitude, PhoneNumber, ImagePath, ImageType, Radious, PlantName, IsPlant, Category_sno,  direction FROM            branchdata WHERE        (UserName = 'modeldairy') AND (IsPlant = 1)");
                            DataTable vehbarnches = vdm.SelectQuery(cmd).Tables[0];
                            if (vehbarnches.Rows.Count == 0)
                            {
                                return;
                            }
                            DataRow[] selectPlantsarray = vehbarnches.Select("Category_sno=0");
                            DataTable selectPlants = vehbarnches.Clone();

                            if (selectPlantsarray.Length > 0)
                                selectPlants = selectPlantsarray.CopyToDataTable();




                            foreach (string vehiclestr in checkedvhcles)
                            {
                                Maxspeed = 0;
                                bool isfirlstlog = true;
                                bool islocation1 = true;

                                DataTable TripData = new DataTable();
                                DataTable logs = new DataTable();
                                DataTable tottable = new DataTable();


                                DateTime decidedfromdate = GoBackandGetPlantTouchdate(logstbls, fromdate, selectPlants,vehiclestr);


                                foreach (string tbname in logstbls)
                                {
                                    cmd = new MySqlCommand("SELECT '' AS SNo, " + tbname + ".VehicleID, " + tbname + ".DateTime, " + tbname + ".Speed, " + tbname + ".Latitiude, " + tbname + ".Longitude, " + tbname + ".Direction, " + tbname + ".Diesel, " + tbname + ".Odometer, " + tbname + ".Direction AS Expr1, " + tbname + ".Direction AS Expr2, vehiclemaster.MaintenancePlantName, vehiclemaster.VendorName, vehiclemaster.VendorNo, vehiclemaster.VehicleTypeName FROM " + tbname + " LEFT OUTER JOIN vehiclemaster ON " + tbname + ".VehicleID = vehiclemaster.VehicleID WHERE (" + tbname + ".DateTime >= @starttime) AND (" + tbname + ".DateTime <= @endtime) AND (" + tbname + ".VehicleID = '" + vehiclestr + "') and (" + tbname + ".UserID='" + mainuser + "')  ORDER BY " + tbname + ".DateTime");
                                    //cmd = new MySqlCommand("select * from " + tbname + " where DateTime>= @starttime and DateTime<=@endtime and VehicleID='" + vehiclestr + "' and UserID='" + UserName + "' order by DateTime");
                                    cmd.Parameters.Add(new MySqlParameter("@starttime", decidedfromdate));//.AddMinutes(-30)));
                                    cmd.Parameters.Add(new MySqlParameter("@endtime", todate));
                                    logs = vdm.SelectQuery(cmd).Tables[0];
                                    if (tottable.Rows.Count == 0)
                                    {
                                        tottable = logs.Clone();
                                    }
                                    foreach (DataRow dr in logs.Rows)
                                    {
                                        tottable.ImportRow(dr);
                                    }
                                }
                                DataView dv = tottable.DefaultView;
                                dv.Sort = "DateTime ASC";
                                TripData = dv.ToTable();


                                #region for all locations
                                DataRow Prevrow = null;
                                summeryRow = null;
                               // cmd = new MySqlCommand("SELECT BranchID, Latitude, Longitude, PhoneNumber, PlantName, UserName, Radious, ImageType, ImagePath, IsPlant FROM branchdata WHERE (PlantName IN (SELECT branchdata_1.Sno FROM cabmanagement INNER JOIN branchdata branchdata_1 ON cabmanagement.PlantName = branchdata_1.BranchID WHERE (cabmanagement.UserID = '" + UserName + "') AND (cabmanagement.VehicleID = '" + vehiclestr + "'))) AND (UserName = '" + UserName + "')");
                           
                                Dictionary<string, string> statusobserver = new Dictionary<string, string>();
                                foreach (DataRow dr in vehbarnches.Rows)
                                {
                                    if (!statusobserver.ContainsKey(dr["BranchID"].ToString()))
                                    {
                                        statusobserver.Add(dr["BranchID"].ToString(), "");
                                    }
                                }

                                string touchedlocs = "";
                                foreach (DataRow tripdatarow in TripData.Rows)
                                {
                                    DateTime Reachingdt = (DateTime)tripdatarow["DateTime"];

                                    foreach (DataRow Brncs in selectPlants.Rows)
                                    //foreach (DataRow Brncs in vehbarnches.Rows)
                                    {
                                        ////DataRow[] branch = vehbarnches.Select("BranchID='" + lstLocation + "'");

                                        double presLat = (double)tripdatarow["Latitiude"];
                                        double PresLng = (double)tripdatarow["Longitude"];

                                        //foreach (DataRow Brncs in branch)
                                        //{
                                        double ag_Lat = 0;
                                        double.TryParse(Brncs["Latitude"].ToString(), out ag_Lat);
                                        double ag_lng = 0;
                                        double.TryParse(Brncs["Longitude"].ToString(), out ag_lng);
                                        double ag_radious = 100;
                                        double.TryParse(Brncs["Radious"].ToString(), out ag_radious);
                                        string statusvalue = ddwnldr.getGeofenceStatus(presLat, PresLng, ag_Lat, ag_lng, ag_radious);

                                        if (statusobserver[Brncs["BranchID"].ToString()] != statusvalue)
                                        {
                                            statusobserver[Brncs["BranchID"].ToString()] = statusvalue;
                                            if (statusobserver[Brncs["BranchID"].ToString()] == "In Side")
                                            {
                                                if (!isfirlstlog)
                                                {
                                                    if (selectPlants.Select("BranchID='" + Brncs["BranchID"].ToString() + "'").Length > 0)
                                                    {

                                                        summeryRow["To Location"] = Brncs["BranchID"];
                                                        string Reachdate = Reachingdt.ToString("M/dd/yyyy");
                                                        string ReachTime = Reachingdt.ToString("hh:mm:ss tt");
                                                        summeryRow["Reaching Date"] = Reachdate;


                                                        string[] Reachsplt = ReachTime.ToString().Split(' ');
                                                        if (Reachsplt.Length > 1)
                                                        {
                                                            int departuretimemin = 0;
                                                            int dephours = 0;
                                                            int depmin = 0;
                                                            int.TryParse(Reachsplt[0].Split(':')[0], out dephours);
                                                            int.TryParse(Reachsplt[0].Split(':')[1], out depmin);
                                                            //departuretimemin = 720 - ((dephours * 60) + depmin);

                                                            if (Reachsplt[1] == "PM")
                                                            {
                                                                if (Reachsplt[0].Split(':')[0] == "12")
                                                                    departuretimemin = ((dephours * 60) + depmin);
                                                                else
                                                                    departuretimemin = 720 + ((dephours * 60) + depmin);
                                                            }
                                                            else
                                                            {
                                                                if (Reachsplt[0].Split(':')[0] == "12")
                                                                    departuretimemin = ((dephours * 60) + depmin) - 720;
                                                                else
                                                                    departuretimemin = ((dephours * 60) + depmin);
                                                            }

                                                            //ddlTravels.Items.Add(dr["traveler_agent"].ToString());

                                                            int time = departuretimemin;
                                                            int aaa = time % 60;
                                                            int sss = time / 60;
                                                            if ((time % 60) < 10 && (time / 60) < 10)
                                                            {
                                                                Duration = "0" + time / 60 + ":" + "0" + time % 60;
                                                            }
                                                            else if ((time % 60) >= 10 && (time / 60) < 10)
                                                            {
                                                                Duration = "0" + time / 60 + ":" + time % 60;
                                                            }
                                                            else if ((time % 60) < 10 && (time / 60) >= 10)
                                                            {
                                                                Duration = time / 60 + ":" + "0" + time % 60;
                                                            }
                                                            else if ((time % 60) >= 10 && (time / 60) >= 10)
                                                            {
                                                                Duration = time / 60 + ":" + time % 60;
                                                            }

                                                        }
                                                        summeryRow["Reaching Time"] = Duration;
                                                        presodometer = double.Parse(tripdatarow["Odometer"].ToString());
                                                        if (presodometer < prevodometer)
                                                        {
                                                            summeryTable.Rows.Remove(summeryRow);
                                                            sno--;
                                                            summeryRow = null;
                                                            isfirlstlog = true;
                                                            break;
                                                        }
                                                        double totaldistance = presodometer - prevodometer;
                                                        totaldistance = Math.Abs(totaldistance);
                                                        summeryRow["Distance(Kms)"] = totaldistance.ToString("00.00");
                                                        double Cost = 0;
                                                        double.TryParse(txtCost.Text, out Cost);
                                                        double totcost = 0;
                                                        totcost = totaldistance * Cost;
                                                        summeryRow["Total Cost"] = totcost.ToString("00.00");

                                                        DateTime l1et = Reachingdt;
                                                        DateTime l1lt = Startingdt;

                                                        TimeSpan l1ets = new TimeSpan(l1et.Ticks);
                                                        TimeSpan l1lts = new TimeSpan(l1lt.Ticks);
                                                        TimeSpan difftime = l1ets.Subtract(l1lts);

                                                        if (l1et.Ticks != l1lt.Ticks)
                                                        {
                                                            //summeryRow["Location1LeftTime"] = Prevrow["DateTime"];
                                                        }
                                                        else
                                                        {
                                                            summeryTable.Rows.Remove(summeryRow);
                                                            sno--;
                                                            summeryRow = null;
                                                            isfirlstlog = true;
                                                            break;
                                                        }
                                                        // summeryRow["Journey Hours"] = (int)(difftime.TotalHours % 24) + "Hours " + (int)(difftime.TotalMinutes % 60) + "Min ";
                                                        if ((int)(difftime.TotalDays) > 0)
                                                        {
                                                            summeryRow["Journey Hours"] = (int)(difftime.TotalDays) + "Days " + (int)(difftime.TotalHours % 24) + "Hours " + (int)(difftime.TotalMinutes % 60) + "Min ";
                                                        }
                                                        else
                                                        {
                                                            summeryRow["Journey Hours"] = (int)(difftime.TotalHours % 24) + "Hours " + (int)(difftime.TotalMinutes % 60) + "Min ";
                                                        
                                                        }


                                                        summeryRow["Reached Locations"] = touchedlocs;

                                                        islocation1 = false;
                                                    }
                                                    else
                                                    {
                                                        if (touchedlocs.Length > 0)
                                                        {
                                                            touchedlocs += "," + Brncs["BranchID"].ToString();
                                                        }
                                                        else
                                                        {
                                                            touchedlocs += Brncs["BranchID"].ToString();
                                                        }

                                                    }
                                                }
                                                else
                                                {

                                                    if (selectPlants.Select("BranchID='" + Brncs["BranchID"].ToString() + "'").Length > 0)
                                                    {

                                                        summeryRow = summeryTable.NewRow();
                                                        summeryRow["SNo"] = sno;
                                                        summeryRow["VehicleNo"] = tripdatarow["VehicleID"];
                                                        summeryRow["From Location"] = Brncs["BranchID"];
                                                        //   DateTime  Startingdt1 = (DateTime)tripdatarow["DateTime"];
                                                        summeryRow["Starting Date"] = Startingdt.ToString("M/dd/yyyy");
                                                        summeryRow["Starting Time"] = Startingdt.ToString("hh:mm:ss tt");

                                                        //summeryRow["Location1EnteredTime"] = tripdatarow["DateTime"];
                                                        sno++;
                                                        summeryTable.Rows.Add(summeryRow);
                                                        isfirlstlog = false;
                                                        islocation1 = true;
                                                        prevodometer = double.Parse(tripdatarow["Odometer"].ToString());

                                                    }
                                                }
                                            }
                                            if (statusobserver[Brncs["BranchID"].ToString()] == "Out Side")
                                            {

                                               

                                                if (summeryRow != null && Prevrow != null)
                                                {
                                                    Startingdt = (DateTime)tripdatarow["DateTime"];
                                                    string Startdate = Startingdt.ToString("M/dd/yyyy");
                                                    string startTime = Startingdt.ToString("hh:mm:ss tt");
                                                    string[] Reachsplt = startTime.ToString().Split(' ');
                                                    if (Reachsplt.Length > 1)
                                                    {
                                                        int departuretimemin = 0;
                                                        int dephours = 0;
                                                        int depmin = 0;
                                                        int.TryParse(Reachsplt[0].Split(':')[0], out dephours);
                                                        int.TryParse(Reachsplt[0].Split(':')[1], out depmin);
                                                        //departuretimemin = 720 - ((dephours * 60) + depmin);

                                                        if (Reachsplt[1] == "PM")
                                                        {
                                                            if (Reachsplt[0].Split(':')[0] == "12")
                                                                departuretimemin = ((dephours * 60) + depmin);
                                                            else
                                                                departuretimemin = 720 + ((dephours * 60) + depmin);
                                                        }
                                                        else
                                                        {
                                                            if (Reachsplt[0].Split(':')[0] == "12")
                                                                departuretimemin = ((dephours * 60) + depmin) - 720;
                                                            else
                                                                departuretimemin = ((dephours * 60) + depmin);
                                                        }

                                                        //ddlTravels.Items.Add(dr["traveler_agent"].ToString());

                                                        int time = departuretimemin;
                                                        if ((time % 60) < 10 && (time / 60) < 10)
                                                        {
                                                            StDuration = "0" + time / 60 + ":" + "0" + time % 60;
                                                        }
                                                        else if ((time % 60) >= 10 && (time / 60) < 10)
                                                        {
                                                            StDuration = "0" + time / 60 + ":" + time % 60;
                                                        }
                                                        else if ((time % 60) < 10 && (time / 60) >= 10)
                                                        {
                                                            StDuration = time / 60 + ":" + "0" + time % 60;
                                                        }
                                                        else if ((time % 60) >= 10 && (time / 60) >= 10)
                                                        {
                                                            StDuration = time / 60 + ":" + time % 60;
                                                        }

                                                    }
                                                    if (islocation1)
                                                    {

                                                        summeryRow["Starting Date"] = Startdate;
                                                        summeryRow["Starting Time"] = StDuration;


                                                        prevodometer = double.Parse(tripdatarow["Odometer"].ToString());
                                                    }
                                                    else
                                                    {
                                                        if (selectPlants.Select("BranchID='" + Brncs["BranchID"].ToString() + "'").Length > 0)
                                                        {
                                                            summeryRow = null;
                                                            //isfirlstlog = true;
                                                            touchedlocs = "";
                                                            summeryRow = summeryTable.NewRow();
                                                            summeryRow["SNo"] = sno;
                                                            summeryRow["VehicleNo"] = tripdatarow["VehicleID"];
                                                            summeryRow["From Location"] = Brncs["BranchID"];
                                                            //summeryRow["Location1EnteredTime"] = prevtime;
                                                            summeryRow["Starting Date"] = Startdate;
                                                            summeryRow["Starting Time"] = StDuration;

                                                            sno++;
                                                            summeryTable.Rows.Add(summeryRow);
                                                            prevodometer = double.Parse(tripdatarow["Odometer"].ToString());
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            foreach (DataRow Brncs1 in vehbarnches.Rows)
                                            {
                                                ////DataRow[] branch = vehbarnches.Select("BranchID='" + lstLocation + "'");

                                                double presLat1 = (double)tripdatarow["Latitiude"];
                                                double PresLng1 = (double)tripdatarow["Longitude"];

                                                //foreach (DataRow Brncs in branch)
                                                //{
                                                double ag_Lat1 = 0;
                                                double.TryParse(Brncs1["Latitude"].ToString(), out ag_Lat1);
                                                double ag_lng1 = 0;
                                                double.TryParse(Brncs1["Longitude"].ToString(), out ag_lng1);
                                                double ag_radious1 = 100;
                                                double.TryParse(Brncs1["Radious"].ToString(), out ag_radious1);
                                                string statusvalue1 = ddwnldr.getGeofenceStatus(presLat1, PresLng1, ag_Lat1, ag_lng1, ag_radious1);

                                                if (statusobserver[Brncs1["BranchID"].ToString()] != statusvalue1)
                                                {
                                                    statusobserver[Brncs1["BranchID"].ToString()] = statusvalue1;
                                                    if (statusobserver[Brncs1["BranchID"].ToString()] == "In Side")
                                                    {
                                                        if (touchedlocs.Length > 0)
                                                        {
                                                            touchedlocs += "," + Brncs1["BranchID"].ToString() + " (" + Reachingdt.ToString("HH:mm") + ")";
                                                        }
                                                        else
                                                        {
                                                            touchedlocs += Brncs1["BranchID"].ToString() + " (" + Reachingdt.ToString("HH:mm") + ")";
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        Prevrow = tripdatarow;
                                        //}
                                    }
                                }
                                
                                #endregion

                            }

                            if (summeryTable.Rows.Count > 0)
                            {
                                int snocnt = 1;
                                for (int cnt = 0; cnt < summeryTable.Rows.Count; cnt++)
                                {
                                    if (summeryTable.Rows[cnt]["To Location"].ToString() == "" || summeryTable.Rows[cnt]["Starting Date"].ToString() == "")
                                    {
                                        summeryTable.Rows.RemoveAt(cnt);
                                        cnt--;
                                    }
                                    else
                                    {
                                        summeryTable.Rows[cnt]["SNo"] = snocnt;
                                        snocnt++;
                                    }
                                }
                            }
                            DataRow newvartical = summeryTable.NewRow();
                            newvartical["From Location"] = "Total";
                            double val = 0.0;
                            foreach (DataColumn dc in summeryTable.Columns)
                            {
                                if (dc.DataType == typeof(Double))
                                {
                                    val = 0.0;
                                    double.TryParse(summeryTable.Compute("sum([" + dc.ToString() + "])", "[" + dc.ToString() + "]<>'0'").ToString(), out val);
                                    newvartical[dc.ToString()] = val;
                                }
                            }
                            summeryTable.Rows.Add(newvartical);
                            grdReports.DataSource = summeryTable;
                            grdReports.DataBind();
                            Session["xportdata"] = summeryTable;
                            divPieChart.Visible = false;
                            for (int i = 0; i < grdReports.Rows.Count; i++)
                            {
                                if (grdReports.Rows[i].Cells[3].Text == "Total")
                                {
                                    grdReports.Rows[i].Cells[0].Controls.Remove(grdReports.Rows[i].Cells[0].Controls[1]);
                                }
                            }
                        }
                        #endregion
                    }

                    else if (reportname == "Sales Trip Report")
                    {

                        #region Sales Trip Report
                        if (txtCost.Text == "")
                        {
                            lbl_nofifier.Text = "Enter Cost per km";
                        }
                        else
                        {
                            DateTime Startingdt = DateTime.Now;
                            DateTime ltl_Startingdt = DateTime.Now;
                            string Duration = "";
                            string StDuration = "";
                            ddwnldr = new DataDownloader();
                            ddwnldr.UpdateBranchDetails(UserName);
                            string vehicls = "";
                            ////string Status = "";
                            int sno = 1;
                            DataTable summeryTable = new DataTable();
                            DataColumn summeryColumn = new DataColumn("SNo");
                            summeryTable.Columns.Add(summeryColumn);
                            summeryColumn = new DataColumn("VehicleNo");
                            summeryTable.Columns.Add(summeryColumn);
                            summeryColumn = new DataColumn("From Location");
                            summeryTable.Columns.Add(summeryColumn);
                            //summeryColumn = new DataColumn("Location1EnteredTime");
                            //summeryColumn.DataType = System.Type.GetType("System.DateTime");
                            //summeryTable.Columns.Add(summeryColumn);
                            summeryColumn = new DataColumn("Starting Date Time");
                            //summeryColumn.DataType = System.Type.GetType("System.DateTime");
                            summeryTable.Columns.Add(summeryColumn);

                            //summeryColumn = new DataColumn("Starting Time");
                            //summeryTable.Columns.Add(summeryColumn);

                            //summeryColumn = new DataColumn("TimeSpentinL1");
                            //summeryTable.Columns.Add(summeryColumn);
                            summeryColumn = new DataColumn("To Location");
                            summeryTable.Columns.Add(summeryColumn);
                            summeryColumn = new DataColumn("Reaching Date Time");
                            //summeryColumn.DataType = System.Type.GetType("System.DateTime");
                            summeryTable.Columns.Add(summeryColumn);

                            //summeryColumn = new DataColumn("Reaching Time");
                            //summeryTable.Columns.Add(summeryColumn);


                            //summeryColumn = new DataColumn("Location2LeftTime");
                            //summeryColumn.DataType = System.Type.GetType("System.DateTime");
                            //summeryTable.Columns.Add(summeryColumn);
                            //summeryColumn = new DataColumn("TimeSpentinL2");
                            //summeryTable.Columns.Add(summeryColumn);
                            //summeryColumn = new DataColumn("Distance(Kms)");
                            summeryTable.Columns.Add("Distance(Kms)").DataType = typeof(Double);
                            //summeryTable.Columns.Add(summeryColumn).DataType = typeof(Double);
                            //summeryColumn = new DataColumn("Cost per kms");
                            //summeryTable.Columns.Add(summeryColumn);
                            summeryTable.Columns.Add("Total Cost").DataType = typeof(Double);
                            summeryColumn = new DataColumn("Journey Hours");
                            summeryTable.Columns.Add(summeryColumn);

                            summeryColumn = new DataColumn("Reached Locations");
                            summeryTable.Columns.Add(summeryColumn);


                            DataTable summeryTable_locationreport = summeryTable.Clone();

                            summeryTable_locationreport.Columns.Remove("VehicleNo");
                            summeryTable_locationreport.Columns.Remove("Total Cost");
                            summeryTable_locationreport.Columns.Remove("Reached Locations");
                            DataRow summeryRow = null;
                            DataRow summmeryltlrow = null;
                            double prevodometer = 0;
                            double ltl_prevodometer = 0;
                            double presodometer = 0;

                            //foreach (ListItem obj in chbl_vehicles.Items)
                            //{
                            //    if (obj.Selected)
                            //    {
                            //        vehicls += "'" + obj.Text + "',";
                            //    }
                            //}
                            //vehicls = vehicls.Substring(0, vehicls.LastIndexOf(','));

                            lbl_ReportStatus.Text = "Tanker and Puffs Trip REPORT FROM: " + fromdate.ToString("M/dd/yyyy") + "  To: " + todate.ToString("M/dd/yyyy");
                            Session["title"] = lbl_ReportStatus.Text;

                            //cmd = new MySqlCommand("SELECT        Sno, UserName, BranchID, Description, Latitude, Longitude, PhoneNumber, ImagePath, ImageType, Radious, PlantName, IsPlant, Category_sno,  direction FROM            branchdata WHERE        (UserName = 'modeldairy') AND (IsPlant = 1)");
                            //DataTable vehbarnches = vdm.SelectQuery(cmd).Tables[0];
                            //if (vehbarnches.Rows.Count == 0)
                            //{
                            //    return;
                            //}
                            //DataRow[] selectPlantsarray = vehbarnches.Select("Category_sno=0");
                            //DataTable selectPlants = vehbarnches.Clone();
                           

                            //if (selectPlantsarray.Length > 0)
                            //    selectPlants = selectPlantsarray.CopyToDataTable();




                            foreach (string vehiclestr in checkedvhcles)
                            {
                                DataTable selectPlants = new DataTable();
                                selectPlants.Columns.Add("Latitude");
                                selectPlants.Columns.Add("Longitude");
                                selectPlants.Columns.Add("Radious");
                                selectPlants.Columns.Add("BranchID");

                                cmd = new MySqlCommand("SELECT        tripconfiguration.Veh_Sno, paireddata.VehicleNumber, paireddata.UserID, routesubtable.Rank, routetable.RouteName, branchdata.Sno, branchdata.UserName, branchdata.BranchID, branchdata.Description, branchdata.Latitude, branchdata.Longitude, branchdata.PhoneNumber, branchdata.ImagePath, branchdata.ImageType, branchdata.Radious, branchdata.PlantName, branchdata.IsPlant, branchdata.Category_sno, branchdata.direction, routetable.startpoint, routetable.endpoint, branchdata_1.BranchID AS startpoint_name, branchdata_1.Latitude AS startpoint_lat, branchdata_1.Longitude AS startpoint_lng, branchdata_1.Radious AS startpoint_radius, branchdata_2.BranchID AS endpoint_name, branchdata_2.Latitude AS endpoint_Lat, branchdata_2.Longitude AS endpoint_lng, branchdata_2.Radious AS endpoint_radius FROM            tripconfiguration INNER JOIN routetable ON tripconfiguration.RouteID = routetable.SNo AND tripconfiguration.UserID = routetable.UserID INNER JOIN routesubtable ON routetable.SNo = routesubtable.SNo INNER JOIN paireddata ON tripconfiguration.UserID = paireddata.UserID AND tripconfiguration.Veh_Sno = paireddata.Sno INNER JOIN branchdata ON routesubtable.LocationID = branchdata.Sno INNER JOIN branchdata branchdata_2 ON routetable.startpoint = branchdata_2.Sno INNER JOIN branchdata branchdata_1 ON routetable.endpoint = branchdata_1.Sno WHERE        (tripconfiguration.UserID = @uid) AND (paireddata.VehicleNumber = @vnum)");
                                cmd.Parameters.Add("@uid", mainuser);
                                cmd.Parameters.Add("@vnum", vehiclestr);
                                DataTable vehbarnches = vdm.SelectQuery(cmd).Tables[0];

                                DataTable selectPlants_data = vehbarnches.DefaultView.ToTable(true, "startpoint_name", "startpoint_lat", "startpoint_lng", "startpoint_radius", "endpoint_name", "endpoint_Lat", "endpoint_lng", "endpoint_radius");
                                Dictionary<string, string> statusobserver = new Dictionary<string, string>();

                                foreach (DataRow spdr in selectPlants_data.Rows)
                                {
                                    //DataRow addplantsalso=  vehbarnches.NewRow();


                                    DataRow new_row = selectPlants.NewRow();
                                    new_row["BranchID"] = spdr["startpoint_name"].ToString();
                                    new_row["Latitude"] = spdr["startpoint_lat"].ToString();
                                    new_row["Longitude"] = spdr["startpoint_lng"].ToString();
                                    new_row["Radious"] = spdr["startpoint_radius"].ToString();
                                    selectPlants.Rows.Add(new_row);



                                    //addplantsalso["BranchID"] = spdr["startpoint_name"].ToString();
                                    //addplantsalso["Latitude"] = spdr["startpoint_lat"].ToString();
                                    //addplantsalso["Longitude"] = spdr["startpoint_lng"].ToString();
                                    //addplantsalso["Radious"] = spdr["startpoint_radius"].ToString();
                                    //vehbarnches.Rows.Add(addplantsalso);
                                    statusobserver.Add(spdr["startpoint_name"].ToString(), "");

                                    new_row = selectPlants.NewRow();
                                    new_row["BranchID"] = spdr["endpoint_name"].ToString();
                                    new_row["Latitude"] = spdr["endpoint_Lat"].ToString();
                                    new_row["Longitude"] = spdr["endpoint_lng"].ToString();
                                    new_row["Radious"] = spdr["endpoint_radius"].ToString();
                                    selectPlants.Rows.Add(new_row);
                                    //addplantsalso = vehbarnches.NewRow();

                                    //addplantsalso["BranchID"] = spdr["endpoint_name"].ToString();
                                    //addplantsalso["Latitude"] = spdr["endpoint_Lat"].ToString();
                                    //addplantsalso["Longitude"] = spdr["endpoint_lng"].ToString();
                                    //addplantsalso["Radious"] = spdr["endpoint_radius"].ToString();
                                    //vehbarnches.Rows.Add(addplantsalso);

                                    statusobserver.Add(spdr["endpoint_name"].ToString(), "");
                                }



                                Maxspeed = 0;
                                bool isfirlstlog = true;
                                bool islocation1 = true;

                                DataTable TripData = new DataTable();
                                DataTable logs = new DataTable();
                                DataTable tottable = new DataTable();


                                if (selectPlants.Rows.Count == 0)
                                {
                                    lbl_nofifier.Text += "Trip Assignment Not Done to " + vehiclestr + ". Please Check in \"Route_Management_Sales\" and TripAssignment";
                                    continue;
                                }
                                DateTime decidedfromdate = GoBackandGetPlantTouchdate(logstbls, fromdate, selectPlants, vehiclestr);


                                foreach (string tbname in logstbls)
                                {
                                    cmd = new MySqlCommand("SELECT '' AS SNo, " + tbname + ".VehicleID, " + tbname + ".DateTime, " + tbname + ".Speed, " + tbname + ".Latitiude, " + tbname + ".Longitude, " + tbname + ".Direction, " + tbname + ".Diesel, " + tbname + ".Odometer, " + tbname + ".Direction AS Expr1, " + tbname + ".Direction AS Expr2, vehiclemaster.MaintenancePlantName, vehiclemaster.VendorName, vehiclemaster.VendorNo, vehiclemaster.VehicleTypeName FROM " + tbname + " LEFT OUTER JOIN vehiclemaster ON " + tbname + ".VehicleID = vehiclemaster.VehicleID WHERE (" + tbname + ".DateTime >= @starttime) AND (" + tbname + ".DateTime <= @endtime) AND (" + tbname + ".VehicleID = '" + vehiclestr + "') and (" + tbname + ".UserID='" + mainuser + "')  ORDER BY " + tbname + ".DateTime");
                                    //cmd = new MySqlCommand("select * from " + tbname + " where DateTime>= @starttime and DateTime<=@endtime and VehicleID='" + vehiclestr + "' and UserID='" + UserName + "' order by DateTime");
                                    cmd.Parameters.Add(new MySqlParameter("@starttime", decidedfromdate));//.AddMinutes(-30)));
                                    cmd.Parameters.Add(new MySqlParameter("@endtime", todate));
                                    logs = vdm.SelectQuery(cmd).Tables[0];
                                    if (tottable.Rows.Count == 0)
                                    {
                                        tottable = logs.Clone();
                                    }
                                    foreach (DataRow dr in logs.Rows)
                                    {
                                        tottable.ImportRow(dr);
                                    }
                                }
                                DataView dv = tottable.DefaultView;
                                dv.Sort = "DateTime ASC";
                                TripData = dv.ToTable();


                                #region for all locations
                                DataRow Prevrow = null;
                                summeryRow = null;
                                // cmd = new MySqlCommand("SELECT BranchID, Latitude, Longitude, PhoneNumber, PlantName, UserName, Radious, ImageType, ImagePath, IsPlant FROM branchdata WHERE (PlantName IN (SELECT branchdata_1.Sno FROM cabmanagement INNER JOIN branchdata branchdata_1 ON cabmanagement.PlantName = branchdata_1.BranchID WHERE (cabmanagement.UserID = '" + UserName + "') AND (cabmanagement.VehicleID = '" + vehiclestr + "'))) AND (UserName = '" + UserName + "')");

                                foreach (DataRow dr in vehbarnches.Rows)
                                {
                                    if (!statusobserver.ContainsKey(dr["BranchID"].ToString()))
                                    {
                                        statusobserver.Add(dr["BranchID"].ToString(), "");
                                    }
                                }

                                string touchedlocs = "";
                                foreach (DataRow tripdatarow in TripData.Rows)
                                {
                                    DateTime Reachingdt = (DateTime)tripdatarow["DateTime"];

                                    foreach (DataRow Brncs in selectPlants.Rows)
                                    //foreach (DataRow Brncs in vehbarnches.Rows)
                                    {
                                        ////DataRow[] branch = vehbarnches.Select("BranchID='" + lstLocation + "'");

                                        double presLat = (double)tripdatarow["Latitiude"];
                                        double PresLng = (double)tripdatarow["Longitude"];

                                        //foreach (DataRow Brncs in branch)
                                        //{
                                        double ag_Lat = 0;
                                        double.TryParse(Brncs["Latitude"].ToString(), out ag_Lat);
                                        double ag_lng = 0;
                                        double.TryParse(Brncs["Longitude"].ToString(), out ag_lng);
                                        double ag_radious = 100;
                                        double.TryParse(Brncs["Radious"].ToString(), out ag_radious);
                                        string statusvalue = ddwnldr.getGeofenceStatus(presLat, PresLng, ag_Lat, ag_lng, ag_radious);
                                        presodometer = double.Parse(tripdatarow["Odometer"].ToString());

                                        if (statusobserver[Brncs["BranchID"].ToString()] != statusvalue)
                                        {
                                            statusobserver[Brncs["BranchID"].ToString()] = statusvalue;
                                            if (statusobserver[Brncs["BranchID"].ToString()] == "In Side")
                                            {
                                                if (!isfirlstlog)
                                                {
                                                    string Reachdate = Reachingdt.ToString("M/dd/yyyy");
                                                    string ReachTime = Reachingdt.ToString("HH:mm");



                                                    DateTime l1et = Reachingdt;
                                                    DateTime l1lt = Startingdt;

                                                    TimeSpan l1ets = new TimeSpan(l1et.Ticks);
                                                    TimeSpan l1lts = new TimeSpan(l1lt.Ticks);
                                                    TimeSpan difftime = l1ets.Subtract(l1lts);





                                                    if (selectPlants.Select("BranchID='" + Brncs["BranchID"].ToString() + "'").Length > 0)
                                                    {
                                                        if (summeryRow["From Location"].ToString() != Brncs["BranchID"].ToString())
                                                        {
                                                            summeryRow["To Location"] = Brncs["BranchID"];
                                                            summeryRow["Reaching Date Time"] = Reachdate + " " + ReachTime;
                                                            //summeryRow["Reaching Time"] = Duration;

                                                            if (presodometer < prevodometer)
                                                            {
                                                                try
                                                                {
                                                                    
                                                                        summeryTable.Rows.Remove(summeryRow);
                                                                        sno--;
                                                                   
                                                                }
                                                                catch
                                                                {
                                                                }
                                                                summeryRow = null;
                                                                isfirlstlog = true;
                                                                break;
                                                            }
                                                            double totaldistance = presodometer - prevodometer;
                                                            totaldistance = Math.Abs(totaldistance);
                                                            summeryRow["Distance(Kms)"] = totaldistance.ToString("00.00");
                                                            double Cost = 0;
                                                            double.TryParse(txtCost.Text, out Cost);
                                                            double totcost = 0;
                                                            totcost = totaldistance * Cost;
                                                            summeryRow["Total Cost"] = totcost.ToString("00.00");

                                                            //DateTime l1et = Reachingdt;
                                                            //DateTime l1lt = Startingdt;

                                                            //TimeSpan l1ets = new TimeSpan(l1et.Ticks);
                                                            //TimeSpan l1lts = new TimeSpan(l1lt.Ticks);
                                                            //TimeSpan difftime = l1ets.Subtract(l1lts);

                                                            if (l1et.Ticks != l1lt.Ticks)
                                                            {
                                                                //summeryRow["Location1LeftTime"] = Prevrow["DateTime"];
                                                            }
                                                            else
                                                            {
                                                                summeryTable.Rows.Remove(summeryRow);
                                                                sno--;
                                                                summeryRow = null;
                                                                isfirlstlog = true;
                                                                break;
                                                            }
                                                            // summeryRow["Journey Hours"] = (int)(difftime.TotalHours % 24) + "Hours " + (int)(difftime.TotalMinutes % 60) + "Min ";
                                                            if ((int)(difftime.TotalDays) > 0)
                                                            {
                                                                summeryRow["Journey Hours"] = (int)(difftime.TotalDays) + "Days " + (int)(difftime.TotalHours % 24) + "Hours " + (int)(difftime.TotalMinutes % 60) + "Min ";
                                                            }
                                                            else
                                                            {
                                                                summeryRow["Journey Hours"] = (int)(difftime.TotalHours % 24) + "Hours " + (int)(difftime.TotalMinutes % 60) + "Min ";

                                                            }


                                                            summeryRow["Reached Locations"] = touchedlocs;

                                                            islocation1 = false;
                                                        }
                                                        else
                                                        {
                                                            summeryTable.Rows.RemoveAt(summeryTable.Rows.Count - 1);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (touchedlocs.Length > 0)
                                                        {
                                                            touchedlocs += "," + Brncs["BranchID"].ToString() + " (" + Reachingdt.ToString("HH:mm") + ")";
                                                        }
                                                        else
                                                        {
                                                            touchedlocs += Brncs["BranchID"].ToString() + " (" + Reachingdt.ToString("HH:mm") + ")";
                                                        }

                                                    }
                                                }
                                                else
                                                {


                                                    if (selectPlants.Select("BranchID='" + Brncs["BranchID"].ToString() + "'").Length > 0)
                                                    {

                                                        summeryRow = summeryTable.NewRow();
                                                        summeryRow["SNo"] = sno;
                                                        summeryRow["VehicleNo"] = tripdatarow["VehicleID"];
                                                        summeryRow["From Location"] = Brncs["BranchID"];
                                                        //   DateTime  Startingdt1 = (DateTime)tripdatarow["DateTime"];
                                                        summeryRow["Starting Date Time"] = Startingdt.ToString("M/dd/yyyy hh:mm:ss tt");
                                                        //summeryRow["Starting Time"] = Startingdt.ToString("hh:mm:ss tt");

                                                        //summeryRow["Location1EnteredTime"] = tripdatarow["DateTime"];
                                                        sno++;
                                                        summeryTable.Rows.Add(summeryRow);
                                                        isfirlstlog = false;
                                                        islocation1 = true;
                                                        prevodometer = double.Parse(tripdatarow["Odometer"].ToString());

                                                    }
                                                }
                                            }
                                            if (statusobserver[Brncs["BranchID"].ToString()] == "Out Side")
                                            {
                                                if (summeryRow != null && Prevrow != null)
                                                {
                                                    Startingdt = (DateTime)tripdatarow["DateTime"];
                                                    string Startdate = Startingdt.ToString("M/dd/yyyy");
                                                    string startTime = Startingdt.ToString("HH:mm");

                                                    if (islocation1)
                                                    {

                                                        summeryRow["Starting Date Time"] = Startdate + " " + startTime;
                                                        // summeryRow["Starting Time"] = StDuration;
                                                        prevodometer = double.Parse(tripdatarow["Odometer"].ToString());
                                                    }
                                                    else
                                                    {


                                                        if (selectPlants.Select("BranchID='" + Brncs["BranchID"].ToString() + "'").Length > 0)
                                                        {
                                                            summeryRow = null;
                                                            //isfirlstlog = true;
                                                            touchedlocs = "";
                                                            summeryRow = summeryTable.NewRow();
                                                            summeryRow["SNo"] = sno;
                                                            summeryRow["VehicleNo"] = tripdatarow["VehicleID"];
                                                            summeryRow["From Location"] = Brncs["BranchID"];
                                                            //summeryRow["Location1EnteredTime"] = prevtime;
                                                            summeryRow["Starting Date Time"] = Startdate + " " + startTime;
                                                            // summeryRow["Starting Time"] = StDuration;

                                                            sno++;
                                                            summeryTable.Rows.Add(summeryRow);
                                                            prevodometer = double.Parse(tripdatarow["Odometer"].ToString());
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            foreach (DataRow Brncs1 in vehbarnches.Rows)
                                            {
                                                ////DataRow[] branch = vehbarnches.Select("BranchID='" + lstLocation + "'");

                                                double presLat1 = (double)tripdatarow["Latitiude"];
                                                double PresLng1 = (double)tripdatarow["Longitude"];

                                                //foreach (DataRow Brncs in branch)
                                                //{
                                                double ag_Lat1 = 0;
                                                double.TryParse(Brncs1["Latitude"].ToString(), out ag_Lat1);
                                                double ag_lng1 = 0;
                                                double.TryParse(Brncs1["Longitude"].ToString(), out ag_lng1);
                                                double ag_radious1 = 100;
                                                double.TryParse(Brncs1["Radious"].ToString(), out ag_radious1);
                                                string statusvalue1 = ddwnldr.getGeofenceStatus(presLat1, PresLng1, ag_Lat1, ag_lng1, ag_radious1);

                                                if (statusobserver[Brncs1["BranchID"].ToString()] != statusvalue1)
                                                {
                                                    statusobserver[Brncs1["BranchID"].ToString()] = statusvalue1;
                                                    if (statusobserver[Brncs1["BranchID"].ToString()] == "In Side")
                                                    {

                                                        #region code here
                                                        if (summmeryltlrow != null)
                                                        {
                                                            summmeryltlrow["SNo"] = sno;
                                                            summmeryltlrow["To Location"] = Brncs1["BranchID"];
                                                            summmeryltlrow["Reaching Date Time"] = Reachingdt.ToString("M/dd/yyyy HH:mm");
                                                            double totalloldistance = presodometer - ltl_prevodometer;
                                                            totalloldistance = Math.Abs(totalloldistance);
                                                            summmeryltlrow["Distance(Kms)"] = totalloldistance.ToString("00.00");


                                                            DateTime l1et = Reachingdt;
                                                            DateTime l1lt = ltl_Startingdt;

                                                            TimeSpan l1ets = new TimeSpan(l1et.Ticks);
                                                            TimeSpan l1lts = new TimeSpan(l1lt.Ticks);
                                                            TimeSpan difftime = l1ets.Subtract(l1lts);


                                                            if ((int)(difftime.TotalDays) > 0)
                                                            {
                                                                summmeryltlrow["Journey Hours"] = (int)(difftime.TotalDays) + "Days " + (int)(difftime.TotalHours % 24) + "Hours " + (int)(difftime.TotalMinutes % 60) + "Min ";
                                                            }
                                                            else
                                                            {
                                                                summmeryltlrow["Journey Hours"] = (int)(difftime.TotalHours % 24) + "Hours " + (int)(difftime.TotalMinutes % 60) + "Min ";
                                                            }

                                                        }




                                                        #endregion
                                                        if (touchedlocs.Length > 0)
                                                        {
                                                            touchedlocs += "," + Brncs1["BranchID"].ToString() + " (" + Reachingdt.ToString("HH:mm") + ")";
                                                        }
                                                        else
                                                        {
                                                            touchedlocs += Brncs1["BranchID"].ToString() + " (" + Reachingdt.ToString("HH:mm") + ")";
                                                        }
                                                    }
                                                    else
                                                    {
                                                        summmeryltlrow = summeryTable_locationreport.NewRow();
                                                        
                                                        //summmeryltlrow["VehicleNo"] = tripdatarow["VehicleID"];
                                                        summmeryltlrow["From Location"] = Brncs1["BranchID"];
                                                        //summeryRow["Location1EnteredTime"] = prevtime;
                                                        summmeryltlrow["Starting Date Time"] = Reachingdt.ToString("M/dd/yyyy HH:mm");
                                                        // summeryRow["Starting Time"] = StDuration;
                                                        ltl_Startingdt = Reachingdt;
                                                        ltl_prevodometer = double.Parse(tripdatarow["Odometer"].ToString());
                                                        summeryTable_locationreport.Rows.Add(summmeryltlrow);

                                                    }
                                                }
                                            }
                                        }
                                        Prevrow = tripdatarow;
                                        //}
                                    }
                                }

                                #endregion

                            }
                            DataTable finaltable_summeryTable_location = summeryTable_locationreport.Clone();
                            if (summeryTable.Rows.Count > 0)
                            {
                                int snocnt = 1;
                                for (int cnt = 0; cnt < summeryTable.Rows.Count; cnt++)
                                {
                                    if (summeryTable.Rows[cnt]["To Location"].ToString() == "" || summeryTable.Rows[cnt]["Starting Date Time"].ToString() == "")
                                    {
                                        summeryTable.Rows.RemoveAt(cnt);
                                        cnt--;
                                    }
                                    else
                                    {
                                        string getsno = summeryTable.Rows[cnt]["SNo"].ToString();
                                        DataRow[] summerytable_rows = summeryTable_locationreport.Select("SNo='" + (int.Parse(getsno) + 1)+"'");
                                        if (summerytable_rows.Length > 0)
                                            for (int ddrrc = 0; ddrrc < summerytable_rows.Length; ddrrc++)
                                            {
                                                DataRow ddrr = summerytable_rows[ddrrc];
                                                if (ddrr["To Location"].ToString() != "")
                                                {
                                                    ddrr["SNo"] = snocnt;
                                                    finaltable_summeryTable_location.ImportRow(ddrr);
                                                }
                                            }
                                        summeryTable.Rows[cnt]["SNo"] = snocnt;
                                        snocnt++;
                                    }
                                }
                            }
                            DataRow newvartical = summeryTable.NewRow();
                            newvartical["From Location"] = "Total";
                            double val = 0.0;
                            foreach (DataColumn dc in summeryTable.Columns)
                            {
                                if (dc.DataType == typeof(Double))
                                {
                                    val = 0.0;
                                    double.TryParse(summeryTable.Compute("sum([" + dc.ToString() + "])", "[" + dc.ToString() + "]<>'0'").ToString(), out val);
                                    newvartical[dc.ToString()] = val;
                                }
                            }
                            summeryTable.Rows.Add(newvartical);
                            grdReports.DataSource = summeryTable;
                            grdReports.DataBind();
                            Session["xportdata"] = summeryTable;
                            divPieChart.Visible = false;
                            for (int i = 0; i < grdReports.Rows.Count; i++)
                            {
                                if (grdReports.Rows[i].Cells[3].Text == "Total")
                                {
                                    grdReports.Rows[i].Cells[0].Controls.Remove(grdReports.Rows[i].Cells[0].Controls[1]);
                                }
                            }
                            Session["salestripreportdata"] = finaltable_summeryTable_location;
                        }

                        #endregion

                    }
					// else if (reportname == "Tanker and Puffs Trip Report")
                    else if (reportname == "Vehicle Remarks Report")
                    {
                        #region Vehicle Remarks Report
                        DataTable Remarks = new DataTable();
                        Remarks.Columns.Add("VehicleNo");
                        Remarks.Columns.Add("DateTime");
                        Remarks.Columns.Add("Remarks");
                        Remarks.Columns.Add("User");
                        foreach (string vehiclestr in checkedvhcles)
                        {
                            cmd = new MySqlCommand("SELECT vehicleno as VehicleNo, datetime as DateTime, remarks as Remarks,username FROM vehicleremarks WHERE  vehicleno=@vehicleno and datetime>= @starttime and datetime<=@endtime ORDER BY username");
                            cmd.Parameters.Add(new MySqlParameter("@vehicleno", vehiclestr));
                            cmd.Parameters.Add(new MySqlParameter("@starttime", fromdate));
                            cmd.Parameters.Add(new MySqlParameter("@endtime", todate));
                            DataTable dtble = vdm.SelectQuery(cmd).Tables[0];
                            foreach (DataRow dr in dtble.Rows)
                            {
                                DataRow newrow = Remarks.NewRow();
                                newrow["VehicleNo"] = dr["VehicleNo"].ToString();
                                newrow["DateTime"] = dr["DateTime"].ToString();
                                newrow["Remarks"] = dr["Remarks"].ToString();
                                newrow["User"] = dr["username"].ToString();
                                Remarks.Rows.Add(newrow);
                            }
                        }
                        lbl_ReportStatus.Text = "VEHICLE REMARKS REPORT FROM: " + fromdate.ToString() + "  To: " + todate.ToString() + "";
                        Session["title"] = lbl_ReportStatus.Text;

                        grdReports.DataSource = Remarks;
                        Session["xportdata"] = Remarks;

                        grdReports.DataBind();
                        for (int i = 0; i < grdReports.Rows.Count; i++)
                        {
                            //grdReports.Rows[i].Cells[1].Text = (i+1).ToString();
                            grdReports.Rows[i].Cells[0].Controls.Remove(grdReports.Rows[i].Cells[0].Controls[1]);
                            grdReports.Rows[i].Cells[0].Controls.Remove(grdReports.Rows[i].Cells[0].Controls[2]);
                        }
                        #endregion
                    }
                    else if (reportname == "Trips Report")
                    {
                        #region tripsreport
                        DataTable summeryTable = new DataTable();
                        DataColumn summeryColumn = new DataColumn("SNo");
                        summeryTable.Columns.Add(summeryColumn);
                        summeryColumn = new DataColumn("Date");
                        summeryTable.Columns.Add(summeryColumn);
                        summeryColumn = new DataColumn("VehicleNo");
                        summeryTable.Columns.Add(summeryColumn);
                        summeryColumn = new DataColumn("Route Name");
                        summeryTable.Columns.Add(summeryColumn);
                        summeryColumn = new DataColumn("AM StartTime");
                        summeryTable.Columns.Add(summeryColumn);
                        summeryColumn = new DataColumn("AM EndTime");
                        summeryTable.Columns.Add(summeryColumn);
                        summeryColumn = new DataColumn("AM GPS KMS");
                        summeryTable.Columns.Add(summeryColumn);
                        summeryColumn = new DataColumn("AM EMPTY KMS");
                        summeryTable.Columns.Add(summeryColumn);
                        summeryColumn = new DataColumn("AM TOTAL KMS");
                        summeryTable.Columns.Add(summeryColumn);
                        summeryColumn = new DataColumn("PM StartTime");
                        summeryTable.Columns.Add(summeryColumn);
                        summeryColumn = new DataColumn("PM EndTime");
                        summeryTable.Columns.Add(summeryColumn);
                        summeryColumn = new DataColumn("PM GPS KMS");
                        summeryTable.Columns.Add(summeryColumn);
                        summeryColumn = new DataColumn("PM EMPTY KMS");
                        summeryTable.Columns.Add(summeryColumn);
                        summeryColumn = new DataColumn("PM TOTAL KMS");
                        summeryTable.Columns.Add(summeryColumn);
                        summeryColumn = new DataColumn("Total KMS");
                        summeryTable.Columns.Add(summeryColumn);
                        summeryColumn = new DataColumn("Remarks");
                        summeryTable.Columns.Add(summeryColumn);
                        DataRow summeryRow = null;
                        int sno = 1;
                        lbl_ReportStatus.Text = "Trips Report From: " + fromdate.ToString() + "  To: " + todate.ToString();// +" and  TotalDistance Travelled:" + (int)TotalDistance + "\n" +
                        foreach (string vehiclestr in checkedvhcles)
                        {
                            double AMgpskms = 0.0;
                            double AMemptykms = 0.0;
                            double PMgpskms = 0.0;
                            double PMemptykms = 0.0;
                            double AMtotkms = 0.0;
                            double PMtotkms = 0.0;
                            double AMPMtotkms = 0.0;
                            for (DateTime date = fromdate; GetHighDate(todate).CompareTo(GetLowDate(date)) > 0; date = date.AddDays(1.0))
                            {
                                AMfromdate = date.AddHours(1).AddMinutes(30);
                                AMtodate = date.AddHours(11).AddMinutes(30);
                                PMfromdate = date.AddHours(15).AddMinutes(30);
                                PMtodate = date.AddHours(23).AddMinutes(30);
                                TimeSpan spanAMfromtime = new TimeSpan(AMfromdate.Hour, AMfromdate.Minute, AMfromdate.Second);
                                TimeSpan spanAMtotime = new TimeSpan(AMtodate.Hour, AMtodate.Minute, AMtodate.Second);
                                TimeSpan spanPMfromtime = new TimeSpan(PMfromdate.Hour, PMfromdate.Minute, PMfromdate.Second);
                                TimeSpan spanPMtotime = new TimeSpan(PMtodate.Hour, PMtodate.Minute, PMtodate.Second);
                                cmd = new MySqlCommand("SELECT paireddata.VehicleNumber, tripconfiguration.PlantName, routetable.RouteName, tripconfiguration.UserID, tripconfiguration.StartTime, tripconfiguration.EndTime, routesubtable.Rank, branchdata.BranchID, branchdata.Latitude, branchdata.Longitude, branchdata.Radious,routesubtable.SNo FROM routesubtable INNER JOIN routetable ON routesubtable.SNo = routetable.SNo INNER JOIN tripconfiguration ON routetable.SNo = tripconfiguration.RouteID INNER JOIN branchdata ON routesubtable.LocationID = branchdata.Sno INNER JOIN paireddata ON tripconfiguration.UserID = paireddata.UserID AND tripconfiguration.Veh_Sno = paireddata.Sno WHERE (tripconfiguration.UserID = @UserID) AND (paireddata.VehicleNumber = @VehicleNumber) AND (tripconfiguration.StartTime > @tme) AND (tripconfiguration.StartTime < @tme1)  ORDER BY tripconfiguration.sno, routesubtable.Rank");
                                cmd.Parameters.Add("@tme", spanAMfromtime);
                                cmd.Parameters.Add("@tme1", spanAMtotime);
                                cmd.Parameters.Add("@UserID", UserName);
                                cmd.Parameters.Add("@VehicleNumber", vehiclestr);
                                DataTable AMresult = vdm.SelectQuery(cmd).Tables[0];
                                cmd = new MySqlCommand("SELECT paireddata.VehicleNumber, tripconfiguration.PlantName, routetable.RouteName, tripconfiguration.UserID, tripconfiguration.StartTime, tripconfiguration.EndTime, routesubtable.Rank, branchdata.BranchID, branchdata.Latitude, branchdata.Longitude, branchdata.Radious,routesubtable.SNo FROM routesubtable INNER JOIN routetable ON routesubtable.SNo = routetable.SNo INNER JOIN tripconfiguration ON routetable.SNo = tripconfiguration.RouteID INNER JOIN branchdata ON routesubtable.LocationID = branchdata.Sno INNER JOIN paireddata ON tripconfiguration.UserID = paireddata.UserID AND tripconfiguration.Veh_Sno = paireddata.Sno WHERE (tripconfiguration.UserID = @UserID) AND (paireddata.VehicleNumber = @VehicleNumber) AND (tripconfiguration.StartTime > @tme) AND (tripconfiguration.StartTime < @tme1)  ORDER BY tripconfiguration.sno, routesubtable.Rank");
                                cmd.Parameters.Add("@tme", spanPMfromtime);
                                cmd.Parameters.Add("@tme1", spanPMtotime);
                                cmd.Parameters.Add("@UserID", UserName);
                                cmd.Parameters.Add("@VehicleNumber", vehiclestr);
                                DataTable PMresult = vdm.SelectQuery(cmd).Tables[0];

                                DataTable amuniqueTrips = AMresult.DefaultView.ToTable(true, "sno", "StartTime", "EndTime", "RouteName", "VehicleNumber");
                                DataTable amunderTrips = AMresult.DefaultView.ToTable(true, "sno", "rank", "BranchID", "StartTime", "Latitude", "Longitude", "Radious");
                                DataTable pmuniqueTrips = PMresult.DefaultView.ToTable(true, "sno", "StartTime", "EndTime", "RouteName", "VehicleNumber");
                                DataTable pmunderTrips = PMresult.DefaultView.ToTable(true, "sno", "rank", "BranchID", "StartTime", "Latitude", "Longitude", "Radious");

                                cmd = new MySqlCommand("SELECT branchdata.BranchID, branchdata.UserName, branchdata.Description, branchdata.Latitude, branchdata.Longitude, branchdata.PhoneNumber, branchdata.ImagePath, branchdata.ImageType, branchdata.Radious, branchdata.PlantName FROM routesubtable INNER JOIN branchdata ON routesubtable.LocationID = branchdata.Sno WHERE (routesubtable.SNo = @SNo) ORDER BY routesubtable.Rank");
                                cmd.Parameters.Add("@SNo", AMresult.Rows[0]["SNo"].ToString());
                                DataTable routesubtabledata = vdm.SelectQuery(cmd).Tables[0];
                                DDL_locations.Items.Clear();
                                DDL_locations.Items.Add("ALL");
                                foreach (DataRow dr in routesubtabledata.Rows)
                                {
                                    DDL_locations.Items.Add(dr["BranchID"].ToString().TrimEnd());
                                }

                                DateTime tamstarttime = DateTime.Now;
                                DateTime tamendtime = DateTime.Now;
                                DateTime tpmstarttime = DateTime.Now;
                                DateTime tpmendtime = DateTime.Now;

                                double odometervalue = 0;
                                summeryRow = summeryTable.NewRow();
                                summeryRow["SNo"] = sno;
                                summeryRow["Date"] = date.ToShortDateString();
                                summeryRow["VehicleNo"] = vehiclestr;
                                summeryRow["Route Name"] = AMresult.Rows[0]["RouteName"].ToString();
                                double totkms = 0.0;

                                if (amuniqueTrips.Rows.Count > 0)
                                {
                                    if (amuniqueTrips.Rows[0]["StartTime"].ToString() == "")
                                    {
                                        lbl_nofifier.Text = "There is no data in this trip";
                                        return;
                                    }
                                    tamstarttime = date.Date.Add(TimeSpan.Parse(amuniqueTrips.Rows[0]["StartTime"].ToString()));
                                    //tstarttime = (DateTime)uniqueTrips.Rows[0]["StartTime"];
                                    if (amuniqueTrips.Rows[0]["EndTime"].ToString() == "")
                                    {
                                        summeryRow["AM GPS KMS"] = 0;
                                        summeryRow["AM EMPTY KMS"] = 0;
                                        summeryRow["AM TOTAL KMS"] = "Trip not completed";
                                    }
                                    tamendtime = date.Date.AddHours(13);
                                    DataTable ResDT = new DataTable();
                                    ResDT = getfirsttouchOdometer(amuniqueTrips.Rows[0]["VehicleNumber"].ToString(), tamstarttime, tamendtime, vdm, routesubtabledata, out odometervalue, mainuser);
                                    if (ResDT.Rows.Count == 1)
                                    {
                                        summeryRow["AM GPS KMS"] = 0;
                                        summeryRow["AM EMPTY KMS"] = 0;
                                        summeryRow["AM TOTAL KMS"] = "Trip not completed";
                                    }
                                    else if (ResDT.Rows.Count == 2)
                                    {
                                        DataTable routes = new DataTable();
                                        if (Session["routes"] != null)
                                        {
                                            routes = (DataTable)Session["routes"];
                                        }
                                        else
                                        {
                                            cmd = new MySqlCommand("SELECT routetable.RouteName, tripconfiguration.StartTime, tripconfiguration.EndTime, routetable.SNo, tripconfiguration.Kms, tripconfiguration.extrakms, tripconfiguration.Chargeperkm FROM tripconfiguration INNER JOIN branchdata ON tripconfiguration.PlantName = branchdata.Sno INNER JOIN cabmanagement ON branchdata.BranchID = cabmanagement.PlantName INNER JOIN loginsconfigtable ON cabmanagement.VehicleID = loginsconfigtable.VehicleID INNER JOIN loginstable ON loginsconfigtable.Refno = loginstable.refno INNER JOIN routetable ON tripconfiguration.RouteID = routetable.SNo WHERE (loginstable.loginid = @userid) GROUP BY routetable.RouteName");
                                            //cmd = new MySqlCommand("SELECT routetable.RouteName, tripconfiguration.StartTime, tripconfiguration.EndTime, routetable.SNo, tripconfiguration.Kms,tripconfiguration.extrakms,tripconfiguration.Chargeperkm FROM loginsconfigtable INNER JOIN loginstable ON loginsconfigtable.Refno = loginstable.refno INNER JOIN cabmanagement ON loginsconfigtable.VehicleID = cabmanagement.VehicleID INNER JOIN vehiclemanage ON cabmanagement.PlantName = vehiclemanage.ItemName INNER JOIN tripconfiguration ON vehiclemanage.SNo = tripconfiguration.PlantName INNER JOIN routetable ON tripconfiguration.RouteID = routetable.SNo WHERE (loginstable.loginid = @userid) GROUP BY routetable.RouteName");
                                            cmd.Parameters.Add("@userid", UserName);
                                            routes = vdm.SelectQuery(cmd).Tables[0];
                                            Session["routes"] = routes;
                                        }
                                        DataRow[] rows = routes.Select("SNo='" + AMresult.Rows[0]["SNo"].ToString() + "'");
                                        double extrakms = 0;
                                        if (rows.Length > 0)
                                        {
                                            double.TryParse(rows[0]["extrakms"].ToString(), out extrakms);
                                        }
                                        summeryRow["AM StartTime"] = ResDT.Rows[0]["Reaching Time"].ToString();
                                        summeryRow["AM EndTime"] = ResDT.Rows[1]["Reaching Time"].ToString();
                                        summeryRow["AM EMPTY KMS"] = extrakms;
                                        AMemptykms += extrakms;
                                        odometervalue = Math.Round(odometervalue, 2);
                                        summeryRow["AM GPS KMS"] = odometervalue;
                                        AMgpskms += odometervalue;
                                        odometervalue = odometervalue + extrakms;
                                        odometervalue = Math.Round(odometervalue, 2);
                                        summeryRow["AM TOTAL KMS"] = odometervalue.ToString();
                                        totkms += odometervalue;
                                        AMtotkms += odometervalue;
                                    }
                                    else
                                    {
                                        summeryRow["AM GPS KMS"] = 0;
                                        summeryRow["AM EMPTY KMS"] = 0;
                                        summeryRow["AM TOTAL KMS"] = 0;
                                    }
                                }
                                else
                                {
                                    summeryRow["AM GPS KMS"] = 0;
                                    summeryRow["AM EMPTY KMS"] = 0;
                                    summeryRow["AM TOTAL KMS"] = 0;
                                }
                                if (pmuniqueTrips.Rows.Count > 0)
                                {  
                                    if (pmuniqueTrips.Rows[0]["StartTime"].ToString() == "")
                                    {
                                        summeryRow["PM GPS KMS"] = 0;
                                        summeryRow["PM EMPTY KMS"] = 0;
                                        summeryRow["PM TOTAL KMS"] = "Trip not completed";
                                    }
                                    tpmstarttime = date.Date.Add(TimeSpan.Parse(pmuniqueTrips.Rows[0]["StartTime"].ToString()));
                                    //tstarttime = (DateTime)uniqueTrips.Rows[0]["StartTime"];
                                    if (pmuniqueTrips.Rows[0]["EndTime"].ToString() == "")
                                    {
                                        summeryRow["PM GPS KMS"] = 0;
                                        summeryRow["PM EMPTY KMS"] = 0;
                                        summeryRow["PM TOTAL KMS"] = "Trip not completed";
                                    }
                                    tpmendtime = date.Date.AddHours(25);
                                    DataTable ResDT = new DataTable();
                                    ResDT = getfirsttouchOdometer(pmuniqueTrips.Rows[0]["VehicleNumber"].ToString(), tpmstarttime, tpmendtime, vdm, routesubtabledata, out odometervalue, mainuser);
                                    if (ResDT.Rows.Count == 1)
                                    {
                                        summeryRow["PM GPS KMS"] = 0;
                                        summeryRow["PM EMPTY KMS"] = 0;
                                        summeryRow["PM TOTAL KMS"] = "Trip not completed";
                                    }
                                    else if (ResDT.Rows.Count == 2)
                                    {
                                        DataTable routes = new DataTable();
                                        if (Session["routes"] != null)
                                        {
                                            routes = (DataTable)Session["routes"];
                                        }
                                        else
                                        {
                                            cmd = new MySqlCommand("SELECT routetable.RouteName, tripconfiguration.StartTime, tripconfiguration.EndTime, routetable.SNo, tripconfiguration.Kms, tripconfiguration.extrakms, tripconfiguration.Chargeperkm FROM tripconfiguration INNER JOIN branchdata ON tripconfiguration.PlantName = branchdata.Sno INNER JOIN cabmanagement ON branchdata.BranchID = cabmanagement.PlantName INNER JOIN loginsconfigtable ON cabmanagement.VehicleID = loginsconfigtable.VehicleID INNER JOIN loginstable ON loginsconfigtable.Refno = loginstable.refno INNER JOIN routetable ON tripconfiguration.RouteID = routetable.SNo WHERE (loginstable.loginid = @userid) GROUP BY routetable.RouteName");
                                            //cmd = new MySqlCommand("SELECT routetable.RouteName, tripconfiguration.StartTime, tripconfiguration.EndTime, routetable.SNo, tripconfiguration.Kms,tripconfiguration.extrakms,tripconfiguration.Chargeperkm FROM loginsconfigtable INNER JOIN loginstable ON loginsconfigtable.Refno = loginstable.refno INNER JOIN cabmanagement ON loginsconfigtable.VehicleID = cabmanagement.VehicleID INNER JOIN vehiclemanage ON cabmanagement.PlantName = vehiclemanage.ItemName INNER JOIN tripconfiguration ON vehiclemanage.SNo = tripconfiguration.PlantName INNER JOIN routetable ON tripconfiguration.RouteID = routetable.SNo WHERE (loginstable.loginid = @userid) GROUP BY routetable.RouteName");
                                            cmd.Parameters.Add("@userid", UserName);
                                            routes = vdm.SelectQuery(cmd).Tables[0];
                                            Session["routes"] = routes;
                                        }
                                        DataRow[] rows = routes.Select("SNo='" + PMresult.Rows[0]["SNo"].ToString() + "'");
                                        double extrakms = 0;
                                        if (rows.Length > 0)
                                        {
                                            double.TryParse(rows[0]["extrakms"].ToString(), out extrakms);
                                        }
                                        summeryRow["PM StartTime"] = ResDT.Rows[0]["Reaching Time"].ToString();
                                        summeryRow["PM EndTime"] = ResDT.Rows[1]["Reaching Time"].ToString();
                                        summeryRow["PM EMPTY KMS"] = extrakms;
                                        PMemptykms += extrakms;
                                        odometervalue = Math.Round(odometervalue, 2);
                                        summeryRow["PM GPS KMS"] = odometervalue;
                                        PMgpskms += odometervalue;
                                        odometervalue = odometervalue + extrakms;
                                        odometervalue = Math.Round(odometervalue, 2);
                                        summeryRow["PM TOTAL KMS"] = odometervalue.ToString();
                                        totkms += odometervalue;
                                        PMtotkms += odometervalue;
                                    }
                                    else
                                    {
                                        summeryRow["PM GPS KMS"] = 0;
                                        summeryRow["PM EMPTY KMS"] = 0;
                                        summeryRow["PM TOTAL KMS"] = 0;
                                    }
                                }
                                else
                                {
                                    summeryRow["PM GPS KMS"] = 0;
                                    summeryRow["PM EMPTY KMS"] = 0;
                                    summeryRow["PM TOTAL KMS"] = 0;
                                }
                                summeryRow["Total KMS"] = totkms;
                                summeryTable.Rows.Add(summeryRow);
                                sno++;
                            }
                            DataRow sumrow = summeryTable.NewRow();
                            sumrow["Route Name"] = "Total KMS";
                            sumrow["AM GPS KMS"] = AMgpskms;
                            sumrow["AM EMPTY KMS"] = AMemptykms;
                            sumrow["AM TOTAL KMS"] = AMtotkms;
                            sumrow["PM GPS KMS"] = PMgpskms;
                            sumrow["PM EMPTY KMS"] = PMemptykms;
                            sumrow["PM TOTAL KMS"] = PMtotkms;
                            AMPMtotkms = AMtotkms + PMtotkms;
                            sumrow["Total KMS"] = AMPMtotkms;
                            summeryTable.Rows.Add(sumrow);
                        }
                        grdReports.DataSource = summeryTable;
                        grdReports.DataBind();
                        Session["xportdata"] = summeryTable;
                        if (grdReports.Rows.Count > 0)
                        {
                            grdReports.Rows[grdReports.Rows.Count - 1].Cells[0].Controls.Remove(grdReports.Rows[grdReports.Rows.Count - 1].Cells[0].Controls[1]);
                        }
                        #endregion
                    }
                }
                else
                {
                    lbl_nofifier.Text = "Please Select  Start Date and End Date";
                }
            }
            else
            {
                lbl_nofifier.Text = "Please Select at least One Vehicle from list";
            }
            #endregion

        }
        catch (Exception ex)
        {
            lbl_nofifier.Text = ex.ToString();
        }
    }

   
    private DateTime GoBackandGetPlantTouchdate(List<string> logstbls, DateTime fromdate, DataTable selectPlants, string vehiclestr)
    {
        DataTable TripData = new DataTable();
        DataTable logs = new DataTable();
        DataTable tottable = new DataTable();
        DateTime founddatetime = fromdate;
        bool plantfound = false;
        int chkbeforedates = -1;

        while (!plantfound)
        {
            foreach (string tbname in logstbls)
            {
                cmd = new MySqlCommand("SELECT '' AS SNo, " + tbname + ".VehicleID, " + tbname + ".DateTime, " + tbname + ".Speed, " + tbname + ".Latitiude, " + tbname + ".Longitude, " + tbname + ".Direction, " + tbname + ".Diesel, " + tbname + ".Odometer, " + tbname + ".Direction AS Expr1, " + tbname + ".Direction AS Expr2, vehiclemaster.MaintenancePlantName, vehiclemaster.VendorName, vehiclemaster.VendorNo, vehiclemaster.VehicleTypeName FROM " + tbname + " LEFT OUTER JOIN vehiclemaster ON " + tbname + ".VehicleID = vehiclemaster.VehicleID WHERE (" + tbname + ".DateTime >= @starttime) AND (" + tbname + ".DateTime <= @endtime) AND (" + tbname + ".VehicleID = '" + vehiclestr + "') and (" + tbname + ".UserID='" + mainuser + "')  ORDER BY " + tbname + ".DateTime DESC");
                //cmd = new MySqlCommand("select * from " + tbname + " where DateTime>= @starttime and DateTime<=@endtime and VehicleID='" + vehiclestr + "' and UserID='" + UserName + "' order by DateTime");
                cmd.Parameters.Add(new MySqlParameter("@starttime", GetLowDate(fromdate.AddDays(chkbeforedates))));
                cmd.Parameters.Add(new MySqlParameter("@endtime", GetHighDate(fromdate.AddDays(chkbeforedates))));
                logs = vdm.SelectQuery(cmd).Tables[0];
                if (tottable.Rows.Count == 0)
                {
                    tottable = logs.Clone();
                }
                foreach (DataRow dr in logs.Rows)
                {
                    tottable.ImportRow(dr);
                }
            }
            DataView dv = tottable.DefaultView;
            dv.Sort = "DateTime DESC";
            TripData = dv.ToTable();
            bool istouchedplant = false;
            foreach (DataRow tripdatarow in TripData.Rows)
            {
                foreach (DataRow Brncs in selectPlants.Rows)
                {
                    ////DataRow[] branch = vehbarnches.Select("BranchID='" + lstLocation + "'");

                    double presLat = (double)tripdatarow["Latitiude"];
                    double PresLng = (double)tripdatarow["Longitude"];

                    //foreach (DataRow Brncs in branch)
                    //{
                    double ag_Lat = 0;
                    double.TryParse(Brncs["Latitude"].ToString(), out ag_Lat);
                    double ag_lng = 0;
                    double.TryParse(Brncs["Longitude"].ToString(), out ag_lng);
                    double ag_radious = 100;
                    double.TryParse(Brncs["Radious"].ToString(), out ag_radious);
                    string statusvalue = ddwnldr.getGeofenceStatus(presLat, PresLng, ag_Lat, ag_lng, ag_radious);


                    if (statusvalue == "In Side")
                    {
                        founddatetime = (DateTime)tripdatarow["DateTime"];
                        istouchedplant = true;
                        break;
                    }
                }

                if (istouchedplant)
                {
                    plantfound = true;
                    break;
                }
            }

            chkbeforedates--;
        }
        return founddatetime;
    }

    public static DataTable getfirsttouchOdometer(string vehiclestr, DateTime fromdate, DateTime todate, VehicleDBMgr vdm, DataTable BranchDetails, out double odometervalue, string mainuser)
    {
        odometervalue = 0;
        MySqlCommand cmd;
        #region location wise Reports
        DateTime Startingdt = DateTime.Now;
        int sno = 1;
        DataTable summeryTable = new DataTable();
        DataColumn summeryColumn = new DataColumn("SNo");
        summeryTable.Columns.Add(summeryColumn);
        summeryColumn = new DataColumn("From Location");
        summeryTable.Columns.Add(summeryColumn);
        summeryTable.Columns.Add("Reaching Date").DataType = typeof(DateTime);
        summeryColumn = new DataColumn("Reaching Time");
        summeryTable.Columns.Add(summeryColumn);
        summeryColumn = new DataColumn("Distance(Kms)");
        summeryTable.Columns.Add(summeryColumn);
        summeryColumn = new DataColumn("Journey Hours");
        summeryTable.Columns.Add(summeryColumn);
        DataRow summeryRow = null;
        DateTime prevdate = DateTime.Now;
        double presodometer = 0;
        List<string> logstbls = new List<string>();
        logstbls.Add("GpsTrackVehicleLogs");
        logstbls.Add("GpsTrackVehicleLogs1");
        logstbls.Add("GpsTrackVehicleLogs2");
        logstbls.Add("GpsTrackVehicleLogs3");
        DataTable TripData = new DataTable();
        DataTable logs = new DataTable();
        DataTable tottable = new DataTable();
        foreach (string tbname in logstbls)
        {
            cmd = new MySqlCommand("SELECT '' AS SNo, " + tbname + ".VehicleID, " + tbname + ".DateTime, " + tbname + ".Speed, " + tbname + ".Latitiude, " + tbname + ".Longitude, " + tbname + ".Direction, " + tbname + ".Diesel, " + tbname + ".Odometer, " + tbname + ".Direction AS Expr1, " + tbname + ".Direction AS Expr2, vehiclemaster.MaintenancePlantName, vehiclemaster.VendorName, vehiclemaster.VendorNo, vehiclemaster.VehicleTypeName FROM " + tbname + " LEFT OUTER JOIN vehiclemaster ON " + tbname + ".VehicleID = vehiclemaster.VehicleID WHERE (" + tbname + ".DateTime >= @starttime) AND (" + tbname + ".DateTime <= @endtime) AND (" + tbname + ".VehicleID = '" + vehiclestr + "') and (" + tbname + ".UserID='" + mainuser + "')  ORDER BY " + tbname + ".DateTime");
            //cmd = new MySqlCommand("select * from " + tbname + " where DateTime>= @starttime and DateTime<=@endtime and VehicleID='" + vehiclestr + "' and UserID='" + UserName + "' order by DateTime");
            cmd.Parameters.Add(new MySqlParameter("@starttime", fromdate));
            cmd.Parameters.Add(new MySqlParameter("@endtime", todate));
            logs = vdm.SelectQuery(cmd).Tables[0];
            if (tottable.Rows.Count == 0)
            {
                tottable = logs.Clone();
            }
            foreach (DataRow dr in logs.Rows)
            {
                tottable.ImportRow(dr);
            }
        }
        DataView dv = tottable.DefaultView;
        dv.Sort = "DateTime ASC";
        TripData = dv.ToTable();
        DataRow Prevrow = null;
        summeryRow = null;

        Dictionary<string, string> statusobserver = new Dictionary<string, string>();
        foreach (DataRow dr in BranchDetails.Rows)
        {
            if (!statusobserver.ContainsKey(dr["BranchID"].ToString()))
            {
                statusobserver.Add(dr["BranchID"].ToString(), "In Side");
            }
        }
        foreach (DataRow tripdatarow in TripData.Rows)
        {
            for (int cnt = 0; cnt < BranchDetails.Rows.Count - 1; cnt++)
            {
                DataRow Brncs = BranchDetails.Rows[cnt];
                double presLat = (double)tripdatarow["Latitiude"];
                double PresLng = (double)tripdatarow["Longitude"];
                double ag_Lat = 0;
                double.TryParse(Brncs["Latitude"].ToString(), out ag_Lat);
                double ag_lng = 0;
                double.TryParse(Brncs["Longitude"].ToString(), out ag_lng);
                double ag_radious = 100;
                double.TryParse(Brncs["Radious"].ToString(), out ag_radious);
                string statusvalue = GeoCodeCalc.getGeofenceStatus(presLat, PresLng, ag_Lat, ag_lng, ag_radious);
                if (statusobserver[Brncs["BranchID"].ToString()] == statusvalue)
                {
                    DateTime Reachingdt = (DateTime)tripdatarow["DateTime"];
                    summeryRow = summeryTable.NewRow();
                    summeryRow["SNo"] = sno;
                    summeryRow["From Location"] = Brncs["BranchID"];
                    string Reachdate = Reachingdt.ToString();
                    string ReachTime = Reachingdt.ToString("HH:mm");
                    summeryRow["Reaching Date"] = Reachingdt.ToString();
                    summeryRow["Reaching Time"] = ReachTime.ToString();
                    presodometer = double.Parse(tripdatarow["Odometer"].ToString());
                    summeryRow["Distance(Kms)"] = presodometer;
                    summeryTable.Rows.Add(summeryRow);
                    break;
                }
                Prevrow = tripdatarow;
            }
        }
        foreach (DataRow tripdatarow in TripData.Rows)
        {
            double presLat = (double)tripdatarow["Latitiude"];
            double PresLng = (double)tripdatarow["Longitude"];
            double ag_Lat = 0;
            double.TryParse(BranchDetails.Rows[BranchDetails.Rows.Count - 1]["Latitude"].ToString(), out ag_Lat);
            double ag_lng = 0;
            double.TryParse(BranchDetails.Rows[BranchDetails.Rows.Count - 1]["Longitude"].ToString(), out ag_lng);
            double ag_radious = 100;
            double.TryParse(BranchDetails.Rows[BranchDetails.Rows.Count - 1]["Radious"].ToString(), out ag_radious);
            string statusvalue = GeoCodeCalc.getGeofenceStatus(presLat, PresLng, ag_Lat, ag_lng, ag_radious);
            if (statusobserver[BranchDetails.Rows[BranchDetails.Rows.Count - 1]["BranchID"].ToString()] == statusvalue)
            {
                if (summeryTable.Rows.Count > 0)
                {
                    DateTime Reachingdt = (DateTime)tripdatarow["DateTime"];
                    prevdate = (DateTime)summeryTable.Rows[0]["Reaching Date"];
                    if (Reachingdt < prevdate.AddHours(1).AddMinutes(30))
                    {
                        continue;
                    }
                    summeryRow = summeryTable.NewRow();
                    summeryRow["SNo"] = sno;
                    summeryRow["From Location"] = BranchDetails.Rows[BranchDetails.Rows.Count - 1]["BranchID"];
                    string Reachdate = Reachingdt.ToString("M/dd/yyyy");
                    string ReachTime = Reachingdt.ToString("HH:mm");
                    summeryRow["Reaching Date"] = Reachingdt.ToString();
                    summeryRow["Reaching Time"] = ReachTime.ToString();
                    presodometer = double.Parse(tripdatarow["Odometer"].ToString());
                    summeryRow["Distance(Kms)"] = presodometer;
                    summeryTable.Rows.Add(summeryRow);
                    break;
                }
            }
            Prevrow = tripdatarow;
        }
        DataTable finaltable = new DataTable();
        finaltable = summeryTable.Clone();
        if (summeryTable.Rows.Count > 0)
        {
            DataRow newrw = finaltable.NewRow();
            newrw["SNo"] = "1";
            newrw["From Location"] = summeryTable.Rows[0]["From Location"].ToString();
            newrw["Reaching Date"] = summeryTable.Rows[0]["Reaching Date"].ToString();
            newrw["Reaching Time"] = summeryTable.Rows[0]["Reaching Time"].ToString();
            newrw["Distance(Kms)"] = summeryTable.Rows[0]["Distance(Kms)"].ToString();
            newrw["Journey Hours"] = summeryTable.Rows[0]["Journey Hours"].ToString();
            finaltable.Rows.Add(newrw);
            DataRow[] finalrow = summeryTable.Select("[From Location]='" + BranchDetails.Rows[BranchDetails.Rows.Count - 1]["BranchID"].ToString() + "'");
            if (finalrow.Length > 0)
            {
                DataRow row = finaltable.NewRow();
                row["SNo"] = "1";
                row["From Location"] = finalrow[finalrow.Length - 1]["From Location"].ToString();
                row["Reaching Date"] = finalrow[finalrow.Length - 1]["Reaching Date"].ToString();
                row["Reaching Time"] = finalrow[finalrow.Length - 1]["Reaching Time"].ToString();
                row["Distance(Kms)"] = finalrow[finalrow.Length - 1]["Distance(Kms)"].ToString();
                row["Journey Hours"] = finalrow[finalrow.Length - 1]["Journey Hours"].ToString();
                finaltable.Rows.Add(row);
            }
            if (finaltable.Rows.Count == 2)
            {
                odometervalue = double.Parse(finaltable.Rows[1]["Distance(Kms)"].ToString()) - double.Parse(finaltable.Rows[0]["Distance(Kms)"].ToString());
            }
        }
        #endregion
        return finaltable;
    }
    
    string dateconverter(string time)
    {
        string[] Reachsplt = time.ToString().Split(' ');
        string StDuration = "";
        if (Reachsplt.Length > 1)
        {
            int departuretimemin = 0;
            int dephours = 0;
            int depmin = 0;
            int.TryParse(Reachsplt[0].Split(':')[0], out dephours);
            int.TryParse(Reachsplt[0].Split(':')[1], out depmin);
            //departuretimemin = 720 - ((dephours * 60) + depmin);

            if (Reachsplt[1] == "PM")
            {
                if (Reachsplt[0].Split(':')[0] == "12")
                    departuretimemin = ((dephours * 60) + depmin);
                else
                    departuretimemin = 720 + ((dephours * 60) + depmin);
            }
            else
            {
                if (Reachsplt[0].Split(':')[0] == "12")
                    departuretimemin = ((dephours * 60) + depmin) - 720;
                else
                    departuretimemin = ((dephours * 60) + depmin);
            }

            //ddlTravels.Items.Add(dr["traveler_agent"].ToString());

            int duration = departuretimemin;
            if ((duration % 60) < 10 && (duration / 60) < 10)
            {
                StDuration = "0" + duration / 60 + ":" + "0" + duration % 60;
            }
            else if ((duration % 60) >= 10 && (duration / 60) < 10)
            {
                StDuration = "0" + duration / 60 + ":" + duration % 60;
            }
            else if ((duration % 60) < 10 && (duration / 60) >= 10)
            {
                StDuration = duration / 60 + ":" + "0" + duration % 60;
            }
            else if ((duration % 60) >= 10 && (duration / 60) >= 10)
            {
                StDuration = duration / 60 + ":" + duration % 60;
            }
        }
        return StDuration;
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        return;
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

    GooglePoint GP;
    ListBox lstbx = new ListBox();
    protected void grdReports_SelectedIndexChanged(object sender, EventArgs e)
    {
      
    }
   
    static DataTable fleetVehiceData;
    public void FillSelectVehicle()
    {
        try
        {

            cmd = new MySqlCommand("select * from ManageData where UserName=@UserName");
            cmd.Parameters.Add("@UserName", UserName);
            fleetVehiceData = vdm.SelectQuery(cmd).Tables[0];

        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, this);
        }
    }
   
   
    GooglePoint GP1;
    bool ShowMyLocations = false;
    protected void btn_Focus_Click(object sender, EventArgs e)
    {
        //GoogleMapForASPNet1.GoogleMapObject.ZoomLevel = 14;
        zoomlevel = 14;
        MessageBox.Show("Geofence", this);
    }

    static DataTable selecteddata = null;
    static int pointcount = 0;

    bool showw = true;
    protected void OnclickDrawRoute(object sender, CommandEventArgs e)
    {
        try
        {
            Session["Data"] = null;
            List<string> logstbls = new List<string>();
            logstbls.Add("GpsTrackVehicleLogs");
            logstbls.Add("GpsTrackVehicleLogs1");
            logstbls.Add("GpsTrackVehicleLogs2");
            logstbls.Add("GpsTrackVehicleLogs3");
            if (Session["main_user"] == null)
            {
                cmd = new MySqlCommand("SELECT main_user FROM loginstable WHERE (loginid = @loginid)");
                cmd.Parameters.Add("@loginid", UserName);
                DataTable mainusertbl = vdm.SelectQuery(cmd).Tables[0];
                if (mainusertbl.Rows.Count > 0)
                {
                    mainuser = mainusertbl.Rows[0]["main_user"].ToString();
                    Session["main_user"] = mainuser;
                }
                else
                {
                    mainuser = UserName;
                }
            }
            else
            {
                mainuser = Session["main_user"].ToString();
            }
            GridViewRow row = grdReports.Rows[int.Parse(e.CommandArgument.ToString())];
            reportData = new Dictionary<string, DataTable>();
            reportData = (Dictionary<string, DataTable>)Session["reportdata"];
            reportname = Request.QueryString["Report"];
            if (reportname == "Daily Report")
            {
                if (Session["reportdata"] == null)
                {
                }
                else
                {
                    if (row != null)
                    {
                        selecteddata = reportData[row.RowIndex.ToString()];
                        Session["Data"] = selecteddata;
                    }
                }
                //for (int i = 0; i < grdReports.Rows.Count - 1; i++)
                //{
                //    if (grdReports.Rows[i].Cells[1].Text == "" || grdReports.Rows[i].Cells[1].Text == "&nbsp;")
                //    {
                //        grdReports.Rows[i].BackColor = System.Drawing.Color.Khaki;
                //        grdReports.Rows[i].Cells[0].Controls.Remove(grdReports.Rows[i].Cells[0].Controls[1]);
                //    }
                //}
            }
            else if (reportname == "LocationWiseReport")
            {
                //selecteddata = reportData[row.RowIndex.ToString()];
                //DateTime fromdate = DateTime.Parse(row.Cells[4].Text);
                //DateTime todate = DateTime.Parse(row.Cells[5].Text);
                string fromtime = row.Cells[4].Text + " " + row.Cells[5].Text + ":00";
                string totime = row.Cells[6].Text + " " + row.Cells[7].Text + ":00";
                DateTime fromdate = DateTime.ParseExact(fromtime, "M/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                DateTime todate = DateTime.ParseExact(totime, "M/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                DataTable logs = new DataTable();
                DataTable tottable = new DataTable();
                foreach (string tbname in logstbls)
                {
                    cmd = new MySqlCommand("SELECT  " + tbname + ".UserID, " + tbname + ".VehicleID, " + tbname + ".Speed, " + tbname + ".DateTime, " + tbname + ".Distance, " + tbname + ".Diesel, " + tbname + ".TripFlag, " + tbname + ".Latitiude, " + tbname + ".Longitude, " + tbname + ".TimeInterval, " + tbname + ".Status, " + tbname + ".Direction, " + tbname + ".Remarks, " + tbname + ".Odometer, " + tbname + ".inp1, " + tbname + ".inp2, " + tbname + ".inp3, " + tbname + ".inp4, " + tbname + ".inp5, " + tbname + ".inp6, " + tbname + ".inp7, " + tbname + ".inp8, " + tbname + ".out1, " + tbname + ".out2, " + tbname + ".out3, " + tbname + ".out4, " + tbname + ".out5, " + tbname + ".out6, " + tbname + ".out7, " + tbname + ".out8, " + tbname + ".ADC1, " + tbname + ".ADC2, " + tbname + ".GSMSignal, " + tbname + ".GPSSignal, " + tbname + ".SatilitesAvail, " + tbname + ".EP, " + tbname + ".BP, " + tbname + ".Altitude, " + tbname + ".sno, loginstable.loginid FROM " + tbname + " INNER JOIN loginstable ON " + tbname + ".UserID = loginstable.main_user WHERE (" + tbname + ".DateTime >= @starttime) AND (" + tbname + ".DateTime <= @endtime) AND (" + tbname + ".VehicleID = '" + row.Cells[2].Text + "') and (" + tbname + ".UserID='" + mainuser + "') AND (loginstable.loginid = '" + UserName + "') ORDER BY " + tbname + ".DateTime");
                    cmd.Parameters.Add(new MySqlParameter("@starttime", fromdate));
                    cmd.Parameters.Add(new MySqlParameter("@endtime", todate));
                    logs = vdm.SelectQuery(cmd).Tables[0];
                    if (tottable.Rows.Count == 0)
                    {
                        tottable = logs.Clone();
                    }
                    foreach (DataRow dr in logs.Rows)
                    {
                        tottable.ImportRow(dr);
                    }
                }
                DataView dv = tottable.DefaultView;
                dv.Sort = "DateTime ASC";
                selecteddata = dv.ToTable();
                Session["Data"] = selecteddata;
                //Session["fromdate"] = fromdate;
                //Session["todate"] = todate;
                //Session["VehicleID"] = row.Cells[2].Text;
            }
            else if (reportname == "Location To Location Report")
            {
                string fromtime = row.Cells[4].Text + " " + row.Cells[5].Text + ":00";
                string totime = row.Cells[7].Text + " " + row.Cells[8].Text + ":00";
                DateTime fromdate = DateTime.ParseExact(fromtime, "M/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                DateTime todate = DateTime.ParseExact(totime, "M/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                DataTable logs = new DataTable();
                DataTable tottable = new DataTable();
                foreach (string tbname in logstbls)
                {
                    cmd = new MySqlCommand("SELECT  " + tbname + ".UserID, " + tbname + ".VehicleID, " + tbname + ".Speed, " + tbname + ".DateTime, " + tbname + ".Distance, " + tbname + ".Diesel, " + tbname + ".TripFlag, " + tbname + ".Latitiude, " + tbname + ".Longitude, " + tbname + ".TimeInterval, " + tbname + ".Status, " + tbname + ".Direction, " + tbname + ".Remarks, " + tbname + ".Odometer, " + tbname + ".inp1, " + tbname + ".inp2, " + tbname + ".inp3, " + tbname + ".inp4, " + tbname + ".inp5, " + tbname + ".inp6, " + tbname + ".inp7, " + tbname + ".inp8, " + tbname + ".out1, " + tbname + ".out2, " + tbname + ".out3, " + tbname + ".out4, " + tbname + ".out5, " + tbname + ".out6, " + tbname + ".out7, " + tbname + ".out8, " + tbname + ".ADC1, " + tbname + ".ADC2, " + tbname + ".GSMSignal, " + tbname + ".GPSSignal, " + tbname + ".SatilitesAvail, " + tbname + ".EP, " + tbname + ".BP, " + tbname + ".Altitude, " + tbname + ".sno, loginstable.loginid FROM " + tbname + " INNER JOIN loginstable ON " + tbname + ".UserID = loginstable.main_user WHERE (" + tbname + ".DateTime >= @starttime) AND (" + tbname + ".DateTime <= @endtime) AND (" + tbname + ".VehicleID = '" + row.Cells[2].Text + "') and (" + tbname + ".UserID='" + mainuser + "') AND (loginstable.loginid = '" + UserName + "') ORDER BY " + tbname + ".DateTime");
                    cmd.Parameters.Add(new MySqlParameter("@starttime", fromdate));
                    cmd.Parameters.Add(new MySqlParameter("@endtime", todate));
                    logs = vdm.SelectQuery(cmd).Tables[0];
                    if (tottable.Rows.Count == 0)
                    {
                        tottable = logs.Clone();
                    }
                    foreach (DataRow dr in logs.Rows)
                    {
                        tottable.ImportRow(dr);
                    }
                }
                DataView dv = tottable.DefaultView;
                dv.Sort = "DateTime ASC";
                selecteddata = dv.ToTable();
                Session["Data"] = selecteddata;
                //for (int i = 0; i < grdReports.Rows.Count; i++)
                //{
                //    grdReports.Rows[i].Cells[0].Controls.Remove(grdReports.Rows[i].Cells[0].Controls[1]);
                //    string amtext = grdReports.Rows[i].Cells[5].Text;
                //    string pmtext = grdReports.Rows[i].Cells[10].Text;
                //    LinkButton amlb = new LinkButton();
                //    amlb.Text = amtext;
                //    amlb.CommandName = amtext;
                //    amlb.OnClientClick = "NormalOpening();";
                //    grdReports.Rows[i].Cells[5].Controls.Add(amlb);
                //    LinkButton pmlb = new LinkButton();
                //    pmlb.Text = amtext;
                //    pmlb.CommandName = amtext;
                //    pmlb.OnClientClick = "NormalOpening();";
                //    grdReports.Rows[i].Cells[10].Controls.Add(pmlb);
                //}
            }
            else if (reportname == "Sales Trip Report")
            {
                string fromtime = row.Cells[4].Text + ":00";
                string totime = row.Cells[6].Text + ":00";
                DateTime fromdate = DateTime.ParseExact(fromtime, "M/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                DateTime todate = DateTime.ParseExact(totime, "M/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                DataTable logs = new DataTable();
                DataTable tottable = new DataTable();
                foreach (string tbname in logstbls)
                {
                    cmd = new MySqlCommand("SELECT  " + tbname + ".UserID, " + tbname + ".VehicleID, " + tbname + ".Speed, " + tbname + ".DateTime, " + tbname + ".Distance, " + tbname + ".Diesel, " + tbname + ".TripFlag, " + tbname + ".Latitiude, " + tbname + ".Longitude, " + tbname + ".TimeInterval, " + tbname + ".Status, " + tbname + ".Direction, " + tbname + ".Remarks, " + tbname + ".Odometer, " + tbname + ".inp1, " + tbname + ".inp2, " + tbname + ".inp3, " + tbname + ".inp4, " + tbname + ".inp5, " + tbname + ".inp6, " + tbname + ".inp7, " + tbname + ".inp8, " + tbname + ".out1, " + tbname + ".out2, " + tbname + ".out3, " + tbname + ".out4, " + tbname + ".out5, " + tbname + ".out6, " + tbname + ".out7, " + tbname + ".out8, " + tbname + ".ADC1, " + tbname + ".ADC2, " + tbname + ".GSMSignal, " + tbname + ".GPSSignal, " + tbname + ".SatilitesAvail, " + tbname + ".EP, " + tbname + ".BP, " + tbname + ".Altitude, " + tbname + ".sno, loginstable.loginid FROM " + tbname + " INNER JOIN loginstable ON " + tbname + ".UserID = loginstable.main_user WHERE (" + tbname + ".DateTime >= @starttime) AND (" + tbname + ".DateTime <= @endtime) AND (" + tbname + ".VehicleID = '" + row.Cells[2].Text + "') and (" + tbname + ".UserID='" + mainuser + "') AND (loginstable.loginid = '" + UserName + "') ORDER BY " + tbname + ".DateTime");
                    cmd.Parameters.Add(new MySqlParameter("@starttime", fromdate));
                    cmd.Parameters.Add(new MySqlParameter("@endtime", todate));
                    logs = vdm.SelectQuery(cmd).Tables[0];
                    if (tottable.Rows.Count == 0)
                    {
                        tottable = logs.Clone();
                    }
                    foreach (DataRow dr in logs.Rows)
                    {
                        tottable.ImportRow(dr);
                    }
                }
                DataView dv = tottable.DefaultView;
                dv.Sort = "DateTime ASC";
                selecteddata = dv.ToTable();
                Session["Data"] = selecteddata;
                //for (int i = 0; i < grdReports.Rows.Count; i++)
                //{
                //    grdReports.Rows[i].Cells[0].Controls.Remove(grdReports.Rows[i].Cells[0].Controls[1]);
                //    string amtext = grdReports.Rows[i].Cells[5].Text;
                //    string pmtext = grdReports.Rows[i].Cells[10].Text;
                //    LinkButton amlb = new LinkButton();
                //    amlb.Text = amtext;
                //    amlb.CommandName = amtext;
                //    amlb.OnClientClick = "NormalOpening();";
                //    grdReports.Rows[i].Cells[5].Controls.Add(amlb);
                //    LinkButton pmlb = new LinkButton();
                //    pmlb.Text = amtext;
                //    pmlb.CommandName = amtext;
                //    pmlb.OnClientClick = "NormalOpening();";
                //    grdReports.Rows[i].Cells[10].Controls.Add(pmlb);
                //}
            }
            else if (reportname == "Tanker and Puffs Trip Report")
            {
                string fromtime = row.Cells[4].Text + " " + row.Cells[5].Text + ":00";
                string totime = row.Cells[7].Text + " " + row.Cells[8].Text + ":00";
                DateTime fromdate = DateTime.ParseExact(fromtime, "M/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                DateTime todate = DateTime.ParseExact(totime, "M/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                DataTable logs = new DataTable();
                DataTable tottable = new DataTable();
                foreach (string tbname in logstbls)
                {
                    cmd = new MySqlCommand("SELECT  " + tbname + ".UserID, " + tbname + ".VehicleID, " + tbname + ".Speed, " + tbname + ".DateTime, " + tbname + ".Distance, " + tbname + ".Diesel, " + tbname + ".TripFlag, " + tbname + ".Latitiude, " + tbname + ".Longitude, " + tbname + ".TimeInterval, " + tbname + ".Status, " + tbname + ".Direction, " + tbname + ".Remarks, " + tbname + ".Odometer, " + tbname + ".inp1, " + tbname + ".inp2, " + tbname + ".inp3, " + tbname + ".inp4, " + tbname + ".inp5, " + tbname + ".inp6, " + tbname + ".inp7, " + tbname + ".inp8, " + tbname + ".out1, " + tbname + ".out2, " + tbname + ".out3, " + tbname + ".out4, " + tbname + ".out5, " + tbname + ".out6, " + tbname + ".out7, " + tbname + ".out8, " + tbname + ".ADC1, " + tbname + ".ADC2, " + tbname + ".GSMSignal, " + tbname + ".GPSSignal, " + tbname + ".SatilitesAvail, " + tbname + ".EP, " + tbname + ".BP, " + tbname + ".Altitude, " + tbname + ".sno, loginstable.loginid FROM " + tbname + " INNER JOIN loginstable ON " + tbname + ".UserID = loginstable.main_user WHERE (" + tbname + ".DateTime >= @starttime) AND (" + tbname + ".DateTime <= @endtime) AND (" + tbname + ".VehicleID = '" + row.Cells[2].Text + "') and (" + tbname + ".UserID='" + mainuser + "') AND (loginstable.loginid = '" + UserName + "') ORDER BY " + tbname + ".DateTime");
                    cmd.Parameters.Add(new MySqlParameter("@starttime", fromdate));
                    cmd.Parameters.Add(new MySqlParameter("@endtime", todate));
                    logs = vdm.SelectQuery(cmd).Tables[0];
                    if (tottable.Rows.Count == 0)
                    {
                        tottable = logs.Clone();
                    }
                    foreach (DataRow dr in logs.Rows)
                    {
                        tottable.ImportRow(dr);
                    }
                }
                DataView dv = tottable.DefaultView;
                dv.Sort = "DateTime ASC";
                selecteddata = dv.ToTable();
                Session["Data"] = selecteddata;

            }

            else if (reportname == "Sales Trip Report")
            {
                string fromtime = row.Cells[4].Text + " " + row.Cells[5].Text + ":00";
                string totime = row.Cells[7].Text + " " + row.Cells[8].Text + ":00";
                DateTime fromdate = DateTime.ParseExact(fromtime, "M/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                DateTime todate = DateTime.ParseExact(totime, "M/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                DataTable logs = new DataTable();
                DataTable tottable = new DataTable();
                foreach (string tbname in logstbls)
                {
                    cmd = new MySqlCommand("SELECT  " + tbname + ".UserID, " + tbname + ".VehicleID, " + tbname + ".Speed, " + tbname + ".DateTime, " + tbname + ".Distance, " + tbname + ".Diesel, " + tbname + ".TripFlag, " + tbname + ".Latitiude, " + tbname + ".Longitude, " + tbname + ".TimeInterval, " + tbname + ".Status, " + tbname + ".Direction, " + tbname + ".Remarks, " + tbname + ".Odometer, " + tbname + ".inp1, " + tbname + ".inp2, " + tbname + ".inp3, " + tbname + ".inp4, " + tbname + ".inp5, " + tbname + ".inp6, " + tbname + ".inp7, " + tbname + ".inp8, " + tbname + ".out1, " + tbname + ".out2, " + tbname + ".out3, " + tbname + ".out4, " + tbname + ".out5, " + tbname + ".out6, " + tbname + ".out7, " + tbname + ".out8, " + tbname + ".ADC1, " + tbname + ".ADC2, " + tbname + ".GSMSignal, " + tbname + ".GPSSignal, " + tbname + ".SatilitesAvail, " + tbname + ".EP, " + tbname + ".BP, " + tbname + ".Altitude, " + tbname + ".sno, loginstable.loginid FROM " + tbname + " INNER JOIN loginstable ON " + tbname + ".UserID = loginstable.main_user WHERE (" + tbname + ".DateTime >= @starttime) AND (" + tbname + ".DateTime <= @endtime) AND (" + tbname + ".VehicleID = '" + row.Cells[2].Text + "') and (" + tbname + ".UserID='" + mainuser + "') AND (loginstable.loginid = '" + UserName + "') ORDER BY " + tbname + ".DateTime");
                    cmd.Parameters.Add(new MySqlParameter("@starttime", fromdate));
                    cmd.Parameters.Add(new MySqlParameter("@endtime", todate));
                    logs = vdm.SelectQuery(cmd).Tables[0];
                    if (tottable.Rows.Count == 0)
                    {
                        tottable = logs.Clone();
                    }
                    foreach (DataRow dr in logs.Rows)
                    {
                        tottable.ImportRow(dr);
                    }
                }
                DataView dv = tottable.DefaultView;
                dv.Sort = "DateTime ASC";
                selecteddata = dv.ToTable();
                Session["Data"] = selecteddata;

            }
            else if (reportname == "Trips Report")
            {
                string fromtime = row.Cells[2].Text + " " + row.Cells[5].Text + ":00";
                string totime = row.Cells[2].Text + " " + row.Cells[6].Text + ":00";
                DateTime fromdate = DateTime.ParseExact(fromtime, "M/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                DateTime todate = DateTime.ParseExact(totime, "M/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                DataTable logs = new DataTable();
                DataTable tottable = new DataTable();
                foreach (string tbname in logstbls)
                {
                    cmd = new MySqlCommand("SELECT  " + tbname + ".UserID, " + tbname + ".VehicleID, " + tbname + ".Speed, " + tbname + ".DateTime, " + tbname + ".Distance, " + tbname + ".Diesel, " + tbname + ".TripFlag, " + tbname + ".Latitiude, " + tbname + ".Longitude, " + tbname + ".TimeInterval, " + tbname + ".Status, " + tbname + ".Direction, " + tbname + ".Remarks, " + tbname + ".Odometer, " + tbname + ".inp1, " + tbname + ".inp2, " + tbname + ".inp3, " + tbname + ".inp4, " + tbname + ".inp5, " + tbname + ".inp6, " + tbname + ".inp7, " + tbname + ".inp8, " + tbname + ".out1, " + tbname + ".out2, " + tbname + ".out3, " + tbname + ".out4, " + tbname + ".out5, " + tbname + ".out6, " + tbname + ".out7, " + tbname + ".out8, " + tbname + ".ADC1, " + tbname + ".ADC2, " + tbname + ".GSMSignal, " + tbname + ".GPSSignal, " + tbname + ".SatilitesAvail, " + tbname + ".EP, " + tbname + ".BP, " + tbname + ".Altitude, " + tbname + ".sno, loginstable.loginid FROM " + tbname + " INNER JOIN loginstable ON " + tbname + ".UserID = loginstable.main_user WHERE (" + tbname + ".DateTime >= @starttime) AND (" + tbname + ".DateTime <= @endtime) AND (" + tbname + ".VehicleID = '" + row.Cells[3].Text + "') and (" + tbname + ".UserID='" + mainuser + "') AND (loginstable.loginid = '" + UserName + "') ORDER BY " + tbname + ".DateTime");
                    cmd.Parameters.Add(new MySqlParameter("@starttime", fromdate));
                    cmd.Parameters.Add(new MySqlParameter("@endtime", todate));
                    logs = vdm.SelectQuery(cmd).Tables[0];
                    if (tottable.Rows.Count == 0)
                    {
                        tottable = logs.Clone();
                    }
                    foreach (DataRow dr in logs.Rows)
                    {
                        tottable.ImportRow(dr);
                    }
                }
                DataView dv = tottable.DefaultView;
                dv.Sort = "DateTime ASC";
                selecteddata = dv.ToTable();
                Session["Data"] = selecteddata;
                if (grdReports.Rows.Count > 0)
                {
                    grdReports.Rows[grdReports.Rows.Count - 1].Cells[0].Controls.Remove(grdReports.Rows[grdReports.Rows.Count - 1].Cells[0].Controls[1]);
                }
            }
            else
            {
                selecteddata = reportData[row.Cells[1].Text];
                Session["Data"] = selecteddata;
            }
        }
        catch
        {
        }
    }
    protected void OnclickViewDetails(object sender, CommandEventArgs e)
    {
        if (e.CommandArgument.ToString() != "")
        {
            GridViewRow row = grdReports.Rows[int.Parse(e.CommandArgument.ToString())];

            reportname = Request.QueryString["Report"];
            if (reportname == "Sales Trip Report")
            {
                if (Session["salestripreportdata"] == null)
                {
                }
                else
                {
                    DataTable salestripdata = (DataTable)Session["salestripreportdata"];
                    DataTable bindtable = salestripdata.Clone();
                    if (row != null)
                    {
                        //selecteddata = reportData[row.RowIndex.ToString()];
                        //Session["Data"] = selecteddata;
                        DataRow[] totaldata = salestripdata.Select("SNo='" + row.Cells[1].Text+"'");
                        if (totaldata.Length > 0)
                        {
                            for (int totcount = 0; totcount < totaldata.Length; totcount++)
                            {
                                DataRow newrow = bindtable.NewRow();
                                newrow.ItemArray = totaldata[totcount].ItemArray;
                                // DataRow bindrow = totaldata[totcount];
                                newrow["SNo"] = (totcount + 1);
                                bindtable.Rows.Add(newrow);
                            }
                        }
                        GridView1.DataSource = bindtable;
                        GridView1.DataBind();
                    }
                }

                //for (int i = 0; i < grdReports.Rows.Count - 1; i++)
                //{
                //    if (grdReports.Rows[i].Cells[1].Text == "" || grdReports.Rows[i].Cells[1].Text == "&nbsp;")
                //    {
                //        grdReports.Rows[i].BackColor = System.Drawing.Color.Khaki;
                //        grdReports.Rows[i].Cells[0].Controls.Remove(grdReports.Rows[i].Cells[0].Controls[1]);
                //    }
                //}
            }
        }
    }
   
    protected void btn_exporttoxl_Click(object sender, EventArgs e)
    {
        Response.ClearContent();
        Response.Buffer = true;
      
    }

    public void ExportToExcel(DataTable dt)
    {
        try
        {
            if (dt.Rows.Count > 0)
            {
                string filename = "Report.xls";
                System.IO.StringWriter tw = new System.IO.StringWriter();
                System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
                DataGrid dgGrid = new DataGrid();
                dgGrid.DataSource = dt;
                dgGrid.DataBind();

                //Get the HTML for the control.
                dgGrid.RenderControl(hw);
                //Write the HTML back to the browser.
                //Response.ContentType = application/vnd.ms-excel;
                Response.ContentType = "application/vnd.ms-excel";
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename + "");
                this.EnableViewState = false;
                Response.Write(tw.ToString());
                Response.End();
            }
        }
        catch (Exception ex)
        {
        }
    }
}