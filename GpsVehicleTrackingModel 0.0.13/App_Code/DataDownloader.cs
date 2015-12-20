using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.NetworkInformation;
using System.Threading;
using System.Data;
using System.Drawing;
using System.Resources;
using System.Globalization;
//using Demo.WindowsForms;
using MySql.Data.MySqlClient;
using GPSApplication;


   public class DataDownloader
    {
         VehicleDBMgr vdm;

       public DataDownloader()
       {
           vdm = new VehicleDBMgr();
       }
        public bool StartDownload = false;
        //  public void UploadData()
        //{
        //    bool result = false;
        //    bool prevresult = false;

        //    result = _ping();

        //    if (result)
        //    {
        //        //result = _ping();
        //        prevresult = result;

        //        foreach (DataRow dr in VehicleDBClass.VehicleDBMngr.Select("TempWeighTransactions", "*"))
        //        {
        //            if (VehicleASnTechDbClass.VehicleASnTechDBMngr.checkDBConn())
        //            {
        //                VehicleASnTechDbClass.VehicleASnTechDBMngr.insert("WeighingTransactions", new TransactionDetailsMgr.TransationDetails(dr));
        //                VehicleDBClass.VehicleDBMngr.Delete("TempWeighTransactions", "TransactionNo=" + dr[1].ToString());
        //                //VehicleASnTechDbClass.VehicleASnTechDBMngr.Delete("WeighingTransactions", "TransactionNo=" + dr[1].ToString());
        //            }
        //        }
        //    }
        //    while (StartUPload)
        //    {
        //        try
        //        {
        //            result = _ping();
        //            if (!result)
        //                prevresult = false;

        //            if ((!(prevresult == result)) && result == true)
        //            {
        //                prevresult = result;
        //                foreach (DataRow dr in LocalDBManager.SelectQuery("TempWeighTransactions", "*").Tables[0].Rows)
        //                {
        //                    //if (vdm.checkDBConn())
        //                    if(_ping())
        //                    {
        //                        DataTable dt = vdm.SelectQuery("GpsTrackVehicleLogs", "*", new string[] { "UserName=@UserName" }, new string[] { UserName }, new string[] { "" }).Tables[0];
        //                        //vdm.SelectQuery("WeighingTransactions", new TransactionDetailsMgr.TransationDetails(dr));
        //                        //VehicleDBClass.VehicleDBMngr.Delete("TempWeighTransactions", "TransactionNo=" + dr[1].ToString());
        //                    }
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {

        //        }
        //        Thread.Sleep(500);
        //    }
        //}
       public List<string> vehicleList = new List<string>();
        public class DataClass
        {
           public string[] data = null;
        }
        public delegate void RemarkEventHandler(DataClass data);
        public   event RemarkEventHandler RemarkEvent;
         Dictionary<string, LogDetails> LogDetailsList = new Dictionary<string, LogDetails>();
       public delegate void ErrorReportsHandler(string msg);
       public   event ErrorReportsHandler ErrorReports;
        class LogDetails
        {
            internal double prvLat;//.Text = dr[2].ToString();
            internal double prvLogitude;//.Text = dr[3].ToString();
            internal double preLat;//.Text = dr[2].ToString();
            internal double preLogitude;
            internal double Speed;//.Text = dr[4].ToString();
            internal double Diesel;
            internal DateTime predatetime;//.Text = String.Format("{0:d}",DateTime.Parse(dr[5].ToString()));
            internal DateTime prvdatetime;//.Text = String.Format("{0:d}",DateTime.Parse(dr[5].ToString()));
            //lbl_Time.Text = DateTime.Parse(dr[5].ToString()).ToLongTimeString();
            internal string VehicleNumber;//.Text = dr[1].ToString();
            internal string DriverName;

            internal double stopedInterval;
            internal double IdleInterval;
            internal double MovingInterval;

        }

         public Bitmap rotateImage(Bitmap b, float angle)
        {
            //create a new empty bitmap to hold rotated image
            Bitmap returnBitmap = new Bitmap(b.Width, b.Height);
            //make a graphics object from the empty bitmap
            Graphics g = Graphics.FromImage(returnBitmap);
            //move rotation point to center of image
            g.TranslateTransform((float)b.Width / 2, (float)b.Height / 2);
            //rotate
            g.RotateTransform(angle);
            //move image back
            g.TranslateTransform(-(float)b.Width / 2, -(float)b.Height / 2);
            //draw passed in image onto graphics object
            g.DrawImage(b, new Point(0, 0));
            return returnBitmap;
        }
        //  internal void downloadData()
        //{
        //    bool result = false;
        //    bool prevresult = false;
           

        //    while (StartDownload)
        //    {
        //        //try
        //        //{
        //        //    result = _ping();
        //        //    if (!result)
        //        //        prevresult = false;

        //        //    if ((!(prevresult==result))&&result==true)
        //        //    {
        //        //        prevresult=result;

        //        result = _ping();

        //        try
        //        {
        //            if (result)
        //            {
        //                foreach (string vehicleID in vehicleList)
        //                {
        //                    //result = _ping();
        //                    //prevresult = result;
        //                    DataSet DataFromDb = null;
        //                    DataRowCollection values = LocalDBManager.SelectQuery("VehiclData", "DateTime", "DateTime=(Select max(DateTime) from VehiclData where VehicleID='" + vehicleID + "') and VehicleID='" + vehicleID + "'").Rows;
        //                    if (values.Count > 0)
        //                    {
        //                        foreach (DataRow value in values)
        //                        {
        //                            MySqlCommand cmd = new MySqlCommand("select * from GpsTrackVehicleLogs where DateTime >@DateTime and UserID='Vihari' and VehicleID='" + vehicleID + "'");
        //                            MySqlParameter parm = new MySqlParameter("@DateTime", DateTime.Parse(value[0].ToString()));
        //                            cmd.Parameters.Add(parm);
        //                            DataFromDb = vdm.SelectQuery(cmd);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        MySqlCommand cmd = new MySqlCommand("select * from GpsTrackVehicleLogs where  DateTime >@DateTime and UserID='Vihari' and VehicleID='" + vehicleID + "'");
        //                        MySqlParameter parm = new MySqlParameter("@DateTime", DateTime.Now.AddDays(-1));
        //                        cmd.Parameters.Add(parm);
        //                        DataFromDb = vdm.SelectQuery(cmd);
        //                    }

        //                    if (DataFromDb != null)
        //                    {
        //                        if (VehicleAssighData != null)
        //                        {
        //                            foreach (DataRow dr in DataFromDb.Tables[0].Rows)
        //                            {
        //                                DataRow[] assignTableData = null;
        //                                try
        //                                {
        //                                    assignTableData = VehicleAssighData.Select("VehicleNumber='" + dr[1].ToString() + "' and Sno=" + VehicleAssighData.Select("VehicleNumber='" + dr[1].ToString() + "'", "Sno DESC")[0][0]);//(select Max(Sno) from VehiclAssignData where VehicleNumber= '" + dr[1].ToString() + "')");
        //                                }
        //                                catch (Exception ex)
        //                                {
        //                                }
        //                                if (assignTableData != null)
        //                                {
        //                                    if (assignTableData.Length > 0)
        //                                    {
        //                                        /* If the arrived log is very much older then the present assigned log for same vehicle
        //                                         * then fetch the completed
        //                                         * trip and compare the start time and end time 
        //                                         * if it lies between that time consider the log as that trip log.... 
        //                                         * */
        //                                        //if(DateTime.Parse(assignTableData[0][3].ToString())>
        //                                        MySqlCommand cmd;
        //                                        MySqlParameter parm;


        //                                        cmd = new MySqlCommand("insert into VehiclData values (@VehicleID,@Speed,@DateTime,@Distance,@Diesel,@TripFlag,@Latitude,@Longitude,@TimeInterval,@Status,@Direction,@Remarks,@DriverID,@TripID)");
        //                                        parm = new MySqlParameter("@VehicleID", dr[1].ToString());
        //                                        cmd.Parameters.Add(parm);
        //                                        parm = new MySqlParameter("@Speed", dr[2].ToString());
        //                                        cmd.Parameters.Add(parm);
        //                                        parm = new MySqlParameter("@DateTime", (DateTime)dr[3]);
        //                                        cmd.Parameters.Add(parm);
        //                                        parm = new MySqlParameter("@Distance", dr[4].ToString());
        //                                        cmd.Parameters.Add(parm);
        //                                        parm = new MySqlParameter("@Diesel", dr[5].ToString());
        //                                        cmd.Parameters.Add(parm);
        //                                        parm = new MySqlParameter("@TripFlag", dr[6].ToString());
        //                                        cmd.Parameters.Add(parm);
        //                                        parm = new MySqlParameter("@Latitude", dr[7].ToString());
        //                                        cmd.Parameters.Add(parm);
        //                                        parm = new MySqlParameter("@Longitude", dr[8].ToString());
        //                                        cmd.Parameters.Add(parm);
        //                                        parm = new MySqlParameter("@TimeInterval", dr[9].ToString());
        //                                        cmd.Parameters.Add(parm);
        //                                        parm = new MySqlParameter("@Status", dr[10].ToString());
        //                                        cmd.Parameters.Add(parm);
        //                                        parm = new MySqlParameter("@Direction", dr[11].ToString());
        //                                        cmd.Parameters.Add(parm);
        //                                        parm = new MySqlParameter("@Remarks", dr[12].ToString());
        //                                        cmd.Parameters.Add(parm);
        //                                        parm = new MySqlParameter("@DriverID", assignTableData[0][5].ToString());
        //                                        cmd.Parameters.Add(parm);
        //                                        parm = new MySqlParameter("@TripID", assignTableData[0][0].ToString());
        //                                        cmd.Parameters.Add(parm);
        //                                        LocalDBManager.Insert(cmd);

        //                                        //if vehicle exceeds maximum speed specified by user
        //                                        if (int.Parse(assignTableData[0][6].ToString()) < int.Parse(dr[2].ToString()))
        //                                        {
        //                                            cmd = new MySqlCommand("insert into RemarksTable values (@TripId,@DriverId,@TimeStamp,@Latitude,@Longitude,@Speed,@Diesel,@Remark)");
        //                                            parm = new MySqlParameter("@TripId", assignTableData[0][0].ToString());
        //                                            cmd.Parameters.Add(parm);
        //                                            parm = new MySqlParameter("@DriverId", assignTableData[0][5].ToString());
        //                                            cmd.Parameters.Add(parm);
        //                                            parm = new MySqlParameter("@TimeStamp", (DateTime)dr[3]);
        //                                            cmd.Parameters.Add(parm);
        //                                            parm = new MySqlParameter("@Latitude", dr[7].ToString());
        //                                            cmd.Parameters.Add(parm);
        //                                            parm = new MySqlParameter("@Longitude",dr[8].ToString());
        //                                            cmd.Parameters.Add(parm);
        //                                            parm = new MySqlParameter("@Speed", dr[2].ToString());
        //                                            cmd.Parameters.Add(parm);
        //                                            parm = new MySqlParameter("@Diesel", dr[5].ToString());
        //                                            cmd.Parameters.Add(parm);
        //                                            parm = new MySqlParameter("@Remark", "Over Speed ");
        //                                            cmd.Parameters.Add(parm);
        //                                            //LocalDBManager.Insert(cmd);

        //                                            if (RemarkEvent != null)
        //                                                RemarkEvent(new DataClass() { data = new string[] { "Over speed", dr[1].ToString(), assignTableData[0][1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[7].ToString(), dr[8].ToString() } });
        //                                        }

        //                                        if (LogDetailsList.Keys.Contains(dr[1].ToString()))
        //                                        {
        //                                            LogDetailsList[dr[1].ToString()].prvdatetime = LogDetailsList[dr[1].ToString()].predatetime;
        //                                            LogDetailsList[dr[1].ToString()].prvLat = LogDetailsList[dr[1].ToString()].preLat;
        //                                            LogDetailsList[dr[1].ToString()].prvLogitude = LogDetailsList[dr[1].ToString()].preLogitude;

        //                                            LogDetailsList[dr[1].ToString()].predatetime = (DateTime)dr[3];
        //                                            LogDetailsList[dr[1].ToString()].preLat = (double)dr[7];
        //                                            LogDetailsList[dr[1].ToString()].preLogitude = (double)dr[8];
        //                                            LogDetailsList[dr[1].ToString()].Speed = (double)dr[2];
        //                                            LogDetailsList[dr[1].ToString()].Diesel = (double)dr[5];

        //                                            System.TimeSpan diffResult;

        //                                            if (LogDetailsList[dr[1].ToString()].prvLat == LogDetailsList[dr[1].ToString()].preLat && LogDetailsList[dr[1].ToString()].prvLogitude == LogDetailsList[dr[1].ToString()].preLogitude)
        //                                            {
        //                                                DataRow datarow = null;
        //                                                //DataRow[] Nearbranch = BranchDetails.Select("Latitude-" + LogDetailsList[dr[1].ToString()].preLat + "< 0.000500 and Longitude-" + LogDetailsList[dr[1].ToString()].preLogitude + "<0.000500");
        //                                                foreach(DataRow NearBranch in BranchDetails.Select())
        //                                                {
        //                                                    MapRoute route = GMaps.Instance.GetRouteBetweenPoints(new PointLatLng(double.Parse(NearBranch[3].ToString()), double.Parse(NearBranch[4].ToString())), new PointLatLng(LogDetailsList[dr[1].ToString()].preLat, LogDetailsList[dr[1].ToString()].preLogitude), false, 3);
        //                                                 if (route.Distance <= 0.1)
        //                                                 {
        //                                                     datarow = NearBranch;
        //                                                     LogDetailsList[dr[1].ToString()].IdleInterval=0;
        //                                                     LogDetailsList[dr[1].ToString()].stopedInterval=0;

        //                                                     break;
        //                                                     ////if (RemarkEvent != null)
        //                                                     ////    RemarkEvent(new DataClass() { data = new string[] { "Stopped at Branch" + Nearbranch[i][2].ToString(), dr[1].ToString(), assignTableData[0][1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[7].ToString(), dr[8].ToString() } });
        //                                                 }
        //                                                }
        //                                                if (datarow!=null)
        //                                                {
        //                                                    if (RemarkEvent != null)
        //                                                        RemarkEvent(new DataClass() { data = new string[] { "Stopped at Branch" + datarow[2].ToString(), dr[1].ToString(), assignTableData[0][1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[7].ToString(), dr[8].ToString() } });
        //                                                }
        //                                                else
        //                                                {
        //                                                    diffResult = LogDetailsList[dr[1].ToString()].predatetime.Subtract(LogDetailsList[dr[1].ToString()].prvdatetime);
        //                                                    LogDetailsList[dr[1].ToString()].IdleInterval += Math.Abs(diffResult.TotalSeconds / 60);
        //                                                    if (LogDetailsList[dr[1].ToString()].Speed == 0 && LogDetailsList[dr[1].ToString()].Diesel > 50)
        //                                                    {
        //                                                        if (LogDetailsList[dr[1].ToString()].IdleInterval > int.Parse(assignTableData[0][7].ToString()))
        //                                                        {
        //                                                            cmd = new MySqlCommand("insert into RemarksTable values (@TripId,@DriverId,@TimeStamp,@Latitude,@Longitude,@Speed,@Diesel,@Remark)");
        //                                                            parm = new MySqlParameter("@TripId", assignTableData[0][0].ToString());
        //                                                            cmd.Parameters.Add(parm);
        //                                                            parm = new MySqlParameter("@DriverId", assignTableData[0][5].ToString());
        //                                                            cmd.Parameters.Add(parm);
        //                                                            parm = new MySqlParameter("@TimeStamp", (DateTime)dr[3]);
        //                                                            cmd.Parameters.Add(parm);
        //                                                            parm = new MySqlParameter("@Latitude", dr[7].ToString());
        //                                                            cmd.Parameters.Add(parm);
        //                                                            parm = new MySqlParameter("@Longitude", dr[8].ToString());
        //                                                            cmd.Parameters.Add(parm);
        //                                                            parm = new MySqlParameter("@Speed", dr[2].ToString());
        //                                                            cmd.Parameters.Add(parm);
        //                                                            parm = new MySqlParameter("@Diesel", dr[5].ToString());
        //                                                            cmd.Parameters.Add(parm);
        //                                                            parm = new MySqlParameter("@Remark", "Over Idle" + LogDetailsList[dr[1].ToString()].IdleInterval + " Min");
        //                                                            cmd.Parameters.Add(parm);
        //                                                            //LocalDBManager.Insert(cmd);
        //                                                            if (RemarkEvent != null)
        //                                                                RemarkEvent(new DataClass() { data = new string[] { "Over Idle" + LogDetailsList[dr[1].ToString()].IdleInterval + " Min" + assignTableData[0][7] + " Min", dr[1].ToString(), assignTableData[0][1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[7].ToString(), dr[8].ToString() } });

        //                                                    }
        //                                                }
        //                                                if (LogDetailsList[dr[1].ToString()].Speed == 0 && LogDetailsList[dr[1].ToString()].Diesel <= 50)
        //                                                {
        //                                                    diffResult = LogDetailsList[dr[1].ToString()].predatetime.Subtract(LogDetailsList[dr[1].ToString()].prvdatetime);
        //                                                    LogDetailsList[dr[1].ToString()].stopedInterval += Math.Abs(diffResult.TotalSeconds / 60);

        //                                                    if (LogDetailsList[dr[1].ToString()].stopedInterval > int.Parse(assignTableData[0][9].ToString()))
        //                                                    {
        //                                                        cmd = new MySqlCommand("insert into RemarksTable values (@TripId,@DriverId,@TimeStamp,@Latitude,@Longitude,@Speed,@Diesel,@Remark)");
        //                                                        parm = new MySqlParameter("@TripId", assignTableData[0][0].ToString());
        //                                                        cmd.Parameters.Add(parm);
        //                                                        parm = new MySqlParameter("@DriverId", assignTableData[0][5].ToString());
        //                                                        cmd.Parameters.Add(parm);
        //                                                        parm = new MySqlParameter("@TimeStamp", (DateTime)dr[3]);
        //                                                        cmd.Parameters.Add(parm);
        //                                                        parm = new MySqlParameter("@Latitude", dr[7].ToString());
        //                                                        cmd.Parameters.Add(parm);
        //                                                        parm = new MySqlParameter("@Longitude", dr[8].ToString());
        //                                                        cmd.Parameters.Add(parm);
        //                                                        parm = new MySqlParameter("@Speed",dr[2].ToString());
        //                                                        cmd.Parameters.Add(parm);
        //                                                        parm = new MySqlParameter("@Diesel", dr[5].ToString());
        //                                                        cmd.Parameters.Add(parm);
        //                                                        parm = new MySqlParameter("@Remark", "Over Stopped" + LogDetailsList[dr[1].ToString()].stopedInterval + " Min");
        //                                                        cmd.Parameters.Add(parm);
        //                                                            //LocalDBManager.Insert(cmd);
        //                                                            if (RemarkEvent != null)
        //                                                                RemarkEvent(new DataClass() { data = new string[] { "Over Stopped" + LogDetailsList[dr[1].ToString()].stopedInterval + " Min", dr[1].ToString(), assignTableData[0][1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[7].ToString(), dr[8].ToString() } });
        //                                                        }
        //                                                    }
        //                                                }
        //                                            }
        //                                        }
        //                                        else
        //                                        {
        //                                            LogDetailsList.Add(dr[1].ToString(),
        //                                                                                    new LogDetails
        //                                                                                    {
        //                                                                                        DriverName = assignTableData[0][1].ToString(),
        //                                                                                        VehicleNumber = dr[1].ToString(),
        //                                                                                        predatetime = DateTime.Now,
        //                                                                                        preLat = (double)dr[7],
        //                                                                                        preLogitude = (double)dr[8],
        //                                                                                        prvdatetime = DateTime.Now,
        //                                                                                        prvLat = 0,
        //                                                                                        prvLogitude = 0,
        //                                                                                        Speed = (double)dr[2],
        //                                                                                        Diesel = (double)dr[5]

        //                                                                                    });
        //                                        }
        //                                    }
        //                                    else
        //                                    {
        //                                        MySqlCommand cmd = new MySqlCommand("insert into VehiclData values (@VehicleID,@Speed,@DateTime,@Distance,@Diesel,@TripFlag,@Latitude,@Longitude,@TimeInterval,@Status,@Direction,@Remarks,@DriverID,@TripID)");
        //                                        MySqlParameter parm = new MySqlParameter("@VehicleID", dr[1].ToString());
        //                                        cmd.Parameters.Add(parm);
        //                                        parm = new MySqlParameter("@Speed", dr[2].ToString());
        //                                        cmd.Parameters.Add(parm);
        //                                        parm = new MySqlParameter("@DateTime", (DateTime)dr[3]);
        //                                        cmd.Parameters.Add(parm);
        //                                        parm = new MySqlParameter("@Distance", dr[4].ToString());
        //                                        cmd.Parameters.Add(parm);
        //                                        parm = new MySqlParameter("@Diesel", dr[5].ToString());
        //                                        cmd.Parameters.Add(parm);
        //                                        parm = new MySqlParameter("@TripFlag", dr[6].ToString());
        //                                        cmd.Parameters.Add(parm);
        //                                        parm = new MySqlParameter("@Latitude", dr[7].ToString());
        //                                        cmd.Parameters.Add(parm);
        //                                        parm = new MySqlParameter("@Longitude", dr[8].ToString());
        //                                        cmd.Parameters.Add(parm);
        //                                        parm = new MySqlParameter("@TimeInterval", dr[9].ToString());
        //                                        cmd.Parameters.Add(parm);
        //                                        parm = new MySqlParameter("@Status", dr[10].ToString());
        //                                        cmd.Parameters.Add(parm);
        //                                        parm = new MySqlParameter("@Direction", dr[11].ToString());
        //                                        cmd.Parameters.Add(parm);
        //                                        parm = new MySqlParameter("@Remarks", dr[12].ToString());
        //                                        cmd.Parameters.Add(parm);
        //                                        parm = new MySqlParameter("@DriverID", "0");
        //                                        cmd.Parameters.Add(parm);
        //                                        parm = new MySqlParameter("@TripID", "0");
        //                                        cmd.Parameters.Add(parm);
        //                                        LocalDBManager.Insert(cmd);
        //                                    }
        //                                }
        //                                else
        //                                {
        //                                    MySqlCommand cmd = new MySqlCommand("insert into VehiclData values (@VehicleID,@Speed,@DateTime,@Distance,@Diesel,@TripFlag,@Latitude,@Longitude,@TimeInterval,@Status,@Direction,@Remarks,@DriverID,@TripID)");
        //                                    MySqlParameter parm = new MySqlParameter("@VehicleID", dr[1].ToString());
        //                                    cmd.Parameters.Add(parm);
        //                                    parm = new MySqlParameter("@Speed", dr[2].ToString());
        //                                    cmd.Parameters.Add(parm);
        //                                    parm = new MySqlParameter("@DateTime", (DateTime)dr[3]);
        //                                    cmd.Parameters.Add(parm);
        //                                    parm = new MySqlParameter("@Distance", dr[4].ToString());
        //                                    cmd.Parameters.Add(parm);
        //                                    parm = new MySqlParameter("@Diesel", dr[5].ToString());
        //                                    cmd.Parameters.Add(parm);
        //                                    parm = new MySqlParameter("@TripFlag", dr[6].ToString());
        //                                    cmd.Parameters.Add(parm);
        //                                    parm = new MySqlParameter("@Latitude", dr[7].ToString());
        //                                    cmd.Parameters.Add(parm);
        //                                    parm = new MySqlParameter("@Longitude", dr[8].ToString());
        //                                    cmd.Parameters.Add(parm);
        //                                    parm = new MySqlParameter("@TimeInterval", dr[9].ToString());
        //                                    cmd.Parameters.Add(parm);
        //                                    parm = new MySqlParameter("@Status", dr[10].ToString());
        //                                    cmd.Parameters.Add(parm);
        //                                    parm = new MySqlParameter("@Direction", dr[11].ToString());
        //                                    cmd.Parameters.Add(parm);
        //                                    parm = new MySqlParameter("@Remarks", dr[12].ToString());
        //                                    cmd.Parameters.Add(parm);
        //                                    parm = new MySqlParameter("@DriverID", "0");
        //                                    cmd.Parameters.Add(parm);
        //                                    parm = new MySqlParameter("@TripID", "0");
        //                                    cmd.Parameters.Add(parm);
        //                                    LocalDBManager.Insert(cmd);
        //                                }
        //                                //0-UserID
        //                                //1-VehicleID
        //                                //2=Speed
        //                                //3--DateTime
        //                                //4-Disatance
        //                                //5-Diesel
        //                                //6-TripFlag
        //                                //7-Latitude
        //                                //8-Longitude
        //                                //9-TimeInterval
        //                                //10-Status
        //                                //11-Direction
        //                                //12-Remarks

        //                            }
        //                        }
        //                        //foreach(DataRow dr in LocalDBManager.SelectQuery("

        //                        //foreach (DataRow dr in VehicleDBMngr.Select("TempWeighTransactions", "*"))
        //                        //{
        //                        //    if (VehicleASnTechDbClass.VehicleASnTechDBMngr.checkDBConn())
        //                        //    {
        //                        //        VehicleASnTechDbClass.VehicleASnTechDBMngr.insert("WeighingTransactions", new TransactionDetailsMgr.TransationDetails(dr));
        //                        //        VehicleDBClass.VehicleDBMngr.Delete("TempWeighTransactions", "TransactionNo=" + dr[1].ToString());
        //                        //        //VehicleASnTechDbClass.VehicleASnTechDBMngr.Delete("WeighingTransactions", "TransactionNo=" + dr[1].ToString());
        //                        //    }
        //                        //}
        //                    }
        //                }
        //                //while (StartDownload)
        //                //{
        //                //    try
        //                //    {

        //                //        result = _ping();
        //                //        if (!result)
        //                //            prevresult = false;

        //                //        if ((!(prevresult == result)) && result == true)
        //                //        {
        //                //            prevresult = result;
        //                //            foreach (DataRow dr in LocalDBManager.SelectQuery("TempWeighTransactions", "*").Tables[0].Rows)
        //                //            {
        //                //                //if (vdm.checkDBConn())
        //                //                if (_ping())
        //                //                {
        //                //                    DataTable dt = vdm.SelectQuery("GpsTrackVehicleLogs", "*", new string[] { "UserName=@UserName" }, new string[] { UserName }, new string[] { "" }).Tables[0];
        //                //                    //vdm.SelectQuery("WeighingTransactions", new TransactionDetailsMgr.TransationDetails(dr));
        //                //                    //VehicleDBClass.VehicleDBMngr.Delete("TempWeighTransactions", "TransactionNo=" + dr[1].ToString());
        //                //                }
        //                //            }
        //                //        }
        //                //    }
        //                //    catch (Exception ex)
        //                //    {

        //                //    }
        //                //    Thread.Sleep(500);
        //                //}
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            if (ErrorReports != null)
        //                ErrorReports(ex.ToString());
        //        }
        //    }
        //}
          object lock_ping = new object();
          Ping pingSender = new Ping();
        public  DataTable VehicleAssighData = new DataTable("VehiclAssignData");
        public  DataTable BranchDetails = new DataTable("BranchData");
        public  DataTable DriverDetails = new DataTable("DriverDataTable");
          public bool _ping()
        {
            lock (lock_ping)
            {
                PingOptions options = new PingOptions();

                // Use the default Ttl value which is 128,
                // but change the fragmentation behavior.
                options.DontFragment = true;

                // Create a buffer of 32 bytes of data to be transmitted.
                string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
                byte[] buffer = Encoding.ASCII.GetBytes(data);
                int timeout = 5000;
                bool success = false;
                //for (int i = 0; i < 3; i++)
                //{
                PingReply reply = pingSender.Send("67.227.237.47", timeout, buffer, options);
                if (reply.Status == IPStatus.Success)
                {
                    success = success || true;
                }
                else
                {
                    success = success || false;
                }
                //}
                return success;
            }
        }

         public void UpdateVehicleAssingDataTable(string username)
        {
            lock (VehicleAssighData)
            {
                MySqlCommand cmd = new MySqlCommand("select * from VehicleAssignData where Status='INCOMPLETED' and UserName=@un");
                cmd.Parameters.Add("@un", username);
                vdm.InitializeDB();

            VehicleAssighData=    vdm.SelectQuery(cmd).Tables[0];

                //VehicleAssighData = vdm.SelectQuery("VehicleAssignData", "*", "Status='INCOMPLETED'");
            }
        }
     //    MySqlCommand cmd;
        public void UpdateBranchDetails(string username)
        {
            lock (BranchDetails)
            {
                MySqlCommand cmd = new MySqlCommand("SELECT branchdata.Sno, branchdata.BranchID, branchdata.Description, branchdata.Latitude, branchdata.Longitude, branchdata.PhoneNumber, branchdata.ImagePath, branchdata.ImageType, branchdata.Radious, branchdata.UserName FROM loginstable INNER JOIN branchdata ON loginstable.main_user = branchdata.UserName WHERE (loginstable.loginid = @un)");
                //MySqlCommand cmd = new MySqlCommand("select * from BranchData where UserName=@un");
                cmd.Parameters.Add("@un", username);
                vdm.InitializeDB();
                BranchDetails = vdm.SelectQuery(cmd).Tables[0];
            }
        }
        public void UpdateDriverDetails(string username)
        {
            lock (BranchDetails)
            {
              MySqlCommand  cmd = new MySqlCommand("select * from DriverDataTable where UserName=@un");
                cmd.Parameters.Add("@un", username);
                vdm.InitializeDB();

                DriverDetails = vdm.SelectQuery(cmd).Tables[0];//vdm.SelectQuery("DriverDataTable", "*").Tables[0];
            }
        }

        #region finding Geofence
        public string getGeofenceStatus(double lat1, double lon1, double lat2, double lon2, double radious)
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

        #endregion


        //public   ResourceSet GetImageList()
        //{
        //    //System.Reflection.Assembly asm = System.Reflection.Assembly.GetExecutingAssembly();
        //    //System.Globalization.CultureInfo culture = Thread.CurrentThread.CurrentCulture;
        //    //string resourceName = asm.GetName().Name + ".g";
        //    //System.Resources.ResourceManager rm = new System.Resources.ResourceManager(resourceName, asm);
        //    //System.Resources.ResourceSet resourceSet = rm.GetResourceSet(culture, true, true);

        //    List<string> resources = new List<string>();

        //    ResourceManager rm = new ResourceManager(typeof(Image));
        //    //Demo.WindowsForms.Properties.Resources
        //    ResourceSet resourceSet = Demo.WindowsForms.Properties.Resources.ResourceManager.GetResourceSet(CultureInfo.CurrentUICulture, true, true);
        //    //foreach (DictionaryEntry entry in resourceSet)
        //    //{
        //    //    //string resourceKey = entry.Key;
        //    //    //object resource = entry.Value;
        //    //    resources.Add((string)entry.Key);

        //    //}
        //    //using (ResourceSet rs = rm.GetResourceSet(CultureInfo.CurrentUICulture, true, true))
        //    //{
        //    //    IDictionaryEnumerator resourceEnumerator = rs.GetEnumerator();
        //    //    while (resourceEnumerator.MoveNext())
        //    //    {
        //    //        if (resourceEnumerator.Value is Image)
        //    //        {
        //    //            //Console.WriteLine(resourceEnumerator.Key);
        //    //            resources.Add((string)resourceEnumerator.Key);
        //    //        }
        //    //    }
        //    //}
        //    //foreach (DictionaryEntry resource in resourceSet)
        //    //{
        //    //}
        //    //rm.ReleaseAllResources();
        //    return resourceSet;
        //}

    }

