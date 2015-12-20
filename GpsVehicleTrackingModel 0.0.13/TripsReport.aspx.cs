using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;

public partial class BillingReport : System.Web.UI.Page
{
    MySqlCommand cmd;
    static VehicleDBMgr vdm;
    static string UserName = "";
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
                //cmd = new MySqlCommand("SELECT sno, TripName, StartTime, EndTime, RouteID, Status, CreationDate, Isrepeat, UserID, PlantName, Kms, extrakms, Chargeperkm FROM tripconfiguration WHERE (UserID = @UserName)");
                //cmd = new MySqlCommand("SELECT routetable.RouteName, tripconfiguration.StartTime, tripconfiguration.EndTime, routetable.SNo, tripconfiguration.Kms,tripconfiguration.extrakms,tripconfiguration.Chargeperkm FROM loginsconfigtable INNER JOIN loginstable ON loginsconfigtable.Refno = loginstable.refno INNER JOIN cabmanagement ON loginsconfigtable.VehicleID = cabmanagement.VehicleID INNER JOIN vehiclemanage ON cabmanagement.PlantName = vehiclemanage.ItemName INNER JOIN tripconfiguration ON vehiclemanage.SNo = tripconfiguration.PlantName INNER JOIN routetable ON tripconfiguration.RouteID = routetable.SNo WHERE (loginstable.loginid = @UserName) GROUP BY routetable.RouteName");
                cmd = new MySqlCommand("SELECT routetable.RouteName, tripconfiguration.StartTime, tripconfiguration.EndTime, routetable.SNo, tripconfiguration.Kms, tripconfiguration.extrakms, tripconfiguration.Chargeperkm FROM tripconfiguration INNER JOIN branchdata ON tripconfiguration.PlantName = branchdata.Sno INNER JOIN cabmanagement ON branchdata.BranchID = cabmanagement.PlantName INNER JOIN loginsconfigtable ON cabmanagement.VehicleID = loginsconfigtable.VehicleID INNER JOIN loginstable ON loginsconfigtable.Refno = loginstable.refno INNER JOIN routetable ON tripconfiguration.RouteID = routetable.SNo WHERE (loginstable.loginid = @UserName) GROUP BY routetable.RouteName");
                cmd.Parameters.Add("@UserName", UserName);
                DataTable routetabledata = vdm.SelectQuery(cmd).Tables[0];
                ddl_routes.Items.Clear();
                DataRow newrow = routetabledata.NewRow();
                newrow["SNo"] = 0;
                newrow["RouteName"] = "Select All";
                routetabledata.Rows.InsertAt(newrow, 0);
                ddl_routes.DataSource = routetabledata;
                ddl_routes.DataTextField = "RouteName";
                ddl_routes.DataValueField = "SNo";
                ddl_routes.DataBind();
                startdate.Text = GetLowDate(DateTime.Now).ToString("dd-MM-yyyy HH:mm");
                enddate.Text = GetHighDate(DateTime.Now).ToString("dd-MM-yyyy HH:mm");
                //cmd = new MySqlCommand("SELECT tripdata.RouteName, tripdata.PlantName, routesubtable.LocationID FROM routesubtable INNER JOIN routetable ON routesubtable.SNo = routetable.SNo INNER JOIN tripdata ON routetable.RouteName = tripdata.RouteName GROUP BY routesubtable.LocationID");
                //DataTable plantsdata = vdm.SelectQuery(cmd).Tables[0];
                //cmd = new MySqlCommand("SELECT Sno,UserName, BranchID, Description, Latitude, Longitude, PhoneNumber, ImagePath, ImageType, Radious, PlantName, IsPlant FROM branchdata WHERE (UserName = 'vyshnavi')");
                //DataTable branchdata = vdm.SelectQuery(cmd).Tables[0];
                //foreach (DataRow dr in plantsdata.Rows)
                //{
                //    DataRow[] datarows = branchdata.Select("BranchID='" + dr["PlantName"].ToString() + "' and UserName='vyshnavi'");
                //    if (datarows.Length > 0)
                //    {
                //        cmd = new MySqlCommand("update branchdata set PlantName='" + datarows[0]["Sno"].ToString() + "' WHERE Sno='" + dr["LocationID"].ToString() + "' and (UserName = 'vyshnavi')");
                //        vdm.Update(cmd);
                //    }
                //}
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
    protected void btn_generate_Click(object sender, EventArgs e)
    {
        try
        {
            if (Session["field1"] == null)
                Response.Redirect("Login.aspx");
            else
                UserName = Session["field1"].ToString();
            lbl_nofifier.Text = "";
            //if (ddl_routes.SelectedIndex == 0)
            //{
            //    lbl_nofifier.Text = "Please select Route";
            //    return;
            //}
            DataTable sampletable = new DataTable();
            grdReports.DataSource = sampletable;
            grdReports.DataBind();
            lbl_nofifier.Text = "";
            if (startdate.Text != "" && enddate.Text != "")
            {
                DateTime fromdate = DateTime.Now;//System.Convert.ToDateTime(startdate.Text);//startdate_CalendarExtender.SelectedDate ?? DateTime.Now;// DateTime.Now.AddMonths(-3);//DateTime.Parse(startdate.Text); ;
                DateTime todate = DateTime.Now;//System.Convert.ToDateTime(enddate.Text);//enddate_CalendarExtender.SelectedDate ?? DateTime.Now; //DateTime.Parse(enddate.Text);
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

                datestrig = enddate.Text.Split(' ');
                if (datestrig.Length > 1)
                {
                    if (datestrig[0].Split('-').Length > 0)
                    {
                        string[] dates = datestrig[0].Split('-');
                        string[] times = datestrig[1].Split(':');
                        todate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                    }
                }
                else
                {
                    // MessageBox.Show("Date Time Format Wrong");
                    lbl_nofifier.Text = "To Date Time Format Wrong";
                    return;
                }
                DataTable finaltable = new DataTable();
                finaltable.Columns.Add("SNo");
                finaltable.Columns.Add("AUTO NO");
                finaltable.Columns.Add("ROUTE NAME");
                finaltable.Columns.Add("AM KMS").DataType=typeof(double);
                finaltable.Columns.Add("PM KMS").DataType = typeof(double);
                finaltable.Columns.Add("TOTAL KMS").DataType = typeof(double);
                finaltable.Columns.Add("RATE PER KM.").DataType = typeof(double);
                finaltable.Columns.Add("TOTAL AMOUNT").DataType = typeof(double);
                if (ddl_routes.SelectedIndex == 0)
                {
                    if (ddl_trip.SelectedValue == "ALL")
                    {
                        cmd = new MySqlCommand("SELECT TripMeridian, RouteName, VehicleNumber, SUM(GPSKMs) AS GPSKMs, SUM(BillingKMs) AS BillingKMs, ChargePerKM, SUM(TotalCharge) AS TotalCharge, Remarks FROM (SELECT distancestatistics.TripMeridian, routetable.RouteName, MIN(distancestatistics.Actual_kms) AS ActualKMS, distancestatistics.GPS_kms AS GPSKMs,MIN(distancestatistics.Billing_kms) AS BillingKMs, MIN(distancestatistics.ChargePerKM) AS ChargePerKM, distancestatistics.TotalCharge,distancestatistics.Remarks, paireddata.VehicleNumber, distancestatistics.G_Date FROM tripconfiguration INNER JOIN routetable ON tripconfiguration.RouteID = routetable.SNo INNER JOIN paireddata ON tripconfiguration.Veh_Sno = paireddata.Sno INNER JOIN distancestatistics ON routetable.SNo = distancestatistics.RouteName_sno WHERE (distancestatistics.UserID = @UserID) AND (distancestatistics.G_Date >= @d1) AND (distancestatistics.G_Date <= @d2) AND (tripconfiguration.PlantName = @PlantName) AND (distancestatistics.TripMeridian = 'AM') GROUP BY distancestatistics.TripMeridian, routetable.RouteName, distancestatistics.Remarks, paireddata.VehicleNumber, distancestatistics.G_Date union all SELECT distancestatistics.TripMeridian, routetable.RouteName, MIN(distancestatistics.Actual_kms) AS ActualKMS, distancestatistics.GPS_kms AS GPSKMs, MIN(distancestatistics.Billing_kms) AS BillingKMs, MIN(distancestatistics.ChargePerKM) AS ChargePerKM, distancestatistics.TotalCharge, distancestatistics.Remarks, paireddata.VehicleNumber, distancestatistics.G_Date FROM tripconfiguration INNER JOIN routetable ON tripconfiguration.RouteID = routetable.SNo INNER JOIN paireddata ON tripconfiguration.Veh_Sno = paireddata.Sno INNER JOIN distancestatistics ON routetable.SNo = distancestatistics.RouteName_sno WHERE (distancestatistics.UserID = @UserID) AND (distancestatistics.G_Date >= @d1) AND (distancestatistics.G_Date <= @d2) AND (tripconfiguration.PlantName = @PlantName) AND (distancestatistics.TripMeridian = 'PM') GROUP BY distancestatistics.TripMeridian, routetable.RouteName, distancestatistics.Remarks, paireddata.VehicleNumber, distancestatistics.G_Date) final GROUP BY TripMeridian, RouteName");
                        cmd.Parameters.Add("@UserID", UserName);
                        cmd.Parameters.Add("@PlantName", ddl_plant.SelectedValue);
                        cmd.Parameters.Add("@RouteName_sno", ddl_routes.SelectedValue);
                        cmd.Parameters.Add("@TripMeridian", ddl_trip.SelectedValue);
                        cmd.Parameters.Add("@d1", fromdate);
                        cmd.Parameters.Add("@d2", todate);
                        DataTable rslt = vdm.SelectQuery(cmd).Tables[0];
                        DataTable uniqueTrips = rslt.DefaultView.ToTable(true, "RouteName");
                        foreach (DataRow dr in uniqueTrips.Rows)
                        {
                            DataRow[] AMPMROWS = rslt.Select("RouteName='" + dr["RouteName"].ToString() + "'");
                            if (AMPMROWS.Length > 0)
                            {
                                DataRow newrow = finaltable.NewRow();
                                newrow["SNo"] = finaltable.Rows.Count + 1;
                                newrow["AUTO NO"] = AMPMROWS[0]["VehicleNumber"].ToString();
                                newrow["ROUTE NAME"] = AMPMROWS[0]["RouteName"].ToString();
                                newrow["RATE PER KM."] = AMPMROWS[0]["ChargePerKM"].ToString();
                                foreach (DataRow ttlrow in AMPMROWS)
                                {
                                    if (ttlrow["TripMeridian"].ToString() == "AM")
                                    {
                                        newrow["AM KMS"] = ttlrow["GPSKMs"].ToString();
                                    }
                                    else if (ttlrow["TripMeridian"].ToString() == "PM")
                                    {
                                        newrow["PM KMS"] = ttlrow["GPSKMs"].ToString();
                                    }
                                }
                                if (AMPMROWS.Length == 2)
                                {
                                    newrow["TOTAL KMS"] = double.Parse(AMPMROWS[0]["GPSKMs"].ToString()) + double.Parse(AMPMROWS[1]["GPSKMs"].ToString());
                                    newrow["TOTAL AMOUNT"] = double.Parse(AMPMROWS[0]["TotalCharge"].ToString()) + double.Parse(AMPMROWS[1]["TotalCharge"].ToString());
                                }
                                else
                                {
                                    newrow["TOTAL KMS"] = double.Parse(AMPMROWS[0]["GPSKMs"].ToString());
                                    newrow["TOTAL AMOUNT"] = double.Parse(AMPMROWS[0]["TotalCharge"].ToString());
                                }
                                finaltable.Rows.Add(newrow);
                            }
                        }
                        DataRow sumrow = finaltable.NewRow();
                        sumrow["ROUTE NAME"] = "TOTAL";
                        sumrow["RATE PER KM."] = finaltable.Compute("sum([RATE PER KM.])", "[RATE PER KM.]<>'0'");
                        sumrow["AM KMS"] = finaltable.Compute("sum([AM KMS])", "[AM KMS]<>'0'");
                        sumrow["PM KMS"] = finaltable.Compute("sum([PM KMS])", "[PM KMS]<>'0'");
                        sumrow["TOTAL KMS"] = finaltable.Compute("sum([TOTAL KMS])", "[TOTAL KMS]<>'0'");
                        sumrow["TOTAL AMOUNT"] = finaltable.Compute("sum([TOTAL AMOUNT])", "[TOTAL AMOUNT]<>'0'");
                        finaltable.Rows.Add(sumrow);
                        grdReports.DataSource = finaltable;
                        grdReports.DataBind();
                        Session["xportdata"] = finaltable;
                        Session["title"] = "Billing Report From " + startdate.Text + " To " + enddate.Text + "";
                    }
                    else
                    {
                        cmd = new MySqlCommand("SELECT TripMeridian, RouteName, VehicleNumber, SUM(GPSKMs) AS GPSKMs, SUM(BillingKMs) AS BillingKMs, ChargePerKM, SUM(TotalCharge) AS TotalCharge, Remarks FROM (SELECT distancestatistics.TripMeridian, routetable.RouteName, MIN(distancestatistics.Actual_kms) AS ActualKMS, distancestatistics.GPS_kms AS GPSKMs,MIN(distancestatistics.Billing_kms) AS BillingKMs, MIN(distancestatistics.ChargePerKM) AS ChargePerKM, distancestatistics.TotalCharge,distancestatistics.Remarks, paireddata.VehicleNumber, distancestatistics.G_Date FROM tripconfiguration INNER JOIN routetable ON tripconfiguration.RouteID = routetable.SNo INNER JOIN paireddata ON tripconfiguration.Veh_Sno = paireddata.Sno INNER JOIN distancestatistics ON routetable.SNo = distancestatistics.RouteName_sno WHERE (distancestatistics.UserID = @UserID) AND (distancestatistics.G_Date >= @d1) AND (distancestatistics.G_Date <= @d2) AND (tripconfiguration.PlantName = @PlantName) AND (distancestatistics.TripMeridian = @TripMeridian) GROUP BY distancestatistics.TripMeridian, routetable.RouteName, distancestatistics.Remarks, paireddata.VehicleNumber, distancestatistics.G_Date) final GROUP BY TripMeridian, RouteName");
                        cmd.Parameters.Add("@UserID", UserName);
                        cmd.Parameters.Add("@PlantName", ddl_plant.SelectedValue);
                        cmd.Parameters.Add("@RouteName_sno", ddl_routes.SelectedValue);
                        cmd.Parameters.Add("@TripMeridian", ddl_trip.SelectedValue);
                        cmd.Parameters.Add("@d1", fromdate);
                        cmd.Parameters.Add("@d2", todate);
                        DataTable rslt = vdm.SelectQuery(cmd).Tables[0];
                        DataTable uniqueTrips = rslt.DefaultView.ToTable(true, "RouteName");
                        foreach (DataRow dr in uniqueTrips.Rows)
                        {
                            DataRow[] AMPMROWS = rslt.Select("RouteName='" + dr["RouteName"].ToString() + "'");
                            if (AMPMROWS.Length > 0)
                            {
                                DataRow newrow = finaltable.NewRow();
                                newrow["SNo"] = finaltable.Rows.Count + 1;
                                newrow["AUTO NO"] = AMPMROWS[0]["VehicleNumber"].ToString();
                                newrow["ROUTE NAME"] = AMPMROWS[0]["RouteName"].ToString();
                                newrow["RATE PER KM."] = AMPMROWS[0]["ChargePerKM"].ToString();
                                foreach (DataRow ttlrow in AMPMROWS)
                                {
                                    if (ttlrow["TripMeridian"].ToString() == "AM")
                                    {
                                        newrow["AM KMS"] = ttlrow["GPSKMs"].ToString();
                                    }
                                    else if (ttlrow["TripMeridian"].ToString() == "PM")
                                    {
                                        newrow["PM KMS"] = ttlrow["GPSKMs"].ToString();
                                    }
                                }
                                if (AMPMROWS.Length == 2)
                                {
                                    newrow["TOTAL KMS"] = double.Parse(AMPMROWS[0]["GPSKMs"].ToString()) + double.Parse(AMPMROWS[1]["GPSKMs"].ToString());
                                    newrow["TOTAL AMOUNT"] = double.Parse(AMPMROWS[0]["TotalCharge"].ToString()) + double.Parse(AMPMROWS[1]["TotalCharge"].ToString());
                                }
                                else
                                {
                                    newrow["TOTAL KMS"] = double.Parse(AMPMROWS[0]["GPSKMs"].ToString());
                                    newrow["TOTAL AMOUNT"] = double.Parse(AMPMROWS[0]["TotalCharge"].ToString());
                                }
                                finaltable.Rows.Add(newrow);
                            }
                        }
                        DataRow sumrow = finaltable.NewRow();
                        sumrow["ROUTE NAME"] = "TOTAL";
                        sumrow["RATE PER KM."] = finaltable.Compute("sum([RATE PER KM.])", "[RATE PER KM.]<>'0'");
                        sumrow["AM KMS"] = finaltable.Compute("sum([AM KMS])", "[AM KMS]<>'0'");
                        sumrow["PM KMS"] = finaltable.Compute("sum([PM KMS])", "[PM KMS]<>'0'");
                        sumrow["TOTAL KMS"] = finaltable.Compute("sum([TOTAL KMS])", "[TOTAL KMS]<>'0'");
                        sumrow["TOTAL AMOUNT"] = finaltable.Compute("sum([TOTAL AMOUNT])", "[TOTAL AMOUNT]<>'0'");
                        finaltable.Rows.Add(sumrow);
                        grdReports.DataSource = finaltable;
                        grdReports.DataBind();
                        Session["xportdata"] = finaltable;
                        Session["title"] = "Billing Report From " + startdate.Text + " To " + enddate.Text + " For " + ddl_trip.SelectedValue + " Trip";
                    }
                }
                else
                {
                    if (ddl_trip.SelectedValue == "ALL")
                    {
                        cmd = new MySqlCommand("SELECT Date_Format(distancestatistics.G_Date,'%m/%d/%Y') AS TripTime,distancestatistics.TripMeridian,routetable.RouteName,  distancestatistics.Actual_kms AS ActualKMs, distancestatistics.GPS_kms AS GPSKMs, distancestatistics.Billing_kms AS BillingKMs, distancestatistics.ChargePerKM, distancestatistics.TotalCharge, distancestatistics.Remarks FROM distancestatistics INNER JOIN routetable ON distancestatistics.RouteName_sno = routetable.SNo WHERE (distancestatistics.RouteName_sno = @RouteName_sno) AND (distancestatistics.UserID = @UserID) AND (distancestatistics.G_Date >= @d1) AND (distancestatistics.G_Date <= @d2) order by distancestatistics.G_Date");
                        cmd.Parameters.Add("@UserID", UserName);
                        cmd.Parameters.Add("@PlantName", ddl_plant.SelectedValue);
                        cmd.Parameters.Add("@RouteName_sno", ddl_routes.SelectedValue);
                        cmd.Parameters.Add("@TripMeridian", ddl_trip.SelectedValue);
                        cmd.Parameters.Add("@d1", fromdate);
                        cmd.Parameters.Add("@d2", todate);
                        DataTable rslt = vdm.SelectQuery(cmd).Tables[0];
                        grdReports.DataSource = rslt;
                        grdReports.DataBind();
                        Session["xportdata"] = rslt;
                        Session["title"] = "Billing Report From " + startdate.Text + " To " + enddate.Text + "";
                    }
                    else
                    {
                        cmd = new MySqlCommand("SELECT Date_Format(distancestatistics.G_Date,'%m/%d/%Y') AS TripTime,distancestatistics.TripMeridian,routetable.RouteName,  distancestatistics.Actual_kms AS ActualKMs, distancestatistics.GPS_kms AS GPSKMs, distancestatistics.Billing_kms AS BillingKMs, distancestatistics.ChargePerKM, distancestatistics.TotalCharge, distancestatistics.Remarks FROM distancestatistics INNER JOIN routetable ON distancestatistics.RouteName_sno = routetable.SNo WHERE (distancestatistics.RouteName_sno = @RouteName_sno) AND (distancestatistics.TripMeridian = @TripMeridian) AND (distancestatistics.UserID = @UserID) AND (distancestatistics.G_Date >= @d1) AND (distancestatistics.G_Date <= @d2) order by distancestatistics.G_Date");
                        cmd.Parameters.Add("@UserID", UserName);
                        cmd.Parameters.Add("@PlantName", ddl_plant.SelectedValue);
                        cmd.Parameters.Add("@RouteName_sno", ddl_routes.SelectedValue);
                        cmd.Parameters.Add("@TripMeridian", ddl_trip.SelectedValue);
                        cmd.Parameters.Add("@d1", fromdate);
                        cmd.Parameters.Add("@d2", todate);
                        DataTable rslt = vdm.SelectQuery(cmd).Tables[0];
                        grdReports.DataSource = rslt;
                        grdReports.DataBind();
                        Session["xportdata"] = rslt;
                        Session["title"] = "Billing Report From " + startdate.Text + " To " + enddate.Text + "";
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lbl_nofifier.Text = ex.Message;
        }
    }
    protected void ddl_plant_selectionchanged(object sender, EventArgs e)
    {
        if (ddl_plant.SelectedIndex > 0)
        {
            vdm = new VehicleDBMgr();
            vdm.InitializeDB();
            cmd = new MySqlCommand("SELECT routetable.RouteName, routetable.SNo FROM routetable INNER JOIN tripconfiguration ON routetable.SNo = tripconfiguration.RouteID WHERE (tripconfiguration.PlantName = @PlantNameSno) GROUP BY routetable.SNo");
            cmd.Parameters.Add("@PlantNameSno", ddl_plant.SelectedValue);
            DataTable routetabledata = vdm.SelectQuery(cmd).Tables[0];
            ddl_routes.Items.Clear();
            DataRow newrow = routetabledata.NewRow();
            newrow["SNo"] = 0;
            newrow["RouteName"] = "Select All";
            routetabledata.Rows.InsertAt(newrow, 0);
            ddl_routes.DataSource = routetabledata;
            ddl_routes.DataTextField = "RouteName";
            ddl_routes.DataValueField = "SNo";
            ddl_routes.DataBind();
        }
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