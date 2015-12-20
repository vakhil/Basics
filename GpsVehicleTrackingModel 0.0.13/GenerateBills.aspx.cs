using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using MySql.Data.MySqlClient;
using GPSApplication;
using System.Globalization;
using System.Drawing;

public partial class GenerateBills : System.Web.UI.Page
{
    MySqlCommand cmd;
    //string UserName = "";
    //string VehicleID = "";
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
    DataTable routes = new DataTable();
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
                        ddwnldr = new DataDownloader();
                        ddwnldr.UpdateBranchDetails(UserName);
                        PL1 = new GooglePolyline();
                        PL1.ID = "PL1";
                        //Give Hex code for line color
                        PL1.ColorCode = "#0000FF";
                        //Specify width for line
                        PL1.Width = 5;
                        routes = new DataTable();
                        cmd = new MySqlCommand("SELECT SNo,UserName, BranchID, Description, Latitude, Longitude, PhoneNumber, ImagePath, ImageType, Radious, PlantName, IsPlant FROM branchdata WHERE (UserName = @UserName) AND (IsPlant = '1')");
                        cmd.Parameters.Add("@UserName", UserName);
                        DataTable plants = vdm.SelectQuery(cmd).Tables[0];
                        ddl_plant.Items.Clear();
                        DataRow row = plants.NewRow();
                        row["SNo"] = 0;
                        row["BranchID"] = "Select Plant";
                        plants.Rows.InsertAt(row, 0);
                        ddl_plant.DataSource = plants;
                        ddl_plant.DataTextField = "BranchID";
                        ddl_plant.DataValueField = "SNo";
                        ddl_plant.DataBind();
                        cmd = new MySqlCommand("SELECT routetable.RouteName, tripconfiguration.StartTime, tripconfiguration.EndTime, routetable.SNo, tripconfiguration.Kms, tripconfiguration.extrakms, tripconfiguration.Chargeperkm, tripconfiguration.TripName FROM tripconfiguration INNER JOIN branchdata ON tripconfiguration.PlantName = branchdata.Sno INNER JOIN cabmanagement ON branchdata.BranchID = cabmanagement.PlantName INNER JOIN loginsconfigtable ON cabmanagement.VehicleID = loginsconfigtable.VehicleID INNER JOIN loginstable ON loginsconfigtable.Refno = loginstable.refno INNER JOIN routetable ON tripconfiguration.RouteID = routetable.SNo WHERE (loginstable.loginid = @userid) GROUP BY routetable.RouteName, tripconfiguration.TripName");
                        //cmd = new MySqlCommand("SELECT routetable.RouteName, tripconfiguration.StartTime, tripconfiguration.EndTime, routetable.SNo, tripconfiguration.Kms, tripconfiguration.extrakms, tripconfiguration.Chargeperkm FROM tripconfiguration INNER JOIN branchdata ON tripconfiguration.PlantName = branchdata.Sno INNER JOIN cabmanagement ON branchdata.BranchID = cabmanagement.PlantName INNER JOIN loginsconfigtable ON cabmanagement.VehicleID = loginsconfigtable.VehicleID INNER JOIN loginstable ON loginsconfigtable.Refno = loginstable.refno INNER JOIN routetable ON tripconfiguration.RouteID = routetable.SNo WHERE (loginstable.loginid = @userid) GROUP BY routetable.RouteName");
                        cmd.Parameters.Add("@userid", UserName);
                        routes = vdm.SelectQuery(cmd).Tables[0];
                        Session["routes"] = routes;
                        ddl_routes.Items.Clear();
                        DataRow newrow = routes.NewRow();
                        newrow["SNo"] = 0;
                        newrow["RouteName"] = "Select Route";
                        routes.Rows.InsertAt(newrow, 0);
                        ddl_routes.DataSource = routes;
                        ddl_routes.DataTextField = "RouteName";
                        ddl_routes.DataValueField = "SNo";
                        ddl_routes.DataBind();
                        btn_save.Visible = false;

