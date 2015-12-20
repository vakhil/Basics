using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;
using System.Globalization;
using GPSApplication;

public partial class TripReport : System.Web.UI.Page
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
                    FillSelectVehicle();
                    UpdateVehicleGroupData();
                    startdate.Text = GetLowDate(DateTime.Now).ToString("dd-MM-yyyy HH:mm");
                    enddate.Text = GetHighDate(DateTime.Now).ToString("dd-MM-yyyy HH:mm");
                    reportname = Request.QueryString["Report"];
                    lblreportname.Text = reportname;
                }
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
                            cmd = new MySqlCommand("SELECT paireddata.VehicleNumber, tripconfiguration.PlantName,tripconfiguration.TripName,tripconfiguration.extrakms, routetable.RouteName, tripconfiguration.UserID, tripconfiguration.StartTime, tripconfiguration.EndTime, routesubtable.Rank, branchdata.BranchID, branchdata.Latitude, branchdata.Longitude, branchdata.Radious,routesubtable.SNo FROM routesubtable INNER JOIN routetable ON routesubtable.SNo = routetable.SNo INNER JOIN tripconfiguration ON routetable.SNo = tripconfiguration.RouteID INNER JOIN branchdata ON routesubtable.LocationID = branchdata.Sno INNER JOIN paireddata ON tripconfiguration.UserID = paireddata.UserID AND tripconfiguration.Veh_Sno = paireddata.Sno WHERE (tripconfiguration.UserID = @UserID) AND (paireddata.VehicleNumber = @VehicleNumber) AND (tripconfiguration.StartTime > @tme) AND (tripconfiguration.StartTime < @tme1)  ORDER BY tripconfiguration.sno, routesubtable.Rank");
                            cmd.Parameters.Add("@tme", spanAMfromtime);
                            cmd.Parameters.Add("@tme1", spanAMtotime);
                            cmd.Parameters.Add("@UserID", mainuser);
                            cmd.Parameters.Add("@VehicleNumber", vehiclestr);
                            DataTable AMresult = vdm.SelectQuery(cmd).Tables[0];
                            cmd = new MySqlCommand("SELECT paireddata.VehicleNumber, tripconfiguration.PlantName,tripconfiguration.TripName,tripconfiguration.extrakms, routetable.RouteName, tripconfiguration.UserID, tripconfiguration.StartTime, tripconfiguration.EndTime, routesubtable.Rank, branchdata.BranchID, branchdata.Latitude, branchdata.Longitude, branchdata.Radious,routesubtable.SNo FROM routesubtable INNER JOIN routetable ON routesubtable.SNo = routetable.SNo INNER JOIN tripconfiguration ON routetable.SNo = tripconfiguration.RouteID INNER JOIN branchdata ON routesubtable.LocationID = branchdata.Sno INNER JOIN paireddata ON tripconfiguration.UserID = paireddata.UserID AND tripconfiguration.Veh_Sno = paireddata.Sno WHERE (tripconfiguration.UserID = @UserID) AND (paireddata.VehicleNumber = @VehicleNumber) AND (tripconfiguration.StartTime > @tme) AND (tripconfiguration.StartTime < @tme1)  ORDER BY tripconfiguration.sno, routesubtable.Rank");
                            cmd.Parameters.Add("@tme", spanPMfromtime);
                            cmd.Parameters.Add("@tme1", spanPMtotime);
                            cmd.Parameters.Add("@UserID", mainuser);
                            cmd.Parameters.Add("@VehicleNumber", vehiclestr);
                            DataTable PMresult = vdm.SelectQuery(cmd).Tables[0];

                            DataTable amuniqueTrips = AMresult.DefaultView.ToTable(true, "sno", "StartTime", "EndTime", "RouteName", "VehicleNumber");
                            DataTable amunderTrips = AMresult.DefaultView.ToTable(true, "sno", "rank", "BranchID", "StartTime", "Latitude", "Longitude", "Radious");
                            DataTable pmuniqueTrips = PMresult.DefaultView.ToTable(true, "sno", "StartTime", "EndTime", "RouteName", "VehicleNumber");
                            DataTable pmunderTrips = PMresult.DefaultView.ToTable(true, "sno", "rank", "BranchID", "StartTime", "Latitude", "Longitude", "Radious");

                        
                          
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
                                cmd = new MySqlCommand("SELECT branchdata.BranchID, branchdata.UserName, branchdata.Description, branchdata.Latitude, branchdata.Longitude, branchdata.PhoneNumber, branchdata.ImagePath, branchdata.ImageType, branchdata.Radious, branchdata.PlantName FROM routesubtable INNER JOIN branchdata ON routesubtable.LocationID = branchdata.Sno WHERE (routesubtable.SNo = @SNo) ORDER BY routesubtable.Rank");
                                cmd.Parameters.Add("@SNo", AMresult.Rows[0]["SNo"].ToString());
                                DataTable routesubtabledata = vdm.SelectQuery(cmd).Tables[0];

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
                                    DataRow[] rows = AMresult.Select("SNo='" + AMresult.Rows[0]["SNo"].ToString() + "'");
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
                                cmd = new MySqlCommand("SELECT branchdata.BranchID, branchdata.UserName, branchdata.Description, branchdata.Latitude, branchdata.Longitude, branchdata.PhoneNumber, branchdata.ImagePath, branchdata.ImageType, branchdata.Radious, branchdata.PlantName FROM routesubtable INNER JOIN branchdata ON routesubtable.LocationID = branchdata.Sno WHERE (routesubtable.SNo = @SNo) ORDER BY routesubtable.Rank");
                                cmd.Parameters.Add("@SNo", PMresult.Rows[0]["SNo"].ToString());
                                DataTable routesubtabledata = vdm.SelectQuery(cmd).Tables[0];
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
                                    DataRow[] rows = PMresult.Select("SNo='" + PMresult.Rows[0]["SNo"].ToString() + "'");
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
                        grdReports.Rows[grdReports.Rows.Count - 1].Cells[0].Controls.Remove(grdReports.Rows[grdReports.Rows.Count - 1].Cells[0].Controls[2]);
                    }
                    #endregion
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
            statusobserver.Add(dr["BranchID"].ToString(), "In Side");
        }
        for (int cnt = 0; cnt < BranchDetails.Rows.Count; cnt++)
        {
            foreach (DataRow tripdatarow in TripData.Rows)
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
    protected void OnclickDrawAMRoute(object sender, CommandEventArgs e)
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
            if (row.Cells[5].Text != "" && row.Cells[5].Text != "&nbsp;")
            {
                string tripdate = row.Cells[2].Text;
                string newdate = "";
                string[] timearry;
                if (tripdate.Contains('/'))
                {
                    timearry = tripdate.Split('/');
                    foreach (string s in timearry)
                    {
                        if (s.Length == 1)
                        {
                            newdate += '0' + s + '/';
                        }
                        else
                        {
                            newdate += s + '/';
                        }
                    }
                    newdate = newdate.Remove(newdate.Length - 1);
                }
                else if (tripdate.Contains('-'))
                {
                    timearry = tripdate.Split('-');
                    foreach (string s in timearry)
                    {
                        if (s.Length == 1)
                        {
                            newdate += '0' + s + '-';
                        }
                        else
                        {
                            newdate += s + '-';
                        }
                    }
                    newdate = newdate.Remove(newdate.Length - 1);
                }

                string fromtime = newdate + " " + row.Cells[5].Text + ":00";
                string totime = newdate + " " + row.Cells[6].Text + ":00";
                DateTime fromdate = DateTime.ParseExact(fromtime, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                DateTime todate = DateTime.ParseExact(totime, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
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
                ScriptManager.RegisterStartupScript(this, GetType(), "NormalOpening", "NormalOpening();", true);
            }
            if (grdReports.Rows.Count > 0)
            {
                grdReports.Rows[grdReports.Rows.Count - 1].Cells[0].Controls.Remove(grdReports.Rows[grdReports.Rows.Count - 1].Cells[0].Controls[1]);
                grdReports.Rows[grdReports.Rows.Count - 1].Cells[0].Controls.Remove(grdReports.Rows[grdReports.Rows.Count - 1].Cells[0].Controls[2]);
            }
        }
        catch
        {
        }
    }
    protected void OnclickDrawPMRoute(object sender, CommandEventArgs e)
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
            if (row.Cells[10].Text != "" && row.Cells[10].Text != "&nbsp;")
            {
                string tripdate = row.Cells[2].Text;
                string newdate = "";
                string[] timearry;
                if (tripdate.Contains('/'))
                {
                    timearry = tripdate.Split('/');
                    foreach (string s in timearry)
                    {
                        if (s.Length == 1)
                        {
                            newdate += '0' + s + '/';
                        }
                        else
                        {
                            newdate += s + '/';
                        }
                    }
                    newdate = newdate.Remove(newdate.Length - 1);
                }
                else if (tripdate.Contains('-'))
                {
                    timearry = tripdate.Split('-');
                    foreach (string s in timearry)
                    {
                        if (s.Length == 1)
                        {
                            newdate += '0' + s + '-';
                        }
                        else
                        {
                            newdate += s + '-';
                        }
                    }
                    newdate = newdate.Remove(newdate.Length - 1);
                }


                string fromtime = newdate + " " + row.Cells[10].Text + ":00";
                string totime = newdate + " " + row.Cells[11].Text + ":00";
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
                ScriptManager.RegisterStartupScript(this, GetType(), "NormalOpening", "NormalOpening();", true);
            }
            if (grdReports.Rows.Count > 0)
            {
                grdReports.Rows[grdReports.Rows.Count - 1].Cells[0].Controls.Remove(grdReports.Rows[grdReports.Rows.Count - 1].Cells[0].Controls[1]);
                grdReports.Rows[grdReports.Rows.Count - 1].Cells[0].Controls.Remove(grdReports.Rows[grdReports.Rows.Count - 1].Cells[0].Controls[2]);
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