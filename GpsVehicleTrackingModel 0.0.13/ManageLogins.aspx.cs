using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;

public partial class Manage_Logins : System.Web.UI.Page
{
    MySqlCommand cmd;
    static VehicleDBMgr vdm;
    static string UserName = "";
    static string mainusrsno = "";
    string VehicleTypestring = "";
    string Plantsstring = "";
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
                cmd = new MySqlCommand("select Sno from paireddata where UserID=@UserID");
                cmd.Parameters.Add("@UserID", UserName);
                mainusrsno = vdm.SelectQuery(cmd).Tables[0].Rows[0][0].ToString();
                FillPlant();
                UpdateLogins();

            }
        }
    }

    void FillPlant()
    {
        chblVehicleTypes.Items.Clear();
        string itemtype = "Vehicle Type";
        cmd = new MySqlCommand("select * from vehiclemanage  where UserName=@UserName and ItemType='" + itemtype + "'");
        cmd.Parameters.Add("@UserName", UserName);
        DataTable dtMaintaince = vdm.SelectQuery(cmd).Tables[0];
        foreach (DataRow dr in dtMaintaince.Rows)
        {
            chblVehicleTypes.Items.Add(dr["ItemName"].ToString());
        }
        chblZones.Items.Clear();
        cmd = new MySqlCommand("SELECT UserName, BranchID, Description, Latitude, Longitude, PhoneNumber, ImagePath, Radious,Sno, PlantName, IsPlant FROM branchdata WHERE (UserName = @UserName) AND (IsPlant = '1')");
        cmd.Parameters.Add("@UserName", UserName);
        DataTable totaldata = vdm.SelectQuery(cmd).Tables[0];
        foreach (DataRow dr in totaldata.Rows)
        {
            chblZones.Items.Add(dr["BranchID"].ToString());
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        long refno = 0;
        try
        {
            if (btnSave.Text == "Save")
            {
                cmd = new MySqlCommand("insert into loginstable  (main_user,loginid, pwd,usertype,emailid,phonenumber,main_user_id) values (@main_user,@loginid,@pwd,@usertype,@emailid,@phonenumber,@main_user_id)");
                cmd.Parameters.Add("@main_user", UserName);
                cmd.Parameters.Add("@loginid", txtLoginID.Text);
                cmd.Parameters.Add("@pwd", txtPassword.Text.Trim());
                cmd.Parameters.Add("@usertype", ddlUserType.SelectedValue);
                cmd.Parameters.Add("@emailid", txt_emailid.Text.Trim());
                cmd.Parameters.Add("@phonenumber", txt_mobno.Text.Trim());
                cmd.Parameters.Add("@main_user_id", mainusrsno);
                refno = vdm.insertgetrefno(cmd);
                foreach (ListItem obj in ckbVehicles.Items)
                {
                    if (obj.Selected)
                    {
                        cmd = new MySqlCommand("insert into loginsconfigtable (Refno,VehicleID) values(@Refno,@VehicleID)");
                        cmd.Parameters.Add("@Refno", refno);
                        cmd.Parameters.Add("@VehicleID", obj.Text);
                        vdm.insert(cmd);
                    }
                }
                Refresh();
                UpdateLogins();
                lbl_nofifier.Text = "Record added Successfully";
            }
            else
            {
                cmd = new MySqlCommand("Delete from loginsconfigtable where Refno=@Refno");
                cmd.Parameters.Add("@Refno", Sno);
                vdm.Delete(cmd);
                cmd = new MySqlCommand("Update  loginstable  set  main_user=@main_user,pwd=@pwd,usertype=@usertype,emailid=@emailid,phonenumber=@phonenumber,main_user_id=@main_user_id where loginid=@loginid ");
                cmd.Parameters.Add("@main_user", UserName);
                cmd.Parameters.Add("@loginid", txtLoginID.Text);
                cmd.Parameters.Add("@pwd", txtPassword.Text.Trim());
                cmd.Parameters.Add("@usertype", ddlUserType.SelectedValue);
                cmd.Parameters.Add("@emailid", txt_emailid.Text.Trim());
                cmd.Parameters.Add("@phonenumber", txt_mobno.Text.Trim());
                cmd.Parameters.Add("@main_user_id", mainusrsno);
                vdm.Update(cmd);
                foreach (ListItem obj in ckbVehicles.Items)
                {
                    if (obj.Selected)
                    {
                        cmd = new MySqlCommand("insert into loginsconfigtable (Refno,VehicleID) values(@Refno,@VehicleID)");
                        cmd.Parameters.Add("@Refno", Sno);
                        cmd.Parameters.Add("@VehicleID", obj.Text);
                        vdm.insert(cmd);
                    }
                }
                Refresh();
                UpdateLogins();
                btnSave.Text = "Save";
                lbl_nofifier.Text = "Record Updated Successfully";
            }
        }
        catch(Exception ex)
        {
            if (ex.Message.Contains("PRIMARY"))
            {
                lbl_nofifier.Text = "Oops..! Username alredy existed";
            }
            else
            {
                lbl_nofifier.Text = "Oops..! Error occurred please try again";
            }
        }
    }

    void Refresh()
    {
        txtLoginID.Text = "";
        txtPassword.Text = "";
        txt_emailid.Text = "";
        txt_mobno.Text = "";
        ddlUserType.ClearSelection();
        lbl_nofifier.Text = "";
        btnSave.Text = "Save";
        lblvehcount.Text = "0";
        ckbVehicles.Items.Clear();
        foreach (ListItem chkitem in chblVehicleTypes.Items)
        {
            chkitem.Selected = false;
        }
        foreach (ListItem chkitem in ckbVehicles.Items)
        {
            chkitem.Selected = false;
        }
        foreach (ListItem chkitem in chblZones.Items)
        {
            chkitem.Selected = false;
        }
    }
    void UpdateLogins()
    {
        cmd = new MySqlCommand("select main_user,loginid, pwd,usertype,refno,phonenumber,emailid from loginstable where main_user=@main_user ");
        cmd.Parameters.Add("@main_user", UserName);
        DataTable dt = vdm.SelectQuery(cmd).Tables[0];
        grdManageLogins.DataSource = dt;
        grdManageLogins.DataBind();
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Refresh();
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        if (grdManageLogins.SelectedIndex > -1)
        {
            cmd = new MySqlCommand("Delete from loginstable where Refno=@Refno");
            cmd.Parameters.Add("@Refno", Sno);
            vdm.Delete(cmd);
            cmd = new MySqlCommand("Delete from loginsconfigtable where Refno=@Refno");
            cmd.Parameters.Add("@Refno", Sno);
            vdm.Delete(cmd);
            Refresh();
            UpdateLogins();
            btnSave.Text = "Save";
            lbl_nofifier.Text = "Record Deleted Successfully";
        }
    }
    protected void ckbVehicleTypes_OnCheckedChanged(object sender, EventArgs e)
    {
        ckbVehicles.Items.Clear();
        if (ckbVehicleTypes.Checked == true)
        {
            foreach (ListItem chkitem in chblVehicleTypes.Items)
            {
                chkitem.Selected = true;
            }
        }
        else
        {
            if (chblVehicleTypes.SelectedIndex == 0)
            {
                foreach (ListItem chkitem in chblVehicleTypes.Items)
                {
                    chkitem.Selected = false;
                }
            }
        }
        checkboxchanged();
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
    
    protected void chblvehtypes_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ckbVehicles.Items.Clear();
            foreach (ListItem chkitem in chblVehicleTypes.Items)
            {
                if (chkitem.Selected == false)
                {
                    ckbVehicleTypes.Checked = false;
                    break;
                }
            }
            checkboxchanged();
        }
        catch
        {
        }
    }
  
    void checkboxchanged()
    {
        lblvehcount.Text = "";
        ckballvehicles.Checked = false;
        foreach (ListItem chkitem in chblVehicleTypes.Items)
        {
            if (chkitem.Selected == true)
            {
                VehicleTypestring += "'" + chkitem.Text + "',";
            }
        }
        if (VehicleTypestring.Length > 0)
        {
            VehicleTypestring = VehicleTypestring.Remove(VehicleTypestring.Length - 1);
        }
        else
        {
            VehicleTypestring = "'All Vehicle Types'";
        }
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
        
        DataTable vendors = new DataTable();
        if (Session["filteredtable"] != null)
        {
            vendors = (DataTable)Session["filteredtable"];
        }
        if (vendors.Rows.Count == 0)
        {

            cmd = new MySqlCommand("SELECT * FROM Cabmanagement WHERE UserID=@UserName");
            cmd.Parameters.Add("@UserName", UserName);
            vendors = vdm.SelectQuery(cmd).Tables[0];
            Session["filteredtable"] = vendors;
        }
        DataRow[] filteredrows = null;
        if ((VehicleTypestring != "'All Vehicle Types'") && Plantsstring != "'All Plants'")
            {

                filteredrows = vendors.Select("Vehiclemake in (" + VehicleTypestring + ") and PlantName in (" + Plantsstring + ") ");
              
            }
        else if (VehicleTypestring == "'All Vehicle Types'" && Plantsstring == "'All Plants'")
            {
                filteredrows = vendors.Select();
            }
        else if (VehicleTypestring != "'All Vehicle Types'" && Plantsstring != "'All Plants'")
            {
                filteredrows = vendors.Select("Vehiclemake in (" + VehicleTypestring + ") and PlantName in (" + Plantsstring + ")");
            }
        else if (VehicleTypestring != "'All Vehicle Types'" && Plantsstring == "'All Plants'")
            {
                filteredrows = vendors.Select("VehicleType in (" + VehicleTypestring + ")");
            }
        else if (VehicleTypestring == "'All Vehicle Types'" && Plantsstring != "'All Plants'")
        {
            filteredrows = vendors.Select("PlantName in (" + Plantsstring + ")");
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
            distinctTable = view.ToTable("DistinctTable", true, "VehicleID");
            foreach (DataRow dr in distinctTable.Rows)
            {

                ckbVehicles.Items.Add(dr["VehicleID"].ToString());
            }
        }
        else
        {
            ckbVehicles.Items.Clear();
        }
        lblvehcount.Text = ckbVehicles.Items.Count.ToString();
    }
    protected void chblVehicleTypes_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ckbVehicles.Items.Clear();
            foreach (ListItem chkitem in chblVehicleTypes.Items)
            {
                if (chkitem.Selected == false)
                {
                    ckbVehicleTypes.Checked = false;
                    break;
                }
            }
            checkboxchanged();
        }
        catch
        {
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
    protected void ddlAuthorizedType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
        }
        catch
        {
        }
    }
    public static string Sno = "";
    protected void grdManageLogins_SelectedIndexChanged(object sender, EventArgs e)
    {
        lbl_nofifier.Text = "";
        foreach (ListItem chkitem in chblZones.Items)
        {
            chkitem.Selected = false;
        }
        foreach (ListItem chkitem in chblVehicleTypes.Items)
        {
            chkitem.Selected = false;
        }
        ckballvehicles.Checked = false;
        ckbZones.Checked = false;
        ckbVehicleTypes.Checked = false;
        ckbVehicles.Items.Clear();


        if (grdManageLogins.SelectedIndex >= -1)
        {
            GridViewRow selectedrw = grdManageLogins.SelectedRow;
            txtLoginID.Text = selectedrw.Cells[2].Text;
            txtPassword.Text = selectedrw.Cells[3].Text;
            ddlUserType.SelectedValue = selectedrw.Cells[4].Text;
            Sno = selectedrw.Cells[5].Text;
            txt_mobno.Text = selectedrw.Cells[6].Text;
            txt_emailid.Text = selectedrw.Cells[7].Text;
            btnSave.Text = "Edit";
            cmd = new MySqlCommand("SELECT loginsconfigtable.VehicleID,loginstable.main_user,loginstable.authorizedtype,loginstable.phonenumber,loginstable.emailid,Cabmanagement.VehicleType as vehiclemodel, Cabmanagement.PlantName as MaintenancePlantName FROM loginsconfigtable INNER JOIN loginstable ON loginsconfigtable.Refno = loginstable.refno INNER JOIN Cabmanagement ON loginsconfigtable.VehicleID = Cabmanagement.VehicleID WHERE (loginsconfigtable.Refno = @Refno)");
            cmd.Parameters.Add("@Refno", Sno);
            DataTable dtallVehicles = vdm.SelectQuery(cmd).Tables[0];
            if (dtallVehicles.Rows.Count > 0)
            {
                string authorizedtype = dtallVehicles.Rows[0]["authorizedtype"].ToString();
                ddlUserType.SelectedValue = authorizedtype;
                DataView view = new DataView(dtallVehicles);
                DataTable plants = view.ToTable(true, "MaintenancePlantName");
                DataTable vehicletypes = view.ToTable(true, "vehiclemodel");

                foreach (DataRow dr in plants.Rows)
                {
                    if (dr["MaintenancePlantName"].ToString() != "")
                    {
                        foreach (ListItem chkitem in chblZones.Items)
                        {
                            if (chkitem.Text == dr["MaintenancePlantName"].ToString())
                            {
                                chkitem.Selected = true;
                            }
                        }
                    }
                }


                foreach (DataRow dr in vehicletypes.Rows)
                {
                    if (dr["vehiclemodel"].ToString() != "")
                    {
                        foreach (ListItem chkitem in chblVehicleTypes.Items)
                        {
                            if (chkitem.Text == dr["vehiclemodel"].ToString())
                            {
                                chkitem.Selected = true;
                            }
                        }
                    }
                }
                checkboxchanged();
                foreach (DataRow dr in dtallVehicles.Rows)
                {
                    string VehicleNo = dr["VehicleID"].ToString();
                    foreach (ListItem chkitem in ckbVehicles.Items)
                    {
                        if (chkitem.Text == VehicleNo)
                        {
                            chkitem.Selected = true;
                        }
                    }
                }
            }
        }
    }
}