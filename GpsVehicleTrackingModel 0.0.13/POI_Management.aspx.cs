using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;
using System.IO;
using System.Text;

public partial class POI_Management : System.Web.UI.Page
{
    MySqlCommand cmd;
    static string SlNo = "";
    string UserName = "";
    string User_sno = "";
    //GooglePoint GP1;
    double Lvalue1 = 17.497535;
    double Lonvalue2 = 78.408622;
    static decimal MyLocationNameSno = 0;
    DataTable dtAddress = new DataTable();
    VehicleDBMgr vdm;
    DataDownloader ddwnldr;
    protected void Page_Load(object sender, EventArgs e)
    {
        /////////////////.................MyLocation....................////////////////
        if (Session["field1"] == null)
            Response.Redirect("Login.aspx");
        else
        {

            UserName = Session["field1"].ToString();
            User_sno = Session["field3"].ToString();
            Session["field2"] = "MyLocationtrue";
            vdm = new VehicleDBMgr();
            vdm.InitializeDB();
            if (!Page.IsPostBack)
            {
                if (!Page.IsCallback)
                {
                   // FillplantName();
                    ddwnldr = new DataDownloader();
                    vdm.InitializeDB();
                   // ImageButton1.ImageUrl = "UserImgs/build10.png";
                    UpdateMyLocationUI();
                    FillCategoryName();
                }
            }
        }

    }


    public void FillCategoryName()
    {
      //  ddl_category.Items.Clear();
        MySqlCommand cmd = new MySqlCommand("SELECT        * FROM            location_groups WHERE        (user_sno = @userid)");
        cmd.Parameters.Add("@userid", User_sno);
        DataTable dtPlant = vdm.SelectQuery(cmd).Tables[0];
        //DataRow newrow = dtPlant.NewRow();
        //newrow["sno"] = 0;
        //newrow["category_name"] = "Select Category";
        //dtPlant.Rows.InsertAt(newrow, 0);
        //ddl_category.DataSource = dtPlant;
        //ddl_category.DataTextField = "category_name";
        //ddl_category.DataValueField = "sno";
        //ddl_category.DataBind();
        list_groups.DataSource = dtPlant;
        list_groups.DataBind();
    }

   
    string BranchID = "";
 
