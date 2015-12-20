using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;

public partial class Alerts : System.Web.UI.Page
{
    MySqlCommand cmd;
    string UserName = "";
    VehicleDBMgr vdm;
    DataDownloader ddwnldr;
    string Plantsstring = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["field1"] == null)
            Response.Redirect("Login.aspx");
        else
        {
            UserName = Session["field1"].ToString();
            Session["field2"] = "MyLocationtrue";
            vdm = new VehicleDBMgr();
            vdm.InitializeDB();
            if (!Page.IsPostBack)
            {
                if (!Page.IsCallback)
                {
                    ddwnldr = new DataDownloader();
                    vdm.InitializeDB();
                    fillPlant();
                    fillperson_name();
                    UpdateLogins();
                }
            }
        }
    }
    public void getRoutesInfo()
    {
        cmd = new MySqlCommand("SELECT * FROM Cabmanagement WHERE UserID=@UserName");
        cmd.Parameters.Add("@UserName", UserName);
        DataTable PlantInfo = vdm.SelectQuery(cmd).Tables[0];
        Session["filteredtable"] = PlantInfo;
    }
    public void fillPlant()
    {
        //ddlAlertPlantName.Items.Clear();
        //string Itemtype = "Plant Name";
        //cmd = new MySqlCommand("select SNo,ItemName from vehiclemanage where UserName=@UserName and Itemtype='" + Itemtype + "'");
        //cmd.Parameters.Add("@UserName", UserName);
        //DataTable dtPlant = vdm.SelectQuery(cmd).Tables[0];
        //ddlAlertPlantName.DataSource = dtPlant;
        //ddlAlertPlantName.DataTextField = "ItemName";
        //ddlAlertPlantName.DataValueField = "SNo";
        //ddlAlertPlantName.DataBind();

        
        chblZones.Items.Clear();
        cmd = new MySqlCommand("SELECT SNo,UserName, BranchID, Description, Latitude, Longitude, PhoneNumber, ImagePath, ImageType, Radious, PlantName, IsPlant FROM branchdata WHERE (UserName = @UserName) AND (IsPlant = '1')");
        cmd.Parameters.Add("@UserName", UserName);
        DataTable plants = vdm.SelectQuery(cmd).Tables[0];
        foreach (DataRow dr in plants.Rows)
        {
            chblZones.Items.Add(dr["BranchID"].ToString());
        }
    }
    public void fillperson_name()
    {
        //ddlSelectEmail.Items.Clear();
        //string Itemtype = "Plant Name";
        cmd = new MySqlCommand("select sno,EmailAddress,PName from emailmgmt where UserID=@UserName");
        cmd.Parameters.Add("@UserName", UserName);
        DataTable dtPlant = vdm.SelectQuery(cmd).Tables[0];
        Session["filiemails"] = dtPlant;
        ddlSelectEmail.DataSource = dtPlant;
        ddlSelectEmail.DataTextField = "EmailAddress";
        ddlSelectEmail.DataValueField = "sno";
        ddlSelectEmail.DataBind();
        //ddlSelectEmail.Items.Add("Select Email");
        ddlSelectEmail.Items.Insert(0, new ListItem("Select Email", "-1"));

    }
    protected void emailSelected(object sender, EventArgs e)
    {
        string selectedemail = ddlSelectEmail.SelectedItem.Text;
        DataTable emails = new DataTable();
        if (Session["filiemails"] != null)
        {
            emails = (DataTable)Session["filiemails"];
        }
        if (emails.Rows.Count == 0)
        {
            cmd = new MySqlCommand("select sno,EmailAddress,PName from emailmgmt where UserID=@UserName");
            cmd.Parameters.Add("@UserName", UserName);
            emails = vdm.SelectQuery(cmd).Tables[0];
            Session["filiemails"] = emails;
        }
        DataRow[] filtredemailrows = null;
        if (selectedemail!= "")
        {
            filtredemailrows = emails.Select("EmailAddress ='" + selectedemail + "'");
        }
        DataTable filteredtable = new DataTable();
        if (filtredemailrows.Length > 0)
        {
            filteredtable = filtredemailrows.CopyToDataTable();
        }
        DataView view = filteredtable.DefaultView;
        DataTable distinctTable = new DataTable();
        if (view.Count > 0)
        {
            distinctTable = view.ToTable("DistinctTable", true, "PName");
            foreach (DataRow dr in distinctTable.Rows)
            {

                lblvehcount.Text = dr["PName"].ToString(); 
            }
        }
        else
        {
            lblvehcount.Text="__________";
        }
        
       
    }
    protected void ckbZones_OnCheckedChanged(object sender, EventArgs e)
    {
        ckbVehicles.Items.Clear();
        if (ckbZones.Checked == true)
        {
            foreach (ListItem chkitem in chblZones.Items)
            {
                chkitem.Selected = true;
            }
        }
        else
        {
            if (chblZones.SelectedIndex == 0)
            {
                foreach (ListItem chkitem in chblZones.Items)
                {
                    chkitem.Selected = false;
                }
            }
        }
        checkboxchanged();
    }
    void checkboxchanged()
    {
        

        //ckbVehicles.Items.Clear();
        //chblZones.Items.Clear();
        ckballvehicles.Checked = false;
        //foreach (ListItem chkitem in chblVehicleTypes.Items)
        //{
        //    if (chkitem.Selected == true)
        //    {
        //        VehicleTypestring += "'" + chkitem.Text + "',";
        //    }
        //}
        //if (VehicleTypestring.Length > 0)
        //{
        //    VehicleTypestring = VehicleTypestring.Remove(VehicleTypestring.Length - 1);
        //}
        //else
        //{
        //    VehicleTypestring = "'All Vehicle Types'";
        //}
        foreach (ListItem chkitem in chblZones.Items)
        {
            if (chkitem.Selected == true)
            {
                Plantsstring += "'" + chkitem.Text + "',";
            }
        }
        if (Plantsstring.Length > 0)
        {
            Plantsstring = Plantsstring.Remove(Plantsstring.Length - 1);
        }
        else
        {
            Plantsstring = "'All Plants'";
        }

        DataTable PlantInfo = new DataTable();
        if (Session["filteredtable"] != null)
        {
            PlantInfo = (DataTable)Session["filteredtable"];
        }
        if (PlantInfo.Rows.Count == 0)
        {
            //cmd = new MySqlCommand("SELECT vehiclemanage.ItemName, routetable.RouteName, routetable.UserID, vehiclemanage.ItemType FROM tripconfiguration INNER JOIN vehiclemanage ON tripconfiguration.PlantName = vehiclemanage.SNo INNER JOIN routetable ON tripconfiguration.RouteID = routetable.SNo WHERE (vehiclemanage.ItemType = 'Plant Name') and routetable.UserID=@UserName");
            cmd = new MySqlCommand("SELECT routetable.RouteName, routetable.UserID, branchdata.BranchID FROM tripconfiguration INNER JOIN routetable ON tripconfiguration.RouteID = routetable.SNo INNER JOIN branchdata ON tripconfiguration.PlantName = branchdata.Sno WHERE (routetable.UserID = @UserName) GROUP BY routetable.RouteName");
            cmd.Parameters.Add("@UserName", UserName);
            PlantInfo = vdm.SelectQuery(cmd).Tables[0];
            Session["filteredtable"] = PlantInfo;
        }
        DataRow[] filteredrows = null;
        if (Plantsstring != "'All Plants'")
        {
            filteredrows = PlantInfo.Select("BranchID in (" + Plantsstring + ")");
        }
        else if (Plantsstring == "'All Plants'")
        {
            filteredrows = PlantInfo.Select("BranchID in (" + Plantsstring + ")");
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
            distinctTable = view.ToTable("DistinctTable", true, "RouteName");
            foreach (DataRow dr in distinctTable.Rows)
            {

                ckbVehicles.Items.Add(dr["RouteName"].ToString());
            }
        }
        else
        {
            ckbVehicles.Items.Clear();
        }
        //lblvehcount.Text = ckbVehicles.Items.Count.ToString();
    }
    protected void chblZones_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ckbVehicles.Items.Clear();
            foreach (ListItem chkitem in chblZones.Items)
            {
                if (chkitem.Selected == false)
                {
                    ckbZones.Checked = false;
                    break;
                }
            }
            checkboxchanged();
        }
        catch
        {
        }
    }
    protected void ckballvehicles_SelectedIndexChanged(object sender, EventArgs e)
    {

        if (ckballvehicles.Checked == true)
        {
            foreach (ListItem chkitem in ckbVehicles.Items)
            {
                chkitem.Selected = true;
            }
        }
        else
        {
            if (ckbVehicles.SelectedIndex == 0)
            {
                foreach (ListItem chkitem in ckbVehicles.Items)
                {
                    chkitem.Selected = false;
                }
            }
        }
    }
    public static string Sno = "";
    protected void btnalertSave_Click(object sender, EventArgs e)
    {
        long refno = 0;
        try
        {

            if (btnalertSave.Text == "Save")
            {
                cmd = new MySqlCommand("insert into alerts  (AlertType,EmailID,username) values (@alerttype,@emailid,@username)");
                cmd.Parameters.Add("@username", UserName);
                cmd.Parameters.Add("@alerttype", "Email");
                cmd.Parameters.Add("@emailid", ddlSelectEmail.SelectedItem.Value);
                //cmd.Parameters.Add("@usertype", ddlUserType.SelectedValue);
                refno = vdm.insertgetrefno(cmd);
                foreach (ListItem obj in ckbVehicles.Items)
                {
                    if (obj.Selected)
                    {
                        cmd = new MySqlCommand("insert into alert_subinfo (alert_sno,routecode) values(@Refno,@routeID)");
                        cmd.Parameters.Add("@Refno", refno);
                        cmd.Parameters.Add("@routeID", obj.Text);
                        vdm.insert(cmd);
                    }
                }
                Refresh();
                //foreach (ListItem li in chblZones.Items)
                //{
                //    if (li.Selected)
                //    {
                //        plants += li.Text + ",";
                //    }
                //}
                UpdateLogins();
                lblmsg.Text = "Record added Successfully";
            }
            else
            {
                cmd = new MySqlCommand("Delete from alert_subinfo where alert_sno=@Refno");
                cmd.Parameters.Add("@Refno", Session["reffno"]);
                vdm.Delete(cmd);
                cmd = new MySqlCommand("Update alerts set  EmailID=@emailid where sno=@Refno ");
                cmd.Parameters.Add("@Refno", Session["reffno"]);
                cmd.Parameters.Add("@emailid", ddlSelectEmail.SelectedItem.Value);
                //cmd.Parameters.Add("@loginid", txtLoginID.Text);
                //cmd.Parameters.Add("@pwd", txtPassword.Text.Trim());
                //cmd.Parameters.Add("@usertype", ddlUserType.SelectedValue);
                vdm.Update(cmd);
                foreach (ListItem obj in ckbVehicles.Items)
                {
                    if (obj.Selected)
                    {
                        cmd = new MySqlCommand("insert into alert_subinfo (alert_sno,routecode) values(@Refno,@routeID)");
                        cmd.Parameters.Add("@Refno", Session["reffno"]);
                        cmd.Parameters.Add("@routeID", obj.Text);
                        vdm.insert(cmd);
                    }
                }
                Refresh();
                UpdateLogins();
                btnalertSave.Text = "Save";
                lblmsg.Text = "Record Updated Successfully";
            }
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("PRIMARY"))
            {
                //lbl_nofifier.Text = "Oops..! Username alredy existed";
            }
            else
            {
                //lbl_nofifier.Text = "Oops..! Error occurred please try again";
            }
        }

    }
    protected void btnalertDelete_Click(object sender, EventArgs e)
    {
        cmd = new MySqlCommand("Delete from alert_subinfo where alert_sno=@Refno");
        cmd.Parameters.Add("@Refno", Session["reffno"]);
        vdm.Delete(cmd);
        cmd = new MySqlCommand("Delete from alerts  where sno=@Refno ");
        cmd.Parameters.Add("@Refno", Session["reffno"]);
       // cmd.Parameters.Add("@emailid", ddlSelectEmail.SelectedItem.Value);
        //cmd.Parameters.Add("@loginid", txtLoginID.Text);
        //cmd.Parameters.Add("@pwd", txtPassword.Text.Trim());
        //cmd.Parameters.Add("@usertype", ddlUserType.SelectedValue);
        vdm.Delete(cmd);
        Refresh();
        UpdateLogins();
        btnalertSave.Text = "Save";
        lblmsg.Text = "Record Deleted Successfully";
    }
    protected void btnalertclear_Click(object sender, EventArgs e)
    {
        Refresh();
    }
    void Refresh()
    {
        //txtLoginID.Text = "";
        //txtPassword.Text = "";chblZones
        foreach (ListItem chkitem in chblZones.Items)
        {
            chkitem.Selected = false;
        }
        ckbVehicles.Items.Clear();
        ddlSelectEmail.ClearSelection();
        lblmsg.Text = "";
        lblvehcount.Text = "___________";
        btnalertSave.Text = "Save";
    }
    void UpdateLogins()
    {
        cmd = new MySqlCommand("SELECT routetable.RouteName, alerts.AlertType, alerts.sno, emailmgmt.EmailAddress, branchdata.BranchID FROM alert_subinfo INNER JOIN alerts ON alert_subinfo.alert_sno = alerts.sno INNER JOIN routetable ON alert_subinfo.routecode = routetable.RouteName INNER JOIN emailmgmt ON alerts.EmailID = emailmgmt.sno INNER JOIN tripconfiguration ON routetable.SNo = tripconfiguration.RouteID INNER JOIN branchdata ON tripconfiguration.PlantName = branchdata.Sno WHERE (alerts.username = @username) GROUP BY alert_subinfo.routecode, emailmgmt.EmailAddress, branchdata.BranchID");
        //cmd = new MySqlCommand(" SELECT vehiclemanage.ItemName, routetable.RouteName, alerts.AlertType, alerts.sno, emailmgmt.EmailAddress FROM alert_subinfo INNER JOIN alerts ON alert_subinfo.alert_sno = alerts.sno INNER JOIN routetable ON alert_subinfo.routecode = routetable.RouteName INNER JOIN emailmgmt ON alerts.EmailID = emailmgmt.sno INNER JOIN tripconfiguration ON routetable.SNo = tripconfiguration.RouteID INNER JOIN vehiclemanage ON tripconfiguration.PlantName = vehiclemanage.SNo WHERE (alerts.username = @username) GROUP BY alert_subinfo.routecode, emailmgmt.EmailAddress, vehiclemanage.ItemName");
        cmd.Parameters.Add("@username", UserName);
        DataTable dt = vdm.SelectQuery(cmd).Tables[0];
        DataTable alertshower = new DataTable();
        alertshower.Columns.Add("sno");
        alertshower.Columns.Add("EmailAddress");
        alertshower.Columns.Add("Plants");
        alertshower.Columns.Add("Routes");
        //alertshower.Columns.Add("AlertType");
        alertshower.Columns.Add("RefNo");
        DataTable PlantInfo = new DataTable();
        if (Session["filteredtable"] != null)
        {
            PlantInfo = (DataTable)Session["filteredtable"];
        }
        int i=1;
        DataTable alertdefault = dt.DefaultView.ToTable(true, "sno", "EmailAddress");
        foreach (DataRow dr in alertdefault.Rows)
        {
            string routesConn="";
            string plants = "";
            DataRow[] AlertRow=  dt.Select("sno=" + dr["sno"].ToString());
            if (AlertRow.Length > 0)
            {
                DataTable distinctplants = AlertRow.CopyToDataTable<DataRow>().DefaultView.ToTable(true, "BranchID");
                foreach (DataRow routes in dt.Select("sno=" + dr["sno"].ToString()))
                {
                    routesConn += routes["RouteName"].ToString() + ",";
                }
                if (routesConn.Length > 0)
                {
                    routesConn = routesConn.Remove(routesConn.Length - 1);
                }
                foreach (DataRow plant in distinctplants.Rows)
                {
                    plants += plant["BranchID"].ToString() + ",";
                }
                if (plants.Length > 0)
                {
                    plants = plants.Remove(plants.Length - 1);
                }
            }
            //foreach (DataRow plantsr in dt.Select("EmailAddress='" + dr["EmailAddress"].ToString() + "'"))
            //{
               

            //}
            DataRow nrow = alertshower.NewRow();
            nrow["Sno"] = i++;
            nrow["RefNo"] = dr["sno"].ToString();
            nrow["EmailAddress"] = dr["EmailAddress"].ToString();
            nrow["Routes"] = routesConn;
            nrow["Plants"] = plants;


            alertshower.Rows.Add(nrow);

        }

        grdAlerts.DataSource = alertshower;
        grdAlerts.DataBind();
        //grdAlerts.Columns[5].Visible = false;
    }
    protected void grdAlerts_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[5].Visible = false;
    }
    string reffno = "";
    protected void grdAlerts_SelectedIndexChanged(object sender, EventArgs e)
    {
        //lbl_nofifier.Text = "";
        //foreach (ListItem chkitem in chblplants.Items)
        //{
        //    chkitem.Selected = false;
        //}
        //foreach (ListItem chkitem in chblvehtypes.Items)
        //{
        //    chkitem.Selected = false;
        //}
        //foreach (ListItem chkitem in chblcapacity.Items)
        //{
        //    chkitem.Selected = false;
        //}
        //ckballplants.Checked = false;
        //ckballcapacity.Checked = false;

        //ckballvehtypes.Checked = false;
        //ckbVehicles.Items.Clear();


        if (grdAlerts.SelectedIndex >= -1)
        {
            GridViewRow selectedrw = grdAlerts.SelectedRow;
            //txtLoginID.Text = selectedrw.Cells[2].Text;
            Session["reffno"] = selectedrw.Cells[5].Text;
            divplantsfill();
            ddlSelectEmail.ClearSelection();
            ddlSelectEmail.Items.FindByText(selectedrw.Cells[2].Text).Selected = true;
          // ddlSelectEmail.Ite
            // ddlSelectEmail.SelectedItem.Text = selectedrw.Cells[2].Text;
            emailSelected(null, null);
            //Sno = selectedrw.Cells[5].Text;
            btnalertSave.Text = "Edit";
        }
    }
    void divplantsfill()
    {
        foreach (ListItem chkitem in chblZones.Items)
        {
            chkitem.Selected = false;
        }
        ckbVehicles.Items.Clear();
        //chblZones.Items.Clear();
        string plantchck = "";
        //cmd = new MySqlCommand("SELECT alerts.EmailID, emailmgmt.EmailAddress, alerts.sno, alerts.AlertType, routetable.RouteName, tripconfiguration.PlantName, vehiclemanage.ItemName FROM emailmgmt INNER JOIN alerts ON emailmgmt.sno = alerts.EmailID INNER JOIN alert_subinfo ON alerts.sno = alert_subinfo.alert_sno INNER JOIN routetable ON alert_subinfo.routecode = routetable.RouteName INNER JOIN tripconfiguration ON routetable.SNo = tripconfiguration.RouteID INNER JOIN vehiclemanage ON tripconfiguration.PlantName = vehiclemanage.SNo WHERE (alerts.sno = @refno)");
        cmd = new MySqlCommand("SELECT alerts.EmailID, emailmgmt.EmailAddress, alerts.sno, alerts.AlertType, routetable.RouteName, branchdata.BranchID FROM emailmgmt INNER JOIN alerts ON emailmgmt.sno = alerts.EmailID INNER JOIN alert_subinfo ON alerts.sno = alert_subinfo.alert_sno INNER JOIN routetable ON alert_subinfo.routecode = routetable.RouteName INNER JOIN tripconfiguration ON routetable.SNo = tripconfiguration.RouteID INNER JOIN branchdata ON tripconfiguration.PlantName = branchdata.Sno WHERE (alerts.sno = @refno)");
        cmd.Parameters.Add("@refno", Session["reffno"]);
        DataTable plantcheck = vdm.SelectQuery(cmd).Tables[0];
        foreach (DataRow plantcheckrow in plantcheck.Rows)
        {
            foreach (ListItem chkitem in chblZones.Items)
            {
                if (plantcheckrow["BranchID"].ToString() == chkitem.Value)
                {
                    chkitem.Selected = true;

                }

            }
        }
        checkboxchanged();

        foreach (DataRow plantcheckrow in plantcheck.Rows)
        {
            foreach (ListItem chkitems in ckbVehicles.Items)
            {
                if (plantcheckrow["RouteName"].ToString() == chkitems.Value)
                {
                    chkitems.Selected = true;

                }
            }

        }
    }
}