                        startdate.Text = GetLowDate(DateTime.Now).ToString("dd-MM-yyyy HH:mm");
                    }
                    catch
                    {
                    }
                }
            }
        }
    }

    protected void btn_save_Click(object sender, EventArgs e)
    {
        if (startdate.Text != "")
        {
            if (Session["field1"] == null)
                Response.Redirect("Login.aspx");
            else
            {
                UserName = Session["field1"].ToString();
            }
            vdm = new VehicleDBMgr();
            vdm.InitializeDB();
            DateTime fromdate = DateTime.Today;//System.Convert.ToDateTime(startdate.Text);//startdate_CalendarExtender.SelectedDate ?? DateTime.Now;// DateTime.Now.AddMonths(-3);//DateTime.Parse(startdate.Text); ;
            DateTime todate = DateTime.Today;//System.Convert.ToDateTime(enddate.Text);//enddate_CalendarExtender.SelectedDate ?? DateTime.Now; //DateTime.Parse(enddate.Text);
            // d/M/yyyy HH:mm
            string[] datestrig = startdate.Text.Split(' ');

            if (datestrig.Length > 1)
            {
                if (datestrig[0].Split('-').Length > 0)
                {
                    string[] dates = datestrig[0].Split('-');
                    string[] times = datestrig[1].Split(':');
                    fromdate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                }
            }
            else
            {
                // MessageBox.Show("Date Time Format Wrong");
                lbl_nofifier.Text = "Date Time Format Wrong";
                return;
            }
            if (lbl_actualkms.Text == "")
            {
                lbl_actualkms.Text = "0";
            }
            if (lbl_gpskms.Text == "")
            {
                lbl_gpskms.Text = "0";
            }
            if (lbl_emptykms.Text == "")
            {
                lbl_emptykms.Text = "0";
            }
            if (lbl_gpsandemptykms.Text == "")
            {
                lbl_gpsandemptykms.Text = "0";
            }
            if (txt_billingkms.Text == "")
            {
                txt_billingkms.Text = "0";
            }
            if (lbl_totalcharge.Text == "")
            {
                lbl_totalcharge.Text = "0";
            }
            if (lbl_charge.Text == "")
            {
                lbl_charge.Text = "0";
            }
            cmd = new MySqlCommand("insert into distancestatistics (UserID, RouteName_sno, TripMeridian, G_Date, Actual_kms, GPS_kms, Billing_kms, Remarks,ChargePerKM,TotalCharge) values (@UserID, @RouteName_sno, @TripMeridian, @G_Date, @Actual_kms, @GPS_kms, @Billing_kms, @Remarks,@ChargePerKM,@TotalCharge)");
            cmd.Parameters.Add("@UserID", UserName);
            cmd.Parameters.Add("@RouteName_sno", ddl_routes.SelectedValue);
            cmd.Parameters.Add("@TripMeridian", ddl_trip.SelectedValue);
            cmd.Parameters.Add("@G_Date", fromdate);
            cmd.Parameters.Add("@Actual_kms", lbl_actualkms.Text);
            cmd.Parameters.Add("@GPS_kms", lbl_gpsandemptykms.Text);
            cmd.Parameters.Add("@Billing_kms", txt_billingkms.Text);
            cmd.Parameters.Add("@ChargePerKM", lbl_charge.Text);
            cmd.Parameters.Add("@TotalCharge", lbl_totalcharge.Text);
            cmd.Parameters.Add("@Remarks", "");
            vdm.insert(cmd);
            lbl_savemsg.Text = "Data successfully inserted";
            btn_save.Visible = false;
            lbl_emptykms.Text = "0";
            lbl_actualkms.Text = "0";
            lbl_charge.Text = "0";
            lbl_totalcharge.Text = "0";
            lbl_nofifier.Text = "";
            lbl_gpskms.Text = "0";
            lbl_gpsandemptykms.Text = "0";
            txt_billingkms.Text = "0";
        }
        else
        {
            // MessageBox.Show("Date Time Format Wrong");
            lbl_savemsg.Text = "Date Time Format Wrong";
            return;
        }
    }
    protected void ddl_routes_selectindexchanged(object sender, EventArgs e)
    {
        lbl_savemsg.Text = "";
        ddl_trip.Items.Clear();
        if (Session["routes"] != null)
        {
            routes = (DataTable)Session["routes"];
        }
        else
        {
            cmd = new MySqlCommand("SELECT routetable.RouteName, tripconfiguration.StartTime, tripconfiguration.EndTime, routetable.SNo, tripconfiguration.Kms, tripconfiguration.extrakms, tripconfiguration.Chargeperkm, tripconfiguration.TripName FROM tripconfiguration INNER JOIN branchdata ON tripconfiguration.PlantName = branchdata.Sno INNER JOIN cabmanagement ON branchdata.BranchID = cabmanagement.PlantName INNER JOIN loginsconfigtable ON cabmanagement.VehicleID = loginsconfigtable.VehicleID INNER JOIN loginstable ON loginsconfigtable.Refno = loginstable.refno INNER JOIN routetable ON tripconfiguration.RouteID = routetable.SNo WHERE (loginstable.loginid = @userid) GROUP BY routetable.RouteName, tripconfiguration.TripName");
            //cmd = new MySqlCommand("SELECT routetable.RouteName, tripconfiguration.StartTime, tripconfiguration.EndTime, routetable.SNo, tripconfiguration.Kms,tripconfiguration.extrakms,tripconfiguration.Chargeperkm FROM loginsconfigtable INNER JOIN loginstable ON loginsconfigtable.Refno = loginstable.refno INNER JOIN cabmanagement ON loginsconfigtable.VehicleID = cabmanagement.VehicleID INNER JOIN vehiclemanage ON cabmanagement.PlantName = vehiclemanage.ItemName INNER JOIN tripconfiguration ON vehiclemanage.SNo = tripconfiguration.PlantName INNER JOIN routetable ON tripconfiguration.RouteID = routetable.SNo WHERE (loginstable.loginid = @userid) GROUP BY routetable.RouteName");
            cmd.Parameters.Add("@userid", UserName);
            routes = vdm.SelectQuery(cmd).Tables[0];
            Session["routes"] = routes;
        }
        DataRow[] rows = routes.Select("SNo='" + ddl_routes.SelectedValue + "'");

        ddl_trip.Items.Add("Select");
        foreach (DataRow row in rows)
        {
            ddl_trip.Items.Add(row["TripName"].ToString());
        }
    }
    protected void ddl_trip_selectindexchanged(object sender, EventArgs e)
    {
        if (ddl_trip.SelectedValue == "Select")
        {
            return;
        }
        lbl_savemsg.Text = "";
        if (Session["routes"] != null)
        {
            routes = (DataTable)Session["routes"];
        }
        else
        {
            cmd = new MySqlCommand("SELECT routetable.RouteName, tripconfiguration.StartTime, tripconfiguration.EndTime, routetable.SNo, tripconfiguration.Kms, tripconfiguration.extrakms, tripconfiguration.Chargeperkm, tripconfiguration.TripName FROM tripconfiguration INNER JOIN branchdata ON tripconfiguration.PlantName = branchdata.Sno INNER JOIN cabmanagement ON branchdata.BranchID = cabmanagement.PlantName INNER JOIN loginsconfigtable ON cabmanagement.VehicleID = loginsconfigtable.VehicleID INNER JOIN loginstable ON loginsconfigtable.Refno = loginstable.refno INNER JOIN routetable ON tripconfiguration.RouteID = routetable.SNo WHERE (loginstable.loginid = @userid) GROUP BY routetable.RouteName, tripconfiguration.TripName");
            //cmd = new MySqlCommand("SELECT routetable.RouteName, tripconfiguration.StartTime, tripconfiguration.EndTime, routetable.SNo, tripconfiguration.Kms,tripconfiguration.extrakms,tripconfiguration.Chargeperkm FROM loginsconfigtable INNER JOIN loginstable ON loginsconfigtable.Refno = loginstable.refno INNER JOIN cabmanagement ON loginsconfigtable.VehicleID = cabmanagement.VehicleID INNER JOIN vehiclemanage ON cabmanagement.PlantName = vehiclemanage.ItemName INNER JOIN tripconfiguration ON vehiclemanage.SNo = tripconfiguration.PlantName INNER JOIN routetable ON tripconfiguration.RouteID = routetable.SNo WHERE (loginstable.loginid = @userid) GROUP BY routetable.RouteName");
            cmd.Parameters.Add("@userid", UserName);
            routes = vdm.SelectQuery(cmd).Tables[0];
            Session["routes"] = routes;
        }
        DataRow[] rows = routes.Select("SNo='" + ddl_routes.SelectedValue + "' and TripName='" + ddl_trip.SelectedValue + "'");

        if (rows.Length > 0)
        {
            double charge = 0;
            double actualkms = 0;
            double emptykms = 0;
            double.TryParse(rows[0]["Kms"].ToString(), out actualkms);
            double.TryParse(rows[0]["Chargeperkm"].ToString(), out charge);
            double.TryParse(rows[0]["extrakms"].ToString(), out emptykms);
            lbl_emptykms.Text = emptykms.ToString();
            lbl_actualkms.Text = actualkms.ToString();
            lbl_charge.Text = charge.ToString();
            lbl_nofifier.Text = "";
            lbl_totalcharge.Text = "0";
        }
        else
        {
            lbl_emptykms.Text = "0";
            lbl_actualkms.Text = "0";
            lbl_charge.Text = "0";
            lbl_totalcharge.Text = "0";
            lbl_nofifier.Text = "";
        }
        lbl_gpskms.Text = "0";
        lbl_gpsandemptykms.Text = lbl_emptykms.Text;
        txt_billingkms.Text = "0";
        grdReports.DataSource = null;
        grdReports.DataBind();
        lbl_savemsg.Text = "";
        btn_save.Visible = false;
    }
    protected void txt_billingkmstxtchngd(object sender, EventArgs e)
    {
        lbl_totalcharge.Text = "0";
        double charge = 0;
        double billingkms = 0;
        double totalcharge = 0;
        double.TryParse(lbl_charge.Text, out charge);
        double.TryParse(txt_billingkms.Text, out billingkms);
        totalcharge = charge * billingkms;
        lbl_totalcharge.Text = totalcharge.ToString();
    }
    Dictionary<string, DataTable> reportData = new Dictionary<string, DataTable>();
    string mainuser = "";
    protected void btn_generate_Click(object sender, EventArgs e)
    {
        try
        {
            lbl_savemsg.Text = "";
            if (ddl_trip.SelectedValue == "Select")
            {
                lbl_nofifier.Text = "Please select Trip";
                return;
            }
            if (ddl_routes.SelectedIndex == 0)
            {
                lbl_nofifier.Text = "Please select Route";
                return;
            }
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
            DataTable sampletable = new DataTable();
            grdReports.DataSource = sampletable;
            grdReports.DataBind();
            lbl_nofifier.Text = "";
            reportData = new Dictionary<string, DataTable>();
            if (startdate.Text != "")
            {
                DateTime fromdate = DateTime.Today;//System.Convert.ToDateTime(startdate.Text);//startdate_CalendarExtender.SelectedDate ?? DateTime.Now;// DateTime.Now.AddMonths(-3);//DateTime.Parse(startdate.Text); ;
                DateTime todate = DateTime.Today;//System.Convert.ToDateTime(enddate.Text);//enddate_CalendarExtender.SelectedDate ?? DateTime.Now; //DateTime.Parse(enddate.Text);
                // d/M/yyyy HH:mm
                string[] datestrig = startdate.Text.Split(' ');

                if (datestrig.Length > 1)
                {
                    if (datestrig[0].Split('-').Length > 0)
                    {
                        string[] dates = datestrig[0].Split('-');
                        string[] times = datestrig[1].Split(':');
                        fromdate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                    }
                }
                else
                {
                    // MessageBox.Show("Date Time Format Wrong");
                    lbl_nofifier.Text = "From Date Time Format Wrong";
                    return;
                }
                cmd = new MySqlCommand("SELECT branchdata.BranchID, branchdata.UserName, branchdata.Description, branchdata.Latitude, branchdata.Longitude, branchdata.PhoneNumber, branchdata.ImagePath, branchdata.ImageType, branchdata.Radious, branchdata.PlantName FROM routesubtable INNER JOIN branchdata ON routesubtable.LocationID = branchdata.Sno WHERE (routesubtable.SNo = @SNo) ORDER BY routesubtable.Rank");
                cmd.Parameters.Add("@SNo", ddl_routes.SelectedValue);
                DataTable routesubtabledata = vdm.SelectQuery(cmd).Tables[0];
                DDL_locations.Items.Clear();
                DDL_locations.Items.Add("ALL");
                foreach (DataRow dr in routesubtabledata.Rows)
                {
                    DDL_locations.Items.Add(dr["BranchID"].ToString().TrimEnd());
                }

                cmd = new MySqlCommand("SELECT UserID, RouteName_sno, TripMeridian, G_Date, Actual_kms, GPS_kms, Billing_kms, Remarks, ChargePerKM, TotalCharge FROM distancestatistics WHERE (RouteName_sno = @RouteName_sno) AND (TripMeridian = @TripMeridian) AND (G_Date = @G_Date) AND (UserID = @UserID)");
                cmd.Parameters.Add("@UserID", UserName);
                cmd.Parameters.Add("@RouteName_sno", ddl_routes.SelectedValue);
                cmd.Parameters.Add("@TripMeridian", ddl_trip.SelectedValue);
                cmd.Parameters.Add("@G_Date", fromdate);
                DataTable rslt = vdm.SelectQuery(cmd).Tables[0];

                if (Session["routes"] != null)
                {
                    routes = (DataTable)Session["routes"];
                }
                else
                {
                    cmd = new MySqlCommand("SELECT routetable.RouteName, tripconfiguration.StartTime, tripconfiguration.EndTime, routetable.SNo, tripconfiguration.Kms, tripconfiguration.extrakms, tripconfiguration.Chargeperkm, tripconfiguration.TripName FROM tripconfiguration INNER JOIN branchdata ON tripconfiguration.PlantName = branchdata.Sno INNER JOIN cabmanagement ON branchdata.BranchID = cabmanagement.PlantName INNER JOIN loginsconfigtable ON cabmanagement.VehicleID = loginsconfigtable.VehicleID INNER JOIN loginstable ON loginsconfigtable.Refno = loginstable.refno INNER JOIN routetable ON tripconfiguration.RouteID = routetable.SNo WHERE (loginstable.loginid = @userid) GROUP BY routetable.RouteName, tripconfiguration.TripName");
                    //cmd = new MySqlCommand("SELECT routetable.RouteName, tripconfiguration.StartTime, tripconfiguration.EndTime, routetable.SNo, tripconfiguration.Kms,tripconfiguration.extrakms,tripconfiguration.Chargeperkm FROM loginsconfigtable INNER JOIN loginstable ON loginsconfigtable.Refno = loginstable.refno INNER JOIN cabmanagement ON loginsconfigtable.VehicleID = cabmanagement.VehicleID INNER JOIN vehiclemanage ON cabmanagement.PlantName = vehiclemanage.ItemName INNER JOIN tripconfiguration ON vehiclemanage.SNo = tripconfiguration.PlantName INNER JOIN routetable ON tripconfiguration.RouteID = routetable.SNo WHERE (loginstable.loginid = @userid) GROUP BY routetable.RouteName");
                    cmd.Parameters.Add("@userid", UserName);
                    routes = vdm.SelectQuery(cmd).Tables[0];
                    Session["routes"] = routes;
                }

                DataRow[] triprows = routes.Select("SNo='" + ddl_routes.SelectedValue + "' and TripName='" + ddl_trip.SelectedValue + "'");

                if (triprows.Length > 0)
                {
                    fromdate = fromdate.Date.Add(TimeSpan.Parse(triprows[0]["StartTime"].ToString()));
                    todate = fromdate.Date.Add(TimeSpan.Parse(triprows[0]["EndTime"].ToString()));
                }
                cmd = new MySqlCommand("SELECT paireddata.VehicleNumber, tripconfiguration.PlantName, routetable.RouteName, tripconfiguration.UserID, tripconfiguration.StartTime, tripconfiguration.EndTime, tripconfiguration.Status, routesubtable.Rank, branchdata.BranchID, branchdata.Latitude, branchdata.Longitude, branchdata.Radious, tripconfiguration.Veh_Sno , tripconfiguration.sno FROM            routesubtable INNER JOIN routetable ON routesubtable.SNo = routetable.SNo INNER JOIN tripconfiguration ON routetable.SNo = tripconfiguration.RouteID INNER JOIN branchdata ON routesubtable.LocationID = branchdata.Sno INNER JOIN paireddata ON tripconfiguration.UserID = paireddata.UserID AND tripconfiguration.Veh_Sno = paireddata.Sno WHERE        (tripconfiguration.StartTime >= @tme) AND (tripconfiguration.StartTime <= @tme1) and (tripconfiguration.UserID = @UserID) AND (routetable.RouteName = @RouteName) ORDER BY tripconfiguration.sno, routesubtable.Rank ");
                // cmd = new MySqlCommand("SELECT tripdata.Refno, tripdata.Tripid, tripdata.assigndate, tripdata.completdate, tripdata.Status, tripdata.UserID, tripdata.RouteName, tripdata.PlantName, branchdata.BranchID, tripsubdata.rank, tripsubdata.intime, paireddata.VehicleNumber, tripdata.vehiclemaster_sno, branchdata.Latitude, branchdata.Longitude, branchdata.Radious FROM tripdata INNER JOIN tripsubdata ON tripdata.Refno = tripsubdata.Refno INNER JOIN branchdata ON tripsubdata.locid = branchdata.Sno INNER JOIN paireddata ON tripdata.vehiclemaster_sno = paireddata.Sno WHERE (tripdata.UserID = @UserID) AND (tripdata.assigndate >= @d1) AND (tripdata.assigndate <= @d2) AND (tripdata.RouteName = @RouteName) ORDER BY tripdata.Refno, tripsubdata.rank");
                cmd.Parameters.Add("@UserID", UserName);
                // TimeSpan ts = 
                MySqlParameter param = new MySqlParameter();
                param = new MySqlParameter();
                param.ParameterName = "@tme";
                param.DbType = DbType.Time;
                param.MySqlDbType = MySqlDbType.Time;
                param.IsNullable = true;
                param.Value = new TimeSpan(fromdate.Hour, fromdate.Minute, fromdate.Second);
                cmd.Parameters.Add(param);
                param = new MySqlParameter();
                param = new MySqlParameter();
                param.ParameterName = "@tme1";
                param.DbType = DbType.Time;
                param.MySqlDbType = MySqlDbType.Time;
                param.IsNullable = true;
                param.Value = new TimeSpan(todate.Hour, todate.Minute, todate.Second);
                cmd.Parameters.Add(param);
                cmd.Parameters.Add("@RouteName", ddl_routes.SelectedItem.Text);
                DataTable result = vdm.SelectQuery(cmd).Tables[0];
                DataTable uniqueTrips = result.DefaultView.ToTable(true, "sno", "StartTime", "EndTime", "RouteName", "VehicleNumber");
                DataTable underTrips = result.DefaultView.ToTable(true, "sno", "rank", "BranchID", "StartTime", "Latitude", "Longitude", "Radious");

                DateTime tstarttime = DateTime.Now;
                DateTime tendtime = DateTime.Now;
                if (rslt.Rows.Count > 0)
                {
                    if (Session["routes"] != null)
                    {
                        routes = (DataTable)Session["routes"];
                    }
                    else
                    {
                        cmd = new MySqlCommand("SELECT routetable.RouteName, tripconfiguration.StartTime, tripconfiguration.EndTime, routetable.SNo, tripconfiguration.Kms, tripconfiguration.extrakms, tripconfiguration.Chargeperkm, tripconfiguration.TripName FROM tripconfiguration INNER JOIN branchdata ON tripconfiguration.PlantName = branchdata.Sno INNER JOIN cabmanagement ON branchdata.BranchID = cabmanagement.PlantName INNER JOIN loginsconfigtable ON cabmanagement.VehicleID = loginsconfigtable.VehicleID INNER JOIN loginstable ON loginsconfigtable.Refno = loginstable.refno INNER JOIN routetable ON tripconfiguration.RouteID = routetable.SNo WHERE (loginstable.loginid = @userid) GROUP BY routetable.RouteName, tripconfiguration.TripName");
                        //cmd = new MySqlCommand("SELECT routetable.RouteName, tripconfiguration.StartTime, tripconfiguration.EndTime, routetable.SNo, tripconfiguration.Kms,tripconfiguration.extrakms,tripconfiguration.Chargeperkm FROM loginsconfigtable INNER JOIN loginstable ON loginsconfigtable.Refno = loginstable.refno INNER JOIN cabmanagement ON loginsconfigtable.VehicleID = cabmanagement.VehicleID INNER JOIN vehiclemanage ON cabmanagement.PlantName = vehiclemanage.ItemName INNER JOIN tripconfiguration ON vehiclemanage.SNo = tripconfiguration.PlantName INNER JOIN routetable ON tripconfiguration.RouteID = routetable.SNo WHERE (loginstable.loginid = @userid) GROUP BY routetable.RouteName");
                        cmd.Parameters.Add("@userid", UserName);
                        routes = vdm.SelectQuery(cmd).Tables[0];
                        Session["routes"] = routes;
                    }
                    DataRow[] rows = routes.Select("SNo='" + ddl_routes.SelectedValue + "'");
                    double extrakms = 0;
                    double totalkms = 0;
                    if (rows.Length > 0)
                    {
                        double.TryParse(rows[0]["extrakms"].ToString(), out extrakms);
                    }
                    double.TryParse(rslt.Rows[0]["GPS_kms"].ToString(), out totalkms);

                    lbl_emptykms.Text = extrakms.ToString();
                    lbl_gpskms.Text = (totalkms - extrakms).ToString();
                    lbl_gpsandemptykms.Text = rslt.Rows[0]["GPS_kms"].ToString();
                    lbl_actualkms.Text = rslt.Rows[0]["Actual_kms"].ToString();
                    txt_billingkms.Text = rslt.Rows[0]["Billing_kms"].ToString();
                    lbl_charge.Text = rslt.Rows[0]["ChargePerKM"].ToString();
                    lbl_totalcharge.Text = rslt.Rows[0]["TotalCharge"].ToString();
                    btn_save.Visible = false;
                    //if (uniqueTrips.Rows.Count > 0)
                    //{
                    //    if (uniqueTrips.Rows[0]["StartTime"].ToString() == "")
                    //    {
                    //        lbl_nofifier.Text = "There is no data in this trip";
                    //        return;
                    //    }
                    //    tstarttime = fromdate.Date.Add(TimeSpan.Parse(uniqueTrips.Rows[0]["StartTime"].ToString()));
                    //    if (uniqueTrips.Rows[0]["EndTime"].ToString() == "")
                    //    {
                    //        lbl_nofifier.Text = "Please let the trip complete.";
                    //        return;
                    //    }
                    //    tendtime = fromdate.Date.Add(TimeSpan.Parse(uniqueTrips.Rows[0]["EndTime"].ToString()));
                    //}
                }
                else
                {
                    btn_save.Visible = true;

                    double odometervalue = 0;
                    //double ometer = 0;
                    if (uniqueTrips.Rows.Count > 0)
                    {
                        //if (uniqueTrips.Rows[0]["StartTime"].ToString() == "")
                        //{
                        //    lbl_nofifier.Text = "There is no data in this trip";
                        //    return;
                        //}
                        //tstarttime = fromdate.Date.Add(TimeSpan.Parse(uniqueTrips.Rows[0]["StartTime"].ToString()));
                        //if (uniqueTrips.Rows[0]["EndTime"].ToString() == "")
                        //{
                        //    lbl_nofifier.Text = "Please let the trip complete.";
                        //    return;
                        //}
                        //if (ddl_trip.SelectedValue == "AM")
                        //{
                        //    tendtime = fromdate.Date.AddHours(13);
                        //}
                        //else
                        //{
                        //    tendtime = fromdate.Date.AddHours(25);
                        //}
                        //tendtime = fromdate.Date.Add(TimeSpan.Parse(uniqueTrips.Rows[0]["EndTime"].ToString()));
                        //tendtime = (DateTime)uniqueTrips.Rows[0]["EndTime"];
                        DataTable ResDT = new DataTable();
                        ResDT = getfirsttouchOdometer(uniqueTrips.Rows[0]["VehicleNumber"].ToString(), fromdate, todate, vdm, routesubtabledata, out odometervalue, mainuser);
                        if (ResDT.Rows.Count == 1)
                        {
                            lbl_nofifier.Text = "Please let the trip complete.";
                            return;
                        }
                        else if (ResDT.Rows.Count == 2)
                        {
                            if (Session["routes"] != null)
                            {
                                routes = (DataTable)Session["routes"];
                            }
                            else
                            {
                                cmd = new MySqlCommand("SELECT routetable.RouteName, tripconfiguration.StartTime, tripconfiguration.EndTime, routetable.SNo, tripconfiguration.Kms, tripconfiguration.extrakms, tripconfiguration.Chargeperkm, tripconfiguration.TripName FROM tripconfiguration INNER JOIN branchdata ON tripconfiguration.PlantName = branchdata.Sno INNER JOIN cabmanagement ON branchdata.BranchID = cabmanagement.PlantName INNER JOIN loginsconfigtable ON cabmanagement.VehicleID = loginsconfigtable.VehicleID INNER JOIN loginstable ON loginsconfigtable.Refno = loginstable.refno INNER JOIN routetable ON tripconfiguration.RouteID = routetable.SNo WHERE (loginstable.loginid = @userid) GROUP BY routetable.RouteName, tripconfiguration.TripName");
                                //cmd = new MySqlCommand("SELECT routetable.RouteName, tripconfiguration.StartTime, tripconfiguration.EndTime, routetable.SNo, tripconfiguration.Kms,tripconfiguration.extrakms,tripconfiguration.Chargeperkm FROM loginsconfigtable INNER JOIN loginstable ON loginsconfigtable.Refno = loginstable.refno INNER JOIN cabmanagement ON loginsconfigtable.VehicleID = cabmanagement.VehicleID INNER JOIN vehiclemanage ON cabmanagement.PlantName = vehiclemanage.ItemName INNER JOIN tripconfiguration ON vehiclemanage.SNo = tripconfiguration.PlantName INNER JOIN routetable ON tripconfiguration.RouteID = routetable.SNo WHERE (loginstable.loginid = @userid) GROUP BY routetable.RouteName");
                                cmd.Parameters.Add("@userid", UserName);
                                routes = vdm.SelectQuery(cmd).Tables[0];
                                Session["routes"] = routes;
                            }
                            DataRow[] rows = routes.Select("SNo='" + ddl_routes.SelectedValue + "'");
                            double extrakms = 0;
                            double.TryParse(lbl_emptykms.Text, out extrakms);
                            odometervalue = Math.Round(odometervalue, 2);
                            lbl_gpskms.Text = odometervalue.ToString();
                            odometervalue = odometervalue + extrakms;
                            odometervalue = Math.Round(odometervalue, 2);
                            lbl_gpsandemptykms.Text = odometervalue.ToString();
                            tstarttime = (DateTime)ResDT.Rows[0]["Reaching Date"];
                            tendtime = (DateTime)ResDT.Rows[1]["Reaching Date"];
                        }
                        else
                        {
                            lbl_nofifier.Text = "There is no data in this trip";
                            return;
                        }
                    }
                    else
                    {
                        lbl_nofifier.Text = "There is no data in this trip";
                        return;
                    }
                }
                #region location wise Reports
                DataTable summeryTable = new DataTable();
                DateTime Startingdt = DateTime.Now;
                string Duration = "";
                string StDuration = "";
                ddwnldr = new DataDownloader();
                ddwnldr.UpdateBranchDetails(UserName);
                string vehicls = "";
                ////string Status = "";
                int sno = 1;
                DataColumn summeryColumn = new DataColumn("SNo");
                summeryTable.Columns.Add(summeryColumn);
                summeryColumn = new DataColumn("VehicleNo");
                summeryTable.Columns.Add(summeryColumn);
                summeryColumn = new DataColumn("From Location");
                summeryTable.Columns.Add(summeryColumn);
                summeryColumn = new DataColumn("Starting Date");
                summeryTable.Columns.Add(summeryColumn);
                summeryColumn = new DataColumn("Starting Time");
                summeryTable.Columns.Add(summeryColumn);
                summeryColumn = new DataColumn("To Location");
                summeryTable.Columns.Add(summeryColumn);
                summeryColumn = new DataColumn("Reaching Date");
                summeryTable.Columns.Add(summeryColumn);
                summeryColumn = new DataColumn("Reaching Time");
                summeryTable.Columns.Add(summeryColumn);
                summeryColumn = new DataColumn("Distance(Kms)");
                summeryTable.Columns.Add(summeryColumn);
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

                lbl_ReportStatus.Text = "BILLING REPORT FROM: " + fromdate.ToString("M/dd/yyyy") + "  To: " + todate.ToString("M/dd/yyyy");
                Session["title"] = lbl_ReportStatus.Text;

                string vehiclestr = uniqueTrips.Rows[0]["VehicleNumber"].ToString();

                summeryRow = summeryTable.NewRow();
                summeryRow["VehicleNo"] = vehiclestr;
                summeryRow["From Location"] = routesubtabledata.Rows[routesubtabledata.Rows.Count - 1]["BranchID"].ToString();
                summeryRow["To Location"] = routesubtabledata.Rows[0]["BranchID"].ToString();
                summeryRow["Distance(Kms)"] = lbl_emptykms.Text;
                summeryTable.Rows.Add(summeryRow);

                bool isfirlstlog = true;
                bool islocation1 = true;
                DataTable logs = new DataTable();
                DataTable tottable = new DataTable();
                List<string> logstbls = new List<string>();
                logstbls.Add("GpsTrackVehicleLogs");
                logstbls.Add("GpsTrackVehicleLogs1");
                logstbls.Add("GpsTrackVehicleLogs2");
                logstbls.Add("GpsTrackVehicleLogs3");
                DataTable TripData = new DataTable();
                foreach (string tbname in logstbls)
                {
                    cmd = new MySqlCommand("SELECT '' AS SNo, " + tbname + ".VehicleID, " + tbname + ".DateTime, " + tbname + ".Speed, " + tbname + ".Latitiude, " + tbname + ".Longitude, " + tbname + ".Direction, " + tbname + ".Diesel, " + tbname + ".Odometer, " + tbname + ".Direction AS Expr1, " + tbname + ".Direction AS Expr2, vehiclemaster.MaintenancePlantName, vehiclemaster.VendorName, vehiclemaster.VendorNo, vehiclemaster.VehicleTypeName FROM " + tbname + " LEFT OUTER JOIN vehiclemaster ON " + tbname + ".VehicleID = vehiclemaster.VehicleID WHERE (" + tbname + ".DateTime >= @starttime) AND (" + tbname + ".DateTime <= @endtime) AND (" + tbname + ".VehicleID = '" + vehiclestr + "') and (" + tbname + ".UserID='" + mainuser + "')  ORDER BY " + tbname + ".DateTime");
                    //cmd = new MySqlCommand("select * from " + tbname + " where DateTime>= @starttime and DateTime<=@endtime and VehicleID='" + vehiclestr + "' and UserID='" + UserName + "' order by DateTime");
                    cmd.Parameters.Add(new MySqlParameter("@starttime", tstarttime));
                    cmd.Parameters.Add(new MySqlParameter("@endtime", tendtime));
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
                foreach (DataRow dr in routesubtabledata.Rows)
                {
                    statusobserver.Add(dr["BranchID"].ToString(), "");
                }
                foreach (DataRow dr in routesubtabledata.Rows)
                {
                    bool iscameinside = false;
                    foreach (DataRow tripdatarow in TripData.Rows)
                    {
                        DataRow[] branch = ddwnldr.BranchDetails.Select("BranchID='" + dr["BranchID"].ToString() + "'");

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
                                    iscameinside = true;
                                    if (!isfirlstlog)
                                    {

                                        summeryRow["To Location"] = Brncs["BranchID"];
                                        DateTime Reachingdt = (DateTime)tripdatarow["DateTime"];
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
                                        summeryRow["Journey Hours"] = (int)(difftime.TotalHours % 24) + "Hours " + (int)(difftime.TotalMinutes % 60) + "Min ";

                                        islocation1 = false;
                                    }
                                    else
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
                                if (iscameinside && statusobserver[Brncs["BranchID"].ToString()] == "Out Side")
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
                            Prevrow = tripdatarow;
                        }
                    }
                }
                if (summeryTable.Rows.Count > 0)
                {
                    int snocnt = 1;
                    for (int cnt = 1; cnt < summeryTable.Rows.Count; cnt++)
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
                grdReports.DataSource = summeryTable;
                grdReports.DataBind();
                grdReports.Rows[0].BackColor = System.Drawing.Color.LightYellow;
                grdReports.Rows[0].Font.Bold = true;
                Session["xportdata"] = summeryTable;
                divPieChart.Visible = false;

                #endregion
            }
            else
            {
                lbl_nofifier.Text = "Please Select  Start Date and End Date";
            }
        }
        catch (Exception ex)
        {
            lbl_nofifier.Text = ex.Message;
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
        //for (int cnt = 0; cnt < BranchDetails.Rows.Count; cnt++)
        //{
        //    DataRow Brncs = BranchDetails.Rows[cnt];
        //    foreach (DataRow tripdatarow in TripData.Rows)
        //    {
        //        double presLat = (double)tripdatarow["Latitiude"];
        //        double PresLng = (double)tripdatarow["Longitude"];
        //        double ag_Lat = 0;
        //        double.TryParse(Brncs["Latitude"].ToString(), out ag_Lat);
        //        double ag_lng = 0;
        //        double.TryParse(Brncs["Longitude"].ToString(), out ag_lng);
        //        double ag_radious = 100;
        //        double.TryParse(Brncs["Radious"].ToString(), out ag_radious);
        //        string statusvalue = GeoCodeCalc.getGeofenceStatus(presLat, PresLng, ag_Lat, ag_lng, ag_radious);
        //        if (statusobserver[Brncs["BranchID"].ToString()] == statusvalue)
        //        {
        //            DateTime Reachingdt = (DateTime)tripdatarow["DateTime"];
        //            if (iscompleted)
        //            {
        //                if (Reachingdt < prevdate.AddHours(1).AddMinutes(30))
        //                {
        //                    continue;
        //                }
        //            }
        //            summeryRow = summeryTable.NewRow();
        //            summeryRow["SNo"] = sno;
        //            summeryRow["From Location"] = Brncs["BranchID"];
        //            string Reachdate = Reachingdt.ToString("M/dd/yyyy");
        //            string ReachTime = Reachingdt.ToString("HH:mm");
        //            summeryRow["Reaching Date"] = Reachingdt.ToString();
        //            summeryRow["Reaching Time"] = ReachTime.ToString();
        //            presodometer = double.Parse(tripdatarow["Odometer"].ToString());
        //            summeryRow["Distance(Kms)"] = presodometer;
        //            summeryTable.Rows.Add(summeryRow);
        //            if (!iscompleted)
        //            {
        //                cnt = BranchDetails.Rows.Count - 2;
        //                prevdate = Reachingdt;
        //            }
        //            iscompleted = true;
        //            break;
        //        }
        //        Prevrow = tripdatarow;
        //    }
        //}
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
                    if (Reachingdt < prevdate.AddHours(1))
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

        //foreach (DataRow tripdatarow in TripData.Rows)
        //{
        //    double presLat = (double)tripdatarow["Latitiude"];
        //    double PresLng = (double)tripdatarow["Longitude"];
        //    double ag_Lat = 0;
        //    double.TryParse(BranchDetails.Rows[BranchDetails.Rows.Count - 1]["Latitude"].ToString(), out ag_Lat);
        //    double ag_lng = 0;
        //    double.TryParse(BranchDetails.Rows[BranchDetails.Rows.Count - 1]["Longitude"].ToString(), out ag_lng);
        //    double ag_radious = 100;
        //    double.TryParse(BranchDetails.Rows[BranchDetails.Rows.Count - 1]["Radious"].ToString(), out ag_radious);
        //    string statusvalue = GeoCodeCalc.getGeofenceStatus(presLat, PresLng, ag_Lat, ag_lng, ag_radious);
        //    if (statusobserver[BranchDetails.Rows[BranchDetails.Rows.Count - 1]["BranchID"].ToString()] == statusvalue)
        //    {
        //        if (summeryTable.Rows.Count > 0)
        //        {
        //            DateTime Reachingdt = (DateTime)tripdatarow["DateTime"];
        //            prevdate = (DateTime)summeryTable.Rows[0]["Reaching Date"];
        //            if (Reachingdt < prevdate.AddHours(1).AddMinutes(30))
        //            {
        //                continue;
        //            }
        //            summeryRow = summeryTable.NewRow();
        //            summeryRow["SNo"] = sno;
        //            summeryRow["From Location"] = BranchDetails.Rows[BranchDetails.Rows.Count - 1]["BranchID"];
        //            string Reachdate = Reachingdt.ToString("M/dd/yyyy");
        //            string ReachTime = Reachingdt.ToString("HH:mm");
        //            summeryRow["Reaching Date"] = Reachingdt.ToString();
        //            summeryRow["Reaching Time"] = ReachTime.ToString();
        //            presodometer = double.Parse(tripdatarow["Odometer"].ToString());
        //            summeryRow["Distance(Kms)"] = presodometer;
        //            summeryTable.Rows.Add(summeryRow);
        //            break;
        //        }
        //    }
        //    Prevrow = tripdatarow;
        //}
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
    
 
    #region finding Geofence
    public static string getGeofenceStatus(double lat1, double lon1, double lat2, double lon2, double radious)
    {
        double difference = GeoCodeCalc.CalcDistance(lat1, lon1, lat2, lon2);
        difference = difference * 1000;
        if (radious >= difference)
        {
            return "In Side";

        }
        else //if (radious < difference)
        {
            return "Out Side";
        }
    }
    protected void ddl_plant_selectionchanged(object sender, EventArgs e)
    {
        if (ddl_plant.SelectedIndex > 0)
        {
            cmd = new MySqlCommand("SELECT routetable.RouteName, routetable.SNo FROM routetable INNER JOIN tripconfiguration ON routetable.SNo = tripconfiguration.RouteID WHERE (tripconfiguration.PlantName = @PlantNameSno) GROUP BY routetable.SNo");
            cmd.Parameters.Add("@PlantNameSno", ddl_plant.SelectedValue);
            DataTable routetabledata = vdm.SelectQuery(cmd).Tables[0];
            ddl_routes.Items.Clear();
            DataRow newrow = routetabledata.NewRow();
            newrow["SNo"] = 0;
            newrow["RouteName"] = "Select Route";
            routetabledata.Rows.InsertAt(newrow, 0);
            ddl_routes.DataSource = routetabledata;
            ddl_routes.DataTextField = "RouteName";
            ddl_routes.DataValueField = "SNo";
            ddl_routes.DataBind();
            ddl_trip.Items.Clear();
        }
    }
    #endregion
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