    void GoogleMapForASPNet1_Mylocation_PushpinMoved(string pID)
    {
        ////GooglePoint gp = GoogleMapForASPNet1.GoogleMapObject.Points[pID];
        ////txt_Latitude.Text = gp.Latitude.ToString();
        ////txt_Longitude.Text = gp.Longitude.ToString();
    }
    private void UpdateMyLocationUI()
    {
        try
        {
            //MySqlCommand cmd = new MySqlCommand("SELECT branchdata.BranchID, branchdata.Description, branchdata.Latitude, branchdata.Longitude, branchdata.PhoneNumber, branchdata.ImagePath, branchdata.Radious, branchdata.Sno, branchdata.PlantName AS plantSno, branchdata_1.BranchID AS PlantName,branchdata.IsPlant  FROM branchdata LEFT OUTER JOIN branchdata branchdata_1 ON branchdata.UserName = branchdata_1.UserName AND branchdata.PlantName = branchdata_1.Sno WHERE (branchdata.UserName = @UserName)");
          //  MySqlCommand cmd = new MySqlCommand("SELECT        branchdata.BranchID, branchdata.Description, location_category.category_name, branchdata.Latitude, branchdata.Longitude, branchdata.PhoneNumber, branchdata.ImagePath, branchdata.Radious, branchdata.IsPlant, location_category_mapping.location_category_sno, branchdata.sno as branch_sno, branchdata.PlantName AS plantSno FROM            location_category INNER JOIN location_category_mapping ON location_category.sno = location_category_mapping.location_category_sno RIGHT OUTER JOIN branchdata ON location_category_mapping.branch_sno = branchdata.Sno WHERE        (branchdata.UserName = @UserName)");
            MySqlCommand cmd = new MySqlCommand("SELECT UserName, BranchID, Description, Latitude, Longitude, PhoneNumber, ImagePath, Radious,Sno, PlantName, IsPlant FROM branchdata WHERE (UserName = @UserName)");
            cmd.Parameters.Add("@UserName", UserName);
            DataTable dt = vdm.SelectQuery(cmd).Tables[0];
            grdMylocation.DataSource = dt;
            grdMylocation.DataBind();
            Session["xportdata"] = dt;
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, this);
        }
    }

    protected void BtnMyLocatoinRefresh_Click(object sender, EventArgs e)
    {
        btn_save.Text = "Save";
        clearChecks();
        txt_groupname.Text = "";
    }
   public class checkedvalues
    {
       string branch_id { set; get; }
       string previous_mapping_sno { set; get; }
    }
   protected void BtnCategory_Save_Click(object sender, EventArgs e)
   {
       if (btn_save.Text == "Save")
       {
           // StringBuilder object
           StringBuilder str = new StringBuilder();
           Dictionary<string, string> selectedvalues = new Dictionary<string, string>();
           cmd = new MySqlCommand("insert into location_groups (location_group_name,user_sno) values (@location_group_name,@user_sno)");
           cmd.Parameters.Add("@location_group_name", txt_groupname.Text);
           cmd.Parameters.Add("@user_sno", User_sno);
           long sno = vdm.insertScalar(cmd);
           // Select the checkboxes from the GridView control
           for (int i = 0; i < grdMylocation.Rows.Count; i++)
           {
               GridViewRow row = grdMylocation.Rows[i];
               bool isChecked = ((CheckBox)row.FindControl("chkSelect")).Checked;

               if (isChecked)
               {
                   cmd = new MySqlCommand("insert into location_groups_mapping (location_group_sno,branch_sno) values (@location_group_sno,@branch_sno) ");
                   cmd.Parameters.Add("@location_group_sno", sno);
                   cmd.Parameters.Add("@branch_sno", grdMylocation.Rows[i].Cells[9].Text);
                   vdm.insert(cmd);
                   // Column 2 is the name column]
                   //selectedvalues.Add(grdMylocation.Rows[i].Cells[11].Text, grdMylocation.Rows[i].Cells[10].Text);

                   // str.Append(grdMylocation.Rows[i].Cells[2].Text);
               }
           }
       }
       else
       {
           GridViewRow gvr = list_groups.SelectedRow;
           string group_id = gvr.Cells[1].Text;
           if (group_id != "")
           {
               cmd = new MySqlCommand("update location_groups set location_group_name=@location_group_name,user_sno=@user_sno where sno=@sno");
               cmd.Parameters.Add("@location_group_name", txt_groupname.Text);
               cmd.Parameters.Add("@user_sno", User_sno);
               cmd.Parameters.Add("@sno", group_id);
                vdm.Update(cmd);

               cmd = new MySqlCommand("delete from location_groups_mapping where location_group_sno=@lgs");
               cmd.Parameters.Add("@lgs", group_id);
               vdm.Delete(cmd);
               for (int i = 0; i < grdMylocation.Rows.Count; i++)
               {
                   GridViewRow row = grdMylocation.Rows[i];
                   bool isChecked = ((CheckBox)row.FindControl("chkSelect")).Checked;

                   if (isChecked)
                   {
                       cmd = new MySqlCommand("insert into location_groups_mapping (location_group_sno,branch_sno) values (@location_group_sno,@branch_sno) ");
                       cmd.Parameters.Add("@location_group_sno", group_id);
                       cmd.Parameters.Add("@branch_sno", grdMylocation.Rows[i].Cells[9].Text);
                       vdm.insert(cmd);
                       // Column 2 is the name column]
                       //selectedvalues.Add(grdMylocation.Rows[i].Cells[11].Text, grdMylocation.Rows[i].Cells[10].Text);

                       // str.Append(grdMylocation.Rows[i].Cells[2].Text);
                   }
               }

           }
       }
       FillCategoryName();

       BtnMyLocatoinRefresh_Click(null, null);
       // prints out the result
       // Response.Write(str.ToString());
   }
    protected void Btndelete_all_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < grdMylocation.Rows.Count; i++)
        {
            GridViewRow row = grdMylocation.Rows[i];
            bool isChecked = ((CheckBox)row.FindControl("chkSelect")).Checked;

            if (isChecked)
            {
                try
                {
                    cmd = new MySqlCommand("delete from location_groups_mapping where branch_sno=@branch_sno");

                    cmd.Parameters.Add("@branch_sno", grdMylocation.Rows[i].Cells[9].Text);
                    vdm.Delete(cmd);

                    cmd = new MySqlCommand("delete from branchdata where Sno=@Sno");

                    cmd.Parameters.Add("@Sno", grdMylocation.Rows[i].Cells[9].Text);
                    vdm.Delete(cmd);
                    // Column 2 is the name column]
                    //selectedvalues.Add(grdMylocation.Rows[i].Cells[11].Text, grdMylocation.Rows[i].Cells[10].Text);

                    // str.Append(grdMylocation.Rows[i].Cells[2].Text);
                }
                catch (Exception ex)
                {
                }
            }
        }
        UpdateMyLocationUI();
    }
    protected void Btn_exportXL_Click(object sender, EventArgs e)
    {
       
    }

   
    //protected void list_Groups_rowcreated(object sender, GridViewRowEventArgs e)
    //{
    //    e.Row.Cells[0].Visible = false; 
    //}

    void clearChecks()
    {
        foreach (GridViewRow gridviewrec in grdMylocation.Rows)
        {

            CheckBox chk = (CheckBox)gridviewrec.FindControl("chkSelect");
            chk.Checked = false;
            //  CheckBox chk = ((CheckBox)gridviewrec.FindControl("chkSelect")).Checked = true;
            // grdMylocation.UpdateRow(gridviewrec.RowIndex, false);

        }
    }
    protected void GridView3_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (list_groups.SelectedIndex > -1)
        {
           // clearChecks();
            try
            {
                GridViewRow gvr = list_groups.SelectedRow;
            string group_id = gvr.Cells[1].Text;

                txt_groupname.Text = gvr.Cells[2].Text;

                cmd = new MySqlCommand("select * from location_groups_mapping where location_group_sno=@lgs");
                cmd.Parameters.Add("@lgs", group_id);
                DataTable locinfo = vdm.SelectQuery(cmd).Tables[0];
                foreach (GridViewRow gridviewrec in grdMylocation.Rows)
                    {
                        CheckBox chk = (CheckBox)gridviewrec.FindControl("chkSelect");
                        chk.Checked = false;
                        foreach (DataRow dr in locinfo.Rows)
                        {

                            if (gridviewrec.Cells[9].Text == dr["branch_sno"].ToString())
                            {
                                
                                chk.Checked = true;
                                //  CheckBox chk = ((CheckBox)gridviewrec.FindControl("chkSelect")).Checked = true;
                                // grdMylocation.UpdateRow(gridviewrec.RowIndex, false);
                            }
                        }
                }
                btn_save.Text = "Modify";
                //btn_Mylocation_Refresh.Text = "Edit";
                //txt_LocationName.Text = BranchID;
                //txt_Description.Text = gvr.Cells[2].Text;
                //txt_Latitude.Text = gvr.Cells[3].Text;
                //txt_Longitude.Text = gvr.Cells[4].Text;
                //txt_PhoneNumber.Text = gvr.Cells[5].Text;
                //txt_Mylocation_Radious.Text = gvr.Cells[7].Text;
                //txt_Image.Text = gvr.Cells[6].Text;
                //btn_MyLocation_Del.Enabled = true;
                //MyLocationNameSno = Convert.ToDecimal(gvr.Cells[8].Text);
                //ImageButton1.ImageUrl = ResolveUrl("~/UserImgs/" + txt_Image.Text + ".png");
                //ListItem item = ddlPlantName.Items.FindByText(gvr.Cells[9].Text);
                //if (item != null)
                //{
                //    ddlPlantName.SelectedValue = gvr.Cells[9].Text;
                //}
                //ddlPlantName.SelectedValue = gvr.Cells[9].Text;
                //bool isplant = false;
                //foreach (Control ctrl in gvr.Cells[11].Controls)
                //{
                //    if (ctrl.GetType().Name == "CheckBox")
                //    {
                //        CheckBox chk = (CheckBox)ctrl;
                //        isplant = chk.Checked;
                //    }
                //}
                //if (isplant)
                //    ckb_isplant.Checked = true;
                //else
                //    ckb_isplant.Checked = false;
                //ScriptManager.RegisterStartupScript(Page, GetType(), "JsStatus", "PointsAdd();", true);
                //lblMylocmsg.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this);
            }
        }
    }
   
    protected void grdMylocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (grdMylocation.SelectedIndex > -1)
        {

            try
            {
                GridViewRow gvr = grdMylocation.SelectedRow;
                BranchID = gvr.Cells[1].Text;
                //btn_Mylocation_Refresh.Text = "Edit";
                //txt_LocationName.Text = BranchID;
                //txt_Description.Text = gvr.Cells[2].Text;
                //txt_Latitude.Text = gvr.Cells[3].Text;
                //txt_Longitude.Text = gvr.Cells[4].Text;
                //txt_PhoneNumber.Text = gvr.Cells[5].Text;
                //txt_Mylocation_Radious.Text = gvr.Cells[7].Text;
                //txt_Image.Text = gvr.Cells[6].Text;
                //btn_MyLocation_Del.Enabled = true;
                //MyLocationNameSno = Convert.ToDecimal(gvr.Cells[8].Text);
                //ImageButton1.ImageUrl = ResolveUrl("~/UserImgs/" + txt_Image.Text + ".png");
                //ListItem item = ddlPlantName.Items.FindByText(gvr.Cells[9].Text);
                //if (item != null)
                //{
                //    ddlPlantName.SelectedValue = gvr.Cells[9].Text;
                //}
                //ddlPlantName.SelectedValue = gvr.Cells[9].Text;
                //bool isplant = false;
                //foreach (Control ctrl in gvr.Cells[11].Controls)
                //{
                //    if (ctrl.GetType().Name == "CheckBox")
                //    {
                //        CheckBox chk = (CheckBox)ctrl;
                //        isplant = chk.Checked;
                //    }
                //}
                //if (isplant)
                //    ckb_isplant.Checked = true;
                //else
                //    ckb_isplant.Checked = false;
                //ScriptManager.RegisterStartupScript(Page, GetType(), "JsStatus", "PointsAdd();", true);
                //lblMylocmsg.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this);
            }
        }
    }
    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {
        //GridDataBind();
        //Panel3.Visible = true;
    }

    protected void list_Groups_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
       
        
            e.Row.Cells[1].Visible = false;
            e.Row.Cells[3].Visible = false;
        
        //e.Row.Cells[9].Visible = false;
    }

}