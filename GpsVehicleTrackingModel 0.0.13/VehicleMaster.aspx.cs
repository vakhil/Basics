using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;
public partial class VehicleMaster : System.Web.UI.Page
{
    MySqlCommand cmd;
    static VehicleDBMgr vdm;
    static string UserName = "";
    public static DataRow[] dtVehicleType = null;
    public static DataRow[] dtRoute = null;
    public static DataRow[] dtMaintaince = null;
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
                Refresh();
                FillVehicleNo();
                FillRoute();
                UpdateVehicleMaster();

                cmd = new MySqlCommand("select * from vehiclemanage where UserName=@UserName");
                cmd.Parameters.Add("@UserName", UserName);
                DataTable totaldata = vdm.SelectQuery(cmd).Tables[0];

                ddlVehicleType.Items.Clear();
                if (ddlVehicleType.SelectedIndex == -1)
                {
                    ddlVehicleType.Items.Add("Select Vehicle Type");
                }
                dtVehicleType = totaldata.Select("ItemType='Vehicle Type'");
                foreach (DataRow dr in dtVehicleType)
                {
                    ddlVehicleType.Items.Add(dr["ItemName"].ToString());
                }
                cmb_vehmake.Items.Clear();
                if (cmb_vehmake.SelectedIndex == -1)
                {
                    cmb_vehmake.Items.Add("Select Vehicle Make");
                }
                dtVehicleType = totaldata.Select("ItemType='Vehicle Make'");
                foreach (DataRow dr in dtVehicleType)
                {
                    cmb_vehmake.Items.Add(dr["ItemName"].ToString());
                }
                cmb_vehmodel.Items.Clear();
                if (cmb_vehmodel.SelectedIndex == -1)
                {
                    cmb_vehmodel.Items.Add("Select Vehicle Model");
                }
                dtVehicleType = totaldata.Select("ItemType='Vehicle Model'");
                foreach (DataRow dr in dtVehicleType)
                {
                    cmb_vehmodel.Items.Add(dr["ItemName"].ToString());
                }
                cmd = new MySqlCommand("SELECT UserName, BranchID, Description, Latitude, Longitude, PhoneNumber, ImagePath, ImageType, Radious, PlantName, IsPlant FROM branchdata WHERE (UserName = @UserName) AND (IsPlant = '1')");
                cmd.Parameters.Add("@UserName", UserName);
                DataTable plants = vdm.SelectQuery(cmd).Tables[0];
                ddlPlantName.Items.Clear();
                if (ddlPlantName.SelectedIndex == -1)
                {
                    ddlPlantName.Items.Add("Select Plant Name");
                }
                foreach (DataRow dr in plants.Rows)
                {
                    ddlPlantName.Items.Add(dr["BranchID"].ToString());
                }
                txt_Fitnessexpdate.Text = GetLowDate(DateTime.Now).ToString("dd-MM-yyyy HH:mm");
                txt_insuranceexpdate.Text = GetHighDate(DateTime.Now).ToString("dd-MM-yyyy HH:mm");
                txt_Pollutionexpdate.Text = GetLowDate(DateTime.Now).ToString("dd-MM-yyyy HH:mm");
                txt_RoadTaxempdate.Text = GetHighDate(DateTime.Now).ToString("dd-MM-yyyy HH:mm");
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

