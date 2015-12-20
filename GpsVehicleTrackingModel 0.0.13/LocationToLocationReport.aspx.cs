using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;
using GPSApplication;
using System.Globalization;

public partial class LocationToLocationReport : System.Web.UI.Page
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
                    startdate.Text = GetLowDate(DateTime.Now).ToString("dd-MM-yyyy HH:mm");
                    enddate.Text = GetHighDate(DateTime.Now).ToString("dd-MM-yyyy HH:mm");
                    reportname = Request.QueryString["Report"];
                    lblreportname.Text = reportname;

                    switch (reportname)
                    {
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
                    if (reportname == "Location To Location Report")
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
                                                statusobserver.Add(dr["BranchID"].ToString(), "");
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
                                #endregion
                                    }
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
                                            statusobserver.Add(dr["BranchID"].ToString(), "");
                                            List<obj> objlist = new List<obj>();
                                            foreach (DataRow dr1 in vehbarnches.Rows)
                                            {
                                                obj objct = new obj();
                                                objct.latitude = double.Parse(dr1["Latitude"].ToString());
                                                objct.longitude = double.Parse(dr1["Longitude"].ToString());
                                                objct.lid = int.Parse(dr1["Sno"].ToString());
                                                objct.BranchID = dr1["BranchID"].ToString();
                                                objct.radius = int.Parse(dr1["Radious"].ToString());
                                                double lon1 = Double.Parse(dr["Longitude"].ToString());
                                                double lat1 = Double.Parse(dr["Latitude"].ToString());
                                                double lon22 = Double.Parse(dr1["Longitude"].ToString());
                                                double lat22 = Double.Parse(dr1["Latitude"].ToString());
                                                objct.distance = DistanceAlgorithm.DistanceBetweenPlaces(lon1, lat1, lon22, lat22) * 1000;
                                                objlist.Add(objct);
                                            }
                                            dictionary.Add(int.Parse(dr["Sno"].ToString()), objlist);
                                        }
                                        //foreach (DataRow tripdatarow in TripData.Rows)
                                        //{
                                        //    foreach (DataRow Brncs in vehbarnches.Rows)
                                        //    {
                                        //        ////DataRow[] branch = vehbarnches.Select("BranchID='" + lstLocation + "'");

                                        //        double presLat = (double)tripdatarow["Latitiude"];
                                        //        double PresLng = (double)tripdatarow["Longitude"];

                                        //        //foreach (DataRow Brncs in branch)
                                        //        //{
                                        //        double ag_Lat = 0;
                                        //        double.TryParse(Brncs["Latitude"].ToString(), out ag_Lat);
                                        //        double ag_lng = 0;
                                        //        double.TryParse(Brncs["Longitude"].ToString(), out ag_lng);
                                        //        double ag_radious = 100;
                                        //        double.TryParse(Brncs["Radious"].ToString(), out ag_radious);
                                        //        string statusvalue = ddwnldr.getGeofenceStatus(presLat, PresLng, ag_Lat, ag_lng, ag_radious);

                                        //        if (statusobserver[Brncs["BranchID"].ToString()] != statusvalue)
                                        //        {
                                        //            statusobserver[Brncs["BranchID"].ToString()] = statusvalue;
                                        //            if (statusobserver[Brncs["BranchID"].ToString()] == "In Side")
                                        //            {
                                        //                if (!isfirlstlog)
                                        //                {

                                        //                    summeryRow["To Location"] = Brncs["BranchID"];
                                        //                    DateTime Reachingdt = (DateTime)tripdatarow["DateTime"];
                                        //                    string Reachdate = Reachingdt.ToString("M/dd/yyyy");
                                        //                    string ReachTime = Reachingdt.ToString("hh:mm:ss tt");
                                        //                    summeryRow["Reaching Date"] = Reachdate;


                                        //                    string[] Reachsplt = ReachTime.ToString().Split(' ');
                                        //                    if (Reachsplt.Length > 1)
                                        //                    {
                                        //                        int departuretimemin = 0;
                                        //                        int dephours = 0;
                                        //                        int depmin = 0;
                                        //                        int.TryParse(Reachsplt[0].Split(':')[0], out dephours);
                                        //                        int.TryParse(Reachsplt[0].Split(':')[1], out depmin);
                                        //                        //departuretimemin = 720 - ((dephours * 60) + depmin);

                                        //                        if (Reachsplt[1] == "PM")
                                        //                        {
                                        //                            if (Reachsplt[0].Split(':')[0] == "12")
                                        //                                departuretimemin = ((dephours * 60) + depmin);
                                        //                            else
                                        //                                departuretimemin = 720 + ((dephours * 60) + depmin);
                                        //                        }
                                        //                        else
                                        //                        {
                                        //                            if (Reachsplt[0].Split(':')[0] == "12")
                                        //                                departuretimemin = ((dephours * 60) + depmin) - 720;
                                        //                            else
                                        //                                departuretimemin = ((dephours * 60) + depmin);
                                        //                        }

                                        //                        //ddlTravels.Items.Add(dr["traveler_agent"].ToString());

                                        //                        int time = departuretimemin;
                                        //                        int aaa = time % 60;
                                        //                        int sss = time / 60;
                                        //                        if ((time % 60) < 10 && (time / 60) < 10)
                                        //                        {
                                        //                            Duration = "0" + time / 60 + ":" + "0" + time % 60;
                                        //                        }
                                        //                        else if ((time % 60) >= 10 && (time / 60) < 10)
                                        //                        {
                                        //                            Duration = "0" + time / 60 + ":" + time % 60;
                                        //                        }
                                        //                        else if ((time % 60) < 10 && (time / 60) >= 10)
                                        //                        {
                                        //                            Duration = time / 60 + ":" + "0" + time % 60;
                                        //                        }
                                        //                        else if ((time % 60) >= 10 && (time / 60) >= 10)
                                        //                        {
                                        //                            Duration = time / 60 + ":" + time % 60;
                                        //                        }

                                        //                    }
                                        //                    summeryRow["Reaching Time"] = Duration;
                                        //                    presodometer = double.Parse(tripdatarow["Odometer"].ToString());
                                        //                    if (presodometer < prevodometer)
                                        //                    {
                                        //                        summeryTable.Rows.Remove(summeryRow);
                                        //                        sno--;
                                        //                        summeryRow = null;
                                        //                        isfirlstlog = true;
                                        //                        break;
                                        //                    }
                                        //                    double totaldistance = presodometer - prevodometer;
                                        //                    totaldistance = Math.Abs(totaldistance);
                                        //                    summeryRow["Distance(Kms)"] = totaldistance.ToString("00.00");
                                        //                    double Cost = 0;
                                        //                    double.TryParse(txtCost.Text, out Cost);
                                        //                    double totcost = 0;
                                        //                    totcost = totaldistance * Cost;
                                        //                    summeryRow["Total Cost"] = totcost.ToString("00.00");

                                        //                    DateTime l1et = Reachingdt;
                                        //                    DateTime l1lt = Startingdt;

                                        //                    TimeSpan l1ets = new TimeSpan(l1et.Ticks);
                                        //                    TimeSpan l1lts = new TimeSpan(l1lt.Ticks);
                                        //                    TimeSpan difftime = l1ets.Subtract(l1lts);

                                        //                    if (l1et.Ticks != l1lt.Ticks)
                                        //                    {
                                        //                        //summeryRow["Location1LeftTime"] = Prevrow["DateTime"];
                                        //                    }
                                        //                    else
                                        //                    {
                                        //                        summeryTable.Rows.Remove(summeryRow);
                                        //                        sno--;
                                        //                        summeryRow = null;
                                        //                        isfirlstlog = true;
                                        //                        break;
                                        //                    }
                                        //                    if ((int)(difftime.TotalDays) > 0)
                                        //                    {
                                        //                        summeryRow["Journey Hours"] = (int)(difftime.TotalDays) + "Days " + (int)(difftime.TotalHours % 24) + "Hours " + (int)(difftime.TotalMinutes % 60) + "Min ";
                                        //                    }
                                        //                    else
                                        //                    {
                                        //                        summeryRow["Journey Hours"] = (int)(difftime.TotalHours % 24) + "Hours " + (int)(difftime.TotalMinutes % 60) + "Min ";
                                        //                    }

                                        //                    //  summeryRow["Journey Hours"] = (int)(difftime.TotalHours % 24) + "Hours " + (int)(difftime.TotalMinutes % 60) + "Min ";

                                        //                    islocation1 = false;
                                        //                }
                                        //                else
                                        //                {
                                        //                    summeryRow = summeryTable.NewRow();
                                        //                    summeryRow["SNo"] = sno;
                                        //                    summeryRow["VehicleNo"] = tripdatarow["VehicleID"];
                                        //                    summeryRow["From Location"] = Brncs["BranchID"];
                                        //                    //summeryRow["Location1EnteredTime"] = tripdatarow["DateTime"];
                                        //                    sno++;
                                        //                    summeryTable.Rows.Add(summeryRow);
                                        //                    isfirlstlog = false;
                                        //                    islocation1 = true;
                                        //                }
                                        //            }
                                        //            if (statusobserver[Brncs["BranchID"].ToString()] == "Out Side")
                                        //            {
                                        //                if (summeryRow != null && Prevrow != null)
                                        //                {
                                        //                    Startingdt = (DateTime)tripdatarow["DateTime"];
                                        //                    string Startdate = Startingdt.ToString("M/dd/yyyy");
                                        //                    string startTime = Startingdt.ToString("hh:mm:ss tt");
                                        //                    string[] Reachsplt = startTime.ToString().Split(' ');
                                        //                    if (Reachsplt.Length > 1)
                                        //                    {
                                        //                        int departuretimemin = 0;
                                        //                        int dephours = 0;
                                        //                        int depmin = 0;
                                        //                        int.TryParse(Reachsplt[0].Split(':')[0], out dephours);
                                        //                        int.TryParse(Reachsplt[0].Split(':')[1], out depmin);
                                        //                        //departuretimemin = 720 - ((dephours * 60) + depmin);

                                        //                        if (Reachsplt[1] == "PM")
                                        //                        {
                                        //                            if (Reachsplt[0].Split(':')[0] == "12")
                                        //                                departuretimemin = ((dephours * 60) + depmin);
                                        //                            else
                                        //                                departuretimemin = 720 + ((dephours * 60) + depmin);
                                        //                        }
                                        //                        else
                                        //                        {
                                        //                            if (Reachsplt[0].Split(':')[0] == "12")
                                        //                                departuretimemin = ((dephours * 60) + depmin) - 720;
                                        //                            else
                                        //                                departuretimemin = ((dephours * 60) + depmin);
                                        //                        }

                                        //                        //ddlTravels.Items.Add(dr["traveler_agent"].ToString());

                                        //                        int time = departuretimemin;
                                        //                        if ((time % 60) < 10 && (time / 60) < 10)
                                        //                        {
                                        //                            StDuration = "0" + time / 60 + ":" + "0" + time % 60;
                                        //                        }
                                        //                        else if ((time % 60) >= 10 && (time / 60) < 10)
                                        //                        {
                                        //                            StDuration = "0" + time / 60 + ":" + time % 60;
                                        //                        }
                                        //                        else if ((time % 60) < 10 && (time / 60) >= 10)
                                        //                        {
                                        //                            StDuration = time / 60 + ":" + "0" + time % 60;
                                        //                        }
                                        //                        else if ((time % 60) >= 10 && (time / 60) >= 10)
                                        //                        {
                                        //                            StDuration = time / 60 + ":" + time % 60;
                                        //                        }

                                        //                    }
                                        //                    if (islocation1)
                                        //                    {

                                        //                        summeryRow["Starting Date"] = Startdate;
                                        //                        summeryRow["Starting Time"] = StDuration;


                                        //                        prevodometer = double.Parse(tripdatarow["Odometer"].ToString());
                                        //                    }
                                        //                    else
                                        //                    {
                                        //                        summeryRow = null;
                                        //                        //isfirlstlog = true;

                                        //                        summeryRow = summeryTable.NewRow();
                                        //                        summeryRow["SNo"] = sno;
                                        //                        summeryRow["VehicleNo"] = tripdatarow["VehicleID"];
                                        //                        summeryRow["From Location"] = Brncs["BranchID"];
                                        //                        //summeryRow["Location1EnteredTime"] = prevtime;
                                        //                        summeryRow["Starting Date"] = Startdate;
                                        //                        summeryRow["Starting Time"] = StDuration;

                                        //                        sno++;
                                        //                        summeryTable.Rows.Add(summeryRow);
                                        //                        prevodometer = double.Parse(tripdatarow["Odometer"].ToString());

                                        //                    }

                                        //                }
                                        //            }
                                        //        }
                                        //        Prevrow = tripdatarow;
                                        //        //}
                                        //    }
                                        //}
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
                                if (grdReports.Rows[i].Cells[4].Text == "Total")
                                {
                                    grdReports.Rows[i].Cells[0].Controls.Remove(grdReports.Rows[i].Cells[0].Controls[1]);
                                    grdReports.Rows[i].Cells[1].Controls.Remove(grdReports.Rows[i].Cells[1].Controls[1]);
                                }
                            }
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
            lbl_nofifier.Text = ex.Message;
        }
    }

    protected void btn_compare_Click(object sender, EventArgs e)
    {
        try
        {
            int vehcnt = 1;
            string vehicle1 = "";
            string fromtime1 = "";
            string totime1 = "";
            string vehicle2 = "";
            string fromtime2 = "";
            string totime2 = "";
            foreach (GridViewRow row in grdReports.Rows)
            {
                CheckBox gvchkbx = row.FindControl("myCheckBox") as CheckBox;
                if (gvchkbx.Checked == true)
                {
                    if (vehcnt > 2)
                    {
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alert", "alert('Please check only two vehicles to compare!')", true);
                        return;
                    }
                    if (vehcnt == 1)
                    {
                        vehicle1 = row.Cells[3].Text;
                        string[] frmdtary = row.Cells[5].Text.Split('/');
                        if (frmdtary[0].Length == 1)
                            frmdtary[0] = "0" + frmdtary[0];
                        if (frmdtary[1].Length == 1)
                            frmdtary[1] = "0" + frmdtary[1];
                        fromtime1 = frmdtary[2] + "-" + frmdtary[0] + "-" + frmdtary[1] + "T" + row.Cells[6].Text + ":00";
                        string[] todtary = row.Cells[8].Text.Split('/');
                        if (todtary[0].Length == 1)
                            todtary[0] = "0" + todtary[0];
                        if (todtary[1].Length == 1)
                            todtary[1] = "0" + todtary[1];
                        totime1 = todtary[2] + "-" + todtary[0] + "-" + todtary[1] + "T" + row.Cells[9].Text + ":00";
                    }
                    else
                    {
                        vehicle2 = row.Cells[3].Text;
                        string[] frmdtary = row.Cells[5].Text.Split('/');
                        if (frmdtary[0].Length == 1)
                            frmdtary[0] = "0" + frmdtary[0];
                        if (frmdtary[1].Length == 1)
                            frmdtary[1] = "0" + frmdtary[1];
                        fromtime2 = frmdtary[2] + "-" + frmdtary[0] + "-" + frmdtary[1] + "T" + row.Cells[6].Text + ":00";
                        string[] todtary = row.Cells[8].Text.Split('/');
                        if (todtary[0].Length == 1)
                            todtary[0] = "0" + todtary[0];
                        if (todtary[1].Length == 1)
                            todtary[1] = "0" + todtary[1];
                        totime2 = todtary[2] + "-" + todtary[0] + "-" + todtary[1] + "T" + row.Cells[9].Text + ":00";
                    }
                    vehcnt++;
                }
            }
            if (vehcnt != 3)
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alert", "alert('Please check two vehicles to compare!')", true);
                return;
            }
            for (int i = 0; i < grdReports.Rows.Count; i++)
            {
                if (grdReports.Rows[i].Cells[4].Text == "Total")
                {
                    grdReports.Rows[i].Cells[0].Controls.Remove(grdReports.Rows[i].Cells[0].Controls[1]);
                    grdReports.Rows[i].Cells[1].Controls.Remove(grdReports.Rows[i].Cells[1].Controls[1]);
                }
            }
            //2015-07-20T07:39:57
            //7/22/2015 09:11:00
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('CompareRoutes.aspx?fromtime1=" + fromtime1 + "&totime1=" + totime1 + "&vehicle1=" + vehicle1 + "&fromtime2=" + fromtime2 + "&totime2=" + totime2 + "&vehicle2=" + vehicle2 + "','_blank')", true);
        }
        catch
        {
        }
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
            if (reportname == "Location To Location Report")
            {
                string fromtime = row.Cells[5].Text + " " + row.Cells[6].Text + ":00";
                string totime = row.Cells[8].Text + " " + row.Cells[9].Text + ":00";
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
                for (int i = 0; i < grdReports.Rows.Count; i++)
                {
                    if (grdReports.Rows[i].Cells[4].Text == "Total")
                    {
                        grdReports.Rows[i].Cells[0].Controls.Remove(grdReports.Rows[i].Cells[0].Controls[1]);
                        grdReports.Rows[i].Cells[1].Controls.Remove(grdReports.Rows[i].Cells[1].Controls[1]);
                    }
                }
            }
        }
        catch
        {
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