using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class LogOut : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Session.Abandon();
        Session["Dealer"] = null;
        FormsAuthentication.SignOut();
        Session["field1"] = null;
        Session["UserType"] = null;
        Session["Data"] = null;
        Session["reportdata"] = null;
        Session["title"] = null;
        Session["field2"] = null;
        Session["vendorstable"] = null;
        Session["odometerValues"] = null;
        Session["xportdata"] = null;
        Session["filteredtable"] = null;
        Session["vendorstable"] = null;
        Session["vehiclesdata"] = null;
        Session["allvehicles"] = null;
        Session["SubTriplist"] = null;
        Session["preselectedplant"] = null;
        Session["routes"] = null;
        Response.Redirect("login.aspx");
    }
}