    DataTable dtRoutedata=new DataTable ();
    public void FillRoute()
    {
        try
        {

            cmd = new MySqlCommand("select * from vehiclemanage where UserName=@UserName");
            cmd.Parameters.Add("@UserName", UserName);
            dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
            if (ddlScheduledRoute.SelectedIndex == -1)
            {
                ddlScheduledRoute.Items.Add("Select Route");
            }
            dtRoute = dtRoutedata.Select("ItemType='Scheduled Route'");
            foreach (DataRow dr in dtRoute)
            {
                ddlScheduledRoute.Items.Add(dr["ItemCode"].ToString());
            }
        }
        catch
        {
        }
    }
    protected void ddlScheduledRoute_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlScheduledRoute.SelectedIndex == 0)
        {
            txtRouteName.Text = "";
        }
        foreach (DataRow dr in dtRoute)
        {
            if (ddlScheduledRoute.SelectedValue == dr["ItemCode"].ToString())
            {
                txtRouteName.Text = dr["ItemName"].ToString();
            }
        }
    }
    
    public void FillVehicleNo()
    {
        try
        {

            cmd = new MySqlCommand("select * from PairedData where UserID=@UserName");
            cmd.Parameters.Add("@UserName", UserName);
            if (ddlVehicleNo.SelectedIndex == -1)
            {
                ddlVehicleNo.Items.Add("Select Vehicle No");
            }
            DataTable dt = vdm.SelectQuery(cmd).Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                ddlVehicleNo.Items.Add(dr["VehicleNumber"].ToString());
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, this);
        }
    }
   
    void UpdateVehicleMaster()
    {
        try
        {
          //  cmd = new MySqlCommand("select VehicleID, VehicleType,DriverName,PlantName,RouteName,Routecode,MobileNo,Address,CompanyID,SNo,VehicleMake,VehicleModel,Capacity, RCBookNo, RoadTaxNo,CONVERT(VARCHAR(15), RoadtaxExpdt, 105) AS [DD-MM-YYYY], InsuranceNo,CONVERT(VARCHAR(15), InsuranceNoExpdt, 105) AS [DD-MM-YYYY], PollutionCirtificateNo,  CONVERT(VARCHAR(15), PollutionCRExpdt, 105) AS [DD-MM-YYYY], FitnessNo,  CONVERT(VARCHAR(15), FitnessExpDate, 105) AS [DD-MM-YYYY] from Cabmanagement where UserID=@UserID ");
            cmd = new MySqlCommand("select VehicleID, VehicleType,DriverName,PlantName,RouteName,Routecode,MobileNo,Address,CompanyID,SNo,VehicleMake,VehicleModel,Capacity, RCBookNo, RoadTaxNo, DATE_FORMAT(RoadtaxExpdt,'%d-%m-%Y %h:%i') as RoadtaxExpdt, InsuranceNo, DATE_FORMAT(InsuranceNoExpdt,'%d-%m-%Y %h:%i') as InsuranceNoExpdt, PollutionCirtificateNo, DATE_FORMAT(PollutionCRExpdt,'%d-%m-%Y %h:%i') as PollutionCRExpdt , FitnessNo,   DATE_FORMAT(FitnessExpDate,'%d-%m-%Y %h:%i') as FitnessExpDate from Cabmanagement where UserID=@UserID ");
            cmd.Parameters.Add("@UserID", UserName);
            DataTable dt = vdm.SelectQuery(cmd).Tables[0];
            grdVehicleMaster.DataSource = dt;
            grdVehicleMaster.DataBind();
            Session["xportdata"] = dt;
            Session["title"] = "Vehicle Master";
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, this);
        }
    }
    static string Sno = "";
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {

            DateTime insurancedt = DateTime.Now;//System.Convert.ToDateTime(startdate.Text);//startdate_CalendarExtender.SelectedDate ?? DateTime.Now;// DateTime.Now.AddMonths(-3);//DateTime.Parse(startdate.Text); ;
            DateTime fitnessdt = DateTime.Now;//System.Convert.ToDateTime(startdate.Text);//startdate_CalendarExtender.SelectedDate ?? DateTime.Now;// DateTime.Now.AddMonths(-3);//DateTime.Parse(startdate.Text); ;
            DateTime roadtaxdt = DateTime.Now;//System.Convert.ToDateTime(startdate.Text);//startdate_CalendarExtender.SelectedDate ?? DateTime.Now;// DateTime.Now.AddMonths(-3);//DateTime.Parse(startdate.Text); ;
            DateTime pollutiondt = DateTime.Now;//System.Convert.ToDateTime(startdate.Text);//startdate_CalendarExtender.SelectedDate ?? DateTime.Now;// DateTime.Now.AddMonths(-3);//DateTime.Parse(startdate.Text); ;
            string[] insurancedtstrig = txt_insuranceexpdate.Text.Split(' ');
            string[] fitnessdtdatestrig = txt_Fitnessexpdate.Text.Split(' ');
            string[] roadtaxdtdatestrig = txt_RoadTaxempdate.Text.Split(' ');
            string[] pollutiondtdatestrig = txt_Pollutionexpdate.Text.Split(' ');
            if (insurancedtstrig.Length > 1)
            {
                if (insurancedtstrig[0].Split('-').Length > 0)
                {
                    string[] dates = insurancedtstrig[0].Split('-');
                    string[] times = insurancedtstrig[1].Split(':');
                    insurancedt = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                }
            }
            else if (txt_insurance.Text != "" && txt_insurance.Text != "&nbsp;")
            {
                lblSuccess.Text = "Insurance Date Time Format Wrong";
                return;
            }
            if (fitnessdtdatestrig.Length > 1)
            {
                if (fitnessdtdatestrig[0].Split('-').Length > 0)
                {
                    string[] dates = fitnessdtdatestrig[0].Split('-');
                    string[] times = fitnessdtdatestrig[1].Split(':');
                    fitnessdt = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                }
            }
            else if (txt_Fitness.Text != "" && txt_Fitness.Text != "&nbsp;")
            {
                lblSuccess.Text = "Fitness Date Time Format Wrong";
                return;
            }
            if (roadtaxdtdatestrig.Length > 1)
            {
                if (roadtaxdtdatestrig[0].Split('-').Length > 0)
                {
                    string[] dates = roadtaxdtdatestrig[0].Split('-');
                    string[] times = roadtaxdtdatestrig[1].Split(':');
                    roadtaxdt = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                }
            }
            else if (txt_RoadTax.Text != "" && txt_RoadTax.Text != "&nbsp;")
            {
                lblSuccess.Text = "RoadTax Date Time Format Wrong";
                return;
            }
            if (pollutiondtdatestrig.Length > 1)
            {
                if (pollutiondtdatestrig[0].Split('-').Length > 0)
                {
                    string[] dates = pollutiondtdatestrig[0].Split('-');
                    string[] times = pollutiondtdatestrig[1].Split(':');
                    pollutiondt = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                }
            }
            else if (txt_Pollutionnumber.Text != "" && txt_Pollutionnumber.Text != "&nbsp;")
            {
                lblSuccess.Text = "Pollution Date Time Format Wrong";
                return;
            }
            if (btnAdd.Text == "Add")
            {
                cmd = new MySqlCommand("insert into Cabmanagement  (VehicleID,VehicleMake,VehicleModel,VehicleType,DriverName,PlantName,RouteName,Routecode,MobileNo,Address,CompanyID,UserID,Capacity,RCBookNo,RoadTaxNo, RoadtaxExpdt, InsuranceNo, InsuranceNoExpdt, PollutionCirtificateNo, PollutionCRExpdt,  FitnessNo, FitnessExpDate) values (@VehicleID,@VehicleMake,@VehicleModel, @VehicleType,@DriverName,@PlantName,@RouteName,@Routecode,@MobileNo,@Address,@CompanyID,@UserID,@Capacity,@RCBookNo,@RoadTaxNo, @RoadtaxExpdt, @InsuranceNo, @InsuranceNoExpdt, @PollutionCirtificateNo, @PollutionCRExpdt,  @FitnessNo, @FitnessExpDate)");
                cmd.Parameters.Add("@VehicleID", ddlVehicleNo.SelectedValue);
                cmd.Parameters.Add("@VehicleMake", cmb_vehmake.SelectedValue.Trim());
                cmd.Parameters.Add("@VehicleModel", cmb_vehmodel.SelectedValue.Trim());
                cmd.Parameters.Add("@VehicleType", ddlVehicleType.SelectedValue);
                cmd.Parameters.Add("@DriverName", txtDriverName.Text.Trim());
                cmd.Parameters.Add("@PlantName", ddlPlantName.SelectedValue);
                cmd.Parameters.Add("@MobileNo", txtPhoneNumber.Text.Trim());
                cmd.Parameters.Add("@RouteName", txtRouteName.Text.Trim());
                cmd.Parameters.Add("@Routecode", ddlScheduledRoute.SelectedValue);
                cmd.Parameters.Add("@Address", txtAddress.Text.Trim());
                cmd.Parameters.Add("@CompanyID", txtCompanyID.Text.Trim());
                cmd.Parameters.Add("@UserID", UserName);
                cmd.Parameters.Add("@Capacity", txt_capacity.Text);
                cmd.Parameters.Add("@RCBookNo", txt_rcnumber.Text);

                if (txt_RoadTax.Text != "" && txt_RoadTax.Text != "&nbsp;")
                {
                    cmd.Parameters.Add("@RoadTaxNo", txt_RoadTax.Text);
                    cmd.Parameters.Add("@RoadtaxExpdt", roadtaxdt);
                }
                else
                {
                    cmd.Parameters.Add("@RoadTaxNo", "");
                    cmd.Parameters.Add("@RoadtaxExpdt", null);
                }

                if (txt_insurance.Text != "" && txt_insurance.Text != "&nbsp;")
                {
                    cmd.Parameters.Add("@InsuranceNo", txt_insurance.Text);
                    cmd.Parameters.Add("@InsuranceNoExpdt", insurancedt);
                }
                else
                {
                    cmd.Parameters.Add("@InsuranceNo", "");
                    cmd.Parameters.Add("@InsuranceNoExpdt", null);
                }


                if (txt_Pollutionexpdate.Text != "" && txt_Pollutionexpdate.Text != "&nbsp;")
                {
                    cmd.Parameters.Add("@PollutionCirtificateNo", txt_Pollutionnumber.Text);
                    cmd.Parameters.Add("@PollutionCRExpdt", pollutiondt);
                }
                else
                {
                    cmd.Parameters.Add("@PollutionCirtificateNo", "");
                    cmd.Parameters.Add("@PollutionCRExpdt", null);
                }


                if (txt_Fitness.Text != "" && txt_Fitness.Text != "&nbsp;")
                {
                    cmd.Parameters.Add("@FitnessNo", txt_Fitness.Text);
                    cmd.Parameters.Add("@FitnessExpDate", fitnessdt);
                }
                else
                {
                    cmd.Parameters.Add("@FitnessNo", "");
                    cmd.Parameters.Add("@FitnessExpDate", null);
                }
                vdm.insert(cmd);
                Refresh();
                UpdateVehicleMaster();
                lblSuccess.Text = "Record added Successfully";
            }
            else
            {
                cmd = new MySqlCommand("update Cabmanagement set  VehicleID=@VehicleID,VehicleType=@VehicleType,VehicleMake=@VehicleMake,VehicleModel=@VehicleModel,DriverName=@DriverName,PlantName=@PlantName,RouteName=@RouteName,Routecode=@Routecode,MobileNo=@MobileNo,Address=@Address,CompanyID=@CompanyID,RoadTaxNo=@RoadTaxNo, RoadtaxExpdt=@RoadtaxExpdt, InsuranceNo=@InsuranceNo, InsuranceNoExpdt=@InsuranceNoExpdt, PollutionCirtificateNo=@PollutionCirtificateNo, PollutionCRExpdt=@PollutionCRExpdt, RCBookNo=@RCBookNo, FitnessNo=@FitnessNo, FitnessExpDate=@FitnessExpDate,Capacity=@Capacity where UserID=@UserID and SNo=@SNo");
                cmd.Parameters.Add("@VehicleID", ddlVehicleNo.SelectedValue);
                cmd.Parameters.Add("@VehicleType", ddlVehicleType.SelectedValue);
                cmd.Parameters.Add("@VehicleMake", cmb_vehmake.SelectedValue.Trim());
                cmd.Parameters.Add("@VehicleModel", cmb_vehmodel.SelectedValue.Trim());
                cmd.Parameters.Add("@DriverName", txtDriverName.Text.Trim());
                cmd.Parameters.Add("@PlantName", ddlPlantName.SelectedValue);
                cmd.Parameters.Add("@MobileNo", txtPhoneNumber.Text.Trim());
                cmd.Parameters.Add("@RouteName", txtRouteName.Text.Trim());
                cmd.Parameters.Add("@Routecode", ddlScheduledRoute.SelectedValue);
                cmd.Parameters.Add("@Address", txtAddress.Text.Trim());
                cmd.Parameters.Add("@CompanyID", txtCompanyID.Text.Trim());
                cmd.Parameters.Add("@UserID", UserName);
                cmd.Parameters.Add("@SNo", Sno);
                cmd.Parameters.Add("@Capacity", txt_capacity.Text);
                cmd.Parameters.Add("@RCBookNo", txt_rcnumber.Text);
                if (txt_RoadTax.Text != "" && txt_RoadTax.Text != "&nbsp;")
                {
                    cmd.Parameters.Add("@RoadTaxNo", txt_RoadTax.Text);
                    cmd.Parameters.Add("@RoadtaxExpdt", roadtaxdt);
                }
                else
                {
                    cmd.Parameters.Add("@RoadTaxNo", "");
                    cmd.Parameters.Add("@RoadtaxExpdt", null);
                }

                if (txt_insurance.Text != "" && txt_insurance.Text != "&nbsp;")
                {
                    cmd.Parameters.Add("@InsuranceNo", txt_insurance.Text);
                    cmd.Parameters.Add("@InsuranceNoExpdt", insurancedt);
                }
                else
                {
                    cmd.Parameters.Add("@InsuranceNo", "");
                    cmd.Parameters.Add("@InsuranceNoExpdt", null);
                }


                if (txt_Pollutionexpdate.Text != "" && txt_Pollutionexpdate.Text != "&nbsp;")
                {
                    cmd.Parameters.Add("@PollutionCirtificateNo", txt_Pollutionnumber.Text);
                    cmd.Parameters.Add("@PollutionCRExpdt", pollutiondt);
                }
                else
                {
                    cmd.Parameters.Add("@PollutionCirtificateNo", "");
                    cmd.Parameters.Add("@PollutionCRExpdt", null);
                }


                if (txt_Fitness.Text != "" && txt_Fitness.Text != "&nbsp;")
                {
                    cmd.Parameters.Add("@FitnessNo", txt_Fitness.Text);
                    cmd.Parameters.Add("@FitnessExpDate", fitnessdt);
                }
                else
                {
                    cmd.Parameters.Add("@FitnessNo", "");
                    cmd.Parameters.Add("@FitnessExpDate", null);
                }
                vdm.Update(cmd);
                Refresh();
                UpdateVehicleMaster();
                btnAdd.Text = "Add";
                lblSuccess.Text = "Record Updated Successfully";
            }
        }
        catch(Exception ex)
        {
            lblSuccess.Text = ex.Message;
        }
    }
    void Refresh()
    {
        lblSuccess.Text = "";
        ddlVehicleNo.ClearSelection();
        ddlVehicleType.ClearSelection();
        txtDriverName.Text = "";
        cmb_vehmake.SelectedIndex = 0;
        cmb_vehmodel.SelectedIndex = 0;
        ddlScheduledRoute.ClearSelection();
        txtPhoneNumber.Text = "";
        ddlPlantName.ClearSelection(); ;
        txtRouteName.Text = "";
        txtAddress.Text = "";

        txt_rcnumber.Text = "";
        txt_RoadTax.Text = "";
        txt_Pollutionnumber.Text = "";
        txt_insurance.Text = "";
        txt_Fitness.Text = "";
        txt_Fitnessexpdate.Text = GetLowDate(DateTime.Now).ToString("dd-MM-yyyy HH:mm");
        txt_insuranceexpdate.Text = GetHighDate(DateTime.Now).ToString("dd-MM-yyyy HH:mm");
        txt_Pollutionexpdate.Text = GetLowDate(DateTime.Now).ToString("dd-MM-yyyy HH:mm");
        txt_RoadTaxempdate.Text = GetHighDate(DateTime.Now).ToString("dd-MM-yyyy HH:mm");

        btnAdd.Text = "Add";
    }
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        Refresh();
    }
    protected void grdVehicleMaster_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Refresh();
            //DateTime FormatedDOP = Convert.ToDateTime(selectedrw.Cells[3].Text);
            //string sDOP = FormatedDOP.ToString("dd/M/yyyy HH:mm");
            //txtDOP.Text = sDOP;
            if (grdVehicleMaster.SelectedIndex >= -1)
            {
                GridViewRow selectedrw = grdVehicleMaster.SelectedRow;
                ListItem item = ddlVehicleNo.Items.FindByText(selectedrw.Cells[1].Text);
                if (item != null)
                {
                    ddlVehicleNo.SelectedValue = selectedrw.Cells[1].Text;
                }
                item = ddlVehicleType.Items.FindByText(selectedrw.Cells[2].Text);
                if (item != null)
                {
                    ddlVehicleType.SelectedValue = selectedrw.Cells[2].Text;
                }

                txtDriverName.Text = selectedrw.Cells[3].Text;
                item = ddlPlantName.Items.FindByText(selectedrw.Cells[4].Text);
                if (item != null)
                {
                    ddlPlantName.SelectedValue = selectedrw.Cells[4].Text;
                }
                txtRouteName.Text = selectedrw.Cells[5].Text;
                item = ddlScheduledRoute.Items.FindByText(selectedrw.Cells[6].Text);
                if (item != null)
                {
                    ddlScheduledRoute.SelectedValue = selectedrw.Cells[6].Text;
                }
                //ddlScheduledRoute.SelectedValue = selectedrw.Cells[6].Text;
                //ddlZone.SelectedValue= selectedrw.Cells[4].Text;
                txtPhoneNumber.Text = selectedrw.Cells[7].Text;
                txtAddress.Text = selectedrw.Cells[8].Text;
                txtCompanyID.Text = selectedrw.Cells[9].Text;
                Sno = selectedrw.Cells[10].Text;
                item = cmb_vehmake.Items.FindByText(selectedrw.Cells[11].Text);
                if (item != null)
                {
                    cmb_vehmake.SelectedValue = selectedrw.Cells[11].Text;
                }
                item = cmb_vehmodel.Items.FindByText(selectedrw.Cells[11].Text);
                if (item != null)
                {
                    cmb_vehmodel.SelectedValue = selectedrw.Cells[12].Text;
                }
                txt_capacity.Text = selectedrw.Cells[13].Text;
                txt_rcnumber.Text = selectedrw.Cells[14].Text;
                txt_RoadTax.Text = selectedrw.Cells[15].Text;
                txt_RoadTaxempdate.Text = selectedrw.Cells[16].Text;
                txt_insurance.Text = selectedrw.Cells[17].Text;
                txt_insuranceexpdate.Text = selectedrw.Cells[18].Text;
                txt_Pollutionnumber.Text = selectedrw.Cells[19].Text;
                txt_Pollutionexpdate.Text = selectedrw.Cells[20].Text;
                txt_Fitness.Text = selectedrw.Cells[21].Text;
                txt_Fitnessexpdate.Text = selectedrw.Cells[22].Text;

                btnAdd.Text = "Edit";
                btnDelete.Enabled = true;
                lblSuccess.Text = "";
            }
        }
        catch
        {
        }
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            if (grdVehicleMaster.SelectedIndex > -1)
            {
                cmd = new MySqlCommand("delete from Cabmanagement where  UserID=@UserID and SNo=@SNo");
                cmd.Parameters.Add("@UserID", UserName);
                cmd.Parameters.Add("@SNo", Sno);
                vdm.Delete(cmd);
                Refresh();
                UpdateVehicleMaster();
                btnAdd.Text = "Add";
                lblSuccess.Text = "Record Deleted Successfully";
            }
        }
        catch
        {
        }
    }
}