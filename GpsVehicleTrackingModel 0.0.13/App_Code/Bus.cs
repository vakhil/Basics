namespace Bustop.Hanlders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Script.Serialization;
    using System.Web.SessionState;
    using CookComputing.XmlRpc;
    using System.Data;
    using System.IO;
    using System.Net.Mail;
    using System.Net;
    using System.Text.RegularExpressions;
    using MySql.Data.MySqlClient;
    using System.Globalization;
    using GPSApplication;

    /// <summary>
    /// Summary description for Bus
    /// </summary>
    public class Bus : IHttpHandler, IRequiresSessionState
    {
        public bool IsReusable
        {
            get { return true; }
        }

        public class vehiclesclass
        {
            public string vehicleno { get; set; }
            public string vehiclemake { get; set; }
            public string vehiclemodel { get; set; }
            public string vehicletype { get; set; }
            public string Routename { get; set; }
            public string Model { get; set; }
            public string Make { get; set; }
            public string TripSheetNo { get; set; }
            public string GpsKms { get; set; }
            public string TripDate { get; set; }
            public string Driver { get; set; }
        }
        public class SelectData
        {

        }
        public class vehiclesupdateclass
        {
            public string vehiclenum { get; set; }
            public string latitude { get; set; }
            public string longitude { get; set; }
            public string direction { get; set; }
            public string Speed { get; set; }
            public string Datetime { get; set; }
            public string mainpower { get; set; }
            public string dieselvalue { get; set; }
            public string odometervalue { get; set; }
            public string Ignation { get; set; }
            public string ACStatus { get; set; }
            public string GPSSignal { get; set; }
            public string Geofence { get; set; }
            public string todaymileage { get; set; }
            public double fulltankval { get; set; }
            public string DriverName { get; set; }
            public string CompanyID { get; set; }
            public string MobileNo { get; set; }
            public string RouteName { get; set; }
            public string PlantName { get; set; }
            public string stoppedfor { get; set; }
        }

        public class Groupsclass
        {
            public string vehiclemodeltype { get; set; }
            public string vehicleno { get; set; }
            public string Routename { get; set; }
        }

        //public class odometervalues
        //{
        //    public string odometer { get; set; }
        //}

        public class BranchData
        {
            public string BranchName { get; set; }
            public string latitude { get; set; }
            public string longitude { get; set; }
            public string Decription { get; set; }
            public string Image { get; set; }
            public string isplant { get; set; }
            public string radius { get; set; }
            public string vehno { get; set; }
            public List<string> plants { get; set; }
        }

        public class logsclass
        {
            public string Sno { get; set; }
            public string datetime { get; set; }
            public string vehicleno { get; set; }
            public string vehicletype { get; set; }
            public string latitude { get; set; }
            public string longitude { get; set; }
            public string direction { get; set; }
            public string speed { get; set; }
            public string Status { get; set; }
            public DataTable Reportsdata { get; set; }
            public string odometer { get; set; }
        }

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                string operation = context.Request["op"];
                switch (operation)
                {
                    case "InitilizeVehicles":
                        InitilizeVehicles(context);
                        break;
                    case "LiveUpdate":
                        LiveUpdate(context);
                        break;
                    case "InitilizeGroups":
                        InitilizeGroups(context);
                        break;
                    case "ShowMyLocations":
                        ShowMyLocations(context);
                        break;
                    case "getdata":
                        getdata(context);
                        break;
                    case "getNearestVehicle":
                        getNearestVehicle(context);
                        break;
                    case "InitilizeVehiclesreports":
                        InitilizeVehiclesreports(context);
                        break;
                    case "setauthorizedsession":
                        setauthorizedsession(context);
                        break;
                    case "GetRouteValues":
                        GetRouteValues(context);
                        break;
                    case "get_Routes":
                        get_Routes(context);
                        break;
                    case "get_routes_trips":
                        get_routes_trips(context);
                        break;
                    case "GetRouteNames":
                        GetRouteNames(context);
                        break;
                    case "GetVehicleNos":
                        GetVehicleNos(context);
                        break;
                    case "ddlRouteNameChange":
                        ddlRouteNameChange(context);
                        break;
                    case "BtnTripSaveClick":
                        BtnTripSaveClick(context);
                        break;
                    case "GetTripdata":
                        GetTripdata(context);
                        break;
                    case "GetSubTripdata":
                        GetSubTripdata(context);
                        break;
                    case "BtnGenerateclick":
                        BtnGenerateclick(context);
                        break;
                    case "dataforpoly":
                        dataforpoly(context);
                        break;
                    case "updateroutesAssigntogrid":
                        updateroutesAssigntogrid(context);
                        break;
                    case "remarkssave":
                        remarkssave(context);
                        break;
                    case "btnRoutesDeleteClick":
                        btnRoutesDeleteClick(context);
                        break;
                    case "updatedivselected":
                        updatedivselected(context);
                        break;
                    case "updatedivselectedsales":
                        updatedivselectedsales(context);
                        break;
                    case "plantvehiclesdata":
                        plantvehiclesdata(context);
                        break;
                    case "plantvehiclesdata_gen":
                        plantvehiclesdata_gen(context);
                        break;
                        
                    case "getplantsandtrips":
                        getplantsandtrips(context);
                        break;
                    case "loginauthorisation":
                        loginauthorisation(context);
                        break;
                    case "loaddet_save":
                        loaddet_save(context);
                        break;
                    case "expdet_save":
                        expdet_save(context);
                        break;
                    case "get_loaddet_grid":
                        get_loaddet_grid(context);
                        break;
                    case "loaddet_delete":
                        loaddet_delete(context);
                        break;
                    case "tripdetals_save":
                        tripdetals_save(context);
                        break;
                    case "gettripsheetdata":
                        gettripsheetdata(context);
                        break;
                    case "trip_end":
                        trip_end(context);
                        break;
                    case "ShowMyplants":
                        ShowMyplants(context);
                        break;
                    case "GetNotifications":
                        GetNotifications(context);
                        break;
                    case "getnotificationvehicles":
                        getnotificationvehicles(context);
                        break;
                    case "getvehnotification":
                        getvehnotification(context);
                        break;
                    case "EmployeeManageSaveClick":
                        EmployeeManageSaveClick(context);
                        break;
                    case "UpdateEmployeeManamentDetails":
                        UpdateEmployeeManamentDetails(context);
                        break;
                    case "EmployeeManamentDeleteDetails":
                        EmployeeManamentDeleteDetails(context);
                        break;
                    case "retrieve_alrtname":
                        retrieve_alrtname(context);
                        break;
                    case "rettrieve_alert_data":
                        rettrieve_alert_data(context);
                        break;
                    case "for_alert_delete":
                        for_alert_delete(context);
                        break;
                    case "for_alert_status":
                        for_alert_status(context);
                        break;
                    case "getvehicles":
                        getvehicles(context);
                        break;
                    case "get_assignalerts":
                        get_assignalerts(context);
                        break;
                    case "assignvehper_del":
                        assignvehper_del(context);
                        break;
                    case "getvehicles_assign":
                        getvehicles_assign(context);
                        break;
                    case "Person_details_save":
                        Person_details_save(context);
                        break;
                    case "person_details_delete":
                        person_details_delete(context);
                        break;
                    case "get_persondetails":
                        get_persondetails(context);
                        break;
                    case "Initilizedatafortripsheet":
                        Initilizedatafortripsheet(context);
                        break;
                    case "btnTripSheetSaveClick":
                        btnTripSheetSaveClick(context);
                        break;
                    case "GetVehicleDetails":
                        GetVehicleDetails(context);
                        break;
                    case "GetAssignTripSheets":
                        GetAssignTripSheets(context);
                        break;
                    case "GetBtnViewJobcardclick":
                        GetBtnViewJobcardclick(context);
                        break;
                    case "UpdateJobcardbtnclick":
                        UpdateJobcardbtnclick(context);
                        break;
                    case "getassignedtrips":
                        getassignedtrips(context);
                        break;
                    case "gettripjobcards":
                        gettripjobcards(context);
                        break;
                    case "gettripalldetails":
                        gettripalldetails(context);
                        break;
                    case "btnTripendSaveClick":
                        btnTripendSaveClick(context);
                        break;
						  case "btnTripSheetPrintClick":
                        btnTripSheetPrintClick(context);
                        break;
                    case "GetTripExpenses":
                        GetTripExpenses(context);
                        break;
                    case "MylocationSaveClick":
                        MylocationSaveClick(context);
                        break;
                    case "getgridlocations":
                        getgridlocations(context);
                        break;
                    case "deletelocation":
                        deletelocation(context);
                        break;
                    case "getdata_compare":
                        getdata_compare(context);
                        break;
                    default:
                        var js = new JavaScriptSerializer();
                        var title1 = context.Request.Params[1];
                        Routes obj = js.Deserialize<Routes>(title1);
                        if (obj.op == "btnRoutesSaveClick")
                        {
                            btnRouteAssignSaveclick(context);
                        }
                        if (obj.op == "btnRoutesSalesSaveClick")
                        {
                            btnRouteSalesAssignSaveclick(context);
                        }
                        if (obj.op == "jobcardsaveclick")
                        {
                            jobcardsaveclick(context);
                        }
						  vehicle_routes obj1 = js.Deserialize<vehicle_routes>(title1);
                        if (obj1.op == "Vehicle_alrts_save")
                        {
                            Vehicle_alrts_save(context);
                        }

                        assign_alrts obj2 = js.Deserialize<assign_alrts>(title1);
                        if (obj1.op == "assignalerts_save")
                        {
                            assignalerts_save(context);
                        }

                        reports_mail obj3 = js.Deserialize<reports_mail>(title1);
                        if (obj3.op == "send_reports_mail")
                        {
                            send_reports_mail(context);
                        }
					    if (obj.op == "btnSaveTripsheetLogsClick")
                        {
                            btnSaveTripsheetLogsClick(context);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void deletelocation(HttpContext context)
        {
            try
            {
                vdm = new VehicleDBMgr();
                vdm.InitializeDB();
                string refno = context.Request["refno"].ToString();
                string Userid = context.Session["field1"].ToString();
                cmd = new MySqlCommand("delete from BranchData where Sno=@Sno and UserName= @UserName ");
                cmd.Parameters.Add("@UserName", Userid);
                cmd.Parameters.Add("@Sno", refno);
                vdm.Delete(cmd);
                string msg = "Deleted successfully";
                string response = GetJson(msg);
                context.Response.Write(response);
            }
            catch (Exception ex)
            {
                string response = GetJson(ex.ToString());
                context.Response.Write(response);
            }
        }
        public class reports_mail
        {
            public string op { set; get; }
            public string[] report { get; set; }
        }
        public class getgridlocationscls
        {
            public string UserName { set; get; }
            public string BranchID { set; get; }
            public string Description { set; get; }
            public string Latitude { set; get; }
            public string Longitude { set; get; }
            public string PhoneNumber { set; get; }
            public string Radious { set; get; }
            public string PlantName { set; get; }
            public string IsPlant { set; get; }
            public string Sno { set; get; }
        }
        private void getgridlocations(HttpContext context)
        {
            try
            {
                vdm = new VehicleDBMgr();
                vdm.InitializeDB();
                string Userid = context.Session["field1"].ToString();
                MySqlCommand cmd = new MySqlCommand("SELECT UserName, BranchID, Description, Latitude, Longitude, PhoneNumber, Radious,Sno, PlantName, IsPlant FROM branchdata WHERE (UserName = @UserName)");
                cmd.Parameters.Add("@UserName", Userid);
                DataTable dtlocations = vdm.SelectQuery(cmd).Tables[0];
                List<getgridlocationscls> getgridlocationsclslst = new List<getgridlocationscls>();
                foreach (DataRow dr in dtlocations.Rows)
                {
                    getgridlocationscls getretve_alrtnme = new getgridlocationscls();
                    getretve_alrtnme.UserName = dr["UserName"].ToString();
                    getretve_alrtnme.BranchID = dr["BranchID"].ToString();
                    getretve_alrtnme.Description = dr["Description"].ToString();
                    getretve_alrtnme.Latitude = dr["Latitude"].ToString();
                    getretve_alrtnme.Longitude = dr["Longitude"].ToString();
                    getretve_alrtnme.PhoneNumber = dr["PhoneNumber"].ToString();
                    getretve_alrtnme.Radious = dr["Radious"].ToString();
                    getretve_alrtnme.Sno = dr["Sno"].ToString();
                    getretve_alrtnme.PlantName = dr["PlantName"].ToString();
                    getretve_alrtnme.IsPlant = dr["IsPlant"].ToString();
                    getgridlocationsclslst.Add(getretve_alrtnme);
                }
                if (getgridlocationsclslst != null)
                {
                    string response = GetJson(getgridlocationsclslst);
                    context.Response.Write(response);
                }
            }
            catch (Exception ex)
            {
                string response = GetJson(ex.ToString());
                context.Response.Write(response);
            }
        }
        private void MylocationSaveClick(HttpContext context)
        {
            try
            {
                vdm = new VehicleDBMgr();
                vdm.InitializeDB();
                string Userid = context.Session["field1"].ToString();
                string txtlocacationname = context.Request["txtlocacationname"].ToString();
                string txtDescription = context.Request["txtDescription"].ToString();
                string txtlatitude = context.Request["txtlatitude"].ToString();
                string txtlongitude = context.Request["txtlongitude"].ToString();
                string txtMyLocationRadious = context.Request["txtMyLocationRadious"].ToString();
                string txtplantname = context.Request["txtplantname"].ToString();
                string txtphonenum = context.Request["txtphonenum"].ToString();
                string btnval = context.Request["btnval"].ToString();
                string ckbisplant = context.Request["ckbisplant"].ToString();
                string refno = context.Request["refno"].ToString();

                if (btnval == "Add")
                {
                    cmd = new MySqlCommand("insert into BranchData (UserName,BranchID, Description, Latitude, Longitude, PhoneNumber,Radious,PlantName,IsPlant ) values (@UserName,@BranchID, @Description, @Latitude, @Longitude, @PhoneNumber,@Radious,@PlantName,@IsPlant)");
                    cmd.Parameters.Add("@BranchID", txtlocacationname.Trim());
                    cmd.Parameters.Add("@Description", txtDescription.Trim());
                    cmd.Parameters.Add("@Latitude", txtlatitude.Trim());
                    cmd.Parameters.Add("@Longitude", txtlongitude.Trim());
                    cmd.Parameters.Add("@PhoneNumber", txtphonenum.Trim());
                    cmd.Parameters.Add("@UserName", Userid);
                    cmd.Parameters.Add("@Radious", txtMyLocationRadious.Trim());
                    cmd.Parameters.Add("@PlantName", txtplantname);
                    cmd.Parameters.Add("@IsPlant", ckbisplant);
                    vdm.insert(cmd);
                    string msg = "Successfully Data Added";
                    string response = GetJson(msg);
                    context.Response.Write(response);
                }
                else
                {
                    cmd = new MySqlCommand("update BranchData set BranchID=@BranchID, Description=@Description, Latitude=@Latitude, Longitude=@Longitude, PhoneNumber=@PhoneNumber,Radious=@Radious,PlantName=@PlantName,IsPlant=@IsPlant where  Sno=@Sno and UserName= @UserName ");
                    cmd.Parameters.Add("@BranchID", txtlocacationname.Trim());
                    cmd.Parameters.Add("@Description", txtDescription.Trim());
                    cmd.Parameters.Add("@Latitude", txtlatitude.Trim());
                    cmd.Parameters.Add("@Longitude", txtlongitude.Trim());
                    cmd.Parameters.Add("@PhoneNumber", txtphonenum.Trim());
                    cmd.Parameters.Add("@UserName", Userid);
                    cmd.Parameters.Add("@Radious", txtMyLocationRadious.Trim());
                    cmd.Parameters.Add("@PlantName", txtplantname);
                    cmd.Parameters.Add("@IsPlant", ckbisplant);
                    cmd.Parameters.Add("@Sno", refno);
                    vdm.Update(cmd);
                    string msg = "Successfully Data Modified";
                    string response = GetJson(msg);
                    context.Response.Write(response);
                }
            }
            catch (Exception ex)
            {
                string response = GetJson(ex.ToString());
                context.Response.Write(response);
            }
        }

        private void send_reports_mail(HttpContext context)
        {
            try
            {
                vdm = new VehicleDBMgr();
                vdm.InitializeDB();
                string Userid = context.Session["field3"].ToString();

                var js = new JavaScriptSerializer();
                var title1 = context.Request.Params[1];
                reports_mail obj3 = js.Deserialize<reports_mail>(title1);

                cmd = new MySqlCommand("SELECT sno, pname, emailid, phonenumber, designation, main_user FROM person_details WHERE (Userid = @Userid)");
                cmd.Parameters.Add("@Userid", Userid);
                DataTable alrttbl = vdm.SelectQuery(cmd).Tables[0];
                DataTable dt = (DataTable)context.Session["xportdata"];
                string html = "<table style='border-collapse: collapse;'>";
                //add header row
                html += "<tr>";
                for (int i = 0; i < dt.Columns.Count; i++)
                    html += "<th style='border: 1px solid black;'>" + dt.Columns[i].ColumnName + "</th>";
                html += "</tr>";
                //add rows
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    html += "<tr>";
                    for (int j = 0; j < dt.Columns.Count; j++)
                        html += "<td style='border: 1px solid black;'>" + dt.Rows[i][j].ToString() + "</td>";
                    html += "</tr>";
                }
                html += "</table>";
                if (alrttbl.Rows.Count > 0)
                {
                    foreach (string per in obj3.report)
                    {
                        DataRow[] roe = alrttbl.Select("sno=" + per);
                        foreach (DataRow dr in roe)
                        {
                            string mailid = dr.ItemArray[2].ToString();
                            //  string senderID = "info.asntech@gmail.com";// use sender's email id here..
                            string senderID = "gps.asntech@gmail.com";// use sender's email id here..
                            const string senderPassword = "9849992010"; // sender password here...

                            SmtpClient smtp = new SmtpClient
                            {
                                Host = "smtp.gmail.com", // smtp server address here...
                                Port = 587,
                                EnableSsl = true,
                                DeliveryMethod = SmtpDeliveryMethod.Network,
                                Credentials = new System.Net.NetworkCredential(senderID, senderPassword),
                                Timeout = 30000,
                            };

                            MailMessage message = new MailMessage(senderID, mailid, "GPS Report", html);



                            //message.CC.Add(new MailAddress("sun.bandi@gmail.com"));
                            //message.CC.Add(new MailAddress("ravindra1507@gmail.com"));
                            message.IsBodyHtml = true;
                            smtp.Send(message);
                        }
                    }

                }
                string response = GetJson("OK");
                context.Response.Write(response);
            }
            catch (Exception ex)
            {
                string response = GetJson(ex.ToString());
                context.Response.Write(response);
            }
        }





        public class get_persons
        {
            public string sno { get; set; }
            public string pname { get; set; }
            public string emailid { get; set; }
            public string phonenumber { get; set; }
            public string designation { get; set; }
            public string userid { get; set; }
        }
        private void get_persondetails(HttpContext context)
        {
            try
            {
                vdm = new VehicleDBMgr();
                vdm.InitializeDB();
                string Userid = context.Session["field3"].ToString();
                cmd = new MySqlCommand("SELECT sno, pname, emailid, phonenumber, designation, userid FROM person_details WHERE (userid = @userid)");
                cmd.Parameters.Add("@userid", Userid);
                List<get_persons> alrtnme = new List<get_persons>();
                DataTable alrttble = vdm.SelectQuery(cmd).Tables[0];
                foreach (DataRow dr in alrttble.Rows)
                {
                    get_persons getretve_alrtnme = new get_persons();
                    getretve_alrtnme.sno = dr["sno"].ToString();
                    getretve_alrtnme.pname = dr["pname"].ToString();
                    getretve_alrtnme.emailid = dr["emailid"].ToString();
                    getretve_alrtnme.phonenumber = dr["phonenumber"].ToString();
                    getretve_alrtnme.designation = dr["designation"].ToString();
                    getretve_alrtnme.userid = dr["userid"].ToString();
                    alrtnme.Add(getretve_alrtnme);
                }
                if (alrtnme != null)
                {
                    string response = GetJson(alrtnme);
                    context.Response.Write(response);
                }
            }
            catch (Exception ex)
            {
                string response = GetJson(ex.ToString());
                context.Response.Write(response);
            }
        }

        private void person_details_delete(HttpContext context)
        {
            try
            {
                vdm = new VehicleDBMgr();
                vdm.InitializeDB();
                string sno = context.Request["sno"].ToString();
                cmd = new MySqlCommand("delete from person_details where sno=@sno");
                cmd.Parameters.Add("@sno", sno);
                vdm.Delete(cmd);
                string response = GetJson("Successfully deleted");
                context.Response.Write(response);
            }
            catch (Exception ex)
            {
                string response = GetJson(ex.ToString());
                context.Response.Write(response);
            }
        }

        private void Person_details_save(HttpContext context)
        {

            try
            {
                vdm = new VehicleDBMgr();
                vdm.InitializeDB();
                string name = context.Request["name"].ToString();
                string mobile = context.Request["mobile"].ToString();
                string mail = context.Request["mail"].ToString();
                string designation = context.Request["designation"].ToString();
                string btnval = context.Request["btnval"].ToString();
                string sno = context.Request["sno"].ToString();
                string Userid = context.Session["field3"].ToString();

                if (btnval == "SAVE")
                {
                    cmd = new MySqlCommand("insert into person_details (pname, emailid, phonenumber, designation, userid) values ( @pname, @emailid, @phonenumber, @designation, @userid)");
                    cmd.Parameters.Add("@pname", name);
                    cmd.Parameters.Add("@emailid", mail);
                    cmd.Parameters.Add("@phonenumber", mobile);
                    cmd.Parameters.Add("@designation", designation);
                    cmd.Parameters.Add("@userid", Userid);
                    vdm.insert(cmd);
                    string response = GetJson("Successfully Inserted");
                    context.Response.Write(response);
                }
                else
                {
                    cmd = new MySqlCommand("update person_details set pname=@pname, emailid=@emailid, phonenumber=@phonenumber, designation=@designation, userid=@userid where sno=@sno");
                    cmd.Parameters.Add("@pname", name);
                    cmd.Parameters.Add("@emailid", mail);
                    cmd.Parameters.Add("@phonenumber", mobile);
                    cmd.Parameters.Add("@designation", designation);
                    cmd.Parameters.Add("@userid", Userid);
                    cmd.Parameters.Add("@sno", sno);
                    vdm.Update(cmd);
                    string response = GetJson("Successfully Edited");
                    context.Response.Write(response);
                }
            }
            catch (Exception ex)
            {
                string response = GetJson(ex.ToString());
                context.Response.Write(response);
            }

        }
        private void getvehicles_assign(HttpContext context)
        {
            vdm = new VehicleDBMgr();
            vdm.InitializeDB();
            try
            {
                List<vehicles> Vehlist = new List<vehicles>();
                string Username = context.Session["field3"].ToString();
                cmd = new MySqlCommand("SELECT paireddata.UserID, paireddata.VehicleNumber, paireddata.Sno FROM paireddata INNER JOIN loginsconfigtable ON paireddata.VehicleNumber = loginsconfigtable.VehicleID WHERE (loginsconfigtable.Refno = @UserName) AND (paireddata.Sno NOT IN (SELECT vehicle_Sno FROM alert_assignment_vehicles alert_assignment_vehicles))");
                cmd.Parameters.Add("@UserName", Username);
                DataTable vehicles = vdm.SelectQuery(cmd).Tables[0];
                foreach (DataRow dr in vehicles.Rows)
                {
                    vehicles Branch = new vehicles();
                    Branch.VehicleNumber = dr["VehicleNumber"].ToString();
                    Branch.Sno = dr["Sno"].ToString();
                    Vehlist.Add(Branch);
                }
                if (Vehlist != null)
                {
                    string respnceString = GetJson(Vehlist);
                    context.Response.Write(respnceString);
                }
            }
            catch (Exception ex)
            {
                string response = GetJson("Error");
                context.Response.Write(response);
            }
        }
        private void assignvehper_del(HttpContext context)
        {

            string sno = context.Request["sno"].ToString();
            try
            {
                vdm = new VehicleDBMgr();
                vdm.InitializeDB();
                cmd = new MySqlCommand("delete from alertassignment where sno=@sno;delete from alert_assignment_vehicles where alert_ass_sno=@sno;delete from alert_assignment_persons where alert_ass_sno=@sno;");
                cmd.Parameters.Add("@sno", sno);
                vdm.Delete(cmd);
                string response = GetJson("Assigned Alert Deleted Successfully");
                context.Response.Write(response);
            }
            catch (Exception ex)
            {
                string response = GetJson(ex.ToString());
                context.Response.Write(response);
            }
        }
        public class ret_assining
        {
            public string alert_name { get; set; }
            public string alertid { get; set; }
            public string sno { get; set; }
            public string alertassignmentName { get; set; }
            //public List<ass_veh> vehicle = new List<ass_veh>();
            //public List<ass_per> person = new List<ass_per>();
            public string vehicle { get; set; }
            public string person { get; set; }
            public string vehicle_sno { get; set; }
            public string person_sno { get; set; }
        }
        public class ass_veh
        {
            public string vehicle_Sno { get; set; }
            public string VehicleNumber { get; set; }
        }
        public class ass_per
        {
            public string persons_sno { get; set; }
            public string pname { get; set; }
        }

        private void get_assignalerts(HttpContext context)
        {
            try
            {
                vdm = new VehicleDBMgr();
                vdm.InitializeDB();
                string Userid = context.Session["field3"].ToString();
                cmd = new MySqlCommand("SELECT alert_manager.alert_name, alert_assignment_vehicles.vehicle_Sno, alert_assignment_persons.persons_sno, paireddata.VehicleNumber, person_details.pname, alertassignment.sno,alertassignment.alertid,alertassignment.alertassignmentName FROM alertassignment INNER JOIN alert_manager ON alertassignment.alertid = alert_manager.sno INNER JOIN alert_assignment_persons ON alertassignment.sno = alert_assignment_persons.alert_ass_sno INNER JOIN alert_assignment_vehicles ON alertassignment.sno = alert_assignment_vehicles.alert_ass_sno INNER JOIN paireddata ON alert_assignment_vehicles.vehicle_Sno = paireddata.Sno INNER JOIN person_details ON alert_assignment_persons.persons_sno = person_details.sno WHERE (alert_manager.userid = @userid)");
                cmd.Parameters.Add("@userid", Userid);
                // cmd = new MySqlCommand("SELECT paireddata.UserID, paireddata.VehicleNumber, paireddata.Sno FROM paireddata INNER JOIN loginsconfigtable ON paireddata.VehicleNumber = loginsconfigtable.VehicleID WHERE (loginsconfigtable.Refno = @UserName)");
                DataTable asstable = vdm.SelectQuery(cmd).Tables[0];
                DataTable defaults = asstable.DefaultView.ToTable(true, "alert_name", "alertid", "sno", "alertassignmentName");
                DataTable vehicles = asstable.DefaultView.ToTable(true, "vehicle_Sno", "VehicleNumber", "sno");
                DataTable persons = asstable.DefaultView.ToTable(true, "persons_sno", "pname", "sno");
                List<ret_assining> assdata = new List<ret_assining>();
                DataRow[] data;
                DataRow[] data1;

                foreach (DataRow dr in defaults.Rows)
                {
                    string vehsno = "";
                    string vehcls = "";
                    string persno = "";
                    string persond = "";
                    ret_assining getassdata = new ret_assining();
                    getassdata.alert_name = dr["alert_name"].ToString();
                    getassdata.alertid = dr["alertid"].ToString();
                    getassdata.sno = dr["sno"].ToString();
                    getassdata.alertassignmentName = dr["alertassignmentName"].ToString();

                    data = vehicles.Select("sno='" + dr["sno"].ToString() + "'");
                    data1 = persons.Select("sno='" + dr["sno"].ToString() + "'");

                    foreach (DataRow dr1 in data)
                    {
                        vehsno += dr1["vehicle_Sno"].ToString() + ",";
                        vehcls += dr1["VehicleNumber"].ToString() + "->";
                    }
                    foreach (DataRow dr2 in data1)
                    {
                        persno += dr2["persons_sno"].ToString() + ",";
                        persond += dr2["pname"].ToString() + "->";
                    }
                    vehcls = vehcls.Substring(0, vehcls.Length - 2);
                    persond = persond.Substring(0, persond.Length - 2);
                    vehsno = vehsno.Substring(0, vehsno.Length - 1);
                    persno = persno.Substring(0, persno.Length - 1);
                    getassdata.vehicle = vehcls;
                    getassdata.person = persond;
                    getassdata.vehicle_sno = vehsno;
                    getassdata.person_sno = persno;
                    assdata.Add(getassdata);
                }
                string response = GetJson(assdata);
                context.Response.Write(response);
            }

            catch (Exception ex)
            {
                string response = GetJson(ex.ToString());
                context.Response.Write(response);
            }

        }
        public class assign_alrts
        {
            public string assgnnme { get; set; }
            public string alrtgrp { get; set; }
            public List<string> vehicleids { get; set; }
            public List<string> personids { get; set; }
            public string btnval { get; set; }
            public string sno { get; set; }
            public string persno { get; set; }
            public string vehsno { get; set; }
        }
        private void assignalerts_save(HttpContext context)
        {
            try
            {
                vdm = new VehicleDBMgr();
                vdm.InitializeDB();
                var js = new JavaScriptSerializer();
                string Username = context.Session["field1"].ToString();
                var title1 = context.Request.Params[1];
                assign_alrts obj2 = js.Deserialize<assign_alrts>(title1);
                string Userid = context.Session["field3"].ToString();
                if (obj2.btnval == "SAVE")
                {
                    cmd = new MySqlCommand("insert into alertassignment (alertid, alertassignmentName) values (@alertid, @alertassignmentName)");
                    cmd.Parameters.Add("@alertid", obj2.alrtgrp);
                    cmd.Parameters.Add("@alertassignmentName", obj2.assgnnme);
                    long asssno = vdm.insertScalar(cmd);
                    foreach (var pers in obj2.personids)
                    {
                        if (pers != "" && pers != null)
                        {
                            cmd = new MySqlCommand("insert into alert_assignment_persons (alert_ass_sno, persons_sno) values (@alert_ass_sno, @persons_sno)");
                            cmd.Parameters.Add("@alert_ass_sno", asssno);
                            cmd.Parameters.Add("@persons_sno", pers);
                            vdm.insert(cmd);
                        }
                    }
                    foreach (var vehi in obj2.vehicleids)
                    {
                        if (vehi != "" && vehi != null)
                        {
                            cmd = new MySqlCommand("insert into alert_assignment_vehicles (alert_ass_sno, vehicle_Sno) values (@alert_ass_sno, @vehicle_Sno)");
                            cmd.Parameters.Add("@alert_ass_sno", asssno);
                            cmd.Parameters.Add("@vehicle_Sno", vehi);
                            vdm.insert(cmd);
                        }
                    }
                    string response = GetJson("Alert Assigned Successfully");
                    context.Response.Write(response);
                }

                else
                {
                    string persno = obj2.persno;
                    string vehsno = obj2.vehsno;
                    string sno = obj2.sno;


                    cmd = new MySqlCommand("delete from alert_assignment_vehicles where alert_ass_sno=@sno;delete from alert_assignment_persons where alert_ass_sno=@sno;");
                    cmd.Parameters.Add("@sno", sno);
                    vdm.Delete(cmd);
                    cmd = new MySqlCommand("update alertassignment set alertid=@alertid, alertassignmentName=@alertassignmentName where sno=@sno");
                    cmd.Parameters.Add("@alertid", obj2.alrtgrp);
                    cmd.Parameters.Add("@alertassignmentName", obj2.assgnnme);
                    cmd.Parameters.Add("@sno", obj2.sno);
                    vdm.Update(cmd);
                    long asssno = Convert.ToInt64(obj2.sno);
                    foreach (var pers in obj2.personids)
                    {
                        if (pers != "" && pers != null)
                        {
                            cmd = new MySqlCommand("insert into alert_assignment_persons (alert_ass_sno, persons_sno) values (@alert_ass_sno, @persons_sno)");
                            cmd.Parameters.Add("@alert_ass_sno", asssno);
                            cmd.Parameters.Add("@persons_sno", pers);
                            vdm.insert(cmd);
                        }
                    }
                    foreach (var vehi in obj2.vehicleids)
                    {
                        if (vehi != "" && vehi != null)
                        {
                            cmd = new MySqlCommand("insert into alert_assignment_vehicles (alert_ass_sno, vehicle_Sno) values (@alert_ass_sno, @vehicle_Sno)");
                            cmd.Parameters.Add("@alert_ass_sno", asssno);
                            cmd.Parameters.Add("@vehicle_Sno", vehi);
                            vdm.insert(cmd);
                        }
                    }
                    string response = GetJson("Alert Assign Edited Successfully");
                    context.Response.Write(response);
                }

            }
            catch (Exception ex)
            {
                string response = GetJson(ex.ToString());
                context.Response.Write(response);
            }
        }

        public class vehicles
        {
            public string VehicleNumber { get; set; }
            public string Sno { get; set; }
        }
        private void getvehicles(HttpContext context)
        {
            vdm = new VehicleDBMgr();
            vdm.InitializeDB();
            try
            {
                List<vehicles> Vehlist = new List<vehicles>();
                string Username = context.Session["field3"].ToString();
                cmd = new MySqlCommand("SELECT paireddata.UserID, paireddata.VehicleNumber, paireddata.Sno FROM paireddata INNER JOIN loginsconfigtable ON paireddata.VehicleNumber = loginsconfigtable.VehicleID WHERE (loginsconfigtable.Refno = @UserName)");
                cmd.Parameters.Add("@UserName", Username);
                DataTable vehicles = vdm.SelectQuery(cmd).Tables[0];
                foreach (DataRow dr in vehicles.Rows)
                {
                    vehicles Branch = new vehicles();
                    Branch.VehicleNumber = dr["VehicleNumber"].ToString();
                    Branch.Sno = dr["Sno"].ToString();
                    Vehlist.Add(Branch);
                }
                if (Vehlist != null)
                {
                    string respnceString = GetJson(Vehlist);
                    context.Response.Write(respnceString);
                }
            }
            catch (Exception ex)
            {
                string response = GetJson("Error");
                context.Response.Write(response);
            }
        }

        private void for_alert_status(HttpContext context)
        {
            try
            {
                vdm = new VehicleDBMgr();
                vdm.InitializeDB();
                string status = context.Request["status"].ToString();
                string sno = context.Request["sno"].ToString();
                if (status == "Enable")
                {
                    cmd = new MySqlCommand("update alert_manager set status='0' where sno=@sno ");
                    cmd.Parameters.Add("@sno", sno);
                    vdm.Update(cmd);
                }
                else
                {
                    cmd = new MySqlCommand("update alert_manager set status='1' where sno=@sno");
                    cmd.Parameters.Add("@sno", sno);
                    vdm.Update(cmd);
                }
                string response = GetJson("Status Updated Successfully");
                context.Response.Write(response);
            }
            catch (Exception ex)
            {
                string response = GetJson("Error");
                context.Response.Write(response);
            }
        }

        private void for_alert_delete(HttpContext context)
        {
            try
            {
                vdm = new VehicleDBMgr();
                vdm.InitializeDB();
                string subsno = context.Request["subs"].ToString();
                string sno = context.Request["sno"].ToString();
                string[] subsnoval = subsno.Split(',');
                string collofsnos = "";
                foreach (string subval in subsnoval)
                {
                    collofsnos += "alert_mgr_sinfo_sno =" + subval + " or ";
                }
                if (collofsnos.Length > 0)
                    collofsnos = collofsnos.Substring(0, collofsnos.LastIndexOf("or"));

                if (collofsnos != "")
                {
                    cmd = new MySqlCommand("delete from alert_manager where sno=@sno;delete from alert_mgr_subinfo where alert_mgr_sno=@sno;delete from alert_mgr_loc_info where " + collofsnos + ";");
                }
                else
                {
                    cmd = new MySqlCommand("delete from alert_manager where sno=@sno;delete from alert_mgr_subinfo where alert_mgr_sno=@sno;");
                }
                cmd.Parameters.Add("@sno", sno);
                vdm.Delete(cmd);
                string response = GetJson("Successfully Deleted");
                context.Response.Write(response);
            }
            catch (Exception ex)
            {
                string response = GetJson("Error");
                context.Response.Write(response);
            }
        }
        public class retrieve_alrtnme
        {
            public string alert_nam { get; set; }
            public string status { get; set; }
            public string timegap { get; set; }
            public string nooftimes { get; set; }
            public string email { get; set; }
            public string mobile { get; set; }
            //public string alert_type { get; set; }
            // public string sub_type { get; set; }
            //public List<string> sub_type { get; set; }
            //public List<string> outpoi { get; set; }
            public List<alrttype> alert_type = new List<alrttype>();


            public string sno { get; set; }
            public List<sub_sno> subsno = new List<sub_sno>();

        }
        public class alrttype
        {
            public string alert_type { get; set; }
            public string sub_type { get; set; }
            public string value { get; set; }
            public List<locations> loc_id = new List<locations>();
            //public string timegap { get; set; }
        }
        public class locations
        {
            public string loc_id { get; set; }
            public string loc_name { get; set; }
            public string locval { get; set; }
        }
        public class sub_sno
        {
            public string subsno { get; set; }

        }

        private void rettrieve_alert_data(HttpContext context)
        {
            try
            {
                vdm = new VehicleDBMgr();
                vdm.InitializeDB();
                List<retrieve_alrtnme> getalerts = new List<retrieve_alrtnme>();
                string alertname = context.Request["alertname"].ToString();
                string alertsno = context.Request["alertsno"].ToString();
                cmd = new MySqlCommand("SELECT alert_manager.alert_name, alert_manager.sno, alert_manager.status, alert_manager.timegap, alert_manager.nooftimes, alert_manager.email, alert_manager.mobile, alert_mgr_subinfo.alert_type, alert_mgr_subinfo.sub_type, alert_mgr_subinfo.value, alert_mgr_loc_info.loc_id, alert_mgr_loc_info.value AS locval, branchdata.BranchID, alert_mgr_subinfo.sno AS subsno FROM branchdata INNER JOIN alert_mgr_loc_info ON branchdata.Sno = alert_mgr_loc_info.loc_id RIGHT OUTER JOIN alert_manager INNER JOIN alert_mgr_subinfo ON alert_manager.sno = alert_mgr_subinfo.alert_mgr_sno ON alert_mgr_loc_info.alert_mgr_sinfo_sno = alert_mgr_subinfo.sno WHERE (alert_manager.sno = @alertsno)");
                cmd.Parameters.Add("@alertsno", alertsno);
                DataTable griddata = vdm.SelectQuery(cmd).Tables[0];
                DataTable alert_val = griddata.DefaultView.ToTable(true, "alert_name", "status", "timegap", "nooftimes", "email", "mobile", "sno");
                DataTable alert_type = griddata.DefaultView.ToTable(true, "alert_type", "sub_type", "value");
                DataTable subsno = griddata.DefaultView.ToTable(true, "subsno");
                retrieve_alrtnme retalrt = new retrieve_alrtnme();
                foreach (DataRow dr in alert_val.Rows)
                {
                    retalrt.alert_nam = dr["alert_name"].ToString();
                    retalrt.status = dr["status"].ToString();
                    retalrt.timegap = dr["timegap"].ToString();
                    retalrt.nooftimes = dr["nooftimes"].ToString();
                    retalrt.email = dr["email"].ToString();
                    retalrt.mobile = dr["mobile"].ToString();
                    retalrt.sno = dr["sno"].ToString();
                    getalerts.Add(retalrt);
                }

                foreach (DataRow dr in alert_type.Rows)
                {
                    alrttype alrttpe = new alrttype();
                    alrttpe.alert_type = dr["alert_type"].ToString();
                    alrttpe.sub_type = dr["sub_type"].ToString();
                    alrttpe.value = dr["value"].ToString();
                    DataRow[] data;
                    if (dr["sub_type"].ToString() != "" && dr["alert_type"].ToString() == "stoppage")
                    {
                        data = griddata.Select("sub_type='" + dr["sub_type"].ToString() + "'");
                    }
                    else
                    {
                        data = griddata.Select("alert_type='" + dr["alert_type"].ToString() + "'");
                    }
                    foreach (DataRow dr1 in data)
                    {
                        if (dr1["loc_id"].ToString() != "")
                        {
                            locations loc = new locations();
                            loc.loc_id = dr1["loc_id"].ToString();
                            loc.loc_name = dr1["BranchID"].ToString();
                            loc.locval = dr1["locval"].ToString();
                            alrttpe.loc_id.Add(loc);
                        }
                    }
                    retalrt.alert_type.Add(alrttpe);
                }
                foreach (DataRow dr3 in subsno.Rows)
                {
                    sub_sno alrttpe2 = new sub_sno();
                    alrttpe2.subsno = dr3["subsno"].ToString();
                    retalrt.subsno.Add(alrttpe2);
                }

                string response = GetJson(getalerts);
                context.Response.Write(response);

            }
            catch (Exception ex)
            {
                string response = GetJson("Error");
                context.Response.Write(response);
            }
        }
        public class retve_alrtnme
        {
            public string alert_name { get; set; }
            public string alert_sno { get; set; }
        }
        private void retrieve_alrtname(HttpContext context)
        {
            try
            {
                vdm = new VehicleDBMgr();
                vdm.InitializeDB();
                string Userid = context.Session["field3"].ToString();
                cmd = new MySqlCommand("SELECT alert_name, sno FROM alert_manager where userid=@userid");
                cmd.Parameters.Add("@userid", Userid);
                List<retve_alrtnme> alrtnme = new List<retve_alrtnme>();
                DataTable alrttble = vdm.SelectQuery(cmd).Tables[0];
                foreach (DataRow dr in alrttble.Rows)
                {
                    retve_alrtnme getretve_alrtnme = new retve_alrtnme();
                    getretve_alrtnme.alert_name = dr["alert_name"].ToString();
                    getretve_alrtnme.alert_sno = dr["sno"].ToString();
                    alrtnme.Add(getretve_alrtnme);
                }
                if (alrtnme != null)
                {
                    string response = GetJson(alrtnme);
                    context.Response.Write(response);
                }
            }
            catch (Exception ex)
            {
                string response = GetJson("Error");
                context.Response.Write(response);
            }

        }


        class tablelocs1
        {
            public string maxtime { set; get; }
            // public string loc_logs { set; get; }
            public List<string> loc_logs { get; set; }

        }

        class alerttype1
        {
            public string speedchk { set; get; }
            public string inpoichk { set; get; }
            public string outpoichk { set; get; }
            public string stoppagechk { set; get; }
            public string stpinpoichk { set; get; }
            public string stpoutpoichk { set; get; }
            public string mainpowerchk { set; get; }
        }
        class vehicle_routes
        {
            public string op { set; get; }
            public string alertname { set; get; }
            public List<alerttype1> alerttype = new List<alerttype1>();

            //public string alerttype { set; get; }
            public string alrtrept { set; get; }
            public string timegap { set; get; }
            public string maxspeed { set; get; }
            public List<string> inpoi { get; set; }
            public List<string> outpoi { get; set; }

            //public string inpoi { set; get; }
            //public string outpoi { set; get; }
            //public string tablelocs { set; get; }
            public List<tablelocs1> tablelocs = new List<tablelocs1>();

            public string stpmaxstptme { set; get; }
            public string btnval { set; get; }
            public string mail { set; get; }
            public string mobile { set; get; }
            public string sno { set; get; }
            public string subsno { set; get; }
            public List<string> noalert { get; set; }

        }


        public void vehicel_alers_fun(HttpContext context)
        {
            try
            {
                vdm = new VehicleDBMgr();
                vdm.InitializeDB();
                var js = new JavaScriptSerializer();
                string Username = context.Session["field1"].ToString();
                var title1 = context.Request.Params[1];
                vehicle_routes obj2 = js.Deserialize<vehicle_routes>(title1);
                string Userid = context.Session["field3"].ToString();

                cmd = new MySqlCommand("insert into  alert_manager (alert_name, status, userid,timegap, nooftimes, email, mobile) values (@alert_name, @status, @userid,@timegap, @nooftimes, @email, @mobile)");
                cmd.Parameters.Add("@alert_name", obj2.alertname);
                cmd.Parameters.Add("@userid", Userid);
                cmd.Parameters.Add("@status", '1');
                cmd.Parameters.Add("@timegap", obj2.timegap);
                cmd.Parameters.Add("@nooftimes", obj2.alrtrept);
                cmd.Parameters.Add("@email", obj2.mail);
                cmd.Parameters.Add("@mobile", obj2.mobile);
                long Sno = vdm.insertScalar(cmd);
                foreach (alerttype1 alrt in obj2.alerttype)
                {

                    if (alrt.mainpowerchk == "mainpower")
                    {
                        cmd = new MySqlCommand("Insert into alert_mgr_subinfo (alert_mgr_sno, alert_type, sub_type) values (@alert_mgr_sno, @alert_type, @sub_type)");
                        cmd.Parameters.Add("@alert_mgr_sno", Sno);
                        cmd.Parameters.Add("@alert_type", alrt.mainpowerchk);
                        cmd.Parameters.Add("@sub_type", "");

                        vdm.insert(cmd);
                    }

                    if (alrt.speedchk == "speed")
                    {
                        cmd = new MySqlCommand("Insert into alert_mgr_subinfo (alert_mgr_sno, alert_type, sub_type, value) values (@alert_mgr_sno, @alert_type, @sub_type, @value)");
                        cmd.Parameters.Add("@alert_mgr_sno", Sno);
                        cmd.Parameters.Add("@alert_type", alrt.speedchk);
                        cmd.Parameters.Add("@sub_type", "");
                        if (obj2.maxspeed != "")
                        {
                            cmd.Parameters.Add("@value", obj2.maxspeed);
                        }
                        else
                        {
                            cmd.Parameters.Add("@value", 0);
                        }
                        vdm.insert(cmd);
                    }
                    if (alrt.inpoichk == "inpoi")
                    {
                        cmd = new MySqlCommand("Insert into alert_mgr_subinfo (alert_mgr_sno, alert_type, sub_type, value) values (@alert_mgr_sno, @alert_type, @sub_type, @value)");
                        cmd.Parameters.Add("@alert_mgr_sno", Sno);
                        cmd.Parameters.Add("@alert_type", alrt.inpoichk);
                        cmd.Parameters.Add("@sub_type", "");
                        cmd.Parameters.Add("@value", 0);
                        long inpoisno = vdm.insertScalar(cmd);
                        //string inpoilocsstr = obj2.inpoi;
                        //string[] delimiters = new string[] { "<br>" };
                        //string[] inpoilocs = inpoilocsstr.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

                        foreach (var locs in obj2.inpoi)
                        {
                            if (locs != "" && locs != null)
                            {
                                cmd = new MySqlCommand("insert into alert_mgr_loc_info (alert_mgr_sinfo_sno, loc_id) values (@alert_mgr_sinfo_sno, @loc_id)");
                                cmd.Parameters.Add("@alert_mgr_sinfo_sno", inpoisno);
                                cmd.Parameters.Add("@loc_id", locs);
                                vdm.insert(cmd);
                            }
                        }
                    }
                    if (alrt.outpoichk == "outpoi")
                    {
                        cmd = new MySqlCommand("Insert into alert_mgr_subinfo (alert_mgr_sno, alert_type, sub_type, value) values (@alert_mgr_sno, @alert_type, @sub_type, @value)");
                        cmd.Parameters.Add("@alert_mgr_sno", Sno);
                        cmd.Parameters.Add("@alert_type", alrt.outpoichk);
                        cmd.Parameters.Add("@sub_type", "");
                        cmd.Parameters.Add("@value", 0);
                        long outpoisno = vdm.insertScalar(cmd);
                        //string inpoilocsstr = obj2.outpoi;
                        //string[] delimiters = new string[] { "<br>" };
                        //string[] outpoilocs = inpoilocsstr.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

                        foreach (var locs in obj2.outpoi)
                        {
                            if (locs != "" && locs != null)
                            {
                                cmd = new MySqlCommand("insert into alert_mgr_loc_info (alert_mgr_sinfo_sno, loc_id) values (@alert_mgr_sinfo_sno, @loc_id)");
                                cmd.Parameters.Add("@alert_mgr_sinfo_sno", outpoisno);
                                cmd.Parameters.Add("@loc_id", locs);
                                vdm.insert(cmd);
                            }
                        }
                    }
                    if (alrt.stoppagechk == "stoppage")
                    {
                        if (alrt.stpinpoichk == "stopinpoi")
                        {
                            cmd = new MySqlCommand("Insert into alert_mgr_subinfo (alert_mgr_sno, alert_type, sub_type, value) values (@alert_mgr_sno, @alert_type, @sub_type, @value)");
                            cmd.Parameters.Add("@alert_mgr_sno", Sno);
                            cmd.Parameters.Add("@alert_type", alrt.stoppagechk);
                            cmd.Parameters.Add("@sub_type", "inpoi");
                            cmd.Parameters.Add("@value", 0);
                            long stopinpoisno = vdm.insertScalar(cmd);


                            foreach (tablelocs1 tbl in obj2.tablelocs)
                            {
                                //string stpinpoilocsstr = "";
                                //stpinpoilocsstr = tbl.loc_logs;
                                //string[] delimiters = new string[] { "<br>" };
                                //string[] stpinpoilocsstrlocs = stpinpoilocsstr.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                                foreach (var locs in tbl.loc_logs)
                                {
                                    if (locs != "" && locs != null)
                                    {
                                        cmd = new MySqlCommand("insert into alert_mgr_loc_info (alert_mgr_sinfo_sno, loc_id, value) values (@alert_mgr_sinfo_sno, @loc_id,@value)");
                                        cmd.Parameters.Add("@alert_mgr_sinfo_sno", stopinpoisno);
                                        cmd.Parameters.Add("@loc_id", locs);
                                        cmd.Parameters.Add("@value", tbl.maxtime);
                                        vdm.insert(cmd);
                                    }
                                }
                            }


                        }
                        if (alrt.stpoutpoichk == "stopoutpoi")
                        {
                            cmd = new MySqlCommand("Insert into alert_mgr_subinfo (alert_mgr_sno, alert_type, sub_type, value) values (@alert_mgr_sno, @alert_type, @sub_type, @value)");
                            cmd.Parameters.Add("@alert_mgr_sno", Sno);
                            cmd.Parameters.Add("@alert_type", alrt.stoppagechk);
                            cmd.Parameters.Add("@sub_type", "outpoi");
                            if (obj2.stpmaxstptme != "")
                            {
                                cmd.Parameters.Add("@value", obj2.stpmaxstptme);
                            }
                            else
                            {
                                cmd.Parameters.Add("@value", 0);

                            }
                            long stopoutpoisno = vdm.insertScalar(cmd);


                            foreach (var locs in obj2.noalert)
                            {
                                if (locs != "" && locs != null)
                                {
                                    cmd = new MySqlCommand("insert into alert_mgr_loc_info (alert_mgr_sinfo_sno, loc_id) values (@alert_mgr_sinfo_sno, @loc_id)");
                                    cmd.Parameters.Add("@alert_mgr_sinfo_sno", stopoutpoisno);
                                    cmd.Parameters.Add("@loc_id", locs);
                                    vdm.insert(cmd);
                                }
                            }


                        }

                    }

                }


                string response = GetJson("Successfully Saved");
                context.Response.Write(response);


            }
            catch (Exception ex)
            {
                string response = GetJson("Error");
                context.Response.Write(response);
            }
        }

        private void Vehicle_alrts_save(HttpContext context)
        {
            try
            {
                vdm = new VehicleDBMgr();
                vdm.InitializeDB();
                var js = new JavaScriptSerializer();
                string Username = context.Session["field1"].ToString();
                var title1 = context.Request.Params[1];
                vehicle_routes obj2 = js.Deserialize<vehicle_routes>(title1);
                string Userid = context.Session["field3"].ToString();

                if (obj2.btnval == "SAVE")
                {
                    vehicel_alers_fun(context);
                }

                else
                {
                    string[] subsnoval = obj2.subsno.Split(',');
                    string collofsnos = "";
                    foreach (string subval in subsnoval)
                    {
                        collofsnos += "alert_mgr_sinfo_sno =" + subval + " or ";
                    }
                    if (collofsnos.Length > 0)
                        collofsnos = collofsnos.Substring(0, collofsnos.LastIndexOf("or"));

                    if (collofsnos != "")
                    {
                        cmd = new MySqlCommand("delete from alert_mgr_subinfo where alert_mgr_sno=@sno;delete from alert_mgr_loc_info where " + collofsnos + ";");
                    }
                    else
                    {
                        cmd = new MySqlCommand("delete from alert_mgr_subinfo where alert_mgr_sno=@sno;");
                    }
                    cmd.Parameters.Add("@sno", obj2.sno);
                    vdm.Delete(cmd);
                    //vehicel_alers_fun(context);


                    cmd = new MySqlCommand("update alert_manager set alert_name=@alert_name, status=@status, userid=@userid,timegap=@timegap, nooftimes=@nooftimes, email=@email, mobile=@mobile where sno=@sno ");
                    cmd.Parameters.Add("@alert_name", obj2.alertname);
                    cmd.Parameters.Add("@userid", Userid);
                    cmd.Parameters.Add("@status", '1');
                    cmd.Parameters.Add("@timegap", obj2.timegap);
                    cmd.Parameters.Add("@nooftimes", obj2.alrtrept);
                    cmd.Parameters.Add("@email", obj2.mail);
                    cmd.Parameters.Add("@mobile", obj2.mobile);
                    cmd.Parameters.Add("@sno", obj2.sno);
                    vdm.Update(cmd);

                    long Sno = Convert.ToInt64(obj2.sno);

                    foreach (alerttype1 alrt in obj2.alerttype)
                    {

                        if (alrt.mainpowerchk == "mainpower")
                        {
                            cmd = new MySqlCommand("Insert into alert_mgr_subinfo (alert_mgr_sno, alert_type, sub_type, value) values (@alert_mgr_sno, @alert_type, @sub_type, @value)");
                            cmd.Parameters.Add("@alert_mgr_sno", Sno);
                            cmd.Parameters.Add("@alert_type", alrt.mainpowerchk);
                            cmd.Parameters.Add("@sub_type", "");
                            
                            vdm.insert(cmd);
                        }

                        if (alrt.speedchk == "speed")
                        {
                            cmd = new MySqlCommand("Insert into alert_mgr_subinfo (alert_mgr_sno, alert_type, sub_type, value) values (@alert_mgr_sno, @alert_type, @sub_type, @value)");
                            cmd.Parameters.Add("@alert_mgr_sno", Sno);
                            cmd.Parameters.Add("@alert_type", alrt.speedchk);
                            cmd.Parameters.Add("@sub_type", "");
                            if (obj2.maxspeed != "")
                            {
                                cmd.Parameters.Add("@value", obj2.maxspeed);
                            }
                            else
                            {
                                cmd.Parameters.Add("@value", 0);
                            }
                            vdm.insert(cmd);
                        }
                        if (alrt.inpoichk == "inpoi")
                        {
                            cmd = new MySqlCommand("Insert into alert_mgr_subinfo (alert_mgr_sno, alert_type, sub_type, value) values (@alert_mgr_sno, @alert_type, @sub_type, @value)");
                            cmd.Parameters.Add("@alert_mgr_sno", Sno);
                            cmd.Parameters.Add("@alert_type", alrt.inpoichk);
                            cmd.Parameters.Add("@sub_type", "");
                            cmd.Parameters.Add("@value", 0);
                            long inpoisno = vdm.insertScalar(cmd);
                            //string inpoilocsstr = obj2.inpoi;
                            //string[] delimiters = new string[] { "<br>" };
                            //string[] inpoilocs = inpoilocsstr.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

                            foreach (var locs in obj2.inpoi)
                            {
                                if (locs != "" && locs != null)
                                {
                                    cmd = new MySqlCommand("insert into alert_mgr_loc_info (alert_mgr_sinfo_sno, loc_id) values (@alert_mgr_sinfo_sno, @loc_id)");
                                    cmd.Parameters.Add("@alert_mgr_sinfo_sno", inpoisno);
                                    cmd.Parameters.Add("@loc_id", locs);
                                    vdm.insert(cmd);
                                }
                            }
                        }
                        if (alrt.outpoichk == "outpoi")
                        {
                            cmd = new MySqlCommand("Insert into alert_mgr_subinfo (alert_mgr_sno, alert_type, sub_type, value) values (@alert_mgr_sno, @alert_type, @sub_type, @value)");
                            cmd.Parameters.Add("@alert_mgr_sno", Sno);
                            cmd.Parameters.Add("@alert_type", alrt.outpoichk);
                            cmd.Parameters.Add("@sub_type", "");
                            cmd.Parameters.Add("@value", 0);
                            long outpoisno = vdm.insertScalar(cmd);
                            //string inpoilocsstr = obj2.outpoi;
                            //string[] delimiters = new string[] { "<br>" };
                            //string[] outpoilocs = inpoilocsstr.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

                            foreach (var locs in obj2.outpoi)
                            {
                                if (locs != "" && locs != null)
                                {
                                    cmd = new MySqlCommand("insert into alert_mgr_loc_info (alert_mgr_sinfo_sno, loc_id) values (@alert_mgr_sinfo_sno, @loc_id)");
                                    cmd.Parameters.Add("@alert_mgr_sinfo_sno", outpoisno);
                                    cmd.Parameters.Add("@loc_id", locs);
                                    vdm.insert(cmd);
                                }
                            }
                        }
                        if (alrt.stoppagechk == "stoppage")
                        {
                            if (alrt.stpinpoichk == "stopinpoi")
                            {
                                cmd = new MySqlCommand("Insert into alert_mgr_subinfo (alert_mgr_sno, alert_type, sub_type, value) values (@alert_mgr_sno, @alert_type, @sub_type, @value)");
                                cmd.Parameters.Add("@alert_mgr_sno", Sno);
                                cmd.Parameters.Add("@alert_type", alrt.stoppagechk);
                                cmd.Parameters.Add("@sub_type", "inpoi");
                                cmd.Parameters.Add("@value", 0);
                                long stopinpoisno = vdm.insertScalar(cmd);


                                foreach (tablelocs1 tbl in obj2.tablelocs)
                                {
                                    //string stpinpoilocsstr = "";
                                    //stpinpoilocsstr = tbl.loc_logs;
                                    //string[] delimiters = new string[] { "<br>" };
                                    //string[] stpinpoilocsstrlocs = stpinpoilocsstr.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                                    foreach (var locs in tbl.loc_logs)
                                    {
                                        if (locs != "" && locs != null)
                                        {
                                            cmd = new MySqlCommand("insert into alert_mgr_loc_info (alert_mgr_sinfo_sno, loc_id, value) values (@alert_mgr_sinfo_sno, @loc_id,@value)");
                                            cmd.Parameters.Add("@alert_mgr_sinfo_sno", stopinpoisno);
                                            cmd.Parameters.Add("@loc_id", locs);
                                            cmd.Parameters.Add("@value", tbl.maxtime);
                                            vdm.insert(cmd);
                                        }
                                    }
                                }


                            }
                            if (alrt.stpoutpoichk == "stopoutpoi")
                            {
                                cmd = new MySqlCommand("Insert into alert_mgr_subinfo (alert_mgr_sno, alert_type, sub_type, value) values (@alert_mgr_sno, @alert_type, @sub_type, @value)");
                                cmd.Parameters.Add("@alert_mgr_sno", Sno);
                                cmd.Parameters.Add("@alert_type", alrt.stoppagechk);
                                cmd.Parameters.Add("@sub_type", "outpoi");
                                if (obj2.stpmaxstptme != "")
                                {
                                    cmd.Parameters.Add("@value", obj2.stpmaxstptme);
                                }
                                else
                                {
                                    cmd.Parameters.Add("@value", 0);

                                }
                                long stopoutpoisno = vdm.insertScalar(cmd);


                                foreach (var locs in obj2.noalert)
                                {
                                    if (locs != "" && locs != null)
                                    {
                                        cmd = new MySqlCommand("insert into alert_mgr_loc_info (alert_mgr_sinfo_sno, loc_id) values (@alert_mgr_sinfo_sno, @loc_id)");
                                        cmd.Parameters.Add("@alert_mgr_sinfo_sno", stopoutpoisno);
                                        cmd.Parameters.Add("@loc_id", locs);
                                        vdm.insert(cmd);
                                    }
                                }


                            }

                        }
                    }
                    string response = GetJson("Successfully Modified");
                    context.Response.Write(response);
                }
            }
            catch (Exception ex)
            {
                string response = GetJson("Error");
                context.Response.Write(response);
            }
        }
        private void btnTripendSaveClick(HttpContext context)
        {
            vdm = new VehicleDBMgr();
            vdm.InitializeDB();
            try
            {
                string Username = context.Session["field1"].ToString();
                string tripsno = context.Request["tripsno"];
                string endodordng = context.Request["endodordng"];
                string endfuelrdng = context.Request["endfuelrdng"];
                string endhourmtrrdng = context.Request["endhourmtrrdng"];
                string gpskms = context.Request["gpskms"];
                string TripDate = context.Request["TripDate"];
                //DateTime ServerDateCurrentdate = VehicleDBMgr.GetTime(vdm.conn);

                cmd = new MySqlCommand("SELECT sno, tripsheetsno, jobcarddate, jobcardname, status, userid, operatedby FROM fleetjobcards WHERE (status = @status) AND (tripsheetsno = @tripsheetsno)");
                cmd.Parameters.Add("@tripsheetsno", tripsno);
                cmd.Parameters.Add("@status", "A");
                DataTable dtJobCards = vdm.SelectQuery(cmd).Tables[0];
                if (dtJobCards.Rows.Count > 0)
                {
                    string msg = "Please Complete Job cards";
                    string response = GetJson(msg);
                    context.Response.Write(response);
                }
                else
                {
                    var Status = "C";
                    cmd = new MySqlCommand("UPDATE fleettripsheet SET Status=@Status,GpsKms = @GpsKms, EndDate = @EndDate, EndFuelValue = @EndFuelValue, EndOdometerReading = @EndOdometerReading, EndHrMeter = @EndHrMeter WHERE (Sno = @tripsno)");
                    cmd.Parameters.Add("@Status", Status);
                    cmd.Parameters.Add("@GpsKms", gpskms);
                    cmd.Parameters.Add("@EndDate", TripDate);
                    cmd.Parameters.Add("@EndFuelValue", endfuelrdng);
                    cmd.Parameters.Add("@EndOdometerReading", endodordng);
                    cmd.Parameters.Add("@EndHrMeter", endhourmtrrdng);
                    cmd.Parameters.Add("@tripsno", tripsno);
                    vdm.Update(cmd);
                    string msg = "Trip ended successfully";
                    string response = GetJson(msg);
                    context.Response.Write(response);
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                string response = GetJson(msg);
                context.Response.Write(response);
            }
        }
        public class tripdetailscls
        {
            public List<JobCardDetails> jobcards { get; set; }
            public string Tripdate { get; set; }
            public string Vehicleno { get; set; }
            public string Drivername { get; set; }
            public string RouteName { get; set; }
            public string StrartReading { get; set; }
            public string StrartFuel { get; set; }
            public string gpskms { get; set; }
        }
        private void gettripalldetails(HttpContext context)
        {
            vdm = new VehicleDBMgr();
            vdm.InitializeDB();
            try
            {
                string Username = context.Session["field1"].ToString();
                string mainuser = "";
                if (context.Session["main_user"] == null)
                {
                    cmd = new MySqlCommand("SELECT main_user FROM loginstable WHERE (loginid = @loginid)");
                    cmd.Parameters.Add("@loginid", Username);
                    DataTable mainusertbl = vdm.SelectQuery(cmd).Tables[0];
                    if (mainusertbl.Rows.Count > 0)
                    {
                        mainuser = mainusertbl.Rows[0]["main_user"].ToString();
                        context.Session["main_user"] = mainuser;
                    }
                    else
                    {
                        mainuser = Username;
                    }
                }
                else
                {
                    mainuser = context.Session["main_user"].ToString();
                }
                string tripsno = context.Request["tripsno"];
                string TripDate = context.Request["TripDate"];
                cmd = new MySqlCommand("SELECT fleettripsheet.TripSheetNo, fleettripsheet.Tripdate, fleettripsheet.Vehicleno, employeestable.EmployeeName, employeestable.phoneNumber, fleettripsheet.Sno, fleettripsheet.LoadType, employeestable.LicenseNo, routetable.RouteName, fleetjobcards.jobcarddate, fleetjobcards.jobcardname, fleetjobcards.jobcarddetails, fleetjobcards.status, fleettripsheet.StrartReading, fleettripsheet.FuelTank FROM fleettripsheet LEFT OUTER JOIN employeestable ON fleettripsheet.DriverID = employeestable.Sno LEFT OUTER JOIN routetable ON fleettripsheet.RouteID = routetable.SNo LEFT OUTER JOIN fleetjobcards ON fleettripsheet.Sno = fleetjobcards.tripsheetsno AND fleettripsheet.UserID = fleetjobcards.userid WHERE (fleettripsheet.Sno = @tripsheetsno)");
                cmd.Parameters.Add("@tripsheetsno", tripsno);
                DataTable dttripsdata = vdm.SelectQuery(cmd).Tables[0];
                if (dttripsdata.Rows.Count > 0)
                {
                    tripdetailscls tripdetails = new tripdetailscls();
                    tripdetails.Tripdate = dttripsdata.Rows[0]["Tripdate"].ToString();
                    tripdetails.Vehicleno = dttripsdata.Rows[0]["Vehicleno"].ToString();
                    tripdetails.Drivername = dttripsdata.Rows[0]["EmployeeName"].ToString();
                    tripdetails.RouteName = dttripsdata.Rows[0]["RouteName"].ToString();
                    tripdetails.StrartReading = dttripsdata.Rows[0]["StrartReading"].ToString();
                    tripdetails.StrartFuel = dttripsdata.Rows[0]["FuelTank"].ToString();

                    #region codefor selected vehicles
                    DataTable logs = new DataTable();
                    DataTable tottable = new DataTable();
                    List<string> logstbls = new List<string>();
                    logstbls.Add("GpsTrackVehicleLogs");
                    logstbls.Add("GpsTrackVehicleLogs1");
                    logstbls.Add("GpsTrackVehicleLogs2");
                    logstbls.Add("GpsTrackVehicleLogs3");
                    //DateTime ServerDateCurrentdate = VehicleDBMgr.GetTime(vdm.conn);
                    DateTime fromdate = DateTime.Now;
                    fromdate = DateTime.Parse(tripdetails.Tripdate);
                    foreach (string tbname in logstbls)
                    {
                        cmd = new MySqlCommand("SELECT " + tbname + ".VehicleID, " + tbname + ".Speed, " + tbname + ".DateTime, " + tbname + ".Diesel, " + tbname + ".Latitiude, " + tbname + ".Longitude, " + tbname + ".TimeInterval, " + tbname + ".inp4, " + tbname + ".Status, " + tbname + ".Odometer, " + tbname + ".Direction, " + tbname + ".Direction AS Expr1, vehiclemaster.VendorNo, vehiclemaster.VendorName, vehiclemaster.VehicleTypeName, vehiclemaster.MaintenancePlantName FROM " + tbname + " LEFT OUTER JOIN vehiclemaster ON " + tbname + ".VehicleID = vehiclemaster.VehicleID AND " + tbname + ".UserID = vehiclemaster.UserName WHERE (" + tbname + ".DateTime >= @starttime) AND (" + tbname + ".DateTime <= @endtime) AND (" + tbname + ".VehicleID = '" + tripdetails.Vehicleno + "') and (" + tbname + ".UserID='" + mainuser + "') ORDER BY " + tbname + ".DateTime");
                        cmd.Parameters.Add(new MySqlParameter("@starttime", fromdate));
                        cmd.Parameters.Add(new MySqlParameter("@endtime", TripDate));
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
                    DataTable table = dv.ToTable();
                    DataRow firstrow = null;
                    DataRow lastrow = null;
                    double TotalDistance = 0;
                    if (table.Rows.Count > 1)
                    {
                        firstrow = table.Rows[0];
                        lastrow = table.Rows[table.Rows.Count - 1];
                        if (firstrow != null && lastrow != null)
                        {
                            double firstval = 0;
                            double.TryParse(firstrow["Odometer"].ToString(), out firstval);
                            double lastval = 0;
                            double.TryParse(lastrow["Odometer"].ToString(), out lastval);
                            if (lastval > 0 && firstval > 0)
                                TotalDistance = lastval - firstval;
                        }
                    }

                    #endregion
                    tripdetails.gpskms = TotalDistance.ToString();
                    List<JobCardDetails> jobcards = new List<JobCardDetails>();
                    foreach (DataRow dr in dttripsdata.Rows)
                    {
                        JobCardDetails JobCard = new JobCardDetails();
                        JobCard.jobcardname = dr["jobcardname"].ToString();
                        JobCard.jobcarddetails = dr["jobcarddetails"].ToString();
                        JobCard.status = dr["status"].ToString();
                        jobcards.Add(JobCard);
                    }
                    tripdetails.jobcards = jobcards;
                    string response = GetJson(tripdetails);
                    context.Response.Write(response);
                }
            }
            catch(Exception ex)
            {
                string msg = ex.Message;
                string response = GetJson(msg);
                context.Response.Write(response);
            }
        }
        public class jobtypescls
        {
            public string jobtype { get; set; }
            public string jobdetails { get; set; }
        }
        public class Employeeclass
        {
            public string EmpID { get; set; }
            public string EmpName { get; set; }
            public string EmployeeType { get; set; }
        }
        public class tripsheetroutes
        {
            public string routesno { get; set; }
            public string routesname { get; set; }
        }
        public class gettripsheetperametersclass
        {
            public List<vehiclesclass> vehicles { get; set; }
            public List<Employeeclass> Employee { get; set; }
            public List<tripsheetroutes> routes { get; set; }
        }
       private void GetTripExpenses(HttpContext context)
        {
            vdm = new VehicleDBMgr();
            vdm.InitializeDB();
            try
            {
                string Username = context.Session["field1"].ToString();
                cmd = new MySqlCommand("SELECT ItemType, ItemName, UserName, ItemCode, SNo FROM vehiclemanage WHERE (UserName = @UserName) AND (ItemType = 'Expenses')");
                cmd.Parameters.Add("@UserName", Username);
                DataTable dtExpenses = vdm.SelectQuery(cmd).Tables[0];
                List<expenses> Explist = new List<expenses>();
                foreach (DataRow dr in dtExpenses.Rows)
                {
                    expenses Getexpenses = new expenses();
                    Getexpenses.Expenseno = dr["ItemName"].ToString();
                    Getexpenses.ExpenseName = dr["ItemName"].ToString();
                    Explist.Add(Getexpenses);
                }
                string response = GetJson(Explist);
                context.Response.Write(response);
            }
            catch
            {
            }
        }
        public class expenses
        {
            public string Expenseno { get; set; }
            public string ExpenseName { get; set; }
        }
        private void btnSaveTripsheetLogsClick(HttpContext context)
        {
            vdm = new VehicleDBMgr();
            vdm.InitializeDB();
            try
            {
                string Username = context.Session["field1"].ToString();
                var js = new JavaScriptSerializer();
                var title1 = context.Request.Params[1];
                Routes obj = js.Deserialize<Routes>(title1);
                string tripsno = obj.tripsno;
                foreach (TripLogs tsl in obj.tripLogDetails)
                {
                    string TripDate = tsl.TripDate;
                    DateTime dtLogTime = Convert.ToDateTime(TripDate);
                    cmd = new MySqlCommand("insert into fleettriplogs (tripsno, doe,km,place,details,amount,qty,diesel) values (@tripsno, @doe,@km,@place,@details,@amount,@qty,@diesel)");
                    cmd.Parameters.Add("@tripsno", tripsno);
                    cmd.Parameters.Add("@doe", dtLogTime);
                    cmd.Parameters.Add("@km", tsl.Kms);
                    cmd.Parameters.Add("@place", tsl.Place);
                    cmd.Parameters.Add("@details", tsl.Details);
                    cmd.Parameters.Add("@amount", tsl.Amount);
                    cmd.Parameters.Add("@qty", tsl.Qty);
                    cmd.Parameters.Add("@diesel", tsl.Diesel);
                    vdm.insert(cmd);
                }
                foreach (TripLogs tsl in obj.tripExpDetails)
                {
                    cmd = new MySqlCommand("insert into fleettripexpenses (tripno, expensetype,amount,location,userid,operatedby) values (@tripno, @expensetype,@amount,@location,@userid,@operatedby)");
                    cmd.Parameters.Add("@tripno", tripsno);
                    cmd.Parameters.Add("@expensetype", tsl.ExpenseType);
                    cmd.Parameters.Add("@amount", tsl.Amount);
                    cmd.Parameters.Add("@location", tsl.Place);
                    cmd.Parameters.Add("@userid", Username);
                    cmd.Parameters.Add("@operatedby", Username);
                    vdm.insert(cmd);
                }
                string msg = "Logs saved Successfully";
                string response = GetJson(msg);
                context.Response.Write(response);
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                string response = GetJson(msg);
                context.Response.Write(response);
            }
        }
        private void btnTripSheetPrintClick(HttpContext context)
        {
            vdm = new VehicleDBMgr();
            vdm.InitializeDB();
            try
            {
                context.Session["TripSheetNo"] = context.Request["TripSheetNo"];
                string msg = "Success";
                string response = GetJson(msg);
                context.Response.Write(response);
            }
            catch
            {
            }
        }
        private void UpdateJobcardbtnclick(HttpContext context)
        {
            vdm = new VehicleDBMgr();
            vdm.InitializeDB();
            try
            {
                string Username = context.Session["field1"].ToString();
                string tripsno = context.Request["TripSheetNo"];
                string checkedjobcards = context.Request["JobCardName"];
                string[] jobcards = checkedjobcards.Split(',');
                foreach (string jbcrd in jobcards)
                {
                    cmd = new MySqlCommand("update fleetjobcards set status=@status where userid=@userid and tripsheetsno=@tripsheetsno and jobcardname=@jobcardname");
                    cmd.Parameters.Add("@tripsheetsno", tripsno);
                    cmd.Parameters.Add("@jobcardname", jbcrd);
                    cmd.Parameters.Add("@status", "C");
                    cmd.Parameters.Add("@userid", Username);
                    vdm.insert(cmd);
                    string msg = "Jobcards successfully Updated";
                    string response = GetJson(msg);
                    context.Response.Write(response);
                }
            }
            catch (Exception ex)
            {
                string response = GetJson("Error");
                context.Response.Write(response);
            }
        }
        private void GetBtnViewJobcardclick(HttpContext context)
        {
            try
            {
                vdm = new VehicleDBMgr();
                vdm.InitializeDB();
                string TripSheetNo = context.Request["TripSheetNo"];
                cmd = new MySqlCommand("SELECT sno, tripsheetsno, jobcarddate, jobcardname,jobcarddetails, status FROM fleetjobcards WHERE (tripsheetsno = @tripsheetsno)");
                cmd.Parameters.Add("@tripsheetsno", TripSheetNo);
                DataTable dtVehicleDetails = vdm.SelectQuery(cmd).Tables[0];
                List<JobCardDetails> JobCardlist = new List<JobCardDetails>();
                foreach (DataRow dr in dtVehicleDetails.Rows)
                {
                    JobCardDetails GetCards = new JobCardDetails();
                    GetCards.jobcardname = dr["jobcardname"].ToString();
                    GetCards.jobcarddetails = dr["jobcarddetails"].ToString();
                    string status = dr["status"].ToString();
                    string JobStatus = "";
                    if (status == "A")
                    {
                        JobStatus = "Pending";
                    }
                    if (status == "C")
                    {
                        JobStatus = "Verified";
                    }
                    GetCards.status = JobStatus;
                    JobCardlist.Add(GetCards);
                }
                string response = GetJson(JobCardlist);
                context.Response.Write(response);
            }
            catch
            {
            }
        }
        public class JobCardDetails
        {
            public string status { get; set; }
            public string jobcardname { get; set; }
            public string jobcarddetails { get; set; }
        }
        private void GetAssignTripSheets(HttpContext context)
        {
            try
            {
                vdm = new VehicleDBMgr();
                vdm.InitializeDB();
                string Username = context.Session["field1"].ToString();
                cmd = new MySqlCommand("SELECT fleettripsheet.TripSheetNo, fleettripsheet.Tripdate, fleettripsheet.Vehicleno, employeestable.EmployeeName, employeestable.phoneNumber, fleettripsheet.Sno, fleettripsheet.LoadType, employeestable.LicenseNo, routetable.RouteName, fleettripsheet.StrartReading, fleettripsheet.FuelTank FROM fleettripsheet LEFT OUTER JOIN employeestable ON fleettripsheet.DriverID = employeestable.Sno LEFT OUTER JOIN routetable ON fleettripsheet.RouteID = routetable.SNo WHERE (fleettripsheet.Status = @Status) and (fleettripsheet.UserID=@UserName)");
                cmd.Parameters.Add("@UserName", Username);
                cmd.Parameters.Add("@Status", "A");
                DataTable dtVehicleDetails = vdm.SelectQuery(cmd).Tables[0];
                List<vehiclesclass> vehicleslist = new List<vehiclesclass>();
                foreach (DataRow dr in dtVehicleDetails.Rows)
                {
                    vehiclesclass vehicles = new vehiclesclass();
                    vehicles.TripSheetNo = dr["Sno"].ToString();
                    vehicles.TripDate = dr["Tripdate"].ToString();
                    vehicles.vehicleno = dr["Vehicleno"].ToString();
                    vehicles.Driver = dr["EmployeeName"].ToString();
                    vehicles.Routename = dr["RouteName"].ToString();
                    vehicleslist.Add(vehicles);
                }
                string response = GetJson(vehicleslist);
                context.Response.Write(response);
            }
            catch
            {
            }
        }
        private void GetVehicleDetails(HttpContext context)
        {
            try
            {
                vdm = new VehicleDBMgr();
                vdm.InitializeDB();
                string VehicleNo = context.Request["VehicleNo"];
                string Username = context.Session["field1"].ToString();
                cmd = new MySqlCommand("SELECT paireddata.Sno,paireddata.VehicleNumber, cabmanagement.VehicleMake, cabmanagement.VehicleModel, cabmanagement.VehicleType, paireddata.FullTankVal, paireddata.EmptyTankVal, paireddata.VehicleType AS Expr1, paireddata.FullTankLtrs, paireddata.emptyTankLrs, loginstable.main_user, cabmanagement.RouteName, cabmanagement.RouteCode FROM loginsconfigtable INNER JOIN paireddata ON loginsconfigtable.VehicleID = paireddata.VehicleNumber INNER JOIN loginstable ON paireddata.UserID = loginstable.main_user AND loginsconfigtable.Refno = loginstable.refno INNER JOIN  cabmanagement ON paireddata.VehicleNumber = cabmanagement.VehicleID WHERE (loginstable.loginid = @UserName) and (paireddata.VehicleNumber=@VehicleNo)");
                cmd.Parameters.Add("@VehicleNo", VehicleNo);
                cmd.Parameters.Add("@UserName", Username);
                DataTable dtVehicle = vdm.SelectQuery(cmd).Tables[0];
                List<vehiclesclass> vehicleslist = new List<vehiclesclass>();
                foreach (DataRow dr in dtVehicle.Rows)
                {
                    vehiclesclass vehicles = new vehiclesclass();
                    vehicles.vehiclemake = dr["VehicleMake"].ToString();
                    vehicles.vehiclemodel = dr["VehicleModel"].ToString();
                    vehicles.vehicletype = dr["VehicleType"].ToString();
                    vehicleslist.Add(vehicles);
                }
                string response = GetJson(vehicleslist);
                context.Response.Write(response);
            }
            catch
            {
            }
        }
        private void btnTripSheetSaveClick(HttpContext context)
        {
            try
            {
                vdm = new VehicleDBMgr();
                vdm.InitializeDB();
                string TripDate = context.Request["TripDate"];
                string vehicleNo = context.Request["vehicleNo"];
                string RouteID = context.Request["RouteID"];
                string driver = context.Request["driver"];
                string helper = context.Request["helper"];
                string VehicleStartReading = context.Request["VehicleStartReading"];
                string HrReading = context.Request["HrReading"];
                string FuelTank = context.Request["FuelTank"];
                string load = context.Request["load"];
                string Qty = context.Request["Qty"];
                string tripstartfrom = context.Request["tripstartfrom"];
                string Status = "A";
                //DateTime ServerDateCurrentdate = VehicleDBMgr.GetTime(vdm.conn);
                string UserID = context.Session["field1"].ToString();
                cmd = new MySqlCommand("Select IFNULL(MAX(Sno),0)+1 as Sno from fleettripsheet where UserID=@UserID");
                cmd.Parameters.Add("@UserID", UserID);
                DataTable dtReceipt = vdm.SelectQuery(cmd).Tables[0];
                string TripNo = "T" + dtReceipt.Rows[0]["Sno"].ToString();
                cmd = new MySqlCommand("insert into fleettripsheet(TripSheetNo,Tripdate, Vehicleno, StrartReading, HourReading, DriverID, HelperID, FuelTank, LoadType, Qty,  RouteID,Status,UserID,OperatedBy) values (@TripSheetNo,@Tripdate, @Vehicleno, @StrartReading, @HourReading, @DriverID, @HelperID, @FuelTank, @LoadType, @Qty, @RouteID,@Status,@UserID,@OperatedBy)");
                cmd.Parameters.Add("@TripSheetNo", TripNo);
                cmd.Parameters.Add("@Tripdate", TripDate);
                cmd.Parameters.Add("@Vehicleno", vehicleNo);
                cmd.Parameters.Add("@StrartReading", VehicleStartReading);
                cmd.Parameters.Add("@HourReading", HrReading);
                cmd.Parameters.Add("@DriverID", driver);
                cmd.Parameters.Add("@HelperID", helper);
                cmd.Parameters.Add("@FuelTank", FuelTank);
                cmd.Parameters.Add("@LoadType", load);
                cmd.Parameters.Add("@Qty", Qty);
                cmd.Parameters.Add("@RouteID", RouteID);
                cmd.Parameters.Add("@Status", Status);
                cmd.Parameters.Add("@UserID", UserID);
                cmd.Parameters.Add("@OperatedBy", UserID);
                vdm.insert(cmd);
                string msg = "Trip Sheet Saved Successfully";
                string response = GetJson(msg);
                context.Response.Write(response);
            }
            catch(Exception EX)
            {
                string msg = EX.ToString();
                string response = GetJson(msg);
                context.Response.Write(response);
            }
        }
		
        public class getjobcardscls
        {
            public string jobcard { get; set; }
            public string jobcarddetails { get; set; }
            public string jobcardstatus { get; set; }
        }
        private void gettripjobcards(HttpContext context)
        {
            vdm = new VehicleDBMgr();
            vdm.InitializeDB();
            try
            {
                string Username = context.Session["field1"].ToString();
                string tripsno = context.Request["tripsno"];
                List<getjobcardscls> getjobcardsclslst = new List<getjobcardscls>();

                cmd = new MySqlCommand("SELECT sno, tripsheetsno, jobcarddate, jobcardname,jobcarddetails, status, userid, operatedby FROM fleetjobcards where userid=@userid");
                cmd.Parameters.Add("@userid", Username);
                DataTable jobcarddata = vdm.SelectQuery(cmd).Tables[0];
                foreach (DataRow dr in jobcarddata.Rows)
                {
                    getjobcardscls getjobcardss = new getjobcardscls();
                    getjobcardss.jobcard = dr["jobcardname"].ToString();
                    getjobcardss.jobcarddetails = dr["jobcarddetails"].ToString();
                    getjobcardss.jobcardstatus = dr["status"].ToString();
                    getjobcardsclslst.Add(getjobcardss);
                }
                string response = GetJson(getjobcardsclslst);
                context.Response.Write(response);
            }
            catch (Exception ex)
            {
                string response = GetJson("Error");
                context.Response.Write(response);
            }
        }

        private void jobcardsaveclick(HttpContext context)
        {
            vdm = new VehicleDBMgr();
            vdm.InitializeDB();
            try
            {
                string Username = context.Session["field1"].ToString();
                var js = new JavaScriptSerializer();
                var title1 = context.Request.Params[1];
                Routes obj = js.Deserialize<Routes>(title1);
                string tripsno = obj.tripsno;
                DateTime Currentdate = VehicleDBMgr.GetTime(vdm.conn);
                DateTime jobcarddate = obj.jobcarddate;
                foreach (jobtypescls jbcrd in obj.checkedjobcards)
                {
                    cmd = new MySqlCommand("insert into fleetjobcards (tripsheetsno, jobcarddate,jobcardname,jobcarddetails,status, userid, operatedby) values (@tripsheetsno, @jobcarddate,@jobcardname,@jobcarddetails,@status, @userid, @operatedby)");
                    cmd.Parameters.Add("@tripsheetsno", tripsno);
                    cmd.Parameters.Add("@jobcarddate", jobcarddate);
                    cmd.Parameters.Add("@jobcardname", jbcrd.jobtype);
                    cmd.Parameters.Add("@jobcarddetails", jbcrd.jobdetails);
                    cmd.Parameters.Add("@status", "A");
                    cmd.Parameters.Add("@userid", Username);
                    cmd.Parameters.Add("@operatedby", Username);
                    vdm.insert(cmd);
                }

                cmd = new MySqlCommand("UPDATE fleettripsheet SET Jobcards = 'Y' WHERE (Sno = @Sno)");
                cmd.Parameters.Add("@Sno", tripsno);
                vdm.Update(cmd);
                string msg = "Jobcards successfully added";
                string response = GetJson(msg);
                context.Response.Write(response);
            }
            catch (Exception ex)
            {
                string response = GetJson("Error");
                context.Response.Write(response);
            }
        }
    
        public class getassignedtripscls
        {
            public string tripsno { get; set; }
            public string tripno { get; set; }
            public string vehicleno { get; set; }
            public string Jobcards { get; set; }
        }
        private void getassignedtrips(HttpContext context)
        {
            vdm = new VehicleDBMgr();
            vdm.InitializeDB();
            try
            {
                string Username = context.Session["field1"].ToString();
                DataTable Vehiclesdata = new DataTable();
                List<getassignedtripscls> getassignedtripsclslst = new List<getassignedtripscls>();
                cmd = new MySqlCommand("SELECT * FROM fleettripsheet where UserID=@UserID and status='A'");
                cmd.Parameters.Add("@UserID", Username);
                DataTable tripdata = vdm.SelectQuery(cmd).Tables[0];
                foreach (DataRow dr in tripdata.Rows)
                {
                    getassignedtripscls getassignedtrips = new getassignedtripscls();
                    getassignedtrips.tripsno = dr["Sno"].ToString();
                    getassignedtrips.tripno = dr["TripSheetNo"].ToString();
                    getassignedtrips.vehicleno = dr["Vehicleno"].ToString();
                    getassignedtrips.Jobcards = dr["Jobcards"].ToString();
                    getassignedtripsclslst.Add(getassignedtrips);
                }
                string response = GetJson(getassignedtripsclslst);
                context.Response.Write(response);
            }
            catch (Exception ex)
            {
                string response = GetJson("Error");
                context.Response.Write(response);
            }
        }


        private void Initilizedatafortripsheet(HttpContext context)
        {
            vdm = new VehicleDBMgr();
            vdm.InitializeDB();
            try
            {
                string Username = context.Session["field1"].ToString();
                DataTable Vehiclesdata = new DataTable();
                gettripsheetperametersclass gettripsheetperametersclasslst = new gettripsheetperametersclass();
                cmd = new MySqlCommand("SELECT  paireddata.VehicleNumber, paireddata.VehicleType, fleettripsheet.Status, paireddata.Sno FROM loginsconfigtable INNER JOIN paireddata ON loginsconfigtable.VehicleID = paireddata.VehicleNumber INNER JOIN loginstable ON paireddata.UserID = loginstable.main_user AND loginsconfigtable.Refno = loginstable.refno INNER JOIN cabmanagement ON paireddata.VehicleNumber = cabmanagement.VehicleID LEFT OUTER JOIN fleettripsheet ON paireddata.VehicleNumber = fleettripsheet.Vehicleno WHERE (loginstable.loginid = @UserName) AND (fleettripsheet.Status <> 'A' OR fleettripsheet.Status IS NULL) Group by paireddata.VehicleNumber");
                cmd.Parameters.Add("@UserName", Username);
                Vehiclesdata = vdm.SelectQuery(cmd).Tables[0];
                List<vehiclesclass> vehicleslist = new List<vehiclesclass>();
                foreach (DataRow dr in Vehiclesdata.Rows)
                {
                    vehiclesclass vehicles = new vehiclesclass();
                    vehicles.vehicleno = dr["VehicleNumber"].ToString();
                    vehicles.vehicletype = dr["VehicleType"].ToString();
                    vehicleslist.Add(vehicles);
                }
                gettripsheetperametersclasslst.vehicles = vehicleslist;
                List<Employeeclass> driverslist = new List<Employeeclass>();
                cmd = new MySqlCommand("SELECT Status, DriverID, HelperID, TripSheetNo, UserID FROM fleettripsheet WHERE (Status = 'A') AND (UserID = @UserID)");
                cmd.Parameters.Add("@UserID", Username);
                DataTable tripsdata = vdm.SelectQuery(cmd).Tables[0];
                cmd = new MySqlCommand("SELECT Sno, UserName, EmployeeID, EmployeeName, EmployeeType, DoB, Address, phoneNumber, PermanentAddress, PermanentPhoneNo, BloodGroup, Qualification, Experience, DOJ, LicenseNo, LicenseNoExpDT, BadgeNo, BadgeNoExpDT, Remarks, Photo, OperatorName FROM employeestable WHERE (EmployeeType = 'Driver' OR EmployeeType = 'Helper') AND (UserName = @UserName)");
                cmd.Parameters.Add("@UserName", Username);
                DataTable empdata = vdm.SelectQuery(cmd).Tables[0];
                foreach (DataRow dr in empdata.Rows)
                {
                    DataRow[] drr = tripsdata.Select("DriverID='" + dr["Sno"].ToString() + "' or HelperID='" + dr["Sno"].ToString() + "'");
                    if (drr.Length == 0)
                    {
                        Employeeclass GetEmp = new Employeeclass();
                        GetEmp.EmpID = dr["Sno"].ToString();
                        GetEmp.EmpName = dr["EmployeeName"].ToString();
                        GetEmp.EmployeeType = dr["EmployeeType"].ToString();
                        driverslist.Add(GetEmp);
                    }
                }
                gettripsheetperametersclasslst.Employee = driverslist;

                cmd = new MySqlCommand("Select SNo,RouteName from Routetable where UserID=@UserID");
                cmd.Parameters.Add("@UserID", Username);
                DataTable dtRoute = vdm.SelectQuery(cmd).Tables[0];
                List<tripsheetroutes> fillRoute = new List<tripsheetroutes>();
                foreach (DataRow dr in dtRoute.Rows)
                {
                    tripsheetroutes tripsheetroutescls = new tripsheetroutes();
                    tripsheetroutescls.routesno = dr["SNo"].ToString();
                    tripsheetroutescls.routesname = dr["RouteName"].ToString();
                    fillRoute.Add(tripsheetroutescls);
                }
                gettripsheetperametersclasslst.routes = fillRoute;
                string response = GetJson(gettripsheetperametersclasslst);
                context.Response.Write(response);
            }
            catch (Exception ex)
            {
                string response = GetJson("Error");
                context.Response.Write(response);
            }
        }
        public class getnotificationvehiclesclass
        {
            public string vehicleno { get; set; }
        }
        public class gettripdataclass
        {
            public string Refno { get; set; }
            public string Tripid { get; set; }
            public string assigndate { get; set; }
            public string vehiclemaster_sno { get; set; }
            public string Status { get; set; }
            public string RouteName { get; set; }
            public string StartOdometer { get; set; }
            public string EndOdometer { get; set; }
            public string DriverName { get; set; }
            public string DriverPhNo { get; set; }
            public string AttenderName { get; set; }
            public string AttenderPhNo { get; set; }
            public string FuelFilled { get; set; }
            public string TollAmount { get; set; }
            public string MaintenceClearenceBy { get; set; }
            public string Advance { get; set; }
            public string Remarks { get; set; }
        }

        private void gettripsheetdata(HttpContext context)
        {
            vdm = new VehicleDBMgr();
            vdm.InitializeDB();
            try
            {
                string Username = context.Session["field1"].ToString();
                string vehicleno = context.Request["vehicleno"];
                DataTable Vehiclesdata = new DataTable();
                if (context.Session["allvehicles"] == null)
                {
                    cmd = new MySqlCommand("SELECT paireddata.Sno,paireddata.VehicleNumber, paireddata.VehicleType, paireddata.FullTankVal, paireddata.EmptyTankVal, paireddata.VehicleType AS Expr1, paireddata.FullTankLtrs, paireddata.emptyTankLrs, loginstable.main_user, cabmanagement.RouteName, cabmanagement.RouteCode FROM loginsconfigtable INNER JOIN paireddata ON loginsconfigtable.VehicleID = paireddata.VehicleNumber INNER JOIN loginstable ON paireddata.UserID = loginstable.main_user AND loginsconfigtable.Refno = loginstable.refno INNER JOIN  cabmanagement ON paireddata.VehicleNumber = cabmanagement.VehicleID WHERE (loginstable.loginid = @UserName)");
                    //cmd = new MySqlCommand("SELECT paireddata.VehicleNumber,paireddata.VehicleType, paireddata.FullTankVal, paireddata.EmptyTankVal, paireddata.VehicleType, paireddata.FullTankLtrs, paireddata.emptyTankLrs, loginstable.main_user FROM loginsconfigtable INNER JOIN paireddata ON loginsconfigtable.VehicleID = paireddata.VehicleNumber INNER JOIN loginstable ON paireddata.UserID = loginstable.main_user AND loginsconfigtable.Refno = loginstable.refno WHERE (loginstable.loginid = @UserName)");
                    cmd.Parameters.Add("@UserName", Username);
                    Vehiclesdata = vdm.SelectQuery(cmd).Tables[0];
                    context.Session["allvehicles"] = Vehiclesdata;
                    vdm = null;
                }
                else
                {
                    Vehiclesdata = (DataTable)context.Session["allvehicles"];
                }
                DataRow[] vehsno = Vehiclesdata.Select("VehicleNumber='" + vehicleno + "'");
                if (vehsno.Length > 0)
                {
                    cmd = new MySqlCommand("SELECT Refno, Tripid, assigndate, completdate, vehiclemaster_sno, Status, UserID, RouteName, aCompletTime, PlantName, Triptype, TripConfig_sno, StartOdometer, EndOdometer, DriverName, DriverPhNo, AttenderName, AttenderPhNo, FuelFilled, TollAmount, MaintenceClearenceBy, Advance, Remarks FROM tripdata WHERE (vehiclemaster_sno = @vehno) AND (Status = 'A') AND (UserID = @UserID)");
                    cmd.Parameters.Add("@vehno", vehsno[0]["Sno"].ToString());
                    cmd.Parameters.Add("@UserID", Username);
                    DataTable tripdata = vdm.SelectQuery(cmd).Tables[0];
                    if (tripdata.Rows.Count > 0)
                    {
                        gettripdataclass gettrip = new gettripdataclass();
                        gettrip.Refno = tripdata.Rows[0]["Refno"].ToString();
                        gettrip.Tripid = tripdata.Rows[0]["Tripid"].ToString();
                        gettrip.assigndate = tripdata.Rows[0]["assigndate"].ToString();
                        gettrip.vehiclemaster_sno = tripdata.Rows[0]["vehiclemaster_sno"].ToString();
                        gettrip.Status = tripdata.Rows[0]["Status"].ToString();
                        gettrip.RouteName = tripdata.Rows[0]["RouteName"].ToString();
                        gettrip.StartOdometer = tripdata.Rows[0]["StartOdometer"].ToString();
                        gettrip.EndOdometer = tripdata.Rows[0]["EndOdometer"].ToString();
                        gettrip.DriverName = tripdata.Rows[0]["DriverName"].ToString();
                        gettrip.DriverPhNo = tripdata.Rows[0]["DriverPhNo"].ToString();
                        gettrip.AttenderName = tripdata.Rows[0]["AttenderName"].ToString();
                        gettrip.AttenderPhNo = tripdata.Rows[0]["AttenderPhNo"].ToString();
                        gettrip.FuelFilled = tripdata.Rows[0]["FuelFilled"].ToString();
                        gettrip.TollAmount = tripdata.Rows[0]["TollAmount"].ToString();
                        gettrip.MaintenceClearenceBy = tripdata.Rows[0]["MaintenceClearenceBy"].ToString();
                        gettrip.Advance = tripdata.Rows[0]["Advance"].ToString();
                        gettrip.Remarks = tripdata.Rows[0]["Remarks"].ToString();
                        string response = GetJson(gettrip);
                        context.Response.Write(response);
                    }
                    else
                    {
                        string response = GetJson("Empty");
                        context.Response.Write(response);
                    }
                }
                else
                {
                    string response = GetJson("Empty");
                    context.Response.Write(response);
                }
            }
            catch (Exception ex)
            {
                string response = GetJson("Error");
                context.Response.Write(response);
            }
        }

        private void tripdetals_save(HttpContext context)
        {
            vdm = new VehicleDBMgr();
            vdm.InitializeDB();
            try
            {
                string Username = context.Session["field1"].ToString();
                string vehtype = context.Request["vehtype"];
                string vehicletype = "";
                if (vehtype == "Puff")
                    vehicletype = "P";
                else if (vehtype == "Tanker")
                    vehicletype = "T";
                //DateTime tripdate = DateTime.Parse(context.Request["tripdate"].ToString());
                cmd = new MySqlCommand("SELECT TripSheetNo, UserID FROM configtable WHERE (UserID = @UserID)");
                cmd.Parameters.Add("@UserID", Username);
                DataTable tripid = vdm.SelectQuery(cmd).Tables[0];
                DateTime Currentdate = DateTime.Now;//.ToString("dd-MM-yyyy HH:mm:ss"), "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                string serialno = tripid.Rows[0]["TripSheetNo"].ToString();
                string gnrtdtripid = vehicletype + Currentdate.Month.ToString().PadLeft(2, '0') + Currentdate.Day.ToString().PadLeft(2, '0') + Currentdate.Year.ToString() + Currentdate.Hour.ToString().PadLeft(2, '0') + Currentdate.Minute.ToString().PadLeft(2, '0') + serialno;

                string tripvehno = context.Request["tripvehno"];
                DataTable Vehiclesdata = new DataTable();
                if (context.Session["allvehicles"] == null)
                {
                    cmd = new MySqlCommand("SELECT paireddata.Sno,paireddata.VehicleNumber, paireddata.VehicleType, paireddata.FullTankVal, paireddata.EmptyTankVal, paireddata.VehicleType AS Expr1, paireddata.FullTankLtrs, paireddata.emptyTankLrs, loginstable.main_user, cabmanagement.RouteName, cabmanagement.RouteCode FROM loginsconfigtable INNER JOIN paireddata ON loginsconfigtable.VehicleID = paireddata.VehicleNumber INNER JOIN loginstable ON paireddata.UserID = loginstable.main_user AND loginsconfigtable.Refno = loginstable.refno INNER JOIN  cabmanagement ON paireddata.VehicleNumber = cabmanagement.VehicleID WHERE (loginstable.loginid = @UserName)");
                    //cmd = new MySqlCommand("SELECT paireddata.VehicleNumber,paireddata.VehicleType, paireddata.FullTankVal, paireddata.EmptyTankVal, paireddata.VehicleType, paireddata.FullTankLtrs, paireddata.emptyTankLrs, loginstable.main_user FROM loginsconfigtable INNER JOIN paireddata ON loginsconfigtable.VehicleID = paireddata.VehicleNumber INNER JOIN loginstable ON paireddata.UserID = loginstable.main_user AND loginsconfigtable.Refno = loginstable.refno WHERE (loginstable.loginid = @UserName)");
                    cmd.Parameters.Add("@UserName", Username);
                    Vehiclesdata = vdm.SelectQuery(cmd).Tables[0];
                    context.Session["allvehicles"] = Vehiclesdata;
                    vdm = null;
                }
                else
                {
                    Vehiclesdata = (DataTable)context.Session["allvehicles"];
                }
                DataRow[] vehsno = Vehiclesdata.Select("VehicleNumber='" + tripvehno + "'");
                if (vehsno.Length > 0)
                {
                    string vehstatus = context.Request["vehstatus"];
                    double startodo = 0;
                    double.TryParse(context.Request["startodomtr"], out startodo);
                    double endodo = 0;
                    double.TryParse(context.Request["endodomtr"], out endodo);
                    string drivername = context.Request["drivername"];
                    string driverphno = context.Request["driverphno"];
                    string attender = context.Request["attender"];
                    string attenderph = context.Request["attenderph"];
                    double fuel = 0;
                    double.TryParse(context.Request["fuel"], out fuel);
                    double ttlamt = 0;
                    double.TryParse(context.Request["ttlamt"], out ttlamt);
                    string clearence = context.Request["driverphno"];
                    double advance = 0;
                    double.TryParse(context.Request["advance"], out advance);
                    string remarks = context.Request["remarks"];
                    string RouteName = context.Request["toute"];

                    cmd = new MySqlCommand("insert into tripdata ( Tripid, assigndate,vehiclemaster_sno,RouteName, Status, StartOdometer, EndOdometer, DriverName, DriverPhNo, AttenderName, AttenderPhNo, FuelFilled, TollAmount, MaintenceClearenceBy,Advance, Remarks,UserID) values ( @Tripid, @assigndate,@vehiclemaster_sno,@RouteName,@Status, @StartOdometer, @EndOdometer, @DriverName, @DriverPhNo, @AttenderName, @AttenderPhNo, @FuelFilled, @TollAmount,@MaintenceClearenceBy,@Advance,@Remarks,@UserID)");
                    cmd.Parameters.Add("@Tripid", gnrtdtripid);
                    cmd.Parameters.Add("@assigndate", Currentdate);
                    cmd.Parameters.Add("@vehiclemaster_sno", vehsno[0]["Sno"].ToString());
                    cmd.Parameters.Add("@RouteName", RouteName);
                    cmd.Parameters.Add("@Status", "A");
                    cmd.Parameters.Add("@StartOdometer", startodo);
                    cmd.Parameters.Add("@EndOdometer", endodo);
                    cmd.Parameters.Add("@DriverName", drivername);
                    cmd.Parameters.Add("@DriverPhNo", driverphno);
                    cmd.Parameters.Add("@AttenderName", attender);
                    cmd.Parameters.Add("@AttenderPhNo", attenderph);
                    cmd.Parameters.Add("@FuelFilled", fuel);
                    cmd.Parameters.Add("@TollAmount", ttlamt);
                    cmd.Parameters.Add("@MaintenceClearenceBy", clearence);
                    cmd.Parameters.Add("@Advance", advance);
                    cmd.Parameters.Add("@Remarks", remarks);
                    cmd.Parameters.Add("@UserID", Username);
                    vdm.insert(cmd);
                    int refno = (int)vdm.insertScalar(cmd);

                    int srlno = int.Parse(serialno);
                    int incrementedtripid = srlno + 1;
                    cmd = new MySqlCommand("update configtable set TripSheetNo=@TripSheetNo where UserID=@UserID");
                    cmd.Parameters.Add("@TripSheetNo", incrementedtripid);
                    cmd.Parameters.Add("@UserID", Username);
                    vdm.Update(cmd);
                    string[] data = new string[3];
                    data[0] = gnrtdtripid;
                    data[1] = "Successfully Trip Generated";
                    data[2] = refno.ToString();
                    string response = GetJson(data);
                    context.Response.Write(response);
                }
                else
                {
                    string response = GetJson("Error");
                    context.Response.Write(response);
                }
            }
            catch (Exception ex)
            {
                string response = GetJson("Error");
                context.Response.Write(response);
            }
        }
        private void trip_end(HttpContext context)
        {
            vdm = new VehicleDBMgr();
            vdm.InitializeDB();
            try
            {
                string Username = context.Session["field1"].ToString();
                double endodo = 0;
                double.TryParse(context.Request["endodomtr"], out endodo);
                DataTable Vehiclesdata = new DataTable();
                string tripsheetno = context.Request["tripsheetno"];
                cmd = new MySqlCommand("update tripdata set EndOdometer=@EndOdometer,Status='C' where Refno='" + tripsheetno + "' and UserID='" + Username + "'");
                cmd.Parameters.Add("@EndOdometer", endodo);
                vdm.Update(cmd);
                string response = GetJson("Trip Completed Successfully");
                context.Response.Write(response);
            }
            catch (Exception ex)
            {
                string response = GetJson("Error");
                context.Response.Write(response);
            }
        }
        private void loaddet_delete(HttpContext context)
        {
            vdm = new VehicleDBMgr();
            vdm.InitializeDB();
            try
            {
                string sno = context.Request["sno"];
                cmd = new MySqlCommand("delete from expensestable where sno=@sno");
                cmd.Parameters.Add("@sno", sno);
                vdm.Delete(cmd);
                string response = GetJson("Successfully Deleted");
                context.Response.Write(response);
            }
            catch (Exception ex)
            {
                string response = GetJson(ex.ToString());
                context.Response.Write(response);
            }

        }

        public class loading_grid_data
        {
            public string TripID { get; set; }
            public string Date { get; set; }
            public string KMs { get; set; }
            public string Place { get; set; }
            public string LoadorExpencesDetails { get; set; }
            public string InputType { get; set; }
            public string Amount { get; set; }
            public string sno { get; set; }
            public string TransactionType { get; set; }
            public string CommodityType { get; set; }
            public string Quantity { get; set; }
        }

        private void get_loaddet_grid(HttpContext context)
        {
            try
            {
                string tripsheetno = context.Request["tripsheetno"];
                vdm = new VehicleDBMgr();
                vdm.InitializeDB();
                cmd = new MySqlCommand("SELECT TripID, Date, Odometer, Place, LoadorExpencesDetails, InputType, Amount, sno,TransactionType, CommodityType, Quantity FROM expensestable where TripID=@TripID");
                cmd.Parameters.Add("@TripID", tripsheetno);
                DataTable DocketsResult = vdm.SelectQuery(cmd).Tables[0];
                List<loading_grid_data> griddata = new List<loading_grid_data>();
                if (DocketsResult.Rows.Count > 0)
                {
                    foreach (DataRow dr in DocketsResult.Rows)
                    {
                        loading_grid_data Getgriddata = new loading_grid_data();
                        Getgriddata.TripID = dr["TripID"].ToString();
                        //Getgriddata.Date = dr["Date"].ToString();
                        Getgriddata.Date = ((DateTime)dr["Date"]).ToString("yyyy-MM-dd HH:mm:ss");
                        Getgriddata.KMs = dr["Odometer"].ToString();
                        Getgriddata.Place = dr["Place"].ToString();
                        Getgriddata.LoadorExpencesDetails = dr["LoadorExpencesDetails"].ToString();
                        Getgriddata.InputType = dr["InputType"].ToString();
                        Getgriddata.Amount = dr["Amount"].ToString();
                        Getgriddata.sno = dr["sno"].ToString();
                        Getgriddata.TransactionType = dr["TransactionType"].ToString();
                        Getgriddata.CommodityType = dr["CommodityType"].ToString();
                        Getgriddata.Quantity = dr["Quantity"].ToString();
                        griddata.Add(Getgriddata);
                    }
                }
                string response = GetJson(griddata);
                context.Response.Write(response);
            }
            catch (Exception ex)
            {
                string response = GetJson(ex.ToString());
                context.Response.Write(response);
            }
        }

        private void expdet_save(HttpContext context)
        {
            try
            {
                vdm = new VehicleDBMgr();
                vdm.InitializeDB();
                string Username = context.Session["field1"].ToString();
                string expdatetime = context.Request["expdatetime"];
                string expkms = context.Request["expkms"];
                string expplace = context.Request["expplace"];
                string expdetails = context.Request["expdetails"];
                string tripsheetno = context.Request["tripsheetno"];
                string expamount = context.Request["expamount"];
                string btnval = context.Request["btnval"];
                string sno = context.Request["sno"];

                if (btnval == "Save")
                {
                    cmd = new MySqlCommand("Insert into expensestable(TripID, Date, Odometer, Place, LoadorExpencesDetails, InputType,Amount,Username) values(@TripID, @Date, @Odometer, @Place, @LoadorExpencesDetails, @InputType,@Amount,@Username)");
                    cmd.Parameters.Add("@TripID", tripsheetno);
                    cmd.Parameters.Add("@Date", expdatetime);
                    cmd.Parameters.Add("@Odometer", expkms);
                    cmd.Parameters.Add("@Place", expplace);
                    cmd.Parameters.Add("@LoadorExpencesDetails", expdetails);
                    cmd.Parameters.Add("@InputType", "E");
                    cmd.Parameters.Add("@Amount", expamount);
                    cmd.Parameters.Add("@Username", Username);

                    vdm.insert(cmd);
                    string respnceString = GetJson("Successfully Expenditured");
                    context.Response.Write(respnceString);
                }
                else
                {
                    cmd = new MySqlCommand("Update expensestable set Date=@Date,Odometer=@Odometer,Place=@Place,LoadorExpencesDetails=@LoadorExpencesDetails,InputType=@InputType,Amount=@Amount where sno=@sno and Username=@Username");
                    // cmd.Parameters.Add("@TripID", tripsheetno);
                    cmd.Parameters.Add("@Date", expdatetime);
                    cmd.Parameters.Add("@Odometer", expkms);
                    cmd.Parameters.Add("@Place", expplace);
                    cmd.Parameters.Add("@LoadorExpencesDetails", expdetails);
                    cmd.Parameters.Add("@InputType", "E");
                    cmd.Parameters.Add("@Amount", expamount);
                    cmd.Parameters.Add("@sno", sno);
                    cmd.Parameters.Add("@Username", Username);

                    vdm.Update(cmd);
                    string respnceString = GetJson("Expenditures Successfully Edited");
                    context.Response.Write(respnceString);
                }
            }
            catch (Exception ex)
            {
                string respnceString = GetJson(ex.ToString());
                context.Response.Write(respnceString);
            }
        }

        private void loaddet_save(HttpContext context)
        {
            try
            {
                vdm = new VehicleDBMgr();
                vdm.InitializeDB();
                string Username = context.Session["field1"].ToString();
                string loaddatetime = context.Request["loaddatetime"];
                string loadkms = context.Request["loadkms"];
                string loadplace = context.Request["loadplace"];
                string loaddetails = context.Request["loaddetails"];
                string tripsheetno = context.Request["tripsheetno"];
                string btnval = context.Request["btnval"];
                string sno = context.Request["sno"];
                string operationtpr = context.Request["operationtpr"];
                string commodity = context.Request["commodity"];
                string Quantity = context.Request["Quantity"];

                if (btnval == "Save")
                {

                    cmd = new MySqlCommand("Insert into expensestable(TripID, Date, Odometer, Place, LoadorExpencesDetails, InputType,Username,TransactionType, CommodityType, Quantity) values(@TripID, @Date, @Odometer, @Place, @LoadorExpencesDetails, @InputType,@Username,@TransactionType, @CommodityType, @Quantity)");
                    cmd.Parameters.Add("@TripID", tripsheetno);
                    cmd.Parameters.Add("@Date", loaddatetime);
                    cmd.Parameters.Add("@Odometer", loadkms);
                    cmd.Parameters.Add("@Place", loadplace);
                    cmd.Parameters.Add("@LoadorExpencesDetails", loaddetails);
                    cmd.Parameters.Add("@InputType", "L");
                    cmd.Parameters.Add("@Username", Username);
                    cmd.Parameters.Add("@TransactionType", operationtpr);
                    cmd.Parameters.Add("@CommodityType", commodity);
                    cmd.Parameters.Add("@Quantity", Quantity);

                    vdm.insert(cmd);
                    string respnceString = GetJson("Successfully Loaded");
                    context.Response.Write(respnceString);
                }
                else
                {
                    cmd = new MySqlCommand("Update expensestable set Date=@Date,Odometer=@Odometer,Place=@Place,LoadorExpencesDetails=@LoadorExpencesDetails,InputType=@InputType,TransactionType=@TransactionType, CommodityType=@CommodityType, Quantity=@Quantity where sno=@sno and Username=@Username");
                    // cmd.Parameters.Add("@TripID", tripsheetno);
                    cmd.Parameters.Add("@Date", loaddatetime);
                    cmd.Parameters.Add("@Odometer", loadkms);
                    cmd.Parameters.Add("@Place", loadplace);
                    cmd.Parameters.Add("@LoadorExpencesDetails", loaddetails);
                    cmd.Parameters.Add("@InputType", "L");
                    cmd.Parameters.Add("@sno", sno);
                    cmd.Parameters.Add("@Username", Username);
                    cmd.Parameters.Add("@TransactionType", operationtpr);
                    cmd.Parameters.Add("@CommodityType", commodity);
                    cmd.Parameters.Add("@Quantity", Quantity);
                    vdm.Update(cmd);
                    string respnceString = GetJson("Loading Detailes Successfully Edited");
                    context.Response.Write(respnceString);
                }
            }
            catch (Exception ex)
            {
                string respnceString = GetJson(ex.ToString());
                context.Response.Write(respnceString);
            }
        }

        private void loginauthorisation(HttpContext context)
        {
            try
            {
                vdm = new VehicleDBMgr();
                vdm.InitializeDB();
                string plant = context.Request["plant"];
                context.Session["preselectedplant"] = plant;
            }
            catch (Exception ex)
            {
            }
        }
        private void updatedivselected(HttpContext context)
        {
            try
            {
                vdm = new VehicleDBMgr();
                vdm.InitializeDB();
                string RefNo = context.Request["refno"];
                cmd = new MySqlCommand(" SELECT routesubtable.Rank, routetable.RouteName, routesubtable.LocationID, branchdata.BranchID,branchdata.Latitude, branchdata.Longitude FROM routetable INNER JOIN routesubtable ON routetable.SNo = routesubtable.SNo INNER JOIN branchdata ON routesubtable.LocationID = branchdata.Sno WHERE (routetable.SNo = @refno)");
                cmd.Parameters.Add("@refno", RefNo);
                DataTable routetabledata = vdm.SelectQuery(cmd).Tables[0];
                List<Branches> getBranchList = new List<Branches>();
                foreach (DataRow dr in routetabledata.Rows)
                {
                    Branches getBranches = new Branches();
                    getBranches.id = dr["LocationID"].ToString();
                    getBranches.Name = dr["BranchID"].ToString();
                    getBranches.latitude = dr["Latitude"].ToString();
                    getBranches.longitude = dr["Longitude"].ToString();
                    getBranchList.Add(getBranches);
                }
                string respnceString = GetJson(getBranchList);
                context.Response.Write(respnceString);
            }
            catch
            {
            }
        }

        private void updatedivselectedsales(HttpContext context)
        {
            try
            {
                vdm = new VehicleDBMgr();
                vdm.InitializeDB();
                string RefNo = context.Request["refno"];
                cmd = new MySqlCommand(" SELECT routesubtable.Rank, routetable.RouteName, routesubtable.LocationID, branchdata.BranchID,branchdata.Latitude, branchdata.Longitude, routetable.startpoint, routetable.endpoint FROM routetable INNER JOIN routesubtable ON routetable.SNo = routesubtable.SNo INNER JOIN branchdata ON routesubtable.LocationID = branchdata.Sno WHERE (routetable.SNo = @refno)");
                cmd.Parameters.Add("@refno", RefNo);
                DataTable routetabledata = vdm.SelectQuery(cmd).Tables[0];
                List<Branches> getBranchList = new List<Branches>();
                foreach (DataRow dr in routetabledata.Rows)
                {
                    Branches getBranches = new Branches();
                    getBranches.id = dr["LocationID"].ToString();
                    getBranches.Name = dr["BranchID"].ToString();
                    getBranches.latitude = dr["Latitude"].ToString();
                    getBranches.longitude = dr["Longitude"].ToString();
                    getBranches.startpoint = dr["startpoint"].ToString();
                    getBranches.endpoint = dr["endpoint"].ToString();
                    getBranchList.Add(getBranches);
                }
                string respnceString = GetJson(getBranchList);
                context.Response.Write(respnceString);
            }
            catch
            {
            }
        }
        class Routes
        {
            public string op { set; get; }
            public string RouteName { set; get; }
            public string PlantSno { set; get; }
            public List<string> data { set; get; }
            public string btnSave { set; get; }
            public string refno { set; get; }
            public string tripsno { set; get; }
            public List<jobtypescls> checkedjobcards { set; get; }
            public List<TripLogs> tripLogDetails { set; get; }
            public List<TripLogs> tripExpDetails { set; get; }
            public DateTime jobcarddate { set; get; }
            public string StartLoaction { set; get; }
            public string EndLoaction { set; get; }
        }
        public class TripLogs
        {
            public string TripDate { get; set; }
            public string Kms { get; set; }
            public string Place { get; set; }
            public string Details { get; set; }
            public string Amount { get; set; }
            public string Qty { get; set; }
            public string Diesel { get; set; }
            public string ExpenseType { get; set; }
        }
        class RouteDetail
        {
            public string LocationID { set; get; }
        }
        private void btnRoutesDeleteClick(HttpContext context)
        {
            try
            {
                vdm = new VehicleDBMgr();
                vdm.InitializeDB();
                string RefNo = context.Request["refno"];
                cmd = new MySqlCommand("delete from Routetable where SNo=@SNo");
                cmd.Parameters.Add("@SNo", RefNo);
                vdm.Delete(cmd);
                cmd = new MySqlCommand("delete from Routesubtable where SNo=@SNo");
                cmd.Parameters.Add("@SNo", RefNo);
                vdm.Delete(cmd);
                List<string> MsgList = new List<string>();
                string msg = "Data Successfully Deleted";
                MsgList.Add(msg);
                string response = GetJson(MsgList);
                context.Response.Write(response);
            }
            catch
            {
            }
        }
        private void btnRouteAssignSaveclick(HttpContext context)
        {
            try
            {
                vdm = new VehicleDBMgr();
                vdm.InitializeDB();
                var js = new JavaScriptSerializer();
                string Username = context.Session["field1"].ToString();
                var title1 = context.Request.Params[1];
                Routes obj = js.Deserialize<Routes>(title1);
                string RouteName = obj.RouteName;
                string plantsno = obj.PlantSno;
                if (obj.btnSave == "Save")
                {
                    cmd = new MySqlCommand("Insert into Routetable(RouteName,UserID,Plant) values(@RouteName,@UserID,@Plant)");
                    cmd.Parameters.Add("@RouteName", RouteName);
                    cmd.Parameters.Add("@UserID", Username);
                    cmd.Parameters.Add("@Plant", plantsno);
                    long Sno = vdm.insertScalar(cmd);
                    int i = 0;
                    foreach (string o in obj.data)
                    {
                        if (o != null)
                        {
                            cmd = new MySqlCommand("Insert into Routesubtable(SNo,LocationID,Rank) values(@SNo,@LocationID,@Rank)");
                            cmd.Parameters.Add("@SNo", Sno);
                            cmd.Parameters.Add("@LocationID", o);
                            cmd.Parameters.Add("@Rank", i);
                            vdm.insert(cmd);
                            i++;
                        }
                    }
                    List<string> MsgList = new List<string>();
                    string msg = "Data Successfully Saved";
                    MsgList.Add(msg);
                    string response = GetJson(MsgList);
                    context.Response.Write(response);
                }
                else
                {
                    string refno = obj.refno;
                    cmd = new MySqlCommand("Update Routetable set RouteName=@RouteName,Plant=@Plant where UserID=@UserID and SNo=@SNo");
                    cmd.Parameters.Add("@RouteName", RouteName);
                    cmd.Parameters.Add("@UserID", Username);
                    cmd.Parameters.Add("@Plant", plantsno);
                    cmd.Parameters.Add("@SNo", refno);
                    vdm.Update(cmd);
                    cmd = new MySqlCommand("Delete from  Routesubtable where SNo=@SNo");
                    cmd.Parameters.Add("@Sno", refno);
                    vdm.Delete(cmd);
                    int i = 0;
                    foreach (string o in obj.data)
                    {
                        if (o != null)
                        {
                            cmd = new MySqlCommand("Insert into Routesubtable(SNo,LocationID,Rank) values(@SNo,@LocationID,@Rank)");
                            cmd.Parameters.Add("@SNo", refno);
                            cmd.Parameters.Add("@LocationID", o);
                            cmd.Parameters.Add("@Rank", i);
                            vdm.insert(cmd);
                            i++;
                        }
                    }
                    List<string> MsgList = new List<string>();
                    string msg = "Data Successfully Updated";
                    MsgList.Add(msg);
                    string response = GetJson(MsgList);
                    context.Response.Write(response);
                }
            }
            catch
            {
            }
        }


        private void btnRouteSalesAssignSaveclick(HttpContext context)
        {
            try
            {
                vdm = new VehicleDBMgr();
                vdm.InitializeDB();
                var js = new JavaScriptSerializer();
                string Username = context.Session["field1"].ToString();
                var title1 = context.Request.Params[1];
                Routes obj = js.Deserialize<Routes>(title1);
                string RouteName = obj.RouteName;
                string plantsno = obj.PlantSno;
                string startpoint = obj.StartLoaction;
                string endpoint = obj.EndLoaction;
                if (obj.btnSave == "Save")
                {
                    cmd = new MySqlCommand("Insert into Routetable(RouteName,UserID,Plant,startpoint,endpoint) values(@RouteName,@UserID,@Plant,@startpoint,@endpoint)");
                    cmd.Parameters.Add("@RouteName", RouteName);
                    cmd.Parameters.Add("@UserID", Username);
                    cmd.Parameters.Add("@Plant", plantsno);
                    cmd.Parameters.Add("@startpoint", startpoint);
                    cmd.Parameters.Add("@endpoint", endpoint);
                    long Sno = vdm.insertScalar(cmd);
                    int i = 0;
                    foreach (string o in obj.data)
                    {
                        if (o != null)
                        {
                            cmd = new MySqlCommand("Insert into Routesubtable(SNo,LocationID,Rank) values(@SNo,@LocationID,@Rank)");
                            cmd.Parameters.Add("@SNo", Sno);
                            cmd.Parameters.Add("@LocationID", o);
                            cmd.Parameters.Add("@Rank", i);
                            vdm.insert(cmd);
                            i++;
                        }
                    }
                    List<string> MsgList = new List<string>();
                    string msg = "Data Successfully Saved";
                    MsgList.Add(msg);
                    string response = GetJson(MsgList);
                    context.Response.Write(response);
                }
                else
                {
                    string refno = obj.refno;
                    cmd = new MySqlCommand("Update Routetable set RouteName=@RouteName,Plant=@Plant,startpoint=@startpoint,endpoint=@endpoint where UserID=@UserID and SNo=@SNo");
                    cmd.Parameters.Add("@RouteName", RouteName);
                    cmd.Parameters.Add("@UserID", Username);
                    cmd.Parameters.Add("@Plant", plantsno);
                    cmd.Parameters.Add("@SNo", refno);
                    cmd.Parameters.Add("@startpoint", startpoint);
                    cmd.Parameters.Add("@endpoint", endpoint);
                    vdm.Update(cmd);
                    cmd = new MySqlCommand("Delete from  Routesubtable where SNo=@SNo");
                    cmd.Parameters.Add("@Sno", refno);
                    vdm.Delete(cmd);
                    int i = 0;
                    foreach (string o in obj.data)
                    {
                        if (o != null)
                        {
                            cmd = new MySqlCommand("Insert into Routesubtable(SNo,LocationID,Rank) values(@SNo,@LocationID,@Rank)");
                            cmd.Parameters.Add("@SNo", refno);
                            cmd.Parameters.Add("@LocationID", o);
                            cmd.Parameters.Add("@Rank", i);
                            vdm.insert(cmd);
                            i++;
                        }
                    }
                    List<string> MsgList = new List<string>();
                    string msg = "Data Successfully Updated";
                    MsgList.Add(msg);
                    string response = GetJson(MsgList);
                    context.Response.Write(response);
                }
            }
            catch
            {
            }
        }
      
        private void updateroutesAssigntogrid(HttpContext context)
        {
            try
            {
                vdm = new VehicleDBMgr();
                vdm.InitializeDB();
                string Username = context.Session["field1"].ToString();
                cmd = new MySqlCommand("SELECT routetable.RouteName, routetable.SNo, routetable.Plant, branchdata.BranchID AS PlantName FROM routetable LEFT OUTER JOIN branchdata ON routetable.Plant = branchdata.Sno WHERE (routetable.UserID = @UserName)");
                //cmd = new MySqlCommand("SELECT RouteName,SNo,Plant FROM routetable where UserID=@UserName");
                cmd.Parameters.Add("@UserName", Username);
                DataTable routetabledata = vdm.SelectQuery(cmd).Tables[0];
                List<RouteAsign> getroutetabledataList = new List<RouteAsign>();
                foreach (DataRow dr in routetabledata.Rows)
                {
                    RouteAsign GetRoutes = new RouteAsign();

                    GetRoutes.SNo = dr["SNo"].ToString();
                    GetRoutes.RouteName = dr["RouteName"].ToString();
                    GetRoutes.PlantName = dr["PlantName"].ToString();
                    GetRoutes.PlantSno = dr["Plant"].ToString();
                    getroutetabledataList.Add(GetRoutes);
                }
                string respnceString = GetJson(getroutetabledataList);
                context.Response.Write(respnceString);
            }
            catch
            {
            }
        }
        class RouteAsign
        {
            public string SNo { get; set; }
            public string RouteName { get; set; }
            public string PlantName { get; set; }
            public string PlantSno { get; set; }
        }
        private void get_Routes(HttpContext context)
        {
            try
            {
                vdm = new VehicleDBMgr();
                vdm.InitializeDB();
                string Username = context.Session["field1"].ToString();
                cmd = new MySqlCommand("SELECT branchdata.Sno, branchdata.BranchID, branchdata.Latitude, branchdata.Longitude, branchdata_1.BranchID AS PlantName, branchdata_1.Sno AS PlantSno FROM branchdata INNER JOIN branchdata branchdata_1 ON branchdata.PlantName = branchdata_1.Sno AND branchdata.UserName = branchdata_1.UserName WHERE (branchdata.UserName = @UserName)");
                // cmd = new MySqlCommand("SELECT branchdata.BranchID, branchdata.Description, branchdata.Latitude, branchdata.Longitude, branchdata.PhoneNumber, branchdata.ImagePath, branchdata.ImageType, branchdata.Radious, branchdata.UserName FROM loginstable INNER JOIN branchdata ON loginstable.main_user = branchdata.UserName WHERE (loginstable.loginid = @UserName)");
                cmd.Parameters.Add("@UserName", Username);
                DataTable Branchdata = vdm.SelectQuery(cmd).Tables[0];
                List<Branches> getBranchList = new List<Branches>();
                foreach (DataRow dr in Branchdata.Rows)
                {
                    Branches getBranches = new Branches();
                    getBranches.id = dr["Sno"].ToString();
                    getBranches.Name = dr["BranchID"].ToString();
                    getBranches.latitude = dr["Latitude"].ToString();
                    getBranches.longitude = dr["Longitude"].ToString();
                    getBranches.PlantName = dr["PlantName"].ToString();
                    getBranches.PlantSno = dr["PlantSno"].ToString();
                    getBranchList.Add(getBranches);
                }
                string respnceString = GetJson(getBranchList);
                context.Response.Write(respnceString);
            }
            catch
            {
            }
        }


        private void get_routes_trips(HttpContext context)
        {
            try
            {
                vdm = new VehicleDBMgr();
                vdm.InitializeDB();
                string Username = context.Session["field1"].ToString();
                string Plantid = context.Request["Plantid"];
                cmd = new MySqlCommand("SELECT        tripconfiguration.TripName, tripconfiguration.RouteID, tripconfiguration.PlantName, tripconfiguration.Kms, tripconfiguration.extrakms, tripconfiguration.Chargeperkm, branchdata.BranchID, routetable.RouteName, branchdata.Latitude, branchdata.Longitude FROM            tripconfiguration INNER JOIN routetable ON tripconfiguration.RouteID = routetable.SNo INNER JOIN routesubtable ON routetable.SNo = routesubtable.SNo INNER JOIN branchdata ON routesubtable.LocationID = branchdata.Sno WHERE        (tripconfiguration.PlantName = @rid) AND (routesubtable.Rank = 0)");
                // cmd = new MySqlCommand("SELECT branchdata.BranchID, branchdata.Description, branchdata.Latitude, branchdata.Longitude, branchdata.PhoneNumber, branchdata.ImagePath, branchdata.ImageType, branchdata.Radious, branchdata.UserName FROM loginstable INNER JOIN branchdata ON loginstable.main_user = branchdata.UserName WHERE (loginstable.loginid = @UserName)");
                cmd.Parameters.Add("@rid", Plantid);
                DataTable Branchdata = vdm.SelectQuery(cmd).Tables[0];
                List<routes_trips> getBranchList = new List<routes_trips>();
                foreach (DataRow dr in Branchdata.Rows)
                {
                    routes_trips get_routes_trips = new routes_trips();
                    get_routes_trips.TripName = dr["TripName"].ToString();
                    get_routes_trips.RouteID = dr["RouteID"].ToString();
                    get_routes_trips.PlantName = dr["PlantName"].ToString();
                    get_routes_trips.Kms = dr["Kms"].ToString();
                    get_routes_trips.PlantName = dr["PlantName"].ToString();
                    get_routes_trips.Chargeperkm = dr["Chargeperkm"].ToString();
                    get_routes_trips.emptykms = dr["extrakms"].ToString();
                    //   branchdata.BranchID, routetable.RouteName, branchdata.Latitude, branchdata.Longitude
                    get_routes_trips.BranchID = dr["BranchID"].ToString();
                    get_routes_trips.RouteName = dr["RouteName"].ToString();
                    get_routes_trips.Latitude = dr["Latitude"].ToString();
                    get_routes_trips.Longitude = dr["Longitude"].ToString();
                    getBranchList.Add(get_routes_trips);
                }
                string respnceString = GetJson(getBranchList);
                context.Response.Write(respnceString);
            }
            catch
            {
            }
        }
        class Branches
        {
            public string id { get; set; }
            public string Name { get; set; }
            public string latitude { get; set; }
            public string longitude { get; set; }
            public string PlantName { get; set; }
            public string PlantSno { get; set; }
            public string startpoint { get; set; }
            public string endpoint { get; set; }
            public string RouteName { get; set; }
            public string RouteSno { get; set; }
        }

        class routes_trips
        {
            //tripconfiguration.TripName, tripconfiguration.RouteID, tripconfiguration.PlantName, tripconfiguration.Kms, tripconfiguration.extrakms, tripconfiguration.Chargeperkm, branchdata.BranchID, routetable.RouteName, branchdata.Latitude, branchdata.Longitude
            public string TripName { get; set; }
            public string RouteID { get; set; }
            public string PlantName { get; set; }
            public string Kms { get; set; }
            public string emptykms { get; set; }
            public string Chargeperkm { get; set; }
            public string BranchID { get; set; }
            public string RouteName { get; set; }
            public string Latitude { get; set; }
            public string Longitude { get; set; }
        }
        private void remarkssave(HttpContext context)
        {
            try
            {
                vdm = new VehicleDBMgr();
                vdm.InitializeDB();
                string Username = context.Session["field1"].ToString();
                string vehicleno = context.Request["vehicleno"];
                string remarks = context.Request["remarks"];
                DateTime now = DateTime.Now;
                cmd = new MySqlCommand("insert into vehicleremarks (vehicleno, remarks, datetime, username) values (@vehicleno, @remarks, @datetime, @username)");
                cmd.Parameters.Add("@vehicleno", vehicleno);
                cmd.Parameters.Add("@remarks", remarks);
                cmd.Parameters.Add("@datetime", DateTime.Now);
                cmd.Parameters.Add("@username", Username);
                vdm.insert(cmd);
                string msg = "Data successfully saved";
                string respnceString = GetJson(msg);
                context.Response.Write(respnceString);
            }
            catch
            {
            }
        }
        private void BtnGenerateclick(HttpContext context)
        {
            try
            {
                vdm = new VehicleDBMgr();
                vdm.InitializeDB();
                string Username = context.Session["field1"].ToString();
                string Fromdate = context.Request["Fromdate"];
                string Todate = context.Request["todate"];
                cmd = new MySqlCommand("SELECT  tripdata.Refno, tripdata.Tripid, tripdata.assigndate, tripdata.completdate, tripdata.Status, tripdata.UserID, tripdata.RouteName, branchdata.BranchID, tripsubdata.rank,  tripsubdata.intime, paireddata.VehicleNumber, tripdata.vehiclemaster_sno FROM  tripdata INNER JOIN  tripsubdata ON tripdata.Refno = tripsubdata.Refno INNER JOIN branchdata ON tripsubdata.locid = branchdata.Sno INNER JOIN paireddata ON tripdata.vehiclemaster_sno = paireddata.Sno WHERE   (tripdata.UserID = @UserID) AND (tripdata.assigndate >= @d1) AND (tripdata.assigndate <= @d2)ORDER BY tripdata.Refno, tripsubdata.rank");
                cmd.Parameters.Add("@UserID", Username);
                cmd.Parameters.Add("@d1", Fromdate);
                cmd.Parameters.Add("@d2", Todate);
                DataTable dtStatustrip = vdm.SelectQuery(cmd).Tables[0];
                if (dtStatustrip.Rows.Count > 0)
                {
                    List<Tripclass> GeStatustriplist = new List<Tripclass>();
                    DataTable trips = dtStatustrip.DefaultView.ToTable(true, "Refno", "Tripid", "VehicleNumber", "RouteName", "assigndate");
                    foreach (DataRow drTrip in trips.Rows)
                    {
                        string VehicleNo = drTrip["VehicleNumber"].ToString();
                        Tripclass GetTrip = new Tripclass();
                        GetTrip.TripName = drTrip["Tripid"].ToString();
                        GetTrip.VehicleNo = VehicleNo;
                        GetTrip.RouteName = drTrip["RouteName"].ToString();
                        GetTrip.Assigndate = drTrip["assigndate"].ToString();
                        List<SubTripclass> SubTriplist = new List<SubTripclass>();
                        foreach (DataRow dr in dtStatustrip.Select("Refno=" + drTrip["Refno"].ToString()))
                        {
                            SubTripclass GetsubTrip = new SubTripclass();
                            GetsubTrip.Sno = dr["rank"].ToString();
                            string BranchName = dr["BranchID"].ToString();
                            GetsubTrip.VehicleNo = dr["VehicleNumber"].ToString();
                            GetsubTrip.LocationName = BranchName;
                            GetsubTrip.EnterTime = dr["intime"].ToString();
                            SubTriplist.Add(GetsubTrip);
                        }
                        GetTrip.SubTriplist = SubTriplist;
                        GeStatustriplist.Add(GetTrip);
                    }
                    string response = GetJson(GeStatustriplist);
                    context.Response.Write(response);
                }
                else
                {
                    List<string> msglist = new List<string>();
                    string msg = "No data were found";
                    msglist.Add(msg);
                    string respnceString = GetJson(msglist);
                    context.Response.Write(respnceString);
                }
            }
            catch
            {
            }
        }
        DataTable dtTrip;
        private void GetTripdata(HttpContext context)
        {
            try
            {
                vdm = new VehicleDBMgr();
                vdm.InitializeDB();
                string Username = context.Session["field1"].ToString();
                string plant = context.Request["plant"].ToString();
                List<Tripclass> Getriplist = new List<Tripclass>();
                DateTime todaydate = DateTime.Now;
                string mainuser = "";
                if (context.Session["main_user"] == null)
                {
                    cmd = new MySqlCommand("SELECT main_user FROM loginstable WHERE (loginid = @loginid)");
                    cmd.Parameters.Add("@loginid", Username);
                    DataTable mainusertbl = vdm.SelectQuery(cmd).Tables[0];
                    if (mainusertbl.Rows.Count > 0)
                    {
                        mainuser = mainusertbl.Rows[0]["main_user"].ToString();
                        context.Session["main_user"] = mainuser;
                    }
                    else
                    {
                        mainuser = Username;
                    }
                }
                else
                {
                    mainuser = context.Session["main_user"].ToString();
                }
                if (plant != "ALL")
                {
                    DateTime presentdt = DateTime.Now;
                    DateTime fromdate = DateTime.Today;
                    DateTime todate = DateTime.Today;
                    string totaydt = fromdate.ToString("dd-MM-yyyy HH:mm");
                    string[] datestrig = totaydt.Split(' ');
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
                        return;
                    }
                    if (presentdt > fromdate.AddHours(4).AddMinutes(30) && presentdt < fromdate.AddHours(12).AddMinutes(30))
                    {
                        fromdate = fromdate.AddHours(1).AddMinutes(30);
                        todate = todate.AddHours(12).AddMinutes(30);
                    }
                    else if (presentdt > fromdate.AddHours(15).AddMinutes(30) && presentdt < fromdate.AddHours(23).AddMinutes(59))
                    {
                        fromdate = fromdate.AddHours(15).AddMinutes(30);
                        todate = todate.AddHours(23).AddMinutes(59);
                    }

                    cmd = new MySqlCommand("SELECT tripconfiguration.TripName, tripconfiguration.StartTime, tripconfiguration.EndTime, tripconfiguration.RouteID, tripconfiguration.Status, tripconfiguration.UserID, tripconfiguration.PlantName, paireddata.VehicleNumber FROM tripconfiguration INNER JOIN paireddata ON tripconfiguration.Veh_Sno = paireddata.Sno WHERE (tripconfiguration.UserID = @UserID) AND (tripconfiguration.PlantName = @PlantName)");
                    cmd.Parameters.Add("@UserID", Username);
                    cmd.Parameters.Add("@PlantName", plant);
                    dtTrip = vdm.SelectQuery(cmd).Tables[0];
                    DataTable vehicles = dtTrip.DefaultView.ToTable(true, "VehicleNumber");
                    cmd = new MySqlCommand("SELECT routesubtable.LocationID, routesubtable.Rank, branchdata.BranchID, branchdata.Latitude, branchdata.Longitude, branchdata.Radious,routetable.SNo,routetable.RouteName FROM routesubtable INNER JOIN routetable ON routesubtable.SNo = routetable.SNo INNER JOIN branchdata ON routesubtable.LocationID = branchdata.Sno WHERE (routetable.UserID = @UserID) and (routetable.Plant=@Plant)");
                    cmd.Parameters.Add("@UserID", Username);
                    cmd.Parameters.Add("@Plant", plant);
                    DataTable routebranches = vdm.SelectQuery(cmd).Tables[0];
                    foreach (DataRow dr in vehicles.Rows)
                    {
                        string vehno = dr["VehicleNumber"].ToString();
                        DataRow[] trips = dtTrip.Select("VehicleNumber='" + vehno + "'");
                        if (trips.Length > 0)
                        {
                            foreach (DataRow trip in trips)
                            {
                                DateTime startdt = DateTime.Today;
                                DateTime enddt = DateTime.Today;
                                startdt = startdt.AddHours(int.Parse(trip["StartTime"].ToString().Split(':')[0])).AddMinutes(int.Parse(trip["StartTime"].ToString().Split(':')[1]));
                                enddt = enddt.AddHours(int.Parse(trip["EndTime"].ToString().Split(':')[0])).AddMinutes(int.Parse(trip["EndTime"].ToString().Split(':')[1]));
                                if (fromdate < startdt && todate > enddt)
                                {
                                    string routeid = trip["RouteID"].ToString();
                                    DataRow[] route = routebranches.Select("SNo='" + routeid + "'");
                                    if (route.Length > 0)
                                    {
                                        #region location wise Reports
                                        DateTime Startingdt = DateTime.Now;
                                        int sno = 1;
                                        DataTable summeryTable = new DataTable();
                                        DataColumn summeryColumn = new DataColumn("SNo");
                                        summeryTable.Columns.Add(summeryColumn);
                                        summeryColumn = new DataColumn("FromLocation");
                                        summeryTable.Columns.Add(summeryColumn);
                                        summeryTable.Columns.Add("ReachingDate").DataType = typeof(DateTime);
                                        summeryColumn = new DataColumn("ReachingTime");
                                        summeryTable.Columns.Add(summeryColumn);
                                        summeryColumn = new DataColumn("Distance(Kms)");
                                        summeryTable.Columns.Add(summeryColumn);
                                        summeryColumn = new DataColumn("JourneyHours");
                                        summeryTable.Columns.Add(summeryColumn);
                                        DataRow summeryRow = null;
                                        DateTime prevdate = DateTime.Now;
                                        double presodometer = 0;
                                        List<string> logstbls = new List<string>();
                                        logstbls.Add("GpsTrackVehicleLogs");
                                        logstbls.Add("GpsTrackVehicleLogs1");
                                        logstbls.Add("GpsTrackVehicleLogs2");
                                        logstbls.Add("GpsTrackVehicleLogs3");
                                        DataTable logs = new DataTable();
                                        DataTable tottable = new DataTable();
                                        foreach (string tbname in logstbls)
                                        {
                                            cmd = new MySqlCommand("SELECT '' AS SNo, " + tbname + ".VehicleID, " + tbname + ".DateTime, " + tbname + ".Speed, " + tbname + ".Latitiude, " + tbname + ".Longitude, " + tbname + ".Direction, " + tbname + ".Diesel, " + tbname + ".Odometer, " + tbname + ".Direction AS Expr1, " + tbname + ".Direction AS Expr2, vehiclemaster.MaintenancePlantName, vehiclemaster.VendorName, vehiclemaster.VendorNo, vehiclemaster.VehicleTypeName FROM " + tbname + " LEFT OUTER JOIN vehiclemaster ON " + tbname + ".VehicleID = vehiclemaster.VehicleID WHERE (" + tbname + ".DateTime >= @starttime) AND (" + tbname + ".DateTime <= @endtime) AND (" + tbname + ".VehicleID = '" + vehno + "') and (" + tbname + ".UserID='" + mainuser + "')  ORDER BY " + tbname + ".DateTime");
                                            //cmd = new MySqlCommand("select * from " + tbname + " where DateTime>= @starttime and DateTime<=@endtime and VehicleID='" + vehiclestr + "' and UserID='" + UserName + "' order by DateTime");
                                            cmd.Parameters.Add(new MySqlParameter("@starttime", fromdate));
                                            cmd.Parameters.Add(new MySqlParameter("@endtime", todate));
                                            logs = vdm.SelectQuery(cmd).Tables[0];
                                            if (tottable.Rows.Count == 0)
                                            {
                                                tottable = logs.Clone();
                                            }
                                            foreach (DataRow dr1 in logs.Rows)
                                            {
                                                tottable.ImportRow(dr1);
                                            }
                                        }
                                        DataView dv = tottable.DefaultView;
                                        dv.Sort = "DateTime ASC";
                                        DataTable TripData = dv.ToTable();
                                        DataRow Prevrow = null;
                                        summeryRow = null;

                                        Dictionary<string, string> statusobserver = new Dictionary<string, string>();
                                        foreach (DataRow brnch in route)
                                        {
                                            statusobserver.Add(brnch["BranchID"].ToString(), "In Side");
                                        }
                                        //string VehicleNo = drTrip["VehicleNumber"].ToString();
                                        Tripclass GetTrip = new Tripclass();
                                        GetTrip.TripName = trip["TripName"].ToString();
                                        GetTrip.VehicleNo = vehno;
                                        GetTrip.RouteName = route[0]["RouteName"].ToString();
                                        //GetTrip.Assigndate = drTrip["assigndate"].ToString();
                                        List<SubTripclass> SubTriplist = new List<SubTripclass>();
                                        for (int cnt = 0; cnt < route.Length; cnt++)
                                        {
                                            foreach (DataRow tripdatarow in TripData.Rows)
                                            {
                                                DataRow Brncs = route[cnt];
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
                                                    summeryRow["FromLocation"] = Brncs["BranchID"];
                                                    string Reachdate = Reachingdt.ToString();
                                                    string ReachTime = Reachingdt.ToString("HH:mm");
                                                    summeryRow["ReachingDate"] = Reachingdt.ToString();
                                                    summeryRow["ReachingTime"] = ReachTime.ToString();
                                                    presodometer = double.Parse(tripdatarow["Odometer"].ToString());
                                                    summeryRow["Distance(Kms)"] = presodometer;
                                                    summeryTable.Rows.Add(summeryRow);

                                                    break;
                                                }
                                                Prevrow = tripdatarow;
                                            }
                                        }
                                        for (int cnt = 0; cnt < route.Length; cnt++)
                                        {
                                            DataRow[] dra = summeryTable.Select("FromLocation='" + route[cnt]["BranchID"].ToString() + "'");
                                            cmd = new MySqlCommand("SELECT routesubtable.LocationID, routesubtable.Rank, branchdata.BranchID, branchdata.Latitude, branchdata.Longitude, branchdata.Radious,routetable.SNo,routetable.RouteName FROM routesubtable INNER JOIN routetable ON routesubtable.SNo = routetable.SNo INNER JOIN branchdata ON routesubtable.LocationID = branchdata.Sno WHERE (routetable.UserID = @UserID) and (routetable.Plant=@Plant)");
                                            SubTripclass GetsubTrip = new SubTripclass();
                                            GetsubTrip.Sno = route[cnt]["rank"].ToString();
                                            string BranchName = route[cnt]["BranchID"].ToString();
                                            GetsubTrip.VehicleNo = vehno;
                                            GetsubTrip.LocationName = BranchName;
                                            if (dra.Length > 0)
                                            {
                                                GetsubTrip.EnterTime = dra[0]["ReachingDate"].ToString();
                                            }
                                            SubTriplist.Add(GetsubTrip);
                                        }
                                        GetTrip.SubTriplist = SubTriplist;
                                        Getriplist.Add(GetTrip);
                                        #endregion
                                    }
                                }
                            }
                        }
                    }
                }
                DataTable plants = new DataTable();
                if (context.Session["allplants"] == null)
                {
                    cmd = new MySqlCommand("SELECT SNo,UserName, BranchID, Description, Latitude, Longitude, PhoneNumber, ImagePath, ImageType, Radious, PlantName, IsPlant FROM branchdata WHERE (UserName = @UserName) AND (IsPlant = '1')");
                    cmd.Parameters.Add("@UserName", Username);
                    plants = vdm.SelectQuery(cmd).Tables[0];
                    context.Session["allplants"] = plants;
                }
                else
                {
                    plants = (DataTable)context.Session["allplants"];
                }
                List<tripplants> allplants = new List<tripplants>();
                foreach (DataRow drr in plants.Rows)
                {
                    tripplants plnts = new tripplants();
                    plnts.plantname = drr["BranchID"].ToString();
                    plnts.plantsno = (int)drr["SNo"];
                    allplants.Add(plnts);
                }
                Tripclass gtTrip = new Tripclass();
                gtTrip.allplants = allplants;
                Getriplist.Add(gtTrip);
                if (Getriplist != null)
                {
                    string response = GetJson(Getriplist);
                    context.Response.Write(response);
                }
            }
            catch
            {
            }
        }

        public class aaa
        {
            public List<Tripclass> tc { set; get; }
        }
        public class tripplants
        {
            public int plantsno { set; get; }
            public string plantname { set; get; }
        }
        public class Tripclass
        {
            public string TripName { get; set; }
            public string VehicleNo { get; set; }
            public string RouteName { get; set; }
            public string Assigndate { get; set; }
            public string EndTime { get; set; }
            public List<SubTripclass> SubTriplist { set; get; }
            public List<tripplants> allplants { set; get; }

        }
        private void GetSubTripdata(HttpContext context)
        {
            try
            {
                vdm = new VehicleDBMgr();
                vdm.InitializeDB();
                List<SubTripclass> SubTriplist = new List<SubTripclass>();
                if (context.Session["SubTriplist"] != null)
                {
                    SubTriplist = (List<SubTripclass>)context.Session["SubTriplist"];
                }
                else
                {
                    if (SubTriplist.Count == 0)
                    {
                        foreach (DataRow drTrip in dtTrip.Rows)
                        {
                            int TripRefNo = (int)drTrip["Refno"];
                            cmd = new MySqlCommand("Select * from tripsubdata where Refno=@Refno order by Rank");
                            cmd.Parameters.Add("@Refno", TripRefNo);
                            DataTable dtSubtrip = vdm.SelectQuery(cmd).Tables[0];
                            cmd = new MySqlCommand("Select VehicleNumber from paireddata where Sno='" + drTrip["Vehiclemaster_sno"].ToString() + "'");
                            DataTable dtVehicleNo = vdm.SelectQuery(cmd).Tables[0];
                            string VehicleNo = dtVehicleNo.Rows[0]["VehicleNumber"].ToString();
                            foreach (DataRow drSubTrip in dtSubtrip.Rows)
                            {
                                SubTripclass GetsubTrip = new SubTripclass();
                                GetsubTrip.Sno = drSubTrip["rank"].ToString();
                                cmd = new MySqlCommand("Select BranchID from branchdata where Sno='" + drSubTrip["locid"].ToString() + "'");
                                DataTable dtBranchName = vdm.SelectQuery(cmd).Tables[0];
                                string BranchName = dtBranchName.Rows[0]["BranchID"].ToString();
                                GetsubTrip.VehicleNo = VehicleNo;
                                GetsubTrip.LocationName = BranchName;
                                GetsubTrip.EnterTime = drSubTrip["intime"].ToString();
                                SubTriplist.Add(GetsubTrip);
                            }
                        }
                        context.Session["SubTriplist"] = SubTriplist;
                    }
                }
                string response = GetJson(SubTriplist);
                context.Response.Write(response);
            }
            catch
            {
            }
        }
        public class SubTripclass
        {
            public string VehicleNo { get; set; }
            public string Sno { get; set; }
            public string LocationName { get; set; }
            public string EnterTime { get; set; }
        }
        private void BtnTripSaveClick(HttpContext context)
        {
            try
            {
                List<string> VehicleManageDet = new List<string>();
                string ddlVehicleno = context.Request["ddlVehicleno"];
                context.Session["Vehicleno"] = ddlVehicleno;
                string txtAdate = context.Request["txtAdate"];
                string ddlStatus = context.Request["ddlStatus"];
                string ddlRouteName = context.Request["ddlRouteName"];
                context.Session["RouteName"] = ddlRouteName;
                string jsgrid = context.Request["all"];
                context.Session["jsgrid"] = jsgrid;
                string msg = "Trip Assigned Successfully";
                VehicleManageDet.Add(msg);
                string response = GetJson(VehicleManageDet);
                context.Response.Write(response);
            }
            catch
            {
            }
        }
        private void ddlRouteNameChange(HttpContext context)
        {
            try
            {
                vdm = new VehicleDBMgr();
                vdm.InitializeDB();
                string Username = context.Session["field1"].ToString();
                string RoteName = context.Request["RoteName"];
                List<GetTripClass> GetTripList = new List<GetTripClass>();
                cmd = new MySqlCommand("select SNo from Routetable where UserID=@UserName and RouteName=@RouteName");
                cmd.Parameters.Add("@UserName", Username);
                cmd.Parameters.Add("@RouteName", RoteName);
                DataTable dtSno = vdm.SelectQuery(cmd).Tables[0];
                string SNo = dtSno.Rows[0]["SNo"].ToString();
                //cmd = new MySqlCommand("select SNo,LocationID,BranchName,Rank from Routesubtable where SNo=@SNo");
                cmd = new MySqlCommand("SELECT routesubtable.SNo, routesubtable.Rank, routesubtable.LocationID, branchdata.BranchID AS BranchName FROM routesubtable INNER JOIN branchdata ON routesubtable.LocationID = branchdata.Sno WHERE (routesubtable.SNo = @SNo)");
                cmd.Parameters.Add("@SNo", SNo);
                DataTable dtRoutesubtable = vdm.SelectQuery(cmd).Tables[0];
                foreach (DataRow dr in dtRoutesubtable.Rows)
                {
                    GetTripClass GetTrip = new GetTripClass();
                    GetTrip.Sno = dr["SNo"].ToString();
                    GetTrip.LocationID = dr["LocationID"].ToString();
                    GetTrip.BranchName = dr["BranchName"].ToString();
                    GetTrip.Rank = dr["Rank"].ToString();
                    GetTripList.Add(GetTrip);
                }
                string response = GetJson(GetTripList);
                context.Response.Write(response);

            }
            catch
            {
            }
        }
        public class GetTripClass
        {
            public string Sno { get; set; }
            public string LocationID { get; set; }
            public string Rank { get; set; }
            public string BranchName { get; set; }
        }
        private void GetVehicleNos(HttpContext context)
        {
            try
            {
                vdm = new VehicleDBMgr();
                vdm.InitializeDB();
                string Username = context.Session["field1"].ToString();
                cmd = new MySqlCommand("select VehicleNumber from PairedData where UserID=@UserName");
                cmd.Parameters.Add("@UserName", Username);
                DataTable dtVehicleno = vdm.SelectQuery(cmd).Tables[0];
                GetRouteNameValues GetVehicle = new GetRouteNameValues();
                List<string> fillVehicle = new List<string>();
                foreach (DataRow dr in dtVehicleno.Rows)
                {
                    fillVehicle.Add(dr["VehicleNumber"].ToString());
                }
                GetVehicle.VehicleNo = fillVehicle;
                List<GetRouteNameValues> Routelist = new List<GetRouteNameValues>();
                Routelist.Add(GetVehicle);
                string response = GetJson(Routelist);
                context.Response.Write(response);
            }
            catch
            {
            }
        }
        private void GetRouteValues(HttpContext context)
        {
            vdm = new VehicleDBMgr();
            vdm.InitializeDB();
            string Username = context.Session["field1"].ToString();
            if (context.Session["Branchdata"] == null)
            {
                cmd = new MySqlCommand("SELECT Sno,BranchID,Latitude,Longitude FROM branchdata where UserName=@UserName");
                cmd.Parameters.Add("@UserName", Username);
                DataTable Branchdata = vdm.SelectQuery(cmd).Tables[0];
                context.Session["Branchdata"] = Branchdata;
            }
            string Locations = context.Request["Locations"];
            DataTable dtlocations = (DataTable)context.Session["Branchdata"];
            List<GetLocations> GetLocationsList = new List<GetLocations>();
            foreach (string str in Locations.Split(','))
            {
                foreach (DataRow dr in dtlocations.Rows)
                {
                    string loc1 = str;
                    char[] charsToTrim = { ' ' };
                    loc1 = loc1.Trim(charsToTrim);
                    string loc2 = dr["Sno"].ToString();
                    if (loc1 == loc2)
                    {
                        GetLocations GetLocation = new GetLocations();
                        GetLocation.Sno = dr["Sno"].ToString();
                        GetLocation.BranchName = dr["BranchID"].ToString();
                        GetLocation.latitude = dr["Latitude"].ToString();
                        GetLocation.longitude = dr["Longitude"].ToString();
                        GetLocationsList.Add(GetLocation);
                    }
                }
            }
            if (GetLocationsList.Count != 0)
            {
                string respnceString = GetJson(GetLocationsList);
                context.Response.Write(respnceString);
            }
        }
        public class GetLocations
        {
            public string Sno { get; set; }
            public string BranchName { get; set; }
            public string latitude { get; set; }
            public string longitude { get; set; }
        }
        private void GetRouteNames(HttpContext context)
        {
            try
            {
                vdm = new VehicleDBMgr();
                vdm.InitializeDB();
                string Username = context.Session["field1"].ToString();
                cmd = new MySqlCommand("Select RouteName from Routetable where UserID=@UserID");
                cmd.Parameters.Add("@UserID", Username);
                DataTable dtRoute = vdm.SelectQuery(cmd).Tables[0];
                GetRouteNameValues GetRouteName = new GetRouteNameValues();
                List<string> fillRoute = new List<string>();
                foreach (DataRow dr in dtRoute.Rows)
                {
                    fillRoute.Add(dr["RouteName"].ToString());
                }
                cmd = new MySqlCommand("Select Sno,BranchID from branchdata where UserName=@UserName");
                cmd.Parameters.Add("@UserName", Username);
                DataTable dtLocations = vdm.SelectQuery(cmd).Tables[0];
                Dictionary<string, string> fillLocations = new Dictionary<string, string>();
                foreach (DataRow dr in dtLocations.Rows)
                {
                    fillLocations.Add(dr["BranchID"].ToString(), dr["Sno"].ToString());
                }
                GetRouteName.RouteNames = fillRoute;
                GetRouteName.Locations = fillLocations;
                List<GetRouteNameValues> Routelist = new List<GetRouteNameValues>();
                Routelist.Add(GetRouteName);
                string response = GetJson(Routelist);
                context.Response.Write(response);
            }
            catch
            {
            }
        }
        public class GetRouteNameValues
        {
            public List<string> RouteNames { get; set; }
            public Dictionary<string, string> Locations { get; set; }
            public List<string> VehicleNo { get; set; }
        }

        public class NearestVehicle
        {
            public string Vehicleno { get; set; }
            public string latitude { get; set; }
            public string longitude { get; set; }
            public string Distance { get; set; }
            public string ExpectedTime { get; set; }
        }
        double vehdistance = 0;
        public double GetDistanceBetweenPoints(double Lat1, double Long1, double Lat2, double Long2)
        {
            vehdistance = 0;

            double dLat1InRad = Lat1 * (Math.PI / 180.0);
            double dLong1InRad = Long1 * (Math.PI / 180.0);
            double dLat2InRad = Lat2 * (Math.PI / 180.0);
            double dLong2InRad = Long2 * (Math.PI / 180.0);

            double dLongitude = dLong2InRad - dLong1InRad;
            double dLatitude = dLat2InRad - dLat1InRad;

            // Intermediate result a.
            double a = Math.Pow(Math.Sin(dLatitude / 2.0), 2.0) +
            Math.Cos(dLat1InRad) * Math.Cos(dLat2InRad) *
            Math.Pow(Math.Sin(dLongitude / 2.0), 2.0);

            // Intermediate result c (great circle distance in Radians).
            double c = 2.0 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1.0 - a));

            // Distance.
            // const Double kEarthRadiusMiles = 3956.0;
            const Double kEarthRadiusKms = 6376.5;
            vehdistance = kEarthRadiusKms * c;
            return (vehdistance);
        }
        DataTable vehdata = new DataTable();
        private void getNearestVehicle(HttpContext context)
        {
            vehdata = new DataTable();
            List<NearestVehicle> NearestVehiclelist = new List<NearestVehicle>();
            vdm = new VehicleDBMgr();
            vdm.InitializeDB();
            string Username = context.Session["field1"].ToString();
            string Lattitude = context.Request["latt"];
            string Longitude = context.Request["long"];
            string Nokm = context.Request["Nokm"];
            vehdata.Columns.Add("SNo");
            vehdata.Columns.Add("VehicleID");
            vehdata.Columns.Add("Distance");
            vehdata.Columns.Add("ExpectedTime");
            double lat = 0;
            double lng = 0;
            double.TryParse(Lattitude, out lat);
            double.TryParse(Longitude, out lng);
            ////objects.Markers.Clear();
            cmd = new MySqlCommand("SELECT loginstable.refno, loginstable.loginid, loginstable.pwd, loginstable.usertype, loginsconfigtable.VehicleID AS VehicleNumber, onlinetable.VehicleID, onlinetable.Lat, onlinetable.Longi, onlinetable.Speed, onlinetable.`Timestamp`, onlinetable.Odometer, onlinetable.Diesel, onlinetable.Ignation, onlinetable.Status, onlinetable.Direction, loginstable.main_user FROM loginsconfigtable INNER JOIN loginstable ON loginsconfigtable.Refno = loginstable.refno INNER JOIN onlinetable ON loginsconfigtable.VehicleID = onlinetable.VehicleID AND loginstable.main_user = onlinetable.UserName WHERE (loginstable.loginid = @UserName)");
            //cmd = new MySqlCommand("select UserName,VehicleID,Lat,Longi,Speed, Timestamp,Direction,Diesel,Odometer,Ignation from OnlineTable Where UserName=@UserName");
            cmd.Parameters.Add("@UserName", Username);
            DataTable dt = vdm.SelectQuery(cmd).Tables[0];
            double distance = 0;

            double.TryParse(Nokm, out distance);
            foreach (DataRow dr in dt.Rows)
            {
                NearestVehicle Nearestdistance = new NearestVehicle();
                double lat1 = 0;
                double.TryParse(dr["Lat"].ToString(), out lat1);

                double lng1 = 0;
                double.TryParse(dr["Longi"].ToString(), out lng1);

                GetDistanceBetweenPoints(lat, lng, lat1, lng1);
                if (vehdistance <= distance)
                {
                    double hours = vehdistance / 40;
                    double seconds = hours * 3600;
                    DataRow newrow = vehdata.NewRow();
                    newrow["SNo"] = vehdata.Rows.Count + 1;
                    newrow["VehicleID"] = dr["VehicleID"].ToString();
                    newrow["Distance"] = vehdistance.ToString("0.00");
                    newrow["ExpectedTime"] = (int)(seconds / 3600) + "Hrs" + (int)(seconds / 60) + "Min with Speed " + 40 + " KMPH";
                    Nearestdistance.Vehicleno = dr["VehicleID"].ToString();
                    Nearestdistance.Distance = vehdistance.ToString("0.00");
                    Nearestdistance.ExpectedTime = (int)(seconds / 3600) + "Hrs" + (int)(seconds / 60) + "with Speed " + 40 + " KMPH";
                    Nearestdistance.latitude = lat1.ToString();
                    Nearestdistance.longitude = lng1.ToString();
                    NearestVehiclelist.Add(Nearestdistance);
                }
            }
            if (NearestVehiclelist != null)
            {
                string respnceString = GetJson(NearestVehiclelist);
                context.Response.Write(respnceString);
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

        private void getdata(HttpContext context)
        {
            List<logsclass> vehiclelogslist = new List<logsclass>();
            vdm = new VehicleDBMgr();
            vdm.InitializeDB();
            DataTable dt = (DataTable)context.Session["Data"];
            DataTable selecteddata = dt;
            DataTable allvehicles = new DataTable();
            if (context.Session["allvehicles"] != null)
            {
                allvehicles = (DataTable)context.Session["allvehicles"];
            }
            int rowcount = 1;
            foreach (DataRow dr in dt.Rows)
            {
                logsclass vehlogs = new logsclass();
                vehlogs.Sno = rowcount.ToString();
                vehlogs.vehicleno = dr["VehicleID"].ToString();
                if (allvehicles.Rows.Count > 0)
                {
                    DataRow[] vehtypearray = allvehicles.Select("VehicleNumber='" + dr["VehicleID"].ToString() + "'");
                    try
                    {
                        vehlogs.vehicletype = vehtypearray[0]["VehicleType"].ToString();
                    }
                    catch
                    {
                    }
                    if (vehlogs.vehicletype == "Procurement")
                    {
                        vehlogs.vehicletype = "Car";
                    }
                }
                else
                {
                    vehlogs.vehicletype = "Car";
                }
                vehlogs.latitude = dr["Latitiude"].ToString();
                vehlogs.longitude = dr["Longitude"].ToString();
                vehlogs.direction = dr["Direction"].ToString();
                vehlogs.speed = dr["Speed"].ToString();
                vehlogs.datetime = dr["DateTime"].ToString();
                vehlogs.odometer = dr["Odometer"].ToString();
                if (dr["Status"].ToString() == "0" || dr["Status"].ToString() == "Running")
                    vehlogs.Status = "Running";
                else if (dr["Status"].ToString() == "1" || dr["Status"].ToString() == "Stopped")
                    vehlogs.Status = "Stopped";
                //  vehlogs.Reportsdata=
                vehiclelogslist.Add(vehlogs);
                rowcount++;
            }

            if (vehiclelogslist != null)
            {
                string respnceString = GetJson(vehiclelogslist);
                context.Response.Write(respnceString);
            }
        }
        private void InitilizeVehiclesreports(HttpContext context)
        {
            List<vehiclesclass> vehicleslist = new List<vehiclesclass>();
            vdm = new VehicleDBMgr();
            vdm.InitializeDB();
            string Username = context.Session["field1"].ToString();
            DataTable Vehiclesdata = new DataTable();
            if (context.Session["allvehicles"] == null)
            {
                cmd = new MySqlCommand("SELECT paireddata.VehicleNumber, paireddata.VehicleType, paireddata.FullTankVal, paireddata.EmptyTankVal, paireddata.VehicleType AS Expr1, paireddata.FullTankLtrs, paireddata.emptyTankLrs, loginstable.main_user, cabmanagement.RouteName, cabmanagement.RouteCode FROM loginsconfigtable INNER JOIN paireddata ON loginsconfigtable.VehicleID = paireddata.VehicleNumber INNER JOIN loginstable ON paireddata.UserID = loginstable.main_user AND loginsconfigtable.Refno = loginstable.refno INNER JOIN  cabmanagement ON paireddata.VehicleNumber = cabmanagement.VehicleID WHERE (loginstable.loginid = @UserName)");
                //cmd = new MySqlCommand("SELECT paireddata.VehicleNumber,paireddata.VehicleType, paireddata.FullTankVal, paireddata.EmptyTankVal, paireddata.VehicleType, paireddata.FullTankLtrs, paireddata.emptyTankLrs,  loginstable.main_user FROM loginsconfigtable INNER JOIN paireddata ON loginsconfigtable.VehicleID = paireddata.VehicleNumber INNER JOIN loginstable ON paireddata.UserID = loginstable.main_user AND loginsconfigtable.Refno = loginstable.refno WHERE (loginstable.loginid = @UserName)");
                cmd.Parameters.Add("@UserName", Username);
                Vehiclesdata = vdm.SelectQuery(cmd).Tables[0];
                context.Session["allvehicles"] = Vehiclesdata;
                vdm = null;
            }
            else
            {
                Vehiclesdata = (DataTable)context.Session["allvehicles"];
            }
            foreach (DataRow dr in Vehiclesdata.Rows)
            {
                vehiclesclass veh = new vehiclesclass();
                string vehicleno = dr["VehicleNumber"].ToString();
                veh.vehicletype = dr["VehicleType"].ToString();
                veh.Routename = dr["RouteCode"].ToString();
                //veh.maintenanceplantName = dr["MaintenancePlantName"].ToString();
                //veh.vendorname = dr["VendorName"].ToString();
                //veh.vehiclemodeltype = dr["VehicleTypeName"].ToString();
                //if (!vehicleno.Contains('/'))
                //{
                //char[] charsToTrim = { ' ' };
                //vehicleno = vehicleno.Trim(charsToTrim);
                //}
                //else
                //{
                //    char[] charsToTrim = { '/' };
                //    vehicleno = vehicleno.Remove(7, 1);
                //}
                //vehicleno = vehicleno.Replace(" ", "");
                veh.vehicleno = vehicleno;
                vehicleslist.Add(veh);
            }

            if (vehicleslist != null)
            {
                string respnceString = GetJson(vehicleslist);
                context.Response.Write(respnceString);
            }
        }
        VehicleDBMgr vdm;
        MySqlCommand cmd;
        private void InitilizeVehicles(HttpContext context)
        {
            if (context.Session["preselectedplant"] != null)
            {
                context.Session["preselectedplant"] = null;
            }
            List<vehiclesclass> vehicleslist = new List<vehiclesclass>();
            vdm = new VehicleDBMgr();
            vdm.InitializeDB();
            string Username = "";
            if (context.Session["field1"] != null)
            {
                Username = context.Session["field1"].ToString();
            }
            else
            {
                string responsestring = "Login.aspx";
                string sendresponse = GetJson(new redirecturl() { responseurl = responsestring });
                context.Response.Write(sendresponse);
                return;
            }
            //string Username = context.Request["Username"];
            DataTable Vehiclesdata = new DataTable();
            if (context.Session["allvehicles"] == null)
            {
                cmd = new MySqlCommand("SELECT paireddata.Sno,paireddata.VehicleNumber, paireddata.VehicleType, paireddata.FullTankVal, paireddata.EmptyTankVal, paireddata.VehicleType AS Expr1, paireddata.FullTankLtrs, paireddata.emptyTankLrs, loginstable.main_user, cabmanagement.RouteName, cabmanagement.RouteCode FROM loginsconfigtable INNER JOIN paireddata ON loginsconfigtable.VehicleID = paireddata.VehicleNumber INNER JOIN loginstable ON paireddata.UserID = loginstable.main_user AND loginsconfigtable.Refno = loginstable.refno INNER JOIN  cabmanagement ON paireddata.VehicleNumber = cabmanagement.VehicleID WHERE (loginstable.loginid = @UserName)");
                //cmd = new MySqlCommand("SELECT paireddata.VehicleNumber,paireddata.VehicleType, paireddata.FullTankVal, paireddata.EmptyTankVal, paireddata.VehicleType, paireddata.FullTankLtrs, paireddata.emptyTankLrs, loginstable.main_user FROM loginsconfigtable INNER JOIN paireddata ON loginsconfigtable.VehicleID = paireddata.VehicleNumber INNER JOIN loginstable ON paireddata.UserID = loginstable.main_user AND loginsconfigtable.Refno = loginstable.refno WHERE (loginstable.loginid = @UserName)");
                cmd.Parameters.Add("@UserName", Username);
                Vehiclesdata = vdm.SelectQuery(cmd).Tables[0];
                context.Session["allvehicles"] = Vehiclesdata;
                vdm = null;
            }
            else
            {
                Vehiclesdata = (DataTable)context.Session["allvehicles"];
            }
            foreach (DataRow dr in Vehiclesdata.Rows)
            {
                vehiclesclass veh = new vehiclesclass();
                string vehicleno = dr["VehicleNumber"].ToString();
                veh.vehicletype = dr["VehicleType"].ToString();
                veh.Routename = dr["RouteCode"].ToString();
                if (!vehicleno.Contains('/'))
                {
                    char[] charsToTrim = { ' ' };
                    vehicleno = vehicleno.Trim(charsToTrim);
                }
                else
                {
                    char[] charsToTrim = { '/' };
                    vehicleno = vehicleno.Remove(7, 1);
                }
                vehicleno = vehicleno.Replace(" ", "");
                veh.vehicleno = vehicleno;
                vehicleslist.Add(veh);
            }

            if (vehicleslist != null)
            {
                string respnceString = GetJson(vehicleslist);
                context.Response.Write(respnceString);
            }
        }
        public double dieseldivistion = 0;

        public void calculatedieseldivision(double fulltankadc, double emptytankadc, double fulltankval, double emptytankval)
        {
            try
            {
                dieseldivistion = (fulltankadc - emptytankadc) / (fulltankval - emptytankval);
            }
            catch (Exception ex)
            {
            }
        }
        public class redirecturl
        {
            public string responseurl { set; get; }
        }
        public class vehiclesupdateclasslist
        {
            public string ServerDt { get; set; }
            public List<vehiclesupdateclass> vehiclesupdatelist { get; set; }
        }
        private void LiveUpdate(HttpContext context)
        {
            try
            {
                List<vehiclesupdateclass> vehiclesupdatelist = new List<vehiclesupdateclass>();
                vdm = new VehicleDBMgr();
                vdm.InitializeDB();
                DateTime Currentdate = DbManager.GetTime(vdm.conn);
                double fulltankadc;
                double emptytankadc;
                double fulltankval = 0;
                double emptytankval;
                string Username = "";

                if (context.Session["field1"] != null)
                {
                    Username = context.Session["field1"].ToString();
                }
                else
                {
                    string responsestring = "Login.aspx";
                    string sendresponse = GetJson(new redirecturl() { responseurl = responsestring });
                    context.Response.Write(sendresponse);
                    return;
                }
                //cmd = new MySqlCommand("SELECT onlinetable.Lat, onlinetable.Longi, onlinetable.Speed, onlinetable.Direction, onlinetable.Diesel, onlinetable.Odometer, onlinetable.Ignation, onlinetable.AC, onlinetable.Status, onlinetable.Geofense, onlinetable.In1, onlinetable.In2, onlinetable.In3, onlinetable.In4, onlinetable.In5, onlinetable.Op1, onlinetable.Op2, onlinetable.Op3, onlinetable.Op4, onlinetable.Op5, onlinetable.GSMSignal, onlinetable.GPSSignal, onlinetable.SatilitesAvail, onlinetable.EP, onlinetable.BP, onlinetable.Altitude, onlinetable.UserName, cabmanagement.DriverName, cabmanagement.CompanyID, onlinetable.VehicleID, onlinetable.Timestamp, cabmanagement.MobileNo FROM onlinetable INNER JOIN loginsconfigtable ON onlinetable.VehicleID = loginsconfigtable.VehicleID INNER JOIN loginstable ON loginsconfigtable.Refno = loginstable.refno AND onlinetable.UserName = loginstable.main_user INNER JOIN cabmanagement ON onlinetable.VehicleID = cabmanagement.VehicleID WHERE (loginstable.loginid = @UserName)");
                //cmd = new MySqlCommand("SELECT cabmanagement.DriverName, cabmanagement.CompanyID, cabmanagement.MobileNo, tripdata.RouteName, tripdata.vehiclemaster_sno, cabmanagement.UserID, onlinetable.VehicleID, onlinetable.Lat, onlinetable.Longi, onlinetable.Speed, onlinetable.Timestamp, onlinetable.Direction, onlinetable.Diesel, onlinetable.Odometer, onlinetable.Ignation, onlinetable.AC, onlinetable.Status, onlinetable.Geofense, onlinetable.In1, onlinetable.In2, onlinetable.In3, onlinetable.In4, onlinetable.In5, onlinetable.Op1, onlinetable.Op2, onlinetable.Op3, onlinetable.Op4, onlinetable.Op5, onlinetable.GSMSignal, onlinetable.GPSSignal, onlinetable.SatilitesAvail,  onlinetable.EP, onlinetable.BP, onlinetable.Altitude FROM   paireddata INNER JOIN  cabmanagement ON paireddata.VehicleNumber = cabmanagement.VehicleID INNER JOIN onlinetable ON cabmanagement.VehicleID = onlinetable.VehicleID LEFT OUTER JOIN  tripdata ON paireddata.Sno = tripdata.vehiclemaster_sno WHERE  (cabmanagement.UserID = @UserName)");
                //cmd = new MySqlCommand("SELECT        paireddata.UserID, paireddata.VehicleNumber, paireddata.ADC1, paireddata.ADCVal1, paireddata.ADC2, paireddata.ADCVal2, paireddata.FullTankVal, paireddata.EmptyTankVal, paireddata.VehicleType, paireddata.FullTankLtrs, paireddata.emptyTankLrs, paireddata.Sno, paireddata.VehicleNumber AS Expr2, cabmanagement.CompanyID, cabmanagement.MobileNo, cabmanagement.DriverName, onlinetable.Lat, onlinetable.Longi, onlinetable.Speed, onlinetable.Direction, onlinetable.Diesel, onlinetable.Odometer, onlinetable.Ignation,onlinetable.Timestamp, onlinetable.AC, onlinetable.Status AS Expr3, onlinetable.Geofense, onlinetable.GSMSignal, onlinetable.GPSSignal, onlinetable.SatilitesAvail, onlinetable.EP, onlinetable.BP, onlinetable.Altitude, cabmanagement_1.RouteCode FROM cabmanagement INNER JOIN paireddata ON cabmanagement.VehicleID = paireddata.VehicleNumber INNER JOIN onlinetable ON cabmanagement.VehicleID = onlinetable.VehicleID INNER JOIN loginsconfigtable ON onlinetable.VehicleID = loginsconfigtable.VehicleID INNER JOIN loginstable ON loginsconfigtable.Refno = loginstable.refno AND onlinetable.UserName = loginstable.main_user INNER JOIN cabmanagement cabmanagement_1 ON cabmanagement.VehicleID = cabmanagement_1.VehicleID AND onlinetable.VehicleID = cabmanagement_1.VehicleID WHERE (loginstable.loginid = @UserName)");
                //cmd = new MySqlCommand("SELECT  tripdata_1.Refno, tripdata_1.Tripid, tripdata_1.assigndate, tripdata_1.completdate, tripdata_1.Status, tripdata_1.UserID AS Expr1, tripdata_1.RouteName,  tripdata_1.vehiclemaster_sno, paireddata.UserID, paireddata.VehicleNumber, paireddata.GprsDevID, paireddata.DeviceType, paireddata.PhoneNumber,  paireddata.SimNumber, paireddata.Inp1, paireddata.InpVal1, paireddata.Inp2, paireddata.InpVal2, paireddata.Inp3, paireddata.InpVal3, paireddata.Inp4,  paireddata.InpVal4, paireddata.Inp5, paireddata.InpVal5, paireddata.Inp6, paireddata.InpVal6, paireddata.Inp7, paireddata.InpVal7, paireddata.Inp8, paireddata.InpVal8, paireddata.Oup1, paireddata.OupVal1, paireddata.Oup2, paireddata.OupVal2, paireddata.Oup3, paireddata.OupVal3, paireddata.Oup4, paireddata.OupVal4, paireddata.Oup5, paireddata.OupVal5, paireddata.Oup6, paireddata.OupVal6, paireddata.Oup7, paireddata.OupVal7, paireddata.Oup8, paireddata.OupVal8, paireddata.ADC1, paireddata.ADCVal1, paireddata.ADC2, paireddata.ADCVal2, paireddata.FullTankVal, paireddata.EmptyTankVal, paireddata.VehicleType, paireddata.FullTankLtrs, paireddata.emptyTankLrs, paireddata.Sno, paireddata.VehicleNumber AS Expr2, cabmanagement.CompanyID, cabmanagement.MobileNo, cabmanagement.DriverName, onlinetable.Lat, onlinetable.VehicleID, onlinetable.Longi, onlinetable.Speed, onlinetable.Timestamp, onlinetable.Direction, onlinetable.Diesel, onlinetable.Odometer, onlinetable.Ignation, onlinetable.AC, onlinetable.Status AS Expr3, onlinetable.Geofense,  onlinetable.In1, onlinetable.In2, onlinetable.In3, onlinetable.In4, onlinetable.In5, onlinetable.Op1, onlinetable.Op2, onlinetable.Op3, onlinetable.Op4, onlinetable.Op5, onlinetable.GSMSignal, onlinetable.GPSSignal, onlinetable.SatilitesAvail, onlinetable.EP, onlinetable.BP, onlinetable.Altitude FROM cabmanagement INNER JOIN paireddata ON cabmanagement.VehicleID = paireddata.VehicleNumber INNER JOIN onlinetable ON cabmanagement.VehicleID = onlinetable.VehicleID LEFT OUTER JOIN (SELECT Refno, Tripid, assigndate, completdate, vehiclemaster_sno, Status, UserID, RouteName FROM tripdata WHERE  (Status = 'A')) tripdata_1 ON paireddata.UserID = tripdata_1.UserID AND paireddata.Sno = tripdata_1.vehiclemaster_sno INNER JOIN  loginsconfigtable ON onlinetable.VehicleID = loginsconfigtable.VehicleID INNER JOIN loginstable ON loginsconfigtable.Refno = loginstable.refno AND onlinetable.UserName = loginstable.main_user WHERE  (loginstable.loginid = @UserName)");
                cmd = new MySqlCommand("SELECT  paireddata.VehicleNumber, paireddata.ADC1, paireddata.ADCVal1, paireddata.ADC2, paireddata.ADCVal2, paireddata.FullTankVal, paireddata.EmptyTankVal, paireddata.VehicleType, paireddata.FullTankLtrs, paireddata.emptyTankLrs, paireddata.Sno, paireddata.VehicleNumber AS Expr2, cabmanagement.CompanyID, cabmanagement.MobileNo, cabmanagement.DriverName,onlinetable.StoppedTime, onlinetable.Lat, onlinetable.Longi, onlinetable.Speed, onlinetable.Direction, onlinetable.Diesel, onlinetable.Timestamp, onlinetable.Odometer, onlinetable.Ignation, onlinetable.AC, onlinetable.Status AS Expr3, onlinetable.Geofense, onlinetable.GSMSignal, onlinetable.GPSSignal, onlinetable.SatilitesAvail, onlinetable.EP, onlinetable.BP, onlinetable.Altitude, cabmanagement.RouteCode,cabmanagement.PlantName FROM cabmanagement INNER JOIN paireddata ON cabmanagement.VehicleID = paireddata.VehicleNumber AND cabmanagement.UserID = paireddata.UserID INNER JOIN onlinetable ON cabmanagement.VehicleID = onlinetable.VehicleID INNER JOIN loginsconfigtable ON onlinetable.VehicleID = loginsconfigtable.VehicleID INNER JOIN loginstable ON loginsconfigtable.Refno = loginstable.refno AND onlinetable.UserName = loginstable.main_user WHERE (loginstable.loginid = @UserName)");
                cmd.Parameters.Add("@UserName", Username);
                DataTable vehupdate = vdm.SelectQuery(cmd).Tables[0];
                //cmd = new MySqlCommand("select * from PairedData where UserID=@UserName");
                DataTable Vehiclesdata = new DataTable();
                if (context.Session["vehiclesdata"] != null)
                {
                    Vehiclesdata = (DataTable)context.Session["vehiclesdata"];
                }
                else
                {
                    //  cmd = new MySqlCommand("select * from PairedData where UserID=@UserName");
                    cmd = new MySqlCommand("SELECT loginstable.refno, loginstable.main_user, loginstable.loginid, loginstable.pwd, loginstable.usertype, loginsconfigtable.VehicleID  as VehicleNumber, paireddata.FullTankVal, paireddata.EmptyTankVal, paireddata.VehicleType, paireddata.FullTankLtrs, paireddata.emptyTankLrs FROM loginsconfigtable INNER JOIN loginstable ON loginsconfigtable.Refno = loginstable.refno INNER JOIN paireddata ON loginsconfigtable.VehicleID = paireddata.VehicleNumber WHERE (loginstable.loginid = @UserName)");
                    cmd.Parameters.Add("@UserName", Username);
                    Vehiclesdata = vdm.SelectQuery(cmd).Tables[0];
                    vdm = null;
                    context.Session["vehiclesdata"] = Vehiclesdata;
                }

                //Dictionary<string, double> odometerValues = new Dictionary<string, double>();
                //odometerValues = (Dictionary<string, double>)context.Session["odometerValues"];
                //string OdometerRound = "";

                foreach (DataRow dr in vehupdate.Rows)
                {
                    vehiclesupdateclass vehiclesupdate = new vehiclesupdateclass();
                    string vehicleno = dr["VehicleNumber"].ToString();
                    if (!vehicleno.Contains('/'))
                    {
                        char[] charsToTrim = { ' ' };
                        vehicleno = vehicleno.Trim(charsToTrim);
                    }
                    else
                    {
                        char[] charsToTrim = { '/' };
                        vehicleno = vehicleno.Remove(7, 1);
                    }
                    vehicleno = vehicleno.Replace(" ", "");
                    vehiclesupdate.vehiclenum = vehicleno;
                    vehiclesupdate.latitude = dr["Lat"].ToString();
                    vehiclesupdate.longitude = dr["Longi"].ToString();
                    vehiclesupdate.Speed = dr["Speed"].ToString();
                    vehiclesupdate.Datetime = dr["Timestamp"].ToString();
                    vehiclesupdate.dieselvalue = dr["Diesel"].ToString();
                    //if (odometerValues.Keys.Contains(dr["VehicleNumber"].ToString()))
                    //{
                    //    double prevodmrdng = odometerValues[dr["VehicleNumber"].ToString()];
                    //    if (prevodmrdng > 0)
                    //    {
                    //        double odometervalue = 0;
                    //        double.TryParse(dr["Odometer"].ToString(), out odometervalue);
                    //        double TotalDistancea = (odometervalue - prevodmrdng);
                    //        OdometerRound = Math.Round(TotalDistancea, 2).ToString() + " KMs ";
                    //    }
                    //    else
                    //    {
                    //        OdometerRound = 0 + " KMs ";
                    //    }
                    //}
                    foreach (DataRow drR in Vehiclesdata.Rows)
                    {
                        if (dr["VehicleNumber"].ToString() == drR["VehicleNumber"].ToString())
                        {
                            double.TryParse(drR["FullTankVal"].ToString(), out fulltankadc);
                            double.TryParse(drR["EmptyTankVal"].ToString(), out emptytankadc);
                            double.TryParse(drR["FullTankLtrs"].ToString(), out fulltankval);
                            double.TryParse(drR["emptyTankLrs"].ToString(), out emptytankval);
                            calculatedieseldivision(fulltankadc, emptytankadc, fulltankval, emptytankval);
                        }
                    }
                    double dieselvalue = 0;
                    double.TryParse(dr["Diesel"].ToString(), out dieselvalue);
                    double dieselstring = 0;
                    try
                    {
                        if (dieseldivistion != 0)
                        {
                            dieselstring = Math.Round((dieselvalue) / dieseldivistion, 2);

                            dieselstring = Math.Abs(dieselstring);
                        }
                    }
                    catch
                    {
                    }
                    vehiclesupdate.dieselvalue = dieselstring.ToString();
                    vehiclesupdate.fulltankval = fulltankval;
                    vehiclesupdate.odometervalue = dr["Odometer"].ToString();
                    //vehiclesupdate.todaymileage = OdometerRound;
                    vehiclesupdate.Ignation = dr["Ignation"].ToString();
                    vehiclesupdate.ACStatus = dr["AC"].ToString();
                    vehiclesupdate.Geofence = dr["Geofense"].ToString();
                    vehiclesupdate.GPSSignal = dr["GPSSignal"].ToString();
                    vehiclesupdate.mainpower = dr["EP"].ToString();
                    vehiclesupdate.direction = dr["Direction"].ToString();
                    vehiclesupdate.DriverName = dr["DriverName"].ToString();
                    vehiclesupdate.CompanyID = dr["CompanyID"].ToString();
                    vehiclesupdate.MobileNo = dr["MobileNo"].ToString();
                    if (dr["RouteCode"].ToString() != "")
                    {
                        vehiclesupdate.RouteName = dr["RouteCode"].ToString();
                    }
                    else
                    {
                        vehiclesupdate.RouteName = "NA";
                    }

                    if (dr["PlantName"].ToString() != "")
                    {
                        vehiclesupdate.PlantName = dr["PlantName"].ToString();
                    }
                    else
                    {
                        vehiclesupdate.PlantName = "NA";
                    }
                    vehiclesupdate.stoppedfor = dr["StoppedTime"].ToString();
                    vehiclesupdatelist.Add(vehiclesupdate);
                }

                if (vehiclesupdatelist != null)
                {
                    vehiclesupdateclasslist VDMlist = new vehiclesupdateclasslist();
                    VDMlist.vehiclesupdatelist = vehiclesupdatelist;
                    VDMlist.ServerDt = Currentdate.ToString("dd/MM/yyyy HH:mm:ss");
                    string respnceString = GetJson(VDMlist);
                    context.Response.Write(respnceString);
                }
            }
            catch
            {
            }
        }

        private void InitilizeGroups(HttpContext context)
        {
            vdm = new VehicleDBMgr();
            vdm.InitializeDB();
            string VehicleTypestring = "";
            string Plantstring = "";
            string Username = "";
            if (context.Session["field1"] != null)
            {
                Username = context.Session["field1"].ToString();
            }
            else
            {
                string responsestring = "Login.aspx";
                string sendresponse = GetJson(new redirecturl() { responseurl = responsestring });
                context.Response.Write(sendresponse);
                return;
            }
            string filterstring = context.Request["filterstring"];
            string authorizedtype;
            if (context.Session["Authorized"] != null)
            {
                authorizedtype = context.Session["Authorized"].ToString();
            }
            else
            {
                context.Session["Authorized"] = "Plants";
                authorizedtype = context.Session["Authorized"].ToString();
            }
            string[] filterstringarray = filterstring.Split('#');
            string[] plantsarray = filterstringarray[0].Split(',');
            for (int i = 0; i < plantsarray.Length; i++)
            {
                VehicleTypestring += "'" + plantsarray[i].TrimStart() + "',";
            }
            VehicleTypestring = VehicleTypestring.Remove(VehicleTypestring.Length - 1);
            string[] vehtypesarray = filterstringarray[1].Split(',');
            for (int i = 0; i < vehtypesarray.Length; i++)
            {
                Plantstring += "'" + vehtypesarray[i].TrimStart() + "',";
            }
            Plantstring = Plantstring.Remove(Plantstring.Length - 1);
            DataTable vendors = new DataTable();
            if (context.Session["vendorstable"] != null)
            {
                vendors = (DataTable)context.Session["vendorstable"];
            }
            else
            {
                //cmd = new MySqlCommand(" SELECT  cabmanagement.PlantName, cabmanagement.VehicleType, cabmanagement.VehicleID FROM  cabmanagement INNER JOIN loginstable ON cabmanagement.UserID = loginstable.main_user WHERE (loginstable.loginid = @UserName)");
                // cmd = new MySqlCommand("SELECT vehiclemaster.VehicleTypeCode, vehiclemaster.VehicleID, vehiclemaster.Capacity, vehiclemaster.MaintenancePlantCode, vehiclemaster.VendorNo, vehiclemaster.VendorName, vehiclemaster.Address, vehiclemaster.UserName, vehiclemaster.VehicleTypeName, vehiclemaster.MaintenancePlantName, vehiclemaster.Sno,vehiclemaster.vehiclemodel FROM vehiclemaster INNER JOIN loginsconfigtable ON vehiclemaster.VehicleID = loginsconfigtable.VehicleID INNER JOIN loginstable ON vehiclemaster.UserName = loginstable.main_user AND loginsconfigtable.Refno = loginstable.refno WHERE (loginstable.loginid = @UserName)");
                cmd = new MySqlCommand("SELECT cabmanagement.PlantName, cabmanagement.VehicleType, cabmanagement.VehicleID,cabmanagement.RouteName, cabmanagement.RouteCode FROM cabmanagement INNER JOIN loginsconfigtable ON cabmanagement.VehicleID = loginsconfigtable.VehicleID INNER JOIN  loginstable ON cabmanagement.UserID = loginstable.main_user AND loginsconfigtable.Refno = loginstable.refno WHERE (loginstable.loginid = @UserName)");
                cmd.Parameters.Add("@UserName", Username);
                vendors = vdm.SelectQuery(cmd).Tables[0];
                context.Session["vendorstable"] = vendors;
            }
            DataRow[] filteredrows = null;
            if ((VehicleTypestring != "'All Vehicle Types'") && Plantstring != "'All Plants'")
            {

                filteredrows = vendors.Select("VehicleType in (" + VehicleTypestring + ") and PlantName in (" + Plantstring + ") ");

            }
            else if (VehicleTypestring == "'All Vehicle Types'" && Plantstring == "'All Plants'")
            {
                filteredrows = vendors.Select("VehicleType in (" + VehicleTypestring + ") and PlantName in (" + Plantstring + ") ");
                //filteredrows = vendors.Select();
            }
            else if (VehicleTypestring != "'All Vehicle Types'" && Plantstring != "'All Plants'")
            {
                filteredrows = vendors.Select("VehicleType in (" + VehicleTypestring + ") and PlantName in (" + Plantstring + ")");
            }
            else if (VehicleTypestring != "'All Vehicle Types'" && Plantstring == "'All Plants'")
            {
                filteredrows = vendors.Select("VehicleType in (" + VehicleTypestring + ")");
            }
            else if (VehicleTypestring == "'All Vehicle Types'" && Plantstring != "'All Plants'")
            {
                filteredrows = vendors.Select("PlantName in (" + Plantstring + ")");
            }
            DataTable filteredtable = new DataTable();
            if (filteredrows.Length > 0)
            {
                filteredtable = filteredrows.CopyToDataTable();
            }
            DataView view = filteredtable.DefaultView;
            DataTable distinctTable = new DataTable();
            if (view.Count > 0)
            {
                distinctTable = view.ToTable("DistinctTable", true, "VehicleID", "RouteCode", "VehicleType");
            }
            vdm = null;
            List<Groupsclass> Groupslist = new List<Groupsclass>();
            foreach (DataRow vehicleid in distinctTable.Rows)
            {
                Groupsclass groupsdict = new Groupsclass();
                string vehicleno = vehicleid["VehicleID"].ToString();
                string vehicletype = vehicleid["VehicleType"].ToString();
                char[] charsToTrim = { ' ' };
                vehicleno = vehicleno.Trim(charsToTrim);
                vehicleno = vehicleno.Replace(" ", "");
                groupsdict.vehicleno = vehicleno;
                groupsdict.vehiclemodeltype = vehicletype;
                groupsdict.Routename = vehicleid["RouteCode"].ToString();
                Groupslist.Add(groupsdict);
            }
            if (Groupslist != null)
            {
                string respnceString = GetJson(Groupslist);
                context.Response.Write(respnceString);
            }
        }

        private void ShowMyplants(HttpContext context)
        {
            List<BranchData> Branchlist = new List<BranchData>();
            vdm = new VehicleDBMgr();
            vdm.InitializeDB();
            string Username = context.Session["field1"].ToString();
            string Usertype = context.Session["UserType"].ToString();
            if (Usertype == "User")
            {
                cmd = new MySqlCommand("SELECT cabmanagement.PlantName, cabmanagement.VehicleType, cabmanagement.VehicleID, cabmanagement.RouteName, cabmanagement.RouteCode, onlinetable.Lat, onlinetable.Longi, onlinetable.`Timestamp`, onlinetable.Speed, onlinetable.Odometer, onlinetable.GPSSignal, onlinetable.EP FROM cabmanagement INNER JOIN loginsconfigtable ON cabmanagement.VehicleID = loginsconfigtable.VehicleID INNER JOIN loginstable ON cabmanagement.UserID = loginstable.main_user AND loginsconfigtable.Refno = loginstable.refno INNER JOIN onlinetable ON cabmanagement.VehicleID = onlinetable.VehicleID WHERE (loginstable.loginid = @UserName)");
                cmd.Parameters.Add("@UserName", Username);
                DataTable vehiclesstatus = vdm.SelectQuery(cmd).Tables[0];
                List<vehiclesupdateclass> vehiclesupdatelist = new List<vehiclesupdateclass>();
                foreach (DataRow dr in vehiclesstatus.Rows)
                {
                    vehiclesupdateclass vehiclesupdate = new vehiclesupdateclass();
                    string vehicleno = dr["VehicleID"].ToString();
                    if (!vehicleno.Contains('/'))
                    {
                        char[] charsToTrim = { ' ' };
                        vehicleno = vehicleno.Trim(charsToTrim);
                    }
                    else
                    {
                        char[] charsToTrim = { '/' };
                        vehicleno = vehicleno.Remove(7, 1);
                    }
                    vehicleno = vehicleno.Replace(" ", "");
                    vehiclesupdate.vehiclenum = vehicleno;
                    vehiclesupdate.latitude = dr["Lat"].ToString();
                    vehiclesupdate.longitude = dr["Longi"].ToString();
                    vehiclesupdate.Speed = dr["Speed"].ToString();
                    vehiclesupdate.Datetime = dr["Timestamp"].ToString();
                    vehiclesupdate.odometervalue = dr["Odometer"].ToString();
                    vehiclesupdate.GPSSignal = dr["GPSSignal"].ToString();
                    vehiclesupdate.mainpower = dr["EP"].ToString();
                    vehiclesupdate.PlantName = dr["PlantName"].ToString();
                    vehiclesupdatelist.Add(vehiclesupdate);
                }
                DateTime Currentdate = DbManager.GetTime(vdm.conn);
                if (vehiclesupdatelist != null)
                {
                    vehiclesupdateclasslist VDMlist = new vehiclesupdateclasslist();
                    VDMlist.vehiclesupdatelist = vehiclesupdatelist;
                    VDMlist.ServerDt = Currentdate.ToString("dd/MM/yyyy HH:mm:ss");
                    string respnceString = GetJson(VDMlist);
                    context.Response.Write(respnceString);
                }
            }
            else
            {
                cmd = new MySqlCommand("SELECT cabmanagement.PlantName, cabmanagement.VehicleType, cabmanagement.VehicleID, cabmanagement.RouteName, cabmanagement.RouteCode, onlinetable.Lat, onlinetable.Longi, onlinetable.Timestamp, onlinetable.Speed, onlinetable.Odometer, onlinetable.GPSSignal, onlinetable.EP FROM cabmanagement INNER JOIN onlinetable ON cabmanagement.VehicleID = onlinetable.VehicleID WHERE (cabmanagement.UserID = @UserID)");
                cmd.Parameters.Add("@UserID", Username);
                DataTable vehiclesstatus = vdm.SelectQuery(cmd).Tables[0];
                List<vehiclesupdateclass> vehiclesupdatelist = new List<vehiclesupdateclass>();
                foreach (DataRow dr in vehiclesstatus.Rows)
                {
                    vehiclesupdateclass vehiclesupdate = new vehiclesupdateclass();
                    string vehicleno = dr["VehicleID"].ToString();
                    if (!vehicleno.Contains('/'))
                    {
                        char[] charsToTrim = { ' ' };
                        vehicleno = vehicleno.Trim(charsToTrim);
                    }
                    else
                    {
                        char[] charsToTrim = { '/' };
                        vehicleno = vehicleno.Remove(7, 1);
                    }
                    vehicleno = vehicleno.Replace(" ", "");
                    vehiclesupdate.vehiclenum = vehicleno;
                    vehiclesupdate.latitude = dr["Lat"].ToString();
                    vehiclesupdate.longitude = dr["Longi"].ToString();
                    vehiclesupdate.Speed = dr["Speed"].ToString();
                    vehiclesupdate.Datetime = dr["Timestamp"].ToString();
                    vehiclesupdate.odometervalue = dr["Odometer"].ToString();
                    vehiclesupdate.GPSSignal = dr["GPSSignal"].ToString();
                    vehiclesupdate.mainpower = dr["EP"].ToString();
                    vehiclesupdate.PlantName = dr["PlantName"].ToString();
                    vehiclesupdatelist.Add(vehiclesupdate);
                }
                DateTime Currentdate = DbManager.GetTime(vdm.conn);
                if (vehiclesupdatelist != null)
                {
                    vehiclesupdateclasslist VDMlist = new vehiclesupdateclasslist();
                    VDMlist.vehiclesupdatelist = vehiclesupdatelist;
                    VDMlist.ServerDt = Currentdate.ToString("dd/MM/yyyy HH:mm:ss");
                    string respnceString = GetJson(VDMlist);
                    context.Response.Write(respnceString);
                }
            }
        }

        private void ShowMyLocations(HttpContext context)
        {
            List<BranchData> Branchlist = new List<BranchData>();
            vdm = new VehicleDBMgr();
            vdm.InitializeDB();
            string Username = context.Session["field1"].ToString();
            string Usertype = context.Session["UserType"].ToString();
            string gettype = context.Request["Type"].ToString();
            string mainuser = "";
            if (context.Session["main_user"] == null)
            {
                cmd = new MySqlCommand("SELECT main_user FROM loginstable WHERE (loginid = @loginid)");
                cmd.Parameters.Add("@loginid", Username);
                DataTable mainusertbl = vdm.SelectQuery(cmd).Tables[0];
                if (mainusertbl.Rows.Count > 0)
                {
                    mainuser = mainusertbl.Rows[0]["main_user"].ToString();
                    context.Session["main_user"] = mainuser;
                }
                else
                {
                    mainuser = Username;
                }
            }
            else
            {
                mainuser = context.Session["main_user"].ToString();
            }
            if (Usertype == "User")
            {
                
                DataTable totaldata = new DataTable();
                if (context.Session["vendorstable"] != null)
                {
                    totaldata = (DataTable)context.Session["vendorstable"];
                }
                else
                {
                    cmd = new MySqlCommand("SELECT cabmanagement.PlantName, cabmanagement.VehicleType, cabmanagement.VehicleID,cabmanagement.RouteName, cabmanagement.RouteCode FROM cabmanagement INNER JOIN loginsconfigtable ON cabmanagement.VehicleID = loginsconfigtable.VehicleID INNER JOIN  loginstable ON cabmanagement.UserID = loginstable.main_user AND loginsconfigtable.Refno = loginstable.refno WHERE (loginstable.loginid = @UserName)");
                    //cmd = new MySqlCommand("SELECT cabmanagement.PlantName, cabmanagement.VehicleType, cabmanagement.VehicleID FROM cabmanagement INNER JOIN loginsconfigtable ON cabmanagement.VehicleID = loginsconfigtable.VehicleID INNER JOIN  loginstable ON cabmanagement.UserID = loginstable.main_user AND loginsconfigtable.Refno = loginstable.refno WHERE (loginstable.loginid = @UserName)");
                    cmd.Parameters.Add("@UserName", Username);
                    totaldata = vdm.SelectQuery(cmd).Tables[0];
                    context.Session["vendorstable"] = totaldata;
                }
                DataView view = new DataView(totaldata);
                DataTable dtPlantName = view.ToTable(true, "PlantName");
                if (gettype == "ALL")
                {
                    cmd = new MySqlCommand("SELECT UserName, BranchID, Description, Latitude, Longitude, PhoneNumber, ImagePath, ImageType, Radious, PlantName, IsPlant FROM branchdata where UserName=@UserName");
                    cmd.Parameters.Add("@UserName", mainuser);
                }
                else
                {
                    cmd = new MySqlCommand("SELECT paireddata.VehicleNumber, branchdata.UserName, branchdata.BranchID, branchdata.Description, branchdata.Latitude, branchdata.Longitude, branchdata.PhoneNumber, branchdata.ImagePath, branchdata.ImageType, branchdata.Radious, branchdata.PlantName, branchdata.IsPlant FROM paireddata INNER JOIN tripconfiguration ON paireddata.Sno = tripconfiguration.Veh_Sno INNER JOIN routetable ON tripconfiguration.RouteID = routetable.SNo INNER JOIN routesubtable ON routetable.SNo = routesubtable.SNo INNER JOIN branchdata ON routesubtable.LocationID = branchdata.Sno WHERE (tripconfiguration.UserID = @UserID) GROUP BY branchdata.BranchID");
                    cmd.Parameters.Add("@UserID", mainuser);
                }
               
                DataTable Branchdata = vdm.SelectQuery(cmd).Tables[0];
                List<string> plants = new List<string>();
                if (dtPlantName.Rows.Count > 0)
                {
                    foreach (DataRow drr in dtPlantName.Rows)
                    {
                        plants.Add(drr["PlantName"].ToString());
                    }
                    //DataRow[] plantbranches = Branchdata.Select("BranchID='" + drr["PlantName"].ToString() + "'");
                    vdm = null;
                    foreach (DataRow dr in Branchdata.Rows)
                    {
                        BranchData Branch = new BranchData();
                        Branch.BranchName = dr["BranchID"].ToString();
                        Branch.latitude = dr["Latitude"].ToString();
                        Branch.longitude = dr["Longitude"].ToString();
                        Branch.Decription = dr["Description"].ToString();
                        Branch.Image = dr["ImagePath"].ToString();
                        Branch.isplant = dr["IsPlant"].ToString();
                        Branch.radius = dr["Radious"].ToString();
                        if (gettype != "ALL")
                        {
                            Branch.vehno = dr["VehicleNumber"].ToString();
                        }
                        Branchlist.Add(Branch);
                    }
                    BranchData Branchs = new BranchData();
                    Branchs.plants = plants;
                    Branchlist.Add(Branchs);
                    if (Branchlist != null)
                    {
                        string respnceString = GetJson(Branchlist);
                        context.Response.Write(respnceString);
                    }
                }
            }
            else
            {
                if (gettype == "ALL")
                {
                    cmd = new MySqlCommand("SELECT UserName, BranchID, Description, Latitude, Longitude, PhoneNumber, ImagePath, ImageType, Radious, PlantName, IsPlant FROM branchdata where UserName=@UserName");
                    cmd.Parameters.Add("@UserName", mainuser);
                }
                else
                {
                    cmd = new MySqlCommand("SELECT paireddata.VehicleNumber, branchdata.UserName, branchdata.BranchID, branchdata.Description, branchdata.Latitude, branchdata.Longitude, branchdata.PhoneNumber, branchdata.ImagePath, branchdata.ImageType, branchdata.Radious, branchdata.PlantName, branchdata.IsPlant FROM paireddata INNER JOIN tripconfiguration ON paireddata.Sno = tripconfiguration.Veh_Sno INNER JOIN routetable ON tripconfiguration.RouteID = routetable.SNo INNER JOIN routesubtable ON routetable.SNo = routesubtable.SNo INNER JOIN branchdata ON routesubtable.LocationID = branchdata.Sno WHERE (tripconfiguration.UserID = @UserID) GROUP BY branchdata.BranchID");
                    cmd.Parameters.Add("@UserID", mainuser);
                }
                DataTable Branchdata = vdm.SelectQuery(cmd).Tables[0];
                vdm = null;
                List<string> plants = new List<string>();
                foreach (DataRow dr in Branchdata.Rows)
                {
                    if (dr["IsPlant"].ToString() == "True")
                    {
                        plants.Add(dr["BranchID"].ToString());
                    }
                    BranchData Branch = new BranchData();
                    Branch.BranchName = dr["BranchID"].ToString();
                    Branch.latitude = dr["Latitude"].ToString();
                    Branch.longitude = dr["Longitude"].ToString();
                    Branch.Decription = dr["Description"].ToString();
                    Branch.Image = dr["ImagePath"].ToString();
                    Branch.isplant = dr["IsPlant"].ToString();
                    Branch.radius = dr["Radious"].ToString();
                    if (gettype != "ALL")
                    {
                        Branch.vehno = dr["VehicleNumber"].ToString();
                    }
                    Branchlist.Add(Branch);
                }
                BranchData Branchs = new BranchData();
                Branchs.plants = plants;
                Branchlist.Add(Branchs);
                if (Branchlist != null)
                {
                    string respnceString = GetJson(Branchlist);
                    context.Response.Write(respnceString);
                }
            }
        }

        private static string GetJson(object obj)
        {
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            jsonSerializer.MaxJsonLength = 2147483644;
            return jsonSerializer.Serialize(obj);
        }
        private void setauthorizedsession(HttpContext context)
        {
            string Authorizedtype = context.Request["Authorizedtype"];
            context.Session["Authorized"] = Authorizedtype;
        }

        public class dataforpolyclass
        {
            public string Sno { get; set; }
            public string datetime { get; set; }
            public string vehicleno { get; set; }
            public string vehicletype { get; set; }
            public string latitude { get; set; }
            public string longitude { get; set; }
            public string direction { get; set; }
            public string speed { get; set; }
            public string Status { get; set; }
            public DataTable Reportsdata { get; set; }
            public string odometer { get; set; }
        }
        public class datalistclass
        {
            public string vehicleno { get; set; }
            public List<dataforpolyclass> logslist { get; set; }
        }

        private void dataforpoly(HttpContext context)
        {
            List<datalistclass> datalist = new List<datalistclass>();
            vdm = new VehicleDBMgr();
            vdm.InitializeDB();
            string Username = "";
            if (context.Session["field1"] != null)
            {
                Username = context.Session["field1"].ToString();
            }
            else
            {
                string responsestring = "Login.aspx";
                string sendresponse = GetJson(new redirecturl() { responseurl = responsestring });
                context.Response.Write(sendresponse);
                return;
            }
            DataTable dt = new DataTable();

            List<string> logstbls = new List<string>();
            logstbls.Add("GpsTrackVehicleLogs");
            logstbls.Add("GpsTrackVehicleLogs1");
            logstbls.Add("GpsTrackVehicleLogs2");
            logstbls.Add("GpsTrackVehicleLogs3");
            DataTable logs = new DataTable();
            DataTable tottable = new DataTable();
            foreach (string tbname in logstbls)
            {
                cmd = new MySqlCommand("SELECT tripdata.assigndate, tripdata.completdate, " + tbname + ".UserID, " + tbname + ".VehicleID, " + tbname + ".Speed, " + tbname + ".DateTime, " + tbname + ".Latitiude, " + tbname + ".Longitude, " + tbname + ".Status, " + tbname + ".Direction, " + tbname + ".Odometer, paireddata.VehicleNumber FROM paireddata INNER JOIN " + tbname + " ON paireddata.UserID = " + tbname + ".UserID AND paireddata.VehicleNumber = " + tbname + ".VehicleID INNER JOIN tripdata ON " + tbname + ".DateTime > tripdata.assigndate AND paireddata.Sno = tripdata.vehiclemaster_sno WHERE (tripdata.Status = 'A') and tripdata.UserID=@UserID");
                cmd.Parameters.Add("@UserID", Username);
                logs = vdm.SelectQuery(cmd).Tables[0];
                if (tottable.Rows.Count == 0)
                {
                    tottable = logs.Clone();
                }
                foreach (DataRow dr1 in logs.Rows)
                {
                    tottable.ImportRow(dr1);
                }
            }
            DataView dv = tottable.DefaultView;
            dv.Sort = "DateTime ASC";
            dt = dv.ToTable();
            DataTable allvehicles = new DataTable();

            if (context.Session["allvehicles"] == null)
            {
                cmd = new MySqlCommand("SELECT paireddata.VehicleNumber, paireddata.VehicleType, paireddata.FullTankVal, paireddata.EmptyTankVal, paireddata.VehicleType AS Expr1, paireddata.FullTankLtrs, paireddata.emptyTankLrs, loginstable.main_user, cabmanagement.RouteName, cabmanagement.RouteCode FROM loginsconfigtable INNER JOIN paireddata ON loginsconfigtable.VehicleID = paireddata.VehicleNumber INNER JOIN loginstable ON paireddata.UserID = loginstable.main_user AND loginsconfigtable.Refno = loginstable.refno INNER JOIN  cabmanagement ON paireddata.VehicleNumber = cabmanagement.VehicleID WHERE (loginstable.loginid = @UserName)");
                //cmd = new MySqlCommand("SELECT paireddata.VehicleNumber,paireddata.VehicleType, paireddata.FullTankVal, paireddata.EmptyTankVal, paireddata.VehicleType, paireddata.FullTankLtrs, paireddata.emptyTankLrs, loginstable.main_user FROM loginsconfigtable INNER JOIN paireddata ON loginsconfigtable.VehicleID = paireddata.VehicleNumber INNER JOIN loginstable ON paireddata.UserID = loginstable.main_user AND loginsconfigtable.Refno = loginstable.refno WHERE (loginstable.loginid = @UserName)");
                cmd.Parameters.Add("@UserName", Username);
                allvehicles = vdm.SelectQuery(cmd).Tables[0];
                context.Session["allvehicles"] = allvehicles;
                vdm = null;
            }
            else
            {
                allvehicles = (DataTable)context.Session["allvehicles"];
            }
            int rowcount = 1;
            foreach (DataRow allvehiclesrow in allvehicles.Rows)
            {
                DataRow[] vehrows = dt.Select("VehicleID='" + allvehiclesrow["VehicleNumber"].ToString() + "'", "DateTime ASC");
                List<dataforpolyclass> vehiclelogslist = new List<dataforpolyclass>();
                foreach (DataRow dr in vehrows)
                {
                    dataforpolyclass vehlogs = new dataforpolyclass();
                    vehlogs.Sno = rowcount.ToString();
                    vehlogs.vehicleno = dr["VehicleID"].ToString();
                    vehlogs.vehicletype = "Car";
                    vehlogs.latitude = dr["Latitiude"].ToString();
                    vehlogs.longitude = dr["Longitude"].ToString();
                    vehlogs.direction = dr["Direction"].ToString();
                    vehlogs.speed = dr["Speed"].ToString();
                    vehlogs.datetime = dr["DateTime"].ToString();
                    vehlogs.odometer = dr["Odometer"].ToString();
                    if (dr["Status"].ToString() == "0" || dr["Status"].ToString() == "Running")
                        vehlogs.Status = "Running";
                    else if (dr["Status"].ToString() == "1" || dr["Status"].ToString() == "Stopped")
                        vehlogs.Status = "Stopped";
                    //  vehlogs.Reportsdata=
                    vehiclelogslist.Add(vehlogs);
                    rowcount++;
                }
                datalistclass vehlogslist = new datalistclass();
                vehlogslist.vehicleno = allvehiclesrow["VehicleNumber"].ToString();
                vehlogslist.logslist = vehiclelogslist;
                datalist.Add(vehlogslist);
            }

            if (datalist != null)
            {
                string respnceString = GetJson(datalist);
                context.Response.Write(respnceString);
            }
        }

        public class plantvehiclesdataclass
        {
            public int sno { get; set; }
            public string datetime { get; set; }
            public string vehicleno { get; set; }
            public string latitude { get; set; }
            public string longitude { get; set; }
            public string direction { get; set; }
            public string speed { get; set; }
            public string Status { get; set; }
            public string odometer { get; set; }
            public string plantname { get; set; }
        }

        public class plantdatalistclass
        {
            public string vehicleno { get; set; }
            public string routename { get; set; }
            public Dictionary<string, plantvehiclesdataclass> logslist = new Dictionary<string, plantvehiclesdataclass>();
            public List<plantvehiclesdataclass> listoflogs = new List<plantvehiclesdataclass>();
        }
        public class plantvehcountclass
        {
            public List<string> GlobelLooper = new List<string>();
            public int maxcount { get; set; }
            public List<plantdatalistclass> vehicleslist = new List<plantdatalistclass>();
            public List<BranchData> BranchDatalist = new List<BranchData>();
        }
        string mainuser = "";
        private void plantvehiclesdata(HttpContext context)
        {
            try
            {
                vdm = new VehicleDBMgr();
                vdm.InitializeDB();
                string Username = "";
                if (context.Session["field1"] != null)
                {
                    Username = context.Session["field1"].ToString();
                }
                else
                {
                    string responsestring = "Login.aspx";
                    string sendresponse = GetJson(new redirecturl() { responseurl = responsestring });
                    context.Response.Write(sendresponse);
                    return;
                }

                if (context.Session["main_user"] == null)
                {
                    cmd = new MySqlCommand("SELECT main_user FROM loginstable WHERE (loginid = @loginid)");
                    cmd.Parameters.Add("@loginid", Username);
                    DataTable mainusertbl = vdm.SelectQuery(cmd).Tables[0];
                    if (mainusertbl.Rows.Count > 0)
                    {
                        mainuser = mainusertbl.Rows[0]["main_user"].ToString();
                        context.Session["main_user"] = mainusertbl.Rows[0]["main_user"].ToString();
                    }
                    else
                    {
                        mainuser = Username;
                    }
                }
                else
                {
                    mainuser = context.Session["main_user"].ToString();
                }

                string plantname = context.Request["plantname"].ToString();
                string tripname = context.Request["tripname"].ToString();
                string startdt = context.Request["startdt"].ToString();
                string inputroutes = context.Request["routes"].ToString();
                string[] allroutes = new string[0];
                if (inputroutes.Length > 0)
                {
                    allroutes = inputroutes.Split('@');
                }
                string strngroutes = "";
                foreach (string str in allroutes)
                {
                    strngroutes += "'" + str + "',";
                }
                if (strngroutes.Length > 0)
                {
                    strngroutes = strngroutes.Remove(strngroutes.Length - 1, 1);
                }
                DateTime fromdate = DateTime.Today;
                DateTime todate = DateTime.Today;
                DateTime tripsdate = DateTime.Parse(startdt);
                if (tripname == "AM")
                {
                    fromdate = tripsdate.AddHours(4).AddMinutes(30);
                    todate = tripsdate.AddHours(11).AddMinutes(50);
                }
                else
                {
                    fromdate = tripsdate.AddHours(16).AddMinutes(30);
                    todate = tripsdate.AddHours(23).AddMinutes(50);
                }

                //cmd = new MySqlCommand("SELECT branchdata.Sno AS Refno,tripconfiguration.TripName as Tripid,paireddata.VehicleNumber, tripconfiguration.PlantName, routetable.RouteName, tripconfiguration.UserID, tripconfiguration.StartTime, tripconfiguration.EndTime, tripconfiguration.Status, routesubtable.Rank, branchdata.BranchID, branchdata.Latitude, branchdata.Longitude, branchdata.Radious, tripconfiguration.Veh_Sno , tripconfiguration.sno FROM            routesubtable INNER JOIN routetable ON routesubtable.SNo = routetable.SNo INNER JOIN tripconfiguration ON routetable.SNo = tripconfiguration.RouteID INNER JOIN branchdata ON routesubtable.LocationID = branchdata.Sno INNER JOIN paireddata ON tripconfiguration.UserID = paireddata.UserID AND tripconfiguration.Veh_Sno = paireddata.Sno WHERE        (tripconfiguration.StartTime > @d1) AND (tripconfiguration.StartTime < @d2) and (tripconfiguration.UserID = @UserID) AND (tripconfiguration.PlantName = @plantname) ORDER BY tripconfiguration.sno, routesubtable.Rank ");
                cmd = new MySqlCommand("SELECT branchdata.Sno AS Refno, tripconfiguration.TripName AS Tripid, paireddata.VehicleNumber, tripconfiguration.PlantName, routetable.RouteName, tripconfiguration.UserID, tripconfiguration.StartTime, tripconfiguration.EndTime, tripconfiguration.Status, routesubtable.Rank, branchdata.BranchID, branchdata.Latitude, branchdata.Longitude, branchdata.Radious, tripconfiguration.Veh_Sno, tripconfiguration.sno FROM routesubtable INNER JOIN routetable ON routesubtable.SNo = routetable.SNo INNER JOIN tripconfiguration ON routetable.SNo = tripconfiguration.RouteID INNER JOIN branchdata ON routesubtable.LocationID = branchdata.Sno INNER JOIN paireddata ON tripconfiguration.UserID = paireddata.UserID AND tripconfiguration.Veh_Sno = paireddata.Sno INNER JOIN loginstable ON tripconfiguration.UserID = loginstable.main_user WHERE (tripconfiguration.StartTime > @d1) AND (tripconfiguration.StartTime < @d2) AND (tripconfiguration.PlantName = @plantname) AND (loginstable.loginid = @UserID) ORDER BY tripconfiguration.sno, routesubtable.Rank");
                cmd.Parameters.Add("@UserID", Username);
                cmd.Parameters.Add("@plantname", plantname);
                cmd.Parameters.Add("@d1", fromdate);
                cmd.Parameters.Add("@d2", todate);
                DataTable dtStatustrip = vdm.SelectQuery(cmd).Tables[0];
                DataTable allvehiclesdata = new DataTable();
                allvehiclesdata.Columns.Add("PlantName");
                allvehiclesdata.Columns.Add("TripName");
                allvehiclesdata.Columns.Add("VehicleNumber");
                allvehiclesdata.Columns.Add("DateTime").DataType = typeof(DateTime);
                allvehiclesdata.Columns.Add("Speed");
                allvehiclesdata.Columns.Add("Latitiude");
                allvehiclesdata.Columns.Add("Longitude");
                allvehiclesdata.Columns.Add("Status");
                allvehiclesdata.Columns.Add("Direction");
                allvehiclesdata.Columns.Add("Odometer");
                allvehiclesdata.Columns.Add("RouteName");
                if (dtStatustrip.Rows.Count > 0)
                {
                    DataTable trips = dtStatustrip.DefaultView.ToTable(true, "Tripid", "VehicleNumber", "RouteName", "StartTime");
                    foreach (DataRow drTrip in trips.Rows)
                    {
                        string RouteName = drTrip["RouteName"].ToString();
                        int pos = Array.IndexOf(allroutes, RouteName);
                        if (pos > -1)
                        {
                            string VehicleNo = drTrip["VehicleNumber"].ToString();
                            DateTime trpstrtdt = DateTime.Now;
                            DateTime trpenddt = DateTime.Now;
                            DataTable ResDT = new DataTable();
                            DataRow[] branches = dtStatustrip.Select("RouteName='" + drTrip["RouteName"].ToString() + "'");
                            DataTable allbtanches = new DataTable();
                            allbtanches = branches.CopyToDataTable();
                            ResDT = getfirsttouchOdometer(VehicleNo, fromdate, todate, vdm, allbtanches, mainuser);
                            //ResDT = getfirsttouchOdometer(VehicleNo, trpstrtdt, trpenddt, vdm, routesubtabledata, mainuser);
                            if (ResDT.Rows.Count == 1)
                            {
                                trpstrtdt = fromdate;
                                trpenddt = todate;
                            }
                            else if (ResDT.Rows.Count == 2)
                            {
                                trpstrtdt = (DateTime)ResDT.Rows[0]["Reaching Date"];
                                trpenddt = (DateTime)ResDT.Rows[1]["Reaching Date"];
                            }
                            else
                            {
                                trpstrtdt = fromdate;
                                trpenddt = todate;
                            }
                            List<string> logstbls = new List<string>();
                            logstbls.Add("GpsTrackVehicleLogs");
                            logstbls.Add("GpsTrackVehicleLogs1");
                            logstbls.Add("GpsTrackVehicleLogs2");
                            logstbls.Add("GpsTrackVehicleLogs3");
                            DataTable logs = new DataTable();
                            DataTable tottable = new DataTable();
                            DataTable data = new DataTable();
                            foreach (string tbname in logstbls)
                            {
                                cmd = new MySqlCommand("SELECT cabmanagement.PlantName, tripconfiguration.TripName, paireddata.VehicleNumber, " + tbname + ".DateTime, " + tbname + ".Speed, " + tbname + ".Latitiude, " + tbname + ".Longitude, " + tbname + ".Status, " + tbname + ".Direction, " + tbname + ".Odometer, routetable.RouteName FROM cabmanagement INNER JOIN paireddata ON cabmanagement.VehicleID = paireddata.VehicleNumber AND cabmanagement.UserID = paireddata.UserID INNER JOIN tripconfiguration ON paireddata.Sno = tripconfiguration.Veh_Sno INNER JOIN " + tbname + " ON paireddata.VehicleNumber = " + tbname + ".VehicleID INNER JOIN routetable ON tripconfiguration.RouteID = routetable.SNo WHERE (paireddata.UserID = @UserName) AND (" + tbname + ".DateTime >= @d1) AND (" + tbname + ".DateTime <= @d2) AND (" + tbname + ".UserID = '" + mainuser + "') AND (paireddata.VehicleNumber = @VehicleNumber) ORDER BY paireddata.VehicleNumber");
                                cmd.Parameters.Add("@VehicleNumber", VehicleNo);
                                cmd.Parameters.Add("@d1", trpstrtdt);
                                cmd.Parameters.Add("@d2", trpenddt);
                                cmd.Parameters.Add("@UserName", mainuser);
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
                            data = dv.ToTable();

                            foreach (DataRow row in data.Rows)
                            {
                                allvehiclesdata.ImportRow(row);
                            }
                        }
                    }
                }
                DataTable vehicles = allvehiclesdata.DefaultView.ToTable(true, "VehicleNumber", "RouteName");
                plantvehcountclass vehdatalist = new plantvehcountclass();
                DataTable plants = new DataTable();
                if (allroutes.Length > 0)
                {
                    //cmd = new MySqlCommand(" SELECT branchdata.UserName, branchdata.BranchID, branchdata.Description, branchdata.Latitude, branchdata.Longitude, branchdata.PhoneNumber, tripdata.PlantName, branchdata_1.Latitude AS PlantLat, branchdata_1.Longitude AS PlantLong FROM routesubtable INNER JOIN routetable ON routesubtable.SNo = routetable.SNo INNER JOIN branchdata ON routesubtable.LocationID = branchdata.Sno INNER JOIN tripdata ON routetable.RouteName = tripdata.RouteName INNER JOIN branchdata branchdata_1 ON tripdata.PlantName = branchdata_1.BranchID WHERE (tripdata.RouteName IN (" + strngroutes + ")) AND (tripdata.PlantName = @PlantName) GROUP BY branchdata.BranchID");
                    cmd = new MySqlCommand("SELECT branchdata.UserName, branchdata.BranchID, branchdata.Description, branchdata.Latitude, branchdata.Longitude, branchdata.PhoneNumber, branchdata_1.Latitude AS PlantLat, branchdata_1.Longitude AS PlantLong, branchdata_1.BranchID AS PlantName FROM routesubtable INNER JOIN routetable ON routesubtable.SNo = routetable.SNo INNER JOIN branchdata ON routesubtable.LocationID = branchdata.Sno INNER JOIN tripconfiguration ON routetable.SNo = tripconfiguration.RouteID INNER JOIN branchdata branchdata_1 ON tripconfiguration.PlantName = branchdata_1.Sno WHERE (tripconfiguration.PlantName = @PlantName) and (routetable.RouteName IN (" + strngroutes + ")) GROUP BY branchdata.BranchID, branchdata_1.BranchID");
                }
                else
                {
                    //cmd = new MySqlCommand(" SELECT branchdata.UserName, branchdata.BranchID, branchdata.Description, branchdata.Latitude, branchdata.Longitude, branchdata.PhoneNumber, tripdata.PlantName, branchdata_1.Latitude AS PlantLat, branchdata_1.Longitude AS PlantLong FROM routesubtable INNER JOIN routetable ON routesubtable.SNo = routetable.SNo INNER JOIN branchdata ON routesubtable.LocationID = branchdata.Sno INNER JOIN tripdata ON routetable.RouteName = tripdata.RouteName INNER JOIN branchdata branchdata_1 ON tripdata.PlantName = branchdata_1.BranchID WHERE (tripdata.PlantName = @PlantName) GROUP BY branchdata.BranchID");
                    cmd = new MySqlCommand("SELECT branchdata.UserName, branchdata.BranchID, branchdata.Description, branchdata.Latitude, branchdata.Longitude, branchdata.PhoneNumber, branchdata_1.Latitude AS PlantLat, branchdata_1.Longitude AS PlantLong, branchdata_1.BranchID AS PlantName FROM routesubtable INNER JOIN routetable ON routesubtable.SNo = routetable.SNo INNER JOIN branchdata ON routesubtable.LocationID = branchdata.Sno INNER JOIN tripconfiguration ON routetable.SNo = tripconfiguration.RouteID INNER JOIN branchdata branchdata_1 ON tripconfiguration.PlantName = branchdata_1.Sno WHERE (tripconfiguration.PlantName = @PlantName) GROUP BY branchdata.BranchID, branchdata_1.BranchID");
                }
                cmd.Parameters.Add("@PlantName", plantname);
                plants = vdm.SelectQuery(cmd).Tables[0];
                //DataRow[] plntdata = plants.Select("BranchID='" + plantname + "'");
                List<BranchData> allbranches = new List<BranchData>();
                if (plants.Rows.Count > 0)
                {
                    BranchData brchdata = new BranchData();
                    brchdata.BranchName = plants.Rows[0]["PlantName"].ToString();
                    brchdata.latitude = plants.Rows[0]["PlantLat"].ToString();
                    brchdata.longitude = plants.Rows[0]["PlantLong"].ToString();
                    allbranches.Add(brchdata);
                    foreach (DataRow dr in plants.Rows)
                    {
                        brchdata = new BranchData();
                        brchdata.BranchName = dr["BranchID"].ToString();
                        brchdata.latitude = dr["Latitude"].ToString();
                        brchdata.longitude = dr["Longitude"].ToString();
                        allbranches.Add(brchdata);
                    }
                }
                vehdatalist.BranchDatalist = allbranches;
                //  2013-12-20T01:00
                DataView v = allvehiclesdata.DefaultView;
                allvehiclesdata.DefaultView.Sort = "DateTime asc";
                allvehiclesdata.DefaultView.ToTable();
                string mindate = allvehiclesdata.Compute("min(DateTime)", "").ToString();
                string maxdate = allvehiclesdata.Compute("max(DateTime)", "").ToString();
                DateTime dt1 = new DateTime();
                DateTime dt2 = new DateTime();
                DateTime.TryParse(mindate, out dt1);
                DateTime.TryParse(maxdate, out dt2);

                for (DateTime dttm = dt1; dt2 >= dttm; dttm = dttm.AddMinutes(1))
                {
                    vehdatalist.GlobelLooper.Add(dttm.ToString("dd/MM/yyyy HH:mm"));
                }

                //List<plantdatalistclass> datalist = new List<plantdatalistclass>();
                foreach (DataRow vehicle in vehicles.Rows)
                {
                    DataRow[] vehrows = allvehiclesdata.Select("VehicleNumber='" + vehicle["VehicleNumber"].ToString() + "'", "DateTime ASC");
                    Dictionary<string, plantvehiclesdataclass> plantvehiclesdatalist = new Dictionary<string, plantvehiclesdataclass>();
                    int sno = 0;
                    foreach (DataRow dr in vehrows)
                    {
                        plantvehiclesdataclass plantsdata = new plantvehiclesdataclass();
                        plantsdata.vehicleno = dr["VehicleNumber"].ToString();
                        plantsdata.datetime = dr["DateTime"].ToString();
                        plantsdata.speed = dr["Speed"].ToString();
                        plantsdata.latitude = dr["Latitiude"].ToString();
                        plantsdata.longitude = dr["Longitude"].ToString();
                        if (dr["Status"].ToString() == "0" || dr["Status"].ToString() == "Running")
                            plantsdata.Status = "Running";
                        else if (dr["Status"].ToString() == "1" || dr["Status"].ToString() == "Stopped")
                            plantsdata.Status = "Stopped";
                        plantsdata.direction = dr["Direction"].ToString();
                        plantsdata.odometer = dr["Odometer"].ToString();
                        plantsdata.plantname = dr["PlantName"].ToString();
                        DateTime tme = new DateTime();
                        DateTime.TryParse(dr["DateTime"].ToString(), out tme);
                        if (!plantvehiclesdatalist.Keys.Contains(tme.ToString("dd/MM/yyyy HH:mm")))
                        {
                            plantsdata.sno = sno++;

                            plantvehiclesdatalist.Add(tme.ToString("dd/MM/yyyy HH:mm"), plantsdata);
                        }
                    }

                    plantdatalistclass vehlogslist = new plantdatalistclass();
                    vehlogslist.vehicleno = vehicle["VehicleNumber"].ToString();
                    vehlogslist.routename = vehicle["RouteName"].ToString();
                    vehlogslist.logslist = plantvehiclesdatalist;
                    if (vehdatalist.maxcount < plantvehiclesdatalist.Count)
                    {
                        vehdatalist.maxcount = plantvehiclesdatalist.Count;
                    }
                    vehdatalist.vehicleslist.Add(vehlogslist);
                }
                if (vehdatalist != null)
                {
                    string respnceString = GetJson(vehdatalist);
                    context.Response.Write(respnceString);
                }
            }
            catch
            {
            }
        }


        private void getdata_compare(HttpContext context)
        {
            try
            {
                vdm = new VehicleDBMgr();
                vdm.InitializeDB();
                string Username = "";
                if (context.Session["field1"] != null)
                {
                    Username = context.Session["field1"].ToString();
                }
                else
                {
                    string responsestring = "Login.aspx";
                    string sendresponse = GetJson(new redirecturl() { responseurl = responsestring });
                    context.Response.Write(sendresponse);
                    return;
                }

                if (context.Session["main_user"] == null)
                {
                    cmd = new MySqlCommand("SELECT main_user FROM loginstable WHERE (loginid = @loginid)");
                    cmd.Parameters.Add("@loginid", Username);
                    DataTable mainusertbl = vdm.SelectQuery(cmd).Tables[0];
                    if (mainusertbl.Rows.Count > 0)
                    {
                        mainuser = mainusertbl.Rows[0]["main_user"].ToString();
                        context.Session["main_user"] = mainusertbl.Rows[0]["main_user"].ToString();
                    }
                    else
                    {
                        mainuser = Username;
                    }
                }
                else
                {
                    mainuser = context.Session["main_user"].ToString();
                }

                string startdt = context.Request["startdt"].ToString();
                string enddt = context.Request["enddt"].ToString();
                string checkedvehicle = context.Request["checkedvehicle"].ToString();

                DateTime fromdate = DateTime.Now;
                DateTime todate = DateTime.Now;
                fromdate = DateTime.Parse(startdt);
                todate = DateTime.Parse(enddt);
                //fromdate = GetLowDate(fromdate);
                //todate = GetHighDate(todate);
                List<string> logstbls = new List<string>();
                logstbls.Add("GpsTrackVehicleLogs");
                logstbls.Add("GpsTrackVehicleLogs1");
                logstbls.Add("GpsTrackVehicleLogs2");
                logstbls.Add("GpsTrackVehicleLogs3");
                DataTable logs = new DataTable();
                DataTable tottable = new DataTable();
                DataTable data = new DataTable();
                foreach (string tbname in logstbls)
                {
                    //cmd = new MySqlCommand("SELECT cabmanagement.PlantName, tripconfiguration.TripName, paireddata.VehicleNumber, " + tbname + ".DateTime, " + tbname + ".Speed, " + tbname + ".Latitiude, " + tbname + ".Longitude, " + tbname + ".Status, " + tbname + ".Direction, " + tbname + ".Odometer, routetable.RouteName FROM cabmanagement INNER JOIN paireddata ON cabmanagement.VehicleID = paireddata.VehicleNumber AND cabmanagement.UserID = paireddata.UserID INNER JOIN tripconfiguration ON paireddata.Sno = tripconfiguration.Veh_Sno INNER JOIN " + tbname + " ON paireddata.VehicleNumber = " + tbname + ".VehicleID INNER JOIN routetable ON tripconfiguration.RouteID = routetable.SNo WHERE (paireddata.UserID = @UserName) AND (" + tbname + ".DateTime >= @d1) AND (" + tbname + ".DateTime <= @d2) AND (" + tbname + ".UserID = '" + mainuser + "') AND (paireddata.VehicleNumber = @VehicleNumber) ORDER BY paireddata.VehicleNumber");
                    cmd = new MySqlCommand("SELECT paireddata.VehicleNumber as VehicleID, " + tbname + ".DateTime, " + tbname + ".Speed, " + tbname + ".Latitiude, " + tbname + ".Longitude, " + tbname + ".Status, " + tbname + ".Direction, " + tbname + ".Odometer FROM paireddata INNER JOIN " + tbname + " ON paireddata.VehicleNumber = " + tbname + ".VehicleID WHERE (paireddata.UserID = @UserName) AND (" + tbname + ".DateTime >= @d1) AND (" + tbname + ".DateTime <= @d2) AND (" + tbname + ".UserID = '" + mainuser + "') AND (paireddata.VehicleNumber = @VehicleNumber) ORDER BY paireddata.VehicleNumber");
                    cmd.Parameters.Add("@VehicleNumber", checkedvehicle);
                    cmd.Parameters.Add("@d1", fromdate);
                    cmd.Parameters.Add("@d2", todate);
                    cmd.Parameters.Add("@UserName", mainuser);
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
                data = dv.ToTable();

                List<logsclass> vehiclelogslist = new List<logsclass>();
                vdm = new VehicleDBMgr();
                vdm.InitializeDB();
                DataTable dt = data;
                DataTable selecteddata = dt;
                DataTable allvehicles = new DataTable();
                if (context.Session["allvehicles"] != null)
                {
                    allvehicles = (DataTable)context.Session["allvehicles"];
                }
                int rowcount = 1;
                foreach (DataRow dr in dt.Rows)
                {
                    logsclass vehlogs = new logsclass();
                    vehlogs.Sno = rowcount.ToString();
                    vehlogs.vehicleno = dr["VehicleID"].ToString();
                    if (allvehicles.Rows.Count > 0)
                    {
                        DataRow[] vehtypearray = allvehicles.Select("VehicleNumber='" + dr["VehicleID"].ToString() + "'");
                        try
                        {
                            vehlogs.vehicletype = vehtypearray[0]["VehicleType"].ToString();
                        }
                        catch
                        {
                        }
                        if (vehlogs.vehicletype == "Procurement")
                        {
                            vehlogs.vehicletype = "Car";
                        }
                    }
                    else
                    {
                        vehlogs.vehicletype = "Car";
                    }
                    vehlogs.latitude = dr["Latitiude"].ToString();
                    vehlogs.longitude = dr["Longitude"].ToString();
                    vehlogs.direction = dr["Direction"].ToString();
                    vehlogs.speed = dr["Speed"].ToString();
                    vehlogs.datetime = dr["DateTime"].ToString();
                    vehlogs.odometer = dr["Odometer"].ToString();
                    if (dr["Status"].ToString() == "0" || dr["Status"].ToString() == "Running")
                        vehlogs.Status = "Running";
                    else if (dr["Status"].ToString() == "1" || dr["Status"].ToString() == "Stopped")
                        vehlogs.Status = "Stopped";
                    //  vehlogs.Reportsdata=
                    vehiclelogslist.Add(vehlogs);
                    rowcount++;
                }

                if (vehiclelogslist != null)
                {
                    string respnceString = GetJson(vehiclelogslist);
                    context.Response.Write(respnceString);
                }
            }
            catch
            {
            }
        }

        private void plantvehiclesdata_gen(HttpContext context)
        {
            try
            {
                vdm = new VehicleDBMgr();
                vdm.InitializeDB();
                string Username = "";
                if (context.Session["field1"] != null)
                {
                    Username = context.Session["field1"].ToString();
                }
                else
                {
                    string responsestring = "Login.aspx";
                    string sendresponse = GetJson(new redirecturl() { responseurl = responsestring });
                    context.Response.Write(sendresponse);
                    return;
                }

                if (context.Session["main_user"] == null)
                {
                    cmd = new MySqlCommand("SELECT main_user FROM loginstable WHERE (loginid = @loginid)");
                    cmd.Parameters.Add("@loginid", Username);
                    DataTable mainusertbl = vdm.SelectQuery(cmd).Tables[0];
                    if (mainusertbl.Rows.Count > 0)
                    {
                        mainuser = mainusertbl.Rows[0]["main_user"].ToString();
                        context.Session["main_user"] = mainusertbl.Rows[0]["main_user"].ToString();
                    }
                    else
                    {
                        mainuser = Username;
                    }
                }
                else
                {
                    mainuser = context.Session["main_user"].ToString();
                }

                string startdt = context.Request["startdt"].ToString();
                string enddt = context.Request["enddt"].ToString();
                string checkedvehicle = context.Request["checkedvehicle"].ToString();
                string[] allvehicles = new string[0];
                if (checkedvehicle.Length > 0)
                {
                    allvehicles = checkedvehicle.Split('@');
                }
                DataTable allvehiclesdata = new DataTable();
                allvehiclesdata.Columns.Add("PlantName");
                allvehiclesdata.Columns.Add("TripName");
                allvehiclesdata.Columns.Add("VehicleNumber");
                allvehiclesdata.Columns.Add("DateTime").DataType = typeof(DateTime);
                allvehiclesdata.Columns.Add("Speed");
                allvehiclesdata.Columns.Add("Latitiude");
                allvehiclesdata.Columns.Add("Longitude");
                allvehiclesdata.Columns.Add("Status");
                allvehiclesdata.Columns.Add("Direction");
                allvehiclesdata.Columns.Add("Odometer");
                foreach (string VehicleNo in allvehicles)
                {
                    DateTime fromdate = DateTime.Parse(startdt);
                    DateTime todate = DateTime.Parse(enddt);
                    fromdate = GetLowDate(fromdate);
                    todate = GetHighDate(todate);
                    List<string> logstbls = new List<string>();
                    logstbls.Add("GpsTrackVehicleLogs");
                    logstbls.Add("GpsTrackVehicleLogs1");
                    logstbls.Add("GpsTrackVehicleLogs2");
                    logstbls.Add("GpsTrackVehicleLogs3");
                    DataTable logs = new DataTable();
                    DataTable tottable = new DataTable();
                    DataTable data = new DataTable();
                    foreach (string tbname in logstbls)
                    {
                        //cmd = new MySqlCommand("SELECT cabmanagement.PlantName, tripconfiguration.TripName, paireddata.VehicleNumber, " + tbname + ".DateTime, " + tbname + ".Speed, " + tbname + ".Latitiude, " + tbname + ".Longitude, " + tbname + ".Status, " + tbname + ".Direction, " + tbname + ".Odometer, routetable.RouteName FROM cabmanagement INNER JOIN paireddata ON cabmanagement.VehicleID = paireddata.VehicleNumber AND cabmanagement.UserID = paireddata.UserID INNER JOIN tripconfiguration ON paireddata.Sno = tripconfiguration.Veh_Sno INNER JOIN " + tbname + " ON paireddata.VehicleNumber = " + tbname + ".VehicleID INNER JOIN routetable ON tripconfiguration.RouteID = routetable.SNo WHERE (paireddata.UserID = @UserName) AND (" + tbname + ".DateTime >= @d1) AND (" + tbname + ".DateTime <= @d2) AND (" + tbname + ".UserID = '" + mainuser + "') AND (paireddata.VehicleNumber = @VehicleNumber) ORDER BY paireddata.VehicleNumber");
                        cmd = new MySqlCommand("SELECT paireddata.VehicleNumber, " + tbname + ".DateTime, " + tbname + ".Speed, " + tbname + ".Latitiude, " + tbname + ".Longitude, " + tbname + ".Status, " + tbname + ".Direction, " + tbname + ".Odometer FROM paireddata INNER JOIN " + tbname + " ON paireddata.VehicleNumber = " + tbname + ".VehicleID WHERE (paireddata.UserID = @UserName) AND (" + tbname + ".DateTime >= @d1) AND (" + tbname + ".DateTime <= @d2) AND (" + tbname + ".UserID = '" + mainuser + "') AND (paireddata.VehicleNumber = @VehicleNumber) ORDER BY paireddata.VehicleNumber");
                        cmd.Parameters.Add("@VehicleNumber", VehicleNo);
                        cmd.Parameters.Add("@d1", fromdate);
                        cmd.Parameters.Add("@d2", todate);
                        cmd.Parameters.Add("@UserName", mainuser);
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
                    data = dv.ToTable();

                    foreach (DataRow row in data.Rows)
                    {
                        allvehiclesdata.ImportRow(row);
                    }
                }

                DataTable vehicles = allvehiclesdata.DefaultView.ToTable(true, "VehicleNumber");
                plantvehcountclass vehdatalist = new plantvehcountclass();
                //  2013-12-20T01:00
                DataView v = allvehiclesdata.DefaultView;
                allvehiclesdata.DefaultView.Sort = "DateTime asc";
                allvehiclesdata.DefaultView.ToTable();
                string mindate = allvehiclesdata.Compute("min(DateTime)", "").ToString();
                string maxdate = allvehiclesdata.Compute("max(DateTime)", "").ToString();
                DateTime dt1 = new DateTime();
                DateTime dt2 = new DateTime();
                DateTime.TryParse(mindate, out dt1);
                DateTime.TryParse(maxdate, out dt2);

                for (DateTime dttm = dt1; dt2 >= dttm; dttm = dttm.AddMinutes(1))
                {
                    vehdatalist.GlobelLooper.Add(dttm.ToString("dd/MM/yyyy HH:mm"));
                }

                //List<plantdatalistclass> datalist = new List<plantdatalistclass>();
                foreach (DataRow vehicle in vehicles.Rows)
                {
                    DataRow[] vehrows = allvehiclesdata.Select("VehicleNumber='" + vehicle["VehicleNumber"].ToString() + "'", "DateTime ASC");
                    Dictionary<string, plantvehiclesdataclass> plantvehiclesdatalist = new Dictionary<string, plantvehiclesdataclass>();
                    int sno = 0;
                    foreach (DataRow dr in vehrows)
                    {
                        plantvehiclesdataclass plantsdata = new plantvehiclesdataclass();
                        plantsdata.vehicleno = dr["VehicleNumber"].ToString();
                        plantsdata.datetime = dr["DateTime"].ToString();
                        plantsdata.speed = dr["Speed"].ToString();
                        plantsdata.latitude = dr["Latitiude"].ToString();
                        plantsdata.longitude = dr["Longitude"].ToString();
                        if (dr["Status"].ToString() == "0" || dr["Status"].ToString() == "Running")
                            plantsdata.Status = "Running";
                        else if (dr["Status"].ToString() == "1" || dr["Status"].ToString() == "Stopped")
                            plantsdata.Status = "Stopped";
                        plantsdata.direction = dr["Direction"].ToString();
                        plantsdata.odometer = dr["Odometer"].ToString();
                        DateTime tme = new DateTime();
                        DateTime.TryParse(dr["DateTime"].ToString(), out tme);
                        if (!plantvehiclesdatalist.Keys.Contains(tme.ToString("dd/MM/yyyy HH:mm")))
                        {
                            plantsdata.sno = sno++;
                            plantvehiclesdatalist.Add(tme.ToString("dd/MM/yyyy HH:mm"), plantsdata);
                        }
                    }

                    plantdatalistclass vehlogslist = new plantdatalistclass();
                    vehlogslist.vehicleno = vehicle["VehicleNumber"].ToString();
                    vehlogslist.logslist = plantvehiclesdatalist;
                    if (vehdatalist.maxcount < plantvehiclesdatalist.Count)
                    {
                        vehdatalist.maxcount = plantvehiclesdatalist.Count;
                    }
                    vehdatalist.vehicleslist.Add(vehlogslist);
                }
                if (vehdatalist != null)
                {
                    string respnceString = GetJson(vehdatalist);
                    context.Response.Write(respnceString);
                }
            }
            catch
            {
            }
        }
        public static DataTable getfirsttouchOdometer(string vehiclestr, DateTime fromdate, DateTime todate, VehicleDBMgr vdm, DataTable BranchDetails, string mainuser)
        {
            bool iscompleted = false;
            MySqlCommand cmd;
            #region location wise Reports
            DateTime Startingdt = DateTime.Now;
            int sno = 1;
            DataTable summeryTable = new DataTable();
            DataColumn summeryColumn = new DataColumn("SNo");
            summeryTable.Columns.Add(summeryColumn);
            summeryColumn = new DataColumn("From Location");
            summeryTable.Columns.Add(summeryColumn);
            //summeryColumn = new DataColumn("Reaching Date").DataType = typeof(DateTime);
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
            DataTable TripData = new DataTable();
            List<string> logstbls = new List<string>();
            logstbls.Add("GpsTrackVehicleLogs");
            logstbls.Add("GpsTrackVehicleLogs1");
            logstbls.Add("GpsTrackVehicleLogs2");
            logstbls.Add("GpsTrackVehicleLogs3");
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
            DataTable finaltable = new DataTable();
            if (summeryTable.Rows.Count > 0)
            {
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
                    Prevrow = tripdatarow;
                }
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
                }
            }
            #endregion
            return finaltable;
        }

        public class getplantsandtripsclass
        {
            public List<string> plants = new List<string>();
            public List<string> routes = new List<string>();
        }

        private void getplantsandtrips(HttpContext context)
        {
            vdm = new VehicleDBMgr();
            vdm.InitializeDB();
            string Username = context.Session["field1"].ToString();
            string plant = context.Request["plant"].ToString();
            getplantsandtripsclass Getriplist = new getplantsandtripsclass();

            DataTable totaldata = new DataTable();
            if (plant == "Select Plant" || plant == "ALL")
            {
                cmd = new MySqlCommand("SELECT cabmanagement.PlantName, cabmanagement.VehicleID, branchdata.Sno, routetable.RouteName FROM cabmanagement INNER JOIN loginsconfigtable ON cabmanagement.VehicleID = loginsconfigtable.VehicleID INNER JOIN loginstable ON cabmanagement.UserID = loginstable.main_user AND loginsconfigtable.Refno = loginstable.refno INNER JOIN branchdata ON cabmanagement.PlantName = branchdata.BranchID INNER JOIN routetable ON branchdata.Sno = routetable.Plant WHERE (loginstable.loginid = @UserName)");
            }
            else
            {
                cmd = new MySqlCommand("SELECT cabmanagement.PlantName, cabmanagement.VehicleID, branchdata.Sno, routetable.RouteName FROM cabmanagement INNER JOIN loginsconfigtable ON cabmanagement.VehicleID = loginsconfigtable.VehicleID INNER JOIN loginstable ON cabmanagement.UserID = loginstable.main_user AND loginsconfigtable.Refno = loginstable.refno INNER JOIN branchdata ON cabmanagement.PlantName = branchdata.BranchID INNER JOIN routetable ON branchdata.Sno = routetable.Plant WHERE (loginstable.loginid = @UserName) AND (branchdata.BranchID = @plantname)");
            }
            cmd.Parameters.Add("@plantname", plant);
            cmd.Parameters.Add("@UserName", Username);
            totaldata = vdm.SelectQuery(cmd).Tables[0];
            DataView view = new DataView(totaldata);
            DataTable dtPlantName = view.ToTable(true, "PlantName", "SNo");
            List<string> plantslist = new List<string>();

            //cmd = new MySqlCommand("SELECT routetable.RouteName, tripconfiguration.StartTime, tripconfiguration.EndTime, routetable.SNo, tripconfiguration.Kms, tripconfiguration.extrakms, tripconfiguration.Chargeperkm FROM tripconfiguration INNER JOIN branchdata ON tripconfiguration.PlantName = branchdata.Sno INNER JOIN cabmanagement ON branchdata.BranchID = cabmanagement.PlantName INNER JOIN loginsconfigtable ON cabmanagement.VehicleID = loginsconfigtable.VehicleID INNER JOIN loginstable ON loginsconfigtable.Refno = loginstable.refno INNER JOIN routetable ON tripconfiguration.RouteID = routetable.SNo WHERE (loginstable.loginid = @userid) GROUP BY routetable.RouteName");
            //cmd = new MySqlCommand("SELECT routetable.RouteName, tripconfiguration.StartTime, tripconfiguration.EndTime, routetable.SNo, tripconfiguration.Kms, tripconfiguration.extrakms, tripconfiguration.Chargeperkm FROM tripconfiguration INNER JOIN branchdata ON tripconfiguration.PlantName = branchdata.Sno INNER JOIN cabmanagement ON branchdata.BranchID = cabmanagement.PlantName INNER JOIN loginsconfigtable ON cabmanagement.VehicleID = loginsconfigtable.VehicleID INNER JOIN loginstable ON loginsconfigtable.Refno = loginstable.refno INNER JOIN routetable ON tripconfiguration.RouteID = routetable.SNo WHERE (loginstable.loginid = @userid) AND (branchdata.BranchID = @plantname) GROUP BY routetable.RouteName");
            //cmd.Parameters.Add("@plantname", plant);
            //cmd.Parameters.Add("@userid", Username);
            //DataTable routetabledata = vdm.SelectQuery(cmd).Tables[0];

            foreach (DataRow drr in dtPlantName.Rows)
            {
                plantslist.Add(drr["PlantName"].ToString() + "@" + drr["SNo"].ToString());
            }
            DataTable dtroutes = view.ToTable(true, "RouteName");
            List<string> routeslist = new List<string>();
            foreach (DataRow drr in dtroutes.Rows)
            {
                routeslist.Add(drr["RouteName"].ToString());
            }
            Getriplist.plants = plantslist;
            Getriplist.routes = routeslist;
            if (Getriplist != null)
            {
                string respnceString = GetJson(Getriplist);
                context.Response.Write(respnceString);
            }
        }
        private void GetNotifications(HttpContext context)
        {
        }
        private void getnotificationvehicles(HttpContext context)
        {
            try
            {
                vdm = new VehicleDBMgr();
                vdm.InitializeDB();
                List<getnotificationvehiclesclass> getvehicles = new List<getnotificationvehiclesclass>();
                string Username = "";
                if (context.Session["field1"] != null)
                {
                    Username = context.Session["field1"].ToString();
                }
                else
                {
                    string responsestring = "Login.aspx";
                    string sendresponse = GetJson(new redirecturl() { responseurl = responsestring });
                    context.Response.Write(sendresponse);
                    return;
                }
                cmd = new MySqlCommand("SELECT paireddata.VehicleNumber, loginstable.main_user FROM loginsconfigtable INNER JOIN paireddata ON loginsconfigtable.VehicleID = paireddata.VehicleNumber INNER JOIN loginstable ON paireddata.UserID = loginstable.main_user AND loginsconfigtable.Refno = loginstable.refno INNER JOIN alert_assignment_vehicles ON paireddata.Sno = alert_assignment_vehicles.vehicle_Sno WHERE (loginstable.loginid = @UserName) GROUP BY paireddata.VehicleNumber");
                cmd.Parameters.Add("@UserName", Username);
                DataTable Vehiclesdata = vdm.SelectQuery(cmd).Tables[0];
                vdm = null;
                foreach (DataRow dr in Vehiclesdata.Rows)
                {
                    getnotificationvehiclesclass veh = new getnotificationvehiclesclass();
                    string vehicleno = dr["VehicleNumber"].ToString();
                    if (!vehicleno.Contains('/'))
                    {
                        char[] charsToTrim = { ' ' };
                        vehicleno = vehicleno.Trim(charsToTrim);
                    }
                    else
                    {
                        char[] charsToTrim = { '/' };
                        vehicleno = vehicleno.Remove(7, 1);
                    }
                    vehicleno = vehicleno.Replace(" ", "");
                    veh.vehicleno = vehicleno;
                    getvehicles.Add(veh);
                }

                if (getvehicles != null)
                {
                    string respnceString = GetJson(getvehicles);
                    context.Response.Write(respnceString);
                }
            }
            catch
            {
            }
        }
        private class getvehlogsclass
        {
            public string vehicleid { get; set; }
            public string aletdt { get; set; }
            public string alertinfo { get; set; }
            public string alerttype { get; set; }
        }
        private void getvehnotification(HttpContext context)
        {
            try
            {
                vdm = new VehicleDBMgr();
                vdm.InitializeDB();
                List<getvehlogsclass> getvehicles = new List<getvehlogsclass>();
                string vehno = context.Request["VehicleNo"].ToString();
                string Username = "";
                if (context.Session["field1"] != null)
                {
                    Username = context.Session["field1"].ToString();
                }
                else
                {
                    string responsestring = "Login.aspx";
                    string sendresponse = GetJson(new redirecturl() { responseurl = responsestring });
                    context.Response.Write(sendresponse);
                    return;
                }
                cmd = new MySqlCommand("SELECT alert_logs.sno, alert_logs.userid, alert_logs.vehicleid, alert_logs.aletdt, alert_logs.alertinfo, alert_logs.alerttype, alert_logs.servertime,paireddata.VehicleNumber FROM alert_logs INNER JOIN loginstable ON alert_logs.userid = loginstable.refno INNER JOIN paireddata ON alert_logs.vehicleid = paireddata.Sno WHERE (loginstable.loginid = @userid) AND (paireddata.VehicleNumber = @vehicleid) ORDER BY alert_logs.aletdt desc");
                cmd.Parameters.Add("@userid", Username);
                cmd.Parameters.Add("@vehicleid", vehno);
                DataTable Vehiclesdata = vdm.SelectQuery(cmd).Tables[0];
                vdm = null;
                foreach (DataRow dr in Vehiclesdata.Rows)
                {
                    getvehlogsclass veh = new getvehlogsclass();
                    veh.vehicleid = dr["VehicleNumber"].ToString();
                    veh.aletdt = dr["aletdt"].ToString();
                    veh.alertinfo = dr["alertinfo"].ToString();
                    veh.alerttype = dr["alerttype"].ToString();
                    getvehicles.Add(veh);
                }

                if (getvehicles != null)
                {
                    string respnceString = GetJson(getvehicles);
                    context.Response.Write(respnceString);
                }
            }
            catch
            {
            }
        }

        private void EmployeeManageSaveClick(HttpContext context)
        {
            try
            {
                vdm = new VehicleDBMgr();
                vdm.InitializeDB();
                string UserName = context.Session["field1"].ToString();
                string OperatorName = context.Session["field1"].ToString();
                List<string> VehicleTypeDet = new List<string>();
                string txtEmployeeID = context.Request["txtEmployeeID"];
                string txtEmployeeName = context.Request["txtEmployeeName"];
                string txtAddress = context.Request["txtAddress"];
                string txtPhoneNo = context.Request["txtPhoneNo"];
                string txtPAddress = context.Request["txtPAddress"];
                string txtPPhoneNo = context.Request["txtPPhoneNo"];
                string txtDOB = context.Request["txtDOB"];
                string txtLicenceNo = context.Request["txtLicenceNo"];
                string txtBloodGroup = context.Request["txtBloodGroup"];
                string txtLNExpiryDate = context.Request["txtINExpiryDate"];
                string txtQualification = context.Request["txtQualification"];
                string txtBadgeNo = context.Request["txtBadgeNo"];
                string txtExperience = context.Request["txtExperience"];
                string txtBNExpiryDate = context.Request["txtRCExpiryDate"];
                string txtDOJ = context.Request["txtDOJ"];
                string txtRemarks = context.Request["txtRemarks"];
                string emptype = context.Request["emptype"];
                string empstatus = context.Request["empstatus"];
                string btnSave = context.Request["btnSave"];
                if (btnSave == "Save")
                {
                    cmd = new MySqlCommand("insert into employeestable(UserName, EmployeeID, EmployeeName, EmployeeType, DoB, Address, phoneNumber, PermanentAddress, PermanentPhoneNo, BloodGroup, Qualification, Experience, DOJ, LicenseNo, LicenseNoExpDT, Remarks, OperatorName,Status) values (@UserName, @EmployeeID, @EmployeeName, @EmployeeType, @DoB, @Address, @phoneNumber, @PermanentAddress, @PermanentPhoneNo, @BloodGroup, @Qualification, @Experience, @DOJ, @LicenseNo, @LicenseNoExpDT, @Remarks, @OperatorName,@Status)");
                    cmd.Parameters.Add("@UserName", UserName);
                    cmd.Parameters.Add("@EmployeeID", txtEmployeeID);
                    cmd.Parameters.Add("@EmployeeName", txtEmployeeName);
                    cmd.Parameters.Add("@EmployeeType", emptype);
                    cmd.Parameters.Add("@DoB", txtDOB);
                    cmd.Parameters.Add("@Address", txtAddress);
                    cmd.Parameters.Add("@phoneNumber", txtPhoneNo);
                    cmd.Parameters.Add("@PermanentAddress", txtPAddress);
                    cmd.Parameters.Add("@PermanentPhoneNo", txtPPhoneNo);
                    cmd.Parameters.Add("@BloodGroup", txtBloodGroup);
                    cmd.Parameters.Add("@Qualification", txtQualification);
                    cmd.Parameters.Add("@Experience", txtExperience);
                    cmd.Parameters.Add("@DOJ", txtDOJ);
                    cmd.Parameters.Add("@LicenseNo", txtLicenceNo);
                    cmd.Parameters.Add("@LicenseNoExpDT", txtLNExpiryDate);
                    //cmd.Parameters.Add("@BadgeNo", txtBadgeNo);
                    //cmd.Parameters.Add("@BadgeNoExpDT", txtBNExpiryDate);
                    cmd.Parameters.Add("@Remarks", txtRemarks);
                    cmd.Parameters.Add("@OperatorName", OperatorName);
                    cmd.Parameters.Add("@Status", empstatus);
                    vdm.insert(cmd);
                    string msg = "Driver Data Saved Successfully";
                    VehicleTypeDet.Add(msg);
                    string response = GetJson(VehicleTypeDet);
                    context.Response.Write(response);
                }
                else if (btnSave == "Edit")
                {
                    cmd = new MySqlCommand("update employeestable set  EmployeeName=@EmployeeName,EmployeeType=@EmployeeType,DoB=@DoB,Address=@Address,phoneNumber=@phoneNumber,PermanentAddress=@PermanentAddress,PermanentPhoneNo=@PermanentPhoneNo, BloodGroup=@BloodGroup,Qualification=@Qualification,Experience=@Experience,DOJ=@DOJ,LicenseNo=@LicenseNo,LicenseNoExpDT=@LicenseNoExpDT,Remarks=@Remarks,OperatorName=@OperatorName,Status=@Status where UserName=@UserName and EmployeeID=@EmployeeID");
                    cmd.Parameters.Add("@UserName", UserName);
                    cmd.Parameters.Add("@EmployeeID", txtEmployeeID);
                    cmd.Parameters.Add("@EmployeeName", txtEmployeeName);
                    cmd.Parameters.Add("@EmployeeType", emptype);
                    cmd.Parameters.Add("@DoB", txtDOB);
                    cmd.Parameters.Add("@Address", txtAddress);
                    cmd.Parameters.Add("@phoneNumber", txtPhoneNo);
                    cmd.Parameters.Add("@PermanentAddress", txtPAddress);
                    cmd.Parameters.Add("@PermanentPhoneNo", txtPPhoneNo);
                    cmd.Parameters.Add("@BloodGroup", txtBloodGroup);
                    cmd.Parameters.Add("@Qualification", txtQualification);
                    cmd.Parameters.Add("@Experience", txtExperience);
                    cmd.Parameters.Add("@DOJ", txtDOJ);
                    cmd.Parameters.Add("@LicenseNo", txtLicenceNo);
                    cmd.Parameters.Add("@LicenseNoExpDT", txtLNExpiryDate);
                    //cmd.Parameters.Add("@BadgeNo", txtBadgeNo);
                    //cmd.Parameters.Add("@BadgeNoExpDT", txtBNExpiryDate);
                    cmd.Parameters.Add("@Remarks", txtRemarks);
                    cmd.Parameters.Add("@OperatorName", OperatorName);
                    cmd.Parameters.Add("@Status", empstatus);
                    vdm.Update(cmd);
                    string msg = "Employee Data Updated Successfully";
                    VehicleTypeDet.Add(msg);
                    //MessageBox.Show("VehicleType Deatails Sccessfully Added");
                    string response = GetJson(VehicleTypeDet);
                    context.Response.Write(response);
                }
            }
            catch
            {
                string response = GetJson("Error! Please try again later");
                context.Response.Write(response);
            }
        }

        private void UpdateEmployeeManamentDetails(HttpContext context)
        {
            try
            {
                List<EmployeeManamentclass> EmployeeManamentlist = new List<EmployeeManamentclass>();
                vdm = new VehicleDBMgr();
                vdm.InitializeDB();
                string UserName = context.Session["field1"].ToString();
                cmd = new MySqlCommand("select EmployeeID, EmployeeName, EmployeeType,DoB,Address,phoneNumber,PermanentAddress,PermanentPhoneNo,BloodGroup,Qualification,Experience,DOJ,LicenseNo,LicenseNoExpDT,Remarks,OperatorName,Status from employeestable where UserName=@UserName and (EmployeeType='Driver' or EmployeeType='Helper')");
                cmd.Parameters.Add("@UserName", UserName);
                DataTable dtEmployeeManament = vdm.SelectQuery(cmd).Tables[0];
                foreach (DataRow dr in dtEmployeeManament.Rows)
                {
                    EmployeeManamentclass EmployeeManamentDet = new EmployeeManamentclass();
                    EmployeeManamentDet.EmployeeID = dr["EmployeeID"].ToString();
                    EmployeeManamentDet.EmployeeName = dr["EmployeeName"].ToString();
                    EmployeeManamentDet.EmployeeType = dr["EmployeeType"].ToString();
                    EmployeeManamentDet.DoB = dr["DoB"].ToString();
                    EmployeeManamentDet.Address = dr["Address"].ToString();
                    EmployeeManamentDet.phoneNumber = dr["phoneNumber"].ToString();
                    EmployeeManamentDet.PermanentAddress = dr["PermanentAddress"].ToString();
                    EmployeeManamentDet.BloodGroup = dr["BloodGroup"].ToString();
                    EmployeeManamentDet.Qualification = dr["Qualification"].ToString();
                    EmployeeManamentDet.Experience = dr["Experience"].ToString();
                    DateTime MyDateTime = Convert.ToDateTime(dr["DOJ"].ToString());
                    string DOJ = MyDateTime.ToString("yyyy-MM-dd");
                    EmployeeManamentDet.DOJ = DOJ;
                    EmployeeManamentDet.LicenseNo = dr["LicenseNo"].ToString();
                    DateTime ExpDT = Convert.ToDateTime(dr["LicenseNoExpDT"].ToString());
                    string LicenseNoExpDT = ExpDT.ToString("yyyy-MM-dd");
                    EmployeeManamentDet.LicenseNoExpDT = LicenseNoExpDT;
                    EmployeeManamentDet.PermanentPhoneNo = dr["PermanentPhoneNo"].ToString();
                    
                    //EmployeeManamentDet.BadgeNo = dr["BadgeNo"].ToString();
                    //EmployeeManamentDet.BadgeNoExpDT = dr["BadgeNoExpDT"].ToString();
                    EmployeeManamentDet.Remarks = dr["Remarks"].ToString();
                    EmployeeManamentDet.OperatorName = dr["OperatorName"].ToString();
                    EmployeeManamentDet.Status = dr["Status"].ToString();
                    EmployeeManamentlist.Add(EmployeeManamentDet);
                }
                if (EmployeeManamentlist != null)
                {
                    string response = GetJson(EmployeeManamentlist);
                    context.Response.Write(response);
                }
            }
            catch
            {
                string response = GetJson("Error! Please try again later");
                context.Response.Write(response);
            }
        }

        private void EmployeeManamentDeleteDetails(HttpContext context)
        {
            try
            {
                vdm = new VehicleDBMgr();
                vdm.InitializeDB();
                string UserName = context.Session["field1"].ToString();
                string OperatorName = context.Session["field1"].ToString();
                List<string> VehicleTypeDet = new List<string>();
                string txtEmployeeID = context.Request["txtEmployeeID"];
                cmd = new MySqlCommand("delete from employeestable where UserName=@UserName and EmployeeID=@EmployeeID");
                cmd.Parameters.Add("@UserName", UserName);
                cmd.Parameters.Add("@EmployeeID", txtEmployeeID);
                vdm.Delete(cmd);
                string msg = "Employee Data Deleted Successfully";
                VehicleTypeDet.Add(msg);
                string response = GetJson(VehicleTypeDet);
                context.Response.Write(response);
            }
            catch
            {
                string response = GetJson("Error! Please try again later");
                context.Response.Write(response);
            }
        }
        public class EmployeeManamentclass
        {
            public string EmployeeID { set; get; }
            public string EmployeeName { set; get; }
            public string EmployeeType { set; get; }
            public string DoB { set; get; }
            public string Address { set; get; }
            public string phoneNumber { set; get; }
            public string PermanentAddress { set; get; }
            public string PermanentPhoneNo { set; get; }
            public string BloodGroup { set; get; }
            public string Qualification { set; get; }
            public string Experience { set; get; }
            public string DOJ { set; get; }
            public string LicenseNo { set; get; }
            public string LicenseNoExpDT { set; get; }
            //public string BadgeNo { set; get; }
            //public string BadgeNoExpDT { set; get; }
            public string Remarks { set; get; }
            public string OperatorName { set; get; }
            public string Status { set; get; }
        }
    }
}

  

//ReplayRoutes.aspx
//MasterCss/style.css
//js/jquery-1.4.4.js
//js/jquery-ui-1.10.3.custom.min.js
//js/jquery.blockUI.js
//js/jquery.json-2.4.js
//js/jquery.nivo.slider.pack.js
//js/JTemplate.js
//js/sortable.js
//js/WdatePicker.js
//Style.css
//Style123.css
//Images/TitleLogo.png
//thumbnails/loading.gif
//CSS/ButtonStyles.css
//DropDownCheckList.js
//NETWORK:
//WebResource.axd
//WebResource.axd
//ScriptResource.axd
//ScriptResource.axd
// https://maps.googleapis.com
// MasterCss/images/navigation.png  
// MasterCss/images/down-arrow.gif  
// Bus.axd
// https://csi.gstatic.com
